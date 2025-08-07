namespace QueryMultiDbGui
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.guiVersionTextBox = new System.Windows.Forms.TextBox();
            this.guiFilenameTextBox = new System.Windows.Forms.TextBox();
            this.guiPathTextBox = new System.Windows.Forms.TextBox();
            this.guiVersionLabel = new System.Windows.Forms.Label();
            this.guiPathLabel = new System.Windows.Forms.Label();
            this.guiFilenameLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cliVersionTextBox = new System.Windows.Forms.TextBox();
            this.cliFilenameTextBox = new System.Windows.Forms.TextBox();
            this.cliPathTextBox = new System.Windows.Forms.TextBox();
            this.cliVersionLlabel = new System.Windows.Forms.Label();
            this.cliPathLabel = new System.Windows.Forms.Label();
            this.cliFilenameLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(267, 314);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(224, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "QueryMultiDb";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "by Alexandre Lecoq";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(180, 59);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(244, 13);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://github.com/alexandre-lecoq/QueryMultiDb";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.guiVersionTextBox);
            this.groupBox1.Controls.Add(this.guiFilenameTextBox);
            this.groupBox1.Controls.Add(this.guiPathTextBox);
            this.groupBox1.Controls.Add(this.guiVersionLabel);
            this.groupBox1.Controls.Add(this.guiPathLabel);
            this.groupBox1.Controls.Add(this.guiFilenameLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(570, 101);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GUI";
            // 
            // guiVersionTextBox
            // 
            this.guiVersionTextBox.Location = new System.Drawing.Point(81, 71);
            this.guiVersionTextBox.Name = "guiVersionTextBox";
            this.guiVersionTextBox.ReadOnly = true;
            this.guiVersionTextBox.Size = new System.Drawing.Size(483, 20);
            this.guiVersionTextBox.TabIndex = 11;
            // 
            // guiFilenameTextBox
            // 
            this.guiFilenameTextBox.Location = new System.Drawing.Point(81, 19);
            this.guiFilenameTextBox.Name = "guiFilenameTextBox";
            this.guiFilenameTextBox.ReadOnly = true;
            this.guiFilenameTextBox.Size = new System.Drawing.Size(483, 20);
            this.guiFilenameTextBox.TabIndex = 10;
            // 
            // guiPathTextBox
            // 
            this.guiPathTextBox.Location = new System.Drawing.Point(81, 45);
            this.guiPathTextBox.Name = "guiPathTextBox";
            this.guiPathTextBox.ReadOnly = true;
            this.guiPathTextBox.Size = new System.Drawing.Size(483, 20);
            this.guiPathTextBox.TabIndex = 9;
            // 
            // guiVersionLabel
            // 
            this.guiVersionLabel.AutoSize = true;
            this.guiVersionLabel.Location = new System.Drawing.Point(27, 74);
            this.guiVersionLabel.Name = "guiVersionLabel";
            this.guiVersionLabel.Size = new System.Drawing.Size(48, 13);
            this.guiVersionLabel.TabIndex = 8;
            this.guiVersionLabel.Text = "Version :";
            // 
            // guiPathLabel
            // 
            this.guiPathLabel.AutoSize = true;
            this.guiPathLabel.Location = new System.Drawing.Point(40, 48);
            this.guiPathLabel.Name = "guiPathLabel";
            this.guiPathLabel.Size = new System.Drawing.Size(35, 13);
            this.guiPathLabel.TabIndex = 7;
            this.guiPathLabel.Text = "Path :";
            // 
            // guiFilenameLabel
            // 
            this.guiFilenameLabel.AutoSize = true;
            this.guiFilenameLabel.Location = new System.Drawing.Point(15, 22);
            this.guiFilenameLabel.Name = "guiFilenameLabel";
            this.guiFilenameLabel.Size = new System.Drawing.Size(60, 13);
            this.guiFilenameLabel.TabIndex = 6;
            this.guiFilenameLabel.Text = "File Name :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cliVersionTextBox);
            this.groupBox2.Controls.Add(this.cliFilenameTextBox);
            this.groupBox2.Controls.Add(this.cliPathTextBox);
            this.groupBox2.Controls.Add(this.cliVersionLlabel);
            this.groupBox2.Controls.Add(this.cliPathLabel);
            this.groupBox2.Controls.Add(this.cliFilenameLabel);
            this.groupBox2.Location = new System.Drawing.Point(12, 209);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(570, 99);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CLI";
            // 
            // cliVersionTextBox
            // 
            this.cliVersionTextBox.Location = new System.Drawing.Point(81, 71);
            this.cliVersionTextBox.Name = "cliVersionTextBox";
            this.cliVersionTextBox.ReadOnly = true;
            this.cliVersionTextBox.Size = new System.Drawing.Size(483, 20);
            this.cliVersionTextBox.TabIndex = 5;
            // 
            // cliFilenameTextBox
            // 
            this.cliFilenameTextBox.Location = new System.Drawing.Point(81, 19);
            this.cliFilenameTextBox.Name = "cliFilenameTextBox";
            this.cliFilenameTextBox.ReadOnly = true;
            this.cliFilenameTextBox.Size = new System.Drawing.Size(483, 20);
            this.cliFilenameTextBox.TabIndex = 4;
            // 
            // cliPathTextBox
            // 
            this.cliPathTextBox.Location = new System.Drawing.Point(81, 45);
            this.cliPathTextBox.Name = "cliPathTextBox";
            this.cliPathTextBox.ReadOnly = true;
            this.cliPathTextBox.Size = new System.Drawing.Size(483, 20);
            this.cliPathTextBox.TabIndex = 3;
            // 
            // cliVersionLlabel
            // 
            this.cliVersionLlabel.AutoSize = true;
            this.cliVersionLlabel.Location = new System.Drawing.Point(27, 74);
            this.cliVersionLlabel.Name = "cliVersionLlabel";
            this.cliVersionLlabel.Size = new System.Drawing.Size(48, 13);
            this.cliVersionLlabel.TabIndex = 2;
            this.cliVersionLlabel.Text = "Version :";
            // 
            // cliPathLabel
            // 
            this.cliPathLabel.AutoSize = true;
            this.cliPathLabel.Location = new System.Drawing.Point(40, 48);
            this.cliPathLabel.Name = "cliPathLabel";
            this.cliPathLabel.Size = new System.Drawing.Size(35, 13);
            this.cliPathLabel.TabIndex = 1;
            this.cliPathLabel.Text = "Path :";
            // 
            // cliFilenameLabel
            // 
            this.cliFilenameLabel.AutoSize = true;
            this.cliFilenameLabel.Location = new System.Drawing.Point(15, 22);
            this.cliFilenameLabel.Name = "cliFilenameLabel";
            this.cliFilenameLabel.Size = new System.Drawing.Size(60, 13);
            this.cliFilenameLabel.TabIndex = 0;
            this.cliFilenameLabel.Text = "File Name :";
            // 
            // AboutForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 348);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About QueryMultiDb";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label cliVersionLlabel;
        private System.Windows.Forms.Label cliPathLabel;
        private System.Windows.Forms.Label cliFilenameLabel;
        private System.Windows.Forms.TextBox guiVersionTextBox;
        private System.Windows.Forms.TextBox guiFilenameTextBox;
        private System.Windows.Forms.TextBox guiPathTextBox;
        private System.Windows.Forms.Label guiVersionLabel;
        private System.Windows.Forms.Label guiPathLabel;
        private System.Windows.Forms.Label guiFilenameLabel;
        private System.Windows.Forms.TextBox cliVersionTextBox;
        private System.Windows.Forms.TextBox cliFilenameTextBox;
        private System.Windows.Forms.TextBox cliPathTextBox;
    }
}