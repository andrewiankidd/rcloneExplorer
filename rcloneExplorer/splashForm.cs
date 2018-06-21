using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace rcloneExplorer
{
    public partial class splashForm : Form
    {
        private RcloneHandler rcloneHandler { get; set; } = new RcloneHandler();
        private FfplayHandler ffplayHandler { get; set; } = new FfplayHandler();
        private UiHelper uiHelper { get; set; } = new UiHelper();
        private Assembly assembly { get; set; } = Assembly.GetExecutingAssembly();

        public splashForm()
        {
            InitializeComponent();
            lblVersion.Text = $"rcloneExplorer v{FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion}";
        }

        private void splashForm_Open(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                rcloneHandler.CheckUpdates(updateStatus);
                ffplayHandler.CheckUpdates(updateStatus);
                var choices = rcloneHandler.CheckConfig(updateStatus);
                Program.selectedRemote =  uiHelper.ComboPrompt(choices.Keys.ToList());
            }).ContinueWith(antecedant => { CloseSplash(); }); 
        }

        public void CloseSplash()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate () { this.Close(); });
            }
            else
            {
                this.Close();
            }
        }

        public int updateStatus(string statusText)
        {
            if (this.lblSplashText.InvokeRequired)
            {
                this.lblSplashText.BeginInvoke((MethodInvoker)delegate () { this.lblSplashText.Text = statusText; });
            }
            else
            {
                this.lblSplashText.Text = statusText;
            }

            return 1;
        }
    }
}
