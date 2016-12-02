using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public partial class rcloneExplorerSetupWiz : Form
  {
    rcloneExplorerInternalExec internalExecHandler;
    rcloneExplorerExploreHandler exploreHandler;
    IniFile iniSettings;
    string[] providers = { "crypt", "amazon cloud drive", "s3", "b2", "dropbox", "google cloud storage", "drive", "hubic", "local", "onedrive", "swift", "yandex" };

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
      string[] output = internalExecHandler.Execute("config", "", null, "echo q |").Split('\n');
      bool currentlyOnRemoteList = false;
      foreach (string str in output)
      {
        if (str.Contains("==="))
        {
          currentlyOnRemoteList = true;
        }
        else if (currentlyOnRemoteList && str == "")
        {
          currentlyOnRemoteList = false;
        }
        else if (currentlyOnRemoteList)
        {
          foreach (string provider in providers)
          {
            if (str.Contains("  " + provider))
            {
              string remotename = str.Replace("  " + provider, "").Trim();
              string remoteprovider = provider;
              lstConfigs.Items.Add(new ListViewItem(new string[] { remotename, remoteprovider }));
              break;
            }
          }
        }
      }
    }

    private void lstConfigs_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      iniSettings.Write("rcloneRemote", lstConfigs.SelectedItems[0].Text.ToString());
      rcloneExplorer.remoteCD = "";
      exploreHandler.refreshlstExplorer();
      this.Close();
    }
  }
}
