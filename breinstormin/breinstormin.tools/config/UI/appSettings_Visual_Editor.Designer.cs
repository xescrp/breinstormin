namespace breinstormin.tools.config.UI
{
    partial class appSettings_Visual_Editor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lnkAppFile = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnkAddNewKey = new System.Windows.Forms.LinkLabel();
            this.lblstatus = new System.Windows.Forms.Label();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.txtLabelEdit = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lstAppSettingsKEYS = new config.ListViewEx();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkAppFile
            // 
            this.lnkAppFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lnkAppFile.Location = new System.Drawing.Point(0, 0);
            this.lnkAppFile.Name = "lnkAppFile";
            this.lnkAppFile.Size = new System.Drawing.Size(398, 37);
            this.lnkAppFile.TabIndex = 0;
            this.lnkAppFile.TabStop = true;
            this.lnkAppFile.Text = "Archivo....";
            this.lnkAppFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAppFile_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.lnkAppFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(398, 37);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lnkAddNewKey);
            this.panel2.Controls.Add(this.lblstatus);
            this.panel2.Controls.Add(this.lnkSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 414);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(398, 33);
            this.panel2.TabIndex = 2;
            // 
            // lnkAddNewKey
            // 
            this.lnkAddNewKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkAddNewKey.AutoSize = true;
            this.lnkAddNewKey.Location = new System.Drawing.Point(170, 10);
            this.lnkAddNewKey.Name = "lnkAddNewKey";
            this.lnkAddNewKey.Size = new System.Drawing.Size(90, 13);
            this.lnkAddNewKey.TabIndex = 2;
            this.lnkAddNewKey.TabStop = true;
            this.lnkAddNewKey.Text = "Añadir nueva key";
            this.lnkAddNewKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddNewKey_LinkClicked);
            // 
            // lblstatus
            // 
            this.lblstatus.AutoSize = true;
            this.lblstatus.Location = new System.Drawing.Point(12, 11);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(0, 13);
            this.lblstatus.TabIndex = 1;
            // 
            // lnkSave
            // 
            this.lnkSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(277, 10);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(87, 13);
            this.lnkSave.TabIndex = 0;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "Guardar cambios";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // txtLabelEdit
            // 
            this.txtLabelEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLabelEdit.Location = new System.Drawing.Point(160, 298);
            this.txtLabelEdit.Multiline = true;
            this.txtLabelEdit.Name = "txtLabelEdit";
            this.txtLabelEdit.Size = new System.Drawing.Size(100, 20);
            this.txtLabelEdit.TabIndex = 2;
            this.txtLabelEdit.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lstAppSettingsKEYS);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(398, 377);
            this.panel3.TabIndex = 3;
            // 
            // lstAppSettingsKEYS
            // 
            this.lstAppSettingsKEYS.AllowColumnReorder = true;
            this.lstAppSettingsKEYS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstAppSettingsKEYS.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstAppSettingsKEYS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAppSettingsKEYS.DoubleClickActivation = true;
            this.lstAppSettingsKEYS.FullRowSelect = true;
            this.lstAppSettingsKEYS.GridLines = true;
            this.lstAppSettingsKEYS.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstAppSettingsKEYS.HideSelection = false;
            this.lstAppSettingsKEYS.LabelEdit = true;
            this.lstAppSettingsKEYS.Location = new System.Drawing.Point(0, 0);
            this.lstAppSettingsKEYS.MultiSelect = false;
            this.lstAppSettingsKEYS.Name = "lstAppSettingsKEYS";
            this.lstAppSettingsKEYS.ShowItemToolTips = true;
            this.lstAppSettingsKEYS.Size = new System.Drawing.Size(398, 377);
            this.lstAppSettingsKEYS.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstAppSettingsKEYS.TabIndex = 0;
            this.lstAppSettingsKEYS.UseCompatibleStateImageBehavior = false;
            this.lstAppSettingsKEYS.View = System.Windows.Forms.View.Details;
            this.lstAppSettingsKEYS.SubItemClicked += new config.SubItemEventHandler(this.lstAppSettingsKEYS_SubItemClicked);
            this.lstAppSettingsKEYS.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstAppSettingsKEYS_MouseMove);
            this.lstAppSettingsKEYS.SubItemEndEditing += new config.SubItemEndEditingEventHandler(this.lstAppSettingsKEYS_SubItemEndEditing);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "AppSettings.KEYS";
            this.columnHeader1.Width = 154;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "AppSettings.VALUES";
            this.columnHeader2.Width = 212;
            // 
            // appSettings_Visual_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtLabelEdit);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "appSettings_Visual_Editor";
            this.Size = new System.Drawing.Size(398, 447);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void lstAppSettingsKEYS_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.ListViewItem item;
            int idx = this.lstAppSettingsKEYS.GetSubItemAt(e.X, e.Y, out item);
            if (item != null && idx == 3)
                toolTip1.SetToolTip(lstAppSettingsKEYS, item.Tag.ToString());
            else
                toolTip1.SetToolTip(lstAppSettingsKEYS, null);
        }



        #endregion

        private System.Windows.Forms.LinkLabel lnkAppFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel lnkSave;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblstatus;
        private ListViewEx lstAppSettingsKEYS;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox txtLabelEdit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel lnkAddNewKey;
    }
}
