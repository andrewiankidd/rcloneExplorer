using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public class rcloneExplorerDownloadHandler
  {
    rcloneExplorerMiscContainer miscContainer;
    //vars
    public List<String[]> downloading = new List<String[]>();
    public List<String[]> downloadPID = new List<String[]>();

    public void init()
    {
      miscContainer = rcloneExplorer.miscContainer;
    }

    public void ctxtDownloadContext_Cancel_Click(object sender, EventArgs e)
    {
      ListView lstDownloads = rcloneExplorer.myform.Controls.Find("lstDownloads", true)[0] as ListView;
      //find PID for current transfer (list item order should match with downloadPID list... :( )
      int PID = Convert.ToInt32(downloadPID[lstDownloads.SelectedItems[0].Index][0]);
      //find filename for current transfer (easy enough to pick it from the list since it's selected)
      string FN = lstDownloads.SelectedItems[0].SubItems[1].Text;
      //get progress of file (cant cancel 100%)
      string FP = lstDownloads.SelectedItems[0].SubItems[0].Text;

      //if the file process is 100%, it's done
      if (FP == "100%")
      {
        MessageBox.Show("ERR: Can't cancel a transferred file!");
      }
      //if it's not 100%, it might still be ongoing, so check the process is no longer active
      else if (!miscContainer.ProcessExists(PID))
      {
        MessageBox.Show("ERR: Transfer already completed");
      }
      //file is not 100% and the process is still active
      else
      {
        //kill PID
        miscContainer.KillProcessAndChildren(PID);
        //if the file exists, delete it
        if (System.IO.File.Exists(FN))
        {
          System.IO.File.Delete(FN);
        }
        //mark list entry as cancelled
        lstDownloads.SelectedItems[0].SubItems[1].Text = "Cancelled:" + lstDownloads.SelectedItems[0].SubItems[1].Text;
      }
    }

  }
}
