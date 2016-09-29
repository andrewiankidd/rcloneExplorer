using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace rcloneExplorer
{
  public partial class rcloneExplorer : Form
  {
    string[] files;
    string consoletxt;
    long totalFilesize = 0;
    public static bool loaded = false;
    private System.Windows.Forms.Timer downloadTimer;
    List<String[]> downloading =new List<String[]>();
    List<String[]> downloaded = new List<String[]>();
    string remoteCD = "";
    string remoteConnectionName = "";

    public rcloneExplorer()
    {
      //start the splashscreen in a background thread so the main form can work away
      new Thread(() =>
      {
        Application.Run(new rcloneSplash());
      }).Start();

      if (System.IO.File.Exists("rcloneExplorer.ini"))
      {
        //config found, reading remote name
        remoteConnectionName = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\rcloneExplorer.ini");
      }else
      {
        //file not found!
        MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory + "\\rcloneExplorer.ini \r\nfile not found!");
        Environment.Exit(0);
      }

      //hide the main window and do some minor UI adjustments
      this.Visible = false;
      this.CenterToScreen();
      InitializeComponent();
      lstExplorer.Columns[0].Width = 0;
      lstExplorer.Columns[2].Width = -2;
      lstExplorer.Columns[3].Width = -2;
      lstDownloads.Columns[1].Width = -2;
      //run rclone for the first time to get a list of files
      rcloneInit();
    }

    private void rcloneInit()
    {
      //check local dir for rclone
      if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\rclone.exe"))
      {
        //rclone not found, quit
        MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory + "\\rclone.exe \r\nfile not found!");
        Environment.Exit(0);
      }
      //populate the listview with results
      populatelstExplorer(internalExec("/c rclone.exe lsl " + remoteConnectionName + ":"));
      //set console text
      txtRawOut.Text = consoletxt;
      //show total filesize in footer
      lblFooter.Text = "Total Filesize:" + BytesToString(totalFilesize).ToString();
      //mark as loaded/ready (close splashscreen)
      loaded = true;
      //show window
      this.Visible = true;
    }

    private string internalExec(string command)
    {
      //set up cmd to call rclone
      Process process = new Process();
      process.StartInfo.FileName = "cmd.exe";
      process.StartInfo.Arguments = command + " --log-file logfile.log --verbose";
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
      process.Start();

      // Synchronously read the standard output of the spawned process. 
      string output = process.StandardOutput.ReadToEnd();
      if (output == null) { output = process.StandardError.ReadToEnd(); }

      //close process when it's finished
      process.WaitForExit();
      process.Close();

      //set raw output
      consoletxt = output.Replace("\n", Environment.NewLine);

      //return output
      return output;
    }

    private void populatelstExplorer(string fileArray)
    {
      //clear existing
      lstExplorer.Items.Clear();
      //clear total
      totalFilesize = 0;
      //add up [..]
      lstExplorer.Items.Add(new ListViewItem(new string[] { "0", "<up>", ".." }));
      //breakdown output
      files = fileArray.Split('\n');
      //remove last value which is always null
      files = files.Take(files.Count() - 1).ToArray();
      //create array which will store any directories
      List<string> fileDirList = new List<String>();

      //process to list view
      foreach (string item in files)
      {
        //split entry into filesize and path
        List<string> temp = item.TrimStart().Split(new string[] { " " }, 4, StringSplitOptions.None).ToList();

        string fileBytes = temp[0];
        string fileHuman = BytesToString(Convert.ToInt64(temp[0]));
        string fileDate = temp[1];
        string filetime = temp[2].Remove(temp[2].Length - 10);
        string filePath = temp[3];


        if (filePath.Contains("/"))
        {
          string thedir = filePath.Split('/').ToList()[0];
          if (!fileDirList.Contains(thedir))
          {
            //note that this dir is saved
            fileDirList.Add(thedir);
            //create array
            string[] temprow = new string[] { "0", "<dir>", fileDate + " " + filetime, thedir };
            //insert
            lstExplorer.Items.Add(new ListViewItem(temprow));
          }
          else
          {
            //this dir is already covered, do nothing
          }
        }
        else
        {
          //this is a file not a dir, make array
          string[] temprow = new string[] { fileBytes, fileHuman, fileDate + " " + filetime, filePath };
          //insert
          lstExplorer.Items.Add(new ListViewItem(temprow));
        }

        //keep count of total filesize
        totalFilesize += Convert.ToInt64(fileBytes);
        
      }
    }

    static String BytesToString(long byteCount)
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

    private void lstExplorer_MouseDoubleClick(object sender, MouseEventArgs e)
    {    
      long storedFilesizeBytes = Convert.ToInt64(lstExplorer.SelectedItems[0].SubItems[0].Text);
      string storedFilesizeHuman = lstExplorer.SelectedItems[0].SubItems[1].Text;
      string storedFilepath = remoteCD + lstExplorer.SelectedItems[0].SubItems[2].Text;
      string storedFilename = storedFilepath.Split('/').ToList()[storedFilepath.Split('/').GetUpperBound(0)];

      if (storedFilesizeHuman == "<dir>")
      {
        remoteCD = storedFilepath + "/";
        populatelstExplorer(internalExec("/c rclone.exe lsl " + remoteConnectionName + ":" + remoteCD + "/"));
        //set window title
        Form.ActiveForm.Text = remoteCD;
      }
      else if (storedFilesizeHuman == "<up>")
      {
        //seperate directories in string, rebuild array without last two, join back to string
        remoteCD = String.Join(" ", remoteCD.Split('/').Take(remoteCD.Split('/').Count() - 2).ToArray());
        populatelstExplorer(internalExec("/c rclone.exe lsl " + remoteConnectionName + ":" + remoteCD + "/"));
        //set window title
        Form.ActiveForm.Text = remoteCD;
      }
      else {
        MessageBox.Show("Saving file: " + storedFilename);
        //create save dialog
        FolderBrowserDialog savefile = new FolderBrowserDialog();
        //once a folder has been selected
        if (savefile.ShowDialog() == DialogResult.OK)
        {
          //store the path selected via the dialog and filename taken from the selected entry
          string[] storedvsaved = new string[] { savefile.SelectedPath + "\\" + storedFilename, storedFilesizeBytes.ToString(), "⇩" };
          downloading.Add(storedvsaved);
          //in a new thread start downloading the file
          new Thread(() =>
          {
            internalExec("/c rclone.exe copy " + remoteConnectionName + ":\"" + storedFilepath + "\" \"" + savefile.SelectedPath + "\"");
          }).Start();
          //start a timer which can monitor progress periodically
          downloadTimer = new System.Windows.Forms.Timer();
          downloadTimer.Tick += new EventHandler(downloadTimer_Tick);
          downloadTimer.Interval = 1000;
          downloadTimer.Start();
          //set output in console
          txtRawOut.Text = consoletxt;
        }
      }

    }

    private void downloadTimer_Tick(object sender, EventArgs e)
    {
      //check if file exists yet (probably wont the first tick)
      lstDownloads.Items.Clear();

      if (downloaded.Count >= 0)
      {
        foreach (string[] entry in downloaded)
        {
          //this is a file not a dir, make array
          string[] temprow = new string[] { "100%", entry[0] };
          //insert to download view
          lstDownloads.Items.Add(new ListViewItem(temprow));
        }
      }

      try
      { 
        if (downloading.Count >= 0) {
          foreach (string[] entry in downloading)
          {
            //store the filename of the saved file
            string savedFilename = entry[0];
            //set default filesize for saved file
            long savedFilesizeBytes = 0;
            //check the filesize of the saved file so far
            if (System.IO.File.Exists(entry[0]))
            {
              //get file size
              savedFilesizeBytes = new System.IO.FileInfo(entry[0]).Length;
            }
            //store the filesize of the stored file in bytes for comparison
            long storedFilesizeBytes = Convert.ToInt64(entry[1]);
            //calc percentage
            long percentage = (long)((float)savedFilesizeBytes / storedFilesizeBytes * 100);
            //set footer text
            lblFooter.Text = "Total Filesize:" + BytesToString(totalFilesize).ToString() + " | Downloading: " + downloading.Count() + " file(s)";
            if (savedFilesizeBytes == storedFilesizeBytes) {
              downloaded.Add(entry);
              downloading.Remove(entry);
            }
            //insert to download view
            lstDownloads.Items.Add(new ListViewItem(new string[] { percentage.ToString() + "%", savedFilename }));
          }
        }
      }
      catch
      {
        Console.WriteLine("download finished before ticker could get to it");
      }



    }

    private void menuStripToggleConsole_Click(object sender, EventArgs e)
    {
      //if collapsed then expand
      if (txtRawOut.Height == 0)
      {
        txtRawOut.Height = 60;
        tabMainUI.Height -= 60;
      }
      else
      //if expanded then collapse
      {
        txtRawOut.Height = 0;
        tabMainUI.Height += 60;
      }
    }

  }
}
