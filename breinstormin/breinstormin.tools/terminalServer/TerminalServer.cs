using System;
using System.Collections.Generic;

namespace DotNet.Tools.TerminalServer
{
    /// <summary>
    /// Default implementation of <see cref="ITerminalServer" />.
    /// </summary>
    public class TerminalServer : ITerminalServer
    {
        private readonly ITerminalServerHandle _handle;

        public TerminalServer(ITerminalServerHandle handle)
        {
            _handle = handle;
        }

        #region ITerminalServer Members

        public string ServerName
        {
            get { return _handle.ServerName; }
        }

        public bool Local
        {
            get { return _handle.Local; }
        }

        public bool IsOpen
        {
            get { return _handle.IsOpen; }
        }

        public ITerminalServerHandle Handle
        {
            get { return _handle; }
        }

        public IList<ITerminalServicesSession> GetSessions()
        {
            var results = new List<ITerminalServicesSession>();
            var sessionInfos = WIN32.NativeMethodsHelper.GetSessionInfos(Handle);
            foreach (WIN32.Win32API.WTS_SESSION_INFO sessionInfo in sessionInfos)
            {
                results.Add(new TerminalServicesSession(this, sessionInfo));
            }
            return results;
        }

        public ITerminalServicesSession GetSession(int sessionId)
        {
            return new TerminalServicesSession(this, sessionId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Open()
        {
            _handle.Open();
        }

        public void Close()
        {
            _handle.Close();
        }

        public IList<ITerminalServicesProcess> GetProcesses()
        {
            var processes = new List<ITerminalServicesProcess>();
            WIN32.NativeMethodsHelper.ForEachProcessInfo(Handle,
                       delegate(WIN32.Win32API.WTS_PROCESS_INFO processInfo) 
                       { processes.Add(new TerminalServicesProcess(this, processInfo)); });
            return processes;
        }

        public ITerminalServicesProcess GetProcess(int processId)
        {
            foreach (ITerminalServicesProcess process in GetProcesses())
            {
                if (process.ProcessId == processId)
                {
                    return process;
                }
            }
            throw new InvalidOperationException("Process ID " + processId + " not found");
        }

        public void Shutdown(WIN32.Win32API.ShutdownType type)
        {
            WIN32.NativeMethodsHelper.ShutdownSystem(Handle, (int) type);
        }

        #endregion

        ~TerminalServer()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _handle.Dispose();
            }
        }
    }
}