using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public class rcloneExplorerInternalExec
  {
    IniFile iniSettings;
    rcloneExplorerDownloadHandler downloadsHandler;
    rcloneExplorerUploadHandler uploadsHandler;
    rcloneExplorerMiscContainer miscContainer;
    rcloneExplorerSyncHandler syncingHandler;   

    public void init()
    {
      iniSettings = rcloneExplorer.iniSettings;
      downloadsHandler = rcloneExplorer.downloadsHandler;
      uploadsHandler = rcloneExplorer.uploadsHandler;
      miscContainer = rcloneExplorer.miscContainer;
      syncingHandler = rcloneExplorer.syncingHandler;
    }

    public string Execute(string command, string arguments, string operation = null, string prepend = null, string[] rcmdlist = null)
    {
      string output = "";
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
      process.StartInfo.EnvironmentVariables["RCLONE_CONFIG_PASS"] = iniSettings.Read("rcloneConfigPass");
      process.StartInfo.Arguments = "/c " + prepend + "rclone.exe " + command + " " + arguments + rcloneLogs;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardInput = true;
      process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
      process.Start();

      //log process ID for uploads, downloads and sync operations
      if (!String.IsNullOrEmpty(operation))
      {
        if (operation == "passcheck")
        {
            //check if config has password
            string pcout = "";
            while (!process.StandardOutput.EndOfStream)
            { pcout = process.StandardOutput.ReadLine(); }
            if (pcout.Contains("password:"))
            {
                rcloneExplorer.configEncrypted = true;
            }
        }
        else if (operation == "up")
        {
          //log the process in the uploading list
          uploadsHandler.uploadingPID.Add(new string[] { process.Id.ToString(), arguments });
        }
        else if (operation == "down")
        {
          //log the process in the downloading list
          downloadsHandler.downloadPID.Add(new string[] { process.Id.ToString(), arguments });
        }
        else if (operation == "sync")
        {
          //log the process in the syncing list
          syncingHandler.syncingPID = process.Id;
        }
        else if (operation == "config")
        {
          //iterate through the commands needed to set the remote up (its different per remote)
          foreach (string rcmd in rcmdlist)
          {
            //the remote setup has asked to show a message
            if (rcmd.Contains("MSG: "))
            {
              MessageBox.Show(rcmd);
            }
            //the remote setup has hasked to open a url
            else if (rcmd.Contains("OPN|"))
            {
              //grab the predefined regex
              string regexp = rcmd.Split('|')[1];
              String url = "";
              
              //iterate through stdout until find the matching url
              string stdoutline = process.StandardOutput.ReadLine();
              while (!Regex.Match(stdoutline, regexp).Success)
              {
                stdoutline = process.StandardOutput.ReadLine();
                url = Regex.Match(stdoutline, regexp).Value;
              }
              //open the url
              Process.Start(url);
            }
            //the remote setup has asked for some information
            else if (rcmd.Contains("REQ:"))
            {
              string requiredinput = PromptGenerator.ShowDialog(rcmd, "");
              process.StandardInput.WriteLine(requiredinput);
              // this doesnt seem to iterate so just stdin now https://i.imgur.com/x0ml8.png
              System.Threading.Thread.Sleep(100);
              process.StandardInput.WriteLine("y"); System.Threading.Thread.Sleep(100);
              process.StandardInput.WriteLine("q"); System.Threading.Thread.Sleep(100);
            }
            //the remote setup just wants to send some text
            else {
              process.StandardInput.WriteLine(rcmd);            
            }
            //sleep between operations
            System.Threading.Thread.Sleep(100);
          }
        }

      }

      // Synchronously read the standard output of the spawned process. 
      output = process.StandardOutput.ReadToEnd();
      if (output == null) { output = process.StandardError.ReadToEnd(); }   

      //close process when it's finished  
      process.WaitForExit();
      process.Close();

      //debugging
      if (rcmdlist != null && rcmdlist[1].Contains("debug")) { MessageBox.Show(output); }

      //return output
      return output;
    }

  }
}
