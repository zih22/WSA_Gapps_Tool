﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsaGappsTool
{
    public static class config
    {
        public static string sevenZip_Ex = "../tools/7z/7z.exe";
        public static string qemu_Ex = "../qemu/qemu-system-x86_64.exe";
        public static string vm_dir = "../vm/";
        public static string vm_dataDiskImage = vm_dir + "data.vhdx";

        public static string CacheDirectory = "cache/";
    }
}
