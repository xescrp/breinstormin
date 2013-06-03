using System;
using System.Collections.Generic;
using System.Text;

namespace breinstormin.tools.config.Internals
{
    class AppSettingsClass: AppSettings
    {
        private System.Configuration.Configuration _config;

        public override string[] Keys { get { return _config.AppSettings.Settings.AllKeys; } }

        public AppSettingsClass(ref System.Configuration.Configuration config) 
        {
            _config = config;
        }

        public override string GetValue(string keyname) 
        {
            return _config.AppSettings.Settings[keyname].Value;
        }
        public override void AddKey(string keyname, string value) 
        {
            _config.AppSettings.Settings.Add(keyname, value);
        }
        public override void AlterValue(string keyname, string value) 
        {
            _config.AppSettings.Settings[keyname].Value = value;
        }
        public override void RemoveKey(string keyname) 
        {
            _config.AppSettings.Settings.Remove(keyname);
        }
        public override bool ContainsKey(string keyname)
        {
            _key_to_find = keyname;
            string f = Array.Find(_config.AppSettings.Settings.AllKeys, new Predicate<string>(_find));
            return !string.IsNullOrEmpty(f);
        }
        private string _key_to_find;
        private bool _find(string it_key) 
        {
            return _key_to_find == it_key;
        }
    }
}
