using DiscUtils;
using DiscUtils.Ntfs;
using DiscUtils.Vhdx;
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
using System.Management.Automation;

namespace WsaGappsTool
{
    public partial class QEMU_Run : Form
    {
        ProcessStartInfo qemuProcessStartInfo;
        Process qemuProcess;

        const string error_systemImagesNotFound = "System image(s) not found on data partition";
        const string error_gappsNotFound = "Gapps not found on data partition";

        public bool error;
        public string errorMessage = "";

        public bool qemu_showWindow = false; //Enable to show the QEMU window for debugging purposes

        public static string QemuRunCommandArgs = "";

        public bool ProcessSuccessful = false;
        //bool CanClose = false;

        string WSA_InstallPath = "C:\\  WSA";
        string WSA_AppxManifestPath;

        PerformanceCounter pCounter_usedMemory;
        PerformanceCounter pCounter_cpuUsage;
        PerformanceCounter pCounter_diskWrite;
        PerformanceCounter pCounter_diskRead;

        bool installPackage;

        string moreInfo_collapse_string = "Hide details";
        string moreInfo_expand_string = "Show details";
        bool panel_expanded = false;
        int windowHeight_collapsed = 230;
        int windowHeight_expanded = 380;

        public QEMU_Run(string msixPackage_savePath, bool install)
        {
            InitializeComponent();
            WSA_AppxManifestPath = Path.Combine(WSA_InstallPath, "AppxManifest.xml");
            installPackage = install;
            linkLabel1.Text = moreInfo_expand_string;
        }

        private void QEMU_Run_Load(object sender, EventArgs e)
        {
            // Generate QEMU command line arguments

            // Get available cores
            int cores = Environment.ProcessorCount;
            // Get available system RAM
            long availableMemoryInMB = (long)(SystemInfo.GetAvailablePhysicalMemory() / 1024);

            if (availableMemoryInMB < config.MinAvailableSystemRamForVm_MB)
            {
                //CloseWithError("Not enough system memory left to start VM.");
            }

            QemuRunCommandArgs = String.Format(@"-no-user-config -display {2} -serial stdio -net nic -net user -M pc{3} -smp cores={0} -m {1} -device ich9-intel-hda -device ich9-ahci,id=sata -device ide-hd,bus=sata.2,drive=os,bootindex=0 -drive id=os,if=none,file=system.qcow2,format=qcow2,snapshot=on -device ide-hd,bus=sata.3,drive=DATA -drive id=DATA,if=none,file=data.vhdx,format=vhdx -device ide-hd,bus=sata.4,drive=CONFIG -drive id=CONFIG,if=none,file=config.vhdx,format=vhdx,snapshot=on", cores, config.DefaultVmMemoryAllocationAmount, EvaluateBool(qemu_showWindow, trueValue: "sdl", falseValue: "none"), EvaluateBool(SystemInfo.IsHyperVEnabled(), trueValue: " --accel whpx", falseValue: ""));
            backgroundWorker_qemuVm.RunWorkerAsync();
        }

        void setStatusText(string text)
        {
            this.Invoke((MethodInvoker)delegate
            {
                label_processStatus.Text = text;
            });
        }

        bool ExpandSystemImage()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = Path.GetFullPath(Resources.Paths._7zipExe);
            processStartInfo.Arguments = config.Vm_ExpandSystemImageArgs;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.WorkingDirectory = config.vm_dir;
            Process.Start(processStartInfo).WaitForExit();

            if (File.Exists(config.vm_systemDiskImage)) return true;
            else return false;
        }

        void CloseWithError(string message)
        {
            error = true;
            errorMessage = message;
            if (qemuProcess != null)
            {
                qemuProcess.Kill();
            }
            Close();
        }

        string EvaluateBool(bool value, string trueValue, string falseValue)
        {
            if (value)
                return trueValue;
            else return falseValue;
        }

        void SetExit()
        {
            //CanClose = true;
            cancelButton.Enabled = true;
            cancelButton.Text = "Close";
        }

