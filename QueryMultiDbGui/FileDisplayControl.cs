using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using QueryMultiDb.Common;

namespace QueryMultiDbGui
{
    public partial class FileDisplayControl : UserControl
    {
        private string _absoluteFilePath;
        private readonly FileSystemWatcher _watcher;
        private readonly ToolTip _absolutePathToolTip;
        public event EventHandler<AbsoluteFilePathChangedEventArgs> AbsoluteFilePathChanged;

        private static NotifyFilters AllNotifyFilters => NotifyFilters.FileName |
                                                         NotifyFilters.DirectoryName |
                                                         NotifyFilters.Attributes |
                                                         NotifyFilters.Size |
                                                         NotifyFilters.LastWrite |
                                                         NotifyFilters.LastAccess |
                                                         NotifyFilters.CreationTime |
                                                         NotifyFilters.Security;

        public string AbsoluteFilePath
        {
            get => _absoluteFilePath;

            set
            {
                _absoluteFilePath = value;
                OnAbsoluteFilePathChanged();
            }
        }
        
        public FileDisplayControl()
        {
            InitializeComponent();

            _absolutePathToolTip = new ToolTip();
            _watcher = new FileSystemWatcher { NotifyFilter = AllNotifyFilters };
            _watcher.Changed += Watcher_Changed;
            filePathValueLinkLabel.LinkClicked += FilePathValueLinkLabel_LinkClicked;
            AbsoluteFilePath = string.Empty;
        }

        private void OnAbsoluteFilePathChanged()
        {
            var fileName = _absoluteFilePath;

            if (string.IsNullOrEmpty(fileName))
            {
                this.InvokeEx(() => filePathValueLinkLabel.Text = string.Empty);
                this.InvokeEx(() => fileModificationDateValueLabel.Text = string.Empty);
                this.InvokeEx(() => fileSizeValueLabel.Text = string.Empty);
                this.InvokeEx(() => _absolutePathToolTip.SetToolTip(filePathValueLinkLabel, string.Empty));
                AbsoluteFilePathChanged?.Invoke(this, new AbsoluteFilePathChangedEventArgs(string.Empty));

                return;
            }

            _watcher.Path = Path.GetDirectoryName(fileName);
            _watcher.Filter = Path.GetFileName(fileName);
            _watcher.EnableRaisingEvents = true;

            var fileInfo = new FileInfo(fileName);
            this.InvokeEx(() => filePathValueLinkLabel.Text = fileInfo.Name);
            this.InvokeEx(() => filePathValueLinkLabel.Links[0].LinkData = fileInfo.FullName);
            this.InvokeEx(() => _absolutePathToolTip.SetToolTip(filePathValueLinkLabel, fileInfo.FullName));
            AbsoluteFilePathChanged?.Invoke(this, new AbsoluteFilePathChangedEventArgs(fileInfo.FullName));

            if (fileInfo.Exists)
            {
                this.InvokeEx(() => filePathValueLinkLabel.Enabled = true);
                this.InvokeEx(() => fileSizeValueLabel.Text = fileInfo.Length.ToSuffixedSizeString());
                this.InvokeEx(() => fileModificationDateValueLabel.Text = fileInfo.LastWriteTime.ToString("s"));
            }
            else
            {
                this.InvokeEx(() => filePathValueLinkLabel.Enabled = false);
                this.InvokeEx(() => fileSizeValueLabel.Text = "N/A");
                this.InvokeEx(() => fileModificationDateValueLabel.Text = "N/A");
            }
        }

        private static void FilePathValueLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!(e.Link.LinkData is string fileFullName))
            {
                throw new ArgumentNullException(nameof(fileFullName), "A file path must be passed to the action.");
            }

            if (string.IsNullOrWhiteSpace(fileFullName))
            {
                throw new ArgumentException("File path can't be null or empty or whitespaces.", nameof(fileFullName));
            }

            Process.Start(fileFullName);
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            AbsoluteFilePath = e.FullPath;
        }
    }
}
