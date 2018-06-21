using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rcloneExplorer
{
    class UiHelper
    {
        public bool ConfirmPrompt(string text)
        {
            bool result = false;

            // Build form
            Form f = new Form();
            f.FormBorderStyle = FormBorderStyle.FixedSingle;
            f.StartPosition = FormStartPosition.CenterParent;
            f.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            f.TopMost = true;
            f.Text = text;

            Button tB = new Button();
            tB.Text = "Yes";
            tB.Top = 0;
            tB.Width = f.Width;
            tB.Click += (s, e) =>
            {
                result = true;
                f.Close();
            };

            Button fB = new Button();
            fB.Text = "No";
            fB.Top = tB.Bottom;
            fB.Width = f.Width;
            fB.Click += (s, e) =>
            {
                result = false;
                f.Close();
            };

            f.Height = (tB.Height * 4);
            f.Controls.Add(tB);
            f.Controls.Add(fB);

            f.ShowDialog();

            return result;
        }

        public string InputPrompt(string text, string suggested = "")
        {
            return Microsoft.VisualBasic.Interaction.InputBox(text, "rcloneExplorer: Prompt", suggested, -1, -1);
        }

        private void resizeStream(IntPtr handle, Form f)
        {
            WinAPIHelper.MoveWindow(handle, 0, 0, f.ClientSize.Width, f.ClientSize.Height - 20, true);
        }

        public string ComboPrompt(List<string> choices)
        {
            string selectedOption = string.Empty;

            if (choices.Count > 1)
            {
                // Build ComboBox form
                Form f = new Form();
                f.FormBorderStyle = FormBorderStyle.FixedSingle;
                f.StartPosition = FormStartPosition.CenterParent;
                f.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
                f.TopMost = true;
                f.Text = "Select Remote";

                ComboBox c = new ComboBox();
                c.DropDownStyle = ComboBoxStyle.DropDownList;
                c.Left = 10;
                c.Width = (f.ClientSize.Width - 20);
                c.Top = 10;

                Button b = new Button();
                b.Left = c.Left;
                b.Width = c.Width;
                b.Top = c.Bottom;
                b.Text = "OK";
                b.Click += (args, sender) =>
                {
                    f.Close();
                };

                f.Height = ((f.Height - f.ClientSize.Height) + (c.Height + b.Height + 20));
                f.Controls.Add(c);
                f.Controls.Add(b);

                // Bind remote list to combobox
                c.DataSource = new BindingSource(choices, null);

                // Show ComboBox form to user
                f.ShowDialog();
                

                // Take selected value as selectedOption
                selectedOption = c.Text;

            }
            else if (choices.Count == 1)
            {
                selectedOption = choices.First();
            }
            else
            {
                throw new Exception("No Remotes found");
            }

            return selectedOption;
        }

        // Returns the human-readable file size for an arbitrary, 64-bit file size 
        // The default format is "0.### XB", e.g. "4.2 KB" or "1.434 GB"
        public string GetBytesReadable(long i)
        {
            // Get absolute value
            long absolute_i = (i < 0 ? -i : i);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (i >> 50);
            }
            else if (absolute_i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (i >> 40);
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (i >> 30);
            }
            else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (i >> 20);
            }
            else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (i >> 10);
            }
            else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }
            else
            {
                return i.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.### ") + suffix;
        }

        public void MsgBox(string text)
        {
            MessageBox.Show(text);
        }

        internal Form getStreamForm(IntPtr handle, int pid)
        {
            Form streamUI = new Form();
            streamUI.Resize += (s, ev) =>
            {
                resizeStream(handle, streamUI);
            };
            streamUI.Shown += (s, ev) =>
            {
                resizeStream(handle, streamUI);
            };
            streamUI.FormClosed += (s, ev) =>
            {
                Program.KillProcessAndChildren(pid);
            };
            Button pauseplay = new Button();
            pauseplay.Click += (s, ev) =>
            {
                WinAPIHelper.SendInput(handle, Keys.P);
            };
            pauseplay.Top = streamUI.ClientSize.Height - 20;
            pauseplay.Height = 20;
            pauseplay.Width = streamUI.ClientSize.Width;
            pauseplay.Text = "Pause/Play";
            pauseplay.Anchor = AnchorStyles.Bottom;
            streamUI.Controls.Add(pauseplay);
            streamUI.Show();

            return streamUI;
        }
    }
}
