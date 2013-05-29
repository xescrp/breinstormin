using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Configuration;
using System.IO;

namespace breinstormin.tools.log
{
    public class LogEngine
    {
        private static _LogCollection _listaLogs = new _LogCollection();
        public static string GetLogDirectory()
        {
            try
            {
                _Log log = new _Log("dd_name");
                string name = log._GetLogFilename();
                return name.Replace(@"\" + System.IO.Path.GetFileName(name), "");
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.Write(ex.ToString());
#endif
                return null;
            }
        }

        public static void WriteLog(string pLogName, string pText)
        {

            string logName = string.Empty;
            if ((pLogName != null))
            {
                logName = pLogName;
            }
            string logFichero = null;
            if (_LogCollection.IsSingleFile())
            {
                logFichero = _LogCollection.GetSingleFileName();
            }
            else
            {
                logFichero = logName;
            }

            // -- 

            string fmt = null;
            if (_LogCollection.IsSingleFile())
            {
                if (pText == null)
                {
                    fmt = string.Format("{0:s}##", DateTime.Now);
                }
                else
                {
                    fmt = string.Format("{0:s}#{1}#{2}", DateTime.Now, logName, pText);
                }
            }
            else
            {
                if (pText == null)
                {
                    fmt = string.Format("{0:s}", DateTime.Now);
                }
                else
                {
                    fmt = string.Format("{0:s} {1}", DateTime.Now, pText);
                }
            }
            _listaLogs.Put(logName, pText);
            //_listaLogs.Put(pLogName = logFichero, pMessage = string.Format(fmt, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7));

        }
    }
}
