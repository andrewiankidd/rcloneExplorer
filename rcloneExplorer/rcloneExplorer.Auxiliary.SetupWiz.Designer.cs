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
      this.grpBrowseRemotes = new System.Windows.Forms.GroupBox();
      this.cmbProviderList = new System.Windows.Forms.ComboBox();
      this.btnOpenSelectedRemote = new System.Windows.Forms.Button();
      this.btnEditRemote = new System.Windows.Forms.Button();
      this.btnDeleteRemote = new System.Windows.Forms.Button();
      this.btnAddNewRemote = new System.Windows.Forms.Button();
      this.grpBrowseRemotes.SuspendLayout();
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
      this.lstConfigs.Location = new System.Drawing.Point(6, 19);
      this.lstConfigs.Name = "lstConfigs";
      this.lstConfigs.Size = new System.Drawing.Size(317, 362);
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
      // grpBrowseRemotes
      // 
      this.grpBrowseRemotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpBrowseRemotes.Controls.Add(this.cmbProviderList);
      this.grpBrowseRemotes.Controls.Add(this.btnOpenSelectedRemote);
      this.grpBrowseRemotes.Controls.Add(this.btnEditRemote);
      this.grpBrowseRemotes.Controls.Add(this.btnDeleteRemote);
      this.grpBrowseRemotes.Controls.Add(this.btnAddNewRemote);
      this.grpBrowseRemotes.Controls.Add(this.lstConfigs);
      this.grpBrowseRemotes.Location = new System.Drawing.Point(12, 12);
      this.grpBrowseRemotes.Name = "grpBrowseRemotes";
      this.grpBrowseRemotes.Size = new System.Drawing.Size(560, 387);
      this.grpBrowseRemotes.TabIndex = 2;
      this.grpBrowseRemotes.TabStop = false;
      // 
      // cmbProviderList
      // 
      this.cmbProviderList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cmbProviderList.FormattingEnabled = true;
      this.cmbProviderList.Location = new System.Drawing.Point(329, 20);
      this.cmbProviderList.Name = "cmbProviderList";
      this.cmbProviderList.Size = new System.Drawing.Size(221, 21);
      this.cmbProviderList.TabIndex = 7;
      this.cmbProviderList.Visible = false;
      this.cmbProviderList.TextChanged += new System.EventHandler(this.cmbProviderList_TextChanged);
      // 
      // btnOpenSelectedRemote
      // 
      this.btnOpenSelectedRemote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOpenSelectedRemote.Location = new System.Drawing.Point(329, 47);
      this.btnOpenSelectedRemote.Name = "btnOpenSelectedRemote";
      this.btnOpenSelectedRemote.Size = new System.Drawing.Size(220, 23);
      this.btnOpenSelectedRemote.TabIndex = 6;
      this.btnOpenSelectedRemote.Text = "Open Selected Remote";
      this.btnOpenSelectedRemote.UseVisualStyleBackColor = true;
      this.btnOpenSelectedRemote.Click += new System.EventHandler(this.btnOpenSelectedRemote_Click);
      // 
      // btnEditRemote
      // 
      this.btnEditRemote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnEditRemote.Location = new System.Drawing.Point(329, 105);
      this.btnEditRemote.Name = "btnEditRemote";
      this.btnEditRemote.Size = new System.Drawing.Size(220, 23);
      this.btnEditRemote.TabIndex = 4;
      this.btnEditRemote.Text = "Edit Selected Remote";
      this.btnEditRemote.UseVisualStyleBackColor = true;
      this.btnEditRemote.Click += new System.EventHandler(this.btnEditRemote_Click);
      // 
      // btnDeleteRemote
      // 
      this.btnDeleteRemote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDeleteRemote.Location = new System.Drawing.Point(329, 76);
      this.btnDeleteRemote.Name = "btnDeleteRemote";
      this.btnDeleteRemote.Size = new System.Drawing.Size(220, 23);
      this.btnDeleteRemote.TabIndex = 3;
      this.btnDeleteRemote.Text = "Delete Selected Remote";
      this.btnDeleteRemote.UseVisualStyleBackColor = true;
      this.btnDeleteRemote.Click += new System.EventHandler(this.btnDeleteRemote_Click);
      // 
      // btnAddNewRemote
      // 
      this.btnAddNewRemote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddNewRemote.Location = new System.Drawing.Point(329, 19);
      this.btnAddNewRemote.Name = "btnAddNewRemote";
      this.btnAddNewRemote.Size = new System.Drawing.Size(221, 23);
      this.btnAddNewRemote.TabIndex = 2;
      this.btnAddNewRemote.Text = "Add New Remote";
      this.btnAddNewRemote.UseVisualStyleBackColor = true;
      this.btnAddNewRemote.Click += new System.EventHandler(this.btnAddNewRemote_Click);
      // 
      // rcloneExplorerSetupWiz
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(584, 411);
      this.Controls.Add(this.grpBrowseRemotes);
      this.Name = "rcloneExplorerSetupWiz";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "rcloneExplorer Config";
      this.grpBrowseRemotes.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lstConfigs;
    private System.Windows.Forms.ColumnHeader colRemotes;
    private System.Windows.Forms.ColumnHeader colRemoteType;
    private System.Windows.Forms.GroupBox grpBrowseRemotes;
    private System.Windows.Forms.Button btnOpenSelectedRemote;
    private System.Windows.Forms.Button btnEditRemote;
    private System.Windows.Forms.Button btnDeleteRemote;
    private System.Windows.Forms.Button btnAddNewRemote;
    private System.Windows.Forms.ComboBox cmbProviderList;
  }
}