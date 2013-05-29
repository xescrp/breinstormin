using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;

namespace DotNet.Tools.TerminalServer
{
    /// <summary>
    /// Default implementation of <see cref="ITerminalServicesSession" />.
    /// </summary>
    public class TerminalServicesSession : ITerminalServicesSession
    {
        private readonly LazyLoadedProperty<string> _applicationName;
        private readonly LazyLoadedProperty<int> _clientBuildNumber;
        private readonly LazyLoadedProperty<string> _clientDirectory;
        private readonly LazyLoadedProperty<IClientDisplay> _clientDisplay;
        private readonly LazyLoadedProperty<int> _clientHardwareId;
        private readonly LazyLoadedProperty<IPAddress> _clientIPAddress;
        private readonly LazyLoadedProperty<string> _clientName;
        private readonly LazyLoadedProperty<short> _clientProductId;
        private readonly LazyLoadedProperty<ClientProtocolType> _clientProtocolType;
        private readonly GroupLazyLoadedProperty<DateTime?> _connectTime;
        private readonly GroupLazyLoadedProperty<WIN32.Win32API.ConnectionState> _connectionState;
        private readonly GroupLazyLoadedProperty<DateTime?> _currentTime;
        private readonly GroupLazyLoadedProperty<DateTime?> _disconnectTime;
        private readonly GroupLazyLoadedProperty<string> _domainName;
        private readonly GroupLazyLoadedProperty<IProtocolStatistics> _incomingStatistics;
        private readonly LazyLoadedProperty<string> _initialProgram;
        private readonly GroupLazyLoadedProperty<DateTime?> _lastInputTime;
        private readonly GroupLazyLoadedProperty<DateTime?> _loginTime;
        private readonly GroupLazyLoadedProperty<IProtocolStatistics> _outgoingStatistics;
        private readonly LazyLoadedProperty<EndPoint> _remoteEndPoint;
        private readonly ITerminalServer _server;
        private readonly int _sessionId;
        private readonly GroupLazyLoadedProperty<string> _userName;
        private readonly GroupLazyLoadedProperty<string> _windowStationName;
        private readonly LazyLoadedProperty<string> _workingDirectory;

        public TerminalServicesSession(ITerminalServer server, int sessionId)
        {
            _server = server;
            _sessionId = sessionId;

            // TODO: on Windows Server 2008, most of these values can be fetched in one shot from WTSCLIENT.
            // Do this with GroupLazyLoadedProperty.
            _clientBuildNumber = new LazyLoadedProperty<int>(GetClientBuildNumber);
            _clientIPAddress = new LazyLoadedProperty<IPAddress>(GetClientIPAddress);
            _remoteEndPoint = new LazyLoadedProperty<EndPoint>(GetRemoteEndPoint);
            _clientDisplay = new LazyLoadedProperty<IClientDisplay>(GetClientDisplay);
            _clientDirectory = new LazyLoadedProperty<string>(GetClientDirectory);
            _workingDirectory = new LazyLoadedProperty<string>(GetWorkingDirectory);
            _initialProgram = new LazyLoadedProperty<string>(GetInitialProgram);
            _applicationName = new LazyLoadedProperty<string>(GetApplicationName);
            _clientHardwareId = new LazyLoadedProperty<int>(GetClientHardwareId);
            _clientProductId = new LazyLoadedProperty<short>(GetClientProductId);
            _clientProtocolType = new LazyLoadedProperty<ClientProtocolType>(GetClientProtocolType);
            _clientName = new LazyLoadedProperty<string>(GetClientName);

            // TODO: MSDN says most of these properties should be null for the console session.
            // I haven't observed this in practice on Windows Server 2000, 2003, or 2008, but perhaps this 
            // should be considered.
            var loader = IsVistaSp1OrHigher
                             ? (GroupPropertyLoader) LoadWtsInfoProperties
                             : LoadWinStationInformationProperties;
            _windowStationName = new GroupLazyLoadedProperty<string>(loader);
            _connectionState = new GroupLazyLoadedProperty<WIN32.Win32API.ConnectionState>(loader);
            _connectTime = new GroupLazyLoadedProperty<DateTime?>(loader);
            _currentTime = new GroupLazyLoadedProperty<DateTime?>(loader);
            _disconnectTime = new GroupLazyLoadedProperty<DateTime?>(loader);
            _lastInputTime = new GroupLazyLoadedProperty<DateTime?>(loader);
            _loginTime = new GroupLazyLoadedProperty<DateTime?>(loader);
            _userName = new GroupLazyLoadedProperty<string>(loader);
            _domainName = new GroupLazyLoadedProperty<string>(loader);
            _incomingStatistics = new GroupLazyLoadedProperty<IProtocolStatistics>(loader);
            _outgoingStatistics = new GroupLazyLoadedProperty<IProtocolStatistics>(loader);
        }

