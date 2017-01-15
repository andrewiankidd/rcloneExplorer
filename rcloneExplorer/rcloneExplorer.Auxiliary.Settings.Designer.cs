namespace rcloneExplorer
{
    partial class rcloneExplorerSettings
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
            System.Windows.Forms.ColumnHeader colValue;
            this.lstSettingsFiletypeAssociation = new System.Windows.Forms.ListView();
            this.colExtension = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnNewFileAssoc = new System.Windows.Forms.Button();
            this.grpFileTypeAssoc = new System.Windows.Forms.GroupBox();
            colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpFileTypeAssoc.SuspendLayout();
            this.SuspendLayout();
            // 
            // colValue
            // 
            colValue.Text = "Value";
            // 
            // lstSettingsFiletypeAssociation
            // 
            this.lstSettingsFiletypeAssociation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSettingsFiletypeAssociation.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colExtension,
            colValue});
            this.lstSettingsFiletypeAssociation.FullRowSelect = true;
            this.lstSettingsFiletypeAssociation.Location = new System.Drawing.Point(6, 19);
            this.lstSettingsFiletypeAssociation.Name = "lstSettingsFiletypeAssociation";
            this.lstSettingsFiletypeAssociation.Size = new System.Drawing.Size(398, 362);
            this.lstSettingsFiletypeAssociation.TabIndex = 1;
            this.lstSettingsFiletypeAssociation.UseCompatibleStateImageBehavior = false;
            this.lstSettingsFiletypeAssociation.View = System.Windows.Forms.View.Details;
            // 
            // colExtension
            // 
            this.colExtension.Text = "Extension";
            // 
            // btnNewFileAssoc
            // 
            this.btnNewFileAssoc.Location = new System.Drawing.Point(410, 19);
            this.btnNewFileAssoc.Name = "btnNewFileAssoc";
            this.btnNewFileAssoc.Size = new System.Drawing.Size(140, 23);
            this.btnNewFileAssoc.TabIndex = 2;
            this.btnNewFileAssoc.Text = "Add New";
            this.btnNewFileAssoc.UseVisualStyleBackColor = true;
            this.btnNewFileAssoc.Click += new System.EventHandler(this.btnNewFileAssoc_Click);
            // 
            // grpFileTypeAssoc
            // 
            this.grpFileTypeAssoc.Controls.Add(this.btnNewFileAssoc);
            this.grpFileTypeAssoc.Controls.Add(this.lstSettingsFiletypeAssociation);
            this.grpFileTypeAssoc.Location = new System.Drawing.Point(12, 12);
            this.grpFileTypeAssoc.Name = "grpFileTypeAssoc";
            this.grpFileTypeAssoc.Size = new System.Drawing.Size(560, 387);
            this.grpFileTypeAssoc.TabIndex = 3;
            this.grpFileTypeAssoc.TabStop = false;
            this.grpFileTypeAssoc.Text = "FileType Associations";
            // 
            // rcloneExplorerSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.grpFileTypeAssoc);
            this.Name = "rcloneExplorerSettings";
            this.Text = "rcloneExplorerSettings";
            this.Load += new System.EventHandler(this.rcloneExplorerSettings_Load);
            this.grpFileTypeAssoc.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView lstSettingsFiletypeAssociation;
        private System.Windows.Forms.ColumnHeader colExtension;
        private System.Windows.Forms.Button btnNewFileAssoc;
        private System.Windows.Forms.GroupBox grpFileTypeAssoc;
    }
}