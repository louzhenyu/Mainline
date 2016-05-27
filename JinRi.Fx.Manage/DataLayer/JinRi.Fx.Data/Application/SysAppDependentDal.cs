using JinRi.Fx.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using JinRi.Fx.Utility;

namespace JinRi.Fx.Data
{
    /// <summary>
    /// 应用依赖关系访问类
    /// </summary>
    public class SysAppDependentDal
    {
        public int BindingDependent(int appId, int dependentId)
        {
            string sql = "INSERT INTO SysAppDependent(AppId,DependentAppId) VALUES(@AppId,@DependentAppId)";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, new { AppId = appId, DependentAppId = dependentId });
            }
        }
        public int DeleteDependent(int appId, int dependentId)
        {
            string sql = "DELETE SysAppDependent WHERE AppId=@AppId AND DependentAppId=@DependentAppId";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, new { AppId = appId, DependentAppId = dependentId });
            }
        }
        /// <summary>
        /// 获取依赖、被依赖的APP
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<SysAppDependentEntity> GetAppDependentList(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT Id,AppId,DependentAppId FROM SysAppDependent WHERE 1=1 ");
            if (id >= 0)
            {
                sql.AppendFormat("AND (AppId = {0} OR  DependentAppId = {0} ) ", id);
            }
            sql.AppendFormat(" ORDER BY AppId ");
            return DapperHelper<SysAppDependentEntity>.GetPageList(ConnectionStr.FxDb, sql.ToString(), null);
        }
    }
}
