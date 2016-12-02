using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public class PromptGenerator : rcloneExplorer
  {
    //http://stackoverflow.com/a/5427121
    public static string ShowDialog(string text, string caption)
    {
      if (string.IsNullOrEmpty(caption)) { caption = text; text = ""; }
      Form prompt = new Form()
      {
        MaximumSize = new System.Drawing.Size(450, 110),
        MinimumSize = new System.Drawing.Size(450, 110),
        StartPosition = FormStartPosition.CenterScreen,    
        FormBorderStyle = FormBorderStyle.FixedDialog,
        MaximizeBox = false, MinimizeBox = false,
        TopMost = true,
        Text = caption
      };
      Label textLabel = new Label() { Left = 8, Top = 10, Text = text };
      TextBox textBox = new TextBox() { Left = 10, Top = 25, Width = 415 };
      Button confirmation = new Button() { Text = "OK", Left = 376, Width = 50, Top = 45, DialogResult = DialogResult.OK };
      confirmation.Click += (sender, e) => { prompt.Close(); };
      prompt.Controls.Add(textBox);
      prompt.Controls.Add(confirmation);
      prompt.Controls.Add(textLabel);
      prompt.AcceptButton = confirmation;

      return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
    }
  }
}
