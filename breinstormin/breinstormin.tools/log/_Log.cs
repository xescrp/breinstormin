using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace breinstormin.tools.log
{
    class _Log
    {
        private ArrayList _Messages = new ArrayList();
        private string _LogName;
        private bool _MessagesToFlush = false;
        private bool _Flushing = false;
        private int _LogWaitSeconds;
        private int _MaxLogQueueLength;
        private bool _WritersWaiting = false;

        public _Log(string pLogName)
        {
            this._LogName = pLogName;
            // No podemos guardar el nombre del fichero porque cambia de un día a otro 
            this._LogWaitSeconds = this._GetLogWaitSeconds();
            this._MaxLogQueueLength = this._GetMaxQueueLength();
        }

        public void Put(string pMessage)
        {
            // Si hay demasiados mensajes en cola ignoramos los nuevos para no provocar un Out Of Memory 
            if ((this._MaxLogQueueLength == -1) || (this._Messages.Count <= this._MaxLogQueueLength))
            {
                _WritersWaiting = true;
                // Bloqueamos la cola de mensajes frente a otros puts o flushes 
                lock (this)
                {
                    this._Messages.Add(pMessage);
                }
                this._MessagesToFlush = true;
                // Si las listas estaban vacías activamos el hilo de flush 
                if (!this._Flushing)
                {
                    this._Flushing = true;
                    //Console.WriteLine("Activa flush " & Me._LogName) 
                    System.Threading.ThreadStart entry = new System.Threading.ThreadStart(this.DoFlush);
                    System.Threading.Thread thread1 = new System.Threading.Thread(entry);
                    thread1.Start();
                }
            }
            //Else 
            // Console.WriteLine("Para") 
            // Stop 
        }

        // Método privado. Se arranca automáticamente cuando entra la primera escritura en algún log 
        public void DoFlush()
        {
            System.Threading.Thread.Sleep(this._LogWaitSeconds * 1000);
            do
            {
                //Console.WriteLine("Empieza flush " & Me._LogName) 
                this._MessagesToFlush = false;
                this.Flush();
                //Console.WriteLine("Acaba flush " & Me._LogName) 
                System.Threading.Thread.Sleep(this._LogWaitSeconds * 1000);
                if (!this._MessagesToFlush)
                {
                    this._Flushing = false;
                    // Volvemos a comprobar que no han entrado mensajes por escribir antes de salir del flush 
                    if (!this._MessagesToFlush)
                    {
                        break;
                    }
                    else
                    {
                        this._Flushing = true;
                    }
                }
            }
            //Console.WriteLine("Sale del flush " & Me._LogName) 
            while (true);
        }

        public void Flush()
        {

            _WritersWaiting = false;
            if (this._Messages.Count > 0)
            {
                // Bloqueamos la cola de mensajes frente a otros puts o flushes 
                lock (this)
                {
                    // Volvemos a comprobar que no está vacía por si acaso otro hilo lo ha vaciado mientras esperaba en el lock 
                    if (this._Messages.Count > 0)
                    {

                        StreamWriter writer = default(StreamWriter);
                        try
                        {
                            writer = new StreamWriter(_GetLogFilename(), true);
                            //Console.WriteLine("Vacia " & _Messages.Count & " mensajes " & Me._LogName) 
                            //Console.Out.Flush() 

                            foreach (string mensaje in this._Messages)
                            {
                                writer.WriteLine(mensaje);
                            }

                            this._Messages = new ArrayList();
                        }
                        catch (Exception ex)
                        {
                            // Ignora cualquier error 
                            Console.WriteLine("Error en Flush " + this._LogName + ". La exc es: " + ex.Message + "\r\n" + ex.StackTrace);
                        }
                        finally
                        {
                            if ((writer != null))
                            {
                                try
                                {
                                    writer.Close();
                                }
                                catch (Exception ex)
                                {
                                    // Ignora cualquier error 
                                    Console.WriteLine("Error en Flush en el close " + this._LogName + ". La exc es: " + ex.Message + "\r\n" + ex.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
        }

        // LogWaitSeconds -> Número de segundos a esperar entre vaciados del buffer a disco 
        // Si el valor es muy bajo se ralentizará el sistema por los accesos a disco que serán más frecuentes 
        // Si el valor es muy alto hay más riesgo de que se pierdan mensajes si se cae la aplicación 
        // y se tarda más en ver los logs escritos. También la aplicación tarda más en pararse. 
        // Se permite indicar una configuración especifica para cada log con la clave "<LogName>.LogWaitSeconds" 
        // Si no existe se usa la genérica "LogMgr.LogWaitSeconds" 
        // Si tampoco existe se usa 3 
        private int _GetLogWaitSeconds()
        {
            string logConfig = null;
            try
            {
                logConfig = System.Configuration.ConfigurationManager.AppSettings[this._LogName + ".LogWaitSeconds"];
                if (logConfig == null)
                {
                    logConfig = System.Configuration.ConfigurationManager.AppSettings["LogMgr.LogWaitSeconds"];
                }
                if ((logConfig != null))
                {
                    return Convert.ToInt16(logConfig);
                }
                else
                {
                    return 3;
                }
            }
            catch
            {
                return 3;
            }
        }

        // MaxLogQueueLength -> Número máximo de mensajes a encolar sin vaciar a disco. 
        // Si se supera ese valor descarta los mensajes. 
        // Pretende ser una protección frente a un bloqueo en las escrituras a disco para evitar que la aplicación falle for falta de memoria 
        // Si vale -1 no se aplica ningún límite y todos los mensajes se encolan para su escritura 
        // Se permite indicar una configuración especifica para cada log con la clave "<LogName>.MaxLogQueueLength" 
        // Si no existe se usa la genérica "LogMgr.MaxLogQueueLength" 
        // Si tampoco existe se usa 50000 
        private int _GetMaxQueueLength()
        {
            string logConfig = null;
            try
            {
                logConfig = System.Configuration.ConfigurationManager.AppSettings[this._LogName + ".MaxLogQueueLength"];
                if (logConfig == null)
                {
                    logConfig = System.Configuration.ConfigurationManager.AppSettings["LogMgr.MaxLogQueueLength"];
                }
                if ((logConfig != null))
                {
                    return Convert.ToInt16(logConfig);
                }
                else
                {
                    return 100000;
                }
            }
            catch
            {
                return 100000;
            }
        }

        internal string _GetLogFilename()
        {
            string filename = null;
            string foldername = null;
            int pos = 0;

            System.Reflection.Assembly ejecutable = default(System.Reflection.Assembly);
            ejecutable = System.Reflection.Assembly.GetEntryAssembly();
            // Si no existe GetEntryAssembly asumimos que es una aplicacion ASP.NET 
            if (ejecutable == null)
            {
                filename = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                filename = filename.Replace("/", "\\");
            }
            else
            {
                filename = ejecutable.Location;
            }
            pos = filename.LastIndexOf("\\");
            foldername = filename.Substring(0, pos) + "\\Log";
            if (!Directory.Exists(foldername))
            {
                Directory.CreateDirectory(foldername);
            }
            filename = string.Format("{0}\\{1}_{2:yyyyMMdd}.log", foldername, this._LogName, DateTime.Today);

            string dir = System.IO.Path.GetDirectoryName(filename);
            if (!System.IO.Directory.Exists(dir)) { System.IO.Directory.CreateDirectory(dir); }

            return filename;
        }

    }
}
