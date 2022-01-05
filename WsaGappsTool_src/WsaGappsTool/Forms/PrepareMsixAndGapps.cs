using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using WsaGappsTool;
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using WsaGappsTool.VhdxHelper;
using WsaGappsTool.Resources;
//using Microsoft.WindowsAPICodePack.Shell;

namespace WsaGappsTool
{
    public partial class PrepareMsixAndGapps : Form
    {
        bool downloadStatusToShow = false; // false for Gapps; true for MSIX

        HttpWebRequest webRequest;
        HttpWebRequest msix_httpWebRequest;
        WebClient gapps_webClient;
        WebClient msix_webClient;

        bool verifyChecksums = false; // Set to true to perform checks of Gapps package after downlaoding

        // Variables intended for access by the parent form
        public string errorMessage = "";
        public bool error = false;

        // Determined by whether input arguments are null or not
        bool downloadMsix = false;
        bool downloadGapps = false;

        string defaultMsixFileName = "wsa.msix";
        string defaultGappsFileName = "pico.zip";

        string MsixPath = "";
        string GappsPath = "";
        private bool closing = false;

        int msixDownloadProgress = 0;
        int gappsDownloadProgress = 0;

        public PrepareMsixAndGapps()
        {
            InitializeComponent();
        }

        public PrepareMsixAndGapps(string msixPath, string gappsPath)
        {
            InitializeComponent();
            // Check if msixPath is empty
            if (msixPath == null || msixPath == "")
            {
                downloadMsix = true;
                MsixPath = config.CacheDirectory + defaultMsixFileName;
            }
            else
            {
                MsixPath = msixPath;
            }

            if (gappsPath == null || gappsPath == "")
            {
                downloadGapps = true;
                GappsPath = config.CacheDirectory + defaultGappsFileName;
            }
            else
            {
                GappsPath = gappsPath;
            }

            if (Directory.Exists(config.CacheDirectory))
            {
                Directory.Delete(config.CacheDirectory, true);
            }
            Directory.CreateDirectory(config.CacheDirectory);
        }

        void setStatusText(string text)
        {
            this.Invoke((MethodInvoker)delegate
            {
                label_processStatus.Text = text;
            });
        }

        private void PrepareMsixAndGapps_Load(object sender, EventArgs e)
        {
            msix_webClient = new WebClient();
            gapps_webClient = new WebClient();
            if (downloadGapps || downloadMsix)
            {
                if (downloadGapps)
                {
                    try
                    {
                        DownloadGapps();
                    }
                    catch
                    {
                        CloseWithError("Could not download Gapps package. An unknown error occurred.");
                    }
                }
                if (downloadMsix)
                {
                    try
                    {
                        DownloadMsix();
                    }
                    catch
                    {
                        CloseWithError("Could not download MSIX package. An unknown error occurred.");
                    }
                }
            }
            else
            {
                PrepareFiles();
            }
        }

        void DownloadGapps()
        {
            label_processStatus.Text = "Preparing to download gapps package...";
            string gappsUri = Resources.Resources.Gapps_ApiURL;
            string androidVersion = Resources.Resources.Gapps_TargetAndroidVersionString;
            string arch = Resources.Resources.Gapps_TargetArch;
            //string variant = "pico";

            string gappsJson = "";
            // Get the latest gapps
            // Download the latest gapps for android 11; x86-64
            webRequest = (HttpWebRequest)WebRequest.Create(gappsUri);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
            webRequest.Accept = "application/json";

            //Get json response
            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    gappsJson = reader.ReadToEnd();
                }
            }

            // Parse the json with JsonDocument
            JsonDocument gappsJsonDoc = JsonDocument.Parse(gappsJson);
            JsonElement gappsJsonElement = gappsJsonDoc.RootElement;
            JsonElement gappsElement = gappsJsonElement.GetProperty("archs").GetProperty(arch).GetProperty("apis").GetProperty(androidVersion).GetProperty("variants"); // Get the list of the latest releases for android 11; x86-64
            JsonElement gappsVariantElement = gappsElement.EnumerateArray().ElementAt(0); // Get pico archive (first entry should always be pico)
            string gappsDownloadUrl = gappsVariantElement.GetProperty("zip").GetString(); // Download URL for zip
            string gappsMD5 = gappsVariantElement.GetProperty("md5").GetString(); // Download URL for MD5 hash

            // Download the gapps package
            webRequest = (HttpWebRequest)WebRequest.Create(gappsDownloadUrl);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
            webRequest.Accept = "application/json";
            // Trick sourceforge into thinking we're downloading from the site
            webRequest.Referer = "https://sourceforge.net/projects/opengapps/files/";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            webRequest.Headers.Add("Upgrade-Insecure-Requests", "1");

