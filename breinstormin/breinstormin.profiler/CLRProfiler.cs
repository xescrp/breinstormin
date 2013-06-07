using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Threading;

namespace breinstormin.profiler
{




    public partial class CLRProfiler
    {

        //Timing checks
        private System.Timers.Timer _ck_check;

        //the process to be profiled
        private System.Diagnostics.Process profiledProcess;

        private uint maxWaitingTimeInMiliseconds = 10000;
        private string processFileName;
        private string serviceName;
        private string serviceAccountSid;
        private string serviceStartCommand;
        private string serviceStopCommand;
        private string logFileName;
        private long logFileStartOffset;
        private long logFileEndOffset;

        internal ReadNewLog log;
        internal ReadLogResult lastLogResult;


        private NamedManualResetEvent loggingActiveEvent;
        private NamedManualResetEvent forceGcEvent;
        private NamedManualResetEvent loggingActiveCompletedEvent;
        private NamedManualResetEvent forceGcCompletedEvent;
        private NamedManualResetEvent callGraphActiveEvent;
        private NamedManualResetEvent callGraphActiveCompletedEvent;
        private string commandLine = "";
        private string workingDirectory = "";
        private string logDirectory;
        private int attachTargetPID;
        private SafeFileHandle handshakingPipeHandle;
        private SafeFileHandle loggingPipeHandle;
        private FileStream handshakingPipe;
        private FileStream loggingPipe;
        private bool gcOnLogFileComments;
        internal static CLRProfiler instance;


        public enum CLRSKU
        {
            V4DesktopCLR,
            V4CoreCLR,
            V2DesktopCLR,
        };

        internal bool noUI = true;
        private string nameToUse;
        private bool profileAllocations, profileCalls, profilingActive;
        private bool trackCallStacks = true;
        private bool profilerConnected = false;
        private CLRSKU targetCLRVersion = CLRSKU.V4DesktopCLR;
        private string profilingURL = null;


        public CLRProfiler() 
        {
            //Inicializar pooling
            _ck_check = new System.Timers.Timer();
            _ck_check.Elapsed += _check_logging;
            _ck_check.Interval = 100;
            _ck_check.Enabled = true;


        }

        void _check_logging(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Check the pipes
            throw new NotImplementedException();
        }


        public void ProfileProcess(string profilingProcessfilename, CLRSKU clrruntime, ProfilingProcessType processtype) 
        {
            processFileName = profilingProcessfilename;

            switch (processtype) 
            {
                case ProfilingProcessType.ASPNET:
                    serviceName = null;
                    break;
                case ProfilingProcessType.Executable:
                    serviceName = null;
                    break;
                case ProfilingProcessType.Service:
                    break;
            }

            if (processFileName == null)
            {
                throw new Exception("The processFileName is empty!");
            }
            else if (processFileName == "ASP.NET")
            {
                profileASP_NET();
                return;
            }
            else if (serviceName != null)
            {
                ProfileService();
                return;
            }

            if (targetv2DesktopCLR())
            {
                RegisterDLL.Register();  // Register profilerOBJ.dll for v2 CLR, which doesn't support registry free activation
            }

            if (processFileName == null)
                return;

            if (profiledProcess == null || ProfiledProcessHasExited())
            {
                System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo(processFileName);
                if (targetv4CoreCLR())
                {
                    processStartInfo.EnvironmentVariables["CoreCLR_Enable_Profiling"] = "0x1";
                    processStartInfo.EnvironmentVariables["CORECLR_PROFILER"] = "{8C29BC4E-1F57-461a-9B51-1200C32E6F1F}";
                    processStartInfo.EnvironmentVariables["CORECLR_PROFILER_PATH"] = getProfilerFullPath();
                }
                else
                {
                    processStartInfo.EnvironmentVariables["Cor_Enable_Profiling"] = "0x1";
                    processStartInfo.EnvironmentVariables["COR_PROFILER"] = "{8C29BC4E-1F57-461a-9B51-1200C32E6F1F}";
                    processStartInfo.EnvironmentVariables["COR_PROFILER_PATH"] = getProfilerFullPath();
                }

                processStartInfo.EnvironmentVariables["OMV_USAGE"] = CreateUsageString();
                processStartInfo.EnvironmentVariables["OMV_SKIP"] = "0";
                processStartInfo.EnvironmentVariables["OMV_PATH"] = GetLogDir();
                processStartInfo.EnvironmentVariables["OMV_STACK"] = trackCallStacks ? "1" : "0";
                processStartInfo.EnvironmentVariables["OMV_FORMAT"] = "v2";
                processStartInfo.EnvironmentVariables["OMV_DynamicObjectTracking"] = "0x1";
                processStartInfo.EnvironmentVariables["OMV_FORCE_GC_ON_COMMENT"] = gcOnLogFileComments ? "1" : "0";
                processStartInfo.EnvironmentVariables["OMV_INITIAL_SETTING"] = CreateInitialString();
                processStartInfo.EnvironmentVariables["OMV_TargetCLRVersion"] = targetv2DesktopCLR() ? "v2" : "v4";


                if (commandLine != null)
                    processStartInfo.Arguments = commandLine;

                if (workingDirectory != null)
                    processStartInfo.WorkingDirectory = workingDirectory;

                processStartInfo.UseShellExecute = false;

                profiledProcess = System.Diagnostics.Process.Start(processStartInfo);

                if (WaitForProcessToConnect(GetLogDir(), "Waiting for application to start common language runtime") <= 0)
                    profiledProcessEnded();
            }

        }

