using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QueryMultiDbGui
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            guiFilenameTextBox.Text = Path.GetFileName(Application.ExecutablePath);
            guiPathTextBox.Text = Path.GetDirectoryName(Application.ExecutablePath);
            guiVersionTextBox.Text = AssemblyVersion;

            cliFilenameTextBox.Text = Path.GetFileName(MainForm.QueryMultiDbExecutableFilename);
            cliPathTextBox.Text = Path.GetDirectoryName(MainForm.QueryMultiDbExecutableFilename);
            cliVersionTextBox.Text = "Not Implemented";
        }

        private static string GetExternalAssemblyVersion(string fileName)
        {
            var info = FileVersionInfo.GetVersionInfo(fileName);
            var fileVersion = info.ProductVersion;

            return fileVersion;
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }
    }
}
