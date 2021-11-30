using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WsaGappsTool;

namespace WsaGappsTool
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void automaticInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("The automatic installation feature downloads both the WSA .msix package and the latest gapps package, then automatically begins the modification process. \n\nThis feature is experimental, and therefore may not operate as intended. \n\nWould you like to continue anyway?", "Automatic installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Run
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This process might take a while (~10-15 minutes). Are you sure you want to continue?", config.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Run
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }
    }
}