        private void profileASP_NET()
        {

            if (targetv2DesktopCLR())
            {
                RegisterDLL.Register();  // Register profilerOBJ.dll for v2 CLR, which doesn't support registry free activation
            }

            StopIIS();

            // set environment variables

            string logDir = GetLogDir();
            string[] profilerEnvironment = CreateProfilerEnvironment(logDir);

            string[] baseEnvironment = GetServicesEnvironment();
            baseEnvironment = ReplaceTempDir(baseEnvironment, GetLogDir());
            string[] combinedEnvironment = CombineEnvironmentVariables(baseEnvironment, profilerEnvironment);
            SetEnvironmentVariables("IISADMIN", combinedEnvironment);
            SetEnvironmentVariables("W3SVC", combinedEnvironment);
            SetEnvironmentVariables("WAS", combinedEnvironment);

            string asp_netAccountName = GetASP_NETaccountName();
            string asp_netAccountSid = null;
            if (asp_netAccountName != null)
            {
                asp_netAccountSid = LookupAccountSid(asp_netAccountName);
                if (asp_netAccountSid != null)
                    SetAccountEnvironment(asp_netAccountSid, profilerEnvironment);
            }

            if (StartIIS())
            {
                // wait for worker process to start up and connect
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profile",
                    "Waiting for ASP.NET worker process to start up");

                Thread.Sleep(1000);
                int pid = WaitForProcessToConnect(logDir, 
                    "Waiting for ASP.NET to start common language runtime - this is the time to load your test page");

                if (pid > 0)
                {
                    profiledProcess = System.Diagnostics.Process.GetProcessById(pid);

                    breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profile", "Profiling: ASP.NET");
                    breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profile", "Start ASP.NET");

                    processFileName = "ASP.NET";
                }
            }

            /* Delete the environment variables as early as possible, so that even if CLRProfiler crashes, the user's machine
             * won't be screwed up.
             * */
            DeleteEnvironmentVariables("IISADMIN");
            DeleteEnvironmentVariables("W3SVC");
            DeleteEnvironmentVariables("WAS");

            if (asp_netAccountSid != null)
                ResetAccountEnvironment(asp_netAccountSid, profilerEnvironment);

            serviceName = null;
        }

