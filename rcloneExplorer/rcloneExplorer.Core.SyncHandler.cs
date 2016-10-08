using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

//http://stackoverflow.com/questions/217902/reading-writing-an-ini-file
namespace rcloneExplorer
{
  public class rcloneExplorerSyncHandler
  {
    IniFile iniSettings;
    rcloneExplorerInternalExec internalExecHandler;
    //set local vars
    public int syncingPID = 0;

    public void init()
    {
      iniSettings = rcloneExplorer.iniSettings;
      internalExecHandler = rcloneExplorer.internalExecHandler;
    }
    public void startSync()
    {
      GroupBox grpSyncOptions = rcloneExplorer.myform.Controls.Find("grpSyncOptions", true)[0] as GroupBox;

      //write basic sync command
      string synccmd = iniSettings.Read("rcloneSyncSvC");
      string syncargs = "\"" + iniSettings.Read("rcloneSyncSource") + "\" " + iniSettings.Read("rcloneRemote") + ":\"" + iniSettings.Read("rcloneSyncDestination") + "/\"";
      //add extra options
      if (iniSettings.Read("rcloneSyncBandwidthLimit") != "0") { syncargs += " --bwlimit " + iniSettings.Read("rcloneSyncBandwidthLimit"); }
      if (iniSettings.Read("rcloneSyncMinFileSize") != "0") { syncargs += " --max-size " + iniSettings.Read("rcloneSyncMinFileSize"); }
      if (iniSettings.Read("rcloneSyncMaxFileSize") != "0") { syncargs += " --min-size " + iniSettings.Read("rcloneSyncMaxFileSize"); }
      new Thread(() =>
      {
        rcloneExplorer.internalExecHandler.Execute(synccmd, syncargs, "sync");
      }).Start();
      iniSettings.Write("rcloneLastSyncTime", System.DateTime.Now.ToString());
    }

    public void lblSyncOptionsHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      string synchelp = "";
      synchelp += "\r\n" + "Auto Sync Enabled:";
      synchelp += "\r\n" + "If rcloneExplorer should automatically sync on a schedule";
      synchelp += "\r\n" + "";
      synchelp = "\r\n" + "Sync Frequency (hrs):";
      synchelp += "\r\n" + "How often rcloneExplorer will check run rclone to sync data to the remote connection.";
      synchelp += "\r\n" + "";
      synchelp += "\r\n" + "Sync or Copy:";
      synchelp += "\r\n" + "Sync copies source to the destination. Destination is updated to match source, including deleting files if necessary.";
      synchelp += "\r\n" + "Copy copies source to the destination. Source files are added to destination and will be kept even if deleted locally.";
      synchelp += "\r\n" + "";
      synchelp += "\r\n" + "Bandwidth Limit (kbps):";
      synchelp += "\r\n" + "How much bandwidth the sync process can use in kbps";
      synchelp += "\r\n" + "";
      synchelp += "\r\n" + "Ignore files under (kb)";
      synchelp += "\r\n" + "Defines the minimum filesize to be considered for sync";
      synchelp += "\r\n" + "";
      synchelp += "\r\n" + "Ignore files over (kb)";
      synchelp += "\r\n" + "Defines the maximum filesize to be considered for sync";
      MessageBox.Show(synchelp, "Sync Options Help");
    }
    public void btnSyncStart_Click(object sender, EventArgs e)
    {
      GroupBox grpSyncOptions = rcloneExplorer.myform.Controls.Find("grpSyncOptions", true)[0] as GroupBox;
      Button btnSyncStart = rcloneExplorer.myform.Controls.Find("btnSyncStart", true)[0] as Button;
      if (btnSyncStart.Text == "Start Sync")
      {
        //disable ui
        grpSyncOptions.Enabled = false;
        btnSyncStart.Text = "Cancel Sync";
        btnSyncStart.Refresh();
        //start sync
        startSync();
      }
      if (btnSyncStart.Text == "Cancel Sync")
      {
        //enable ui
        grpSyncOptions.Enabled = true;
        btnSyncStart.Text = "Start Sync";
        btnSyncStart.Refresh();
        //endSync();
      }
    }
    public void btnSyncDestinationSelect_Click(object sender, EventArgs e)
    {
      TextBox txtSyncSource = rcloneExplorer.myform.Controls.Find("txtSyncSource", true)[0] as TextBox;
      TextBox txtSyncDestination = rcloneExplorer.myform.Controls.Find("txtSyncDestination", true)[0] as TextBox;
      string dirName = "";
      if (!String.IsNullOrEmpty(txtSyncSource.Text))
      { dirName += new DirectoryInfo(txtSyncSource.Text).Name; }
      txtSyncDestination.Text = "/" + rcloneExplorer.remoteCD + dirName;
    }

