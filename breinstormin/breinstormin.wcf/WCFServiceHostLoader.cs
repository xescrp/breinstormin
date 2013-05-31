using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace breinstormin.wcf.host
{

    public enum ServiceHostType 
    { 
        soap, web
    }

    [Serializable, 
    System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.AutoDual)]
    public class WCFServiceHostLoader : MarshalByRefObject  
    {

        
        private string url = "";
        private Type wcf_service;
        private Type _service_interface;

        //Hosts de servicio (Web service y Soap Service)
        System.ServiceModel.ServiceHost _soap_service_host;
        System.ServiceModel.Web.WebServiceHost _web_service_host;
        ServiceHostType _svc_hosts_type;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public WCFServiceHostLoader() 
        {
            _svc_hosts_type = ServiceHostType.soap;
            
        }

        public void set(ServiceHostType svchostType, Type service_type) 
        {
            _svc_hosts_type = svchostType;
            wcf_service = service_type;
            get_interface();
            _load_host_type();
        }

        public void get_interface() 
        {
            Type t_wrap = wcf_service;
            Type[] interfaces = t_wrap.GetInterfaces();
            if (interfaces != null && interfaces.Length > 0) 
            {
                _service_interface = interfaces[0];
            }
        }

        public void Start() 
        {
            switch (_svc_hosts_type) 
            { 
                case ServiceHostType.soap:
                    if (_soap_service_host != null) 
                    {
                        _soap_service_host.Open();
                    }
                    break;
                case ServiceHostType.web:
                    if (_web_service_host != null) 
                    {
                        _web_service_host.Open();
                    }
                    break;
            }
        }

        public void Stop() 
        {
            switch (_svc_hosts_type)
            {
                case ServiceHostType.soap:
                    if (_soap_service_host != null)
                    {
                        _soap_service_host.Close();
                    }
                    break;
                case ServiceHostType.web:
                    if (_web_service_host != null)
                    {
                        _web_service_host.Close();
                    }
                    break;
            }
        }


        private void _load_host_type() 
        {
            switch (_svc_hosts_type) 
            { 
                case ServiceHostType.soap:
                    _load_soap_host();
                    break;
                case ServiceHostType.web:
                    _load_web_host();
                    break;
            }
        }

        private void _load_soap_host() 
        {
            
            Uri _soap_http_url = new Uri(
                System.Configuration.ConfigurationManager.AppSettings["SOAP_HTTP_Url"]);
            _soap_service_host = new System.ServiceModel.ServiceHost(wcf_service, _soap_http_url);

            //Añadimos endopoint soap
            System.ServiceModel.Description.ServiceEndpoint epoint = 
                _soap_service_host.AddServiceEndpoint(_service_interface, 
                    new System.ServiceModel.BasicHttpBinding(), _soap_http_url);

            

            //Habilitamos metadata para este endpoint
            System.ServiceModel.Description.ServiceMetadataBehavior mthttp =
                _soap_service_host.Description.Behaviors.Find<System.ServiceModel.Description.ServiceMetadataBehavior>();
            

            if (mthttp != null)
            {
                mthttp.HttpGetEnabled = true;
            }
            else 
            {
                mthttp = new System.ServiceModel.Description.ServiceMetadataBehavior();
                mthttp.HttpGetEnabled = true;
                
                _soap_service_host.Description.Behaviors.Add(mthttp);
            }
            
            

            
        }

        private void _load_web_host() 
        {
            Uri _web_http_url = new Uri(
                System.Configuration.ConfigurationManager.AppSettings["Web_HTTP_Url"]);
            _web_service_host = new System.ServiceModel.Web.WebServiceHost(wcf_service, _web_http_url);
            //Añadimos endpoint web
            System.ServiceModel.Description.ServiceEndpoint epoint = 
                _web_service_host.AddServiceEndpoint(_service_interface, new System.ServiceModel.WebHttpBinding(), _web_http_url);

            //Vamos a añadir la carateristica de autoformato en response
            System.ServiceModel.Description.WebHttpBehavior wbhttp =
                epoint.Behaviors.Find<System.ServiceModel.Description.WebHttpBehavior>();

            if (wbhttp != null)
            {
                wbhttp.AutomaticFormatSelectionEnabled = true;
                wbhttp.HelpEnabled = true;
            }
            else
            {
                wbhttp = new System.ServiceModel.Description.WebHttpBehavior();
                wbhttp.AutomaticFormatSelectionEnabled = true;
                wbhttp.HelpEnabled = true;
                wbhttp.FaultExceptionEnabled = true;
                epoint.Behaviors.Add(wbhttp);

            }
        }
    }
}
