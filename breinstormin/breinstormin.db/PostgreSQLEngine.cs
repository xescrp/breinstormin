using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace breinstormin.db
{
    internal class PostgreSQLEngine : IDBEngine
    {
        private Npgsql.NpgsqlConnection connection;
        private Npgsql.NpgsqlCommand comm;

        

        string connstring;
        private string _dbtype = "POSTGRE";
        public string DBType { get{ return _dbtype; } }

        public System.Data.IDbCommand Command
        {
            get { return comm; }
        }

        public System.Data.IDbConnection Connection
        {
            get { return connection; }
        }

        public System.Data.IDataParameterCollection Parameters
        {
            get { return comm.Parameters; }
        }

        public void Open(bool transaction)
        {
            connection = new Npgsql.NpgsqlConnection(connstring);
            connection.Open();
            comm = new Npgsql.NpgsqlCommand("", connection);
            if (transaction)
            {
                comm.Transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            }
        }

        public void Close()
        {
            if (comm != null)
            {
                if (comm.Transaction != null)
                {
                    try
                    {
                        if ((System.Runtime.InteropServices.Marshal.GetExceptionCode() == -858993460) ||
                                (System.Runtime.InteropServices.Marshal.GetExceptionCode() == 0))
                        {
                            comm.Transaction.Commit();
                        }
                        else
                        {
                            comm.Transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                    }
                }
                comm.Connection.Close();
                comm.Dispose();
            }
        }


        public System.Data.ConnectionState ConnectionState()
        {
            return connection.State;
        }

        public void SetConnectionString(string server, string port, string user, string password, string database)
        {
            connstring = GetConnectionString(server, port, user, password, database);
        }

        public string GetConnectionString(string server, string port, string user, string password, string database)
        {
            return String.Format("Server={0};Port={1};" +
                "User Id={2};Password={3};Database={4};",
                server, port, user, password, database);
        }

        public string SetConnectionStringFromConfig()
        {
            connstring = GetConnectionString(
                System.Configuration.ConfigurationManager.AppSettings["DBServer"],
                System.Configuration.ConfigurationManager.AppSettings["DBPort"],
                System.Configuration.ConfigurationManager.AppSettings["DBUser"],
                System.Configuration.ConfigurationManager.AppSettings["DBPassword"],
                System.Configuration.ConfigurationManager.AppSettings["DBDatabaseName"]);
            return connstring;
        }
        public string GetConnectionStringFromConfig()
        {
            return GetConnectionString(
                System.Configuration.ConfigurationManager.AppSettings["DBServer"],
                System.Configuration.ConfigurationManager.AppSettings["DBPort"],
                System.Configuration.ConfigurationManager.AppSettings["DBUser"],
                System.Configuration.ConfigurationManager.AppSettings["DBPassword"],
                System.Configuration.ConfigurationManager.AppSettings["DBDatabaseName"]);
        }

        public void SetupDBProvider(string server, string port, string user, string password, string database) 
        {
            SetConnectionString(server, port, user, password, database);
            connection = new Npgsql.NpgsqlConnection(connstring);
            comm = new Npgsql.NpgsqlCommand("", connection);
        }

        public void SetupDBProviderFromConfig()
        {
            SetConnectionStringFromConfig();
            connection = new Npgsql.NpgsqlConnection(connstring);
            comm = new Npgsql.NpgsqlCommand("", connection);
        }


        public System.Data.IDataReader ExecuteReader()
        {
            return comm.ExecuteReader();
        }

        public System.Data.IDataReader ExecuteReader(string sqlcommand)
        {
            comm.CommandText = sqlcommand;
            return comm.ExecuteReader();
        }

        public int ExecuteNonQuery()
        {
            return comm.ExecuteNonQuery();
        }

        public int ExecuteNonQuery(string sqlcommand)
        {
            comm.CommandText = sqlcommand;
            return comm.ExecuteNonQuery();
        }

        public object GetValue(string pSelect)
        {
            comm.CommandText = pSelect;
            return comm.ExecuteScalar();
        }

        public object GetValue()
        {
            return comm.ExecuteScalar();
        }

        public void AddParameter(string name, object value, System.Data.DbType paramtype) 
        {
            
            Npgsql.NpgsqlParameter param = new Npgsql.NpgsqlParameter(name, paramtype);
            param.Value = value;
            
            comm.Parameters.Add(param);
        }

        public void ClearParameters() 
        {
            
            
            comm.Parameters.Clear();
        }
    }
}
