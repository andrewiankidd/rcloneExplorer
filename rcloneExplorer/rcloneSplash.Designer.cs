namespace rcloneExplorer
{
  partial class rcloneSplash
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.lblSplashTitle = new System.Windows.Forms.Label();
      this.lblSplashText = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lblSplashTitle
      // 
      this.lblSplashTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
      this.lblSplashTitle.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblSplashTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblSplashTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
      this.lblSplashTitle.Location = new System.Drawing.Point(0, 0);
      this.lblSplashTitle.Name = "lblSplashTitle";
      this.lblSplashTitle.Size = new System.Drawing.Size(300, 300);
      this.lblSplashTitle.TabIndex = 0;
      this.lblSplashTitle.Text = "Loading";
      this.lblSplashTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblSplashText
      // 
      this.lblSplashText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
      this.lblSplashText.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lblSplashText.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F);
      this.lblSplashText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
      this.lblSplashText.Location = new System.Drawing.Point(0, 213);
      this.lblSplashText.Name = "lblSplashText";
      this.lblSplashText.Size = new System.Drawing.Size(300, 87);
      this.lblSplashText.TabIndex = 1;
      this.lblSplashText.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      // 
      // rcloneSplash
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(300, 300);
      this.Controls.Add(this.lblSplashText);
      this.Controls.Add(this.lblSplashTitle);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "rcloneSplash";
      this.Text = "rcloneSplash";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblSplashTitle;
    private System.Windows.Forms.Label lblSplashText;
  }
}