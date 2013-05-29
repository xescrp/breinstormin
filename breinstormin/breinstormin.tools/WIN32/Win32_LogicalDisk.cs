using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Tools.WIN32
{
    public class Win32_LogicalDisk
    {
        private System.Management.ManagementObject _win_logicaldisk;


          public UInt16 Access { get { return (UInt16)_get_value("Access"); } }
          public UInt16 Availability { get { return (UInt16)_get_value("Availability"); } }
          public UInt64 BlockSize { get { return (UInt64)_get_value("BlockSize"); } }
          public string Caption { get { return (string)_get_value("Caption"); } }
          public bool Compressed { get { return (bool)_get_value("Compressed"); } }
          public Win32API.ConfigManagerErrorCode ConfigManagerErrorCode { get { return (Win32API.ConfigManagerErrorCode)_get_value("ConfigManagerErrorCode"); } }
          public bool ConfigManagerUserConfig { get { return (bool)_get_value("ConfigManagerUserConfig"); } }
          public string CreationClassName { get { return (string)_get_value("CreationClassName"); } }
          public string Description { get { return (string)_get_value("Description"); } }
          public string DeviceID { get { return (string)_get_value("DeviceID"); } }
          public UInt32 DriveType { get { return (UInt32)_get_value("DriveType"); } }
          public bool ErrorCleared { get { return (bool)_get_value("ErrorCleared"); } }
          public string ErrorDescription { get { return (string)_get_value("ErrorDescription"); } }
          public string ErrorMethodology { get { return (string)_get_value("ErrorMethodology"); } }
          public string FileSystem { get { return (string)_get_value("FileSystem"); } }
          public UInt64 FreeSpace { get { return (UInt64)_get_value("FreeSpace"); } }
          public DateTime InstallDate { get { return (DateTime)_get_value("InstallDate"); } }
          public UInt32 LastErrorCode { get { return (UInt32)_get_value("LastErrorCode"); } }
          public UInt32 MaximumComponentLength { get { return (UInt32)_get_value("MaximumComponentLength"); } }
          public UInt32 MediaType { get { return (UInt32)_get_value("MediaType"); } }
          public string Name { get { return (string)_get_value("Name"); } }
          public UInt64 NumberOfBlocks { get { return (UInt64)_get_value("NumberOfBlocks"); } }
          public string PNPDeviceID { get { return (string)_get_value("PNPDeviceID"); } }
          public UInt16[] PowerManagementCapabilities { get { return (UInt16[])_get_value("PowerManagementCapabilities"); } }
          public bool PowerManagementSupported { get { return (bool)_get_value("PowerManagementSupported"); } }
          public string ProviderName { get { return (string)_get_value("ProviderName"); } }
          public string Purpose { get { return (string)_get_value("Purpose"); } }
          public bool QuotasDisabled { get { return (bool)_get_value("QuotasDisabled"); } }
          public bool QuotasIncomplete { get { return (bool)_get_value("QuotasIncomplete"); } }
          public bool QuotasRebuilding { get { return (bool)_get_value("QuotasRebuilding"); } }
          public UInt64 Size { get { return (UInt64)_get_value("Size"); } }
          public string Status { get { return (string)_get_value("Status"); } }
          public UInt16 StatusInfo { get { return (UInt16)_get_value("StatusInfo"); } }
          public bool SupportsDiskQuotas { get { return (bool)_get_value("SupportDiskQuotas"); } }
          public bool SupportsFileBasedCompression { get { return (bool)_get_value("SupportsFileBasedCompression"); } }
          public string SystemCreationClassName { get { return (string)_get_value("SystemCreationClassName"); } }
          public string SystemName { get { return (string)_get_value("SystemName"); } }
          public bool VolumeDirty { get { return (bool)_get_value("VolumeDirty"); } }
          public string VolumeName { get { return (string)_get_value("VolumeName"); } }
          public string VolumeSerialNumber { get { return (string)_get_value("VolumeSerialNumber"); } }

          public static Win32_LogicalDisk[] GetWin32_LogicalDisks(string machineName)
          {
              List<Win32_LogicalDisk> lst = new List<Win32_LogicalDisk>();
              System.Management.ManagementObjectCollection mg =
                  WIN32.WmiAccess.GetInstancesOfClass(machineName, "Win32_LogicalDisk");
              foreach (System.Management.ManagementObject m in mg)
              {

                  WIN32.Win32_LogicalDisk prc = new DotNet.Tools.WIN32.Win32_LogicalDisk(m);
                  lst.Add(prc);

              }
              return lst.ToArray();
          }


        private object _get_value(string property) 
        {
            return _win_logicaldisk[property];
        }

        int _execute_method(string methodname, string[] parameters)
        {
            return (int)_win_logicaldisk.InvokeMethod(methodname, parameters);
        }

        internal Win32_LogicalDisk(System.Management.ManagementObject mg_object) 
        {
            _win_logicaldisk = mg_object;
        }
    }
}
