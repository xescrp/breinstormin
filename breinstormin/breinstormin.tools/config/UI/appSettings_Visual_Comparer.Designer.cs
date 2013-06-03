namespace breinstormin.tools.config.UI
{
    partial class appSettings_Visual_Comparer
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtLabelEdit = new System.Windows.Forms.TextBox();
            this.lstAppSettingsKEYS = new config.ListViewEx();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblstatus = new System.Windows.Forms.Label();
            this.lnkAddNewKey = new System.Windows.Forms.LinkLabel();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtLabelEdit2 = new System.Windows.Forms.TextBox();
            this.lstDiff_Appsettings = new config.ListViewEx();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtLabelEdit);
            this.panel1.Controls.Add(this.lstAppSettingsKEYS);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 220);
            this.panel1.TabIndex = 0;
            // 
            // txtLabelEdit
            // 
            this.txtLabelEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLabelEdit.Location = new System.Drawing.Point(249, 100);
            this.txtLabelEdit.Multiline = true;
            this.txtLabelEdit.Name = "txtLabelEdit";
            this.txtLabelEdit.Size = new System.Drawing.Size(100, 20);
            this.txtLabelEdit.TabIndex = 3;
            this.txtLabelEdit.Visible = false;
            // 
            // lstAppSettingsKEYS
            // 
            this.lstAppSettingsKEYS.AllowColumnReorder = true;
            this.lstAppSettingsKEYS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstAppSettingsKEYS.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
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
            this.lstAppSettingsKEYS.Size = new System.Drawing.Size(800, 220);
            this.lstAppSettingsKEYS.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstAppSettingsKEYS.TabIndex = 1;
            this.lstAppSettingsKEYS.UseCompatibleStateImageBehavior = false;
            this.lstAppSettingsKEYS.View = System.Windows.Forms.View.Details;
            this.lstAppSettingsKEYS.SubItemClicked += new config.SubItemEventHandler(this.lstAppSettingsKEYS_SubItemClicked);
            this.lstAppSettingsKEYS.SubItemEndEditing += new config.SubItemEndEditingEventHandler(this.lstAppSettingsKEYS_SubItemEndEditing);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "(Common)AppSettings.KEYS";
            this.columnHeader1.Width = 180;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblstatus);
            this.panel2.Controls.Add(this.lnkAddNewKey);
            this.panel2.Controls.Add(this.lnkSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 413);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 32);
            this.panel2.TabIndex = 1;
            // 
            // lblstatus
            // 
            this.lblstatus.AutoSize = true;
            this.lblstatus.Location = new System.Drawing.Point(12, 11);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(0, 13);
            this.lblstatus.TabIndex = 4;
            // 
            // lnkAddNewKey
            // 
            this.lnkAddNewKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkAddNewKey.AutoSize = true;
            this.lnkAddNewKey.Location = new System.Drawing.Point(580, 10);
            this.lnkAddNewKey.Name = "lnkAddNewKey";
            this.lnkAddNewKey.Size = new System.Drawing.Size(90, 13);
            this.lnkAddNewKey.TabIndex = 3;
            this.lnkAddNewKey.TabStop = true;
            this.lnkAddNewKey.Text = "Añadir nueva key";
            this.lnkAddNewKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddNewKey_LinkClicked);
            // 
            // lnkSave
            // 
            this.lnkSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(690, 10);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(87, 13);
            this.lnkSave.TabIndex = 1;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "Guardar cambios";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 220);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(800, 3);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtLabelEdit2);
            this.panel3.Controls.Add(this.lstDiff_Appsettings);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 223);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(800, 190);
            this.panel3.TabIndex = 3;
            // 
            // txtLabelEdit2
            // 
            this.txtLabelEdit2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLabelEdit2.Location = new System.Drawing.Point(350, 85);
            this.txtLabelEdit2.Multiline = true;
            this.txtLabelEdit2.Name = "txtLabelEdit2";
            this.txtLabelEdit2.Size = new System.Drawing.Size(100, 20);
            this.txtLabelEdit2.TabIndex = 4;
            this.txtLabelEdit2.Visible = false;
            // 
            // lstDiff_Appsettings
            // 
            this.lstDiff_Appsettings.AllowColumnReorder = true;
            this.lstDiff_Appsettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstDiff_Appsettings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.lstDiff_Appsettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDiff_Appsettings.DoubleClickActivation = true;
            this.lstDiff_Appsettings.ForeColor = System.Drawing.Color.Red;
            this.lstDiff_Appsettings.FullRowSelect = true;
            this.lstDiff_Appsettings.GridLines = true;
            this.lstDiff_Appsettings.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstDiff_Appsettings.HideSelection = false;
            this.lstDiff_Appsettings.LabelEdit = true;
            this.lstDiff_Appsettings.Location = new System.Drawing.Point(0, 0);
            this.lstDiff_Appsettings.MultiSelect = false;
            this.lstDiff_Appsettings.Name = "lstDiff_Appsettings";
            this.lstDiff_Appsettings.ShowItemToolTips = true;
            this.lstDiff_Appsettings.Size = new System.Drawing.Size(800, 190);
            this.lstDiff_Appsettings.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstDiff_Appsettings.TabIndex = 1;
            this.lstDiff_Appsettings.UseCompatibleStateImageBehavior = false;
            this.lstDiff_Appsettings.View = System.Windows.Forms.View.Details;
            this.lstDiff_Appsettings.SubItemClicked += new config.SubItemEventHandler(this.lstDiff_Appsettings_SubItemClicked);
            this.lstDiff_Appsettings.SubItemEndEditing += new config.SubItemEndEditingEventHandler(this.lstDiff_Appsettings_SubItemEndEditing);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "(Diff)AppSettings.KEYS";
            this.columnHeader3.Width = 180;
            // 
            // appSettings_Visual_Comparer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "appSettings_Visual_Comparer";
            this.Size = new System.Drawing.Size(800, 445);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel3;
        private ListViewEx lstAppSettingsKEYS;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.LinkLabel lnkSave;
        private ListViewEx lstDiff_Appsettings;
        private System.Windows.Forms.TextBox txtLabelEdit;
        private System.Windows.Forms.LinkLabel lnkAddNewKey;
        private System.Windows.Forms.Label lblstatus;
        private System.Windows.Forms.TextBox txtLabelEdit2;
    }
}
