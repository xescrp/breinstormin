using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace breinstormin.tools.log
{
    class _LogCollection
    {
        private Hashtable _List = new Hashtable();

        public void Put(string pLogName, string pMessage)
        {
            object obj = this._List[pLogName];
            _Log log = default(_Log);
            if (obj == null)
            {
                // Bloqueamos la hashtable de logs por si otro thread está insertando logs al mismo tiempo 
                lock (this)
                {
                    // Volvemos a comprobar que mientras esperabamos en lo lock no ha creado el log otro hilo 
                    obj = this._List[pLogName];
                    if (obj == null)
                    {
                        log = new _Log(pLogName);
                        this._List[pLogName] = log;
                    }
                    else
                    {
                        log = (_Log)obj;
                    }
                }
            }
            else
            {
                log = (_Log)obj;
            }
            log.Put(pMessage);
        }

        public int GetLogLevel(string pLogName)
        {
            string logConfig = null;

            try
            {
                logConfig = System.Configuration.ConfigurationManager.AppSettings[pLogName + ".LogLevel"];
                if (logConfig == null)
                {
                    logConfig = System.Configuration.ConfigurationManager.AppSettings["LogLevel"];
                }
                if ((logConfig != null))
                {
                    return Convert.ToInt16(logConfig);
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }


        public static string GetLogFileName(string pLogName)
        {
            return pLogName + ".log";
        }

        public static string GetLogContent(string pLogName)
        {
            string filename = pLogName + ".log";

            //verifico que exista el archivo 
            if ((System.IO.File.Exists(filename)))
            {
                System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                System.IO.StreamReader sr = new System.IO.StreamReader(fs);

                //Leo toda la informacion del archivo 
                string sContent = sr.ReadToEnd();

                //cierro los objetos 
                fs.Close();
                sr.Close();
                return sContent;
            }
            else
            {
                return "";
            }
        }

        public static bool IsSingleFile()
        {
            string singlefilename = GetSingleFileName();
            return ((singlefilename != null)) && (singlefilename.Length > 0);
        }

        public static string GetSingleFileName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["LogMgr.SingleFileName"];
        }
    }
}
