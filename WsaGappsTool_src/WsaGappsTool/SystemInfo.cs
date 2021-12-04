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
        /// <summary>
        /// Gets amount of available physical memory
        /// </summary>
        /// <returns>Available physical memory in kilobytes (KB)</returns>
        public static UInt64 GetAvailablePhysicalMemory()
        {
            UInt64 availablePhysicalMemory = 0;
            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                availablePhysicalMemory = (UInt64)managementObject["FreePhysicalMemory"];
            }
            return availablePhysicalMemory;
        }

        /// <summary>
        /// Gets amount of available virtual memory
        /// </summary>
        /// <returns>Available virtual memory in kilobytes (KB)</returns>
        public static UInt64 GetAvailableVirtualMemory()
        {
            UInt64 availableVirtualMemory = 0;
            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                availableVirtualMemory = (UInt64)managementObject["FreeVirtualMemory"];
            }
            return availableVirtualMemory;
        }

        public static bool IsHyperVEnabled()
        {
            bool hyperVEnabled = false;
            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                hyperVEnabled = (bool)managementObject["HypervisorPresent"];
            }
            return hyperVEnabled;
        }
    }
}
