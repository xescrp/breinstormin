using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace breinstormin.profiler
{

    public enum ProfilingProcessType 
    {
        Executable, ASPNET, Service
    }

    [Flags]
    public enum OmvUsage : int
    {
        OmvUsageNone = 0,
        OmvUsageObjects = 1,
        OmvUsageTrace = 2,
        OmvUsageBoth = 3
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ProfConfig
    {
        public OmvUsage usage;
        public int bOldFormat;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szPath;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szFileName;
        public int bDynamic;
        public int bStack;
        public uint dwFramesToPrint;
        public uint dwSkipObjects;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szClassToMonitor;
        public uint dwInitialSetting;
        public uint dwDefaultTimeoutMs;
    }


    internal enum InterestLevel
    {
        Ignore = 0,
        Display = 1 << 0,
        Interesting = 1 << 1,
        Parents = 1 << 2,
        Children = 1 << 3,
        InterestingParents = Interesting | Parents,
        InterestingChildren = Interesting | Children,
        ParentsChildren = Parents | Children,
    }

    public partial class CLRProfiler
    {

        #region helpers

        private ReadLogResult GetLogResult()
        {
            ReadLogResult readLogResult = lastLogResult;
            if (readLogResult == null)
            {
                readLogResult = new ReadLogResult();
            }
            readLogResult.liveObjectTable = new LiveObjectTable(log);
            readLogResult.sampleObjectTable = new SampleObjectTable(log);
            readLogResult.allocatedHistogram = new Histogram(log);
            readLogResult.callstackHistogram = new Histogram(log);
            readLogResult.relocatedHistogram = new Histogram(log);
            readLogResult.finalizerHistogram = new Histogram(log);
            readLogResult.criticalFinalizerHistogram = new Histogram(log);
            readLogResult.createdHandlesHistogram = new Histogram(log);
            readLogResult.destroyedHandlesHistogram = new Histogram(log);
            if (readLogResult.objectGraph != null)
                readLogResult.objectGraph.Neuter();
            readLogResult.objectGraph = new ObjectGraph(log, 0);
            readLogResult.functionList = new FunctionList(log);
            readLogResult.hadCallInfo = readLogResult.hadAllocInfo = false;
            readLogResult.handleHash = new Dictionary<ulong, HandleInfo>();

            // We may just have turned a lot of data into garbage - let's try to reclaim the memory
            GC.Collect();

            return readLogResult;
        }

        private long logFileOffset()
        {
            Stream s = new FileStream(logFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            long offset = s.Length;
            s.Close();

            return offset;
        }

        private bool ProfiledProcessHasExited()
        {
            if (profiledProcess == null)
                return true;

            try
            {
                return profiledProcess.HasExited;
            }
            catch
            {
                return System.Diagnostics.Process.GetProcessById(profiledProcess.Id) == null;
            }

        }

        private string getLogFileName(int pid)
        {
            return (nameToUse == null || nameToUse == "" ? string.Format("pipe_{0}.log", pid) : nameToUse);
        }

        private string getProfilerFullPath()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\ProfilerOBJ.dll";
        }

        private bool isProfilerLoaded(int pid)
        {
            NamedManualResetEvent forceGCEvent = new NamedManualResetEvent(string.Format("{0}_{1:x8}", "Global\\OMV_ForceGC", pid),						false, false);
            bool result = forceGCEvent.IsValid();
            forceGCEvent.Dispose();
            return result;
        }

        private string GetLogDir()
        {
            if (logDirectory != null)
                return logDirectory;

            string tempDir = null;
            string winDir = Environment.GetEnvironmentVariable("WINDIR");
            if (winDir != null)
            {
                tempDir = winDir + @"\TEMP";
                if (!Directory.Exists(tempDir))
                    tempDir = null;
            }
            if (tempDir == null)
            {
                tempDir = Environment.GetEnvironmentVariable("TEMP");
                if (tempDir == null)
                {
                    tempDir = Environment.GetEnvironmentVariable("TMP");
                    if (tempDir == null)
                        tempDir = @"C:\TEMP";
                }
            }
            return tempDir;
        }

        private string GetLogFileName(int pid)
        {
            return GetLogDir() + "\\" + getLogFileName(pid);
        }

        private void profiledProcessEnded()
        {
            profiledProcess = null;
            profilerConnected = false;
        }

        private bool attachProfiler(int pid, string fileName)
        {
            if (isProfilerLoaded(pid))
            {
                throw new Exception("CLRProfiler is already loaded in the target process.");
            }

            ProfConfig config = new ProfConfig();
            config.usage = OmvUsage.OmvUsageNone;
            config.bOldFormat = 0;
            config.szFileName = fileName;
            config.bDynamic = 0;
            config.bStack = 0;
            config.dwSkipObjects = 0;
            config.szClassToMonitor = String.Empty;
            config.dwInitialSetting = 0;
            config.dwDefaultTimeoutMs = maxWaitingTimeInMiliseconds;

            uint result = AttachProfiler(pid, "v4.", getProfilerFullPath(), ref config, noUI);

            profiledProcess = System.Diagnostics.Process.GetProcessById(pid);
            if (WaitForProcessToConnect(GetLogDir(), "Waiting for application to load the CLRProfiler", true, result) > 0)
            {
                return true;
            }
            else
            {
                profiledProcessEnded();
                return false;
            }
        }

        private int getPID(string[] arguments)
        {
            if (arguments.Length == 1)
            {
                Console.WriteLine("Please specify the process ID");
                return 0;
            }
            int pid = 0;
            try
            {
                pid = Int32.Parse(arguments[1]);
                System.Diagnostics.Process.GetProcessById(pid);
            }
            catch (Exception e)
            {
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", e.ToString());
                pid = 0;
            }

            return pid;
        }


        


        private string CreateUsageString()
        {
            int index = 0;
            string[] usageStrings = new string[] { "none", "objects", "trace", "both" };
            if ((noUI && profileAllocations) || (!noUI && true))
            {
                index |= 1;
            }
            if ((noUI && profileCalls) || (!noUI && true))
            {
                index |= 2;
            }

            return usageStrings[index];
        }

        private string CreateInitialString()
        {
            int flags = 0;
            if ((noUI && profileAllocations && profilingActive) || (!noUI && true))
            {
                flags |= 1;
            }
            if ((noUI && profileCalls && profilingActive) || (!noUI && true))
            {
                flags |= 2;
            }
            return flags.ToString();
        }


        private bool targetv4DesktopCLR()
        {
            return (noUI && targetCLRVersion == CLRSKU.V4DesktopCLR);
        }

        private bool targetv4CoreCLR()
        {
            return (noUI && targetCLRVersion == CLRSKU.V4CoreCLR);
        }

        private bool targetv2DesktopCLR()
        {
            return (noUI && targetCLRVersion == CLRSKU.V2DesktopCLR);
        }


        private string[] CreateProfilerEnvironment(string tempDir)
        {
            return new string[]
            { 
                "Cor_Enable_Profiling=0x1",
                "COR_PROFILER={8C29BC4E-1F57-461a-9B51-1200C32E6F1F}",
                "COR_PROFILER_PATH=" + getProfilerFullPath(),
                "OMV_SKIP=0",
                "OMV_FORMAT=v2",
                "OMV_STACK=" + (trackCallStacks ? "1" : "0"),
                "OMV_DynamicObjectTracking=0x1",
                "OMV_PATH=" + tempDir,
                "OMV_USAGE=" + CreateUsageString(),
                "OMV_FORCE_GC_ON_COMMENT=" + (gcOnLogFileComments ? "1" : "0"),
                "OMV_INITIAL_SETTING=" + CreateInitialString(),
                "OMV_TargetCLRVersion=" + (targetv2DesktopCLR() ? "v2" : "v4")
            };
        }


        private string GetASP_NETaccountName()
        {
            try
            {
                System.Xml.XmlDocument machineConfig = new System.Xml.XmlDocument();
                string runtimePath = RuntimeEnvironment.GetRuntimeDirectory();
                string configPath = Path.Combine(runtimePath, @"CONFIG\machine.config");
                machineConfig.Load(configPath);
                System.Xml.XmlNodeList elemList = machineConfig.GetElementsByTagName("processModel");
                for (int i = 0; i < elemList.Count; i++)
                {
                    System.Xml.XmlAttributeCollection attributes = elemList[i].Attributes;
                    System.Xml.XmlAttribute userNameAttribute = attributes["userName"];
                    if (userNameAttribute != null)
                    {
                        string userName = userNameAttribute.InnerText;
                        if (userName == "machine")
                            return "ASPNET";
                        else if (userName == "SYSTEM")
                            return null;
                        else
                            return userName;
                    }
                }
            }
            catch
            {
                // swallow all exceptions here
            }
            return "ASPNET";
        }

        #endregion


        #region pipes & events
        private bool CreatePipe(string pipeName, bool blockingPipe, ref SafeFileHandle pipeHandle, ref FileStream pipe)
        {
            SECURITY_ATTRIBUTES sa;
            sa.nLength = 12;
            sa.bInheritHandle = 0;
            if (!ConvertStringSecurityDescriptorToSecurityDescriptor("D: (A;OICI;GRGW;;;AU)", 1,
                out sa.lpSecurityDescriptor, IntPtr.Zero))
                return false;
            uint flags = 4 | 2 | 0;

            if (!blockingPipe)
                flags |= 1;
            pipeHandle = CreateNamedPipe(pipeName, 3, flags, 1, 512, 512, 1000, ref sa);
            LocalFree(sa.lpSecurityDescriptor);
            if (pipeHandle.IsInvalid)
                return false;
            pipe = new FileStream(pipeHandle, FileAccess.ReadWrite, 512, false);
            return true;
        }

        private void ClosePipe(ref SafeFileHandle pipeHandle, ref FileStream pipe)
        {
            pipe.Close();
            pipe = null;
            pipeHandle = null;
        }

        private NamedManualResetEvent CreateEvent(string baseName, int pid)
        {
            string eventName = string.Format("{0}_{1:x8}", baseName, pid);
            return new NamedManualResetEvent(eventName, false, true);
        }

        private void CreateEvents(int pid)
        {
            try
            {
                loggingActiveEvent = CreateEvent("Global\\OMV_TriggerObjects", pid);
                loggingActiveCompletedEvent = CreateEvent("Global\\OMV_TriggerObjects_Completed", pid);
                forceGcEvent = CreateEvent("Global\\OMV_ForceGC", pid);
                forceGcCompletedEvent = CreateEvent("Global\\OMV_ForceGC_Completed", pid);
                callGraphActiveEvent = CreateEvent("Global\\OMV_Callgraph", pid);
                callGraphActiveCompletedEvent = CreateEvent("Global\\OMV_Callgraph_Completed", pid);
            }
            catch
            {
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler",
                    "Could not create events - in case you are profiling a service, " +
                     "start the profiler BEFORE starting the service");
                throw;
            }
        }

        private void ClearEvents()
        {
            loggingActiveEvent.Dispose();
            loggingActiveEvent = null;
            loggingActiveCompletedEvent.Dispose();
            loggingActiveCompletedEvent = null;
            forceGcEvent.Dispose();
            forceGcEvent = null;
            forceGcCompletedEvent.Dispose();
            forceGcCompletedEvent = null;
            callGraphActiveEvent.Dispose();
            callGraphActiveEvent = null;
            callGraphActiveCompletedEvent.Dispose();
            callGraphActiveCompletedEvent = null;
        }

        #endregion


        #region profiler api

        [DllImport("profilerOBJ.dll", CharSet = CharSet.Unicode)]
        private static extern uint AttachProfiler(int pid, string targetVersion,
            string profilerPath, [In] ref ProfConfig profConfig, bool fConsoleMode);


        #endregion


        #region windows api

        struct SECURITY_ATTRIBUTES
        {
            public uint nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        };

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern SafeFileHandle CreateNamedPipe(
            string lpName,         // pointer to pipe name
            uint dwOpenMode,       // pipe open mode
            uint dwPipeMode,       // pipe-specific modes
            uint nMaxInstances,    // maximum number of instances
            uint nOutBufferSize,   // output buffer size, in bytes
            uint nInBufferSize,    // input buffer size, in bytes
            uint nDefaultTimeOut,  // time-out time, in milliseconds
            ref SECURITY_ATTRIBUTES lpSecurityAttributes  // pointer to security attributes
            );

        [DllImport("Kernel32.dll")]
        private static extern IntPtr OpenProcess(
            uint dwDesiredAccess,  // access flag
            bool bInheritHandle,    // handle inheritance option
            int dwProcessId       // process identifier
            );

        [DllImport("Advapi32.dll")]
        private static extern bool OpenProcessToken(
            IntPtr ProcessHandle,
            uint DesiredAccess,
            ref IntPtr TokenHandle
            );

        [DllImport("UserEnv.dll")]
        private static extern bool CreateEnvironmentBlock(
                out IntPtr lpEnvironment,
                IntPtr hToken,
                bool bInherit);

        [DllImport("UserEnv.dll")]
        private static extern bool DestroyEnvironmentBlock(
                IntPtr lpEnvironment);

        [DllImport("Advapi32.dll")]
        private static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(
            string StringSecurityDescriptor,
            uint StringSDRevision,
            out IntPtr SecurityDescriptor,
            IntPtr SecurityDescriptorSize
            );

        [DllImport("Kernel32.dll")]
        private static extern bool LocalFree(IntPtr ptr);

        [DllImport("Advapi32.dll")]
        private static extern bool ConvertSidToStringSidW(byte[] sid, out IntPtr stringSid);

        [DllImport("Advapi32.dll")]
        private static extern bool LookupAccountName(string machineName, string accountName, byte[] sid,
                                 ref int sidLen, StringBuilder domainName, ref int domainNameLen, out int peUse);

        [DllImport("Kernel32.dll")]
        private static extern bool ConnectNamedPipe(
            SafeFileHandle hNamedPipe,  // handle to named pipe to connect
            IntPtr lpOverlapped         // pointer to overlapped structure
            );

        [DllImport("Kernel32.dll")]
        private static extern bool DisconnectNamedPipe(
            SafeFileHandle hNamedPipe   // handle to named pipe
            );

        [DllImport("Kernel32.dll")]
        private static extern int GetLastError();

        [DllImport("Kernel32.dll")]
        private static extern bool ReadFile(
            IntPtr hFile,                // handle of file to read
            byte[] lpBuffer,             // pointer to buffer that receives data
            uint nNumberOfBytesToRead,  // number of bytes to read
            out uint lpNumberOfBytesRead, // pointer to number of bytes read
            IntPtr lpOverlapped    // pointer to structure for data
            );

        #endregion

        #region management
        private void StopIIS()
        {
            // stop IIS
            breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", "Stopping IIS ");
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            if (Environment.OSVersion.Version.Major >= 6/*Vista*/)
                processStartInfo.Arguments = "/c net stop was /y";
            else
                processStartInfo.Arguments = "/c net stop iisadmin /y";
            Process process = Process.Start(processStartInfo);
            while (!process.HasExited)
            {
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", ".");
                Thread.Sleep(100);
                
            }
            if (process.ExitCode != 0)
            {
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", 
                    string.Format(" Error {0} occurred", process.ExitCode));
            }
            else
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", "IIS stopped");
        }

        private bool StartIIS()
        {
            breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", "Starting IIS ");
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.Arguments = "/c net start w3svc";
            Process process = Process.Start(processStartInfo);
            while (!process.HasExited)
            {
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler",  ".");
                Thread.Sleep(100);
                
            }
            if (process.ExitCode != 0)
            {
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", 
                    string.Format(" Error {0} occurred", process.ExitCode));
                return false;
            }
            breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", "IIS running");
            return true;
        }

        private void StopService(string serviceName, string stopCommand)
        {
            // stop service
            breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", "Stopping " + serviceName + " ");
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.Arguments = "/c " + stopCommand;
            Process process = Process.Start(processStartInfo);
            while (!process.HasExited)
            {
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", ".");
                Thread.Sleep(1000);
            }
            if (process.ExitCode != 0)
            {
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", 
                    string.Format(" Error {0} occurred", process.ExitCode));
            }
            else
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", serviceName + " stopped");
        }

        private System.Diagnostics.Process StartService(string serviceName, string startCommand)
        {
            breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler",  "Starting " + serviceName + " ");
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.Arguments = "/c " + startCommand;
            Process process = Process.Start(processStartInfo);
            return process;
        }

        private string GetServiceAccountName(string serviceName)
        {
            Microsoft.Win32.RegistryKey key = GetServiceKey(serviceName);
            if (key != null)
                return key.GetValue("ObjectName") as string;
            return null;
        }

        private string LookupAccountSid(string accountName)
        {
            int sidLen = 0;
            byte[] sid = new byte[sidLen];
            int domainNameLen = 0;
            int peUse;
            StringBuilder domainName = new StringBuilder();
            LookupAccountName(Environment.MachineName, accountName, sid, ref sidLen, domainName, ref domainNameLen, out peUse);

            sid = new byte[sidLen];
            domainName = new StringBuilder(domainNameLen);
            string stringSid = null;
            if (LookupAccountName(Environment.MachineName, accountName, sid, ref sidLen, domainName, ref domainNameLen, out peUse))
            {
                IntPtr stringSidPtr;
                if (ConvertSidToStringSidW(sid, out stringSidPtr))
                {
                    try
                    {
                        stringSid = Marshal.PtrToStringUni(stringSidPtr);
                    }
                    finally
                    {
                        LocalFree(stringSidPtr);
                    }
                }
            }
            return stringSid;
        }

        private string[] ReplaceTempDir(string[] env, string newTempDir)
        {
            for (int i = 0; i < env.Length; i++)
            {
                if (env[i].StartsWith("TEMP="))
                    env[i] = "TEMP=" + newTempDir;
                else if (env[i].StartsWith("TMP="))
                    env[i] = "TMP=" + newTempDir;
            }
            return env;
        }

        private static unsafe int wcslen(char* s)
        {
            char* e;
            for (e = s; *e != '\0'; e++)
                ;
            return (int)(e - s);
        }

        private string[] GetServicesEnvironment()
        {
            System.Diagnostics.Process[] servicesProcesses = System.Diagnostics.Process.GetProcessesByName("services");
            if (servicesProcesses == null || servicesProcesses.Length != 1)
            {
                servicesProcesses = System.Diagnostics.Process.GetProcessesByName("services.exe");
                if (servicesProcesses == null || servicesProcesses.Length != 1)
                    return new string[0];
            }
            System.Diagnostics.Process servicesProcess = servicesProcesses[0];
            IntPtr processHandle = OpenProcess(0x20400, false, servicesProcess.Id);
            if (processHandle == IntPtr.Zero)
                return new string[0];
            IntPtr tokenHandle = IntPtr.Zero;
            if (!OpenProcessToken(processHandle, 0x20008, ref tokenHandle))
                return new string[0];
            IntPtr environmentPtr = IntPtr.Zero;
            if (!CreateEnvironmentBlock(out environmentPtr, tokenHandle, false))
                return new String[0];
            unsafe
            {
                string[] envStrings = null;
                // rather than duplicate the code that walks over the environment, 
                // we have this funny loop where the first iteration just counts the strings,
                // and the second iteration fills in the strings
                for (int i = 0; i < 2; i++)
                {
                    char* env = (char*)environmentPtr.ToPointer();
                    int count = 0;
                    while (true)
                    {
                        int len = wcslen(env);
                        if (len == 0)
                            break;
                        if (envStrings != null)
                            envStrings[count] = new String(env);
                        count++;
                        env += len + 1;
                    }
                    if (envStrings == null)
                        envStrings = new string[count];
                }
                return envStrings;
            }
        }

        private string[] CombineEnvironmentVariables(string[] a, string[] b)
        {
            string[] c = new string[a.Length + b.Length];
            int i = 0;
            foreach (string s in a)
                c[i++] = s;
            foreach (string s in b)
                c[i++] = s;
            return c;
        }

        private Microsoft.Win32.RegistryKey GetServiceKey(string serviceName)
        {
            Microsoft.Win32.RegistryKey localMachine = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey key = localMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\" + serviceName, true);
            return key;
        }

        private void SetEnvironmentVariables(string serviceName, string[] environment)
        {
            Microsoft.Win32.RegistryKey key = GetServiceKey(serviceName);
            if (key != null)
                key.SetValue("Environment", environment);
        }

        private void DeleteEnvironmentVariables(string serviceName)
        {
            Microsoft.Win32.RegistryKey key = GetServiceKey(serviceName);
            if (key != null)
                key.DeleteValue("Environment");
        }

        private string EnvKey(string envVariable)
        {
            int index = envVariable.IndexOf('=');
            //Debug.Assert(index >= 0);
            return envVariable.Substring(0, index);
        }

        private string EnvValue(string envVariable)
        {
            int index = envVariable.IndexOf('=');
            //Debug.Assert(index >= 0);
            return envVariable.Substring(index + 1);
        }

        private Microsoft.Win32.RegistryKey GetAccountEnvironmentKey(string serviceAccountSid)
        {
            Microsoft.Win32.RegistryKey users = Microsoft.Win32.Registry.Users;
            return users.OpenSubKey(serviceAccountSid + @"\Environment", true);
        }

        private void SetAccountEnvironment(string serviceAccountSid, string[] profilerEnvironment)
        {
            Microsoft.Win32.RegistryKey key = GetAccountEnvironmentKey(serviceAccountSid);
            if (key != null)
            {
                foreach (string envVariable in profilerEnvironment)
                {
                    key.SetValue(EnvKey(envVariable), EnvValue(envVariable));
                }
            }
        }

        private void ResetAccountEnvironment(string serviceAccountSid, string[] profilerEnvironment)
        {
            Microsoft.Win32.RegistryKey key = GetAccountEnvironmentKey(serviceAccountSid);
            if (key != null)
            {
                foreach (string envVariable in profilerEnvironment)
                {
                    key.DeleteValue(EnvKey(envVariable));
                }
            }
        }
        #endregion
    }
}
