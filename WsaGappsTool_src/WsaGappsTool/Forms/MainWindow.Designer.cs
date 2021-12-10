﻿namespace WsaGappsTool
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
            this.automaticInstallationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 97);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "MSIX Package:";
            // 
            // textBox_msixPackagePath
            // 
            this.textBox_msixPackagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_msixPackagePath.Location = new System.Drawing.Point(137, 94);
            this.textBox_msixPackagePath.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_msixPackagePath.Name = "textBox_msixPackagePath";
            this.textBox_msixPackagePath.Size = new System.Drawing.Size(560, 22);
            this.textBox_msixPackagePath.TabIndex = 1;
            this.textBox_msixPackagePath.TextChanged += new System.EventHandler(this.textBox_msixPackagePath_TextChanged);
            // 
            // button_browseForMsixPackage
            // 
            this.button_browseForMsixPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_browseForMsixPackage.Location = new System.Drawing.Point(707, 91);
            this.button_browseForMsixPackage.Margin = new System.Windows.Forms.Padding(4);
            this.button_browseForMsixPackage.Name = "button_browseForMsixPackage";
            this.button_browseForMsixPackage.Size = new System.Drawing.Size(127, 28);
            this.button_browseForMsixPackage.TabIndex = 2;
            this.button_browseForMsixPackage.Text = "Browse";
            this.button_browseForMsixPackage.UseVisualStyleBackColor = true;
            this.button_browseForMsixPackage.Click += new System.EventHandler(this.button_browseForMsixPackage_Click);
            // 
            // button_downloadLatestMsixPackage
            // 
            this.button_downloadLatestMsixPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_downloadLatestMsixPackage.Location = new System.Drawing.Point(707, 128);
            this.button_downloadLatestMsixPackage.Margin = new System.Windows.Forms.Padding(4);
            this.button_downloadLatestMsixPackage.Name = "button_downloadLatestMsixPackage";
            this.button_downloadLatestMsixPackage.Size = new System.Drawing.Size(127, 28);
            this.button_downloadLatestMsixPackage.TabIndex = 3;
            this.button_downloadLatestMsixPackage.Text = "Download latest";
            this.button_downloadLatestMsixPackage.UseVisualStyleBackColor = true;
            this.button_downloadLatestMsixPackage.Visible = false;
            // 
            // button_downloadLatestGappsPackage
            // 
            this.button_downloadLatestGappsPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_downloadLatestGappsPackage.Location = new System.Drawing.Point(707, 231);
            this.button_downloadLatestGappsPackage.Margin = new System.Windows.Forms.Padding(4);
            this.button_downloadLatestGappsPackage.Name = "button_downloadLatestGappsPackage";
            this.button_downloadLatestGappsPackage.Size = new System.Drawing.Size(127, 28);
            this.button_downloadLatestGappsPackage.TabIndex = 7;
            this.button_downloadLatestGappsPackage.Text = "Download latest";
            this.button_downloadLatestGappsPackage.UseVisualStyleBackColor = true;
            this.button_downloadLatestGappsPackage.Visible = false;
            // 
            // button_browseForGappsPackage
            // 
            this.button_browseForGappsPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_browseForGappsPackage.Location = new System.Drawing.Point(707, 194);
            this.button_browseForGappsPackage.Margin = new System.Windows.Forms.Padding(4);
            this.button_browseForGappsPackage.Name = "button_browseForGappsPackage";
            this.button_browseForGappsPackage.Size = new System.Drawing.Size(127, 28);
            this.button_browseForGappsPackage.TabIndex = 6;
            this.button_browseForGappsPackage.Text = "Browse";
            this.button_browseForGappsPackage.UseVisualStyleBackColor = true;
            this.button_browseForGappsPackage.Click += new System.EventHandler(this.button_browseForGappsPackage_Click);
            // 
            // textBox_gappsPackagePath
            // 
            this.textBox_gappsPackagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_gappsPackagePath.Location = new System.Drawing.Point(137, 197);
            this.textBox_gappsPackagePath.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_gappsPackagePath.Name = "textBox_gappsPackagePath";
            this.textBox_gappsPackagePath.Size = new System.Drawing.Size(560, 22);
            this.textBox_gappsPackagePath.TabIndex = 5;
            this.textBox_gappsPackagePath.TextChanged += new System.EventHandler(this.textBox_gappsPackagePath_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 201);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Gapps Package:";
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_start.Location = new System.Drawing.Point(733, 314);
            this.button_start.Margin = new System.Windows.Forms.Padding(4);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(100, 28);
            this.button_start.TabIndex = 8;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.startButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 43);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 24);
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
            this.menuStrip.Size = new System.Drawing.Size(849, 28);
            this.menuStrip.TabIndex = 10;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.targetArchToolStripMenuItem,
            this.automaticInstallationToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // targetArchToolStripMenuItem
            // 
            this.targetArchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x64ToolStripMenuItem,
            this.arm64ToolStripMenuItem});
            this.targetArchToolStripMenuItem.Name = "targetArchToolStripMenuItem";
            this.targetArchToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.targetArchToolStripMenuItem.Text = "Target Architecture";
            // 
            // x64ToolStripMenuItem
            // 
            this.x64ToolStripMenuItem.Checked = true;
            this.x64ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x64ToolStripMenuItem.Name = "x64ToolStripMenuItem";
            this.x64ToolStripMenuItem.Size = new System.Drawing.Size(134, 26);
            this.x64ToolStripMenuItem.Text = "x64";
            // 
            // arm64ToolStripMenuItem
            // 
            this.arm64ToolStripMenuItem.Name = "arm64ToolStripMenuItem";
            this.arm64ToolStripMenuItem.Size = new System.Drawing.Size(134, 26);
            this.arm64ToolStripMenuItem.Text = "arm64";
            // 
            // automaticInstallationToolStripMenuItem
            // 
            this.automaticInstallationToolStripMenuItem.Name = "automaticInstallationToolStripMenuItem";
            this.automaticInstallationToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.automaticInstallationToolStripMenuItem.Text = "Automatic installation";
            this.automaticInstallationToolStripMenuItem.Visible = false;
            this.automaticInstallationToolStripMenuItem.Click += new System.EventHandler(this.automaticInstallationToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(235, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
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
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(246, 26);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(243, 6);
            // 
            // viewProjectOnGitHubToolStripMenuItem
            // 
            this.viewProjectOnGitHubToolStripMenuItem.Name = "viewProjectOnGitHubToolStripMenuItem";
            this.viewProjectOnGitHubToolStripMenuItem.Size = new System.Drawing.Size(246, 26);
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
            this.summaryLabel.Location = new System.Drawing.Point(12, 319);
            this.summaryLabel.Name = "summaryLabel";
            this.summaryLabel.Size = new System.Drawing.Size(715, 23);
            this.summaryLabel.TabIndex = 11;
            this.summaryLabel.Text = "MSIX and Gapps will be downloaded";
            this.summaryLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // msix_fileErrorLabel
            // 
            this.msix_fileErrorLabel.AutoSize = true;
            this.msix_fileErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.msix_fileErrorLabel.Location = new System.Drawing.Point(133, 121);
            this.msix_fileErrorLabel.Name = "msix_fileErrorLabel";
            this.msix_fileErrorLabel.Size = new System.Drawing.Size(114, 16);
            this.msix_fileErrorLabel.TabIndex = 12;
            this.msix_fileErrorLabel.Text = "File does not exist";
            this.msix_fileErrorLabel.Visible = false;
            // 
            // gapps_fileErrorLabel
            // 
            this.gapps_fileErrorLabel.AutoSize = true;
            this.gapps_fileErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.gapps_fileErrorLabel.Location = new System.Drawing.Point(133, 223);
            this.gapps_fileErrorLabel.Name = "gapps_fileErrorLabel";
            this.gapps_fileErrorLabel.Size = new System.Drawing.Size(114, 16);
            this.gapps_fileErrorLabel.TabIndex = 13;
            this.gapps_fileErrorLabel.Text = "File does not exist";
            this.gapps_fileErrorLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 357);
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
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
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
        private System.Windows.Forms.ToolStripMenuItem automaticInstallationToolStripMenuItem;
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
    }
}

