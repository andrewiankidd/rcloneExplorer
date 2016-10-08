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
    IniFile iniSettings;
    rcloneExplorerMiscContainer miscContainer;

    public List<String[]> uploading = new List<String[]>();
    public List<String[]> uploadingPID = new List<String[]>();

    public void init()
    {
      iniSettings = rcloneExplorer.iniSettings;
      miscContainer = rcloneExplorer.miscContainer;
    }
    public void ctxtUploadContext_Cancel_Click(object sender, EventArgs e)
    {
      ListView lstUploads = rcloneExplorer.myform.Controls.Find("lstUploads", true)[0] as ListView;
      //find PID for current transfer (list item order should match with downloadPID list... :( )
      int PID = Convert.ToInt32(uploadingPID[lstUploads.SelectedItems[0].Index][0]);
      //find filename for current transfer (easy enough to pick it from the list since it's selected)
      string FN = lstUploads.SelectedItems[0].SubItems[1].Text;
      //get progress of file (cant cancel 100%)
      string FP = lstUploads.SelectedItems[0].SubItems[0].Text;

      //if the file process is 100%, it's done
      if (FP == "Done!")
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
        //mark list entry as cancelled
        lstUploads.SelectedItems[0].SubItems[1].Text = "Cancelled:" + lstUploads.SelectedItems[0].SubItems[1].Text;
      }
    }
  }
}
