using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rcloneExplorer
{
    public partial class rcloneExplorerSettings : Form
    {
        IniFile iniSettings;
        public rcloneExplorerSettings()
        {
            InitializeComponent();
            iniSettings = rcloneExplorer.iniSettings;
        }

        private void rcloneExplorerSettings_Load(object sender, EventArgs e)
        {
            getAssocs();
        }

        private void getAssocs()
        {
            string[] definedFiletypes = iniSettings.Read("definedFiletypes").Split(',');
            foreach (string key in definedFiletypes)
            {
                string value = iniSettings.Read(key, "FTA");
                string[] item = { key, value };
                lstSettingsFiletypeAssociation.Items.Add(new ListViewItem(item));
            }
        }

        private void btnNewFileAssoc_Click(object sender, EventArgs e)
        {
            string extension = PromptGenerator.ShowDialog("Extension (with dot):", "New Association");
            string assoc = PromptGenerator.ShowDialog("Command:", "New Association");
            string existingDefinedFiletypes = iniSettings.Read("definedFiletypes");
            iniSettings.Write("definedFiletypes", existingDefinedFiletypes + "," + extension);
            iniSettings.Write(extension, assoc, "FTA");
            getAssocs();
        }
    }
}
