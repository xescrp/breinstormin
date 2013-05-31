using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace breinstormin.wcf.host
{
    [Serializable()]
    public class WCFServiceHostStarter<T>: MarshalByRefObject
    {
        private WCFServiceHostLoader _wcf_host;
        private ServiceHostType _host_type;
        private T core;

        public override object InitializeLifetimeService()
        {
            return null;
        }



        public WCFServiceHostStarter() 
        {
            _wcf_host = new WCFServiceHostLoader();
        }

        public object SetHostType(ServiceHostType hosttype, Func<object> startmethod) 
        {
            _host_type = hosttype;
             _wcf_host.set(_host_type, typeof(T));
            return startmethod();
        }

        public void Start() 
        {
            _wcf_host.Start();
        }

        public void Stop() 
        {
            _wcf_host.Stop();
        }

    }
}