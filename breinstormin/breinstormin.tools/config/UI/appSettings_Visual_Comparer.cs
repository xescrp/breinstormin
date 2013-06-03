using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace breinstormin.tools.config.UI
{
    public partial class appSettings_Visual_Comparer : UserControl
    {
        private string[] _config_files;
        private config.AppConfig[] _app_configs;
        private List<string> _same_keys;
        private List<string> _different_keys;

        public string[] SameKeys { get { return _same_keys.ToArray(); } }
        public string[] DifferentKeys { get { return _different_keys.ToArray(); } }
        public config.AppConfig[] AppConfigs { get { return _app_configs; } }
        public string[] AppConfigFiles { get { return _config_files; } set { _config_files = value; _load_columns(); _load(); } }

        public appSettings_Visual_Comparer(string[] files)
        {
            InitializeComponent();
            _config_files = files;
            if (_config_files != null) { Array.Sort(_config_files); }
            _load_columns();
            _load();
        }
        public appSettings_Visual_Comparer() 
        {
            InitializeComponent();
        }

        private void _load_columns() 
        {
            if (_config_files != null)
            {
                for (int i = 1; i < lstAppSettingsKEYS.Columns.Count; i++) { lstAppSettingsKEYS.Columns.RemoveAt(i); }
                for (int i = 1; i < lstDiff_Appsettings.Columns.Count; i++) { lstDiff_Appsettings.Columns.RemoveAt(i); }

                foreach (string _cf_file in _config_files)
                {
                    lstDiff_Appsettings.Columns.Add(_cf_file).Width = 120;
                    lstAppSettingsKEYS.Columns.Add(_cf_file).Width = 120;

                }
            }
        }

        private void _load() 
        {
            lstAppSettingsKEYS.Items.Clear();
            lstDiff_Appsettings.Items.Clear();
            if (_config_files != null) 
            {
                List<config.AppConfig> _tmp_list = new List<AppConfig>();
                foreach (string _cf_file in _config_files) 
                {
                    config.AppConfig app_cfg = new AppConfig(_cf_file);
                    _tmp_list.Add(app_cfg);
                }
                _app_configs = _tmp_list.ToArray();
                _tmp_list = null;
                _same_keys = new List<string>();
                _different_keys = new List<string>();
                foreach (config.AppConfig cf in _app_configs) 
                {

                    foreach (string key in cf.AppSettings.Keys) 
                    {
                        bool _for_all = true;
                        int i = 1;
                        string[] _values = new string[_app_configs.Length + 1];
                        _values[0] = key;
                        //Color kk = null;
                        //string _kk_value = null;
                        foreach (config.AppConfig cf_f in _app_configs) 
                        {
                            if (cf_f.AppSettings.ContainsKey(key))
                            {
                                _values[i] = cf_f.AppSettings.GetValue(key);
                            }
                            else 
                            {
                                _values[i] = "";
                            }
                            _for_all = _for_all & cf_f.AppSettings.ContainsKey(key);
                            i++;
                        }
                        ListViewItem item = new ListViewItem(_values);
                        if (_for_all)
                        {
                            if (!_same_keys.Contains(key))
                            {
                                lstAppSettingsKEYS.Items.Add(item);
                                _same_keys.Add(key);
                            }
                        }
                        else 
                        {
                            if (!_different_keys.Contains(key))
                            {
                                lstDiff_Appsettings.Items.Add(item);                       
                                _different_keys.Add(key);
                            }
                        }
                    }
                }
                lstAppSettingsKEYS.Columns[0].Text += " (" + _same_keys.Count.ToString() + ")";
                lstDiff_Appsettings.Columns[0].Text += " (" + _different_keys.Count.ToString() + ")";


            }
        }

        private void lstAppSettingsKEYS_SubItemClicked(object sender, SubItemEventArgs e)
        {
            lstAppSettingsKEYS.StartEditing(txtLabelEdit, e.Item, e.SubItem);
        }

        private void lstDiff_Appsettings_SubItemClicked(object sender, SubItemEventArgs e)
        {
            lstDiff_Appsettings.StartEditing(txtLabelEdit2, e.Item, e.SubItem);
        }

        private void lstAppSettingsKEYS_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
        {

        }

        private void lstDiff_Appsettings_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
        {

        }

        private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try 
            {
                bool _changes = false;
                int i = 1;
                foreach (config.AppConfig cfg in _app_configs) 
                {
                    foreach (ListViewItem item in lstAppSettingsKEYS.Items) 
                    {
                        string key = item.SubItems[0].Text;
                        if (cfg.AppSettings.ContainsKey(key))
                        {
                            string val = cfg.AppSettings.GetValue(key);
                            if (val != item.SubItems[i].Text)
                            {
                                cfg.AppSettings.AlterValue(key, item.SubItems[i].Text);
                                _changes = true;
                            }

                        }
                        else 
                        {
                            cfg.AppSettings.AddKey(key, item.SubItems[i].Text);
                            _changes = true;
                        }
                    }
                    foreach (ListViewItem item in lstDiff_Appsettings.Items)
                    {
                        string key = item.SubItems[0].Text;
                        if (cfg.AppSettings.ContainsKey(key))
                        {
                            string val = cfg.AppSettings.GetValue(key);
                            if (val != item.SubItems[i].Text)
                            {
                                cfg.AppSettings.AlterValue(key, item.SubItems[i].Text);
                                _changes = true;
                            }

                        }
                    }
                    i++;
                    cfg.Save();

                }

                if (_changes)
                {
                    
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
                        List<string> values = new List<string>();
                        values.Add(rs.Text);
                        foreach (config.AppConfig cfg in _app_configs) 
                        {
                            values.Add("");
                        }
                        ListViewItem item = new ListViewItem(values.ToArray());

                        lstAppSettingsKEYS.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error al añadir key", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
