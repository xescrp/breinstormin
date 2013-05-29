using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Tools.WIN32
{
    public class Win32_Service
    {
        public bool AcceptPause { get { return (bool)_get_value("AcceptPause"); } }
        public bool AcceptStop { get { return (bool)_get_value("AcceptStop"); } }
        public string Caption { get { return (string)_get_value("Caption"); } }
        public uint CheckPoint { get { return (uint)_get_value("CheckPoint"); } }
        public string CreationClassName { get { return (string)_get_value("CreationClassName"); } }
        public string Description { get { return (string)_get_value("Description"); } }
        public bool DesktopInteract { get { return (bool)_get_value("DesktopInteract"); } }
        public string DisplayName { get { return (string)_get_value("DisplayName"); } }
        public string ErrorControl { get { return (string)_get_value("ErrorControl"); } }
        public uint ExitCode { get { return (uint)_get_value("ExitCode"); } }
        public DateTime InstallDate { get { return (DateTime)_get_value("InstallDate"); } }
        public string Name { get { return (string)_get_value("Name"); } }
        public string PathName { get { return (string)_get_value("PathName"); } }
        public uint ProcessId { get { return (uint)_get_value("ProcessId"); } }
        public uint ServiceSpecificExitCode { get { return (uint)_get_value("ServiceSpecificExitCode"); } }
        public string ServiceType { get { return (string)_get_value("ServiceType"); } }
        public bool Started { get { return (bool)_get_value("Started"); } }
        public string StartMode { get { return (string)_get_value("StartMode"); } }
        public string StartName { get { return (string)_get_value("StartName"); } }
        public string State { get { return (string)_get_value("State"); } }
        public string Status { get { return (string)_get_value("Status"); } }
        public string SystemCreationClassName { get { return (string)_get_value("SystemCreationClassName"); } }
        public string SystemName { get { return (string)_get_value("SystemName"); } }
        public uint TagId { get { return (uint)_get_value("TagId"); } }
        public uint WaitHint { get { return (uint)_get_value("WaitHint"); } }

        private object _get_value(string propname) 
        {
            if (_wmiservice_instance != null)
            {
                return _wmiservice_instance.GetPropertyValue(propname);
            }
            else
            {
                return null;
            }
        }

        private const string CLASS_NAME = "Win32_Service";
        private readonly IWmiAccess _wmi;
        private System.Management.ManagementObject _wmiservice_instance;

        /// <summary>
        /// Crea una clase WMI Service para controlar un servicio windows
        /// </summary>
        /// <param name="wmi">el objecto de acceso wmi </param>
        public Win32_Service(IWmiAccess wmi)
        {
            _wmi = wmi;

        }

        public Win32_Service(string servername) 
        {
            WmiAccess acc = new WmiAccess(CLASS_NAME, servername);
            _wmi = acc;
        }
        public static Win32_Service[] GetWin32_Services(string machineName) 
        {
            List<Win32_Service> lst = new List<Win32_Service>();
            System.Management.ManagementObjectCollection mg =
                WIN32.WmiAccess.GetInstancesOfClass(machineName, "Win32_Service");
            foreach (System.Management.ManagementObject m in mg)
            {

                WIN32.Win32_Service src = new DotNet.Tools.WIN32.Win32_Service(m);
                lst.Add(src);

            }
            return lst.ToArray();
        }
        
        internal Win32_Service(System.Management.ManagementObject mgo) 
        {
            _wmi = new WIN32.WmiAccess();
            _wmiservice_instance = mgo;
        }
        public Win32_Service(string machineName, string serviceName, string username, string password) 
        {
            _wmi = new WIN32.WmiAccess();
            _wmiservice_instance = WmiAccess.GetInstanceByName(machineName, CLASS_NAME, serviceName);
        }

        int _execute_method(string methodname, string[] parameters) 
        {
            object prd = _wmiservice_instance.InvokeMethod(methodname, parameters);
            int rs = int.Parse(prd.ToString());
            return rs;
        }

        public Win32API.ServiceReturnCode StartService()
        {
            return (Win32API.ServiceReturnCode)_execute_method("StartService", null);
        }
 
        public Win32API.ServiceReturnCode StopService()
        {
            return (Win32API.ServiceReturnCode)_execute_method("StopService", null);
        }
        public Win32API.ServiceReturnCode PauseService()
        {
            return (Win32API.ServiceReturnCode)_execute_method("PauseService", null);
        }
         
        public Win32API.ServiceReturnCode ResumeService()
        {
            return (Win32API.ServiceReturnCode)_execute_method("ResumeService", null);
        }
         
        public Win32API.ServiceReturnCode InterrogateService()
        {
            return (Win32API.ServiceReturnCode)_execute_method("InterrogateService", null);
        }
         
        public Win32API.ServiceReturnCode UserControlService(int id_user_command)
        {
            string[] parms = new string[] { id_user_command.ToString() };
            return (Win32API.ServiceReturnCode)_execute_method("UserControlService", parms);
        }
 

 
        public Win32API.ServiceReturnCode Change(string DisplayName,
                    string mPathName,
                    UInt32 mServiceType,
                    UInt32 mErrorControl,
                    string mStartMode,
                    bool mDesktopInteract,
                    string mStartName,
                    string mStartPassword,
                    string mLoadOrderGroup,
                    string mLoadOrderGroupDependencies,
                    string mServiceDependencies)
        {
            string[] parms = new string[] { mPathName, ServiceType.ToString(), mErrorControl.ToString(), 
                mStartMode, mDesktopInteract.ToString(), mStartName, 
                mStartPassword, mLoadOrderGroup, mLoadOrderGroupDependencies, mServiceDependencies  };
            return (Win32API.ServiceReturnCode)_execute_method("Change", parms);
        }
         
        public Win32API.ServiceReturnCode ChangeStartMode(string start_mode)
        {
            string[] parms = new string[] { start_mode };
            return (Win32API.ServiceReturnCode)_execute_method("ChangeStartMode", parms);
        }

 





        public Win32API.ServiceReturnCode Install(string name, string displayName,
            string physicalLocation, Win32API.ServiceStartMode startMode, string userName, string password, 
            string[] dependencies)
        {
            return Install(Environment.MachineName, name, displayName, 
                physicalLocation, startMode, userName, password, dependencies, false);
        }

        public Win32API.ServiceReturnCode Install(string machineName, string name,
            string displayName, string physicalLocation, Win32API.ServiceStartMode startMode, 
            string userName, string password, string[] dependencies)
        {
            return Install(machineName, name, 
                displayName, physicalLocation, startMode, userName, password, dependencies, false);
        }

        /// <summary>
        /// Instala un servicio en una maquina
        /// </summary>
        /// <param name="machineName">nombre de la maquina</param>
        /// <param name="name">nombre del servicio</param>
        /// <param name="displayName">El nombre que se mostrara en la consola de administracion</param>
        /// <param name="physicalLocation">el path de instalacion</param>
        /// <param name="startMode">Como arrancara el servicio - normalmente: Automatic</param>
        /// <param name="userName">el usuario que ejecuta el servicio</param>
        /// <param name="password">el password del usuario</param>
        /// <param name="dependencies">Otras dependencias del servicio</param>
        /// <param name="interactWithDesktop">Interactua con el escritorio?</param>
        /// <returns>Codigo devuelto indicando el exito de la operacion</returns>
        public Win32API.ServiceReturnCode Install(string machineName, string name,
            string displayName, string physicalLocation, Win32API.ServiceStartMode startMode, 
            string userName, string password, string[] dependencies, bool interactWithDesktop)
        {
            const string methodName = "Create";
            //string[] serviceDependencies = dependencies != null ? dependencies.Split(',') : null;
            //if (userName.IndexOf('\\') < 0)
            //{
            //    //userName = ".\\" + userName;
            //    //
            //}

            try
            {
                object[] parameters = new object[]
                                      {
                                          name, // Nombre
                                          displayName, // nombre mostrado
                                          physicalLocation, // Path 
                                          Convert.ToInt32(Win32API.ServiceType.OwnProcess), // Tipo de servicio
                                          Convert.ToInt32(Win32API.ServiceErrorControl.UserNotified), // Error Control
                                          startMode.ToString(), // Modo arranque
                                          interactWithDesktop, // interaccion con el escritorio
                                          userName, // usuario
                                          password, // Password
                                          null, // 
                                          null, // Dependencias
                                          dependencies // ServiceDependencies
                                      };
                return (Win32API.ServiceReturnCode)_wmi.InvokeStaticMethod(machineName, CLASS_NAME, methodName, parameters);
            }
            catch
            {
                return Win32API.ServiceReturnCode.UnknownFailure;
            }
        }

        public Win32API.ServiceReturnCode Uninstall(string name)
        {
            return Uninstall(Environment.MachineName, name);
        }

        /// <summary>
        /// Desinstala un servicio en una maquina
        /// </summary>
        /// <param name="machineName">nombre de la maquina</param>
        /// <param name="name">nombre del servicio</param>
        /// <returns>codigo devuelto informando el exito de la operacion</returns>
        public Win32API.ServiceReturnCode Uninstall(string machineName, string name)
        {
            try
            {
                const string methodName = "Delete";
                return (Win32API.ServiceReturnCode)_wmi.InvokeInstanceMethod(machineName, CLASS_NAME, name, methodName);
            }
            catch
            {
                return Win32API.ServiceReturnCode.UnknownFailure;
            }
        }
    }
}
