﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QueryMultiDb.Common;

namespace QueryMultiDbGui
{
    public partial class MainForm : Form
    {
        private static readonly string QueryMultiDbExecutableFilename = ConfigurationManager.AppSettings["QueryMultiDbExecutableFilename"];
        private static readonly string QueryMultiDbDocumentsRootDirectoryName = ConfigurationManager.AppSettings["QueryMultiDbDocumentsRootDirectoryName"];
        private static readonly string DocumentsTargetsDirectoryName = ConfigurationManager.AppSettings["DocumentsTargetsDirectoryName"];
        private static readonly string DocumentsExtractionsDirectoryName = ConfigurationManager.AppSettings["DocumentsExtractionsDirectoryName"];

        private Process _runningProcess;
        private StringBuilder _consoleBuffer;

        // Sample line : "DataReader.GetQueryResults : 0% (0/1)"
        private static readonly Regex ProgressLineRegex = new Regex("^([^ ]+) : (\\d+)%.*$", RegexOptions.Compiled);

        private static string TargetsFullPath
        {
            get
            {
                var myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var targetsFullPath = Path.Combine(myDocumentsPath, QueryMultiDbDocumentsRootDirectoryName, DocumentsTargetsDirectoryName);

                return targetsFullPath;
            }
        }

        private static string ExtractionFullPath
        {
            get
            {
                var myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var extractionFullPath = Path.Combine(myDocumentsPath, QueryMultiDbDocumentsRootDirectoryName, DocumentsExtractionsDirectoryName);

                return extractionFullPath;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            SetTargetsValueLabels(string.Empty, string.Empty);

            cancelButton.Enabled = false;
            commandLineCopyButton.Click += CommandLineCopyButton_Click;
            CreateToolDirectoriesIfNotExists();

            var targetsDirectoryWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(TargetsFullPath),
                Filter = "*",
                NotifyFilter = AllNotifyFilters,
                EnableRaisingEvents = true
            };
            targetsDirectoryWatcher.Created += targetsDirectoryWatcher_Changed;
            targetsDirectoryWatcher.Deleted += targetsDirectoryWatcher_Changed;
            targetsDirectoryWatcher.Changed += targetsDirectoryWatcher_Changed;
            targetsDirectoryWatcher.Renamed += targetsDirectoryWatcher_Changed;
            
            openTargetsFolderLinkLabel.LinkClicked += OpenTargetsFolderLinkLabel_LinkClicked;

            SetTargetsComboBox();
        }

        private static NotifyFilters AllNotifyFilters => NotifyFilters.FileName |
                                                         NotifyFilters.DirectoryName |
                                                         NotifyFilters.Attributes |
                                                         NotifyFilters.Size |
                                                         NotifyFilters.LastWrite |
                                                         NotifyFilters.LastAccess |
                                                         NotifyFilters.CreationTime |
                                                         NotifyFilters.Security;