        public TerminalServicesSession(ITerminalServer server, WIN32.Win32API.WTS_TERNMINALSESSION_INFO sessionInfo)
            : this(server, sessionInfo.SessionID)
        {
            _windowStationName.Value = sessionInfo.WinStationName;
            _connectionState.Value = sessionInfo.State;
        }

        public TerminalServicesSession(ITerminalServer server, WIN32.Win32API.WTS_SESSION_INFO sessionInfo)
            : this(server, sessionInfo.SessionID)
        {
            _windowStationName.Value = sessionInfo.pWinStationName;
            _connectionState.Value = (WIN32.Win32API.ConnectionState)sessionInfo.State;
        }

        private static bool IsVistaSp1OrHigher
        {
            get { return Environment.OSVersion.Version >= new Version(6, 0, 6001); }
        }

        #region ITerminalServicesSession Members

        public IProtocolStatistics IncomingStatistics
        {
            get { return _incomingStatistics.Value; }
        }

        public IProtocolStatistics OutgoingStatistics
        {
            get { return _outgoingStatistics.Value; }
        }

        public string ApplicationName
        {
            get { return _applicationName.Value; }
        }

        public bool Local
        {
            get { return _server.Local; }
        }

        public EndPoint RemoteEndPoint
        {
            get { return _remoteEndPoint.Value; }
        }

        public string InitialProgram
        {
            get { return _initialProgram.Value; }
        }

        public string WorkingDirectory
        {
            get { return _workingDirectory.Value; }
        }

        public ClientProtocolType ClientProtocolType
        {
            get { return _clientProtocolType.Value; }
        }

        public short ClientProductId
        {
            get { return _clientProductId.Value; }
        }

        public int ClientHardwareId
        {
            get { return _clientHardwareId.Value; }
        }

        public string ClientDirectory
        {
            get { return _clientDirectory.Value; }
        }

        public IClientDisplay ClientDisplay
        {
            get { return _clientDisplay.Value; }
        }

        public int ClientBuildNumber
        {
            get { return _clientBuildNumber.Value; }
        }

        public ITerminalServer Server
        {
            get { return _server; }
        }

        public IPAddress ClientIPAddress
        {
            get { return _clientIPAddress.Value; }
        }

        public string WindowStationName
        {
            get { return _windowStationName.Value; }
        }

        public string DomainName
        {
            get { return _domainName.Value; }
        }

        public NTAccount UserAccount
        {
            get { return (string.IsNullOrEmpty(UserName) ? null : new NTAccount(DomainName, UserName)); }
        }

        public string ClientName
        {
            get { return _clientName.Value; }
        }

        public WIN32.Win32API.ConnectionState ConnectionState
        {
            get { return _connectionState.Value; }
        }

        public DateTime? ConnectTime
        {
            get { return _connectTime.Value; }
        }

        public DateTime? CurrentTime
        {
            get { return _currentTime.Value; }
        }

        public DateTime? DisconnectTime
        {
            get { return _disconnectTime.Value; }
        }

        public DateTime? LastInputTime
        {
            get { return _lastInputTime.Value; }
        }

        public DateTime? LoginTime
        {
            get { return _loginTime.Value; }
        }

        public TimeSpan IdleTime
        {
            get
            {
                if (ConnectionState == WIN32.Win32API.ConnectionState.Disconnected)
                {
                    if (CurrentTime != null && DisconnectTime != null)
                    {
                        return CurrentTime.Value - DisconnectTime.Value;
                    }
                }
                else
                {
                    if (CurrentTime != null && LastInputTime != null)
                    {
                        return CurrentTime.Value - LastInputTime.Value;
                    }
                }
                return TimeSpan.Zero;
            }
        }

        public int SessionId
        {
            get { return _sessionId; }
        }

        public string UserName
        {
            get { return _userName.Value; }
        }

        public void Logoff()
        {
            Logoff(true);
        }

        public void Logoff(bool synchronous)
        {
            WIN32.NativeMethodsHelper.LogoffSession(_server.Handle, _sessionId, synchronous);
        }

        public void Disconnect()
        {
            Disconnect(true);
        }

        public void Disconnect(bool synchronous)
        {
            WIN32.NativeMethodsHelper.DisconnectSession(_server.Handle, _sessionId, synchronous);
        }

        public void MessageBox(string text)
        {
            MessageBox(text, null);
        }

        public void MessageBox(string text, string caption)
        {
            MessageBox(text, caption, default(WIN32.Win32API.RemoteMessageBoxIcon));
        }

