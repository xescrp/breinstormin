using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;


//Instalador del servicio (Installutil)
namespace breinstormin.wcf_api.server
{
    [RunInstaller(true)]
    public class WCFAPIServerInstaller : Installer
    {
        /// <summary>
        /// Constructor e instalador de servcios.
        /// </summary>
        public WCFAPIServerInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller = 
                               new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            //# Cuenta local de Usuario para ejecucion del servicio
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            string name = System.Configuration.ConfigurationManager.AppSettings["ServiceInstaller.Name"];
            string description = System.Configuration.ConfigurationManager.AppSettings["ServiceInstaller.Description"];

            if (string.IsNullOrEmpty(name)) 
            {
                name = "WCF-API Server Provider";
            }
            if (string.IsNullOrEmpty(description)) 
            {
                description = "WCF-API Server Provider. Provider/Publisher WCF interfaces Service .";
            }
            //# Informacion del servicio
            serviceInstaller.DisplayName = name;  //Cambiar este valor con el nombre del servicio
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceInstaller.ServiceName = name; //Cambiar este valor con el nombre del servicio
            serviceInstaller.Description = description;
                ; //Descripcion del servicio (opcional)

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
