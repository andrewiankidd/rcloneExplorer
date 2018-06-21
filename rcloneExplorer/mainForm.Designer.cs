namespace rcloneExplorer
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.lstBrowser = new System.Windows.Forms.ListView();
            this.trNavPane = new System.Windows.Forms.TreeView();
            this.pnlCrumbBar = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnToggleView = new System.Windows.Forms.Button();
            this.ctxtBrowser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTransfers = new System.Windows.Forms.Button();
            this.streamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxtBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstBrowser
            // 
            this.lstBrowser.AllowDrop = true;
            this.lstBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstBrowser.FullRowSelect = true;
            this.lstBrowser.Location = new System.Drawing.Point(149, 40);
            this.lstBrowser.MultiSelect = false;
            this.lstBrowser.Name = "lstBrowser";
            this.lstBrowser.Size = new System.Drawing.Size(523, 243);
            this.lstBrowser.TabIndex = 0;
            this.lstBrowser.UseCompatibleStateImageBehavior = false;
            this.lstBrowser.View = System.Windows.Forms.View.Details;
            this.lstBrowser.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstBrowser_DragDrop);
            this.lstBrowser.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstBrowser_DragEnter);
            this.lstBrowser.DoubleClick += new System.EventHandler(this.lstBrowser_DoubleClick);
            this.lstBrowser.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstBrowser_MouseClick);
            this.lstBrowser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstBrowser_MouseClick);
            // 
            // trNavPane
            // 
            this.trNavPane.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.trNavPane.Location = new System.Drawing.Point(12, 40);
            this.trNavPane.Name = "trNavPane";
            this.trNavPane.Size = new System.Drawing.Size(131, 243);
            this.trNavPane.TabIndex = 1;
            this.trNavPane.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trNavPane_AfterSelect);
            // 
            // pnlCrumbBar
            // 
            this.pnlCrumbBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCrumbBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCrumbBar.Location = new System.Drawing.Point(13, 15);
            this.pnlCrumbBar.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCrumbBar.Name = "pnlCrumbBar";
            this.pnlCrumbBar.Size = new System.Drawing.Size(659, 22);
            this.pnlCrumbBar.TabIndex = 2;
            this.pnlCrumbBar.Click += new System.EventHandler(this.pnlCrumbBar_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(12, 286);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(574, 13);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnToggleView
            // 
            this.btnToggleView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleView.Location = new System.Drawing.Point(650, 286);
            this.btnToggleView.Name = "btnToggleView";
            this.btnToggleView.Size = new System.Drawing.Size(22, 20);
            this.btnToggleView.TabIndex = 4;
            this.btnToggleView.UseVisualStyleBackColor = true;
            this.btnToggleView.Click += new System.EventHandler(this.btnToggleView_Click);
            // 
            // ctxtBrowser
            // 
            this.ctxtBrowser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.streamToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.cancelToolStripMenuItem,
            this.toolStripSeparator1,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.ctxtBrowser.Name = "ctxtBrowser";
            this.ctxtBrowser.Size = new System.Drawing.Size(156, 230);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.lstBrowser_DoubleClick);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.saveToolStripMenuItem.Text = "Save As...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.cancelToolStripMenuItem.Text = "Cancel Transfer";
            this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(152, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // btnTransfers
            // 
            this.btnTransfers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransfers.Location = new System.Drawing.Point(522, 286);
            this.btnTransfers.Name = "btnTransfers";
            this.btnTransfers.Size = new System.Drawing.Size(122, 20);
            this.btnTransfers.TabIndex = 5;
            this.btnTransfers.Text = "Active Transfers: 0";
            this.btnTransfers.UseVisualStyleBackColor = true;
            this.btnTransfers.Click += new System.EventHandler(this.btnTransfers_Click);
            // 
            // streamToolStripMenuItem
            // 
            this.streamToolStripMenuItem.Name = "streamToolStripMenuItem";
            this.streamToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.streamToolStripMenuItem.Text = "Stream";
            this.streamToolStripMenuItem.Click += new System.EventHandler(this.streamToolStripMenuItem_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 311);
            this.Controls.Add(this.btnTransfers);
            this.Controls.Add(this.btnToggleView);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.pnlCrumbBar);
            this.Controls.Add(this.trNavPane);
            this.Controls.Add(this.lstBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rcloneExplorer";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mainForm_KeyUp);
            this.ctxtBrowser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstBrowser;
        private System.Windows.Forms.TreeView trNavPane;
        private System.Windows.Forms.Panel pnlCrumbBar;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnToggleView;
        private System.Windows.Forms.ContextMenuStrip ctxtBrowser;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.Button btnTransfers;
        private System.Windows.Forms.ToolStripMenuItem streamToolStripMenuItem;
    }
}

