namespace rcloneExplorer
{
  partial class rcloneExplorer
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rcloneExplorer));
      this.lstExplorer = new System.Windows.Forms.ListView();
      this.colfileBytes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colfileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colfilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.txtRawOut = new System.Windows.Forms.TextBox();
      this.lblFooter = new System.Windows.Forms.Label();
      this.menuStrip = new System.Windows.Forms.MenuStrip();
      this.menuStripFile = new System.Windows.Forms.ToolStripMenuItem();
      this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.quitKillTransfersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStripView = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStripToggleConsole = new System.Windows.Forms.ToolStripMenuItem();
      this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabMainUI = new System.Windows.Forms.TabControl();
      this.tabRemote = new System.Windows.Forms.TabPage();
      this.tabDownloads = new System.Windows.Forms.TabPage();
      this.lstDownloads = new System.Windows.Forms.ListView();
      this.colDProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colDPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.tabUploads = new System.Windows.Forms.TabPage();
      this.lstUploads = new System.Windows.Forms.ListView();
      this.colUProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colUPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ctxtDownloadContext = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.ctxtDownloadContext_Cancel = new System.Windows.Forms.ToolStripMenuItem();
      this.ctxtUploadContext = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip.SuspendLayout();
      this.tabMainUI.SuspendLayout();
      this.tabRemote.SuspendLayout();
      this.tabDownloads.SuspendLayout();
      this.tabUploads.SuspendLayout();
      this.ctxtDownloadContext.SuspendLayout();
      this.ctxtUploadContext.SuspendLayout();
      this.SuspendLayout();
      // 
      // lstExplorer
      // 
      this.lstExplorer.AllowDrop = true;
      this.lstExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstExplorer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colfileBytes,
            this.colfileSize,
            this.colModified,
            this.colfilePath});
      this.lstExplorer.FullRowSelect = true;
      this.lstExplorer.Location = new System.Drawing.Point(0, 0);
      this.lstExplorer.Name = "lstExplorer";
      this.lstExplorer.Size = new System.Drawing.Size(559, 293);
      this.lstExplorer.TabIndex = 0;
      this.lstExplorer.UseCompatibleStateImageBehavior = false;
      this.lstExplorer.View = System.Windows.Forms.View.Details;
      this.lstExplorer.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstExplorer_DragDrop);
      this.lstExplorer.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstExplorer_DragEnter);
      this.lstExplorer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstExplorer_MouseDoubleClick);
      // 
      // colfileBytes
      // 
      this.colfileBytes.Text = "File Size (Bytes)";
      this.colfileBytes.Width = 70;
      // 
      // colfileSize
      // 
      this.colfileSize.Text = "File Size";
      // 
      // colModified
      // 
      this.colModified.Text = "Modified";
      // 
      // colfilePath
      // 
      this.colfilePath.Text = "File Path";
      // 
      // txtRawOut
      // 
      this.txtRawOut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtRawOut.Location = new System.Drawing.Point(12, 287);
      this.txtRawOut.Multiline = true;
      this.txtRawOut.Name = "txtRawOut";
      this.txtRawOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.txtRawOut.Size = new System.Drawing.Size(568, 0);
      this.txtRawOut.TabIndex = 1;
      // 
      // lblFooter
      // 
      this.lblFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblFooter.AutoSize = true;
      this.lblFooter.BackColor = System.Drawing.SystemColors.Control;
      this.lblFooter.Cursor = System.Windows.Forms.Cursors.Default;
      this.lblFooter.Location = new System.Drawing.Point(9, 350);
      this.lblFooter.Name = "lblFooter";
      this.lblFooter.Size = new System.Drawing.Size(0, 13);
      this.lblFooter.TabIndex = 2;
      // 
      // menuStrip
      // 
      this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripFile,
            this.menuStripView});
      this.menuStrip.Location = new System.Drawing.Point(0, 0);
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new System.Drawing.Size(592, 25);
      this.menuStrip.TabIndex = 3;
      this.menuStrip.Text = "menuStrip";
      // 
      // menuStripFile
      // 
      this.menuStripFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem,
            this.quitKillTransfersToolStripMenuItem});
      this.menuStripFile.Name = "menuStripFile";
      this.menuStripFile.Size = new System.Drawing.Size(39, 21);
      this.menuStripFile.Text = "File";
      // 
      // quitToolStripMenuItem
      // 
      this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
      this.quitToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
      this.quitToolStripMenuItem.Text = "Quit (Continue Transfers)";
      this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
      // 
      // quitKillTransfersToolStripMenuItem
      // 
      this.quitKillTransfersToolStripMenuItem.Name = "quitKillTransfersToolStripMenuItem";
      this.quitKillTransfersToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
      this.quitKillTransfersToolStripMenuItem.Text = "Quit";
      this.quitKillTransfersToolStripMenuItem.Click += new System.EventHandler(this.quitKillTransfersToolStripMenuItem_Click);
      // 
      // menuStripView
      // 
      this.menuStripView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripToggleConsole,
            this.refreshToolStripMenuItem});
      this.menuStripView.Name = "menuStripView";
      this.menuStripView.Size = new System.Drawing.Size(46, 21);
      this.menuStripView.Text = "View";
      // 
      // menuStripToggleConsole
      // 
      this.menuStripToggleConsole.Name = "menuStripToggleConsole";
      this.menuStripToggleConsole.Size = new System.Drawing.Size(162, 22);
      this.menuStripToggleConsole.Text = "Toggle Console";
      this.menuStripToggleConsole.Click += new System.EventHandler(this.menuStripToggleConsole_Click);
      // 
      // refreshToolStripMenuItem
      // 
      this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
      this.refreshToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
      this.refreshToolStripMenuItem.Text = "Refresh";
      this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
      // 
      // tabMainUI
      // 
      this.tabMainUI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabMainUI.Controls.Add(this.tabRemote);
      this.tabMainUI.Controls.Add(this.tabDownloads);
      this.tabMainUI.Controls.Add(this.tabUploads);
      this.tabMainUI.Location = new System.Drawing.Point(13, 28);
      this.tabMainUI.Name = "tabMainUI";
      this.tabMainUI.SelectedIndex = 0;
      this.tabMainUI.Size = new System.Drawing.Size(567, 319);
      this.tabMainUI.TabIndex = 4;
      // 
      // tabRemote
      // 
      this.tabRemote.Controls.Add(this.lstExplorer);
      this.tabRemote.Location = new System.Drawing.Point(4, 22);
      this.tabRemote.Name = "tabRemote";
      this.tabRemote.Padding = new System.Windows.Forms.Padding(3);
      this.tabRemote.Size = new System.Drawing.Size(559, 293);
      this.tabRemote.TabIndex = 0;
      this.tabRemote.Text = "Remote";
      this.tabRemote.UseVisualStyleBackColor = true;
      // 
      // tabDownloads
      // 
      this.tabDownloads.Controls.Add(this.lstDownloads);
      this.tabDownloads.Location = new System.Drawing.Point(4, 22);
      this.tabDownloads.Name = "tabDownloads";
      this.tabDownloads.Padding = new System.Windows.Forms.Padding(3);
      this.tabDownloads.Size = new System.Drawing.Size(559, 293);
      this.tabDownloads.TabIndex = 1;
      this.tabDownloads.Text = "Downloads";
      this.tabDownloads.UseVisualStyleBackColor = true;
      // 
      // lstDownloads
      // 
      this.lstDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstDownloads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDProgress,
            this.colDPath});
      this.lstDownloads.FullRowSelect = true;
      this.lstDownloads.Location = new System.Drawing.Point(0, 0);
      this.lstDownloads.Name = "lstDownloads";
      this.lstDownloads.Size = new System.Drawing.Size(559, 309);
      this.lstDownloads.TabIndex = 0;
      this.lstDownloads.UseCompatibleStateImageBehavior = false;
      this.lstDownloads.View = System.Windows.Forms.View.Details;
      this.lstDownloads.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstDownloads_MouseClick);
      // 
      // colDProgress
      // 
      this.colDProgress.Text = "Progress";
      // 
      // colDPath
      // 
      this.colDPath.Text = "Path";
      // 
      // tabUploads
      // 
      this.tabUploads.Controls.Add(this.lstUploads);
      this.tabUploads.Location = new System.Drawing.Point(4, 22);
      this.tabUploads.Name = "tabUploads";
      this.tabUploads.Padding = new System.Windows.Forms.Padding(3);
      this.tabUploads.Size = new System.Drawing.Size(559, 293);
      this.tabUploads.TabIndex = 2;
      this.tabUploads.Text = "Uploads";
      this.tabUploads.UseVisualStyleBackColor = true;
      // 
      // lstUploads
      // 
      this.lstUploads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstUploads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUProgress,
            this.colUPath});
      this.lstUploads.FullRowSelect = true;
      this.lstUploads.Location = new System.Drawing.Point(-4, 0);
      this.lstUploads.Name = "lstUploads";
      this.lstUploads.Size = new System.Drawing.Size(563, 297);
      this.lstUploads.TabIndex = 1;
      this.lstUploads.UseCompatibleStateImageBehavior = false;
      this.lstUploads.View = System.Windows.Forms.View.Details;
      this.lstUploads.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstUploads_MouseClick);
      // 
      // colUProgress
      // 
      this.colUProgress.Text = "Progress";
      // 
      // colUPath
      // 
      this.colUPath.Text = "Path";
      // 
      // ctxtDownloadContext
      // 
      this.ctxtDownloadContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxtDownloadContext_Cancel});
      this.ctxtDownloadContext.Name = "ctxtDownloadContext";
      this.ctxtDownloadContext.Size = new System.Drawing.Size(115, 26);
      // 
      // ctxtDownloadContext_Cancel
      // 
      this.ctxtDownloadContext_Cancel.Name = "ctxtDownloadContext_Cancel";
      this.ctxtDownloadContext_Cancel.Size = new System.Drawing.Size(114, 22);
      this.ctxtDownloadContext_Cancel.Text = "Cancel";
      this.ctxtDownloadContext_Cancel.Click += new System.EventHandler(this.ctxtDownloadContext_Cancel_Click);
      // 
      // ctxtUploadContext
      // 
      this.ctxtUploadContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cancelToolStripMenuItem});
      this.ctxtUploadContext.Name = "ctxtUploadContext";
      this.ctxtUploadContext.Size = new System.Drawing.Size(115, 26);
      // 
      // cancelToolStripMenuItem
      // 
      this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
      this.cancelToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
      this.cancelToolStripMenuItem.Text = "Cancel";
      this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
      // 
      // rcloneExplorer
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(592, 372);
      this.Controls.Add(this.tabMainUI);
      this.Controls.Add(this.lblFooter);
      this.Controls.Add(this.txtRawOut);
      this.Controls.Add(this.menuStrip);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip;
      this.Name = "rcloneExplorer";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "rcloneExplorer";
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
      this.tabMainUI.ResumeLayout(false);
      this.tabRemote.ResumeLayout(false);
      this.tabDownloads.ResumeLayout(false);
      this.tabUploads.ResumeLayout(false);
      this.ctxtDownloadContext.ResumeLayout(false);
      this.ctxtUploadContext.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView lstExplorer;
    private System.Windows.Forms.TextBox txtRawOut;
    private System.Windows.Forms.Label lblFooter;
    private System.Windows.Forms.MenuStrip menuStrip;
    private System.Windows.Forms.ToolStripMenuItem menuStripFile;
    private System.Windows.Forms.ToolStripMenuItem menuStripView;
    private System.Windows.Forms.ToolStripMenuItem menuStripToggleConsole;
    private System.Windows.Forms.ColumnHeader colfileSize;
    private System.Windows.Forms.ColumnHeader colfilePath;
    private System.Windows.Forms.ColumnHeader colfileBytes;
    private System.Windows.Forms.TabControl tabMainUI;
    private System.Windows.Forms.TabPage tabRemote;
    private System.Windows.Forms.TabPage tabDownloads;
    private System.Windows.Forms.ListView lstDownloads;
    private System.Windows.Forms.ColumnHeader colDProgress;
    private System.Windows.Forms.ColumnHeader colDPath;
    private System.Windows.Forms.ColumnHeader colModified;
    private System.Windows.Forms.ContextMenuStrip ctxtDownloadContext;
    private System.Windows.Forms.ToolStripMenuItem ctxtDownloadContext_Cancel;
    private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem quitKillTransfersToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    private System.Windows.Forms.TabPage tabUploads;
    private System.Windows.Forms.ListView lstUploads;
    private System.Windows.Forms.ColumnHeader colUProgress;
    private System.Windows.Forms.ColumnHeader colUPath;
    private System.Windows.Forms.ContextMenuStrip ctxtUploadContext;
    private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
  }
}

