using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public partial class rcloneExplorerSetupWiz : Form
  {
    rcloneExplorerInternalExec internalExecHandler;
    rcloneExplorerExploreHandler exploreHandler; 
    IniFile iniSettings;
    string[] providers = { "crypt", "amazon cloud drive", "s3", "b2", "dropbox", "google cloud storage", "drive", "hubic", "onedrive", "swift", "yandex" };

    public rcloneExplorerSetupWiz()
    {
      InitializeComponent();
      internalExecHandler = rcloneExplorer.internalExecHandler;
      exploreHandler = rcloneExplorer.exploreHandler;
      iniSettings = rcloneExplorer.iniSettings;
      loadConfigs();
    }

    private void loadConfigs()
    {
      lstConfigs.Items.Clear();
      //get the output of config command, and echo q to quit it so the process doesnt hang
      string[] output = internalExecHandler.Execute("config", "", null, "echo q |").Split('\n');
      bool currentlyOnRemoteList = false;
      //iterate through each all the output line by line (inefficient)
      foreach (string str in output)
      {
        //rclone uses three equals signs to seperate header from output, so start reading rows after that
        if (str.Contains("==="))
        {
          currentlyOnRemoteList = true;
        }
        //rclone has a blank row after all the remotes are listed, so stop reading there
        else if (currentlyOnRemoteList && str == "")
        {
          currentlyOnRemoteList = false;
        }
        else if (currentlyOnRemoteList)
        {
          //iterate through the list of known providers
          foreach (string provider in providers)
          {
            //really hacky way to single out the provider column
            if (str.Contains("  " + provider))
            {
              //remove the remote provider name and trim the whitespace between the columns
              string remotename = str.Replace("  " + provider, "").Trim();
              //we've already identified that the string contains the current provider being iterated, so store it
              string remoteprovider = provider;
              //add this entry to the list and break
              lstConfigs.Items.Add(new ListViewItem(new string[] { remotename, remoteprovider }));
              break;
            }
          }
        }
      }
    }

    private void openSelectedRemote()
    {
      if (lstConfigs.SelectedItems.Count > 0 && !String.IsNullOrEmpty(lstConfigs.SelectedItems[0].Text))
      {
        //write newly selected config to ini
        iniSettings.Write("rcloneRemote", lstConfigs.SelectedItems[0].Text.ToString());
        //not doing first time setup so clear the CD and refresh view   
        if (!rcloneExplorer.initialSetup) {
          //set current path to root
          rcloneExplorer.remoteCD = "";
          //refresh the explorer lst
          exploreHandler.refreshlstExplorer();
          }
        //close config screen
        this.Close();
      }
    }

    private void lstConfigs_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      openSelectedRemote();
    }

    private void btnOpenSelectedRemote_Click(object sender, EventArgs e)
    {
      openSelectedRemote();
    }

    private void btnAddNewRemote_Click(object sender, EventArgs e)
    {
      cmbProviderList.Items.Clear();
      cmbProviderList.Items.AddRange(providers);
      cmbProviderList.Visible = true;
      cmbProviderList.DroppedDown = true;
    }

    private void btnDeleteRemote_Click(object sender, EventArgs e)
    {
      if (lstConfigs.SelectedItems.Count > 0)
      {
        MessageBox.Show("Not yet implemented");
      }
    }

    private void btnEditRemote_Click(object sender, EventArgs e)
    {
      if (lstConfigs.SelectedItems.Count > 0)
      {
        MessageBox.Show("Not yet implemented");
      }
    }

    private void btnRefreshConfigList_Click(object sender, EventArgs e)
    {
      loadConfigs();
    }

    private void btnEncryptRemote_Click(object sender, EventArgs e)
    {
      if (lstConfigs.SelectedItems.Count > 0)
      {
        MessageBox.Show("Not yet implemented :)");
      }
    }

    private void cmbProviderList_TextChanged(object sender, EventArgs e)
    {
      //set remotename and remoteprovider
      string nRemoteName = "";
      string nRemoteProvider = cmbProviderList.Text;
      string[] rcmdlist = { };

      //hide the combobox cos it's no longer needed
      cmbProviderList.Visible = false;

      //if user doesnt enter a valid remote name, keep asking
      while (String.IsNullOrEmpty(nRemoteName.Trim()))
      {
        nRemoteName = PromptGenerator.ShowDialog("Remote Name:", "Please enter an alphanumeric name for the remote");
      }

      //build command list
      if (nRemoteProvider == "amazon cloud drive")
      {
        rcmdlist = new[] { "n", nRemoteName, "1", "\r\n", "\r\n", "y", "MSG: Please Accept the oauth req in your browser before continuing", "y", "q" };
      }
      else if (nRemoteProvider == "dropbox")
      {
        rcmdlist = new[] { "n", nRemoteName, "4", "\r\n", "\r\n", @"OPN|https\:\/\/www\.dropbox(.*?)response_type=code", "REQ:Dropbox Code", "y", "q", "q" };
      }
      else if(nRemoteProvider == "drive")
      {
        rcmdlist = new[] { "n", nRemoteName, "7", "\r\n", "\r\n", "y", "MSG: Please Accept the oauth req in your browser before continuing", "y", "q" };
      }
      else if (nRemoteProvider == "hubic")
      {
        rcmdlist = new[] { "n", nRemoteName, "8", "\r\n", "\r\n", "y", "MSG: Please Accept the oauth req in your browser before continuing", "y", "q" };
      } 
      else if (nRemoteProvider == "onedrive")
      {
        rcmdlist = new[] { "n", nRemoteName, "10", "\r\n", "\r\n", "y", "MSG: Please Accept the oauth req in your browser before continuing", "y", "q" };
      }
      else if (nRemoteProvider == "yandex")
      {
        rcmdlist = new[] { "n", nRemoteName, "12", "\r\n", "\r\n", "y", "MSG: Please Accept the oauth req in your browser before continuing", "y", "q" };
      }
      else
      {
        MessageBox.Show("Currently this remote is not supported, please try one of the following:\r\n\r\nACD, dropbox, drive, hubic, onedrive, yandex");
        return;
      }

      //tell user whats happening
      MessageBox.Show("Making a new '" + nRemoteProvider + "' config, called '" + nRemoteName + "'");

      //run command
      internalExecHandler.Execute("config", "", "config", null, rcmdlist);

      //refresh list view
      loadConfigs();
    }

  }
}
