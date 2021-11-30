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

namespace WsaGappsTool
{
    public partial class PrepareMsixAndGapps : Form
    {
        HttpWebRequest webRequest;
        WebClient gapps_webClient;
        WebClient msix_webClient;

        public string errorMessage = "";
        public bool error = false;

        bool downloadMsix = false;
        bool downloadGapps = false;

        string MsixPath = "";
        string GappsPath = "";

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
            }

            if (gappsPath == null || gappsPath == "")
            {
                downloadGapps = true;
                GappsPath = config.CacheDirectory + "gapps.zip";
            }

            if(!Directory.Exists(config.CacheDirectory))
            {
                Directory.CreateDirectory(config.CacheDirectory);
            }
        }

        private void label_processStatus_Click(object sender, EventArgs e)
        {

        }

        private void PrepareMsixAndGapps_Load(object sender, EventArgs e)
        {
            if (downloadGapps)
            {
                DownloadGapps();
                //try
                //{
                //    DownloadGapps();
                //}
                //catch
                //{
                //    error = true;
                //    errorMessage = "Could not download Gapps package. An unknown error occurred.";
                //    DialogResult = DialogResult.Abort;
                //    Close();
                //}
            }
            // if (downloadMsix)
            // {
            //     try
            //     {
            //         DownloadGapps();
            //     }
            //     catch
            //     {
            //         error = true;
            //         errorMessage = "Could not download Gapps package. An unknown error occurred.";
            //         DialogResult = DialogResult.Abort;
            //         Close();
            //     }
            // }
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
            // Get the latest gapps for android 11; x86-64
            JsonElement gappsElement = gappsJsonElement.GetProperty("archs").GetProperty(arch).GetProperty("apis").GetProperty(androidVersion).GetProperty("variants"); // Get the list of the latest releases for android 11; x86-64
            JsonElement gappsVariantElement = gappsElement.EnumerateArray().ElementAt(0); // Get pico archive
            string gappsUrl = gappsVariantElement.GetProperty("zip").GetString();
            string gappsMD5 = gappsVariantElement.GetProperty("md5").GetString();
            //Debug.WriteLine(gappsUrl);
            
            // Download the gapps package
            webRequest = (HttpWebRequest)WebRequest.Create(gappsUrl);
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
            gapps_webClient = new WebClient();
            gapps_webClient.DownloadProgressChanged += Gapps_webClient_DownloadProgressChanged;
            gapps_webClient.DownloadFileCompleted += Gapps_webClient_DownloadFileCompleted;
            gapps_webClient.DownloadFileAsync(webRequest.RequestUri, GappsPath);

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

        private void Gapps_webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            label_processStatus.Text = String.Format("Verifying MD5 checksum...");
            // Verify MD5 checksum
            MD5 md5_verify = MD5.Create();
            string Checksum;
            using (FileStream stream = File.OpenRead(GappsPath))
            {
                byte[] checksum = md5_verify.ComputeHash(stream);
                Checksum = BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
            Debug.WriteLine(Checksum);
        }

        private void Gapps_webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            label_processStatus.Text = String.Format("Downloading gapps package ({0} bytes / {1} bytes)...", e.BytesReceived, e.TotalBytesToReceive);
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Cancel
            if (MessageBox.Show("Are you sure you want to cancel?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                
            }
        }
    }
}
