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

namespace WsaGappsTool
{
    public partial class PrepareMsixAndGapps : Form
    {
        HttpWebRequest webRequest;

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
            // Check if msixPath is empty
            if (msixPath == null || msixPath == "")
            {
                downloadMsix = true;
            }

            if (gappsPath == null || gappsPath == "")
            {
                downloadGapps = true;
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

        }

        void DownloadGapps()
        {
            label_processStatus.Text = "Downloading gapps package...";
            // Download latest Android 11 arm64 gapps
            string gappsUri = "https://api.opengapps.org/list";
            string androidVersion = "11.0";
            string arch = "x86_64";
            
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
            JsonElement gappsElement = gappsJsonElement.GetProperty("archs").GetProperty(arch).GetProperty("apis").GetProperty(androidVersion).GetProperty("variants");
            JsonElement gappsEntry = gappsElement.EnumerateArray().First();
            string gappsUrl = gappsEntry.GetProperty("url").GetString();
            Debug.WriteLine(gappsUrl);
        }
    }
}
