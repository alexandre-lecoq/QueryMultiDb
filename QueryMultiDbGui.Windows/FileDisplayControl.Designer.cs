namespace QueryMultiDbGui
{
    partial class FileDisplayControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.filePathValueLinkLabel = new System.Windows.Forms.LinkLabel();
            this.filePathLabel = new System.Windows.Forms.Label();
            this.fileSizeValueLabel = new System.Windows.Forms.Label();
            this.fileModificationDateValueLabel = new System.Windows.Forms.Label();
            this.fileSizeLabel = new System.Windows.Forms.Label();
            this.fileModificationDateLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // filePathValueLinkLabel
            // 
            this.filePathValueLinkLabel.AutoSize = true;
            this.filePathValueLinkLabel.Location = new System.Drawing.Point(104, 4);
            this.filePathValueLinkLabel.Name = "filePathValueLinkLabel";
            this.filePathValueLinkLabel.Size = new System.Drawing.Size(60, 13);
            this.filePathValueLinkLabel.TabIndex = 20;
            this.filePathValueLinkLabel.TabStop = true;
            this.filePathValueLinkLabel.Text = "<NoValue>";
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(45, 4);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(53, 13);
            this.filePathLabel.TabIndex = 19;
            this.filePathLabel.Text = "File path :";
            // 
            // fileSizeValueLabel
            // 
            this.fileSizeValueLabel.AutoSize = true;
            this.fileSizeValueLabel.ForeColor = System.Drawing.Color.Brown;
            this.fileSizeValueLabel.Location = new System.Drawing.Point(104, 17);
            this.fileSizeValueLabel.Name = "fileSizeValueLabel";
            this.fileSizeValueLabel.Size = new System.Drawing.Size(60, 13);
            this.fileSizeValueLabel.TabIndex = 18;
            this.fileSizeValueLabel.Text = "<NoValue>";
            // 
            // fileModificationDateValueLabel
            // 
            this.fileModificationDateValueLabel.AutoSize = true;
            this.fileModificationDateValueLabel.ForeColor = System.Drawing.Color.Brown;
            this.fileModificationDateValueLabel.Location = new System.Drawing.Point(104, 30);
            this.fileModificationDateValueLabel.Name = "fileModificationDateValueLabel";
            this.fileModificationDateValueLabel.Size = new System.Drawing.Size(60, 13);
            this.fileModificationDateValueLabel.TabIndex = 17;
            this.fileModificationDateValueLabel.Text = "<NoValue>";
            // 
            // fileSizeLabel
            // 
            this.fileSizeLabel.AutoSize = true;
            this.fileSizeLabel.Location = new System.Drawing.Point(48, 17);
            this.fileSizeLabel.Name = "fileSizeLabel";
            this.fileSizeLabel.Size = new System.Drawing.Size(50, 13);
            this.fileSizeLabel.TabIndex = 16;
            this.fileSizeLabel.Text = "File size :";
            // 
            // fileModificationDateLabel
            // 
            this.fileModificationDateLabel.AutoSize = true;
            this.fileModificationDateLabel.Location = new System.Drawing.Point(4, 30);
            this.fileModificationDateLabel.Name = "fileModificationDateLabel";
            this.fileModificationDateLabel.Size = new System.Drawing.Size(94, 13);
            this.fileModificationDateLabel.TabIndex = 15;
            this.fileModificationDateLabel.Text = "Modification date :";
            // 
            // FileDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.filePathValueLinkLabel);
            this.Controls.Add(this.filePathLabel);
            this.Controls.Add(this.fileSizeValueLabel);
            this.Controls.Add(this.fileModificationDateValueLabel);
            this.Controls.Add(this.fileSizeLabel);
            this.Controls.Add(this.fileModificationDateLabel);
            this.Name = "FileDisplayControl";
            this.Size = new System.Drawing.Size(218, 47);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel filePathValueLinkLabel;
        private System.Windows.Forms.Label filePathLabel;
        private System.Windows.Forms.Label fileSizeValueLabel;
        private System.Windows.Forms.Label fileModificationDateValueLabel;
        private System.Windows.Forms.Label fileSizeLabel;
        private System.Windows.Forms.Label fileModificationDateLabel;
    }
}
