using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WsaGappsTool
{
    public static class SystemInfo
    {
        public static long GetAvailablePhysicalMemory()
        {
            long availableVirtualMemory = 0;
            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                availableVirtualMemory = long.Parse((string)managementObject["FreePhysicalMemory"]);
            }
            return availableVirtualMemory;
        }

        public static long GetAvailableVirtualMemory()
        {
            long availableVirtualMemory = 0;
            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                availableVirtualMemory = long.Parse((string)managementObject["FreeVirtualMemory"]);
            }
            return availableVirtualMemory;
        }
    }
}