        public void MessageBox(string text, string caption, WIN32.Win32API.RemoteMessageBoxIcon icon)
        {
            MessageBox(text, caption, default(WIN32.Win32API.RemoteMessageBoxButtons), icon,
                default(WIN32.Win32API.RemoteMessageBoxDefaultButton),
                       default(WIN32.Win32API.RemoteMessageBoxOptions), TimeSpan.Zero, false);
        }

        public WIN32.Win32API.RemoteMessageBoxResult MessageBox(string text, string caption, 
            WIN32.Win32API.RemoteMessageBoxButtons buttons,
            WIN32.Win32API.RemoteMessageBoxIcon icon, WIN32.Win32API.RemoteMessageBoxDefaultButton defaultButton,
            WIN32.Win32API.RemoteMessageBoxOptions options, TimeSpan timeout, bool synchronous)
        {
            var timeoutSeconds = (int) timeout.TotalSeconds;
            var style = (int) buttons | (int) icon | (int) defaultButton | (int) options;
            // TODO: Win 2003 Server doesn't start timeout counter until user moves mouse in session.
            var result = WIN32.NativeMethodsHelper.SendMessage(_server.Handle, _sessionId, caption, text, style,
                                                         timeoutSeconds, synchronous);
            // TODO: Windows Server 2008 R2 beta returns 0 if the timeout expires.
            // find out why this happens or file a bug report.
            return result == 0 ? WIN32.Win32API.RemoteMessageBoxResult.Timeout : result;
        }

        public IList<ITerminalServicesProcess> GetProcesses()
        {
            var allProcesses = _server.GetProcesses();
            var results = new List<ITerminalServicesProcess>();
            foreach (ITerminalServicesProcess process in allProcesses)
            {
                if (process.SessionId == _sessionId)
                {
                    results.Add(process);
                }
            }
            return results;
        }

        public void StartRemoteControl(ConsoleKey hotkey, WIN32.Win32API.RemoteControlHotkeyModifiers hotkeyModifiers)
        {
            if (IsVistaSp1OrHigher)
            {
                WIN32.NativeMethodsHelper.StartRemoteControl(_server.Handle, _sessionId, hotkey, hotkeyModifiers);
            }
            else
            {
                WIN32.NativeMethodsHelper.LegacyStartRemoteControl(_server.Handle, _sessionId, hotkey, hotkeyModifiers);
            }
        }

        public void StopRemoteControl()
        {
            if (!Local)
            {
                throw new InvalidOperationException(
                    "Cannot stop remote control on sessions that are running on remote servers");
            }
            if (IsVistaSp1OrHigher)
            {
                WIN32.NativeMethodsHelper.StopRemoteControl(_sessionId);
            }
            else
            {
                WIN32.NativeMethodsHelper.LegacyStopRemoteControl(_server.Handle, _sessionId, true);
            }
        }

        public void Connect(ITerminalServicesSession target, string password, bool synchronous)
        {
            if (!Local)
            {
                throw new InvalidOperationException("Cannot connect sessions that are running on remote servers");
            }
            if (IsVistaSp1OrHigher)
            {
                WIN32.NativeMethodsHelper.Connect(_sessionId, target.SessionId, password, synchronous);
            }
            else
            {
                WIN32.NativeMethodsHelper.LegacyConnect(_server.Handle, _sessionId, target.SessionId, password, synchronous);
            }
        }

        #endregion

        private void LoadWinStationInformationProperties()
        {
            var wsInfo = WIN32.NativeMethodsHelper.GetWinStationInformation(_server.Handle, _sessionId);
            _windowStationName.Value = wsInfo.WinStationName;
            _connectionState.Value = wsInfo.State;
            _connectTime.Value = WIN32.NativeMethodsHelper.FileTimeToDateTime(wsInfo.ConnectTime);
            _currentTime.Value = WIN32.NativeMethodsHelper.FileTimeToDateTime(wsInfo.CurrentTime);
            _disconnectTime.Value = WIN32.NativeMethodsHelper.FileTimeToDateTime(wsInfo.DisconnectTime);
            _lastInputTime.Value = WIN32.NativeMethodsHelper.FileTimeToDateTime(wsInfo.LastInputTime);
            _loginTime.Value = WIN32.NativeMethodsHelper.FileTimeToDateTime(wsInfo.LoginTime);
            _userName.Value = wsInfo.UserName;
            _domainName.Value = wsInfo.Domain;
            _incomingStatistics.Value = new ProtocolStatistics(wsInfo.ProtocolStatus.Input);
            _outgoingStatistics.Value = new ProtocolStatistics(wsInfo.ProtocolStatus.Output);
        }

