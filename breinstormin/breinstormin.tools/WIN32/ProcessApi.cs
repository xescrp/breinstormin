using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace DotNet.Tools.WIN32
{
    public class ProcessApi
    {


        const int CNST_SYSTEM_HANDLE_INFORMATION = 16;
        const uint STATUS_INFO_LENGTH_MISMATCH = 0xc0000004;

        public static List<Win32API.SYSTEM_HANDLE_INFORMATION> GetHandles(Process process)
        {
            uint nStatus;
            int nHandleInfoSize = 0x10000;
            IntPtr ipHandlePointer = Marshal.AllocHGlobal(nHandleInfoSize);
            int nLength = 0;
            IntPtr ipHandle = IntPtr.Zero;

            while ((nStatus = Win32API.NtQuerySystemInformation(CNST_SYSTEM_HANDLE_INFORMATION, ipHandlePointer, nHandleInfoSize, ref nLength)) == STATUS_INFO_LENGTH_MISMATCH)
            {
                nHandleInfoSize = nLength;
                Marshal.FreeHGlobal(ipHandlePointer);
                ipHandlePointer = Marshal.AllocHGlobal(nLength);
            }

            byte[] baTemp = new byte[nLength];
            try
            {
                Win32API.CopyMemory(baTemp, ipHandlePointer, (uint)nLength);
            }
            catch (Exception ex) { }

            long lHandleCount = 0;
            if (Is64Bits())
            {
                lHandleCount = Marshal.ReadInt64(ipHandlePointer);
                ipHandle = new IntPtr(ipHandlePointer.ToInt64() + 8);
            }
            else
            {
                lHandleCount = Marshal.ReadInt32(ipHandlePointer);
                ipHandle = new IntPtr(ipHandlePointer.ToInt32() + 4);
            }

            Win32API.SYSTEM_HANDLE_INFORMATION shHandle;
            List<Win32API.SYSTEM_HANDLE_INFORMATION> lstHandles = new List<Win32API.SYSTEM_HANDLE_INFORMATION>();

            for (long lIndex = 0; lIndex < lHandleCount; lIndex++)
            {
                shHandle = new Win32API.SYSTEM_HANDLE_INFORMATION();
                if (Is64Bits())
                {
                    shHandle = (Win32API.SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                    ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle) + 8);
                }
                else
                {
                    ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle));
                    shHandle = (Win32API.SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                }
                if (shHandle.ProcessID != process.Id) continue;
                lstHandles.Add(shHandle);
            }
            return lstHandles;

        }

        public static List<Win32API.SYSTEM_HANDLE_INFORMATION> GetFileHandles(Process process) 
        {
            List<Win32API.SYSTEM_HANDLE_INFORMATION> lstHandles = new List<Win32API.SYSTEM_HANDLE_INFORMATION>();
            lstHandles = GetHandles(process);

            List<Win32API.SYSTEM_HANDLE_INFORMATION> lstFileHandles = new List<Win32API.SYSTEM_HANDLE_INFORMATION>();

            foreach (Win32API.SYSTEM_HANDLE_INFORMATION hand in lstHandles) 
            {
                try
                {
                    FileApi file = new FileApi(hand);
                    if (file.Name != null && file.Name != "")
                    {
                        lstFileHandles.Add(file.Handle);
                    }
                }
                catch (Exception ex) { }
            }
            return lstFileHandles;

        }

        public static List<FileApi> GetFiles(Process process) 
        {
            List<Win32API.SYSTEM_HANDLE_INFORMATION> lstHandles = new List<Win32API.SYSTEM_HANDLE_INFORMATION>();
            lstHandles = GetHandles(process);

            List<FileApi> lstFileHandles = new List<FileApi>();

            foreach (Win32API.SYSTEM_HANDLE_INFORMATION hand in lstHandles)
            {
                try
                {
                    FileApi file = new FileApi(hand);
                    if (file.Name != null && file.Name != "")
                    {
                        lstFileHandles.Add(file);
                    }
                }
                catch (Exception ex) { }
            }
            return lstFileHandles;
        }


        static bool Is64Bits()
        {
            return Marshal.SizeOf(typeof(IntPtr)) == 8 ? true : false;
        }
        [MTAThread()]
        public static Win32API.SYSTEM_HANDLE_INFORMATION[] FindHandles(string filesearchstring) 
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            List<Win32API.SYSTEM_HANDLE_INFORMATION> lstHandles = new List<Win32API.SYSTEM_HANDLE_INFORMATION>();
            List<ManualResetEvent> doneEvents = new List<ManualResetEvent>();
            List<ProcessFileHandleFinder> processfinders = new List<ProcessFileHandleFinder>();
            
            int i = 0;
            foreach (System.Diagnostics.Process proc in processes) 
            {
                
            
                ManualResetEvent event_reset = new ManualResetEvent(false);
                doneEvents.Add(event_reset);
                ProcessFileHandleFinder prc_handler = new ProcessFileHandleFinder(proc, event_reset, filesearchstring);
                processfinders.Add(prc_handler);
                

                ThreadPool.QueueUserWorkItem(prc_handler.FindFileHandlesThreadPoolCallback, i);
                i++;

                if (i > 61) 
                {
                    WaitHandle.WaitAll(doneEvents.ToArray());
                    foreach (ProcessFileHandleFinder prc in processfinders) 
                    {
                        lstHandles.AddRange(prc.Handles.ToArray());
                    }
                    processfinders.Clear();
                    processfinders = new List<ProcessFileHandleFinder>();
                    i = 0;
                    doneEvents.Clear();
                    doneEvents = new List<ManualResetEvent>();
                }
            }
            

            WaitHandle.WaitAll(doneEvents.ToArray(), 60000);

            foreach (ProcessFileHandleFinder prc in processfinders) 
            {
                if (prc.Handles != null && prc.Handles.Count > 0)
                {
                    lstHandles.AddRange(prc.Handles.ToArray());
                }
            }

            return lstHandles.ToArray();
        }
        [MTAThread()]
        public static FileApi[] FindFileHandles(string filesearchstring)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            List<FileApi> lstHandles = new List<FileApi>();
            List<ManualResetEvent> doneEvents = new List<ManualResetEvent>();
            List<ProcessFileHandleFinder> processfinders = new List<ProcessFileHandleFinder>();

            int i = 0;
            foreach (System.Diagnostics.Process proc in processes)
            {
                ManualResetEvent event_reset = new ManualResetEvent(false);
                doneEvents.Add(event_reset);
                ProcessFileHandleFinder prc_handler = new ProcessFileHandleFinder(proc, event_reset, filesearchstring);
                processfinders.Add(prc_handler);

                ThreadPool.QueueUserWorkItem(prc_handler.FindFilesThreadPoolCallback, i);
                i++;
                if (i > 61) 
                {
                    WaitHandle.WaitAll(doneEvents.ToArray());
                    foreach (ProcessFileHandleFinder prc in processfinders)
                    {
                        lstHandles.AddRange(prc.Files.ToArray());
                    }
                    processfinders.Clear();
                    processfinders = new List<ProcessFileHandleFinder>();
                    i = 0;
                    doneEvents.Clear();
                    doneEvents = new List<ManualResetEvent>();
                }

            }

            
            WaitHandle.WaitAll(doneEvents.ToArray(), 60000);

            foreach (ProcessFileHandleFinder prc in processfinders)
            {
                if (prc.Handles != null && prc.Handles.Count > 0)
                {
                    lstHandles.AddRange(prc.Files.ToArray());
                }
            }

            
            return lstHandles.ToArray();
        }


    }
}
