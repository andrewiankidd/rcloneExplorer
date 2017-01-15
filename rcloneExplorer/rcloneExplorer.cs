using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Management;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace rcloneExplorer
{
  public partial class rcloneExplorer : Form
  {
    public static IniFile iniSettings = new IniFile();
    public static rcloneExplorerMiscContainer miscContainer = new rcloneExplorerMiscContainer(); //
    public static rcloneExplorerUploadHandler uploadsHandler = new rcloneExplorerUploadHandler(); //misc
    public static rcloneExplorerDownloadHandler downloadsHandler = new rcloneExplorerDownloadHandler(); //misc
    public static rcloneExplorerSyncHandler syncingHandler = new rcloneExplorerSyncHandler(); //ini, internal
    public static rcloneExplorerInternalExec internalExecHandler = new rcloneExplorerInternalExec(); // downloads, uploads, sync
    public static rcloneExplorerExploreHandler exploreHandler = new rcloneExplorerExploreHandler(); //misc, internal, downloads, uploads
    public static rcloneExplorerInitialization InitializationHandler = new rcloneExplorerInitialization(); //misc, explore, internal
    public static rcloneExplorerTickHandler tickHandler = new rcloneExplorerTickHandler(); //misc, explore, downloads, uploads, syncing
    public static NotifyIcon notifyIconPub;
    public static Form myform = null;

    //set global vars
    public static string[] files;
    public static string remoteCD = "";
    public static long totalFilesize = 0;
    public static string rawOutputBuffer = "";
    public static bool loaded = false;
    public static bool initialSetup = false;
    public static bool configEncrypted = false;
    public static bool consoleEnabled = false;
    public static System.Windows.Forms.Timer transferTimer = new System.Windows.Forms.Timer();

    public rcloneExplorer()
    {
      //init classes      
      miscContainer.init();
      uploadsHandler.init();
      downloadsHandler.init();
      syncingHandler.init();
      internalExecHandler.init();
      exploreHandler.init();
      InitializationHandler.init();
      tickHandler.init();
      //init rclone settings
      InitializationHandler.initRcloneSettings();
      //wait for initial setup to complete if need be
      if (initialSetup) {
        var SetupWiz = new rcloneExplorerSetupWiz();
        var waitforcomplete = SetupWiz.ShowDialog();
        initialSetup = false;
      }
      //start the splashscreen in a background thread so the main form can work away
      new Thread(() =>
      {
        Application.Run(new rcloneSplash());
      }).Start();
      //initialize the form
      myform = this;
      this.Visible = false;
      this.InitializeComponent();
      //form UI adjustments
      InitializationHandler.initMainUI();
      //run rclone for the first time to get a list of files
      InitializationHandler.initSyncSettings();
      //start rclone
      InitializationHandler.initRcloneProcess();
      notifyIconPub = notifyIcon;
    }

    private void menuStripView_ToggleConsole_Click(object sender, EventArgs e)
    {
      //if collapsed then expand
      if (!consoleEnabled)
      {
        consoleEnabled=true;
        txtRawOut.Height = 60;
        tabMainUI.Height -= 60;
      }
      else
      //if expanded then collapse
      {
        consoleEnabled=false;
        txtRawOut.Height = 0;
        tabMainUI.Height += 60;
      }
    }

    private void menuStripFile_Quit_Click(object sender, EventArgs e)
    {
      //quit app, no worries
      Environment.Exit(0);
    }

    private void menuStripFile_QuitKill_Click(object sender, EventArgs e)
    {
      //go through every rclone download process on record
      foreach (string[] entry in downloadsHandler.downloading)
      {
        //get process ID
        int PID = Convert.ToInt32(entry[0]);
        //check if the process is still active
        if (miscContainer.ProcessExists(PID))
        {
          //kill PID
          miscContainer.KillProcessAndChildren(PID);
        }
      }
      //close app
      Environment.Exit(0);
    }

    private void menuStripView_Refresh_Click(object sender, EventArgs e)
    {
      exploreHandler.refreshlstExplorer();
    }

    private void menuStripView_RcloneConfig_Click(object sender, EventArgs e)
    {
      //look for existing wizard forms
      Form fc = Application.OpenForms["rcloneExplorerSetupWiz"];
      //if there are none, open one
      if (fc == null)
      {
        var SetupWiz = new rcloneExplorerSetupWiz();
        SetupWiz.Show();
      }
    }

    private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        Form fc = Application.OpenForms["rcloneExplorerSettings"];
        //if there are none, open one
        if (fc == null)
        {
            var SetupWiz = new rcloneExplorerSettings();
            SetupWiz.Show();
        }
    }

    private void rcloneExplorer_Resize(object sender, EventArgs e)
    {
      if (FormWindowState.Minimized == this.WindowState)
      {
        notifyIcon.Visible = true;
        notifyIcon.ShowBalloonTip(500, "Minimized to tray!", "Transfers will still run in the background", ToolTipIcon.Info);
        this.Hide();
      }

      else if (FormWindowState.Normal == this.WindowState)
      {
        notifyIcon.Visible = false;
      }
    }

    private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
    {
      this.Show();
      this.WindowState = FormWindowState.Normal;
    }

    /*
      _____                      _       
     |  __ \                    | |      
     | |__) |___ _ __ ___   ___ | |_ ___ 
     |  _  // _ \ '_ ` _ \ / _ \| __/ _ \
     | | \ \  __/ | | | | | (_) | ||  __/
     |_|  \_\___|_| |_| |_|\___/ \__\___|

     */

    private void lstExplorer_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        if (lstExplorer.FocusedItem.Bounds.Contains(e.Location) == true)
        {
          ctxtExplorerContext.Show(Cursor.Position);
        }
      }
    }
    private void lstExplorer_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      exploreHandler.lstExplorer_ColumnClick(sender, e);
    }
    private void lstExplorer_DragEnter(object sender, DragEventArgs e)
    {
      exploreHandler.lstExplorer_DragEnter(sender, e);
    }
    private void lstExplorer_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      exploreHandler.lstExplorer_MouseDoubleClick(sender, e);
    }
    private void streamMediaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      exploreHandler.streamMediaToolStripMenuItem_Click(sender, e);
    }
    private void ctxtExplorerContext_NewFolder_Click(object sender, EventArgs e)
    {
      exploreHandler.ctxtExplorerContext_NewFolder_Click(sender, e);
    }

    private void ctxtExplorerContext_Delete_Click(object sender, EventArgs e)
    {
      exploreHandler.ctxtExplorerContext_Delete_Click(sender, e);
    }

    private void ctxtExplorerContext_Rename_Click(object sender, EventArgs e)
    {
      exploreHandler.ctxtExplorerContext_Rename_Click(sender, e);
    }

    private void lstExplorer_DragDrop(object sender, DragEventArgs e)
    {
      exploreHandler.lstExplorer_DragDrop(e);
    }

    /*
      _____                      _                 _     
     |  __ \                    | |               | |    
     | |  | | _____      ___ __ | | ___   __ _  __| |___ 
     | |  | |/ _ \ \ /\ / / '_ \| |/ _ \ / _` |/ _` / __|
     | |__| | (_) \ V  V /| | | | | (_) | (_| | (_| \__ \
     |_____/ \___/ \_/\_/ |_| |_|_|\___/ \__,_|\__,_|___/

     */

    private void lstDownloads_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        if (lstDownloads.FocusedItem.Bounds.Contains(e.Location) == true)
        {
          ctxtDownloadContext.Show(Cursor.Position);
        }
      }
    }
    private void ctxtDownloadContext_Cancel_Click(object sender, EventArgs e)
    {
      downloadsHandler.ctxtDownloadContext_Cancel_Click(sender, e);
    }

    /*
      _    _       _                 _     
     | |  | |     | |               | |    
     | |  | |_ __ | | ___   __ _  __| |___ 
     | |  | | '_ \| |/ _ \ / _` |/ _` / __|
     | |__| | |_) | | (_) | (_| | (_| \__ \
      \____/| .__/|_|\___/ \__,_|\__,_|___/
            | |                            
            |_|                            

     */
    private void lstUploads_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        if (lstUploads.FocusedItem.Bounds.Contains(e.Location) == true)
        {
          ctxtUploadContext.Show(Cursor.Position);
        }
      }
    }
    private void ctxtUploadContext_Cancel_Click(object sender, EventArgs e)
    {
      uploadsHandler.ctxtUploadContext_Cancel_Click(sender, e);
    }

    /*
       _____                  _             
      / ____|                (_)            
     | (___  _   _ _ __   ___ _ _ __   __ _ 
      \___ \| | | | '_ \ / __| | '_ \ / _` |
      ____) | |_| | | | | (__| | | | | (_| |
     |_____/ \__, |_| |_|\___|_|_| |_|\__, |
              __/ |                    __/ |
             |___/                    |___/ 

     */

    private void btnSyncSourceSelect_Click(object sender, EventArgs e)
    {
      syncingHandler.btnSyncSourceSelect_Click(sender, e);
    }

    private void btnSyncDestinationSelect_Click(object sender, EventArgs e)
    {
      syncingHandler.btnSyncDestinationSelect_Click(sender, e);
    }

    private void lblSyncOptionsHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      syncingHandler.lblSyncOptionsHelp_LinkClicked(sender, e);
    }

    private void btnSyncSave_Click(object sender, EventArgs e)
    {
      syncingHandler.btnSyncSave_Click(sender, e);
    }

    private void btnSyncStart_Click(object sender, EventArgs e)
    {
      syncingHandler.btnSyncStart_Click(sender, e);
    }

    }
}
