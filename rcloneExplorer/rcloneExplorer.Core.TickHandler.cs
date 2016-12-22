using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public class rcloneExplorerTickHandler
  {
    IniFile iniSettings;
    rcloneExplorerMiscContainer miscContainer;
    rcloneExplorerExploreHandler exploreHandler;
    rcloneExplorerDownloadHandler downloadsHandler;
    rcloneExplorerUploadHandler uploadsHandler;
    rcloneExplorerSyncHandler syncingHandler;

    public void init()
    {
      iniSettings = rcloneExplorer.iniSettings;
      miscContainer = rcloneExplorer.miscContainer;
      exploreHandler = rcloneExplorer.exploreHandler;
      downloadsHandler = rcloneExplorer.downloadsHandler;
      uploadsHandler = rcloneExplorer.uploadsHandler;
      syncingHandler = rcloneExplorer.syncingHandler;
    }

    public void transferTimer_Tick(object sender, EventArgs e)
    {
      ListView lstDownloads = rcloneExplorer.myform.Controls.Find("lstDownloads", true)[0] as ListView;
      ListView lstUploads = rcloneExplorer.myform.Controls.Find("lstUploads", true)[0] as ListView;
      TabPage tabDownloads = rcloneExplorer.myform.Controls.Find("tabDownloads", true)[0] as TabPage;
      TabPage tabUploads = rcloneExplorer.myform.Controls.Find("tabUploads", true)[0] as TabPage;
      TextBox txtSyncLog = rcloneExplorer.myform.Controls.Find("txtSyncLog", true)[0] as TextBox;

      if (downloadsHandler.downloading.Count > 0)
      {
        for (var i = 0; i < downloadsHandler.downloading.Count; i++)
        {
          {
            if (lstDownloads.Items[i].SubItems[0].Text != "100%")
            {
              string[] entry = downloadsHandler.downloading[i];
              //store the filename of the saved file
              string savedFilename = entry[1];
              //set default filesize for saved file
              long savedFilesizeBytes = 0;
              //check the filesize of the saved file so far
              if (System.IO.File.Exists(savedFilename))
              {
                //get file size
                savedFilesizeBytes = new System.IO.FileInfo(savedFilename).Length;
              }
              //store the filesize of the stored file in bytes for comparison
              long storedFilesizeBytes = Convert.ToInt64(entry[0]);
              //calc percentage
              long percentage = (long)((float)savedFilesizeBytes / storedFilesizeBytes * 100);
              //update percentage
              lstDownloads.Items[i].SubItems[0].Text = percentage.ToString() + "%";
            }
          }
        }
        if (tabDownloads.Text != "Downloads (" + lstDownloads.Items.Count + ")")
        {
          tabDownloads.Text = "Downloads (" + lstDownloads.Items.Count + ")";
        }
      }
      if (uploadsHandler.uploading.Count > 0 && uploadsHandler.uploadingPID.Count == uploadsHandler.uploading.Count)
      {
        for (var i = 0; i < uploadsHandler.uploading.Count; i++)
        {
          {
            //store current iteration from list
            string[] entry = uploadsHandler.uploading[i];
            //entry filename
            string uploadedFilename = entry[1];

            //check downloadPId proces.exists to see if uploadis complete yet
            int PID = Convert.ToInt32(uploadsHandler.uploadingPID[i][0]);
            if (miscContainer.ProcessExists(PID))
            {
              //upload still in progress
              lstUploads.Items[i].SubItems[0].Text = uploadsHandler.uploadingPID[i][1];
            }
            else
            {
              if (lstUploads.Items[i].SubItems[0].Text == "Done!")
              {
                //do nothing
              }
              else
              {
                //upload complete (guessing! probs best to validate this)
                lstUploads.Items[i].SubItems[0].Text = "Done!";
                if (iniSettings.Read("refreshAutomatically") == "true")
                {
                  exploreHandler.refreshlstExplorer();
                }
              }
            }
          }
        }
        if (tabUploads.Text != "Uploads (" + lstUploads.Items.Count + ")")
        {
          tabUploads.Text = "Uploads (" + lstUploads.Items.Count + ")";
        }
      }
      if (syncingHandler.syncingPID > 0)
      {
        if (File.Exists("sync.log"))
        {
          using (var fs = new FileStream("sync.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          using (var sr = new StreamReader(fs, Encoding.Default))
          {
            string tmp = sr.ReadToEnd();
            txtSyncLog.AppendText(tmp.Replace("\n", Environment.NewLine));
          }
        }
      }
    }
  }
}
