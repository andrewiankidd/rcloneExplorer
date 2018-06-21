using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListViewItem;

namespace rcloneExplorer
{
    public partial class mainForm : Form
    {
        private RcloneHandler rcloneHandler { get; set; } = new RcloneHandler();
        private FfplayHandler ffplayHandler { get; set; } = new FfplayHandler();
        private static WinAPIHelper winAPI { get; set; } = new WinAPIHelper();
        private UiHelper uiHelper {get; set;} = new UiHelper();
        private Timer ticker { get; set; } = new Timer();
        private string curAddress { get; set; } = string.Empty;
        private Tuple<bool, string> clipBoard { get; set; } = new Tuple<bool, string>(false, string.Empty);
        private int trackedTransfers = 0;


        public mainForm()
        {
            InitializeComponent();
            ticker.Tick += Ticker_Tick;
            ticker.Interval = (int)(TimeSpan.FromSeconds(3)).TotalMilliseconds;
            ticker.Start();
        }

        private void Ticker_Tick(object sender, EventArgs e)
        {
            var transfers = rcloneHandler.ListTransfers(curAddress);
            if (transfers.Count > 0)
            {
                lstBrowser.BeginUpdate();
                transfers.Where(t => lstBrowser.Items.ContainsKey(t.Name)).ToList().ForEach(t => {
                    lstBrowser.Items[t.Name].SubItems["Size"].Text = t.Progress;
                });
                lstBrowser.EndUpdate();
            }
            btnTransfers.Text = $"Active Transfers: {transfers.Count}";

            if (transfers.Count() < (trackedTransfers))
            {
                DisplayDirectory(rcloneHandler.ListDirectory(curAddress));
            }
            trackedTransfers = transfers.Count;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            // List root of remote
            DisplayDirectory(rcloneHandler.ListDirectory());
        }

        private void DisplayDirectory(Dictionary<string, List<rcloneObject>> dirInfo)
        {            
            var directory = dirInfo.Keys.First();
            var objects = dirInfo.Values.First();

            // Address Bar
            string[] pathCrumbs = directory.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            curAddress = directory;

            var buttonsToRemove = pnlCrumbBar.Controls.OfType<Button>().ToArray().ToList();
            buttonsToRemove.ForEach(btn => { if (!directory.Contains(btn.Tag.ToString())) { pnlCrumbBar.Controls.Remove(btn); } });

            int count = 0;
            foreach (string crumb in pathCrumbs)
            {
                int left = pnlCrumbBar.Controls.Count > 0 ? (pnlCrumbBar.Controls[pnlCrumbBar.Controls.Count-1].Right+5) : 0;
                if (count >= pnlCrumbBar.Controls.Count)
                {
                    Button b = new Button();
                    b.Left = left;
                    b.AutoSize = true;
                    b.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    b.Text = $"{crumb} 	▶";
                    b.Tag = directory;
                    b.Click += (sender, args) =>
                    {
                        DisplayDirectory(rcloneHandler.ListDirectory(directory));
                    };

                    pnlCrumbBar.Controls.Add(b);
                    left += (5 + b.Width);
                }
                count++;
            }

            // Navigation Tree
            var remotes = rcloneHandler.getRemoteList();
            remotes.Keys.ToList().Where(obj => !trNavPane.Nodes.ContainsKey($"{obj}:")).ToList().ForEach(remote =>
            {
                trNavPane.Nodes.Add($"{remote}:", remote);
            });
            var drives = System.IO.DriveInfo.GetDrives();
            drives.ToList().Where(obj => !trNavPane.Nodes.ContainsKey(obj.Name)).ToList().ForEach(drive =>
            {
                trNavPane.Nodes.Add(drive.Name, drive.Name);
            });

            // List View
            IList<PropertyInfo> properties = typeof(rcloneObject).GetProperties().ToList();
            ImageList imageList = new ImageList();
            lstBrowser.Items.Clear();
            lstBrowser.Columns.Clear();
            lstBrowser.LargeImageList = imageList;
            lstBrowser.SmallImageList = imageList;
            List<string> exclusions = new List<string>(){
                    "Name",
                    "Progress"
                };

            foreach (PropertyInfo p in properties.Where(p => !exclusions.Contains(p.Name)))
            {
                lstBrowser.Columns.Add(p.Name, p.Name);
            }

            foreach (rcloneObject o in objects)
            {
                ListViewItem i = null;
                
                foreach (PropertyInfo p in properties.Where(p => !exclusions.Contains(p.Name)))
                {
                    ListViewSubItem lvsi = new ListViewSubItem
                    {
                        Name = p.Name,
                        Text = p.GetValue(o).GetType().Equals(typeof(long)) ? uiHelper.GetBytesReadable((long)p.GetValue(o)): p.GetValue(o).ToString(),
                    };

                    if (i == null)
                    {
                        i = new ListViewItem();
                        i.SubItems.Add(lvsi);
                    }
                    else
                    {
                        i.SubItems.Add(lvsi);
                    }
                }
                i.Name = o.Name;
                i.Text = o.Name;
                i.ImageKey = o.IsDir ? "dir" : Path.GetExtension(o.Name);
                getImageKey(i.ImageKey, ref imageList);

                lstBrowser.Items.Add(i);
            }
            lstBrowser.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lstBrowser.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            // Text/Info label
            lblInfo.Text = $"Files: {objects.Where(x => !x.IsDir).Count()} Folders: {objects.Where(x => x.IsDir).Count()}";
        }

