using System;

namespace DotNet.Tools.TerminalServer
{
    /// <summary>
    /// Connection to the local terminal server.
    /// </summary>
    public class LocalServerHandle : ITerminalServerHandle
    {
        #region ITerminalServerHandle Members

        public IntPtr Handle
        {
            get { return WIN32.NativeMethodsHelper.LocalServerHandle; }
        }

        public string ServerName
        {
            get { return Environment.MachineName; }
        }

        public bool IsOpen
        {
            get { return true; }
        }

        public void Open() {}

        public void Close() {}

        public bool Local
        {
            get { return true; }
        }

        public void Dispose() {}

        #endregion
    }
}