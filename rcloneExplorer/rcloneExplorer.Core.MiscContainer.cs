using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public class rcloneExplorerMiscContainer //AKA the stackoverflow class
  {
    public void init()
    {

    }
    public String BytesToString(long byteCount)
    {
      //http://stackoverflow.com/a/4975942

      string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
      if (byteCount == 0)
        return "0" + suf[0];
      long bytes = Math.Abs(byteCount);
      int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
      double num = Math.Round(bytes / Math.Pow(1024, place), 1);
      return (Math.Sign(byteCount) * num).ToString() + suf[place];
    }

    public bool ProcessExists(int id)
    {
      //http://stackoverflow.com/questions/1545270/how-to-determine-if-a-process-id-exists
      return Process.GetProcesses().Any(x => x.Id == id);
    }

    public void KillProcessAndChildren(int pid)
    {
      //http://stackoverflow.com/questions/5901679/kill-process-tree-programatically-in-c-sharp/32595027
      ManagementObjectSearcher searcher = new ManagementObjectSearcher
        ("Select * From Win32_Process Where ParentProcessID=" + pid);
      ManagementObjectCollection moc = searcher.Get();
      foreach (ManagementObject mo in moc)
      {
        KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
      }
      try
      {
        Process proc = Process.GetProcessById(pid);
        proc.Kill();
      }
      catch (ArgumentException)
      {
        // Process already exited.
      }
    }
  }
  public class ItemComparer : IComparer
  {
    //column used for comparison
    public int Column { get; set; }
    //Order of sorting
    public SortOrder Order { get; set; }
    public ItemComparer(int colIndex)
    {
      Column = colIndex;
      Order = SortOrder.None;
    }
    public int Compare(object a, object b)
    {
      int result;
      ListViewItem itemA = a as ListViewItem;
      ListViewItem itemB = b as ListViewItem;
      if (itemA == null && itemB == null)
        result = 0;
      else if (itemA == null)
        result = -1;
      else if (itemB == null)
        result = 1;
      if (itemA == itemB)
        result = 0;
      //alphabetic comparison
      result = String.Compare(itemA.SubItems[Column].Text, itemB.SubItems[Column].Text);
      // if sort order is descending.
      if (Order == SortOrder.Descending)
        // Invert the value returned by Compare.
        result *= -1;
      return result;
    }
  }
}
