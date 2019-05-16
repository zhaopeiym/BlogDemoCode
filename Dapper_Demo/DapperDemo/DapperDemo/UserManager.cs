using Dapper;
using System;

namespace DapperDemo
{
    public class UserManager
    {
        private DBContext dBContext;
        public UserManager(DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public void AddUser()
        {
        }

        [UnitOfWork]
        public virtual void DelUser()
        {
            var sql = "select * from UserTemp";
            var userList = dBContext.DbConnection.Query<object>(sql);

            var sql2 = $@"INSERT into UserTemp VALUES(0,'{DateTime.Now.ToString()}','sql2执行成功')";
            dBContext.DbConnection.Execute(sql2);
            //throw new Exception("主动报错");

            var sq3 = $@"INSERT into UserTemp VALUES(0,'{DateTime.Now.ToString()}','sq3执行成功')";
            dBContext.DbConnection.Execute(sq3);
        }
    }
}
