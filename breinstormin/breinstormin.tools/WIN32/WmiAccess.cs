using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Instrumentation;
using System.Management;

namespace DotNet.Tools.WIN32
{
    public class WmiAccess : IWmiAccess
    {

        string _wmi_class;
        string _machine_name;
        System.Management.ManagementClass _wmi_class_instance;
        System.Management.ManagementObject _wmi_object;

        //public System.Management.ManagementObject WMI_Object { get { return _wmi_object; } }
        public System.Management.ManagementClass WMI_Class_Instance { get { return _wmi_class_instance; } }

        public WmiAccess(string wmiclass, string machineName) 
        {
            _wmi_class = wmiclass;
            _machine_name = machineName;
            _get_acces();
        }

        public WmiAccess() 
        { 
            
        }

        private void _get_acces() 
        {
            ManagementScope scope = Connect(_machine_name);
            ManagementPath mpath = new ManagementPath(_wmi_class);

            ManagementClass m_obj = new ManagementClass(scope, mpath, new ObjectGetOptions());
            _wmi_class_instance = m_obj;
        }



        public static ManagementScope Connect(string machineName)
        {
            ConnectionOptions options = new ConnectionOptions();
            string path = string.Format("\\\\{0}\\root\\cimv2", machineName);
            ManagementScope scope = new ManagementScope(path, options);
            scope.Connect();

            return scope;
        }

        public static ManagementObjectCollection GetAssociatorsByClassName(string machineName, string className,
            string associatedClass) 
        {
            ManagementScope scope = Connect(machineName);
            ObjectQuery query = new ObjectQuery("ASSOCIATORS OF {" + className + "} WHERE AssocClass = '" + associatedClass + "'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection results = searcher.Get();
            return results;
        }

        public static ManagementObject GetInstanceByName(string machineName, string className, string name)
        {
            ManagementScope scope = Connect(machineName);
            ObjectQuery query = new ObjectQuery("SELECT * FROM " + className + " WHERE Name = '" + name + "'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection results = searcher.Get();
            foreach (ManagementObject manObject in results)
                return manObject;
            
            return null;
        }

        public static ManagementObjectCollection GetInstancesByName(string machineName, string className, string name)
        {
            ManagementScope scope = Connect(machineName);
            ObjectQuery query = new ObjectQuery("SELECT * FROM " + className + " WHERE Name = '" + name + "'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection results = searcher.Get();
            return results;
        }

        public static ManagementObjectCollection GetInstancesByNameForFiles(string machineName, string className, string name)
        {
            ManagementScope scope = Connect(machineName);
            
            ObjectQuery query = new ObjectQuery("SELECT * FROM " + className + " WHERE Name = '" + name + "'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection results = searcher.Get();
            return results;
        }

        public static ManagementObjectCollection GetInstancesByLikeNameForFiles(string machineName, string className, string name)
        {
            ManagementScope scope = Connect(machineName);
            ObjectQuery query = new ObjectQuery("SELECT * FROM " + className + " WHERE Name like '%" + name + "%'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection results = searcher.Get();
            return results;
        }

        public static ManagementObjectCollection GetInstancesOfClass(string machineName, string className) 
        {
            ManagementScope scope = Connect(machineName);
            ObjectQuery query = new ObjectQuery("SELECT * FROM " + className);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection results = searcher.Get();
            return results;
        }

        private static ManagementClass GetStaticByName(string machineName, string className)
        {
            ManagementScope scope = Connect(machineName);
            ObjectGetOptions getOptions = new ObjectGetOptions();
            ManagementPath path = new ManagementPath(className);
            ManagementClass manClass = new ManagementClass(scope, path, getOptions);
            return manClass;
        }

        public int InvokeInstanceMethod(string machineName, string className, string name, string methodName)
        {
            return InvokeInstanceMethod(machineName, className, name, methodName, null);
        }

        public int InvokeInstanceMethod(string machineName, string className, string name, string methodName, object[] parameters)
        {
            try
            {
                ManagementObject manObject = GetInstanceByName(machineName, className, name);
                object result = manObject.InvokeMethod(methodName, parameters);
                return Convert.ToInt32(result);
            }
            catch
            {
                return -1;
            }
        }

        public int InvokeStaticMethod(string machineName, string className, string methodName)
        {
            return InvokeStaticMethod(machineName, className, methodName, null);
        }

        public int InvokeStaticMethod(string machineName, string className, string methodName, object[] parameters)
        {
            try
            {
                ManagementClass manClass = GetStaticByName(machineName, className);
                object result = manClass.InvokeMethod(methodName, parameters);
                return Convert.ToInt32(result);
            }
            catch
            {
                return -1;
            }
        }
    }
}
