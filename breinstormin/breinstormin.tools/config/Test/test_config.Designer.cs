namespace breinstormin.tools.config.Test
{
    partial class test_config
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
            this.appSettings_Visual_Editor1 = new config.UI.appSettings_Visual_Editor();
            this.appSettings_Visual_Comparer1 = new config.UI.appSettings_Visual_Comparer();
            this.SuspendLayout();
            // 
            // appSettings_Visual_Editor1
            // 
            this.appSettings_Visual_Editor1.AutoScroll = true;
            this.appSettings_Visual_Editor1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.appSettings_Visual_Editor1.FileName = "web.config";
            this.appSettings_Visual_Editor1.Location = new System.Drawing.Point(31, 23);
            this.appSettings_Visual_Editor1.Name = "appSettings_Visual_Editor1";
            this.appSettings_Visual_Editor1.Size = new System.Drawing.Size(400, 449);
            this.appSettings_Visual_Editor1.TabIndex = 0;
            // 
            // appSettings_Visual_Comparer1
            // 
            this.appSettings_Visual_Comparer1.AppConfigFiles = new string[] { "web,config" };
            this.appSettings_Visual_Comparer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.appSettings_Visual_Comparer1.Location = new System.Drawing.Point(446, 25);
            this.appSettings_Visual_Comparer1.Name = "appSettings_Visual_Comparer1";
            this.appSettings_Visual_Comparer1.Size = new System.Drawing.Size(729, 447);
            this.appSettings_Visual_Comparer1.TabIndex = 1;
            // 
            // test_config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 566);
            this.Controls.Add(this.appSettings_Visual_Comparer1);
            this.Controls.Add(this.appSettings_Visual_Editor1);
            this.Name = "test_config";
            this.Text = "test_config";
            this.ResumeLayout(false);

        }

        #endregion

        private config.UI.appSettings_Visual_Editor appSettings_Visual_Editor1;
        private config.UI.appSettings_Visual_Comparer appSettings_Visual_Comparer1;

    }
}