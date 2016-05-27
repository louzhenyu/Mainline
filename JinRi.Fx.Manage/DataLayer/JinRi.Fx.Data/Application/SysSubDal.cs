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
    /// 子系统数据访问类
    /// </summary>
    public class SysSubDal
    {
        /// <summary>
        /// 获取子系统列表
        /// </summary>
        /// <param name="productId">负数表示不限制</param>
        /// <param name="systemName">子系统名称</param>
        /// <returns></returns>
        public IEnumerable<SysSubEntity> GetSubSystemList(int productId, string systemName, PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT SubSystemId,ProductId,SystemName,SystemEName,Remark FROM SysSub WHERE 1=1 ");
            if (productId > 0)
            {
                sql.AppendFormat("AND ProductId={0} ", productId);
            }
            if (!string.IsNullOrEmpty(systemName))
            {
                sql.AppendFormat("AND (SystemName LIKE '%{0}%' OR SystemEName LIKE '%{0}%' )", systemName);
            }
            sql.AppendFormat(" ORDER BY SubSystemId ");
            return DapperHelper<SysSubEntity>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
        /// <summary>
        /// 获取单个子系统信息
        /// </summary>
        /// <param name="ProductId">子系统编号</param>
        /// <returns>NULL未获取到子系统信息，其他返回相应的子系统信息</returns>
        public SysSubEntity GetSubSystemInfo(int SubSystemId)
        {
            string sql = "SELECT TOP 1 SubSystemId,ProductId,SystemName,SystemEName,Remark FROM SysSub WITH(NOLOCK) WHERE SubSystemId=@SubSystemId";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Query<SysSubEntity>(sql, new { SubSystemId = SubSystemId }).SingleOrDefault<SysSubEntity>();
            }
        }

        public int UpdateSubSystem(SysSubEntity model)
        {
            string sql = "UPDATE SysSub SET ProductId=@ProductId,SystemName=@SystemName,SystemEName=@SystemEName,Remark=@Remark WHERE SubSystemId=@SubSystemId";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }
        public int AddSubSystem(SysSubEntity model)
        {
            string sql = "INSERT INTO SysSub(SubSystemId,ProductId,SystemName,SystemEName,Remark) VALUES(@SubSystemId,@ProductId,@SystemName,@SystemEName,@Remark)";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }

        public int DeleteSubSystemList(List<int> ids)
        {
            string id = string.Empty;
            if (ids == null || ids.Count <= 0) { return 0; }
            for (int index = 0; index < ids.Count; index++)
            {
                id += ids[index] + ",";
            }
            string sql = string.Format("DELETE SysSub WHERE SubSystemId IN ({0})", id.Trim(','));
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql);
            }
        }
    }
}