        private void getImageKey(string imageKey, ref ImageList imageList)
        {
            if (!imageList.Images.ContainsKey(imageKey))
            {
                var x = imageKey.Equals("dir") ? IconManager.GetLargeIcon(Environment.SystemDirectory, true, true) : IconManager.GetLargeIcon(imageKey, false, false);
                imageList.Images.Add(imageKey, x);
            }
        }

        private void lstBrowser_DoubleClick(object sender, EventArgs e)
        {
            var selectedItem = lstBrowser.SelectedItems[0];
            var remotePath = $@"{selectedItem.SubItems["BaseDir"].Text}\{selectedItem.Name}";

            // Check if selected element is directory
            if (Boolean.Parse(selectedItem.SubItems["isDir"].Text))
            {
                // List root of remote                
                DisplayDirectory(rcloneHandler.ListDirectory(remotePath));
            }
            else
            {
                // Cache and open file
                var tempFile = rcloneHandler.CacheFile(remotePath, selectedItem.SubItems["size"].Text);
                System.Diagnostics.Process.Start(tempFile);
            }
        }

        private void pnlCrumbBar_Click(object sender, EventArgs e)
        {
            TextBox tb = new TextBox();
            tb.Text = curAddress;
            tb.Width = pnlCrumbBar.Width;
            tb.Height = pnlCrumbBar.Height;
            tb.KeyDown += (s, a) =>
            {
                if (a.KeyCode == Keys.Enter)
                {
                    if (tb.Text != curAddress)
                    {
                        DisplayDirectory(rcloneHandler.ListDirectory(tb.Text));
                    }
                    tb.Dispose();
                }
            };
            tb.LostFocus += (s, a) =>
            {
                tb.Dispose();
            };
            pnlCrumbBar.Controls.Add(tb);
            tb.BringToFront();
        }

        private void lstBrowser_DragDrop(object sender, DragEventArgs e)
        {
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach( string filePath in droppedFiles)
            {
                FileAttributes attr = File.GetAttributes(filePath);
                bool isDir = attr.HasFlag(FileAttributes.Directory);

                if(!isDir)
                {
                    rcloneHandler.Upload(curAddress, filePath);
                    DisplayDirectory(rcloneHandler.ListDirectory(curAddress));
                }
                
            }
        }

        private void lstBrowser_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void btnToggleView_Click(object sender, EventArgs e)
        {
            // List of all available views
            List<View> lv = new List<View>(new[] { View.Details, View.LargeIcon, View.List, View.SmallIcon, View.Tile });

            // Currently displayed view
            int curIndex = lv.FindIndex(l => l.Equals(lstBrowser.View));

            // Go to next view or reset if there is no 'next' view
            lstBrowser.View = (curIndex+1).Equals(lv.Count) ? lv[0] : lv[curIndex + 1];

        }

