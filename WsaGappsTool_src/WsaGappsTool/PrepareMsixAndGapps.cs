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

namespace WsaGappsTool
{
    public partial class PrepareMsixAndGapps : Form
    {
        bool downloadStatusToShow = false; // false for Gapps; true for MSIX

        HttpWebRequest webRequest;
        HttpWebRequest webRequest2;
        WebClient gapps_webClient;
        WebClient msix_webClient;

        bool verifyChecksums = false; // Set to true to perform checks of Gapps package after downlaoding

        // Variables intended for access by the parent form
        public string errorMessage = "";
        public bool error = false;

        // Determined by whether input arguments are null or not
        bool downloadMsix = false;
        bool downloadGapps = false;

        string MsixPath = "";
        string GappsPath = "";
        private bool closing = false;

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
                MsixPath = config.CacheDirectory + "wsa.msix";
            }
            else
            {
                MsixPath = msixPath;
            }

            if (gappsPath == null || gappsPath == "")
            {
                downloadGapps = true;
                GappsPath = config.CacheDirectory + "gapps.zip";
            }
            else
            {
                GappsPath = gappsPath;
            }

            if (!Directory.Exists(config.CacheDirectory))
            {
                Directory.CreateDirectory(config.CacheDirectory);
            }
        }

        private void label_processStatus_Click(object sender, EventArgs e)
        {

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
            // Download latest Android 11 arm64 gapps
            string gappsUri = "https://api.opengapps.org/list";
            string androidVersion = "11.0";
            string arch = "x86_64";
            string variant = "pico";

            string gappsJson = "";
            // Get the latest gapps
            // Download the latest gapps to gapps.zip; android 11; x86-64
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
            string arch = "x64";
            string lineToSearchFor = "<tr style=\".*?\"><td><a href=\"(?<url>.*?)\" rel=\"noreferrer\">MicrosoftCorporationII\\.WindowsSubsystemForAndroid_[^<]+\\.msixbundle</a></td>.+</tr>";

            Hashtable parameters = new Hashtable()
            {
                {"type", "ProductId"},
                {"url", "9P3395VX91NR"},
                {"ring", "WIS"},
                {"lang", "en_US"}
            };

            webRequest2 = (HttpWebRequest)WebRequest.Create(msixUri);
            webRequest2.Method = "POST";
            webRequest2.ContentType = "application/x-www-form-urlencoded";

            // Set body
            using (StreamWriter streamWriter = new StreamWriter(webRequest2.GetRequestStream()))
            {
                foreach (DictionaryEntry entry in parameters)
                {
                    streamWriter.Write(entry.Key + "=" + entry.Value + "&");
                }
                //streamWriter.Write(String.Join("&", parameters.OfType<DictionaryEntry>().Select(de => String.Format("{0}={1}", de.Key, de.Value))));
                streamWriter.Flush();
                streamWriter.Close();
            }
            HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest2.GetResponse();
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
                }
            }
            else
            {
                label_processStatus.Text = String.Format("Downloading MSIX package ({0} bytes / {1} bytes)...", e.BytesReceived, e.TotalBytesToReceive);
                progressBar1.Value = e.ProgressPercentage;
            }
        }

        private void Gapps_webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (gapps_webClient.IsBusy && msix_webClient.IsBusy)
            {
                if (downloadStatusToShow == false)
                {
                    label_processStatus.Text = String.Format("Downloading gapps package ({0} bytes / {1} bytes)...", e.BytesReceived, e.TotalBytesToReceive);
                    progressBar1.Value = e.ProgressPercentage;
                }
            }
            else
            {
                label_processStatus.Text = String.Format("Downloading gapps package ({0} bytes / {1} bytes)...", e.BytesReceived, e.TotalBytesToReceive);
                progressBar1.Value = e.ProgressPercentage;
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            // Cancel
            if (MessageBox.Show("Are you sure you want to cancel?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
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
            if (fileChecksumString != CalculatedChecksum) return false; // File's checksum did not match expected
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
                // Extract msix to temp folder in cache
                label_processStatus.Text = "Extracting MSIX package...";
                progressBar1.Style = ProgressBarStyle.Marquee;

                // // Use 7zip to extract, and get progress from stdout
                // ProcessStartInfo startInfo = new ProcessStartInfo();
                // startInfo.FileName = config.sevenZip_Ex;
                // startInfo.Arguments = "x -y -o" + config.CacheDirectory + "msix/" + " \"" + Path.GetFullPath(MsixPath) + "\"";
                // startInfo.UseShellExecute = true;
                // //startInfo.RedirectStandardOutput = true;
                // //startInfo.RedirectStandardError = true;
                // startInfo.CreateNoWindow = false;
                // Process extractor = new Process();
                // extractor.StartInfo = startInfo;
                // extractor.Start();
                // extractor.WaitForExit();

                // // Create listener to continuously read stdout, and parse percentage
                // string line;
                // while ((line = extractor.StandardOutput.ReadLine()) != null)
                // {
                //     if (line.Contains("%"))
                //     {
                //         int percentage = int.Parse(line.Substring(0, line.IndexOf("%")));
                //         progressBar1.Value = percentage;
                //     }
                // }

                // Extract using SharpZipLib
                string msixExtractPath = config.CacheDirectory + "msix/";
                ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();
                if (!Directory.Exists(msixExtractPath))
                {
                    Directory.CreateDirectory(msixExtractPath);
                }
                fastZip.ExtractZip(MsixPath, msixExtractPath, @"\bx64\b");

                // Look for package that contains x64 in title using match regex
                string[] files = Directory.GetFiles(msixExtractPath, "*.msix");
                string x64_package = "";
                foreach (string file in files)
                {
                    if (Regex.IsMatch(Path.GetFileName(file), @"\bx64\b", RegexOptions.IgnoreCase))
                    {
                        x64_package = file;
                        break;
                    }
                }
                Debug.WriteLine("x64 package: " + x64_package);

                label_processStatus.Text = String.Format("Checking for existing data image...");
                if (File.Exists(config.vm_dataDiskImage))
                {
                    label_processStatus.Text = String.Format("Deleting...");
                    File.Delete(config.vm_dataDiskImage);
                }
                // label_processStatus.Text = String.Format("Creating new data image...");
                // Process.Start(config.sevenZip_Ex, "x ..\\vm\\data\\data.7z.001 -o\"..\vm\\\"")
                label_processStatus.Text = String.Format("Creating data image with contents...");

            }
        }
    }
}