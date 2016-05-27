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
    /// 应用数据访问类
    /// </summary>
    public class SysApplicationDal
    {
        /// <summary>
        /// 获取应用列表
        /// </summary>
        /// <param name="systemId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <returns></returns>
        public IEnumerable<SysApplicationEntity> GetSysApplicationList(string appId, PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT AppId FROM SysApplication WHERE 1=1 ");
            if (!string.IsNullOrEmpty(appId))
            {

                sql.AppendFormat("AND (AppId LIKE '%{0}%') ", appId);
            }
           
            
            sql.AppendFormat(" ORDER BY AppId ");
            return DapperHelper<SysApplicationEntity>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
        /// <summary>
        /// 获取单个应用信息
        /// </summary>
        /// <param name="appId">应用编号</param>
        /// <returns></returns>
        public SysApplicationEntity GetSysApplicationInfo(int appId)
        {
            string sql = "SELECT TOP 1 AppId,SubSystemId,AppName,AppEName,AppTypeId,Owner,Status,Remark,AddTime FROM SysApplication WITH(NOLOCK) WHERE AppId=@AppId";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Query<SysApplicationEntity>(sql, new { AppId = appId }).SingleOrDefault<SysApplicationEntity>();
            }
        }

    }
}
