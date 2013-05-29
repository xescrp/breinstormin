using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace DotNet.Tools.WIN32
{
    public class Win32API
    {
        [DllImport("ntdll.dll")]
        public static extern int NtQueryObject(IntPtr ObjectHandle, int
            ObjectInformationClass, IntPtr ObjectInformation, int ObjectInformationLength,
            ref int returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

        [DllImport("ntdll.dll")]
        public static extern uint NtQuerySystemInformation(int
            SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength,
            ref int returnLength);

        [DllImport("kernel32.dll", EntryPoint = "RtlCopyMemory")]
        public static extern void CopyMemory(byte[] Destination, IntPtr Source, uint Length);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
        
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle,
           ushort hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle,
           uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);
        
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(
        IntPtr hToken,
        int impersonationLevel,
        ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();



        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(
          string principal,
          string authority,
          string password,
          LogonTypes logonType,
          LogonProviders logonProvider,
          out IntPtr token);


        [DllImport("Wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass,
                                                             out IntPtr buffer, out int bytesReturned);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] String pServerName);

        [DllImport("wtsapi32.dll")]
        public static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern Int32 WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] Int32 Reserved,
            [MarshalAs(UnmanagedType.U4)] Int32 Version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref Int32 pCount);

        [DllImport("wtsapi32.dll")]
        public static extern void WTSFreeMemory(IntPtr pMemory);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern int WTSLogoffSession(IntPtr hServer, int sessionId, bool wait);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern int WTSDisconnectSession(IntPtr hServer, int sessionId, bool wait);

        [DllImport("winsta.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WinStationQueryInformation(IntPtr hServer, int sessionId, int information,
                                                            ref WINSTATIONINFORMATIONW buffer, int bufferLength,
                                                            ref int returnedLength);

        [DllImport("winsta.dll", CharSet = CharSet.Unicode, EntryPoint = "WinStationQueryInformationW",
            SetLastError = true)]
        public static extern int WinStationQueryInformationRemoteAddress(IntPtr hServer, int sessionId,
                                                                         WINSTATIONINFOCLASS information,
                                                                         ref WINSTATIONREMOTEADDRESS buffer,
                                                                         int bufferLength, out int returnedLength);

        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WTSSendMessage(IntPtr hServer, int sessionId,
                                                [MarshalAs(UnmanagedType.LPTStr)] string title, int titleLength,
                                                [MarshalAs(UnmanagedType.LPTStr)] string message, int messageLength,
                                                int style, int timeout, out RemoteMessageBoxResult result, bool wait);

        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WTSEnumerateServers([MarshalAs(UnmanagedType.LPTStr)] string pDomainName, int reserved,
                                                     int version, out IntPtr ppServerInfo, out int pCount);

        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WTSEnumerateProcesses(IntPtr hServer, int reserved, int version,
                                                       out IntPtr ppProcessInfo, out int count);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern int WTSTerminateProcess(IntPtr hServer, int processId, int exitCode);

        [DllImport("ws2_32.dll")]
        public static extern ushort ntohs(ushort netValue);

        [DllImport("kernel32.dll")]
        public static extern int FileTimeToSystemTime(ref FILETIME fileTime, ref SYSTEMTIME systemTime);

        [DllImport("kernel32.dll")]
        public static extern int WTSGetActiveConsoleSessionId();

        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WTSStartRemoteControlSession(string serverName, int targetSessionId, byte hotkeyVk,
                                                              short hotkeyModifiers);

        [DllImport("winsta.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WinStationShadow(IntPtr hServer, string serverName, int targetSessionId, int hotkeyVk,
                                                  int hotkeyModifier);

        [DllImport("winsta.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WinStationShadowStop(IntPtr hServer, int targetSessionId, bool wait);

        [DllImport("winsta.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WinStationConnectW(IntPtr hServer, int targetSessionId, int sourceSessionId,
                                                    string password, bool wait);

        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WTSConnectSession(int targetSessionId, int sourceSessionId, string password, bool wait);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern int WTSStopRemoteControlSession(int targetSessionId);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern int WTSShutdownSystem(IntPtr hServer, int shutdownFlag);

        [StructLayout(LayoutKind.Sequential)]
        public struct WTS_SESSION_INFO
        {
            public Int32 SessionID;

            [MarshalAs(UnmanagedType.LPStr)]
            public String pWinStationName;

            public WTS_CONNECTSTATE_CLASS State;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WTS_TERNMINALSESSION_INFO
        {
            public int SessionID;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string WinStationName;

            public ConnectionState State;
        }

        public enum ShutdownType
        {
            /// <summary>
            /// Logs off all sessions on the server other than the one calling 
            /// <see cref="ITerminalServer.Shutdown" />, preventing any new connections until the server
            /// is restarted.
            /// </summary>
            LogoffAllSessions = 0x00000001,
            /// <summary>
            /// Shuts down the server without powering it off.
            /// </summary>
            Shutdown = 0x00000002,
            /// <summary>
            /// Reboots the server.
            /// </summary>
            Reboot = 0x00000004,
            /// <summary>
            /// Shuts down and powers off the server.
            /// </summary>
            PowerOff = 0x00000008,
            /// <summary>
            /// This value is not yet supported by Windows.
            /// </summary>
            FastReboot = 0x00000010,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short Year;
            public short Month;
            public short DayOfWeek;
            public short Day;
            public short Hour;
            public short Minute;
            public short Second;
            public short Milliseconds;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINSTATIONREMOTEADDRESS
        {
            public System.Net.Sockets.AddressFamily Family;
            public short Port;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] Address;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] Reserved;
        }

        public enum RemoteMessageBoxResult
        {
            /// <summary>
            /// The user pressed the "OK" button.
            /// </summary>
            Ok = 1,
            /// <summary>
            /// The user pressed the "Cancel" button.
            /// </summary>
            Cancel = 2,
            /// <summary>
            /// The user pressed the "Abort" button.
            /// </summary>
            Abort = 3,
            /// <summary>
            /// The user pressed the "Retry" button.
            /// </summary>
            Retry = 4,
            /// <summary>
            /// The user pressed the "Ignore" button.
            /// </summary>
            Ignore = 5,
            /// <summary>
            /// The user pressed the "Yes" button.
            /// </summary>
            Yes = 6,
            /// <summary>
            /// The user pressed the "No" button.
            /// </summary>
            No = 7,
            /// <summary>
            /// The timeout period expired before the user responded to the message box.
            /// </summary>
            Timeout = 0x7D00,
            /// <summary>
            /// The <c>synchronous</c> parameter of <see cref="ITerminalServicesSession.MessageBox(string, string, RemoteMessageBoxButtons, RemoteMessageBoxIcon, RemoteMessageBoxDefaultButton, RemoteMessageBoxOptions, TimeSpan, bool)" />
            /// was set to false, so the method returned immediately, without waiting for a response
            /// from the user.
            /// </summary>
            Asynchronous = 0x7D01,
        }

        public enum WINSTATIONINFOCLASS
        {
            WinStationCreateData,
            WinStationConfiguration,
            WinStationPdParams,
            WinStationWd,
            WinStationPd,
            WinStationPrinter,
            WinStationClient,
            WinStationModules,
            WinStationInformation,
            WinStationTrace,
            WinStationBeep,
            WinStationEncryptionOff,
            WinStationEncryptionPerm,
            WinStationNtSecurity,
            WinStationUserToken,
            WinStationUnused1,
            WinStationVideoData,
            WinStationInitialProgram,
            WinStationCd,
            WinStationSystemTrace,
            WinStationVirtualData,
            WinStationClientData,
            WinStationSecureDesktopEnter,
            WinStationSecureDesktopExit,
            WinStationLoadBalanceSessionTarget,
            WinStationLoadIndicator,
            WinStationShadowInfo,
            WinStationDigProductId,
            WinStationLockedState,
            WinStationRemoteAddress,
            WinStationIdleTime,
            WinStationLastReconnectType,
            WinStationDisallowAutoReconnect,
            WinStationUnused2,
            WinStationUnused3,
            WinStationUnused4,
            WinStationUnused5,
            WinStationReconnectedFromId,
            WinStationEffectsPolicy,
            WinStationType,
            WinStationInformationEx
        }

        public enum WTS_INFO_CLASS
        {
            WTSInitialProgram = 0,
            WTSApplicationName = 1,
            WTSWorkingDirectory = 2,
            WTSOEMId = 3,
            WTSSessionId = 4,
            WTSUserName = 5,
            WTSWinStationName = 6,
            WTSDomainName = 7,
            WTSConnectState = 8,
            WTSClientBuildNumber = 9,
            WTSClientName = 10,
            WTSClientDirectory = 11,
            WTSClientProductId = 12,
            WTSClientHardwareId = 13,
            WTSClientAddress = 14,
            WTSClientDisplay = 15,
            WTSClientProtocolType = 16,
            WTSIdleTime = 17,
            WTSLogonTime = 18,
            WTSIncomingBytes = 19,
            WTSOutgoingBytes = 20,
            WTSIncomingFrames = 21,
            WTSOutgoingFrames = 22,
            WTSClientInfo = 23,
            WTSSessionInfo = 24,
            WTSSessionInfoEx = 25,
            WTSConfigInfo = 26,
            WTSValidationInfo = 27,
            WTSSessionAddressV4 = 28,
            WTSIsRemoteSession = 29
        }

        [Flags]
        public enum RemoteControlHotkeyModifiers
        {
            Shift = 1,
            Control = 2,
            Alt = 4,
        }

        public enum ConnectionState
        {
            /// <summary>
            /// A user is logged on to the session.
            /// </summary>
            Active,
            /// <summary>
            /// A client is connected to the session.
            /// </summary>
            Connected,
            /// <summary>
            /// The session is in the process of connecting to a client.
            /// </summary>
            ConnectQuery,
            /// <summary>
            /// This session is shadowing another session.
            /// </summary>
            Shadowing,
            /// <summary>
            /// The session is active, but the client has disconnected from it.
            /// </summary>
            Disconnected,
            /// <summary>
            /// The session is waiting for a client to connect.
            /// </summary>
            Idle,
            /// <summary>
            /// The session is listening for connections.
            /// </summary>
            Listening,
            /// <summary>
            /// The session is being reset.
            /// </summary>
            Reset,
            /// <summary>
            /// The session is down due to an error.
            /// </summary>
            Down,
            /// <summary>
            /// The session is initializing.
            /// </summary>
            Initializing
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WINSTATIONINFORMATIONW
        {
            public ConnectionState State;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
            public string WinStationName;

            public int SessionId;
            public int Unknown;
            public FILETIME ConnectTime;
            public FILETIME DisconnectTime;
            public FILETIME LastInputTime;
            public FILETIME LoginTime;
            public PROTOCOLSTATUS ProtocolStatus;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string Domain;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
            public string UserName;

            public FILETIME CurrentTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROTOCOLSTATUS
        {
            public PROTOCOLCOUNTERS Output;
            public PROTOCOLCOUNTERS Input;
            public CACHE_STATISTICS Statistics;
            public int AsyncSignal;
            public int AsyncSignalMask;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CACHE_STATISTICS
        {
            private readonly short ProtocolType;
            private readonly short Length;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            private readonly int[] Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROTOCOLCOUNTERS
        {
            public int WdBytes;
            public int WdFrames;
            public int WaitForOutBuf;
            public int Frames;
            public int Bytes;
            public int CompressedBytes;
            public int CompressFlushes;
            public int Errors;
            public int Timeouts;
            public int AsyncFramingError;
            public int AsyncOverrunError;
            public int AsyncOverflowError;
            public int AsyncParityError;
            public int TdErrors;
            public short ProtocolType;
            public short Length;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
            public int[] Reserved;
        }

            [StructLayout(LayoutKind.Sequential)]
            public struct WTS_PROCESS_INFO
            {
                public int SessionId;
                public int ProcessId;

                [MarshalAs(UnmanagedType.LPTStr)]
                public string ProcessName;

                public IntPtr UserSid;
            }


            [StructLayout(LayoutKind.Sequential)]
            public struct WTS_SERVER_INFO
            {
                [MarshalAs(UnmanagedType.LPTStr)]
                public string ServerName;
            }

        public enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        } 



        public enum LogonTypes : uint
        {
            Interactive = 2,
            Network,
            Batch,
            Service,
            NetworkCleartext = 8,
            NewCredentials
        }
        public enum LogonProviders : uint
        {
            Default = 0, // default por plataforma (usar este!)
            WinNT35,     // envia señales de humo a la autoridad
            WinNT40,     // usa NTLM
            WinNT50      // negocia Kerb o NTLM
        }


        public enum ObjectInformationClass : int
        {
            ObjectBasicInformation = 0,
            ObjectNameInformation = 1,
            ObjectTypeInformation = 2,
            ObjectAllTypesInformation = 3,
            ObjectHandleInformation = 4
        }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_BASIC_INFORMATION
        { // Information Class 0
            public int Attributes;
            public int GrantedAccess;
            public int HandleCount;
            public int PointerCount;
            public int PagedPoolUsage;
            public int NonPagedPoolUsage;
            public int Reserved1;
            public int Reserved2;
            public int Reserved3;
            public int NameInformationLength;
            public int TypeInformationLength;
            public int SecurityDescriptorLength;
            public System.Runtime.InteropServices.ComTypes.FILETIME CreateTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_TYPE_INFORMATION
        { // Information Class 2
            public UNICODE_STRING Name;
            public int ObjectCount;
            public int HandleCount;
            public int Reserved1;
            public int Reserved2;
            public int Reserved3;
            public int Reserved4;
            public int PeakObjectCount;
            public int PeakHandleCount;
            public int Reserved5;
            public int Reserved6;
            public int Reserved7;
            public int Reserved8;
            public int InvalidAttributes;
            public GENERIC_MAPPING GenericMapping;
            public int ValidAccess;
            public byte Unknown;
            public byte MaintainHandleDatabase;
            public int PoolType;
            public int PagedPoolUsage;
            public int NonPagedPoolUsage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_NAME_INFORMATION
        { // Information Class 1
            public UNICODE_STRING Name;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct UNICODE_STRING
        {
            public ushort Length;
            public ushort MaximumLength;
            public IntPtr Buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GENERIC_MAPPING
        {
            public int GenericRead;
            public int GenericWrite;
            public int GenericExecute;
            public int GenericAll;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SYSTEM_HANDLE_INFORMATION
        { // Information Class 16
            public int ProcessID;
            public byte ObjectTypeNumber;
            public byte Flags; // 0x01 = PROTECT_FROM_CLOSE, 0x02 = INHERIT
            public ushort Handle;
            public int Object_Pointer;
            public UInt32 GrantedAccess;
        }

        public const int MAX_PATH = 260;
        public const uint STATUS_INFO_LENGTH_MISMATCH = 0xC0000004;
        public const int DUPLICATE_SAME_ACCESS = 0x2;

        

        public enum ProcessReturnCode: uint
        {
            SuccessfulCompletion = 0,
            AccessDenied = 2,
            InsufficientPrivilege = 3,
            UnknownFailure = 8,
            PathNotFound = 9,
            InvalidParameter = 21
        }

        public enum RemoteMessageBoxButtons
        {
            /// <summary>
            /// Show only an "OK" button. This is the default.
            /// </summary>
            Ok = 0,
            /// <summary>
            /// Show "OK" and "Cancel" buttons.
            /// </summary>
            OkCancel = 1,
            /// <summary>
            /// Show "Abort", "Retry", and "Ignore" buttons.
            /// </summary>
            AbortRetryIgnore = 2,
            /// <summary>
            /// Show "Yes", "No", and "Cancel" buttons.
            /// </summary>
            YesNoCancel = 3,
            /// <summary>
            /// Show "Yes" and "No" buttons.
            /// </summary>
            YesNo = 4,
            /// <summary>
            /// Show "Retry" and "Cancel" buttons.
            /// </summary>
            RetryCancel = 5,
        }

        public enum RemoteMessageBoxDefaultButton
        {
            /// <summary>
            /// The first button should be selected. This is the default.
            /// </summary>
            Button1 = 0,
            /// <summary>
            /// The second button should be selected.
            /// </summary>
            Button2 = 0x100,
            /// <summary>
            /// The third button should be selected.
            /// </summary>
            Button3 = 0x200,
            /// <summary>
            /// The fourth button should be selected.
            /// </summary>
            Button4 = 0x300,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WTSINFO
        {
            public ConnectionState State;
            public int SessionId;
            public int IncomingBytes;
            public int OutgoingBytes;
            public int IncomingFrames;
            public int OutgoingFrames;
            public int IncomingCompressedBytes;
            public int OutgoingCompressedBytes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string WinStationName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string Domain;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
            public string UserName;

            [MarshalAs(UnmanagedType.I8)]
            public long ConnectTime;

            [MarshalAs(UnmanagedType.I8)]
            public long DisconnectTime;

            [MarshalAs(UnmanagedType.I8)]
            public long LastInputTime;

            [MarshalAs(UnmanagedType.I8)]
            public long LogonTime;

            [MarshalAs(UnmanagedType.I8)]
            public long CurrentTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WTS_CLIENT_DISPLAY
        {
            public int HorizontalResolution;
            public int VerticalResolution;
            public int ColorDepth;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WTS_CLIENT_ADDRESS
        {
            public System.Net.Sockets.AddressFamily AddressFamily;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] Address;
        }

        [Flags]
        public enum RemoteMessageBoxOptions
        {
            /// <summary>
            /// No additional options. This is the default.
            /// </summary>
            None = 0,
            /// <summary>
            /// Specifies that the text in the message box should be right-aligned. The default is left-aligned.
            /// </summary>
            RightAligned = 0x00080000,
            /// <summary>
            /// Specifies that the message box should use a right-to-left reading order.
            /// </summary>
            RtlReading = 0x00100000,
            /// <summary>
            /// Specifies that the message box should be set to the foreground window when displayed.
            /// </summary>
            SetForeground = 0x00010000,
            /// <summary>
            /// Specifies that the message box should appear above all other windows on the screen.
            /// </summary>
            TopMost = 0x00080000,
        }

        public enum RemoteMessageBoxIcon
        {
            /// <summary>
            /// Show no icon. This is the default.
            /// </summary>
            None = 0,
            /// <summary>
            /// Show a hand icon.
            /// </summary>
            Hand = 0x10,
            /// <summary>
            /// Show a question mark icon.
            /// </summary>
            Question = 0x20,
            /// <summary>
            /// Show an exclamation point icon.
            /// </summary>
            Exclamation = 0x30,
            /// <summary>
            /// Show an informational icon.
            /// </summary>
            Asterisk = 0x40,
            /// <summary>
            /// Show a warning icon.
            /// </summary>
            Warning = Exclamation,
            /// <summary>
            /// Show an error icon.
            /// </summary>
            Error = Hand,
            /// <summary>
            /// Show an informational icon.
            /// </summary>
            Information = Asterisk,
            /// <summary>
            /// Show a stopsign icon.
            /// </summary>
            Stop = Hand,
        }


        // ****************** SERVICIOS ************************* //


        public enum ServiceStartMode
        {
            Automatic,
            Boot,
            System,
            Manual,
            Disabled,
        }

        /// <summary>
        /// El codigo de retorno de la clase WMI Win32_Service
        /// </summary>
        public enum ServiceReturnCode
        {
            Success = 0,
            NotSupported = 1,
            AccessDenied = 2,
            DependentServicesRunning = 3,
            InvalidServiceControl = 4,
            ServiceCannotAcceptControl = 5,
            ServiceNotActive = 6,
            ServiceRequestTimeout = 7,
            UnknownFailure = 8,
            PathNotFound = 9,
            ServiceAlreadyRunning = 10,
            ServiceDatabaseLocked = 11,
            ServiceDependencyDeleted = 12,
            ServiceDependencyFailure = 13,
            ServiceDisabled = 14,
            ServiceLogonFailure = 15,
            ServiceMarkedForDeletion = 16,
            ServiceNoThread = 17,
            StatusCircularDependency = 18,
            StatusDuplicateName = 19,
            StatusInvalidName = 20,
            StatusInvalidParameter = 21,
            StatusInvalidServiceAccount = 22,
            StatusServiceExists = 23,
            ServiceAlreadyPaused = 24
        }

        /// <summary>
        /// El tipo de serivicio del que se trata, normalmente OwnProcess
        /// </summary>
        public enum ServiceType
        {
            KernalDriver = 1,
            FileSystemDriver = 2,
            Adapter = 4,
            RecognizerDriver = 8,
            OwnProcess = 16,
            ShareProcess = 32,
            InteractiveProcess = 256,
        }

        internal enum ServiceErrorControl
        {
            UserNotNotified = 0,
            UserNotified = 1,
            SystemRestartedWithLastKnownGoodConfiguration = 2,
            SystemAttemptsToStartWithAGoodConfiguration = 3
        }
    
    //*********************** FILES **********************//
        public enum FileReturnCode
        {
            Success = 0, 
            Accessdenied = 2,
            Unspecifiedfailure = 8,
            Invalidobject = 9,
            Objectalreadyexists = 10,
            FilesystemnotNTFS = 11,
            PlatformnotWindowsNT_based = 12, 
            Drivenotthesame = 13, 
            Directorynotempty = 14, 
            Sharingviolation = 15, 
            Invalidstartfile = 16, 
            Privilegenotheld = 17, 
            Invalidparameter = 21
        }
 
        public enum ConfigManagerErrorCode:uint 
        {
            Device_is_working_properly = 0x0,
            Device_is_not_configured_correctly = 0x1,
            Windows_cannot_load_the_driver_for_this_device = 0x2,
            Driver_for_this_device_might_be_corrupted_or_the_system_may_be_low_on_memory_or_other_resources = 0x3,
            Device_is_not_working_properly_One_of_its_drivers_or_the_registry_might_be_corrupted = 0x4,
            Driver_for_the_device_requires_a_resource_that_Windows_cannot_manage = 0x5,
            Boot_configuration_for_the_device_conflicts_with_other_devices = 0x6,
            Cannot_filter = 0x7,
            Driver_loader_for_the_device_is_missing = 0x8,
            Device_is_not_working_properly_The_controlling_firmware_is_incorrectly_reporting_the_resources_for_the_device = 0x9,
            Device_cannot_start = 0xA,
            Device_failed = 0xB,
            Device_cannot_find_enough_free_resources_to_use = 0xC,
            Windows_cannot_verify_the_devices_resources = 0xD,
            Device_cannot_work_properly_until_the_computer_is_restarted = 0xE, 
            Device_is_not_working_properly_due_to_a_possible_reenumeration_problem = 0xF, 
            Windows_cannot_identify_all_of_the_resources_that_the_device_uses = 0x10,
            Device_is_requesting_an_unknown_resource_type = 0x11,
            Device_drivers_must_be_reinstalled = 0x12,
            Failure_using_the_VxD_loader = 0x13,
            Registry_might_be_corrupted = 0x14,
            System_failure_If_changing_the_device_driver_is_ineffective_see_the_hardware_documentation_Windows_is_removing_the_device = 0x15,
            Device_is_disabled = 0x16,
            System_failure_If_changing_the_device_driver_is_ineffective_see_the_hardware_documentation = 0x17,
            Device_is_not_present,_not_working_properly_or_does_not_have_all_of_its_drivers_installed = 0x18,
            Windows_is_still_setting_up_the_device = 0x19, 
            Windows_is_still_setting_up_the_device_v = 0x1A,
            Device_does_not_have_valid_log_configuration = 0x1B,
            Device_drivers_are_not_installed = 0x1C,
            Device_is_disabled_The_device_firmware_did_not_provide_the_required_resources = 0x1D,
            Device_is_using_an_IRQ_resource_that_another_device_is_using = 0x1E,
            Device_is_not_working_properly_Windows_cannot_load_the_required_device_drivers = 0x1F
        }
 

    
    
    }

    public static class NativeMethodsHelper
    {

        public static readonly IntPtr LocalServerHandle = IntPtr.Zero;
        #region Delegates

        public delegate void ListProcessInfosCallback(Win32API.WTS_PROCESS_INFO processInfo);

        #endregion

        public static Win32API.ConnectionState GetConnectionState(TerminalServer.ITerminalServerHandle server, int sessionId)
        {
            return QuerySessionInformation(server, sessionId, Win32API.WTS_INFO_CLASS.WTSConnectState,
                                           (mem, returned) => (Win32API.ConnectionState)Marshal.ReadInt32(mem));
        }

        private static T QuerySessionInformation<T>(TerminalServer.ITerminalServerHandle server, int sessionId,
                                                    Win32API.WTS_INFO_CLASS infoClass, ProcessSessionCallback<T> callback)
        {
            int returned;
            IntPtr mem;
            if (Win32API.WTSQuerySessionInformation(server.Handle, sessionId, infoClass, out mem, out returned))
            {
                try
                {
                    return callback(mem, returned);
                }
                finally
                {
                    if (mem != IntPtr.Zero)
                    {
                        Win32API.WTSFreeMemory(mem);
                    }
                }
            }
            throw new System.ComponentModel.Win32Exception();
        }

        public static string QuerySessionInformationForString(TerminalServer.ITerminalServerHandle server, int sessionId,
                                                              Win32API.WTS_INFO_CLASS infoClass)
        {
            return QuerySessionInformation(server, sessionId, infoClass,
                                           (mem, returned) => mem == IntPtr.Zero ? null : Marshal.PtrToStringAuto(mem));
        }

        public static T QuerySessionInformationForStruct<T>(TerminalServer.ITerminalServerHandle server, int sessionId,
                                                            Win32API.WTS_INFO_CLASS infoClass) where T : struct
        {
            return QuerySessionInformation(server, sessionId, infoClass,
                                           (mem, returned) => (T)Marshal.PtrToStructure(mem, typeof(T)));
        }

        public static Win32API.WINSTATIONINFORMATIONW GetWinStationInformation(
            TerminalServer.ITerminalServerHandle server, int sessionId)
        {
            var retLen = 0;
            var wsInfo = new Win32API.WINSTATIONINFORMATIONW();
            if (
                Win32API.WinStationQueryInformation(server.Handle, sessionId,
                                                         (int)Win32API.WINSTATIONINFOCLASS.WinStationInformation, ref wsInfo,
                                                         Marshal.SizeOf(typeof(Win32API.WINSTATIONINFORMATIONW)), ref retLen) !=
                0)
            {
                return wsInfo;
            }
            throw new System.ComponentModel.Win32Exception();
        }

        public static DateTime? FileTimeToDateTime(FILETIME ft)
        {
            var sysTime = new Win32API.SYSTEMTIME();
            if (Win32API.FileTimeToSystemTime(ref ft, ref sysTime) == 0)
            {
                return null;
            }
            if (sysTime.Year < 1900)
            {
                // Must have gotten a bogus date. This happens sometimes on Windows Server 2003.
                return null;
            }
            return
                new DateTime(sysTime.Year, sysTime.Month, sysTime.Day, sysTime.Hour, sysTime.Minute, sysTime.Second,
                             sysTime.Milliseconds, DateTimeKind.Utc).ToLocalTime();
        }

        public static IList<Win32API.WTS_SESSION_INFO> GetSessionInfos(TerminalServer.ITerminalServerHandle server)
        {
            IntPtr ppSessionInfo = IntPtr.Zero;
            int count = 0;

            if (Win32API.WTSEnumerateSessions(server.Handle, 0, 1, ref ppSessionInfo, ref count) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
            try
            {
                return PtrToStructureList<Win32API.WTS_SESSION_INFO>(ppSessionInfo, count);
            }
            finally
            {
                Win32API.WTSFreeMemory(ppSessionInfo);
            }
        }

        public static void LogoffSession(TerminalServer.ITerminalServerHandle server, int sessionId, bool wait)
        {
            if (Win32API.WTSLogoffSession(server.Handle, sessionId, wait) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public static void DisconnectSession(TerminalServer.ITerminalServerHandle server, int sessionId, bool wait)
        {
            if (Win32API.WTSDisconnectSession(server.Handle, sessionId, wait) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public static Win32API.RemoteMessageBoxResult SendMessage(TerminalServer.ITerminalServerHandle server, 
            int sessionId, string title, string message, int style, int timeout, bool wait)
        {
            Win32API.RemoteMessageBoxResult result;
      
            title = string.IsNullOrEmpty(title) ? " " : title;
            message = message ?? string.Empty;
            if (
                Win32API.WTSSendMessage(server.Handle, sessionId, title, title.Length * Marshal.SystemDefaultCharSize,
                                             message, message.Length * Marshal.SystemDefaultCharSize, style, timeout,
                                             out result, wait) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return result;
        }

        public static IList<Win32API.WTS_SERVER_INFO> EnumerateServers(string domainName)
        {
            IntPtr ppServerInfo;
            int count;
            if (Win32API.WTSEnumerateServers(domainName, 0, 1, out ppServerInfo, out count) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
            try
            {
                return PtrToStructureList<Win32API.WTS_SERVER_INFO>(ppServerInfo, count);
            }
            finally
            {
                Win32API.WTSFreeMemory(ppServerInfo);
            }
        }

        private static IList<T> PtrToStructureList<T>(IntPtr ppList, int count) where T : struct
        {
            var result = new List<T>();
            var pointer = ppList.ToInt64();
            var sizeOf = Marshal.SizeOf(typeof(T));
            for (var index = 0; index < count; index++)
            {
                var item = (T)Marshal.PtrToStructure(new IntPtr(pointer), typeof(T));
                result.Add(item);
                pointer += sizeOf;
            }
            return result;
        }

        public static void ForEachProcessInfo(TerminalServer.ITerminalServerHandle server, ListProcessInfosCallback callback)
        {
            IntPtr ppProcessInfo;
            int count;
            if (Win32API.WTSEnumerateProcesses(server.Handle, 0, 1, out ppProcessInfo, out count) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
            try
            {
                // We can't just return a list of WTS_PROCESS_INFOs because those have pointers to 
                // SIDs that have to be copied into managed memory first. So we use a callback instead.
                var processInfos = PtrToStructureList<Win32API.WTS_PROCESS_INFO>(ppProcessInfo, count);
                foreach (Win32API.WTS_PROCESS_INFO processInfo in processInfos)
                {
                    // It seems that WTSEnumerateProcesses likes to return an empty struct in the first 
                    // element of the array, so we ignore that here.
                    // TODO: Find out why this happens.
                    if (processInfo.ProcessId != 0)
                    {
                        callback(processInfo);
                    }
                }
            }
            finally
            {
                Win32API.WTSFreeMemory(ppProcessInfo);
            }
        }

        public static void TerminateProcess(TerminalServer.ITerminalServerHandle server, int processId, int exitCode)
        {
            if (Win32API.WTSTerminateProcess(server.Handle, processId, exitCode) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public static int QuerySessionInformationForInt(TerminalServer.ITerminalServerHandle server, int sessionId,
                                                        Win32API.WTS_INFO_CLASS infoClass)
        {
            return QuerySessionInformation(server, sessionId, infoClass, (mem, returned) => Marshal.ReadInt32(mem));
        }

        public static void ShutdownSystem(TerminalServer.ITerminalServerHandle server, int flags)
        {
            if (Win32API.WTSShutdownSystem(server.Handle, flags) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public static DateTime? FileTimeToDateTime(long fileTime)
        {
            if (fileTime == 0)
            {
                return null;
            }
            return DateTime.FromFileTime(fileTime);
        }

        public static short QuerySessionInformationForShort(TerminalServer.ITerminalServerHandle server, int sessionId,
                                                            Win32API.WTS_INFO_CLASS infoClass)
        {
            return QuerySessionInformation(server, sessionId, infoClass, (mem, returned) => Marshal.ReadInt16(mem));
        }

        public static System.Net.IPAddress ExtractIPAddress(System.Net.Sockets.AddressFamily family, byte[] rawAddress)
        {
            switch (family)
            {
                case System.Net.Sockets.AddressFamily.InterNetwork:
                    var v4Addr = new byte[4];
                    // TODO: I'm not sure what type of address structure this is that we need to start at offset 2.
                    Array.Copy(rawAddress, 2, v4Addr, 0, 4);
                    return new System.Net.IPAddress(v4Addr);
                case System.Net.Sockets.AddressFamily.InterNetworkV6:
                    var v6Addr = new byte[16];
                    Array.Copy(rawAddress, 2, v6Addr, 0, 16);
                    return new System.Net.IPAddress(v6Addr);
            }
            return null;
        }

        public static System.Net.EndPoint QuerySessionInformationForEndPoint(
            TerminalServer.ITerminalServerHandle server, int sessionId)
        {
            int retLen;
            var remoteAddress = new Win32API.WINSTATIONREMOTEADDRESS();
            if (
                Win32API.WinStationQueryInformationRemoteAddress(server.Handle, sessionId,
                                                                      Win32API.WINSTATIONINFOCLASS.WinStationRemoteAddress,
                                                                      ref remoteAddress,
                                                                      Marshal.SizeOf(typeof(Win32API.WINSTATIONREMOTEADDRESS)),
                                                                      out retLen) != 0)
            {
                var ipAddress = ExtractIPAddress(remoteAddress.Family, remoteAddress.Address);
                int port = Win32API.ntohs((ushort)remoteAddress.Port);
                return ipAddress == null ? null : new System.Net.IPEndPoint(ipAddress, port);
            }
            throw new System.ComponentModel.Win32Exception();
        }

        public static void LegacyStartRemoteControl(TerminalServer.ITerminalServerHandle server, 
                            int sessionId, ConsoleKey hotkey,
                                                    Win32API.RemoteControlHotkeyModifiers hotkeyModifiers)
        {
            if (
                Win32API.WinStationShadow(server.Handle, server.ServerName, sessionId, (int)hotkey,
                                               (int)hotkeyModifiers) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public static void StartRemoteControl(TerminalServer.ITerminalServerHandle server, int sessionId, ConsoleKey hotkey,
                                              Win32API.RemoteControlHotkeyModifiers hotkeyModifiers)
        {
            if (
                Win32API.WTSStartRemoteControlSession(server.ServerName, sessionId, (byte)hotkey,
                                                           (short)hotkeyModifiers) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public static void LegacyStopRemoteControl(TerminalServer.ITerminalServerHandle server, int sessionId, bool wait)
        {
            // TODO: Odd that this doesn't return an error code for sessions that do not exist.
            if (Win32API.WinStationShadowStop(server.Handle, sessionId, wait) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public static void StopRemoteControl(int sessionId)
        {
            if (Win32API.WTSStopRemoteControlSession(sessionId) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public static void LegacyConnect(TerminalServer.ITerminalServerHandle server, int sourceSessionId, int targetSessionId,
                                         string password, bool wait)
        {
            if (Win32API.WinStationConnectW(server.Handle, targetSessionId, sourceSessionId, password, wait) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public static void Connect(int sourceSessionId, int targetSessionId, string password, bool wait)
        {
            if (Win32API.WTSConnectSession(targetSessionId, sourceSessionId, password, wait) == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public static int? GetActiveConsoleSessionId()
        {
            var sessionId = Win32API.WTSGetActiveConsoleSessionId();
            return sessionId == -1 ? (int?)null : sessionId;
        }

        #region Nested type: ProcessSessionCallback

        private delegate T ProcessSessionCallback<T>(IntPtr mem, int returnedBytes);

        #endregion
    }

}
