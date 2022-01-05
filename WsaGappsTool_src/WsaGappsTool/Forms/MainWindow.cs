using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WsaGappsTool;
using WsaGappsTool.Resources;
using WsaGappsTool.VhdxHelper;

namespace WsaGappsTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            performPathChecks();
            textBox_outputDirectory.Text = config.DefaultOutputDirectory;
        }

        void RunProcess()
        {
            string driveLetter;
            if (IsDriveTooSmall(out driveLetter))
            {
                MessageBox.Show(String.Format("Error starting process: Drive {0} has less than 8GB of space available. Free up space on the drive, then try again.", driveLetter), "Not enough space", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string message = "This process might take a while. Continue?";
                if (MessageBox.Show(message, Resources.Resources.Config_AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PrepareMsixAndGapps prepareMsixAndGapps = new PrepareMsixAndGapps(textBox_msixPackagePath.Text, textBox_gappsPackagePath.Text, SystemArchitecture.AMD64);
                    DialogResult result = prepareMsixAndGapps.ShowDialog();
                    if (result == DialogResult.Abort && prepareMsixAndGapps.error)
                    {
                        MessageBox.Show(String.Format("Error preparing files: {0}", prepareMsixAndGapps.errorMessage), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        QEMU_Run qemu = new QEMU_Run(textBox_outputDirectory.Text, checkBox_installPackage.Checked);
                        DialogResult qemuResult = qemu.ShowDialog();
                        if (!qemu.ProcessSuccessful)
                        {
                            MessageBox.Show(String.Format("Error modifying images: {0}", qemu.errorMessage), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            try
                            {
                                KillAllQEMUProcesses();
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            if(checkBox_openDirectoryWhenComplete.Checked)
                            {
                                Process.Start(textBox_outputDirectory.Text);
                            }
                            MessageBox.Show("Process complete!", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        void KillAllQEMUProcesses()
        {
            // Find all processes of the QEMU executable
            string qemu = Paths.QemuExe;
            // Get processes by executable
            Process[] qemuProcesses = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(qemu));
            Process[] qemuProcesses2 = Process.GetProcessesByName(Path.GetFileName(qemu));
            foreach (Process qemuProcess in qemuProcesses)
            {
                qemuProcess.Kill();
            }
            foreach (Process qemuProcess in qemuProcesses2)
            {
                qemuProcess.Kill();
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            //string message = "This process might take a while (~10-15 minutes). Are you sure you want to continue?";
            string outputPath = textBox_outputDirectory.Text;
            if (outputPath != "")
            {
                bool PathIsValid = true;
                foreach (char c in Path.GetInvalidPathChars())
                {
                    if (outputPath.Contains(c))
                    {
                        PathIsValid = false;
                        break;
                    }
                }
                if (!PathIsValid)
                {
                    MessageBox.Show("Error starting process: The output path contains invalid characters.", "Invalid path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (Directory.Exists(outputPath))
                    {
                        if (IsDirectoryEmpty(outputPath))
                        {
                            RunProcess();
                        }
                        else
                        {
                            MessageBox.Show("Error starting process: The output directory is not empty.", "Directory not empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("The output directory does not exist. Would you like to create it?", "Directory does not exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Directory.CreateDirectory(outputPath);
                            RunProcess();
                        }
                        else
                        {
                            MessageBox.Show("Process was cancelled.", "Process cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Output path must be specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool IsDirectoryEmpty(string path)
        {
            // string[] files = Directory.GetFiles(path);
            // string[] directories = Directory.GetDirectories(path);
            // 
            // if (files.Length > 0 || directories.Length > 0)
            // {
            //     return false;
            // }
            // else return true;

            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        bool IsDriveTooSmall(out string driveLetter)
        {
            // Get the drive we're running on
            FileInfo fileInfo = new FileInfo(Application.StartupPath);
            driveLetter = fileInfo.Directory.Root.FullName;
            DriveInfo driveInfo = new DriveInfo(driveLetter);
            long freeSpace = driveInfo.AvailableFreeSpace / 1024 / 1024;
            if (freeSpace < 8192) // 8GB
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        void performPathChecks()
        {
            bool error = false;

            if (!File.Exists(Paths._7zipExe))
            {
                MessageBox.Show(String.Format("7z executable could not be found at the expected path: {0}", Paths._7zipExe));
                error = true;
            }

            if (!File.Exists(Paths.QemuExe))
            {
                MessageBox.Show(String.Format("QEMU executable could not be found at the expected path: {0}", Paths.QemuExe));
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
            string msixStr = "MSIX";
            string gappsStr = "Gapps";

            if (textBox_gappsPackagePath.TextLength < 1 && textBox_msixPackagePath.TextLength < 1)
            {
                summaryLabel.Text = String.Format("{0} and {1} will be downloaded", msixStr, gappsStr);
            }
            else
            {
                if (textBox_gappsPackagePath.TextLength < 1)
                {
                    summaryLabel.Text = String.Format("{0} will be downloaded", gappsStr);
                }
                else if (textBox_msixPackagePath.TextLength < 1)
                {
                    summaryLabel.Text = String.Format("{0} will be downloaded", msixStr);
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

        bool CheckIfSpecifiedPathsExist()
        {
            bool finalValue = true;
            if (textBox_msixPackagePath.TextLength != 0 && !File.Exists(textBox_msixPackagePath.Text))
            {
                msix_fileErrorLabel.Visible = true;
                finalValue = false;
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
                finalValue = false;
            }
            return finalValue;
        }

        private void button_browseForMsixPackage_Click(object sender, EventArgs e)
        {
            if (msixPackage_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox_msixPackagePath.Text = msixPackage_openFileDialog.FileName;
            }
        }

        private void button_browseForGappsPackage_Click(object sender, EventArgs e)
        {
            if (gappsPackage_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox_gappsPackagePath.Text = gappsPackage_openFileDialog.FileName;
            }
        }

        private void viewProjectOnGitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Resources.Resources.GitHubRepoURL);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}