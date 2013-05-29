using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Tools.WIN32
{
    public class Win32_DiskDrive
    {
        private System.Management.ManagementObject _win_diskdrive;

          public UInt16   Availability {get{return (UInt16)_get_value("Availability");}}
          public UInt32   BytesPerSector {get{return (UInt32)_get_value("BytesPerSector");}}
          public UInt16[] Capabilities {get{return (UInt16[])_get_value("Capabilities");}}
          public string[] CapabilityDescriptions {get{return (string[])_get_value("CapabilityDescriptions");}}
          public string Caption { get { return (string)_get_value("Caption"); } }
          public string CompressionMethod { get { return (string)_get_value("CompressionMethod"); } }
          public Win32API.ConfigManagerErrorCode ConfigManagerErrorCode { get { return (Win32API.ConfigManagerErrorCode)_get_value("ConfigManagerErrorCode"); } }
          public bool ConfigManagerUserConfig { get { return (bool)_get_value("ConfigManagerUserConfig"); } }
          public string CreationClassName { get { return (string)_get_value("CreationClassName"); } }
          public UInt64 DefaultBlockSize { get { return (UInt64)_get_value("DefaultBlockSize"); } }
          public string Description { get { return (string)_get_value("Description"); } }
          public string DeviceID { get { return (string)_get_value("DeviceID"); } }
          public bool ErrorCleared { get { return (bool)_get_value("ErrorCleared"); } }
          public string ErrorDescription { get { return (string)_get_value("ErrorDescription"); } }
          public string ErrorMethodology { get { return (string)_get_value("ErrorMethodology"); } }
          public string FirmwareRevision { get { return (string)_get_value("FirmwareRevision"); } }
          public UInt32 Index { get { return (UInt32)_get_value("Index"); } }
          public DateTime InstallDate { get { return (DateTime)_get_value("InstallDate"); } }
          public string InterfaceType { get { return (string)_get_value("InterfaceType"); } }
          public UInt32 LastErrorCode { get { return (UInt32)_get_value("LastErrorCode"); } }
          public string Manufacturer { get { return (string)_get_value("Manufacturer"); } }
          public UInt64 MaxBlockSize { get { return (UInt64)_get_value("MaxBlockSize"); } }
          public UInt64 MaxMediaSize { get { return (UInt64)_get_value("MaxMediaSize"); } }
          public bool MediaLoaded { get { return (bool)_get_value("MediaLoaded"); } }
          public string   MediaType { get { return (string)_get_value("MediaType"); } }
          public UInt64 MinBlockSize { get { return (UInt64)_get_value("MinBlockSize"); } }
          public string Model { get { return (string)_get_value("Model"); } }
          public string Name { get { return (string)_get_value("Name"); } }
          public bool NeedsCleaning { get { return (bool)_get_value("NeedsCleaning"); } }
          public UInt32 NumberOfMediaSupported { get { return (UInt32)_get_value("NumbrerOfMediaSupported"); } }
          public UInt32 Partitions { get { return (UInt32)_get_value("Partitions"); } }
          public string PNPDeviceID { get { return (string)_get_value("PNPDeviceID"); } }
          public UInt16[] PowerManagementCapabilities { get { return (UInt16[])_get_value("PowerManagementCapabilities"); } }
          public bool PowerManagementSupported { get { return (bool)_get_value("PowerManagementSupported"); } }
          public UInt32 SCSIBus { get { return (UInt16 )_get_value("SCSIBus"); } }
          public UInt16 SCSILogicalUnit { get { return (UInt16)_get_value("SCSIBus"); } }
          public UInt16 SCSIPort { get { return (UInt16)_get_value("SCSIPort"); } }
          public UInt16 SCSITargetId { get { return (UInt16)_get_value("SCSITargetId"); } }
          public UInt32 SectorsPerTrack { get { return (UInt32)_get_value("SectorsPerTrack"); } }
          public string SerialNumber { get { return (string)_get_value("SerialNumber"); } }
          public UInt32 Signature { get { return (UInt32)_get_value("Signature"); } }
          public UInt64 Size { get { return (UInt64)_get_value("Size"); } }
          public string Status { get { return (string)_get_value("Status"); } }
          public UInt16 StatusInfo { get { return (UInt16)_get_value("StatusInfo"); } }
          public string SystemCreationClassName { get { return (string)_get_value("SystemCreationClassName"); } }
          public string SystemName { get { return (string)_get_value("SystemName"); } }
          public UInt64 TotalCylinders { get { return (UInt64)_get_value("TotalCylinders"); } }
          public UInt32 TotalHeads { get { return (UInt32)_get_value("TotalHeads"); } }
          public UInt64 TotalSectors { get { return (UInt64)_get_value("TotalSectors"); } }
          public UInt64 TotalTracks { get { return (UInt64)_get_value("TotalTracks"); } }
          public UInt32 TracksPerCylinder { get { return (UInt32)_get_value("TracksPerCylinder"); } }


        private object _get_value(string property) 
        {
            return _win_diskdrive[property];
        }

        int _execute_method(string methodname, string[] parameters)
        {
            return (int)_win_diskdrive.InvokeMethod(methodname, parameters);
        }

        internal Win32_DiskDrive(System.Management.ManagementObject mg_object) 
        {
            _win_diskdrive = mg_object;
        }

        public UInt32 Chkdsk(
            bool FixErrors,
            bool VigorousIndexCheck,
            bool SkipFolderCycle,
            bool ForceDismount,
            bool RecoverBadSectors,
            bool OKToRunAtBootUp) 
        {
            string[] inParams = new string[]{FixErrors.ToString(),VigorousIndexCheck.ToString(),SkipFolderCycle.ToString(), 
            ForceDismount.ToString(), RecoverBadSectors.ToString(),OKToRunAtBootUp.ToString()};
            return (UInt32)_execute_method("Chkdsk", inParams);
        }

    }
}
