using System;
using System.Data;
using System.Data.Common;
using System.Configuration;

public class SqlHelper:IDisposable
    {
        private static string dbProviderName = ConfigurationManager.ConnectionStrings["database"].ProviderName;  //db provider
        private static string dbConnectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;  //connection string
        private DbConnection connection;  //connection object
        
        #region Constructor
        public SqlHelper()
        {
            this.connection = CreateConnection(SqlHelper.dbConnectionString);
        }
        public SqlHelper(string connectionString)
        {
            this.connection = CreateConnection(connectionString);
        }
        #endregion

        #region New Connection
        /// <summary>
        /// 根据配置文件中的连接字符串创建数据库连接对象
        /// </summary>
        /// <returns>所创建的连接对象</returns>
        public static DbConnection CreateConnection()
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(SqlHelper.dbProviderName);
            DbConnection dbconn = dbfactory.CreateConnection();
            dbconn.ConnectionString = SqlHelper.dbConnectionString;
            return dbconn;
        }
        /// <summary>
        /// 用指定的连接字符串创建数据库连接对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>所创建的连接对象</returns>
        public static DbConnection CreateConnection(string connectionString)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(SqlHelper.dbProviderName);
            DbConnection dbconn = dbfactory.CreateConnection();
            dbconn.ConnectionString = connectionString;
            return dbconn;
        }
        #endregion

        #region New Command
        /// <summary>
        /// 创建存储过程命令
        /// </summary>
        /// <param name="storedProcedure">存储过程名称</param>
        /// <returns>所创建的命令</returns>
        public DbCommand GetStoredProcCommond(string storedProcedure)
        {
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = storedProcedure;
            dbCommand.CommandType = CommandType.StoredProcedure;
            return dbCommand;
        }
        /// <summary>
        /// 创建文本命令
        /// </summary>
        /// <param name="sql">命令文本</param>
        /// <returns>所创建的命令</returns>
        public DbCommand GetSqlStringCommond(string sql)
        {
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = sql;
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }
        #endregion
        
        #region New Parameter
        /*
        /// <summary>
        /// 批量添加命令参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="dbParameterCollection">参数集合</param>
        public void AddParameterCollection(DbCommand cmd, DbParameterCollection dbParameterCollection)
        {
            foreach (DbParameter dbParameter in dbParameterCollection)
            {
                cmd.Parameters.Add(dbParameter);
            }
        }
        /// <summary>
        /// 添加一个指定类型和大小的参数
        /// </summary>  
        public void AddParameter(DbCommand cmd, string parameterName, DbType dbType, int size, object value,bool output)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Size = size;
            dbParameter.Value = value;
            cmd.Parameters.Add(dbParameter);
        }
        /// <summary>
        /// 添加一个指定类型的参数
        /// </summary>
        public void AddParameter(DbCommand cmd, string parameterName, DbType dbType, object value,bool output)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            cmd.Parameters.Add(dbParameter);
        }
        /// <summary>
        /// 添加一个带值的参数
        /// </summary>
        public void AddParameter(DbCommand cmd, string parameterName, object value,bool output)
        {
            DbParameter dbParameter = cmd.CreateParameter();            
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            cmd.Parameters.Add(dbParameter);
        }
        public void AddReturnParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(dbParameter);
        }       
        */
        /// <summary>
        /// 为命令添加输出参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">大小</param>
        public void AddOutParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Size = size;
            dbParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(dbParameter);
        }
        /// <summary>
        /// 为命令添加输入参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="value">参数值</param>
        public void AddInParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(dbParameter);
        }
        /// <summary>
        /// 根据参数名得到参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="parameterName">参数名</param>
        /// <returns>得到的参数</returns>
        public DbParameter GetParameter(DbCommand cmd, string parameterName)
        {
            return cmd.Parameters[parameterName];
        }
        #endregion

        #region Execute Query
        /*
        public DataSet ExecuteDataSet(DbCommand cmd)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(SqlHelper.dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            dbDataAdapter.Fill(ds);
            return ds;
        }
        */

        //执行命令，得到DataReader对象
        public DbDataReader ExecuteReader(DbCommand cmd)
        {
            cmd.Connection.Open();
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
        //执行没有查询结果的命令(如insert,delete)，返回受影响的行数
        public int ExecuteNonQuery(DbCommand cmd)
        {
            cmd.Connection.Open();
            int ret = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return ret;
        }
        //执行命令，并返回查询到的单个值。
        public object ExecuteScalar(DbCommand cmd)
        {
            cmd.Connection.Open();
            object ret = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return ret;
        }
        /// <summary>
        /// 执行命令，返回一个DataTable
        /// </summary>
        /// <param name="cmd">要执行的命令</param>
        /// <returns>得到的DataTable</returns>
        public DataTable ExecuteDataTable(DbCommand cmd)
        {
            DbDataReader reader = ExecuteReader(cmd);
            DataTable table = new DataTable();
            table.Load(reader);
            reader.Close();
            return table;
        }
        #endregion

        #region Execute Query in transaction
        /*
        public DataSet ExecuteDataSet(DbCommand cmd, Trans t)
        {
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(SqlHelper.dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            dbDataAdapter.Fill(ds);
            return ds;
        }
        */
        public DataTable ExecuteDataTable(DbCommand cmd, Trans t)
        {
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(SqlHelper.dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            dbDataAdapter.Fill(dataTable);
            return dataTable;
        }

        public DbDataReader ExecuteReader(DbCommand cmd, Trans t)
        {
            cmd.Connection.Close();
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            DbDataReader reader = cmd.ExecuteReader();           
            return reader;
        }
        public int ExecuteNonQuery(DbCommand cmd, Trans t)
        {
            cmd.Connection.Close();
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            int ret = cmd.ExecuteNonQuery();
            return ret;
        }

        public object ExecuteScalar(DbCommand cmd, Trans t)
        {
            cmd.Connection.Close();
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            object ret = cmd.ExecuteScalar();
            return ret;
        }
        #endregion

        #region Dispose
        private bool disposed = false;
        #region IDisposable 成员
        public void Dispose()
        {
            dispose(true);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否用户代码调用</param>
        /// <remarks>
        /// 此方法将在两种情况下被调用，（1）用户代码调用Dispose方法（2）垃圾回收
        /// 在第2种情况下，数据库连接已经被垃圾回收自动处理，不需要再释放。
        /// 只有在第1种情况下需要在代码中显式关闭数据库连接。
        /// </remarks>
        private void dispose(bool disposing)
        {
            if (disposed) return;
            disposed = true;
            if(disposing)
                if (connection.State != ConnectionState.Closed)
                    connection.Close();            
        }
        #endregion
        #endregion
    }

    public class Trans : IDisposable
    {
        private DbConnection conn;
        private DbTransaction dbTrans;
        public DbConnection DbConnection
        {
            get { return this.conn; }
        }
        public DbTransaction DbTrans
        {
            get { return this.dbTrans; }
        }

        public Trans()
        {
            conn = SqlHelper.CreateConnection();
            conn.Open();
            dbTrans = conn.BeginTransaction();
        }
        public Trans(string connectionString)
        {
            conn = SqlHelper.CreateConnection(connectionString);
            conn.Open();
            dbTrans = conn.BeginTransaction();
        }
        public void Commit()
        {
            dbTrans.Commit();
            this.Close();
        }

        public void RollBack()
        {
            dbTrans.Rollback();
            this.Close();
        }

        public void Dispose()
        {
            this.Close();
        }

        public void Close()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
   