        private void backgroundWorker_qemuVm_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!File.Exists(config.vm_systemDiskImage))
            {
                setStatusText("Expanding system image...");
                if (ExpandSystemImage() == false)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        CloseWithError(String.Format("Could not find or open system image ({0}).", config.vm_systemDiskImage));
                    });
                }
            }

            qemuProcessStartInfo = new ProcessStartInfo(Resources.Paths.QemuExe, QemuRunCommandArgs);
            qemuProcessStartInfo.RedirectStandardOutput = true;
            qemuProcessStartInfo.RedirectStandardError = true;
            qemuProcessStartInfo.CreateNoWindow = true;
            qemuProcessStartInfo.UseShellExecute = false;
            qemuProcessStartInfo.WorkingDirectory = config.vm_dir;

            qemuProcess = new Process();
            qemuProcess.StartInfo = qemuProcessStartInfo;
            qemuProcess.OutputDataReceived += QemuProcess_OutputDataReceived;
            qemuProcess.ErrorDataReceived += QemuProcess_ErrorDataReceived;
            qemuProcess.Exited += QemuProcess_Exited;
            qemuProcess.Start();
            qemuProcess.BeginOutputReadLine();
            qemuProcess.BeginErrorReadLine();

            pCounter_cpuUsage = new PerformanceCounter("Process", "% Processor Time", qemuProcess.ProcessName);
            pCounter_usedMemory = new PerformanceCounter("Process", "Working Set - Private", qemuProcess.ProcessName);
            pCounter_diskWrite = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", qemuProcess.ProcessName);
            pCounter_diskRead = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", qemuProcess.ProcessName);

            timer_performanceCounters.Enabled = true;

            while (!this.IsHandleCreated)
            {
                // Do nothing; just wait
            }

            this.Invoke((MethodInvoker)delegate
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            });

            //setStatusText("Waiting up to 120 seconds for VM to come alive...");
            setStatusText("Waiting for VM to come alive...");
            qemuProcess.WaitForExit();
            VerifyProcessWasSuccessful();
        }

        void VerifyProcessWasSuccessful()
        {
            if (ProcessSuccessful)
            {
                backgroundWorker_copyFiles.RunWorkerAsync();
            }
            else
            {
                setStatusText("Something went wrong during processing.");
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar.Style = ProgressBarStyle.Blocks;
                    progressBar.Value = 100;
                });
            }
        }

        private void QemuProcess_Exited(object sender, EventArgs e)
        {

        }

        private void QemuProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.Contains("Ready"))
                {
                    setStatusText("VM is running. Getting ready...");
                }
                else if (e.Data.Contains(error_gappsNotFound))
                {
                    CloseWithError(error_gappsNotFound);
                }
                else if (e.Data.Contains(error_systemImagesNotFound))
                {
                    CloseWithError(error_systemImagesNotFound);
                }
                else if (e.Data.Contains("Shutting down"))
                {
                    setStatusText("Process complete. VM is now shutting down...");
                    ProcessSuccessful = true;
                }
                else
                {
                    setStatusText(e.Data);
                }
            }
        }

        private void QemuProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        private void backgroundWorker_copyFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            // Initialize DiscUtils
            setStatusText("Opening disk...");
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            DiscUtils.Setup.SetupHelper.RegisterAssembly(assembly);
            DiscUtils.Containers.SetupHelper.SetupContainers();
            DiscUtils.Complete.SetupHelper.SetupComplete();
            DiscUtils.FileSystems.SetupHelper.SetupFileSystems();

            setStatusText("Preparing to copy files from disk...");
            this.Invoke((MethodInvoker)delegate
            {
                cancelButton.Enabled = false;
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
            });

            using (VirtualDisk vd = VirtualDisk.OpenDisk(config.vm_dataDiskImage, FileAccess.Read))
            {
                using (NtfsFileSystem ntfsFileSystem = new NtfsFileSystem(vd.Partitions[0].Open()))
                {
                    string[] images = ntfsFileSystem.GetFiles("images", "*.img", SearchOption.AllDirectories);
                    int currentIndex = 0;
                    foreach (string image in images)
                    {
                        string imageFilename = Path.GetFileName(image);
                        setStatusText(String.Format("Copying {0}...", imageFilename));
                        this.Invoke((MethodInvoker)delegate
                        {
                            progressBar.Value = (int)(currentIndex / images.Count());
                        });
                        FileStream fileStream = File.Create(config.CacheDirectory + "msix/" + imageFilename);
                        ntfsFileSystem.OpenFile(image, FileMode.Open).CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                        currentIndex++;
                    }
                }
            }
            MoveContents();
            if (installPackage)
            {
                InstallPackage();
            }
            setStatusText("Finishing up...");
        }

        bool MoveContents()
        {
            setStatusText(String.Format("Moving package contents to {0}...", WSA_InstallPath));
            if (!Directory.Exists(WSA_InstallPath))
            {
                Directory.CreateDirectory(WSA_InstallPath);
            }
            try
            {
                Directory.Move(config.CacheDirectory + "msix/*", WSA_InstallPath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Could not move package contents to {1}: {0}", WSA_InstallPath, ex.Message), "Error moving contents", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        bool InstallPackage()
        {
            string command = String.Format("Add-AppxPackage -Register {0}", WSA_AppxManifestPath);
            setStatusText("Installing...");
            PowerShell powerShell = PowerShell.Create();
            powerShell.AddScript(command);
            powerShell.Invoke();

            if (powerShell.HadErrors)
            {
                string message = "";
                string message_suffix = "You can try opening an elevated instance of PowerShell, and running the following command: \r\n\r\n" + command;
                if (File.Exists(WSA_AppxManifestPath))
                {
                    message = String.Format("The package has been moved to C:/WSA, but the AppxManifest.xml file could not be registered. {0}", message_suffix);
                }
                else
                {
                    message = "Something happened, and the package couldn't be registered.";
                }
                MessageBox.Show(message, "Error registering AppxManifest.xml", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            return !powerShell.HadErrors;
        }

        private void backgroundWorker_copyFiles_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                setStatusText("Process complete!");
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 100;
                SetExit();
            });
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (cancelButton.Text == "Close")
            {
                Exit();
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to cancel the current operation?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backgroundWorker_qemuVm.CancelAsync();
                    try
                    {
                        qemuProcess.Kill();
                    }
                    catch
                    {

                    }
                    Exit();
                }
            }
        }

        void Exit()
        {
            //Directory.Delete(config.CacheDirectory);
            AskAboutClearingCache();
            Close();
        }

        void AskAboutClearingCache()
        {
            if (MessageBox.Show("Do you want to delete the cache directory? \r\n\r\n" + Path.GetFullPath(config.CacheDirectory), "Delete cache?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Directory.Delete(config.CacheDirectory, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cache directory could not be deleted. " + ex.Message, "Error deleting cache", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void timer_performanceCounters_Tick(object sender, EventArgs e)
        {
            try
            {
                // Get CPU usage percentage

                // Get process memory usage

                string mem_usage = String.Format("Memory Usage: {0:n1} MB", Math.Round((pCounter_usedMemory.NextValue() / 1024 / 1024), 1));

                //Get disk write speed in MB/s
                //string disk_write_speed = String.Format("Disk Write Speed: {0:n1} MB/s", Math.Round((pCounter_diskWrite.NextValue() / 1024 / 1024), 1));
                label2.Text = mem_usage;
            }
            catch (Exception ex)
            {
                string exc = ex.Message;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(panel_expanded)
            {
                this.Height = windowHeight_collapsed;
                linkLabel1.Text = moreInfo_expand_string;
                panel_expanded = false;
            }
            else
            {
                this.Height = windowHeight_expanded;
                linkLabel1.Text = moreInfo_collapse_string;
                panel_expanded = true;
            }
        }
    }
}