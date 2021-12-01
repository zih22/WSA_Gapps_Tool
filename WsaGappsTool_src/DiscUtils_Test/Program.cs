using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscUtils;
using DiscUtils.Fat;
using DiscUtils.Ntfs;
using DiscUtils.Partitions;
using DiscUtils.Vhdx;

namespace DiscUtils_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            DiscUtils.Setup.SetupHelper.RegisterAssembly(assembly);
            DiscUtils.Containers.SetupHelper.SetupContainers();
            DiscUtils.Complete.SetupHelper.SetupComplete();
            DiscUtils.FileSystems.SetupHelper.SetupFileSystems();

            using (Stream vhdStream = File.Create(@"C:\data.vhdx"))
            {
                VolumeManager volumeManager = new VolumeManager();
                Disk disk = Disk.InitializeDynamic(vhdStream, DiscUtils.Streams.Ownership.None, 8L * 1024L * 1024L * 1024L);
                BiosPartitionTable.Initialize(disk, WellKnownPartitionType.WindowsNtfs);
                volumeManager.AddDisk(disk);
                NtfsFileSystem fs = NtfsFileSystem.Format(volumeManager.GetPhysicalVolumes()[0], "WSA_Files");
                fs.CreateDirectory("images");
                fs.CreateDirectory("gapps");
                using (Stream s = fs.OpenFile("gapps.zip", FileMode.Create))
                {
                    byte[] bytes = File.ReadAllBytes(@"C:\Users\jmig2\Documents\GitHub\WSA_Gapps_Tool\bin\cache2\gapps.zip");
                    s.Write(bytes, 0, bytes.Length);
                    s.Close();
                }
            }

            //using (Stream vhdStream = File.OpenWrite(@"C:\data.vhdx"))
            //{
            //
            //    VirtualDisk vhdx = DiscUtils.Vhdx.Disk.OpenDisk(@"data.vhdx", FileAccess.Write);
            //}
        }
    }
}
