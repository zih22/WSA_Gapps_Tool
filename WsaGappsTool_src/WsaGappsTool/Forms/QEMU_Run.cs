﻿using System;
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

        public bool error;
        public string errorMessage = "";

        public static string QemuRunCommandArgs = @"-no-user-config -display sdl -serial stdio -net nic -net user -display sdl -M pc -smp cores=4 -m 1024 -device ich9-intel-hda -device ich9-ahci,id=sata -device ide-hd,bus=sata.2,drive=ARCH,bootindex=0 -drive id=ARCH,if=none,file=system.qcow2,format=qcow2,snapshot=on -device ide-hd,bus=sata.3,drive=DATA -drive id=DATA,if=none,file=data.vhdx,format=vhdx -device ide-hd,bus=sata.4,drive=CONFIG -drive id=CONFIG,if=none,file=config.vhdx,format=vhdx";

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
                    error = true; 
                    errorMessage = "Could not find or open system image (system.vhdx).";
                }
            }

        }

        bool ExpandSystemImage()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = Resources.Paths._7zipExe;
            processStartInfo.Arguments = config.Vm_ExpandSystemImageArgs;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WorkingDirectory = config.vm_dir;
            Process.Start(processStartInfo).WaitForExit();

            if (File.Exists(config.vm_systemDiskImage)) return true;
            else return false;
        }

    }
}