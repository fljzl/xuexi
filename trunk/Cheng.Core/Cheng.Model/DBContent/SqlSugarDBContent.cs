using Cheng.Comon;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cheng.Model
{
    public class SqlSugarDBContent
    {
        public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作
        public SqlSugarDBContent(string keys)
        {
            var constring = ConfigExtensions.Configuration.GetConnectionString(keys);
            Db = new SqlSugarClient(
                new ConnectionConfig
                {
                    ConnectionString = keys,
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.SystemTable
                });

            //用来打印Sql方便你调式    
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
            };
        }

        public SqlSugarDBContent() : this("sqlserverquartz")
        {

        }
        //public DbSet<logMsg> logMsg { get { return new DbSet<logMsg>(Db); } }

        //public SimpleClient<WxSetting> WxSettingDb => new SimpleClient<WxSetting>(Db);
    }
    /// <summary>
    /// SimpleClient封装了单表大部分操作，此类为扩展类，以提供自定义的单表扩展方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbSet<T> : SimpleClient<T> where T : class, new()
    {
        public DbSet(SqlSugarClient context) : base(context) { }
    }
}
