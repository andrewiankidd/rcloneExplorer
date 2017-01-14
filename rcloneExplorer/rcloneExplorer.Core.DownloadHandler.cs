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

    public void init()
    {
      miscContainer = rcloneExplorer.miscContainer;
    }

    public void ctxtDownloadContext_Cancel_Click(object sender, EventArgs e)
    {
      ListView lstDownloads = rcloneExplorer.myform.Controls.Find("lstDownloads", true)[0] as ListView;
      //find PID for current transfer
      string PID = lstDownloads.SelectedItems[0].SubItems[0].Text;
      //get filename
      string FN = lstDownloads.SelectedItems[0].SubItems[1].Text;
      //get progress of file (cant cancel 100%)
      string FP = lstDownloads.SelectedItems[0].SubItems[2].Text;

      //if the file process is 100%, it's done
      if (FP == "Done!" || FP == "100%" || !miscContainer.ProcessExists(Convert.ToInt32(PID)))
      {
        MessageBox.Show("ERR: Transfer already completed");
      }
      //file is not 100% and the process is still active
      else
      {
        //kill PID
        miscContainer.KillProcessAndChildren(Convert.ToInt32(PID));
        //if the file exists, delete it
        if (System.IO.File.Exists(FN))
        {
          System.IO.File.Delete(FN);
        }
        //mark list entry as cancelled
        lstDownloads.SelectedItems[0].SubItems[0].Text = "Cancelled";
      }
    }

  }
}
