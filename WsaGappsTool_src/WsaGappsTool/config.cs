using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsaGappsTool
{
    public static class config
    {
        public static string vm_dir = "../vm/";
        public static string vm_dataDiskImage = vm_dir + "data.vhdx";
        public static string vm_systemDiskImage = vm_dir + "system.vhdx";
        public static string CacheDirectory = Paths.CacheDirectory;

        public static int DefaultDataDiskImageSizeMB = 8192; // 8GB

        public static string Vm_ExpandSystemImageArgs = @"x system/system.7z.001";
        //public static string QemuRunCommand = 
    }
}