        private void ProfileService()
        {
          

            if (targetv2DesktopCLR())
            {
                RegisterDLL.Register();  // Register profilerOBJ.dll for v2 CLR, which doesn't support registry free activation
            }

            StopService(serviceName, serviceStopCommand);

            string logDir = GetLogDir();
            string[] profilerEnvironment = CreateProfilerEnvironment(logDir);

            // set environment variables

            // this is a bit intricate - if the service is running as LocalSystem, we need to set the environment
            // variables in the registry for the service, otherwise it's better to temporarily set it for the account,
            // assuming we can find out the account SID
            string serviceAccountName = GetServiceAccountName(serviceName);
            if (serviceAccountName.StartsWith(@".\"))
                serviceAccountName = Environment.MachineName + serviceAccountName.Substring(1);
            if (serviceAccountName != null && serviceAccountName != "LocalSystem")
            {
                serviceAccountSid = LookupAccountSid(serviceAccountName);
            }
            if (serviceAccountSid != null)
            {
                SetAccountEnvironment(serviceAccountSid, profilerEnvironment);
            }
            else
            {
                string[] baseEnvironment = GetServicesEnvironment();
                baseEnvironment = ReplaceTempDir(baseEnvironment, GetLogDir());
                string[] combinedEnvironment = CombineEnvironmentVariables(baseEnvironment, profilerEnvironment);
                SetEnvironmentVariables(serviceName, combinedEnvironment);
            }

            System.Diagnostics.Process cmdProcess = StartService(serviceName, serviceStartCommand);

            // wait for service to start up and connect
            breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profile", 
                string.Format("Waiting for {0} to start up", serviceName));

            Thread.Sleep(1000);
            int pid = WaitForProcessToConnect(logDir, "Waiting for service to start common language runtime");
            if (pid > 0)
            {
                profiledProcess = System.Diagnostics.Process.GetProcessById(pid);

                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profile", "Profiling: " + serviceName);
                breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profile", "Start " + serviceName);
                
                processFileName = serviceName;
            }

            /* Delete the environment variables as early as possible, so that even if CLRProfiler crashes, the user's machine
             * won't be screwed up.
             * */

            if (serviceAccountSid != null)
            {
                ResetAccountEnvironment(serviceAccountSid, profilerEnvironment);
            }
            else
            {
                DeleteEnvironmentVariables(serviceName);
            }
        }


        private int WaitForProcessToConnect(string tempDir, string text, bool attachMode = false, uint result = 0)
        {
            bool fProfiledProcessInitialized = profiledProcess != null;

            ConnectNamedPipe(handshakingPipeHandle, IntPtr.Zero);
            ConnectNamedPipe(loggingPipeHandle, IntPtr.Zero);

            int pid = 0;
            byte[] handshakingBuffer = new byte[9];
            int handshakingReadBytes = 0;

            // IMPORTANT: maxloggingBufferSize must match bufferSize defined in ProfilerCallback.cpp.
            const int maxloggingBufferSize = 512;
            byte[] loggingBuffer = new byte[maxloggingBufferSize];
            int loggingReadBytes = 0;
            //WaitingForConnectionForm waitingForConnectionForm = null;
            int beginTickCount = Environment.TickCount;

            //Do not show the text in attachmode 
            if (attachMode == false)
            {
                if (noUI)
                {
                    breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", text);
                }
                else
                {
                    
                }
            }


            // loop reading two pipes,
            // until   
            //  (1)successfully connected 
            //  (2)User canceled
            //  (3)attach failed
            //  (4)target process exited
            while (true)
            {
                #region handshaking
                //(1)succeeded
                try
                {
                    handshakingReadBytes += handshakingPipe.Read(handshakingBuffer, handshakingReadBytes, 9 - handshakingReadBytes);
                }
                catch (System.IO.IOException)
                {
                }

                //Read 9 bytes from handshaking pipe
                //means the profielr was initialized successfully
                if (handshakingReadBytes == 9)
                    break;


                #endregion handshaking
                #region logging
                //  (3)attach failed
                //  (3.1) read logging message
                //  (3.2) break if attach failed.

                //  (3.1) read logging message
                try
                {
                    loggingReadBytes += loggingPipe.Read(loggingBuffer, loggingReadBytes, maxloggingBufferSize - loggingReadBytes);
                }
                catch (System.IO.IOException ex)
                {
                    breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", ex.ToString());
                }

                if (loggingReadBytes == maxloggingBufferSize)
                {
                    char[] charBuffer = new char[loggingReadBytes];
                    for (int i = 0; i < loggingReadBytes; i++)
                        charBuffer[i] = Convert.ToChar(loggingBuffer[i]);

                    string message = new String(charBuffer, 0, loggingReadBytes);

                    if (attachMode == false && noUI == false)
                    {
                        //waitingForConnectionForm.addMessage(message);
                    }
                    else
                    {
                        breinstormin.tools.log.LogEngine.WriteLog("breinstormin.profiler", message);
                    }

                    loggingReadBytes = 0;

                    while (true)
                    {
                        try
                        {
                            if (loggingPipe.Read(loggingBuffer, 0, 1) == 0)
                            {
                                DisconnectNamedPipe(loggingPipeHandle);
                                ConnectNamedPipe(loggingPipeHandle, IntPtr.Zero);
                                break;
                            }
                        }
                        catch (System.IO.IOException)
                        {
                            DisconnectNamedPipe(loggingPipeHandle);
                            ConnectNamedPipe(loggingPipeHandle, IntPtr.Zero);
                            break;
                        }
                    }
                }
                //  (3.2) break if attach failed.
                if (attachMode == true && result != 0)
                {
                    pid = -1;
                    break;
                }
                #endregion logging
                
                
                //  (4)target process exited
                if ((fProfiledProcessInitialized && profiledProcess == null) || 
                    (profiledProcess != null && ProfiledProcessHasExited()))
                {
                    pid = -1;
                    break;
                }
                Thread.Sleep(100);
            }



            if (pid == -1)
                return pid;
            if (handshakingReadBytes == 9)
            {
                char[] charBuffer = new char[9];
                for (int i = 0; i < handshakingBuffer.Length; i++)
                    charBuffer[i] = Convert.ToChar(handshakingBuffer[i]);
                pid = Int32.Parse(new String(charBuffer, 0, 8), System.Globalization.NumberStyles.HexNumber);

                CreateEvents(pid);

                string fileName = getLogFileName(pid);
                byte[] fileNameBuffer = new Byte[fileName.Length + 1];
                for (int i = 0; i < fileName.Length; i++)
                    fileNameBuffer[i] = (byte)fileName[i];

                fileNameBuffer[fileName.Length] = 0;
                handshakingPipe.Write(fileNameBuffer, 0, fileNameBuffer.Length);
                handshakingPipe.Flush();
                logFileName = tempDir + "\\" + fileName;
                log = new ReadNewLog(logFileName);
                lastLogResult = null;
                ObjectGraph.cachedGraph = null;
                
                
                while (true)
                {
                    try
                    {
                        if (handshakingPipe.Read(handshakingBuffer, 0, 1) == 0) // && GetLastError() == 109/*ERROR_BROKEN_PIPE*/)
                        {
                            DisconnectNamedPipe(handshakingPipeHandle);
                            ConnectNamedPipe(handshakingPipeHandle, IntPtr.Zero);
                            break;
                        }
                    }
                    catch (System.IO.IOException)
                    {
                        DisconnectNamedPipe(handshakingPipeHandle);
                        ConnectNamedPipe(handshakingPipeHandle, IntPtr.Zero);
                        break;
                    }
                }
            }
            else
            {
                string error = string.Format("Error {0} occurred", GetLastError());
                
            }

            
            logFileStartOffset = 0;
            logFileEndOffset = long.MaxValue;
            profilerConnected = true;

            return pid;
        }



        private void readLogFile(ReadNewLog log, ReadLogResult logResult, string exeName, Graph.GraphType graphType)
        {
            log.ReadFile(logFileStartOffset, logFileEndOffset, logResult);
            //ViewGraph(logResult, exeName, graphType);
        }

        public ReadLogResult DumpResults()
        {
            forceGcCompletedEvent.Wait(1);
            forceGcCompletedEvent.Reset();

            long startOffset = logFileOffset();
            forceGcEvent.Set();
            const int maxIter = 10; // give up after ten minutes
            for (int iter = 0; iter < maxIter; iter++)
            {
                long lastOffset = logFileOffset();
                if (forceGcCompletedEvent.Wait(60 * 1000))
                {
                    forceGcCompletedEvent.Reset();
                    long saveLogFileStartOffset = logFileStartOffset;
                    logFileStartOffset = startOffset;
                    logFileEndOffset = logFileOffset();
                    ReadLogResult logResult = GetLogResult();
                    readLogFile(log, logResult, processFileName, Graph.GraphType.HeapGraph);
                    lastLogResult = logResult;
                    
                    logFileStartOffset = saveLogFileStartOffset;
                    break;
                }
                else
                {
                    // Hmm, the app didn't get back to us in 60 seconds
                    // If the log file is growing, assume the app is still dumping
                    // the heap, otherwise something is obviously wrong.
                    if (logFileOffset() == lastOffset)
                    {
                        throw new Exception("There was no response from the application");
                    }
                }
            }
            return lastLogResult;
        }


    }
}
