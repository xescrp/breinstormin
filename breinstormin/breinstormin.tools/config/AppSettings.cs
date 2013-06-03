using System;
using System.Collections.Generic;
using System.Text;

namespace breinstormin.tools.config
{
    public abstract class AppSettings
    {
        public abstract string[] Keys { get; }


        public abstract string GetValue(string keyname);
        public abstract void AddKey(string keyname, string value);
        public abstract void AlterValue(string keyname, string value);
        public abstract void RemoveKey(string keyname);
        public abstract bool ContainsKey(string keyname);
    }
}
