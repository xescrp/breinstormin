namespace breinstormin.appdomains
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, ClassInterface(ClassInterfaceType.AutoDual)]
    public class AppDomainFactory : MarshalByRefObject
    {
        public static AtomAppDomain<T> CreateAtomAppDomain<T>(string AssemblyFileName) where T: class
        {
            return new AtomAppDomain<T>(AssemblyFileName);
        }

        public static AtomAppDomain<T> CreateAtomAppDomain<T>(string AssemblyFileName, string AssemblyClassNameToInstance) where T: class
        {
            return new AtomAppDomain<T>(AssemblyFileName, AssemblyClassNameToInstance);
        }
    }
}

