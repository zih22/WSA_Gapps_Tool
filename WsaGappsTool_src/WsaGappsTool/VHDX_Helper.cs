using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscUtils;
using DiscUtils.Fat;
using DiscUtils.Ntfs;
using DiscUtils.Partitions;
using DiscUtils.Vhdx;

namespace WsaGappsTool.VhdxHelper
{
    [Serializable]
    public class FileAlreadyExistsException : Exception
    {
        private readonly string file;
        public FileAlreadyExistsException(string sFile) : base(string.Format("Error creating file on filesystem: File \"{0}\" already exists.", sFile))
        {
            file = sFile;
        }

        public FileAlreadyExistsException(string sFile, string message) : base(message)
        {
            file = sFile;
        }

        private string FilePath
        {
            get
            {
                return file;
            }
        }
    }

    [Serializable]
    public class DirectoryNotEmptyException : Exception
    {
        private readonly string directory;
        public DirectoryNotEmptyException(string dir) : base(string.Format("Virtual directory \"{0}\" already exists.", dir))
        {
            directory = dir;
        }

        public DirectoryNotEmptyException(string dir, string message) : base(message)
        {
            directory = dir;
        }

        private string DirectoryPath
        {
            get
            {
                return directory;
            }
        }
    }

    public delegate void FileAddCompleteEventHandler(object sender, FileAddCompleteEventArgs e);
    public delegate void FilesystemOperationCompleteEventHandler(object sender, FilesystemOperationCompleteEventArgs e);

