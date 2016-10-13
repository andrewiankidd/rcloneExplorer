using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public class rcloneExplorerExploreHandler
  {
    IniFile iniSettings;
    rcloneExplorerMiscContainer miscContainer;
    rcloneExplorerInternalExec internalExecHandler;
    rcloneExplorerDownloadHandler downloadsHandler;
    rcloneExplorerUploadHandler uploadsHandler;
    public void init ()
    {
      iniSettings = rcloneExplorer.iniSettings;
      miscContainer = rcloneExplorer.miscContainer;
      internalExecHandler = rcloneExplorer.internalExecHandler;
      downloadsHandler = rcloneExplorer.downloadsHandler;
      uploadsHandler = rcloneExplorer.uploadsHandler;
    }
    public void lstExplorer_DragDrop(DragEventArgs e)
    {     
      ListView lstUploads = rcloneExplorer.myform.Controls.Find("lstUploads", true)[0] as ListView;
      string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
      foreach (string file in files)
      {
        // get the file attributes for file or directory
        FileAttributes attr = File.GetAttributes(file);

        //detect whether its a directory or file
        if (attr.HasFlag(FileAttributes.Directory))
        {
          //its a folder
          MessageBox.Show("Uploading directory: " + file);
          //in a new thread start uploading the file
          new Thread(() =>
          {
            //get directory name so it can be sent to rclone process
            string dirName = new DirectoryInfo(file).Name;
            //copy local path to rclone folder + directory name
            internalExecHandler.Execute("copy", "\"" + file + "\" " + iniSettings.Read("rcloneRemote") + ":\"" + rcloneExplorer.remoteCD + "\\" + dirName + "\"");
          }).Start();
        }
        else
        {
          //its a file
          MessageBox.Show("Uploading file: " + file);
          //in a new thread start uploading the file
          new Thread(() =>
          {
            //copy local path to rclone folder
            internalExecHandler.Execute("copy", "\"" + file + "\" " + iniSettings.Read("rcloneRemote") + ":\"" + rcloneExplorer.remoteCD + "\"", "up");
          }).Start();
        }

        //store the path selected via the dialog and filename taken from the selected entry
        string[] temp = new string[] { "Uploading", file };
        //store the info into the download history list
        uploadsHandler.uploading.Add(temp);
        //add tolistview
        lstUploads.Items.Add(new ListViewItem(temp));
      }
    }

    public void lstExplorer_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
    }
    public void lstExplorer_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      ListView lstExplorer = rcloneExplorer.myform.Controls.Find("lstExplorer", true)[0] as ListView;
      // Call the sort method to manually sort.
      //lstExplorer.Items[0].Remove();
      ItemComparer sorter = lstExplorer.ListViewItemSorter as ItemComparer;
      //get clicked column num
      int eColumn = e.Column;
      //if clicked column is filesizehuman, sort by filesize bytes instead (dirty hack)
      if (eColumn == 1) { eColumn = 0; }
      if (sorter == null)
      {
        sorter = new ItemComparer(eColumn);
        sorter.Order = SortOrder.Ascending;
        lstExplorer.ListViewItemSorter = sorter;
      }
      // if clicked column is already the column that is being sorted
      if (eColumn == sorter.Column)
      {      
        // Reverse the current sort direction
        if (sorter.Order == SortOrder.Ascending)
          sorter.Order = SortOrder.Descending;
        else
          sorter.Order = SortOrder.Ascending;
      }
      else
      {
        // Set the column number that is to be sorted; default to ascending.
        sorter.Column = eColumn;
        sorter.Order = SortOrder.Ascending;
      }
      lstExplorer.Sort();
      //.Items.Insert(0,new ListViewItem(new string[] { "0", "<up>", "", ".." }));
    }
   
    public void lstExplorer_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      ListView lstExplorer = rcloneExplorer.myform.Controls.Find("lstExplorer", true)[0] as ListView;
      ListView lstDownloads = rcloneExplorer.myform.Controls.Find("lstDownloads", true)[0] as ListView;
      Label lblLoading = rcloneExplorer.myform.Controls.Find("lblLoading", true)[0] as Label;
      //setup vars for Stored (remote) files
      long storedFilesizeBytes = Convert.ToInt64(lstExplorer.SelectedItems[0].SubItems[0].Text);
      string storedFilesizeHuman = lstExplorer.SelectedItems[0].SubItems[1].Text;
      string storedDatemodified = lstExplorer.SelectedItems[0].SubItems[2].Text;
      string storedFilepath = rcloneExplorer.remoteCD + lstExplorer.SelectedItems[0].SubItems[3].Text;
      string storedFilename = storedFilepath.Split('/').ToList()[storedFilepath.Split('/').GetUpperBound(0)];

      if (storedFilesizeHuman == "<dir>")
      {
        //set new path
        rcloneExplorer.remoteCD = storedFilepath + "/";
        //refresh
        lblLoading.Visible = true;
        lblLoading.Refresh();
        populatelstExplorer(iniSettings.Read("rcloneRemote") + ":\"" + rcloneExplorer.remoteCD + "\"");
        lblLoading.Visible = false;
      }
      else if (storedFilesizeHuman == "<up>")
      {
        //seperate directories in string, rebuild array without last two, join back to string
        rcloneExplorer.remoteCD = String.Join(" ", rcloneExplorer.remoteCD.Split('/').Take(rcloneExplorer.remoteCD.Split('/').Count() - 2).ToArray());
        //populate lstview with new directory contents
        lblLoading.Visible = true;
        lblLoading.Refresh();
        populatelstExplorer(iniSettings.Read("rcloneRemote") + ":" + rcloneExplorer.remoteCD + "/");
        lblLoading.Visible = false;
      }
      else
      {
        MessageBox.Show("Saving file: " + storedFilename);
        //create save dialog
        FolderBrowserDialog savefile = new FolderBrowserDialog();
        //once a folder has been selected
        if (savefile.ShowDialog() == DialogResult.OK)
        {
          //store the path selected via the dialog and filename taken from the selected entry
          string[] storedvsaved = new string[] { storedFilesizeBytes.ToString(), savefile.SelectedPath + "\\" + storedFilename };
          //store the info into the download history list
          downloadsHandler.downloading.Add(storedvsaved);
          //then add to list view
          lstDownloads.Items.Add(new ListViewItem(storedvsaved));
          //in a new thread start downloading the file
          new Thread(() =>
          {
            internalExecHandler.Execute("copy", iniSettings.Read("rcloneRemote") + ":\"" + storedFilepath + "\" \"" + savefile.SelectedPath + "\"", "down");
          }).Start();
        }
      }
      //set window title
      Form.ActiveForm.Text = "rcloneExplorer :" + rcloneExplorer.remoteCD;
    }

    public void populatelstExplorer(string remotePath)
    {
      ListView lstExplorer = rcloneExplorer.myform.Controls.Find("lstExplorer", true)[0] as ListView;
      //clear existing
      lstExplorer.Items.Clear();
      //clear total
      rcloneExplorer.totalFilesize = 0;
      //add up [..]
      lstExplorer.Items.Add(new ListViewItem(new string[] { "0", "<up>", "", ".." }));
      //get rclone output
      rcloneExplorer.files = internalExecHandler.Execute("lsd", remotePath).Split('\n');
      //remove last value which is always null
      rcloneExplorer.files = rcloneExplorer.files.Take(rcloneExplorer.files.Count() - 1).ToArray();
      //process to list view
      foreach (string item in rcloneExplorer.files)
      {
        //split entry into data
        List<string> temp = item.TrimStart().Split(new string[] { " " }, 12, StringSplitOptions.None).ToList();
        //organize stored/remote dir information
        string fileBytes = "0";
        string fileHuman = "<dir>";
        string fileDate = temp[1];
        string filetime = temp[2];
        string filePath = temp[11];
        //echo the dir
        string[] temprow = new string[] { fileBytes, fileHuman, fileDate + " " + filetime, filePath };
        //insert
        lstExplorer.Items.Add(new ListViewItem(temprow));
        
      }
      //get rclone output
      rcloneExplorer.files = internalExecHandler.Execute("lsl", remotePath + " --max-depth 1").Split('\n');
      //remove last value which is always null
      rcloneExplorer.files = rcloneExplorer.files.Take(rcloneExplorer.files.Count() - 1).ToArray();
      //process to list view
      foreach (string item in rcloneExplorer.files)
      {
        //split entry into data
        List<string> temp = item.TrimStart().Split(new string[] { " " }, 4, StringSplitOptions.None).ToList();
        //organize stored/remote dir information
        string fileBytes = temp[0];
        string fileHuman = miscContainer.BytesToString(Convert.ToInt64(temp[0]));
        string fileDate = temp[1];
        string filetime = temp[2].Remove(temp[2].Length - 10);
        string filePath = temp[3];
        //echo the dir
        string[] temprow = new string[] { fileBytes, fileHuman, fileDate + " " + filetime, filePath };
        //insert
        lstExplorer.Items.Add(new ListViewItem(temprow));
        //keep count of total filesize
        rcloneExplorer.totalFilesize += Convert.ToInt64(fileBytes);
      }

      
    }
    public void refreshlstExplorer()
    {
      Label lblLoading = rcloneExplorer.myform.Controls.Find("lblLoading", true)[0] as Label;
      lblLoading.Visible = true;
      lblLoading.Refresh();
      populatelstExplorer(iniSettings.Read("rcloneRemote") + ":" + rcloneExplorer.remoteCD + "/");
      lblLoading.Visible = false;
    }
    public void streamMediaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ListView lstExplorer = rcloneExplorer.myform.Controls.Find("lstExplorer", true)[0] as ListView;
      string storedFilesizeHuman = lstExplorer.SelectedItems[0].SubItems[1].Text;
      string storedFilepath = rcloneExplorer.remoteCD + lstExplorer.SelectedItems[0].SubItems[3].Text;
      if (storedFilesizeHuman != "<dir>" && storedFilesizeHuman != "<up>")
      {
        //we're on the honor system for filetypes
        Process.Start("cmd.exe", "/c rclone.exe cat " + iniSettings.Read("rcloneRemote") + ":\"" + storedFilepath + "\" | ffplay -");
      }
      else
      {
        MessageBox.Show("ERR: Can't stream a directory.");
      }
        
    }

    public void ctxtExplorerContext_NewFolder_Click(object sender, EventArgs e)
    {
      string dirName = PromptGenerator.ShowDialog("Folder Name:", "New Folder"); 
      internalExecHandler.Execute("mkdir", iniSettings.Read("rcloneRemote") + ":\"" + rcloneExplorer.remoteCD + "\\" + dirName + "\"");
      //refresh
      if (iniSettings.Read("refreshAutomatically") == "true")
      {
        refreshlstExplorer();
      }
    }

    public void ctxtExplorerContext_Delete_Click(object sender, EventArgs e)
    {
      ListView lstExplorer = rcloneExplorer.myform.Controls.Find("lstExplorer", true)[0] as ListView;

      string storedFilesizeHuman = lstExplorer.SelectedItems[0].SubItems[1].Text;
      string storedFilepath = rcloneExplorer.remoteCD + lstExplorer.SelectedItems[0].SubItems[3].Text;

      DialogResult promptdelete = MessageBox.Show("Delete " + storedFilepath + "?", "Confirm Delete", MessageBoxButtons.YesNo);
      if (promptdelete == DialogResult.Yes)
      {
        if (storedFilesizeHuman == "<dir>")
        {
          new Thread(() =>
          {
            internalExecHandler.Execute("purge", iniSettings.Read("rcloneRemote") + ":\"" + storedFilepath + "\"");
          }).Start();
        }
        else
        {
          new Thread(() =>
          {
            internalExecHandler.Execute("delete", iniSettings.Read("rcloneRemote") + ":\"" + storedFilepath + "\"");
          }).Start();
        }
      }
    }
  }
}
