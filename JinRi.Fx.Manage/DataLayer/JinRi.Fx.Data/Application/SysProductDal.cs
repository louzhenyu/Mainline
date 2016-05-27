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
    /// 产品线数据访问类
    /// </summary>
    public class SysProductDal
    {
        /// <summary>
        /// 获取一个产品线列表
        /// </summary>
        /// <param name="systemId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <returns></returns>
        public IEnumerable<SysProductEntity> GetProductList(string productName, PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ProductId,ProductName,ProductEName,Remark FROM SysProduct WHERE 1=1 ");
            if (!string.IsNullOrEmpty(productName))
            {
                sql.AppendFormat("AND (ProductName LIKE '%{0}%' OR ProductEName LIKE '%{0}%') ", productName);
            }
            sql.AppendFormat(" ORDER BY ProductId ");
            return DapperHelper<SysProductEntity>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
        /// <summary>
        /// 获取单个角色信息
        /// </summary>
        /// <param name="ProductId">角色编号</param>
        /// <returns>NULL未获取到角色信息，其他返回相应的角色信息</returns>
        public SysProductEntity GetProductInfo(int ProductId)
        {
            string sql = "SELECT TOP 1 ProductId,ProductName,ProductEName,Remark FROM SysProduct WITH(NOLOCK) WHERE ProductId=@ProductId";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Query<SysProductEntity>(sql, new { ProductId = ProductId }).SingleOrDefault<SysProductEntity>();
            }
        }

        public int UpdateProduct(SysProductEntity model)
        {
            string sql = "UPDATE SysProduct SET ProductName=@ProductName,ProductEName=@ProductEName,Remark=@Remark WHERE ProductId=@ProductId";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }
        public int AddProduct(SysProductEntity model)
        {
            string sql = "INSERT INTO SysProduct(ProductId,ProductName,ProductEName,Remark) VALUES(@ProductId,@ProductName,@ProductEName,@Remark)";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }

        public int DeleteProductList(List<int> ids)
        {
            string id = string.Empty;
            if (ids == null || ids.Count <= 0) { return 0; }
            for (int index = 0; index < ids.Count; index++)
            {
                id += ids[index] + ",";
            }
            string sql = string.Format("DELETE SysProduct WHERE ProductId IN ({0})", id.Trim(','));
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql);
            }
        }
    }
}
