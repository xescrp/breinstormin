namespace breinstormin.appdomains
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [Serializable, ClassInterface(ClassInterfaceType.AutoDual)]
    public partial class AtomAppDomain<T> : MarshalByRefObject where T: class
    {
        private AppDomain _app_domain;
        private AppDomainSetup _app_domain_info;
        private string _assembly_class_name;
        private string _assembly_file;
        private string _assembly_file_name;
        private T _inner_class;
        private bool _load_ok;
        private string _loading_errors;
        private string _path;

        public AtomAppDomain(string AssemblyFile)
        {
            this._load_ok = true;
            try
            {
                this._assembly_file = AssemblyFile;
                this._assembly_file_name = System.IO.Path.GetFileName(this._assembly_file);
                this._path = this._assembly_file.Replace(@"\" + this._assembly_file_name, "");
                this._assembly_class_name = typeof(T).ToString();
                this._app_domain_info = new AppDomainSetup();
                this._app_domain_info.ApplicationBase = this._path;
                this._app_domain_info.PrivateBinPath = this._path;
                this._app_domain_info.PrivateBinPathProbe = this._path;
                this._app_domain_info.ConfigurationFile = this._path + @"\app.config";
                this._app_domain = AppDomain.CreateDomain(this._assembly_file_name + DateTime.Now.ToString(), null, this._app_domain_info);
                this._inner_class = (T) this._app_domain.CreateInstanceFromAndUnwrap(this._assembly_file, this._assembly_class_name);
                this._load_ok = true;
            }
            catch (Exception exception)
            {
                this._load_ok = false;
                this._loading_errors = exception.ToString();
            }
        }

        public AtomAppDomain(string AssemblyFile, string AssemblyClassNameToInstance)
        {
            this._load_ok = true;
            try
            {
                this._assembly_file = AssemblyFile;
                this._assembly_file_name = System.IO.Path.GetFileName(this._assembly_file);
                this._path = this._assembly_file.Replace(@"\" + this._assembly_file_name, "");
                this._assembly_class_name = AssemblyClassNameToInstance;
                this._app_domain_info = new AppDomainSetup();
                this._app_domain_info.ApplicationBase = this._path;
                this._app_domain_info.PrivateBinPath = this._path;
                this._app_domain_info.PrivateBinPathProbe = this._path;
                this._app_domain_info.ConfigurationFile = this._path + @"\app.config";
                this._app_domain = AppDomain.CreateDomain(this._assembly_file_name + DateTime.Now.ToString(), null, this._app_domain_info);
                this._inner_class = (T) this._app_domain.CreateInstanceFromAndUnwrap(this._assembly_file, this._assembly_class_name);
                this._load_ok = true;
            }
            catch (Exception exception)
            {
                this._load_ok = false;
                this._loading_errors = exception.ToString();
            }
        }

        protected Assembly _asm_resolve(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFrom(this._assembly_file);
        }

        public string AssemblyFile
        {
            get
            {
                return this._assembly_file;
            }
        }

        public string AssemblyFileName
        {
            get
            {
                return this._assembly_file_name;
            }
        }

        public AppDomain AtomicAppDomain
        {
            get
            {
                return this._app_domain;
            }
        }

        public T InstancedObject
        {
            get
            {
                return this._inner_class;
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

