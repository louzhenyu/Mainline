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
    /// 应用类型数据访问类
    /// </summary>
    public class SysAppTypeDal
    {
        /// <summary>
        /// 获取应用类型列表
        /// </summary>
        /// <param name="typeName">应用类型名称</param>
        /// <returns></returns>
        public IEnumerable<SysAppTypeEntity> GetAppTypeList(string typeName, PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT AppTypeId,TypeName FROM SysAppType WHERE 1=1 ");
            if (!string.IsNullOrEmpty(typeName))
            {
                sql.AppendFormat("AND TypeName LIKE '%{0}%' ", typeName);
            }
            sql.AppendFormat(" ORDER BY AppTypeId ");
            return DapperHelper<SysAppTypeEntity>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
        /// <summary>
        /// 获取应用类型信息
        /// </summary>
        /// <param name="TypeName">应用类型名称</param>
        /// <returns></returns>
        public SysAppTypeEntity GetAppTypeInfo(int AppTypeId)
        {
            string sql = "SELECT TOP 1 AppTypeId,TypeName FROM SysAppType WITH(NOLOCK) WHERE AppTypeId=@AppTypeId";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Query<SysAppTypeEntity>(sql, new { AppTypeId = AppTypeId }).SingleOrDefault<SysAppTypeEntity>();
            }
        }

        public int UpdateAppType(SysAppTypeEntity model)
        {
            string sql = "UPDATE SysAppType SET TypeName=@TypeName WHERE AppTypeId=@AppTypeId";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }
        public int AddAppType(SysAppTypeEntity model)
        {
            string sql = "INSERT INTO SysAppType(AppTypeId,TypeName) VALUES(@AppTypeId,@TypeName)";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }

        public int DeleteAppTypeList(List<int> ids)
        {
            string id = string.Empty;
            if (ids == null || ids.Count <= 0) { return 0; }
            for (int index = 0; index < ids.Count; index++)
            {
                id += ids[index] + ",";
            }
            string sql = string.Format("DELETE SysAppType WHERE AppTypeId IN ({0})", id.Trim(','));
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql);
            }
        }
    }
}
