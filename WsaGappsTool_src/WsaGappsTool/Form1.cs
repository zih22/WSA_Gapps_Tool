using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WsaGappsTool;
using WsaGappsTool.VhdxHelper;

namespace WsaGappsTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //VhdxBuilder.CreateFromDirectory("cache", "data.vhdx", "test", 500).Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            performPathChecks();
        }

        private void automaticInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("The automatic installation feature downloads both the WSA .msix package and the latest gapps package, then automatically begins the modification process. \n\nThis feature is experimental, and therefore may not operate as intended. \n\nWould you like to continue anyway?", "Automatic installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Run
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //string message = "This process might take a while (~10-15 minutes). Are you sure you want to continue?";
            string message = "This process might take a while. Continue?";
            if (MessageBox.Show(message, config.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                PrepareMsixAndGapps prepareMsixAndGapps = new PrepareMsixAndGapps(textBox_msixPackagePath.Text, textBox_gappsPackagePath.Text);
                DialogResult result = prepareMsixAndGapps.ShowDialog();
                if (result == DialogResult.Abort && prepareMsixAndGapps.error)
                {
                    MessageBox.Show(String.Format("Error preparing: {0}", prepareMsixAndGapps.errorMessage), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Directory.Delete(config.CacheDirectory, true);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        void performPathChecks()
        {
            bool error = false;

            if (!File.Exists(config.sevenZip_Ex))
            {
                MessageBox.Show(String.Format("7z executable could not be found at the expected path: {0}", config.sevenZip_Ex));
                error = true;
            }

            if (!File.Exists(config.qemu_Ex))
            {
                MessageBox.Show(String.Format("QEMU executable could not be found at the expected path: {0}", config.qemu_Ex));
                error = true;
            }

            if (error)
            {
                Close();
                Application.Exit();
            }
        }

        void refreshSummaryLabel()
        {
            if (textBox_gappsPackagePath.TextLength < 1 && textBox_msixPackagePath.TextLength < 1)
            {
                summaryLabel.Text = "MSIX and Gapps will be downloaded";
            }
            else
            {
                if (textBox_gappsPackagePath.TextLength < 1)
                {
                    summaryLabel.Text = "Gapps will be downloaded";
                }
                else if (textBox_msixPackagePath.TextLength < 1)
                {
                    summaryLabel.Text = "MSIX will be downloaded";
                }
                else
                {
                    summaryLabel.Text = "";
                }
            }
        }

        private void textBox_msixPackagePath_TextChanged(object sender, EventArgs e)
        {
            refreshSummaryLabel();
            CheckIfSpecifiedPathsExist();
        }

        private void textBox_gappsPackagePath_TextChanged(object sender, EventArgs e)
        {
            refreshSummaryLabel();
            CheckIfSpecifiedPathsExist();
        }

        void CheckIfSpecifiedPathsExist()
        {
            if (textBox_msixPackagePath.TextLength != 0 && !File.Exists(textBox_msixPackagePath.Text))
            {
                msix_fileErrorLabel.Visible = true;
            }
            else
            {
                msix_fileErrorLabel.Visible = false;
            }

            if (textBox_gappsPackagePath.TextLength != 0 && !File.Exists(textBox_gappsPackagePath.Text))
            {
                gapps_fileErrorLabel.Visible = true;
            }
            else
            {
                gapps_fileErrorLabel.Visible = false;
            }
        }

        private void backgroundWorker_checkIfFilesExist_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}