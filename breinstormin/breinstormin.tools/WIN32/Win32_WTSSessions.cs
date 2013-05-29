using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Tools.WIN32
{
    public class Win32_WTSSessions
    {
        string _server;
        IntPtr _server_pointer;

        public Win32_WTSSessions() 
        {
            _server = "localhost";
        }
        public Win32_WTSSessions(string server) 
        {
            _server = server;
        }

        public void Connect() 
        {
            _server_pointer = Win32API.WTSOpenServer(_server);
        }

        public void Disconnect() 
        {
            Win32API.WTSCloseServer(_server_pointer);
        }

        public Win32_WTSSession[] GetSessions() 
        {
            List<Win32_WTSSession> _sessions = new List<Win32_WTSSession>();
            if (_server_pointer != IntPtr.Zero) 
            {


                    IntPtr ppSessionInfo = IntPtr.Zero;

                    Int32 count = 0;
                    Int32 retval = Win32API.WTSEnumerateSessions(_server_pointer, 0, 1, ref ppSessionInfo, ref count);
                    //Necesitamos el tamaño del tipo o clase para separarlos en objetos distintos
                    Int32 dataSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Win32API.WTS_SESSION_INFO));

                    Int64 current = (int)ppSessionInfo;

                    if (retval != 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            //Esto obtiene el objeto o estructura a partir del puntero
                            Win32API.WTS_SESSION_INFO si =
                                (Win32API.WTS_SESSION_INFO)System.Runtime.InteropServices.Marshal.PtrToStructure(
                                (System.IntPtr)current, typeof(Win32API.WTS_SESSION_INFO));
                            //Movemos a siguiente posicion (determinada por el tamaño del objeto)
                            current += dataSize;

                            _sessions.Add(new Win32_WTSSession(si.pWinStationName, si.SessionID, si.State));
                        }

                        Win32API.WTSFreeMemory(ppSessionInfo);
                    }
       


            }

            return _sessions.ToArray();
        }

    }

    public class Win32_WTSSession 
    {
        public string WorkstationName { get; set; }
        public Int32 SessionID { get; set; }
        public Win32API.WTS_CONNECTSTATE_CLASS State { get; set; }

        public Win32_WTSSession() 
        {
            //nada que hacer
        }
        public Win32_WTSSession(string workstation, Int32 session, Win32API.WTS_CONNECTSTATE_CLASS state)
        {
            WorkstationName = workstation;
            SessionID = session;
            State = state;
        }
    }
}
