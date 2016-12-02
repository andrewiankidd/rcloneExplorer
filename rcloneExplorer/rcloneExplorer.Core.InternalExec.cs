using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rcloneExplorer
{
  public class rcloneExplorerInternalExec
  {
    IniFile iniSettings;
    rcloneExplorerDownloadHandler downloadsHandler;
    rcloneExplorerUploadHandler uploadsHandler;
    rcloneExplorerSyncHandler syncingHandler;

    public void init()
    {
      iniSettings = rcloneExplorer.iniSettings;
      downloadsHandler = rcloneExplorer.downloadsHandler;
      uploadsHandler = rcloneExplorer.uploadsHandler;
      syncingHandler = rcloneExplorer.syncingHandler;
    }

    public string Execute(string command, string arguments, string operation = null, string prepend = null)
    {

      string rcloneLogs = "";
      //check for verbose logging
      if (operation == "sync")
      {
        rcloneLogs = " --log-file sync.log --verbose";
      }
      else if (iniSettings.Read("rcloneVerbose") == "true")
      {
        rcloneLogs = " --log-file rclone.log --verbose";
      }

      //set up cmd to call rclone
      Process process = new Process();
      process.StartInfo.FileName = "cmd.exe";
      process.StartInfo.Arguments = "/c " + prepend + "rclone.exe " + command + " " + arguments + rcloneLogs;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
      process.Start();

      //log process ID for uploads, downloads and sync operations
      if (!String.IsNullOrEmpty(operation))
      {
        if (operation == "up")
        {
          uploadsHandler.uploadingPID.Add(new string[] { process.Id.ToString(), arguments });
        }
        else if (operation == "down")
        {
          downloadsHandler.downloadPID.Add(new string[] { process.Id.ToString(), arguments });
        }
        else if (operation == "sync")
        {
          syncingHandler.syncingPID = process.Id;
        }
        else if (operation == "config")
        {
          //this space will be used to pipe commands from wizard to interactive config shell
        }

      }

      // Synchronously read the standard output of the spawned process. 
      string output = process.StandardOutput.ReadToEnd();
      if (output == null) { output = process.StandardError.ReadToEnd(); }

      //close process when it's finished
      process.WaitForExit();
      process.Close();

      //return output
      return output;
    }

  }
}
