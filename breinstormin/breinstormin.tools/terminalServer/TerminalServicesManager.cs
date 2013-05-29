using System.Collections.Generic;
using System.Diagnostics;
using DotNet.Tools.TerminalServer;

namespace DotNet.Tools.TerminalServer
{
    /// <summary>
    /// The main Cassia class, and the only class you should directly construct from your application code.
    /// Provides a default implementation of <see cref="ITerminalServicesManager" />.
    /// </summary>
    public class TerminalServicesManager : ITerminalServicesManager
    {
        #region ITerminalServicesManager Members

        /// <inheritdoc />
        public ITerminalServicesSession CurrentSession
        {
            get { return new TerminalServicesSession(GetLocalServer(), Process.GetCurrentProcess().SessionId); }
        }

        /// <inheritdoc />
        public ITerminalServicesSession ActiveConsoleSession
        {
            get
            {
                var sessionId = WIN32.NativeMethodsHelper.GetActiveConsoleSessionId();
                return sessionId == null ? null : new TerminalServicesSession(GetLocalServer(), sessionId.Value);
            }
        }

        /// <inheritdoc />
        public ITerminalServer GetRemoteServer(string serverName)
        {
            return new TerminalServer(new RemoteServerHandle(serverName));
        }

        /// <inheritdoc />
        public ITerminalServer GetLocalServer()
        {
            return new TerminalServer(new LocalServerHandle());
        }

        /// <inheritdoc />
        public IList<ITerminalServer> GetServers(string domainName)
        {
            var servers = new List<ITerminalServer>();
            foreach (WIN32.Win32API.WTS_SERVER_INFO serverInfo in WIN32.NativeMethodsHelper.EnumerateServers(domainName))
            {
                servers.Add(new TerminalServer(new RemoteServerHandle(serverInfo.ServerName)));
            }
            return servers;
        }

        #endregion
    }
}