        private void LoadWtsInfoProperties()
        {
            var info = WIN32.NativeMethodsHelper.QuerySessionInformationForStruct<WIN32.Win32API.WTSINFO>(
                _server.Handle, _sessionId,
                WIN32.Win32API.WTS_INFO_CLASS.WTSSessionInfo);
            _connectionState.Value = info.State;
            _incomingStatistics.Value = new ProtocolStatistics(info.IncomingBytes, info.IncomingFrames,
                                                               info.IncomingCompressedBytes);
            _outgoingStatistics.Value = new ProtocolStatistics(info.OutgoingBytes, info.OutgoingFrames,
                                                               info.OutgoingCompressedBytes);
            _windowStationName.Value = info.WinStationName;
            _domainName.Value = info.Domain;
            _userName.Value = info.UserName;
            _connectTime.Value = WIN32.NativeMethodsHelper.FileTimeToDateTime(info.ConnectTime);
            _disconnectTime.Value = WIN32.NativeMethodsHelper.FileTimeToDateTime(info.DisconnectTime);
            _lastInputTime.Value = WIN32.NativeMethodsHelper.FileTimeToDateTime(info.LastInputTime);
            _loginTime.Value = WIN32.NativeMethodsHelper.FileTimeToDateTime(info.LogonTime);
            _currentTime.Value = WIN32.NativeMethodsHelper.FileTimeToDateTime(info.CurrentTime);
        }

        private string GetClientName()
        {
            return WIN32.NativeMethodsHelper.QuerySessionInformationForString(_server.Handle, _sessionId,
                                                                        WIN32.Win32API.WTS_INFO_CLASS.WTSClientName);
        }

        private string GetApplicationName()
        {
            return WIN32.NativeMethodsHelper.QuerySessionInformationForString(_server.Handle, _sessionId,
                                                                       WIN32.Win32API.WTS_INFO_CLASS.WTSApplicationName);
        }

        private EndPoint GetRemoteEndPoint()
        {
            return WIN32.NativeMethodsHelper.QuerySessionInformationForEndPoint(_server.Handle, _sessionId);
        }

        private string GetInitialProgram()
        {
            return WIN32.NativeMethodsHelper.QuerySessionInformationForString(_server.Handle, _sessionId,
                                                                        WIN32.Win32API.WTS_INFO_CLASS.WTSInitialProgram);
        }

        private string GetWorkingDirectory()
        {
            return WIN32.NativeMethodsHelper.QuerySessionInformationForString(_server.Handle, _sessionId,
                                                                        WIN32.Win32API.WTS_INFO_CLASS.WTSWorkingDirectory);
        }

        private ClientProtocolType GetClientProtocolType()
        {
            return
                (ClientProtocolType)
                WIN32.NativeMethodsHelper.QuerySessionInformationForShort(_server.Handle, _sessionId,
                                                                    WIN32.Win32API.WTS_INFO_CLASS.WTSClientProtocolType);
        }

        private short GetClientProductId()
        {
            return WIN32.NativeMethodsHelper.QuerySessionInformationForShort(_server.Handle, _sessionId,
                                                                       WIN32.Win32API.WTS_INFO_CLASS.WTSClientProductId);
        }

        private int GetClientHardwareId()
        {
            return WIN32.NativeMethodsHelper.QuerySessionInformationForInt(_server.Handle, _sessionId,
                                                                     WIN32.Win32API.WTS_INFO_CLASS.WTSClientHardwareId);
        }

        private string GetClientDirectory()
        {
            return WIN32.NativeMethodsHelper.QuerySessionInformationForString(_server.Handle, _sessionId,
                                                                        WIN32.Win32API.WTS_INFO_CLASS.WTSClientDirectory);
        }

        private IClientDisplay GetClientDisplay()
        {
            var clientDisplay = WIN32.NativeMethodsHelper.QuerySessionInformationForStruct<WIN32.Win32API.WTS_CLIENT_DISPLAY>(
                _server.Handle, _sessionId, WIN32.Win32API.WTS_INFO_CLASS.WTSClientDisplay);
            return new ClientDisplay(clientDisplay);
        }

        private IPAddress GetClientIPAddress()
        {
            var clientAddress = WIN32.NativeMethodsHelper.QuerySessionInformationForStruct<WIN32.Win32API.WTS_CLIENT_ADDRESS>(
                _server.Handle, _sessionId, WIN32.Win32API.WTS_INFO_CLASS.WTSClientAddress);
            return WIN32.NativeMethodsHelper.ExtractIPAddress(clientAddress.AddressFamily, clientAddress.Address);
        }

        private int GetClientBuildNumber()
        {
            return WIN32.NativeMethodsHelper.QuerySessionInformationForInt(_server.Handle, _sessionId,
                                                                     WIN32.Win32API.WTS_INFO_CLASS.WTSClientBuildNumber);
        }
    }
}