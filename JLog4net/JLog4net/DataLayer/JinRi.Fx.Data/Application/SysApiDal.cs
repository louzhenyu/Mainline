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
    /// 系统接口数据访问类
    /// </summary>
    public class SysApiDal
    {
        /// <summary>
        /// 获取一个系统接口列表
        /// </summary>
        /// <param name="systemId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <returns></returns>
        public IEnumerable<SysApiEntity> GetSysApiList(int appId, int status, string aipName, PageItem pageItem)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT SysApiId,AppId,ApiName,ApiType,ApiDescription,ApiOwner,ApiAddress,ApiStatus,Remark,AddTime FROM SysApi WHERE 1=1 ");
            if (appId >= 0)
            {
                sql.AppendFormat("AND AppId={0} ", appId);
            }
            if (status >= 0)
            {
                sql.AppendFormat("AND ApiStatus={0} ", status);
            }
            if (!string.IsNullOrEmpty(aipName))
            {
                sql.AppendFormat("AND (ApiName LIKE '%{0}%') ", aipName);
            }
            sql.AppendFormat(" ORDER BY SysApiId ");
            return DapperHelper<SysApiEntity>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
        /// <summary>
        /// 获取单个系统接口信息
        /// </summary>
        /// <param name="sysApiId">编号</param>
        /// <returns>NULL未获取到系统接口信息，其他返回相应的系统接口信息</returns>
        public SysApiEntity GetSysApiInfo(int sysApiId)
        {
            string sql = "SELECT SysApiId,AppId,ApiName,ApiType,ApiDescription,ApiOwner,ApiAddress,ApiStatus,Remark,AddTime FROM SysApi WHERE SysApiId=@SysApiId ";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Query<SysApiEntity>(sql, new { SysApiId = sysApiId }).SingleOrDefault<SysApiEntity>();
            }
        }

        public int UpdateSysApi(SysApiEntity model)
        {
            string sql = "UPDATE SysApi SET AppId=@AppId,ApiName=@ApiName,ApiType=@ApiType,ApiDescription=@ApiDescription,ApiOwner=@ApiOwner,ApiAddress=@ApiAddress,ApiStatus=@ApiStatus,Remark=@Remark WHERE SysApiId=@SysApiId ";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }
        public int AddSysApi(SysApiEntity model)
        {
            string sql = "INSERT INTO SysApi(AppId,ApiName,ApiType,ApiDescription,ApiOwner,ApiAddress,ApiStatus,Remark,AddTime) VALUES(@AppId,@ApiName,@ApiType,@ApiDescription,@ApiOwner,@ApiAddress,@ApiStatus,@Remark,GETDATE())";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }

        public int DeleteSysApiList(List<int> ids)
        {
            string id = string.Empty;
            if (ids == null || ids.Count <= 0) { return 0; }
            for (int index = 0; index < ids.Count; index++)
            {
                id += ids[index] + ",";
            }
            string sql = string.Format("DELETE SysApi WHERE SysApiId IN ({0})", id.Trim(','));
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql);
            }
        }
    }
}