        private void CommandLineCopyButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(commandLineTextBox.Text))
            {
                Clipboard.SetText(commandLineTextBox.Text);
            }
        }

        private void SetTargetsComboBox()
        {
            var oldSelectedValue = targetsComboBox.SelectedValue as string;

            targetsComboBox.SelectedIndexChanged -= TargetsComboBox_SelectedIndexChanged;
            targetsComboBox.SelectedIndex = -1;
            targetsComboBox.DataSource = null;

            var files = Directory.GetFiles(TargetsFullPath);
            var targetEntries = files.Select(f => new TargetFileEntry(f)).ToList();
            targetsComboBox.SelectedIndexChanged += TargetsComboBox_SelectedIndexChanged;
            targetsComboBox.DataSource = targetEntries;

            if (oldSelectedValue != null)
            {
                targetsComboBox.SelectedValue = oldSelectedValue;
            }

            targetsComboBox.DisplayMember = "Filename";
            targetsComboBox.ValueMember = "Filepath";
        }

        private void targetsDirectoryWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.InvokeEx(SetTargetsComboBox);
        }

        private void TargetsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (targetsComboBox.SelectedItem is TargetFileEntry targetEntry)
            {
                SetTargetsValueLabels(
                    targetEntry.Length.ToSuffixedSizeString(),
                    targetEntry.LastWriteTime.ToString("s"));
            }
            else
            {
                SetTargetsValueLabels(string.Empty, string.Empty);
            }
        }

        private void SetTargetsValueLabels(string fileSize, string modificationDate)
        {
            targetsFileSizeValueLabel.Text = fileSize;
            targetsFileModificationDateValueLabel.Text = modificationDate;
        }

        private void OpenTargetsFolderLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(TargetsFullPath);
        }

        private static void CreateToolDirectoriesIfNotExists()
        {
            Directory.CreateDirectory(TargetsFullPath);
            Directory.CreateDirectory(ExtractionFullPath);
        }

        private void browseSqlFileButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "SQL Script Files|*.sql",
                Title = "Select a SQL Script File",
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DereferenceLinks = true,
                Multiselect = false,
                SupportMultiDottedExtensions = true
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            sqlFileDisplayControl.AbsoluteFilePathChanged += SqlFileDisplayControl_AbsoluteFilePathChanged;
            sqlFileDisplayControl.AbsoluteFilePath = openFileDialog.FileName;
        }

        private void SqlFileDisplayControl_AbsoluteFilePathChanged(object sender, AbsoluteFilePathChangedEventArgs e)
        {
            SetSqlScriptPreview(e.AbsoluteFilePath);
        }

        private void SetSqlScriptPreview(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                var text = sr.ReadToEnd();
                this.InvokeEx(() => sqlScriptPreviewTextBox.Text = text);
                sr.Close();
            }
        }

        private void browseOutputFileButton_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                DefaultExt = "xlsx",
                DereferenceLinks = true,
                Filter = "Excel files|*.xlsx",
                Title = "Select an excel file",
                SupportMultiDottedExtensions = true
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            outputFileDisplayControl.AbsoluteFilePath = saveFileDialog.FileName;
        }

        private static string ToHex(Color c)
        {
            return $"{c.R:X2}{c.G:X2}{c.B:X2}";
        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            if (sqlFileDisplayControl.AbsoluteFilePath == null
                || targetsComboBox.SelectedItem == null
                || outputFileDisplayControl.AbsoluteFilePath == null)
            {
                MessageBox.Show("Select a SQL script, a target, and an output file to execute.",
                    "Cannot execute.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);

                return;
            }

            var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder
            {
                Overwrite = overwriteOutputFileCheckBox.Checked,
                Progress = true,
                QueryFile = sqlFileDisplayControl.AbsoluteFilePath,
                TargetsFile = ((TargetFileEntry)targetsComboBox.SelectedItem).Filepath,
                OutputFile = outputFileDisplayControl.AbsoluteFilePath,
                ShowNulls = showNullCheckBox.Checked,
                NullsColor = ToHex(nullColorPictureBox.BackColor),
                DiscardResults = discardResultsCheckBox.Checked,
                CommandTimeout = (int) commandTimeoutNumericUpDown.Value,
                Parallelism = (int) parallelismNumericUpDown.Value,
                SheetLabels = sheetlabelTextBox.Text,
                ApplicationName = "GUI",
                ShowIpAddress = !hideLotCheckBox.Checked,
                ShowServerName = !hideLotCheckBox.Checked,
                ShowDatabaseName = !hideLotCheckBox.Checked,
                ShowExtraColumns = !hideLotCheckBox.Checked,
                ShowLogSheet = !hideLotCheckBox.Checked,
                ShowParameterSheet = !hideLotCheckBox.Checked,
                ShowInformationMessages = !hideLotCheckBox.Checked
            };

            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                FileName = QueryMultiDbExecutableFilename,
                Arguments = argumentStringBuilder.ToString()
            };

            try
            {
                var process = new Process {StartInfo = startInfo};
                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived;
                process.Exited += Process_Exited;
                process.EnableRaisingEvents = true;

                this.InvokeEx(() => consoleOutputTextBox.Text = string.Empty);
                this.InvokeEx(() => commandLineTextBox.Text = QueryMultiDbExecutableFilename + argumentStringBuilder);

                _consoleBuffer = new StringBuilder(1 * 1024 * 1024);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                executeButton.Enabled = false;
                cancelButton.Enabled = true;
                _runningProcess = process;
            }
            catch (Win32Exception exp)
            {
                MessageBox.Show($"Unable to start the process with the specified file '{startInfo.FileName}'. There might be a path issue. {exp.Message}",
                    "Cannot start QueryMultiDb process.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            _runningProcess.WaitForExit();
            _runningProcess.Dispose();
            _runningProcess = null;
            this.InvokeEx(() =>
            {
                executeButton.Enabled = true;
                cancelButton.Enabled = false;
            });
            this.InvokeEx(UpdateConsoleOutputTextBox);
            _consoleBuffer = null;
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            var errorData = e.Data;

            if (errorData == null)
            {
                return;
            }

            _consoleBuffer.AppendLine(errorData);

            this.InvokeEx(UpdateConsoleOutputTextBox);

            var matches = ProgressLineRegex.Matches(errorData);

            if (matches.Count <= 0)
            {
                return;
            }

            var groups = matches[0].Groups;
            var status = groups[1].Value;
            var progress = int.Parse(groups[2].Value);

            this.InvokeEx(() => progressStateValueLabel.Text = status);
            this.InvokeEx(() => progressBar.Value = progress);
        }

        private void UpdateConsoleOutputTextBox()
        {
            consoleOutputTextBox.Text = _consoleBuffer.ToString();
            consoleOutputTextBox.SelectionStart = consoleOutputTextBox.Text.Length;
            consoleOutputTextBox.ScrollToCaret();
            consoleOutputTextBox.Refresh();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var outputData = e.Data;

            if (outputData == null)
            {
                return;
            }

            _consoleBuffer.AppendLine(outputData);
        }

        private void nullColorPictureBox_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog {FullOpen = false};

            if (colorDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            nullColorPictureBox.BackColor = colorDialog.Color;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (_runningProcess == null)
            {
                return;
            }

            try
            {
                _runningProcess.Kill();
            }
            catch
            {
                // TODO : Add a log.
            }

            try
            {
                _runningProcess.Dispose();
            }
            catch
            {
                // TODO : Add a log.
            }

            _runningProcess = null;
            this.InvokeEx(() =>
            {
                executeButton.Enabled = true;
                cancelButton.Enabled = false;
            });
        }
    }
}