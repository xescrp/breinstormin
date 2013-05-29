using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Tools.WIN32
{
    public class CIM_DataFile
    {
        public UInt32 AccessMask { get { return (UInt32)_get_value("AccessMask"); } }
        public bool Archive { get { return (bool)_get_value("Archive"); } }
        public string Caption { get { return (string)_get_value("Caption"); } }
        public bool Compressed { get { return (bool)_get_value("Compressed"); } }
        public string CompressionMethod { get { return (string)_get_value("CompressionMethod"); } }
        public string CreationClassName { get { return (string)_get_value("CreationClassName"); } }
        public DateTime CreationDate { get { return (DateTime)_get_value("CreationDate"); } }
        public string CSCreationClassName { get { return (string)_get_value("CSCreationClassName"); } }
        public string CSName { get { return (string)_get_value("CSName"); } }
        public string Description { get { return (string)_get_value("Description"); } }
        public string Drive { get { return (string)_get_value("Drive"); } }
        public string EightDotThreeFileName { get { return (string)_get_value("EightDotThreeFileName"); } }
        public bool Encrypted { get { return (bool)_get_value("Encrypted"); } }
        public string EncryptionMethod { get { return (string)_get_value("EncryptionMethod"); } }
        public string Extension { get { return (string)_get_value("Extension"); } }
        public string FileName { get { return (string)_get_value("FileName"); } }
        public UInt64 FileSize { get { return (UInt64)_get_value("FileSize"); } }
        public string FileType { get { return (string)_get_value("FileType"); } }
        public string FSCreationClassName { get { return (string)_get_value("FSCreationClassName"); } }
        public string FSName { get { return (string)_get_value("FSName"); } }
        public bool Hidden { get { return (bool)_get_value("Hidden"); } }
        public DateTime InstallDate { get { return (DateTime)_get_value("InstallDate"); } }
        public UInt64 InUseCount { get { return (UInt64)_get_value("InUseCount"); } }
        public DateTime LastAccessed { get { return (DateTime)_get_value("LastAccessed"); } }
        public DateTime LastModified{ get { return (DateTime)_get_value("LastModified"); } }
        public string Manufacturer { get { return (string)_get_value("Manufacturer"); } }
        public string Name { get { return (string)_get_value("Name"); } }
        public string Path { get { return (string)_get_value("Path"); } }
        public bool Readable { get { return (bool)_get_value("Readable"); } }
        public string Status { get { return (string)_get_value("Status"); } }
        public bool System { get { return (bool)_get_value("System"); } }
        public string Version { get { return (string)_get_value("Version"); } }
        public bool Writeable { get { return (bool)_get_value("Writeable"); } }

        private System.Management.ManagementObject _cim_file;


        private object _get_value(string property)
        {
            return _cim_file[property];
        }

        int _execute_method(string methodname, string[] parameters)
        {
            return (int)_cim_file.InvokeMethod(methodname, parameters);
        }

        public static CIM_DataFile[] GetCIM_DataFilesByName(string machineName, string filename) 
        {
            filename = filename.Replace(@"\", @"\\");
            System.Management.ManagementObjectCollection mog =
                WmiAccess.GetInstancesByNameForFiles(machineName, "CIM_DataFile", filename);
            
            List<CIM_DataFile> _find_files = new List<CIM_DataFile>();
            foreach (System.Management.ManagementObject o_file in mog)
            {
                CIM_DataFile f_o = new CIM_DataFile(o_file);
                _find_files.Add(f_o);
            }
            return _find_files.ToArray();
        }

        public CIM_DataFile(System.Management.ManagementObject cm_mgo) 
        {
            _cim_file = cm_mgo;
        }

        public WIN32.Win32API.FileReturnCode Compress() 
        {
            return (Win32API.FileReturnCode)_execute_method("Compress", null);
        }
        public WIN32.Win32API.FileReturnCode UnCompress() 
        {
            return (Win32API.FileReturnCode)_execute_method("UnCompress", null);
        }

        public WIN32.Win32API.FileReturnCode Rename(string newfullname) 
        {
            string[] parms = new string[] { newfullname };
            return (Win32API.FileReturnCode)_execute_method("Rename", parms);
        }

        public WIN32.Win32API.FileReturnCode Copy(string targetfullname)
        {
            string[] parms = new string[] { targetfullname };
            return (Win32API.FileReturnCode)_execute_method("Copy", parms);
        }

        public WIN32.Win32API.FileReturnCode Delete()
        {
            return (Win32API.FileReturnCode)_execute_method("Delete", null);
        }

    }
}
