namespace WsaGappsTool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_msixPackagePath = new System.Windows.Forms.TextBox();
            this.button_browseForMsixPackage = new System.Windows.Forms.Button();
            this.button_downloadLatestMsixPackage = new System.Windows.Forms.Button();
            this.button_downloadLatestGappsPackage = new System.Windows.Forms.Button();
            this.button_browseForGappsPackage = new System.Windows.Forms.Button();
            this.textBox_gappsPackagePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.targetArchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arm64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewProjectOnGitHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gappsPackage_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.msixPackage_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.summaryLabel = new System.Windows.Forms.Label();
            this.msix_fileErrorLabel = new System.Windows.Forms.Label();
            this.gapps_fileErrorLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_outputDirectory = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox_openDirectoryWhenComplete = new System.Windows.Forms.CheckBox();
            this.checkBox_installPackage = new System.Windows.Forms.CheckBox();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "MSIX Package:";
            // 
            // textBox_msixPackagePath
            // 
            this.textBox_msixPackagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_msixPackagePath.Location = new System.Drawing.Point(105, 76);
            this.textBox_msixPackagePath.Name = "textBox_msixPackagePath";
            this.textBox_msixPackagePath.Size = new System.Drawing.Size(419, 20);
            this.textBox_msixPackagePath.TabIndex = 1;
            this.textBox_msixPackagePath.TextChanged += new System.EventHandler(this.textBox_msixPackagePath_TextChanged);
            // 
            // button_browseForMsixPackage
            // 
            this.button_browseForMsixPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_browseForMsixPackage.Location = new System.Drawing.Point(530, 74);
            this.button_browseForMsixPackage.Name = "button_browseForMsixPackage";
            this.button_browseForMsixPackage.Size = new System.Drawing.Size(95, 23);
            this.button_browseForMsixPackage.TabIndex = 2;
            this.button_browseForMsixPackage.Text = "Browse";
            this.button_browseForMsixPackage.UseVisualStyleBackColor = true;
            this.button_browseForMsixPackage.Click += new System.EventHandler(this.button_browseForMsixPackage_Click);
            // 
            // button_downloadLatestMsixPackage
            // 
            this.button_downloadLatestMsixPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_downloadLatestMsixPackage.Location = new System.Drawing.Point(530, 104);
            this.button_downloadLatestMsixPackage.Name = "button_downloadLatestMsixPackage";
            this.button_downloadLatestMsixPackage.Size = new System.Drawing.Size(95, 23);
            this.button_downloadLatestMsixPackage.TabIndex = 3;
            this.button_downloadLatestMsixPackage.Text = "Download latest";
            this.button_downloadLatestMsixPackage.UseVisualStyleBackColor = true;
            this.button_downloadLatestMsixPackage.Visible = false;
            // 
            // button_downloadLatestGappsPackage
            // 
            this.button_downloadLatestGappsPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_downloadLatestGappsPackage.Location = new System.Drawing.Point(530, 163);
            this.button_downloadLatestGappsPackage.Name = "button_downloadLatestGappsPackage";
            this.button_downloadLatestGappsPackage.Size = new System.Drawing.Size(95, 23);
            this.button_downloadLatestGappsPackage.TabIndex = 7;
            this.button_downloadLatestGappsPackage.Text = "Download latest";
            this.button_downloadLatestGappsPackage.UseVisualStyleBackColor = true;
            this.button_downloadLatestGappsPackage.Visible = false;
            // 
            // button_browseForGappsPackage
            // 
            this.button_browseForGappsPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_browseForGappsPackage.Location = new System.Drawing.Point(530, 133);
            this.button_browseForGappsPackage.Name = "button_browseForGappsPackage";
            this.button_browseForGappsPackage.Size = new System.Drawing.Size(95, 23);
            this.button_browseForGappsPackage.TabIndex = 6;
            this.button_browseForGappsPackage.Text = "Browse";
            this.button_browseForGappsPackage.UseVisualStyleBackColor = true;
            this.button_browseForGappsPackage.Click += new System.EventHandler(this.button_browseForGappsPackage_Click);
            // 
            // textBox_gappsPackagePath
            // 
            this.textBox_gappsPackagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_gappsPackagePath.Location = new System.Drawing.Point(105, 135);
            this.textBox_gappsPackagePath.Name = "textBox_gappsPackagePath";
            this.textBox_gappsPackagePath.Size = new System.Drawing.Size(419, 20);
            this.textBox_gappsPackagePath.TabIndex = 5;
            this.textBox_gappsPackagePath.TextChanged += new System.EventHandler(this.textBox_gappsPackagePath_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Gapps Package:";
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_start.Location = new System.Drawing.Point(550, 276);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 8;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.startButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "Wsa Gapps Tool";
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(637, 24);
            this.menuStrip.TabIndex = 10;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.targetArchToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // targetArchToolStripMenuItem
            // 
            this.targetArchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x64ToolStripMenuItem,
            this.arm64ToolStripMenuItem});
            this.targetArchToolStripMenuItem.Name = "targetArchToolStripMenuItem";
            this.targetArchToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.targetArchToolStripMenuItem.Text = "Target Architecture";
            this.targetArchToolStripMenuItem.Visible = false;
            // 
            // x64ToolStripMenuItem
            // 
            this.x64ToolStripMenuItem.Checked = true;
            this.x64ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x64ToolStripMenuItem.Name = "x64ToolStripMenuItem";
            this.x64ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.x64ToolStripMenuItem.Text = "x64";
            // 
            // arm64ToolStripMenuItem
            // 
            this.arm64ToolStripMenuItem.Name = "arm64ToolStripMenuItem";
            this.arm64ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.arm64ToolStripMenuItem.Text = "arm64";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            this.toolStripSeparator2.Visible = false;
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.toolStripSeparator1,
            this.viewProjectOnGitHubToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
            // 
            // viewProjectOnGitHubToolStripMenuItem
            // 
            this.viewProjectOnGitHubToolStripMenuItem.Name = "viewProjectOnGitHubToolStripMenuItem";
            this.viewProjectOnGitHubToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.viewProjectOnGitHubToolStripMenuItem.Text = "View Project on GitHub";
            this.viewProjectOnGitHubToolStripMenuItem.Click += new System.EventHandler(this.viewProjectOnGitHubToolStripMenuItem_Click);
            // 
            // gappsPackage_openFileDialog
            // 
            this.gappsPackage_openFileDialog.Filter = "Gapps Zip File|*.zip";
            this.gappsPackage_openFileDialog.Title = "Select Gapps Package";
            // 
            // msixPackage_openFileDialog
            // 
            this.msixPackage_openFileDialog.Filter = "MSIX Application Package|*.msix";
            this.msixPackage_openFileDialog.Title = "Select MSIX Package";
            // 
            // summaryLabel
            // 
            this.summaryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.summaryLabel.Location = new System.Drawing.Point(9, 280);
            this.summaryLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.summaryLabel.Name = "summaryLabel";
            this.summaryLabel.Size = new System.Drawing.Size(536, 19);
            this.summaryLabel.TabIndex = 11;
            this.summaryLabel.Text = "MSIX and Gapps will be downloaded";
            this.summaryLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // msix_fileErrorLabel
            // 
            this.msix_fileErrorLabel.AutoSize = true;
            this.msix_fileErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.msix_fileErrorLabel.Location = new System.Drawing.Point(102, 99);
            this.msix_fileErrorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.msix_fileErrorLabel.Name = "msix_fileErrorLabel";
            this.msix_fileErrorLabel.Size = new System.Drawing.Size(91, 13);
            this.msix_fileErrorLabel.TabIndex = 12;
            this.msix_fileErrorLabel.Text = "File does not exist";
            this.msix_fileErrorLabel.Visible = false;
            // 
            // gapps_fileErrorLabel
            // 
            this.gapps_fileErrorLabel.AutoSize = true;
            this.gapps_fileErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.gapps_fileErrorLabel.Location = new System.Drawing.Point(102, 158);
            this.gapps_fileErrorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.gapps_fileErrorLabel.Name = "gapps_fileErrorLabel";
            this.gapps_fileErrorLabel.Size = new System.Drawing.Size(91, 13);
            this.gapps_fileErrorLabel.TabIndex = 13;
            this.gapps_fileErrorLabel.Text = "File does not exist";
            this.gapps_fileErrorLabel.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Output Directory:";
            // 
            // textBox_outputDirectory
            // 
            this.textBox_outputDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_outputDirectory.Location = new System.Drawing.Point(105, 200);
            this.textBox_outputDirectory.Name = "textBox_outputDirectory";
            this.textBox_outputDirectory.Size = new System.Drawing.Size(419, 20);
            this.textBox_outputDirectory.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(530, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // checkBox_openDirectoryWhenComplete
            // 
            this.checkBox_openDirectoryWhenComplete.AutoSize = true;
            this.checkBox_openDirectoryWhenComplete.Location = new System.Drawing.Point(105, 227);
            this.checkBox_openDirectoryWhenComplete.Name = "checkBox_openDirectoryWhenComplete";
            this.checkBox_openDirectoryWhenComplete.Size = new System.Drawing.Size(170, 17);
            this.checkBox_openDirectoryWhenComplete.TabIndex = 17;
            this.checkBox_openDirectoryWhenComplete.Text = "Open directory when complete";
            this.checkBox_openDirectoryWhenComplete.UseVisualStyleBackColor = true;
            // 
            // checkBox_installPackage
            // 
            this.checkBox_installPackage.AutoSize = true;
            this.checkBox_installPackage.Location = new System.Drawing.Point(281, 227);
            this.checkBox_installPackage.Name = "checkBox_installPackage";
            this.checkBox_installPackage.Size = new System.Drawing.Size(98, 17);
            this.checkBox_installPackage.TabIndex = 18;
            this.checkBox_installPackage.Text = "Install package";
            this.checkBox_installPackage.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 311);
            this.Controls.Add(this.checkBox_installPackage);
            this.Controls.Add(this.checkBox_openDirectoryWhenComplete);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_outputDirectory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.gapps_fileErrorLabel);
            this.Controls.Add(this.msix_fileErrorLabel);
            this.Controls.Add(this.summaryLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.button_downloadLatestGappsPackage);
            this.Controls.Add(this.button_browseForGappsPackage);
            this.Controls.Add(this.textBox_gappsPackagePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_downloadLatestMsixPackage);
            this.Controls.Add(this.button_browseForMsixPackage);
            this.Controls.Add(this.textBox_msixPackagePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(653, 350);
            this.Name = "Form1";
            this.Text = "Wsa Gapps Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_msixPackagePath;
        private System.Windows.Forms.Button button_browseForMsixPackage;
        private System.Windows.Forms.Button button_downloadLatestMsixPackage;
        private System.Windows.Forms.Button button_downloadLatestGappsPackage;
        private System.Windows.Forms.Button button_browseForGappsPackage;
        private System.Windows.Forms.TextBox textBox_gappsPackagePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem viewProjectOnGitHubToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog gappsPackage_openFileDialog;
        private System.Windows.Forms.OpenFileDialog msixPackage_openFileDialog;
        private System.Windows.Forms.Label summaryLabel;
        private System.Windows.Forms.Label msix_fileErrorLabel;
        private System.Windows.Forms.Label gapps_fileErrorLabel;
        private System.Windows.Forms.ToolStripMenuItem targetArchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arm64ToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_outputDirectory;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox_openDirectoryWhenComplete;
        private System.Windows.Forms.CheckBox checkBox_installPackage;
    }
}

