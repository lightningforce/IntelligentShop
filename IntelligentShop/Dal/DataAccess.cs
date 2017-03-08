using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Configuration;
namespace BSD.Dal
{
    public enum Provider
    {
        MSSQL,
        MySQL
    }
    public class DataAccess : IDisposable
    {
        #region private properties
        private DbConnection _connection = null;
        private DbCommand _command = null;
        private DbProviderFactory _factory = null;
        private string _connectionString = string.Empty;
        #endregion
        #region constructor
        public DataAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            if (_connectionString == null)
            {
                _connectionString = "Data Source=light;Initial Catalog=IntelligentShop;User ID=dev01;Password=p@ssw0rd";
            }
        }
        public DataAccess(string connectionName)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            if (_connectionString == null)
            {
                _connectionString = "Data Source=light;Initial Catalog=IntelligentShop;User ID=dev01;Password=p@ssw0rd";
            }
        }
        public void Dispose()
        {
            if (_command != null)
            {
                _command.Dispose();
            }
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
        #endregion
        #region private method
        private void CreateConnection()
        {
            if (_connection == null || _connection.State != System.Data.ConnectionState.Closed)
            {
                _connection = _factory.CreateConnection();
                _connection.ConnectionString = _connectionString;
            }
            _connection.Open();
        }
        #endregion
        #region public method
        /// <summary>
        /// Open Connection
        /// </summary>
        /// <param name="provider"></param>
        public void Open(Provider provider)
        {
            if (provider == Provider.MSSQL)
            {
                _factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            }
            else
            {
                _factory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
            }
            CreateConnection();
        }
        /// <summary>
        /// get connection
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection()
        {
            return _connection;
        }
        #endregion
        /// <summary>
        /// create command
        /// </summary>
        /// <returns></returns>
        public DbCommand CreateCommand()
        {
            DbCommand cmd = _factory.CreateCommand();
            cmd.Connection = _connection;
            return cmd;
        }
        /// <summary>
        /// create command with sql text
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public DbCommand CreateCommand(string sqlString)
        {
            _command = CreateCommand();
            _command.CommandText = sqlString;
            return _command;
        }
        /// <summary>
        /// create parameter
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string paramName,object paramValue)
        {
            DbParameter param = _factory.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            return param;
        }
        /// <summary>
        /// create data adapter by sql text
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public DbDataAdapter CreateDataAdapter(string sqlString)
        {
            DbDataAdapter adapter = _factory.CreateDataAdapter();
            adapter.SelectCommand = CreateCommand(sqlString);
            return adapter;
        }
        /// <summary>
        /// create adapter by command
        /// </summary>
        /// <param name="inCmd"></param>
        /// <returns></returns>
        public DbDataAdapter CreateDataAdapter(DbCommand inCmd)
        {
            DbDataAdapter adapter = _factory.CreateDataAdapter();
            adapter.SelectCommand = inCmd;
            return adapter;
        }
        public void SetCommand(DbCommand cmd)
        {
            _command = cmd;
        }

        public int SaveChanges()
        {
            return _command.ExecuteNonQuery();
        }
    }
}