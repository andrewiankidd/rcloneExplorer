using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public class rcloneExplorerUploadHandler
  {
    rcloneExplorerMiscContainer miscContainer;

    public List<String[]> uploading = new List<String[]>();

    public void init()
    {
      miscContainer = rcloneExplorer.miscContainer;
    }
    public void ctxtUploadContext_Cancel_Click(object sender, EventArgs e)
    {
      ListView lstUploads = rcloneExplorer.myform.Controls.Find("lstUploads", true)[0] as ListView;
      //find PID for current transfer 
      string PID = lstUploads.SelectedItems[0].SubItems[0].Text;
      //get progress of file (cant cancel 100%)
      string FP = lstUploads.SelectedItems[0].SubItems[2].Text;
      //if the file process is 100%, it's done
      if (FP == "Done!" || !miscContainer.ProcessExists(Convert.ToInt32(PID)))
      {
        MessageBox.Show("ERR: Transfer already completed");
      }
      //file is not 100% and the process is still active. kill and cancel
      else
      {
        //kill PID
        miscContainer.KillProcessAndChildren(Convert.ToInt32(PID));
        //mark list entry as cancelled
        lstUploads.SelectedItems[0].SubItems[0].Text = "Cancelled";
      }
    }
  }
}
