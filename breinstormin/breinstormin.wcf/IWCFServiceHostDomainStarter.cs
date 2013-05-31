using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace breinstormin.wcf.host
{
    public interface IWCFServiceHostDomainStarter
    {
        void SetHostType(ServiceHostType hosttype);
        void Start();
        void Stop();
    }
}
