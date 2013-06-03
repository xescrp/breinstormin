using System;
using System.Collections.Generic;
using System.Text;

namespace breinstormin.tools.config
{
    public class AppConfig
    {
        System.Configuration.Configuration _config;
        Internals.AppSettingsClass _app_settings;

        public string FilePath { get { return _config.FilePath; } }
        public AppSettings AppSettings { get { return _app_settings; } }
        

        public AppConfig(string filename) 
        {
            System.Configuration.ExeConfigurationFileMap _cfgMap = new System.Configuration.ExeConfigurationFileMap();
            _cfgMap.ExeConfigFilename = filename;
            _config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration
                        (_cfgMap, System.Configuration.ConfigurationUserLevel.None);
            _app_settings = new config.Internals.AppSettingsClass(ref _config);
            
        }

        public void Save() 
        {
            _config.Save(System.Configuration.ConfigurationSaveMode.Modified);
        }
        public void SaveAs(string filename) 
        {
            _config.SaveAs(filename, System.Configuration.ConfigurationSaveMode.Modified);
        }
    }
}
