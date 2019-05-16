using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using StackExchange.Profiling;
using System.Data;
using System.Data.Common;

namespace DapperDemo
{
    /// <summary>
    /// 注册的时候 InstancePerLifetimeScope
    /// 线程内唯一（也就是单个请求内唯一）
    /// </summary>
    public class DBContext
    {
        private IDbConnection _dbConnection;
        private string mySqlConnection;
        public IDbConnection DbConnection
        {
            get
            {
                if (_dbConnection == null)
                {
                    _dbConnection = new MySqlConnection(mySqlConnection);
                    if (MiniProfiler.Current != null)
                    {
                        _dbConnection = new StackExchange.Profiling.Data.ProfiledDbConnection((DbConnection)_dbConnection, MiniProfiler.Current);
                        //https://stackoverflow.com/questions/50581540/dapper-contrib-and-miniprofiler-for-mysql-integration-issues
                        //SqlMapperExtensions.GetDatabaseType = conn => "MySqlConnection";
                    }
                }
                return _dbConnection;
            }
        }
        public DBContext(IConfiguration Configuration)
        {
            //读取配置文件，数据库连接字符串
            mySqlConnection = Configuration.GetValue<string>("MySQLSPConnection");
        }

        public IDbTransaction DbTransaction { get; set; }

        /// <summary>
        /// 是否已被提交
        /// </summary>
        public bool Committed { get; private set; } = true;

        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            Committed = false;
            bool isClosed = DbConnection.State == ConnectionState.Closed;
            if (isClosed) DbConnection.Open();
            DbTransaction = DbConnection?.BeginTransaction();
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void CommitTransaction()
        {
            DbTransaction?.Commit();
            Committed = true;

            Dispose();
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void RollBackTransaction()
        {
            DbTransaction?.Rollback();
            Committed = true;

            Dispose();
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            DbTransaction?.Dispose();
            if (DbConnection.State == ConnectionState.Open)
                _dbConnection?.Close();
        }
    }
}