    public void btnSyncSourceSelect_Click(object sender, EventArgs e)
    {
      TextBox txtSyncSource = rcloneExplorer.myform.Controls.Find("txtSyncSource", true)[0] as TextBox;
      TextBox txtSyncDestination = rcloneExplorer.myform.Controls.Find("txtSyncDestination", true)[0] as TextBox;
      //create save dialog
      FolderBrowserDialog syncsourcedir = new FolderBrowserDialog();
      //once a folder has been selected
      if (syncsourcedir.ShowDialog() == DialogResult.OK)
      {
        txtSyncSource.Text = syncsourcedir.SelectedPath;
        string dirName = new DirectoryInfo(txtSyncSource.Text).Name;
        txtSyncDestination.Text = rcloneExplorer.remoteCD + dirName;
      }
    }
    public void btnSyncSave_Click(object sender, EventArgs e)
    {
      TextBox txtSyncSource = rcloneExplorer.myform.Controls.Find("txtSyncSource", true)[0] as TextBox;
      TextBox txtSyncDestination = rcloneExplorer.myform.Controls.Find("txtSyncDestination", true)[0] as TextBox;
      ComboBox cmbSyncOptionsEnabled = rcloneExplorer.myform.Controls.Find("cmbSyncOptionsEnabled", true)[0] as ComboBox;
      NumericUpDown numSyncOptionsFrequency = rcloneExplorer.myform.Controls.Find("numSyncOptionsFrequency", true)[0] as NumericUpDown;
      ComboBox cmbSyncOptionsSvC = rcloneExplorer.myform.Controls.Find("cmbSyncOptionsSvC", true)[0] as ComboBox;
      NumericUpDown numSyncOptionsBandwidthLimit = rcloneExplorer.myform.Controls.Find("numSyncOptionsBandwidthLimit", true)[0] as NumericUpDown;
      NumericUpDown numSyncOptionsMinSize = rcloneExplorer.myform.Controls.Find("numSyncOptionsMinSize", true)[0] as NumericUpDown;
      NumericUpDown numSyncOptionsMaxSize = rcloneExplorer.myform.Controls.Find("numSyncOptionsMaxSize", true)[0] as NumericUpDown;
      iniSettings.Write("rcloneSyncSource", txtSyncSource.Text);
      iniSettings.Write("rcloneSyncDestination", txtSyncDestination.Text);
      iniSettings.Write("rcloneSyncEnabled", cmbSyncOptionsEnabled.Text);
      iniSettings.Write("rcloneSyncFrequency", numSyncOptionsFrequency.Value.ToString());
      iniSettings.Write("rcloneSyncSvC", cmbSyncOptionsSvC.Text);
      iniSettings.Write("rcloneSyncBandwidthLimit", numSyncOptionsBandwidthLimit.Text);
      iniSettings.Write("rcloneSyncMinFileSize", numSyncOptionsMinSize.Text);
      iniSettings.Write("rcloneSyncMaxFileSize", numSyncOptionsMaxSize.Text);
    }
  }
}