    public class FileAddCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// The path of the local file that was added to the filesystem
        /// </summary>
        public string Source { get; internal set; }
        /// <summary>
        /// The path on the virtual disk's filesystem where the file was added
        /// </summary>
        public string Destination { get; internal set; }
        /// <summary>
        /// Operation status (true if failed)
        /// </summary>
        public bool Failed { get; internal set; }
        /// <summary>
        /// Total number of bytes written
        /// </summary>
        public long BytesWritten { get; internal set; }
        /// <param name="sourceFile">The path of the local file that was added to the filesystem</param>
        /// <param name="destinationFile">The path on the virtual disk's filesystem where the file was added</param>
        public FileAddCompleteEventArgs(string sourceFile, string destinationFile, long bytesWritten, bool failed)
        {
            Source = sourceFile;
            Destination = destinationFile;
            BytesWritten = bytesWritten;
            Failed = failed;
        }
    }

    public enum FilesystemOperation
    {
        AddFile = 0,
        DeleteFile = 1,
        CreateDirectory = 2,
        DeleteDirectory = 3
    }

    public class FilesystemOperationCompleteEventArgs : EventArgs
    {
        public FilesystemOperation Operation { get; internal set; }
        public string VirtualPath { get; internal set; }
        public bool Failed { get; internal set; }

        public FilesystemOperationCompleteEventArgs(FilesystemOperation filesystemOperation, string pathOnImage, bool failed)
        {
            Operation = filesystemOperation;
            VirtualPath = pathOnImage;
            Failed = failed;
        }
    }

    public class VhdxBuilder : IDisposable
    {
        protected static string getFrontDirectory(string fullPath, string dirRoot)
        {
            string path = fullPath.Substring(dirRoot.Length);
            // Remove backslashes from the beginning
            while (path.StartsWith(@"\"))
            {
                path = path.Substring(1);
            }
            while (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
            // And from the end
            while (path.EndsWith(@"\"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            while (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return path;
        }

        public static VhdxBuilder CreateFromDirectory(string sourceDirectory, string vhdxPath, string volumeLabel, long sizeInMB)
        {
            VhdxBuilder vhdxBuilder = new VhdxBuilder(vhdxPath, sizeInMB, volumeLabel);
            string[] files = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);
            string[] directories = Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories);
            foreach (string directory in directories)
            {
                vhdxBuilder.CreateDirectory(getFrontDirectory(directory, sourceDirectory));
            }
            foreach (string file in files)
            {
                Debug.WriteLine(String.Format("FILE - Path: \"{0}\" ; Cropped: \"{1}\"", file, getFrontDirectory(file, sourceDirectory)));
                vhdxBuilder.AddFile(file, getFrontDirectory(file, sourceDirectory), true);
            }
            return vhdxBuilder;
        }

        bool throwErrorOnVolumeLabelOverflow = false;

        /// Constant values
        private const long BytesInKB = 1024L;
        //private const long BytesInKB = 1000L;
        private const long BytesInMB = BytesInKB * BytesInKB;
        private const long BytesInGB = BytesInMB * BytesInKB;

        // Event Handlers
        public event EventHandler<FileAddCompleteEventArgs> FileAddCompleteEventHandler;
        public event EventHandler<FilesystemOperationCompleteEventArgs> FilesystemOperationCompleteEventHandler;

        protected virtual void OnFileAddComplete(FileAddCompleteEventArgs e)
        {
            EventHandler<FileAddCompleteEventArgs> handler = FileAddCompleteEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnFilesystemOperationComplete(FilesystemOperationCompleteEventArgs e)
        {
            EventHandler<FilesystemOperationCompleteEventArgs> handler = FilesystemOperationCompleteEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Define class instances
        FileStream vhdStream; // Stream for the virtual hard disk
        VolumeManager volumeManager; // The volume manager; required to format NTFS
        Disk disk; // Virtual disk declaration
        NtfsFileSystem fs; // Filesystem class: this is how we work with the filesystem (add files, etc.)

        /// <summary>
        /// Creates a new NTFS-formatted VHDX image
        /// </summary>
        /// <param name="vhdxPath">Path for the new image</param>
        /// <param name="volumeCapacityInMB">The size (in megabytes) for the new image</param>
        /// <param name="volumeLabel">The volume label for the primary partition</param>
        public VhdxBuilder(string vhdxPath, long volumeCapacityInMB, string volumeLabel)
        {
            if(volumeLabel.Length >= 32)
            {
                // Volume label is too large. We can either chop it, or throw an error.
                if(throwErrorOnVolumeLabelOverflow)
                {
                    // code to throw Exception
                    throw new ArgumentOutOfRangeException("Volume Label", "The volume label is too long. The max allowed size is 32 characters.");
                }
                else
                {
                    volumeLabel = volumeLabel.Substring(0, 31);
                }    
            }
            // Initialize DiscUtils
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            DiscUtils.Setup.SetupHelper.RegisterAssembly(assembly);
            DiscUtils.Containers.SetupHelper.SetupContainers();
            DiscUtils.Complete.SetupHelper.SetupComplete();
            DiscUtils.FileSystems.SetupHelper.SetupFileSystems();

            // Initialize class instances
            vhdStream = File.Create(vhdxPath); // Initialize data stream
            volumeManager = new VolumeManager(); // Create the volume manager; we'll need to add the disk instance to it later.
            disk = Disk.InitializeDynamic(vhdStream, DiscUtils.Streams.Ownership.None, megabytesToBytes(volumeCapacityInMB)); // Initialize the virtual disk
            BiosPartitionTable.Initialize(disk, WellKnownPartitionType.WindowsNtfs); // Initialize the partition table, and create a new primary partition
            volumeManager.AddDisk(disk); // Add the new disk instance to the volume manager
            fs = NtfsFileSystem.Format(volumeManager.GetPhysicalVolumes()[0], volumeLabel, new NtfsFormatOptions().BootCode = null) ; // Select the first partition (volume), and format it as NTFS.
        }

        /// <summary>
        /// The path to where the VHDX image is located
        /// </summary>
        public string ImagePath { get; internal set; }

        /// <summary>
        /// Size of the filesystem in megabytes
        /// </summary>
        public long TotalCapacity
        {
            get
            {
                return fs.Size;
            }
        }

        /// <summary>
        /// Amount of space used in megabytes
        /// </summary>
        public long SpaceUsed
        {
            get
            {
                return fs.UsedSpace;
            }
        }

        /// <summary>
        /// Size of the filesystem in megabytes
        /// </summary>
        public long SpaceAvailable
        {
            get
            {
                return fs.AvailableSpace;
            }
        }

        /// <summary>
        /// Amount of disk space used represented as a percentage
        /// </summary>
        public double DiskSpaceUsedPercentage
        {
            get
            {
                return fs.UsedSpace / fs.Size;
            }
        }

        /// <summary>
        /// Amount of disk space used represented as a percentage
        /// </summary>
        public string VolumeLabel
        {
            get
            {
                return fs.VolumeLabel;
            }
        }

        // /// <summary>
        // /// Size of the filesystem in megabytes
        // /// </summary>
        // public long CurrentFilesystemSizeInMB { get; internal set; }

        /// <summary>
        /// Copy a locally stored file to a given location on the virtual filesystem
        /// </summary>
        /// <param name="SourceFile">The path to the file stored locally to add to the virtual disk</param>
        /// <param name="TargetPathOnImage">Location on the virtual disk where the source file should be saved</param>
        /// <param name="overwrite">If true, the file will be overwritten if it exists.</param>
        public void AddFile(string SourceFile, string TargetPathOnImage, bool overwrite)
        {
            bool failed = false;
            long bytesWritten;
            try
            {
                if (fs.FileExists(TargetPathOnImage))
                {
                    if (overwrite)
                    {
                        fs.DeleteFile(TargetPathOnImage);
                    }
                    else
                    {
                        throw new FileAlreadyExistsException(TargetPathOnImage, String.Format("Error creating file: The file \"{0}\" already exists, and the overwrite option was not enabled."));
                    }
                }
                using (Stream s = fs.OpenFile(TargetPathOnImage, FileMode.Create))
                {
                    byte[] bytes = File.ReadAllBytes(SourceFile);
                    s.Write(bytes, 0, bytes.Length);
                    s.Close();
                    bytesWritten = bytes.Length;
                }

                FileAddCompleteEventArgs fileAddCompleteEventArgs = new FileAddCompleteEventArgs(SourceFile, TargetPathOnImage, bytesWritten, failed);
                OnFileAddComplete(fileAddCompleteEventArgs);
                FilesystemOperationCompleteEventArgs filesystemOperationCompleteEventArgs = new FilesystemOperationCompleteEventArgs(FilesystemOperation.AddFile, TargetPathOnImage, failed);
                OnFilesystemOperationComplete(filesystemOperationCompleteEventArgs);
            }
            catch (Exception ex)
            {
                failed = true;
                FileAddCompleteEventArgs fileAddCompleteEventArgs = new FileAddCompleteEventArgs(SourceFile, TargetPathOnImage, 0, failed);
                OnFileAddComplete(fileAddCompleteEventArgs);
                FilesystemOperationCompleteEventArgs filesystemOperationCompleteEventArgs = new FilesystemOperationCompleteEventArgs(FilesystemOperation.AddFile, TargetPathOnImage, failed);
                OnFilesystemOperationComplete(filesystemOperationCompleteEventArgs);
            }
        }

        /// <summary>
        /// Delete file from the virtual filesystem
        /// </summary>
        /// <param name="FilePath">Location on the virtual disk of the file to delete</param>
        public void DeleteFile(string FilePath)
        {
            bool failed = false;
            try
            {
                fs.DeleteFile(FilePath);
            }
            catch (Exception ex)
            {
                failed = true;
            }
            FilesystemOperationCompleteEventArgs filesystemOperationCompleteEventArgs = new FilesystemOperationCompleteEventArgs(FilesystemOperation.DeleteFile, FilePath, failed);
            OnFilesystemOperationComplete(filesystemOperationCompleteEventArgs);
        }

        /// <summary>
        /// Create a new directory on the virtual drive
        /// </summary>
        /// <param name="DirectoryPath">The path/name of the directory to create</param>
        public void CreateDirectory(string DirectoryPath)
        {
            fs.CreateDirectory(DirectoryPath);
            FilesystemOperationCompleteEventArgs filesystemOperationCompleteEventArgs = new FilesystemOperationCompleteEventArgs(FilesystemOperation.CreateDirectory, DirectoryPath, false);
            OnFilesystemOperationComplete(filesystemOperationCompleteEventArgs);
        }

        /// <summary>
        /// Delete a directory on the virtual drive. Fails if Recursive is false
        /// </summary>
        /// <param name="DirectoryPath">The path/name of the directory to delete</param>
        /// <param name="RecursiveMode">If true, delete directory, as well as its children.</param>
        public void DeleteDirectory(string DirectoryPath, bool RecursiveMode)
        {
            fs.DeleteDirectory(DirectoryPath, RecursiveMode);
            FilesystemOperationCompleteEventArgs filesystemOperationCompleteEventArgs = new FilesystemOperationCompleteEventArgs(FilesystemOperation.DeleteDirectory, DirectoryPath, false);
            OnFilesystemOperationComplete(filesystemOperationCompleteEventArgs);
        }

        /// <summary>
        /// Deletes a directory on the virtual drive. Fails if directory is not empty.
        /// </summary>
        /// <param name="DirectoryPath">The path/name of the directory to delete</param>
        public void DeleteDirectory(string DirectoryPath)
        {
            try
            {
                fs.DeleteDirectory(DirectoryPath, false);
                FilesystemOperationCompleteEventArgs filesystemOperationCompleteEventArgs = new FilesystemOperationCompleteEventArgs(FilesystemOperation.DeleteDirectory, DirectoryPath, false);
                OnFilesystemOperationComplete(filesystemOperationCompleteEventArgs);
            }
            catch (Exception ex)
            {
                throw new DirectoryNotEmptyException(DirectoryPath);
            }
        }

        /// <summary>
        /// Close the virtual image
        /// </summary>
        public void Close()
        {
            fs.Dispose();
            vhdStream.Flush();
            vhdStream.Close();
        }

        private long megabytesToBytes(long megabytes)
        {
            return megabytes * BytesInMB;
        }

        private long bytesToMegabytes(long bytes)
        {
            return bytes / BytesInMB;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
