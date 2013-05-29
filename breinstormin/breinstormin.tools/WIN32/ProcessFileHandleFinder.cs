using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DotNet.Tools.WIN32
{
    internal class ProcessFileHandleFinder
    {
        private ManualResetEvent _workDone;
        internal System.Diagnostics.Process Process;
        private List<Win32API.SYSTEM_HANDLE_INFORMATION> lstHandles;
        private List<FileApi> lstFiles;
        string filesearchstring;

        internal List<Win32API.SYSTEM_HANDLE_INFORMATION> Handles { get { return lstHandles; } }
        internal List<FileApi> Files { get { return lstFiles; } }


        internal ProcessFileHandleFinder(System.Diagnostics.Process process, System.Threading.ManualResetEvent workDone, string searchstring) 
        {
            _workDone = workDone;
            Process = process;
            filesearchstring = searchstring;
        }

        internal void FindFileHandlesThreadPoolCallback(Object threadContext)
        {
            int threadIndex = (int)threadContext;
            
            //Metodo de busqueda para proceso
            _FindHandles();
            _workDone.Set();
        }

        internal void FindFilesThreadPoolCallback(Object threadContext)
        {
            int threadIndex = (int)threadContext;

            //Metodo de busqueda para proceso
            _FindFiles();

            _workDone.Set();
        }

        private void _FindHandles() 
        {
            lstHandles = new List<Win32API.SYSTEM_HANDLE_INFORMATION>();
            List<FileApi> lsthnds = ProcessApi.GetFiles(Process);
            foreach (FileApi file in lsthnds)
            {
                if (file.Name.ToLower().Contains(filesearchstring.ToLower()))
                {
                    lstHandles.Add(file.Handle);
                }
            }
        
        }

        private void _FindFiles()
        {
            lstHandles = new List<Win32API.SYSTEM_HANDLE_INFORMATION>();
            List<FileApi> lsthnds = ProcessApi.GetFiles(Process);
            foreach (FileApi file in lsthnds)
            {
                if (file.Name.ToLower().Contains(filesearchstring.ToLower()))
                {
                    lstFiles.Add(file);
                }
            }

        }

    }
}
