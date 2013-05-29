using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Tools.WIN32
{
    public class Win32_Process
    {
        private System.Management.ManagementObject _win_process;
        private static CIM_DataFile _search_file;

        public string Caption { get { return (string)_get_value("Caption"); } }
        public string CommandLine { get { return (string)_get_value("CommandLine"); } }
        public string CreationClassName { get { return (string)_get_value("CreationClassName"); } }
        public DateTime CreationDate { get { return (DateTime)_get_value("CreationDate"); } }
        public string CSCreationClassName { get { return (string)_get_value("CSCreationClassName"); } }
        public string CSName { get { return (string)_get_value("CSName"); } }
        public string Description { get { return (string)_get_value("Description"); } }
        public string ExecutablePath { get { return (string)_get_value("ExecutablePath"); } }
        public UInt16 ExecutionState { get { return (UInt16)_get_value("ExecutionState"); } }
        public string Handle { get { return (string)_get_value("Handle"); } }
        public UInt32 HandleCount { get { return (UInt32)_get_value("HandleCount"); } }
        public DateTime InstallDate { get { return (DateTime)_get_value("InstallDate"); } }
        public UInt64 KernelModeTime { get { return (UInt64)_get_value("KernelModeTime"); } }
        public UInt32 MaximumWorkingSetSize { get { return (UInt32)_get_value("MaximumWorkingSetSize"); } }
        public UInt32 MinimumWorkingSetSize { get { return (UInt32)_get_value("MinimumWorkingSetSize"); } }
        public string Name { get { return (string)_get_value("Name"); } }
        public string OSCreationClassName { get { return (string)_get_value("OSCreationClassName"); } }
        public string OSName { get { return (string)_get_value("OSName"); } }
        public UInt64 OtherOperationCount { get { return (UInt64)_get_value("OtherOperationCount"); } }
        public UInt64 OtherTransferCount { get { return (UInt64)_get_value("OtherTransferCount"); } }
        public UInt32 PageFaults { get { return (UInt32)_get_value("PageFaults"); } }
        public UInt32 PageFileUsage { get { return (UInt32)_get_value("PageFileUsage"); } }
        public UInt32 ParentProcessId { get { return (UInt32)_get_value("ParentProcessId"); } }
        public UInt32 PeakPageFileUsage { get { return (UInt32)_get_value("PeakPageFileUsage"); } }
        public UInt64 PeakVirtualSize { get { return (UInt64)_get_value("PeakVirtualSize"); } }
        public UInt32 PeakWorkingSetSize { get { return (UInt32)_get_value("PeakWoringSetSize"); } }
        public UInt32 Priority { get { return (UInt32)_get_value("Priority"); } }
        public UInt64 PrivatePageCount { get { return (UInt64)_get_value("PrivatePageCount"); } }
        public UInt32 ProcessId { get { return (UInt32)_get_value("ProcessId"); } }
        public UInt32 QuotaNonPagedPoolUsage { get { return (UInt32)_get_value("QuotaNonPagedPoolUsage"); } }
        public UInt32 QuotaPagedPoolUsage { get { return (UInt32)_get_value("QuotaPagedPoolUsage"); } }
        public UInt32 QuotaPeakNonPagedPoolUsage { get { return (UInt32)_get_value("QuotaPeakNonPagedPoolUsage"); } }
        public UInt32 QuotaPeakPagedPoolUsage { get { return (UInt32)_get_value("QuotaPeakPagedPoolUsage"); } }
        public UInt64 ReadOperationCount { get { return (UInt64)_get_value("ReadOperationCount"); } }
        public UInt64 ReadTransferCount { get { return (UInt64)_get_value("ReadTransferCount"); } }
        public UInt32 SessionId { get { return (UInt32)_get_value("SessionId"); } }
        public string Status { get { return (string)_get_value("Status"); } }
        public DateTime TerminationDate { get { return (DateTime)_get_value("TerminationDate"); } }
        public UInt32 ThreadCount { get { return (UInt32)_get_value("ThreadCount"); } }
        public UInt64 UserModeTime { get { return (UInt64)_get_value("UserModeTime"); } }
        public UInt64 VirtualSize { get { return (UInt64)_get_value("VirtualSize"); } }
        public string WindowsVersion { get { return (string)_get_value("WindowsVersion"); } }
        public UInt64 WorkingSetSize { get { return (UInt64)_get_value("WorkingSetSize"); } }
        public UInt64 WriteOperationCount { get { return (UInt64)_get_value("WriteOperationCount"); } }
        public UInt64 WriteTransferCount { get { return (UInt32)_get_value("WriteTransferCount"); } }

        private delegate void _find_file_handler(List<CIM_DataFile> _find_files, System.Threading.ManualResetEvent rs_e,
            Win32_Process prc, ref List<Win32_Process> lstProc);

        public static Win32_Process[] GetWin32_Processes(string machineName) 
        {
            List<Win32_Process> lst = new List<Win32_Process>();
            System.Management.ManagementObjectCollection mg =
                WIN32.WmiAccess.GetInstancesOfClass(machineName, "Win32_Process");
            foreach (System.Management.ManagementObject m in mg)
            {
                
                WIN32.Win32_Process prc = new DotNet.Tools.WIN32.Win32_Process(m);
                lst.Add(prc);

            }
            return lst.ToArray();
        }

        public static Win32API.ProcessReturnCode StartProcess(string remotemachine, string commandline) 
        {
            WmiAccess acc = new WmiAccess("Win32_Process", remotemachine);
            System.Management.ManagementBaseObject inParams = acc.WMI_Class_Instance.GetMethodParameters("Create");
            inParams["CommandLine"] = commandline;
            System.Management.ManagementBaseObject rt = acc.WMI_Class_Instance.InvokeMethod("Create", inParams, null);
            
            Win32API.ProcessReturnCode p_out = (Win32API.ProcessReturnCode)int.Parse(rt["ReturnValue"].ToString());
            return p_out;
        }

        private object _get_value(string property) 
        {
            return _win_process[property];
        }

        int _execute_method(string methodname, string[] parameters)
        {
            return (int)_win_process.InvokeMethod(methodname, parameters);
        }

        internal Win32_Process(System.Management.ManagementObject mg_object) 
        {
            _win_process = mg_object;
        }

        public CIM_DataFile[] GetOpenedCIM_DataFiles() 
        {
            System.Management.ManagementObjectCollection mg = _win_process.GetRelated("CIM_DataFile");
            List<CIM_DataFile> dt_diles = new List<CIM_DataFile>();
            try
            {
                foreach (System.Management.ManagementObject mo in mg)
                {
                    CIM_DataFile dt = new CIM_DataFile(mo);
                    dt_diles.Add(dt);
                }
            }
            catch (Exception ex) { }
            return dt_diles.ToArray();
        }

        public string[] GetOpenedFiles() 
        {
            System.Management.ManagementObjectCollection mg = _win_process.GetRelated("CIM_DataFile");
            List<string> dt_diles = new List<string>();
            try
            {
                foreach (System.Management.ManagementObject mo in mg)
                {
                    CIM_DataFile dt = new CIM_DataFile(mo);
                    dt_diles.Add(dt.FileName);
                }
            }
            catch (Exception ex) { }
            return dt_diles.ToArray();
        }

        public static Win32_Process[] GetProcessesWithOpenedFile(string machineName, string filename) 
        {
            filename = filename.Replace(@"\", @"\\");
            System.Management.ManagementObjectCollection mog = 
                WmiAccess.GetInstancesByNameForFiles(machineName, "CIM_DataFile", filename);
            List<Win32_Process> lstProc = new List<Win32_Process>();

            List<CIM_DataFile> _find_files = new List<CIM_DataFile>();
            foreach (System.Management.ManagementObject o_file in mog) 
            {
                CIM_DataFile f_o = new CIM_DataFile(o_file);
                _find_files.Add(f_o);
            }

            

            List<System.Threading.ManualResetEvent> _resets = new List<System.Threading.ManualResetEvent>();

            Win32_Process[] procs = Win32_Process.GetWin32_Processes(machineName);
            int cnt = 0;
            foreach (Win32_Process prc in procs) 
            {
                //CIM_DataFile[] files = prc.GetOpenedCIM_DataFiles();
                //foreach (CIM_DataFile file in files) 
                //{
                //    _search_file = file;
                //    CIM_DataFile d_finded = _find_files.Find(find_file);
                //    if (d_finded != null) 
                //    {
                //        lstProc.Add(prc);
                //    }

                //}
                _find_file_handler dlg = new _find_file_handler(_find_file);
                System.Threading.ManualResetEvent rs = new System.Threading.ManualResetEvent(false);

                dlg.BeginInvoke(_find_files, rs, prc, ref lstProc, null, null);
                _resets.Add(rs);
                cnt++;
                if (cnt == 60) 
                {
                    System.Threading.ManualResetEvent[] m_resets = _resets.ToArray();
                    System.Threading.WaitHandle.WaitAll(m_resets);
                    _resets = new List<System.Threading.ManualResetEvent>();
                    cnt = 0;
                }
                
            }
            
            System.Threading.ManualResetEvent[] resets = _resets.ToArray();

            System.Threading.WaitHandle.WaitAll(resets, new TimeSpan(0,5,0));
            System.Threading.Thread.CurrentThread.Join(500);
                //foreach (System.Management.ManagementObject mo in mog)
                //{
                //    System.Management.ManagementObjectCollection mg_C = mo.GetRelated();
                //    foreach (System.Management.ManagementObject mo_L in mg_C)
                //    {
                //        Console.WriteLine(mo_L.ClassPath);
                //        System.Management.ManagementObjectCollection mg_CC = mo_L.GetRelated("Win32_Process");
                //        foreach (System.Management.ManagementObject mo_LL in mg_CC) 
                //        {
                //            Console.WriteLine(mo_LL.ClassPath);
                //        }
                //        //Win32_Process prc = new Win32_Process(mo_L);
                //        //lstProc.Add(prc);
                //    }
                //}
            

            return lstProc.ToArray();
        }


        private static void _find_file(List<CIM_DataFile> _find_files, System.Threading.ManualResetEvent rs_e, 
            Win32_Process prc, ref List<Win32_Process> lstProc) 
        {
            
                CIM_DataFile[] files = prc.GetOpenedCIM_DataFiles();
                foreach (CIM_DataFile file in files)
                {
                    _search_file = file;
                    CIM_DataFile d_finded = _find_files.Find(find_file);
                    if (d_finded != null)
                    {
                        lstProc.Add(prc);
                    }

                }
                rs_e.Set();
            
        }

        private static bool find_file(CIM_DataFile file) 
        {
            return _search_file.Name.ToLower() == file.Name.ToLower();
        }



        public Win32API.ProcessReturnCode CreateProcess(string CommandLine, out UInt32 ProcessId) 
        {
            ProcessId = 0;
            string[] parms = new string[] { CommandLine, null, null, ProcessId.ToString() };
            object prd = _win_process.InvokeMethod("Create", parms);
            ProcessId = UInt32.Parse(parms[3]);
            Win32API.ProcessReturnCode p_out = (Win32API.ProcessReturnCode)int.Parse(prd.ToString());
            
            return p_out;
        }

        public Win32API.ProcessReturnCode Terminate() 
        {

            return (Win32API.ProcessReturnCode)_execute_method("Terminate", null);
        }

        public Win32API.ProcessReturnCode GetOwner(out string username, out string domain)
        {
            username = "";
            domain = "";
            string[] parms = new string[] { username, domain };
            object prd = _win_process.InvokeMethod("GetOwner", parms);
            int rs = int.Parse(prd.ToString());
            username = parms[0];
            domain = parms[1];
            //Win32API.ProcessReturnCode p_out = (Win32API.ProcessReturnCode)prd["returnValue"];
            //username = (string)prd["User"];
            //domain = (string)prd["Domain"];
            return (Win32API.ProcessReturnCode)rs;
        }
    }
}
