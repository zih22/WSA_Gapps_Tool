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
        public static string vm_systemDiskImage = vm_dir + "system.qcow2";
        public static string CacheDirectory = Resources.Paths.CacheDirectory;

        public static int DefaultDataDiskImageSizeMB = 8192; // 8GB

        //public static int MinAvailableSystemRamForVm_MB = 2048; // 2GB
        public static int MinAvailableSystemRamForVm_MB = 1024; // 1GB

        public static int DefaultVmMemoryAllocationAmount = 1024;

        public static string Vm_ExpandSystemImageArgs = @"x system/system.7z.001";
    }
}
