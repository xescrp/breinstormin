namespace breinstormin.appdomains
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [Serializable, ClassInterface(ClassInterfaceType.AutoDual)]
    public class CustomAppDomain : MarshalByRefObject
    {
        private AppDomain _app_domain;
        private AppDomainSetup _app_domain_info;
        private string _iterate_assembly;
        private bool _load_ok = true;
        private string _loading_errors;
        private string _path;

        public CustomAppDomain(string AssemblyPath)
        {
            try
            {
                this._path = AssemblyPath;
                this._app_domain_info = new AppDomainSetup();
                this._app_domain_info.ApplicationBase = this._path;
                this._app_domain_info.PrivateBinPath = this._path;
                this._app_domain_info.PrivateBinPathProbe = this._path;
                this._app_domain_info.ConfigurationFile = this._path + @"\app.config";
                this._app_domain = AppDomain.CreateDomain("NEW_CUSTOMDOMAIN_" + DateTime.Now.ToString(), null, this._app_domain_info);
                try
                {
                    this._app_domain.AssemblyResolve += new ResolveEventHandler(this._app_domain_AssemblyResolve);
                    this._app_domain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(this._app_domain_ReflectionOnlyAssemblyResolve);
                }
                catch (Exception exception)
                {
                    this._loading_errors = exception.ToString();
                }
                string[] files = Directory.GetFiles(this._path, "*.dll");
                string[] strArray2 = Directory.GetFiles(this._path, "*.exe");
                if (files != null)
                {
                    foreach (string str in files)
                    {
                        try
                        {
                            AssemblyName.GetAssemblyName(str);
                            this._load_assembly(str);
                        }
                        catch (Exception exception2)
                        {
                            this._load_ok = false;
                            this._loading_errors = exception2.ToString();
                        }
                    }
                }
                if (strArray2 != null)
                {
                    foreach (string str2 in strArray2)
                    {
                        try
                        {
                            AssemblyName.GetAssemblyName(str2);
                            this._load_assembly(str2);
                        }
                        catch (Exception exception3)
                        {
                            this._load_ok = false;
                            this._loading_errors = exception3.ToString();
                        }
                    }
                }
                this._load_ok = true;
            }
            catch (Exception exception4)
            {
                this._load_ok = false;
                this._loading_errors = exception4.ToString();
            }
        }

        private Assembly _app_domain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                return Assembly.LoadFrom(this._iterate_assembly);
            }
            catch (Exception exception)
            {
                this._loading_errors = this._loading_errors + exception.ToString();
                return null;
            }
        }

        private Assembly _app_domain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                return Assembly.LoadFrom(this._iterate_assembly);
            }
            catch (Exception exception)
            {
                this._loading_errors = this._loading_errors + exception.ToString();
                return null;
            }
        }

        private void _load_assembly(string nAssembly)
        {
            if (File.Exists(nAssembly))
            {
                foreach (AssemblyName name in Assembly.LoadFrom(nAssembly).GetReferencedAssemblies())
                {
                    this._load_assembly(name.FullName);
                }
            }
            this._app_domain.Load(nAssembly);
        }

        public object GetInstance(string className, string AssemblyName)
        {
            object obj2 = null;
            foreach (Assembly assembly in this._app_domain.GetAssemblies())
            {
                if (assembly.FullName.ToLower().Contains(AssemblyName.ToLower()))
                {
                    obj2 = assembly.CreateInstance(className);
                }
            }
            return obj2;
        }

        public Assembly[] Assemblies
        {
            get
            {
                if (this._app_domain != null)
                {
                    return this._app_domain.GetAssemblies();
                }
                return null;
            }
        }

        public AppDomain AtomicAppDomain
        {
            get
            {
                return this._app_domain;
            }
        }

        public string LoadingErrors
        {
            get
            {
                return this._loading_errors;
            }
        }

        public bool LoadOK
        {
            get
            {
                return this._load_ok;
            }
        }

        public string Path
        {
            get
            {
                return this._path;
            }
        }
    }
}

