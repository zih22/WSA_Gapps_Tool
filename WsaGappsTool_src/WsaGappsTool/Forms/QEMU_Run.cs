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

        public bool qemu_showWindow = false;

        public static string QemuRunCommandArgs = "";

        public QEMU_Run()
        {
            InitializeComponent();
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
                CloseWithError("Not enough system memory left to start VM.");
            }

            QemuRunCommandArgs = String.Format(@"-no-user-config -display {2} -serial stdio -net nic -net user -M pc{3} -smp cores={0} -m {1} -device ich9-intel-hda -device ich9-ahci,id=sata -device ide-hd,bus=sata.2,drive=os,bootindex=0 -drive id=os,if=none,file=system.qcow2,format=qcow2,snapshot=on -device ide-hd,bus=sata.3,drive=DATA -drive id=DATA,if=none,file=data.vhdx,format=vhdx -device ide-hd,bus=sata.4,drive=CONFIG -drive id=CONFIG,if=none,file=config.vhdx,format=vhdx", cores, config.DefaultVmMemoryAllocationAmount, EvaluateBool(qemu_showWindow, trueValue: "sdl", falseValue: "none"), EvaluateBool(SystemInfo.IsHyperVEnabled(), trueValue: " --accel whpx", falseValue: ""));
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
            {
                return trueValue;
            }
            else return falseValue;
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

            this.Invoke((MethodInvoker)delegate
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            });

            //setStatusText("Waiting up to 120 seconds for VM to come alive...");
            setStatusText("Waiting for VM to come alive...");
            qemuProcess.WaitForExit();


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

            Directory.Move(config.CacheDirectory + "msix", "C:/WSA");

            this.Invoke((MethodInvoker)delegate
            {
                cancelButton.Enabled = true;
                cancelButton.Text = "Close";
                setStatusText("Process complete!");
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 100;
            });

            Directory.Delete(config.CacheDirectory);
        }

        private void QemuProcess_Exited(object sender, EventArgs e)
        {
            //backgroundWorker_qemuVm.CancelAsync();
            //backgroundWorker_copyFiles.RunWorkerAsync();
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
                else if (e.Data.Contains("Done"))
                {
                    setStatusText("Process complete. VM is now shutting down...");
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

        }

        private void backgroundWorker_copyFiles_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (cancelButton.Text == "Close")
            {
                Close();
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to cancel?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backgroundWorker_qemuVm.CancelAsync();
                    qemuProcess.Kill();
                    try
                    {
                        Directory.Delete(config.CacheDirectory);
                    }
                    catch
                    {

                    }
                    Close();
                }
            }
        }
    }
}