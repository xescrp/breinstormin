using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace breinstormin.db
{
    internal interface IDBEngine
    {
        System.Data.IDbCommand Command { get; }
        System.Data.IDbConnection Connection { get; }
        string DBType { get; }


        System.Data.ConnectionState ConnectionState();

        System.Data.IDataParameterCollection Parameters { get; }

        void Open(bool transaction);
        void Close();
        System.Data.IDataReader ExecuteReader();
        System.Data.IDataReader ExecuteReader(string sqlcommand);
        int ExecuteNonQuery();
        int ExecuteNonQuery(string sqlcommand);
        void SetConnectionString(string server, string port, string user, string password, string database);
        string GetConnectionString(string server, string port, string user, string password, string database);
        string SetConnectionStringFromConfig();
        string GetConnectionStringFromConfig();
        object GetValue(string pSelect);
        object GetValue();
        void SetupDBProvider(string server, string port, string user, string password, string database);
        void SetupDBProviderFromConfig();
        void AddParameter(string name, object value, System.Data.DbType paramtype);
        void ClearParameters();
    }
}
