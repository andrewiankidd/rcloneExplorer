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
    static string output = "";
    static string errOutput = "";

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
      output = null; errOutput = null;
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
      process.OutputDataReceived += (sender, args) => proc_OutputDataReceived(sender, args);
      process.ErrorDataReceived += (sender, args) => proc_ErrorDataReceived(sender, args);
      process.Start();
      process.BeginOutputReadLine();
      process.BeginErrorReadLine();

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
          uploadsHandler.uploadingPID.Add(new string[] { process.Id.ToString(), "0%" });
          
          int id = uploadsHandler.uploadingPID.Count - 1;
          string percentage = "0%";
          while (!process.HasExited)
          {
            if (errOutput != null) {  string newpercentage = Regex.Match(errOutput, @"\d+(?=%)% done", RegexOptions.RightToLeft).Value; if (newpercentage!="") { percentage = newpercentage; }}
            uploadsHandler.uploadingPID[id] = new string[] { process.Id.ToString(), percentage };
          }
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
      else { process.WaitForExit(); } 
      
      //return output
      if (String.IsNullOrEmpty(output)) { output = ""; }
      return output.Replace("\r", "");
    }
    //process stdout
    static void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
        {
            output += e.Data.ToString() + Environment.NewLine;
            Console.Write(e.Data.ToString());
        }
    }
    //process stderr
    static void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
        {
            errOutput += e.Data.ToString() + Environment.NewLine;
            Console.Write(e.Data.ToString());
        }
    }

  }
    
}