        private void lstBrowser_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ctxtBrowser.Show(Cursor.Position);
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstBrowser.SelectedItems.Count > 0)
            {
                var selectedItemName = lstBrowser.SelectedItems[0].Name;
                var selectedItemBaseDir = lstBrowser.SelectedItems[0].SubItems["Basedir"].Text;

                rcloneHandler.KillTransfer(selectedItemBaseDir, selectedItemName);

                DisplayDirectory(rcloneHandler.ListDirectory(curAddress));
            }
              
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstBrowser.SelectedItems.Count > 0)
            {
                var selectedItemName = lstBrowser.SelectedItems[0].Name;
                var selectedItemBaseDir = lstBrowser.SelectedItems[0].SubItems["Basedir"].Text;
                var selectedItemIsDir = Boolean.Parse(lstBrowser.SelectedItems[0].SubItems["IsDir"].Text);

                bool shouldDelete = uiHelper.ConfirmPrompt($"Delete \"{selectedItemName}\"?");

                if (shouldDelete)
                {
                    if (selectedItemIsDir)
                    {
                        rcloneHandler.Purge(selectedItemBaseDir, selectedItemName);
                    }
                    else
                    {
                        rcloneHandler.Delete(selectedItemBaseDir, selectedItemName);
                    }
                    DisplayDirectory(rcloneHandler.ListDirectory(curAddress));
                }
            }             
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstBrowser.SelectedItems.Count > 0)
            {
                var selectedItemName = lstBrowser.SelectedItems[0].Name;
                var selectedItemBaseDir = lstBrowser.SelectedItems[0].SubItems["Basedir"].Text;

                var newName = uiHelper.InputPrompt($"Rename \"{selectedItemName}\"?", selectedItemName);
                if (!newName.Equals(selectedItemName))
                {
                    rcloneHandler.Rename(selectedItemBaseDir, selectedItemName, newName);
                    DisplayDirectory(rcloneHandler.ListDirectory(curAddress));
                }
            }            
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstBrowser.SelectedItems.Count>0)
            {
                var selectedItemName = lstBrowser.SelectedItems[0].Name;
                var selectedItemBaseDir = lstBrowser.SelectedItems[0].SubItems["Basedir"].Text;

                clipBoard = new Tuple<bool, string>(true, $@"{selectedItemBaseDir}\{selectedItemName}");
            }
         
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstBrowser.SelectedItems.Count > 0)
            {
                var selectedItemName = lstBrowser.SelectedItems[0].Name;
                var selectedItemBaseDir = lstBrowser.SelectedItems[0].SubItems["Basedir"].Text;

                clipBoard = new Tuple<bool, string>(false, $@"{selectedItemBaseDir}\{selectedItemName}");
            }       
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Move = clipBoard.Item1;
            var Name = clipBoard.Item2;
            
            if (System.Windows.Clipboard.ContainsFileDropList())
            {
                var filesToCopy = System.Windows.Clipboard.GetFileDropList();
                foreach (string f in filesToCopy)
                {
                    rcloneHandler.Copy(curAddress, f);
                }
                DisplayDirectory(rcloneHandler.ListDirectory(curAddress));
                System.Windows.Clipboard.Clear();
            }
            else if (!string.IsNullOrEmpty(Name))
            {
                if (Move)
                {
                    rcloneHandler.Move(curAddress, Name);
                }
                else
                {
                    rcloneHandler.Copy(curAddress, Name);
                }
                DisplayDirectory(rcloneHandler.ListDirectory(curAddress));
                clipBoard = new Tuple<bool, string>(false, string.Empty);
            }      
        }

        private void trNavPane_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selected = trNavPane.SelectedNode;
            DisplayDirectory(rcloneHandler.ListDirectory(selected.Name));
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstBrowser.SelectedItems.Count > 0)
            {
                var selectedItemName = lstBrowser.SelectedItems[0].Name;
                var selectedItemBaseDir = lstBrowser.SelectedItems[0].SubItems["Basedir"].Text;

                SaveFileDialog d = new SaveFileDialog();
                d.FileName = selectedItemName;
                var r = d.ShowDialog();

                if (r.Equals(DialogResult.OK))
                {
                    rcloneHandler.Download(selectedItemBaseDir, selectedItemName, d.FileName);
                }
            }
               
        }

        private void mainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.V))
            {
                pasteToolStripMenuItem_Click(sender, e);
            }
            else if(e.KeyData == (Keys.Control | Keys.C))
            {
                copyToolStripMenuItem_Click(sender, e);
            }
            else if (e.KeyData == (Keys.Control | Keys.X))
            {
                cutToolStripMenuItem_Click(sender, e);
            }
        }

        private void btnTransfers_Click(object sender, EventArgs e)
        {
            uiHelper.MsgBox(rcloneHandler.dumpTransfers());
        }

        private void streamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstBrowser.SelectedItems.Count > 0)
            {
                var selectedItemName = lstBrowser.SelectedItems[0].Name;
                var selectedItemBaseDir = lstBrowser.SelectedItems[0].SubItems["Basedir"].Text;
                var guid = Guid.NewGuid();

                var rclone = rcloneHandler.Cat($@"{selectedItemBaseDir}\{selectedItemName}");
                var ffplay = ffplayHandler.Stream(rclone, guid.ToString());

                var foundHandle = WinAPIHelper.getWindow(guid.ToString(), ffplay.Id);

                if (!foundHandle.Equals(new IntPtr(0)))
                {
                    Form streamUI = uiHelper.getStreamForm(foundHandle, ffplay.Id);
                    WinAPIHelper.SetParent(foundHandle, streamUI.Handle);
                }
                else
                {
                    uiHelper.MsgBox("Stream Failed or Media Type not supported.");
                }
            }
            
        }
        
      

    }
}
