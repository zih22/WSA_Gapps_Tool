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
        const string error_systemImagesNotFound = "System image(s) not found on data partition";
        const string error_gappsNotFound = "Gapps not found on data partition";

        public QEMU_Run()
        {
            InitializeComponent();
        }

        private void QEMU_Run_Load(object sender, EventArgs e)
        {
            if(!File.Exists(config.vm_systemDiskImage))
            {
                if (ExpandSystemImage() == false)
                {

                }
            }
        }

        bool ExpandSystemImage()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = Paths._7zipExe;
            processStartInfo.Arguments = config.Vm_ExpandSystemImageArgs;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WorkingDirectory = config.vm_dir;
            Process.Start(processStartInfo).WaitForExit();

            if (File.Exists(config.vm_systemDiskImage)) return true;
            else return false;
        }
    }
}
