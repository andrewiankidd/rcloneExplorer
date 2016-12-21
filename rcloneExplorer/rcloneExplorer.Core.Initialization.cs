using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public partial class rcloneExplorerInitialization
  {
    IniFile iniSettings;
    rcloneExplorerMiscContainer miscContainer;
    rcloneExplorerExploreHandler exploreHandler;
    rcloneExplorerInternalExec internalExecHandler;
    public void init()
    {
      iniSettings = rcloneExplorer.iniSettings;
      miscContainer = rcloneExplorer.miscContainer;
      exploreHandler = rcloneExplorer.exploreHandler;
      internalExecHandler = rcloneExplorer.internalExecHandler;
    }
    public void initRcloneSettings()
    {
      //check local dir for rclone
      if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\rclone.exe"))
      {
        //rclone not found, quit
        MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory + "\\rclone.exe \r\nfile not found!");
        Environment.Exit(0);
      }

      if (System.IO.File.Exists("rcloneExplorer.ini"))
      {
        //config found, checking for settings 
        if (string.IsNullOrEmpty(iniSettings.Read("rcloneRemote")))
        {
          MessageBox.Show("ERR: No default remote selected! Please choose a remote.\r\n\r\nLoading config, this may take a minute...");
          rcloneExplorer.initialSetup = true;
        }
        else
        {
          //config seems ok so read config settings
          iniSettings.Read("rcloneRemote");
        }
      }
      else
      {
        //file not found!
        iniSettings.Write("rcloneRemote", "");
        iniSettings.Write("rcloneVerbose", "false");
        iniSettings.Write("refreshAutomatically", "false");
        iniSettings.Write("rcloneSyncSource", "");
        iniSettings.Write("rcloneSyncDestination", "");
        iniSettings.Write("rcloneSyncEnabled", "false");
        iniSettings.Write("rcloneSyncFrequency", "0");
        iniSettings.Write("rcloneSyncSvC", "copy");
        iniSettings.Write("rcloneSyncBandwidthLimit", "0");
        iniSettings.Write("rcloneSyncMinFileSize", "0");
        iniSettings.Write("rcloneSyncMaxFileSize", "0");
        string configpass = "";
        internalExecHandler.Execute("version", "", "passcheck");
        if (rcloneExplorer.configEncrypted)
        { 
            configpass = PromptGenerator.ShowDialog("config password:", "Encrypted config check");
        }
        iniSettings.Write("rcloneConfigPass", configpass);
        MessageBox.Show("ERR: No ini file found!\r\n\r\nLoading config screen, this may take a minute...");
        rcloneExplorer.initialSetup = true;
      }
      if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\ffplay.exe"))
      {
        rcloneExplorer.streamingEnabled = true;
      }
    }
    public void initSyncSettings()
    {
      TextBox txtSyncSource = rcloneExplorer.myform.Controls.Find("txtSyncSource", true)[0] as TextBox;
      TextBox txtSyncDestination = rcloneExplorer.myform.Controls.Find("txtSyncDestination", true)[0] as TextBox;
      ComboBox cmbSyncOptionsEnabled = rcloneExplorer.myform.Controls.Find("cmbSyncOptionsEnabled", true)[0] as ComboBox;
      NumericUpDown numSyncOptionsFrequency = rcloneExplorer.myform.Controls.Find("numSyncOptionsFrequency", true)[0] as NumericUpDown;
      ComboBox cmbSyncOptionsSvC = rcloneExplorer.myform.Controls.Find("cmbSyncOptionsSvC", true)[0] as ComboBox;
      NumericUpDown numSyncOptionsBandwidthLimit = rcloneExplorer.myform.Controls.Find("numSyncOptionsBandwidthLimit", true)[0] as NumericUpDown;
      NumericUpDown numSyncOptionsMinSize = rcloneExplorer.myform.Controls.Find("numSyncOptionsMinSize", true)[0] as NumericUpDown;
      NumericUpDown numSyncOptionsMaxSize = rcloneExplorer.myform.Controls.Find("numSyncOptionsMaxSize", true)[0] as NumericUpDown;
      //get sync settings  
      txtSyncSource.Text = iniSettings.Read("rcloneSyncSource");
      txtSyncDestination.Text = iniSettings.Read("rcloneSyncDestination");
      cmbSyncOptionsEnabled.Text = iniSettings.Read("rcloneSyncEnabled");
      numSyncOptionsFrequency.Text = iniSettings.Read("rcloneSyncFrequency");
      cmbSyncOptionsSvC.Text = iniSettings.Read("rcloneSyncSvC");
      numSyncOptionsBandwidthLimit.Text = iniSettings.Read("rcloneSyncBandwidthLimit");
      numSyncOptionsMinSize.Text = iniSettings.Read("rcloneSyncMinFileSize");
      numSyncOptionsMaxSize.Text = iniSettings.Read("rcloneSyncMaxFileSize");
    }

    public void initMainUI()
    {
      ListView lstDownloads = rcloneExplorer.myform.Controls.Find("lstDownloads", true)[0] as ListView;
      ListView lstUploads = rcloneExplorer.myform.Controls.Find("lstUploads", true)[0] as ListView;
      ListView lstExplorer = rcloneExplorer.myform.Controls.Find("lstExplorer", true)[0] as ListView;
      
      lstExplorer.Columns[0].Width = 0;
      lstExplorer.Columns[2].Width = -2;
      lstExplorer.Columns[3].Width = -2;
      lstDownloads.Columns[1].Width = -2;
      lstUploads.Columns[1].Width = 30;
      lstUploads.Columns[1].Width = -2;
    }
    public void initRcloneProcess()
    {
      Label lblFooter = rcloneExplorer.myform.Controls.Find("lblFooter", true)[0] as Label;
      //populate the listview with results
      exploreHandler.populatelstExplorer(iniSettings.Read("rcloneRemote") + ":");
      //show total filesize in footer
      //lblFooter.Text = "Total Filesize:" + miscContainer.BytesToString(rcloneExplorer.totalFilesize).ToString();
      //mark as loaded/ready (close splashscreen)
      rcloneExplorer.loaded = true;
      //create a timer which can monitor progress periodically
      rcloneExplorer.transferTimer.Tick += new EventHandler(rcloneExplorer.tickHandler.transferTimer_Tick);
      rcloneExplorer.transferTimer.Interval = 1000;
      //start timer
      rcloneExplorer.transferTimer.Start();
    }
  }
}