            // Start downloading
            gapps_webClient.DownloadProgressChanged += Gapps_webClient_DownloadProgressChanged;
            gapps_webClient.DownloadFileCompleted += Gapps_webClient_DownloadFileCompleted;
            gapps_webClient.DownloadFileAsync(webRequest.RequestUri, GappsPath);

            if (verifyChecksums)
            {
                // Download MD5 hash
                HttpWebRequest webRequestMD5 = (HttpWebRequest)WebRequest.Create(gappsMD5);
                webRequestMD5.Method = "GET";
                webRequestMD5.ContentType = "application/json";
                webRequestMD5.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
                webRequestMD5.Accept = "application/json";
                // Trick sourceforge into thinking we're downloading from the site
                webRequestMD5.Referer = "https://sourceforge.net/projects/opengapps/files/";
                webRequestMD5.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                webRequestMD5.Headers.Add("Accept-Language", "en-US,en;q=0.9");
                webRequestMD5.Headers.Add("Upgrade-Insecure-Requests", "1");
                using (WebClient md5download = new WebClient())
                {
                    md5download.DownloadFileAsync(webRequestMD5.RequestUri, GappsPath + ".md5");
                }
            }
        }

        void DownloadMsix()
        {
            label_processStatus.Text = "Preparing to download MSIX package...";
            string msixUri = "https://store.rg-adguard.net/api/GetFiles";
            //string arch = "x64";
            string lineToSearchFor = "<tr style=\".*?\"><td><a href=\"(?<url>.*?)\" rel=\"noreferrer\">MicrosoftCorporationII\\.WindowsSubsystemForAndroid_[^<]+\\.msixbundle</a></td>.+</tr>";

            Hashtable parameters = new Hashtable()
            {
                {"type", "ProductId"},
                {"url", "9P3395VX91NR"},
                {"ring", "WIS"},
                {"lang", "en_US"}
            };

            msix_httpWebRequest = (HttpWebRequest)WebRequest.Create(msixUri);
            msix_httpWebRequest.Method = "POST";
            msix_httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            // Set body
            using (StreamWriter streamWriter = new StreamWriter(msix_httpWebRequest.GetRequestStream()))
            {
                foreach (DictionaryEntry entry in parameters)
                {
                    streamWriter.Write(entry.Key + "=" + entry.Value + "&");
                }
                //streamWriter.Write(String.Join("&", parameters.OfType<DictionaryEntry>().Select(de => String.Format("{0}={1}", de.Key, de.Value))));
                streamWriter.Flush();
                streamWriter.Close();
            }
            HttpWebResponse httpWebResponse = (HttpWebResponse)msix_httpWebRequest.GetResponse();
            string responseData = new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd();
            Debug.WriteLine(String.Format("Data: {0}", responseData));
            Debug.WriteLine(String.Format("Type: {0}", httpWebResponse.ContentType));

            // Filter out the msixbundle link
            Match match = Regex.Match(responseData, lineToSearchFor);
            string msixUrl = match.Groups["url"].Value;
            Debug.WriteLine(String.Format("MSIX URL: {0}", msixUrl));

            // Download .msix
            msix_webClient.DownloadFileCompleted += Msix_webClient_DownloadFileCompleted;
            msix_webClient.DownloadProgressChanged += Msix_webClient_DownloadProgressChanged;
            msix_webClient.DownloadFileAsync(new Uri(msixUrl), MsixPath);
        }

        private void Msix_webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (gapps_webClient.IsBusy && msix_webClient.IsBusy)
            {
                if (downloadStatusToShow == true)
                {
                    label_processStatus.Text = String.Format("Downloading MSIX package ({0} bytes / {1} bytes)...", e.BytesReceived, e.TotalBytesToReceive);
                    progressBar1.Value = e.ProgressPercentage;
                    //SetProgress(e.ProgressPercentage, false);
                }
            }
            else
            {
                label_processStatus.Text = String.Format("Downloading MSIX package ({0} bytes / {1} bytes)...", e.BytesReceived, e.TotalBytesToReceive);
                progressBar1.Value = e.ProgressPercentage;
                //SetProgress(e.ProgressPercentage, false);
            }
            msixDownloadProgress = e.ProgressPercentage;
            //Taskbar_UpdateDownloadProgress();
        }

        private void Gapps_webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (gapps_webClient.IsBusy && msix_webClient.IsBusy)
            {
                if (downloadStatusToShow == false)
                {
                    label_processStatus.Text = String.Format("Downloading gapps package ({0} bytes / {1} bytes)...", e.BytesReceived, e.TotalBytesToReceive);
                    progressBar1.Value = e.ProgressPercentage;
                    //SetProgress(e.ProgressPercentage, false);
                }
            }
            else
            {
                label_processStatus.Text = String.Format("Downloading gapps package ({0} bytes / {1} bytes)...", e.BytesReceived, e.TotalBytesToReceive);
                progressBar1.Value = e.ProgressPercentage;
                //SetProgress(e.ProgressPercentage, false);
            }
            gappsDownloadProgress = e.ProgressPercentage;
            //Taskbar_UpdateDownloadProgress();
        }

        private void Msix_webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null || closing == true)
            {

            }
            else
            {
                PrepareFiles();
            }
        }

        private void Gapps_webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null || closing == true)
            {

            }
            else
            {
                if (verifyChecksums)
                {
                    label_processStatus.Text = String.Format("Verifying MD5 checksum for gapps.zip...");
                    if (!VerifyMD5(GappsPath))
                    {
                        CloseWithError("Error preparing files. The MD5 checksum for the Gapps package did not match the one retrieved from the server.");
                    }
                }
                PrepareFiles();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            // Cancel
            if (MessageBox.Show("Are you sure you want to cancel?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                backgroundWorker_PrepareFiles.CancelAsync();
                CloseWithError("Process cancelled.");
            }
        }

        bool VerifyMD5(string file, string md5_file)
        {
            int md5_length = 32;

            // Verify MD5 checksum
            MD5 md5_verify = MD5.Create();
            string CalculatedChecksum;
            using (FileStream stream = File.OpenRead(GappsPath))
            {
                byte[] checksum = md5_verify.ComputeHash(stream);
                CalculatedChecksum = BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
            CalculatedChecksum = CalculatedChecksum.ToUpper(); // Make it upper case
            // Debug.WriteLine(CalculatedChecksum);

            // Read checksum from MD5 file
            string fileChecksumString = "";
            using (StreamReader reader = new StreamReader(md5_file))
            {
                fileChecksumString = reader.ReadToEnd().Substring(0, md5_length).ToUpper();
            }
            //Debug.WriteLine(fileChecksumString);

            // fileChecksumString = fileChecksumString.Substring(0, md5_length).ToUpper();

            // Determine the verdict...
            if (fileChecksumString != CalculatedChecksum) return false; // File's checksum did not match expected
            else return true; // All good!
        }

        bool VerifyMD5(string file)
        {
            int md5_length = 32;
            string md5_file = file + ".md5";

            // Verify MD5 checksum
            MD5 md5_verify = MD5.Create();
            string CalculatedChecksum;
            using (FileStream stream = File.OpenRead(GappsPath))
            {
                byte[] checksum = md5_verify.ComputeHash(stream);
                CalculatedChecksum = BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
            CalculatedChecksum = CalculatedChecksum.ToUpper(); // Make it upper case
            // Debug.WriteLine(CalculatedChecksum);

            // Read checksum from MD5 file
            string fileChecksumString = "";
            using (StreamReader reader = new StreamReader(md5_file))
            {
                fileChecksumString = reader.ReadToEnd().Substring(0, md5_length).ToUpper();
            }
            //Debug.WriteLine(fileChecksumString);

            // fileChecksumString = fileChecksumString.Substring(0, md5_length).ToUpper();

            // Determine the verdict...
            if (fileChecksumString != CalculatedChecksum) return false; // The generated checksum did not match the one retrieved from the server
            else return true; // All good!
        }

        void CloseWithError(string message)
        {
            closing = true;
            if (gapps_webClient != null)
            {
                gapps_webClient.CancelAsync();
                gapps_webClient.Dispose();
            }
            if (msix_webClient != null)
            {
                msix_webClient.CancelAsync();
                msix_webClient.Dispose();
            }
            error = true;
            errorMessage = message;
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            downloadStatusToShow = !downloadStatusToShow;
        }

        void PrepareFiles()
        {
            if (!(gapps_webClient.IsBusy || msix_webClient.IsBusy))
            {
                timer1.Stop();
                timer1.Enabled = false;
                backgroundWorker_PrepareFiles.RunWorkerAsync();
            }
        }

        private void backgroundWorker_PrepareFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            setStatusText("Extracting MSIX package...");
            this.Invoke((MethodInvoker)delegate
            {
                progressBar1.Style = ProgressBarStyle.Marquee;
            });

            // Extract MSIX archive
            Regex x64_regex = new Regex(@"_x64_");
            string msixExtractPath = config.CacheDirectory + "msix/";
            ICSharpCode.SharpZipLib.Zip.FastZip unzipper = new ICSharpCode.SharpZipLib.Zip.FastZip();
            if (!Directory.Exists(msixExtractPath))
            {
                Directory.CreateDirectory(msixExtractPath);
            }
            unzipper.ExtractZip(MsixPath, msixExtractPath, x64_regex.ToString());

            // Look for package that contains x64 in title using match regex
            string[] files = Directory.GetFiles(msixExtractPath, "*.msix");
            string x64_package = "";
            foreach (string file in files)
            {
                if (x64_regex.IsMatch(Path.GetFileName(file)))
                {
                    x64_package = file;
                    break;
                }
            }

            //Debug.WriteLine("Package: " + x64_package);
            unzipper.ExtractZip(x64_package, msixExtractPath, null);

            // Delete MSIX
            File.Delete(x64_package);

            List<string> FilesToDelete = new List<string>
                {
                    "AppxBlockMap.xml",
                    "AppxSignature.p7x",
                    "[Content_Types].xml"
                };

            List<string> FoldersToDelete = new List<string>
                {
                    "AppxMetadata"
                };

            // Delete files
            foreach (string file in FilesToDelete)
                if (File.Exists(msixExtractPath + file))
                    File.Delete(msixExtractPath + file);

            // Delete folders (recursively)
            foreach (string folder in FoldersToDelete)
                if (Directory.Exists(msixExtractPath + folder))
                    Directory.Delete(msixExtractPath + folder, true);

            // Check if data.vhdx already exists, and delete it if it does
            setStatusText("Checking for existing data image...");
            if (File.Exists(config.vm_dataDiskImage))
            {
                setStatusText("Deleting existing image...");
                File.Delete(config.vm_dataDiskImage);
            }

            setStatusText("Creating new data image...");
            string vhdx_temp = Resources.Paths.VHDX_Contents_TempDir;
            string vhdx_temp_gapps = Resources.Paths.VHDX_Contents_TempDir_Gapps;
            string vhdx_temp_images = Resources.Paths.VHDX_Contents_TempDir_Images;
            Directory.CreateDirectory(vhdx_temp);
            Directory.CreateDirectory(vhdx_temp_gapps);
            Directory.CreateDirectory(vhdx_temp_images);

            // Move all .img files to vhdx_temp_images
            foreach (string file in Directory.GetFiles(msixExtractPath, "*.img"))
                File.Move(file, vhdx_temp_images + Path.GetFileName(file));

            // Copy gapps zip to vhdx_temp_gapps
            // File.Copy(GappsPath, vhdx_temp_gapps + Path.GetFileName(GappsPath));
            File.Copy(GappsPath, vhdx_temp_gapps + "pico.zip");

            setStatusText("Copying files...");
            VhdxBuilder.CreateFromDirectory(vhdx_temp, config.vm_dataDiskImage, "WSA_Data", config.DefaultDataDiskImageSizeMB).Close(); // Create a virtual VHDX image, and copy the contents of the temporary folder to it

            setStatusText("Image created");
            Thread.Sleep(2000);

            setStatusText("Finishing up...");
            this.Invoke((MethodInvoker)delegate
            {
                cancelButton.Enabled = false;
            });
            Thread.Sleep(2000); // Sleep for 2 seconds to allow things to catch up
            // Clean up
            Directory.Delete(vhdx_temp, true);
            //Directory.Delete(config.CacheDirectory, true);
        }

        private void backgroundWorker_PrepareFiles_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            setStatusText("Preparation complete. Getting ready to start VM...");
            this.Invoke((MethodInvoker)delegate
            {
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Value = 100;
                Close();
            });
        }

        private void PrepareMsixAndGapps_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        // void SetProgress(int value, bool failed)
        // {
        //     var taskbar = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
        //     taskbar.SetProgressValue(value, 100);
        //     if (!failed)
        //     {
        //         taskbar.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Normal);
        //     }
        //     else
        //     {
        //         taskbar.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Error);
        //     }
        // }
        // 
        // void Taskbar_UpdateDownloadProgress()
        // {
        //     var taskbar = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
        //     taskbar.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Normal);
        //     try
        //     {
        //         if (downloadMsix && downloadGapps)
        //         {
        //             taskbar.SetProgressValue(gappsDownloadProgress + msixDownloadProgress, 200);
        //         }
        //         else
        //         {
        //             if (downloadMsix)
        //             {
        //                 taskbar.SetProgressValue(msixDownloadProgress, 100);
        //             }
        //             else
        //             {
        //                 taskbar.SetProgressValue(gappsDownloadProgress, 100);
        //             }
        //         }
        //     }
        //     catch
        //     {
        // 
        //     }
        // }
    }
}