namespace rcloneExplorer
{
  partial class rcloneExplorerSetupWiz
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
      this.lstConfigs = new System.Windows.Forms.ListView();
      this.colRemotes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colRemoteType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SuspendLayout();
      // 
      // lstConfigs
      // 
      this.lstConfigs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstConfigs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRemotes,
            this.colRemoteType});
      this.lstConfigs.FullRowSelect = true;
      this.lstConfigs.Location = new System.Drawing.Point(12, 12);
      this.lstConfigs.Name = "lstConfigs";
      this.lstConfigs.Size = new System.Drawing.Size(920, 477);
      this.lstConfigs.TabIndex = 1;
      this.lstConfigs.UseCompatibleStateImageBehavior = false;
      this.lstConfigs.View = System.Windows.Forms.View.Details;
      this.lstConfigs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstConfigs_MouseDoubleClick);
      // 
      // colRemotes
      // 
      this.colRemotes.Text = "Remote";
      this.colRemotes.Width = 500;
      // 
      // colRemoteType
      // 
      this.colRemoteType.Text = "Type";
      // 
      // rcloneExplorerSetupWiz
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(944, 501);
      this.Controls.Add(this.lstConfigs);
      this.Name = "rcloneExplorerSetupWiz";
      this.Text = "rcloneExplorer Config";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lstConfigs;
    private System.Windows.Forms.ColumnHeader colRemotes;
    private System.Windows.Forms.ColumnHeader colRemoteType;
  }
}