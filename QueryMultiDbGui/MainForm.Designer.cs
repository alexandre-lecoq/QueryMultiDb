using System.Net;

namespace QueryMultiDbGui
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.executeButton = new System.Windows.Forms.Button();
            this.outputGroupBox = new System.Windows.Forms.GroupBox();
            this.commandLineCopyButton = new System.Windows.Forms.Button();
            this.commandLineTextBox = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressStateValueLabel = new System.Windows.Forms.Label();
            this.consoleOutputTextBox = new System.Windows.Forms.TextBox();
            this.optionGroupBox = new System.Windows.Forms.GroupBox();
            this.sheetlabelLegendLabel = new System.Windows.Forms.Label();
            this.parallelismNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.parallelismLabel = new System.Windows.Forms.Label();
            this.hideLotCheckBox = new System.Windows.Forms.CheckBox();
            this.discardResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.sheetlabelsLabel = new System.Windows.Forms.Label();
            this.sheetlabelTextBox = new System.Windows.Forms.TextBox();
            this.nullColorPictureBox = new System.Windows.Forms.PictureBox();
            this.showNullCheckBox = new System.Windows.Forms.CheckBox();
            this.nullColorLabel = new System.Windows.Forms.Label();
            this.commandTimeoutLabel = new System.Windows.Forms.Label();
            this.commandTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.targetDatabaseGroupBox = new System.Windows.Forms.GroupBox();
            this.targetsFileSizeValueLabel = new System.Windows.Forms.Label();
            this.targetsFileModificationDateValueLabel = new System.Windows.Forms.Label();
            this.targetsFileSizeLabel = new System.Windows.Forms.Label();
            this.targetsFileModificationDateLabel = new System.Windows.Forms.Label();
            this.openTargetsFolderLinkLabel = new System.Windows.Forms.LinkLabel();
            this.targetsComboBox = new System.Windows.Forms.ComboBox();
            this.sqlScriptGroupBox = new System.Windows.Forms.GroupBox();
            this.sqlScriptPreviewTextBox = new System.Windows.Forms.TextBox();
            this.outputFileGroupBox = new System.Windows.Forms.GroupBox();
            this.outputFileDisplayControl = new QueryMultiDbGui.FileDisplayControl();
            this.browseOutputFileButton = new System.Windows.Forms.Button();
            this.overwriteOutputFileCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.sqlFileGroupBox = new System.Windows.Forms.GroupBox();
            this.browseSqlFileButton = new System.Windows.Forms.Button();
            this.sqlFileDisplayControl = new QueryMultiDbGui.FileDisplayControl();
            this.outputGroupBox.SuspendLayout();
            this.optionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parallelismNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nullColorPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandTimeoutNumericUpDown)).BeginInit();
            this.targetDatabaseGroupBox.SuspendLayout();
            this.sqlScriptGroupBox.SuspendLayout();
            this.outputFileGroupBox.SuspendLayout();
            this.sqlFileGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // executeButton
            // 
            this.executeButton.Location = new System.Drawing.Point(897, 12);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(75, 36);
            this.executeButton.TabIndex = 0;
            this.executeButton.Text = "Execute";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // outputGroupBox
            // 
            this.outputGroupBox.Controls.Add(this.commandLineCopyButton);
            this.outputGroupBox.Controls.Add(this.commandLineTextBox);
            this.outputGroupBox.Controls.Add(this.progressBar);
            this.outputGroupBox.Controls.Add(this.progressStateValueLabel);
            this.outputGroupBox.Controls.Add(this.consoleOutputTextBox);
            this.outputGroupBox.Location = new System.Drawing.Point(12, 534);
            this.outputGroupBox.Name = "outputGroupBox";
            this.outputGroupBox.Size = new System.Drawing.Size(960, 415);
            this.outputGroupBox.TabIndex = 1;
            this.outputGroupBox.TabStop = false;
            this.outputGroupBox.Text = "Output";
            // 
            // commandLineCopyButton
            // 
            this.commandLineCopyButton.Location = new System.Drawing.Point(902, 19);
            this.commandLineCopyButton.Name = "commandLineCopyButton";
            this.commandLineCopyButton.Size = new System.Drawing.Size(52, 22);
            this.commandLineCopyButton.TabIndex = 4;
            this.commandLineCopyButton.Text = "Copy";
            this.commandLineCopyButton.UseVisualStyleBackColor = true;
            // 
            // commandLineTextBox
            // 
            this.commandLineTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commandLineTextBox.Location = new System.Drawing.Point(6, 19);
            this.commandLineTextBox.Name = "commandLineTextBox";
            this.commandLineTextBox.ReadOnly = true;
            this.commandLineTextBox.Size = new System.Drawing.Size(890, 22);
            this.commandLineTextBox.TabIndex = 3;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(152, 45);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(802, 23);
            this.progressBar.TabIndex = 2;
            // 
            // progressStateValueLabel
            // 
            this.progressStateValueLabel.AutoSize = true;
            this.progressStateValueLabel.Location = new System.Drawing.Point(6, 50);
            this.progressStateValueLabel.Name = "progressStateValueLabel";
            this.progressStateValueLabel.Size = new System.Drawing.Size(52, 13);
            this.progressStateValueLabel.TabIndex = 1;
            this.progressStateValueLabel.Text = "Waiting...";
            // 
            // consoleOutputTextBox
            // 
            this.consoleOutputTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(36)))), ((int)(((byte)(86)))));
            this.consoleOutputTextBox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consoleOutputTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(237)))), ((int)(((byte)(240)))));
            this.consoleOutputTextBox.Location = new System.Drawing.Point(6, 74);
            this.consoleOutputTextBox.Multiline = true;
            this.consoleOutputTextBox.Name = "consoleOutputTextBox";
            this.consoleOutputTextBox.ReadOnly = true;
            this.consoleOutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.consoleOutputTextBox.Size = new System.Drawing.Size(948, 335);
            this.consoleOutputTextBox.TabIndex = 0;
            // 
            // optionGroupBox
            // 
            this.optionGroupBox.Controls.Add(this.sheetlabelLegendLabel);
            this.optionGroupBox.Controls.Add(this.parallelismNumericUpDown);
            this.optionGroupBox.Controls.Add(this.parallelismLabel);
            this.optionGroupBox.Controls.Add(this.hideLotCheckBox);
            this.optionGroupBox.Controls.Add(this.discardResultsCheckBox);
            this.optionGroupBox.Controls.Add(this.sheetlabelsLabel);
            this.optionGroupBox.Controls.Add(this.sheetlabelTextBox);
            this.optionGroupBox.Controls.Add(this.nullColorPictureBox);
            this.optionGroupBox.Controls.Add(this.showNullCheckBox);
            this.optionGroupBox.Controls.Add(this.nullColorLabel);
            this.optionGroupBox.Controls.Add(this.commandTimeoutLabel);
            this.optionGroupBox.Controls.Add(this.commandTimeoutNumericUpDown);
            this.optionGroupBox.Location = new System.Drawing.Point(12, 450);
            this.optionGroupBox.Name = "optionGroupBox";
            this.optionGroupBox.Size = new System.Drawing.Size(960, 78);
            this.optionGroupBox.TabIndex = 2;
            this.optionGroupBox.TabStop = false;
            this.optionGroupBox.Text = "Options";
            // 
            // sheetlabelLegendLabel
            // 
            this.sheetlabelLegendLabel.AutoSize = true;
            this.sheetlabelLegendLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sheetlabelLegendLabel.Location = new System.Drawing.Point(84, 26);
            this.sheetlabelLegendLabel.Name = "sheetlabelLegendLabel";
            this.sheetlabelLegendLabel.Size = new System.Drawing.Size(112, 13);
            this.sheetlabelLegendLabel.TabIndex = 15;
            this.sheetlabelLegendLabel.Text = "(comma-separated list)";
            // 
            // parallelismNumericUpDown
            // 
            this.parallelismNumericUpDown.Location = new System.Drawing.Point(902, 45);
            this.parallelismNumericUpDown.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.parallelismNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.parallelismNumericUpDown.Name = "parallelismNumericUpDown";
            this.parallelismNumericUpDown.Size = new System.Drawing.Size(52, 20);
            this.parallelismNumericUpDown.TabIndex = 14;
            this.parallelismNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // parallelismLabel
            // 
            this.parallelismLabel.AutoSize = true;
            this.parallelismLabel.Location = new System.Drawing.Point(834, 48);
            this.parallelismLabel.Name = "parallelismLabel";
            this.parallelismLabel.Size = new System.Drawing.Size(62, 13);
            this.parallelismLabel.TabIndex = 13;
            this.parallelismLabel.Text = "Parallelism :";
            // 
            // hideLotCheckBox
            // 
            this.hideLotCheckBox.AutoSize = true;
            this.hideLotCheckBox.Location = new System.Drawing.Point(561, 20);
            this.hideLotCheckBox.Name = "hideLotCheckBox";
            this.hideLotCheckBox.Size = new System.Drawing.Size(71, 17);
            this.hideLotCheckBox.TabIndex = 11;
            this.hideLotCheckBox.Text = "Hide a lot";
            this.hideLotCheckBox.UseVisualStyleBackColor = true;
            // 
            // discardResultsCheckBox
            // 
            this.discardResultsCheckBox.AutoSize = true;
            this.discardResultsCheckBox.Location = new System.Drawing.Point(561, 48);
            this.discardResultsCheckBox.Name = "discardResultsCheckBox";
            this.discardResultsCheckBox.Size = new System.Drawing.Size(95, 17);
            this.discardResultsCheckBox.TabIndex = 10;
            this.discardResultsCheckBox.Text = "Discard results";
            this.discardResultsCheckBox.UseVisualStyleBackColor = true;
            // 
            // sheetlabelsLabel
            // 
            this.sheetlabelsLabel.AutoSize = true;
            this.sheetlabelsLabel.Location = new System.Drawing.Point(6, 26);
            this.sheetlabelsLabel.Name = "sheetlabelsLabel";
            this.sheetlabelsLabel.Size = new System.Drawing.Size(71, 13);
            this.sheetlabelsLabel.TabIndex = 9;
            this.sheetlabelsLabel.Text = "Sheet labels :";
            // 
            // sheetlabelTextBox
            // 
            this.sheetlabelTextBox.Location = new System.Drawing.Point(9, 46);
            this.sheetlabelTextBox.Name = "sheetlabelTextBox";
            this.sheetlabelTextBox.Size = new System.Drawing.Size(535, 20);
            this.sheetlabelTextBox.TabIndex = 8;
            // 
            // nullColorPictureBox
            // 
            this.nullColorPictureBox.BackColor = System.Drawing.Color.Fuchsia;
            this.nullColorPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.nullColorPictureBox.Location = new System.Drawing.Point(752, 48);
            this.nullColorPictureBox.Name = "nullColorPictureBox";
            this.nullColorPictureBox.Size = new System.Drawing.Size(13, 13);
            this.nullColorPictureBox.TabIndex = 7;
            this.nullColorPictureBox.TabStop = false;
            this.nullColorPictureBox.Click += new System.EventHandler(this.nullColorPictureBox_Click);
            // 
            // showNullCheckBox
            // 
            this.showNullCheckBox.AutoSize = true;
            this.showNullCheckBox.Checked = true;
            this.showNullCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showNullCheckBox.Location = new System.Drawing.Point(682, 20);
            this.showNullCheckBox.Name = "showNullCheckBox";
            this.showNullCheckBox.Size = new System.Drawing.Size(84, 17);
            this.showNullCheckBox.TabIndex = 6;
            this.showNullCheckBox.Text = "Show NULL";
            this.showNullCheckBox.UseVisualStyleBackColor = true;
            // 
            // nullColorLabel
            // 
            this.nullColorLabel.AutoSize = true;
            this.nullColorLabel.Location = new System.Drawing.Point(679, 48);
            this.nullColorLabel.Name = "nullColorLabel";
            this.nullColorLabel.Size = new System.Drawing.Size(67, 13);
            this.nullColorLabel.TabIndex = 3;
            this.nullColorLabel.Text = "NULL color :";
            // 
            // commandTimeoutLabel
            // 
            this.commandTimeoutLabel.AutoSize = true;
            this.commandTimeoutLabel.Location = new System.Drawing.Point(799, 21);
            this.commandTimeoutLabel.Name = "commandTimeoutLabel";
            this.commandTimeoutLabel.Size = new System.Drawing.Size(97, 13);
            this.commandTimeoutLabel.TabIndex = 2;
            this.commandTimeoutLabel.Text = "Command timeout :";
            // 
            // commandTimeoutNumericUpDown
            // 
            this.commandTimeoutNumericUpDown.Location = new System.Drawing.Point(902, 19);
            this.commandTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.commandTimeoutNumericUpDown.Name = "commandTimeoutNumericUpDown";
            this.commandTimeoutNumericUpDown.Size = new System.Drawing.Size(52, 20);
            this.commandTimeoutNumericUpDown.TabIndex = 1;
            this.commandTimeoutNumericUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // targetDatabaseGroupBox
            // 
            this.targetDatabaseGroupBox.Controls.Add(this.targetsFileSizeValueLabel);
            this.targetDatabaseGroupBox.Controls.Add(this.targetsFileModificationDateValueLabel);
            this.targetDatabaseGroupBox.Controls.Add(this.targetsFileSizeLabel);
            this.targetDatabaseGroupBox.Controls.Add(this.targetsFileModificationDateLabel);
            this.targetDatabaseGroupBox.Controls.Add(this.openTargetsFolderLinkLabel);
            this.targetDatabaseGroupBox.Controls.Add(this.targetsComboBox);
            this.targetDatabaseGroupBox.Location = new System.Drawing.Point(324, 12);
            this.targetDatabaseGroupBox.Name = "targetDatabaseGroupBox";
            this.targetDatabaseGroupBox.Size = new System.Drawing.Size(255, 78);
            this.targetDatabaseGroupBox.TabIndex = 3;
            this.targetDatabaseGroupBox.TabStop = false;
            this.targetDatabaseGroupBox.Text = "Target databases";
            // 
            // targetsFileSizeValueLabel
            // 
            this.targetsFileSizeValueLabel.AutoSize = true;
            this.targetsFileSizeValueLabel.ForeColor = System.Drawing.Color.Brown;
            this.targetsFileSizeValueLabel.Location = new System.Drawing.Point(104, 45);
            this.targetsFileSizeValueLabel.Name = "targetsFileSizeValueLabel";
            this.targetsFileSizeValueLabel.Size = new System.Drawing.Size(60, 13);
            this.targetsFileSizeValueLabel.TabIndex = 22;
            this.targetsFileSizeValueLabel.Text = "<NoValue>";
            // 
            // targetsFileModificationDateValueLabel
            // 
            this.targetsFileModificationDateValueLabel.AutoSize = true;
            this.targetsFileModificationDateValueLabel.ForeColor = System.Drawing.Color.Brown;
            this.targetsFileModificationDateValueLabel.Location = new System.Drawing.Point(104, 58);
            this.targetsFileModificationDateValueLabel.Name = "targetsFileModificationDateValueLabel";
            this.targetsFileModificationDateValueLabel.Size = new System.Drawing.Size(60, 13);
            this.targetsFileModificationDateValueLabel.TabIndex = 21;
            this.targetsFileModificationDateValueLabel.Text = "<NoValue>";
            // 
            // targetsFileSizeLabel
            // 
            this.targetsFileSizeLabel.AutoSize = true;
            this.targetsFileSizeLabel.Location = new System.Drawing.Point(48, 45);
            this.targetsFileSizeLabel.Name = "targetsFileSizeLabel";
            this.targetsFileSizeLabel.Size = new System.Drawing.Size(50, 13);
            this.targetsFileSizeLabel.TabIndex = 20;
            this.targetsFileSizeLabel.Text = "File size :";
            // 
            // targetsFileModificationDateLabel
            // 
            this.targetsFileModificationDateLabel.AutoSize = true;
            this.targetsFileModificationDateLabel.Location = new System.Drawing.Point(4, 58);
            this.targetsFileModificationDateLabel.Name = "targetsFileModificationDateLabel";
            this.targetsFileModificationDateLabel.Size = new System.Drawing.Size(94, 13);
            this.targetsFileModificationDateLabel.TabIndex = 19;
            this.targetsFileModificationDateLabel.Text = "Modification date :";
            // 
            // openTargetsFolderLinkLabel
            // 
            this.openTargetsFolderLinkLabel.AutoSize = true;
            this.openTargetsFolderLinkLabel.Location = new System.Drawing.Point(6, 20);
            this.openTargetsFolderLinkLabel.Name = "openTargetsFolderLinkLabel";
            this.openTargetsFolderLinkLabel.Size = new System.Drawing.Size(49, 13);
            this.openTargetsFolderLinkLabel.TabIndex = 2;
            this.openTargetsFolderLinkLabel.TabStop = true;
            this.openTargetsFolderLinkLabel.Text = "Targets :";
            this.openTargetsFolderLinkLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // targetsComboBox
            // 
            this.targetsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetsComboBox.FormattingEnabled = true;
            this.targetsComboBox.Location = new System.Drawing.Point(61, 17);
            this.targetsComboBox.Name = "targetsComboBox";
            this.targetsComboBox.Size = new System.Drawing.Size(188, 21);
            this.targetsComboBox.TabIndex = 0;
            // 
            // sqlScriptGroupBox
            // 
            this.sqlScriptGroupBox.Controls.Add(this.sqlScriptPreviewTextBox);
            this.sqlScriptGroupBox.Location = new System.Drawing.Point(12, 96);
            this.sqlScriptGroupBox.Name = "sqlScriptGroupBox";
            this.sqlScriptGroupBox.Size = new System.Drawing.Size(960, 348);
            this.sqlScriptGroupBox.TabIndex = 4;
            this.sqlScriptGroupBox.TabStop = false;
            this.sqlScriptGroupBox.Text = "SQL Script";
            // 
            // sqlScriptPreviewTextBox
            // 
            this.sqlScriptPreviewTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(26)))), ((int)(((byte)(15)))));
            this.sqlScriptPreviewTextBox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sqlScriptPreviewTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(175)))), ((int)(((byte)(134)))));
            this.sqlScriptPreviewTextBox.Location = new System.Drawing.Point(6, 19);
            this.sqlScriptPreviewTextBox.Multiline = true;
            this.sqlScriptPreviewTextBox.Name = "sqlScriptPreviewTextBox";
            this.sqlScriptPreviewTextBox.ReadOnly = true;
            this.sqlScriptPreviewTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.sqlScriptPreviewTextBox.Size = new System.Drawing.Size(948, 323);
            this.sqlScriptPreviewTextBox.TabIndex = 22;
            // 
            // outputFileGroupBox
            // 
            this.outputFileGroupBox.Controls.Add(this.outputFileDisplayControl);
            this.outputFileGroupBox.Controls.Add(this.browseOutputFileButton);
            this.outputFileGroupBox.Controls.Add(this.overwriteOutputFileCheckBox);
            this.outputFileGroupBox.Location = new System.Drawing.Point(585, 12);
            this.outputFileGroupBox.Name = "outputFileGroupBox";
            this.outputFileGroupBox.Size = new System.Drawing.Size(306, 78);
            this.outputFileGroupBox.TabIndex = 5;
            this.outputFileGroupBox.TabStop = false;
            this.outputFileGroupBox.Text = "Output file";
            // 
            // outputFileDisplayControl
            // 
            this.outputFileDisplayControl.AbsoluteFilePath = null;
            this.outputFileDisplayControl.Location = new System.Drawing.Point(6, 15);
            this.outputFileDisplayControl.Name = "outputFileDisplayControl";
            this.outputFileDisplayControl.Size = new System.Drawing.Size(218, 47);
            this.outputFileDisplayControl.TabIndex = 14;
            // 
            // browseOutputFileButton
            // 
            this.browseOutputFileButton.Location = new System.Drawing.Point(230, 15);
            this.browseOutputFileButton.Name = "browseOutputFileButton";
            this.browseOutputFileButton.Size = new System.Drawing.Size(70, 23);
            this.browseOutputFileButton.TabIndex = 13;
            this.browseOutputFileButton.Text = "Browse...";
            this.browseOutputFileButton.UseVisualStyleBackColor = true;
            this.browseOutputFileButton.Click += new System.EventHandler(this.browseOutputFileButton_Click);
            // 
            // overwriteOutputFileCheckBox
            // 
            this.overwriteOutputFileCheckBox.AutoSize = true;
            this.overwriteOutputFileCheckBox.Location = new System.Drawing.Point(230, 44);
            this.overwriteOutputFileCheckBox.Name = "overwriteOutputFileCheckBox";
            this.overwriteOutputFileCheckBox.Size = new System.Drawing.Size(71, 17);
            this.overwriteOutputFileCheckBox.TabIndex = 0;
            this.overwriteOutputFileCheckBox.Text = "Overwrite";
            this.overwriteOutputFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(897, 54);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 36);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // sqlFileGroupBox
            // 
            this.sqlFileGroupBox.Controls.Add(this.browseSqlFileButton);
            this.sqlFileGroupBox.Controls.Add(this.sqlFileDisplayControl);
            this.sqlFileGroupBox.Location = new System.Drawing.Point(12, 12);
            this.sqlFileGroupBox.Name = "sqlFileGroupBox";
            this.sqlFileGroupBox.Size = new System.Drawing.Size(306, 78);
            this.sqlFileGroupBox.TabIndex = 7;
            this.sqlFileGroupBox.TabStop = false;
            this.sqlFileGroupBox.Text = "SQL File";
            // 
            // browseSqlFileButton
            // 
            this.browseSqlFileButton.Location = new System.Drawing.Point(230, 15);
            this.browseSqlFileButton.Name = "browseSqlFileButton";
            this.browseSqlFileButton.Size = new System.Drawing.Size(70, 23);
            this.browseSqlFileButton.TabIndex = 1;
            this.browseSqlFileButton.Text = "Browse...";
            this.browseSqlFileButton.UseVisualStyleBackColor = true;
            this.browseSqlFileButton.Click += new System.EventHandler(this.browseSqlFileButton_Click);
            // 
            // sqlFileDisplayControl
            // 
            this.sqlFileDisplayControl.AbsoluteFilePath = "";
            this.sqlFileDisplayControl.Location = new System.Drawing.Point(6, 15);
            this.sqlFileDisplayControl.Name = "sqlFileDisplayControl";
            this.sqlFileDisplayControl.Size = new System.Drawing.Size(218, 47);
            this.sqlFileDisplayControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 961);
            this.Controls.Add(this.sqlFileGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.outputFileGroupBox);
            this.Controls.Add(this.sqlScriptGroupBox);
            this.Controls.Add(this.targetDatabaseGroupBox);
            this.Controls.Add(this.optionGroupBox);
            this.Controls.Add(this.outputGroupBox);
            this.Controls.Add(this.executeButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QueryMultiDb";
            this.outputGroupBox.ResumeLayout(false);
            this.outputGroupBox.PerformLayout();
            this.optionGroupBox.ResumeLayout(false);
            this.optionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parallelismNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nullColorPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandTimeoutNumericUpDown)).EndInit();
            this.targetDatabaseGroupBox.ResumeLayout(false);
            this.targetDatabaseGroupBox.PerformLayout();
            this.sqlScriptGroupBox.ResumeLayout(false);
            this.sqlScriptGroupBox.PerformLayout();
            this.outputFileGroupBox.ResumeLayout(false);
            this.outputFileGroupBox.PerformLayout();
            this.sqlFileGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.GroupBox outputGroupBox;
        private System.Windows.Forms.TextBox consoleOutputTextBox;
        private System.Windows.Forms.GroupBox optionGroupBox;
        private System.Windows.Forms.GroupBox targetDatabaseGroupBox;
        private System.Windows.Forms.LinkLabel openTargetsFolderLinkLabel;
        private System.Windows.Forms.ComboBox targetsComboBox;
        private System.Windows.Forms.GroupBox sqlScriptGroupBox;
        private System.Windows.Forms.GroupBox outputFileGroupBox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label progressStateValueLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label parallelismLabel;
        private System.Windows.Forms.CheckBox hideLotCheckBox;
        private System.Windows.Forms.CheckBox discardResultsCheckBox;
        private System.Windows.Forms.Label sheetlabelsLabel;
        private System.Windows.Forms.TextBox sheetlabelTextBox;
        private System.Windows.Forms.PictureBox nullColorPictureBox;
        private System.Windows.Forms.CheckBox showNullCheckBox;
        private System.Windows.Forms.Label nullColorLabel;
        private System.Windows.Forms.Label commandTimeoutLabel;
        private System.Windows.Forms.NumericUpDown commandTimeoutNumericUpDown;
        private System.Windows.Forms.Button browseOutputFileButton;
        private System.Windows.Forms.CheckBox overwriteOutputFileCheckBox;
        private System.Windows.Forms.TextBox sqlScriptPreviewTextBox;
        private System.Windows.Forms.NumericUpDown parallelismNumericUpDown;
        private System.Windows.Forms.Label sheetlabelLegendLabel;
        private System.Windows.Forms.TextBox commandLineTextBox;
        private System.Windows.Forms.Button commandLineCopyButton;
        private FileDisplayControl outputFileDisplayControl;
        private System.Windows.Forms.GroupBox sqlFileGroupBox;
        private FileDisplayControl sqlFileDisplayControl;
        private System.Windows.Forms.Button browseSqlFileButton;
        private System.Windows.Forms.Label targetsFileSizeValueLabel;
        private System.Windows.Forms.Label targetsFileModificationDateValueLabel;
        private System.Windows.Forms.Label targetsFileSizeLabel;
        private System.Windows.Forms.Label targetsFileModificationDateLabel;
    }
}

