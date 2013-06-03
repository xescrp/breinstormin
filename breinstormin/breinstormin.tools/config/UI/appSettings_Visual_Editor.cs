using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace breinstormin.tools.config.UI
{
    public partial class appSettings_Visual_Editor : UserControl
    {
        private AppConfig _file_app_config;
        private string _file;

        public AppConfig AppConfigFile { get { return _file_app_config; } }
        public string FileName { get { return _file; } set { _file = value; _load(); } }

        public appSettings_Visual_Editor() { InitializeComponent(); }

        public appSettings_Visual_Editor(string file)
        {
            InitializeComponent();
            _file = file;
            _load();
        }


        public void ReLoadConfig() 
        {
            _re_load();
        }

        private void _re_load() 
        {
            lstAppSettingsKEYS.Items.Clear();
            if (_file_app_config != null)
            {
                if (_file_app_config.AppSettings != null)
                {
                    foreach (string key in _file_app_config.AppSettings.Keys)
                    {
                        System.Windows.Forms.ListViewItem item =
                            new ListViewItem(new string[] { key, _file_app_config.AppSettings.GetValue(key) });

                        lstAppSettingsKEYS.Items.Add(item);
                        item.Tag = key;
                    }
                    lstAppSettingsKEYS.Columns[0].Text = "AppSettings.KEYS (" + _file_app_config.AppSettings.Keys.Length.ToString() + ")";
                }
            }

        }

        private void _load() 
        {
            lnkAppFile.Text = _file;

            
            if (!string.IsNullOrEmpty(_file))
            {
                _file_app_config = new AppConfig(_file);

                if (_file_app_config != null)
                {
                    if (_file_app_config.AppSettings != null)
                    {
                        foreach (string key in _file_app_config.AppSettings.Keys)
                        {
                            System.Windows.Forms.ListViewItem item =
                                new ListViewItem(new string[] { key, _file_app_config.AppSettings.GetValue(key) });
                            
                            lstAppSettingsKEYS.Items.Add(item);
                            item.Tag = key;
                        }
                        lstAppSettingsKEYS.Columns[0].Text += " (" + _file_app_config.AppSettings.Keys.Length.ToString() + ")";
                    }
                }
            }
        }

        void lstAppSettingsKEYS_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
        {
            //lstAppSettingsKEYS.EndEditing(true);
        }

        void lstAppSettingsKEYS_SubItemClicked(object sender, SubItemEventArgs e)
        {
            lstAppSettingsKEYS.StartEditing(txtLabelEdit, e.Item, e.SubItem);
        }


        public void SaveConfigFile() 
        {
            lnkSave_LinkClicked(null, null);
        }

        private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                bool _changes = false;
                if (_file_app_config != null) 
                {
                    foreach (ListViewItem item in lstAppSettingsKEYS.Items) 
                    {
                        string key = item.Text;
                        string value = item.SubItems[1].Text;

                        if (_file_app_config.AppSettings.ContainsKey(key))
                        {
                            string _vv = _file_app_config.AppSettings.GetValue(key);
                            //if (_vv != value)
                            //{
                                _file_app_config.AppSettings.AlterValue(key, value);
                                _changes = true;
                            //}
                        }
                        else 
                        {
                            _file_app_config.AppSettings.AddKey(key, value);
                            _changes = true;
                        }
                    }
                }
                if (_changes)
                {
                    _file_app_config.Save();
                    lblstatus.ForeColor = Color.Green;
                    lblstatus.Text = "Cambios guardados...";
                }
                else 
                {
                    lblstatus.ForeColor = Color.Black;
                    lblstatus.Text = "Sin cambios";
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString(), "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkAddNewKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {

                InputBoxResult rs = InputBox.Show("Nombre de la Key");
                if (rs.ReturnCode == DialogResult.OK) 
                {
                    if (!string.IsNullOrEmpty(rs.Text)) 
                    {
                        ListViewItem item = new ListViewItem(new string[] { rs.Text, "" });
                        lstAppSettingsKEYS.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error al añadir key", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkAppFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (System.IO.File.Exists(_file)) 
            {
                System.Diagnostics.Process.Start("notepad.exe", _file);
            }
        }
    }
}
