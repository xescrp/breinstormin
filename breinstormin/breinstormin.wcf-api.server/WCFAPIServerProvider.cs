using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;


namespace breinstormin.wcf_api.server
{
    public partial class WCFAPIServerProvider : ServiceBase
    {

        string[] _service_directories;
        List<breinstormin.appdomains.AtomAppDomain<breinstormin.wcf.host.IWCFServiceHostDomainStarter>> _app_domains;

        public WCFAPIServerProvider()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _get_directories();
            _load_services();
            _start_services();
        }

        protected override void OnStop()
        {
            _stop_services();
        }




        private void _get_directories()
        {
            string parentdir = System.Configuration.ConfigurationManager.AppSettings["WCFServicesPath"];
            if (!string.IsNullOrEmpty(parentdir))
            {
                _service_directories = System.IO.Directory.GetDirectories(parentdir);
            }
        }
        private void _load_services()
        {
            _app_domains = new
                List<breinstormin.appdomains.AtomAppDomain<breinstormin.wcf.host.IWCFServiceHostDomainStarter>>();
            if (_service_directories != null)
            {
                string exceptions = System.Configuration.ConfigurationManager.AppSettings["LoadExceptions"];
                foreach (string dir in _service_directories)
                {
                    System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(dir);
                    bool _can_load = true;
                    if (!string.IsNullOrEmpty(exceptions))
                    {
                        //comprobamos si esta excluido
                        _can_load = !exceptions.ToLower().Contains(info.Name.ToLower());
                    }
                    if (_can_load)
                    {
                        //Cargamos dominio de aplicacion con estos ensamblados 
                        //(buscamos en el config con el nombre del directorio).ç
                        string cfg = System.Configuration.ConfigurationManager.AppSettings[info.Name];
                        if (!string.IsNullOrEmpty(cfg))
                        {
                            string[] _cfg_service =
                                cfg.Split(new string[] { ";" },
                                StringSplitOptions.RemoveEmptyEntries);
                            //Nombre del ensamblado
                            string assembly_name =
                                _cfg_service[0];
                            //Nombre de la clase a instanciar
                            string class_name = _cfg_service[1];
                            string assembly_path = dir + @"\" + assembly_name;


                            //Dominio SOAP (WSDL) para este servicio
                            breinstormin.appdomains.AtomAppDomain<breinstormin.wcf.host.IWCFServiceHostDomainStarter>
                                wcf_soap_app_domain =
                                new breinstormin.appdomains.AtomAppDomain
                                    <breinstormin.wcf.host.IWCFServiceHostDomainStarter>(assembly_path, class_name);

                            if (wcf_soap_app_domain.LoadOK)
                            {
                                wcf_soap_app_domain.InstancedObject.SetHostType(breinstormin.wcf.host.ServiceHostType.soap);
                                _app_domains.Add(wcf_soap_app_domain);
                            }
                            else { throw new Exception(wcf_soap_app_domain.LoadingErrors); }

                            //Dominio WEB (REST) para este servicio
                            breinstormin.appdomains.AtomAppDomain<breinstormin.wcf.host.IWCFServiceHostDomainStarter>
                                wcf_web_app_domain =
                                new breinstormin.appdomains.AtomAppDomain
                                    <breinstormin.wcf.host.IWCFServiceHostDomainStarter>(assembly_path, class_name);
                            if (wcf_web_app_domain.LoadOK)
                            {
                                wcf_web_app_domain.InstancedObject.SetHostType(breinstormin.wcf.host.ServiceHostType.web);
                                _app_domains.Add(wcf_web_app_domain);
                            }
                            else { throw new Exception(wcf_web_app_domain.LoadingErrors); }
                        }

                    }
                }
            }
        }


        private void _start_services()
        {
            if (_app_domains != null & _app_domains.Count > 0)
            {
                foreach (breinstormin.appdomains.AtomAppDomain
                        <breinstormin.wcf.host.IWCFServiceHostDomainStarter> app_domain in _app_domains)
                {
                    try
                    {
                        app_domain.InstancedObject.Start();
                        breinstormin.tools.log.LogEngine.WriteLog("WFCServiceLauncher", "Starting api service...");
                    }
                    catch (Exception ex)
                    {
                        breinstormin.tools.log.LogEngine.WriteLog("WFCServiceLauncher.ERROR", ex.ToString());
                    }
                }
            }
        }

        private void _stop_services()
        {
            if (_app_domains != null & _app_domains.Count > 0)
            {
                foreach (breinstormin.appdomains.AtomAppDomain
                        <breinstormin.wcf.host.IWCFServiceHostDomainStarter> app_domain in _app_domains)
                {
                    app_domain.InstancedObject.Stop();
                }
            }
        }

    }
}
