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
            this.lblSettingsFiletypeAssoc = new System.Windows.Forms.Label();
            this.lstSettingsFiletypeAssociation = new System.Windows.Forms.ListView();
            this.colSetting = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lblSettingsFiletypeAssoc
            // 
            this.lblSettingsFiletypeAssoc.AutoSize = true;
            this.lblSettingsFiletypeAssoc.Location = new System.Drawing.Point(10, 113);
            this.lblSettingsFiletypeAssoc.Name = "lblSettingsFiletypeAssoc";
            this.lblSettingsFiletypeAssoc.Size = new System.Drawing.Size(108, 13);
            this.lblSettingsFiletypeAssoc.TabIndex = 0;
            this.lblSettingsFiletypeAssoc.Text = "Filetype Associations:";
            // 
            // lstSettingsFiletypeAssociation
            // 
            this.lstSettingsFiletypeAssociation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSettingsFiletypeAssociation.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSetting,
            colValue});
            this.lstSettingsFiletypeAssociation.FullRowSelect = true;
            this.lstSettingsFiletypeAssociation.Location = new System.Drawing.Point(13, 129);
            this.lstSettingsFiletypeAssociation.Name = "lstSettingsFiletypeAssociation";
            this.lstSettingsFiletypeAssociation.Size = new System.Drawing.Size(559, 270);
            this.lstSettingsFiletypeAssociation.TabIndex = 1;
            this.lstSettingsFiletypeAssociation.UseCompatibleStateImageBehavior = false;
            this.lstSettingsFiletypeAssociation.View = System.Windows.Forms.View.Details;
            // 
            // colSetting
            // 
            this.colSetting.Text = "Setting";
            // 
            // colValue
            // 
            colValue.Text = "Value";
            // 
            // rcloneExplorerSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.lstSettingsFiletypeAssociation);
            this.Controls.Add(this.lblSettingsFiletypeAssoc);
            this.Name = "rcloneExplorerSettings";
            this.Text = "rcloneExplorerSettings";
            this.Load += new System.EventHandler(this.rcloneExplorerSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSettingsFiletypeAssoc;
        private System.Windows.Forms.ListView lstSettingsFiletypeAssociation;
        private System.Windows.Forms.ColumnHeader colSetting;
    }
}