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
    /// 系统角色数据访问类
    /// </summary>
    public class SysRoleDal
    {
        /// <summary>
        /// 获取一个系统角色列表
        /// </summary>
        /// <param name="systemId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <returns></returns>
        public IEnumerable<SysRole> GetRoleList(string roleName, int systemId, int status, PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT RoleId,RoleName,SystemId,Status,Remark FROM SysRole WHERE 1=1 ");
            if (!string.IsNullOrEmpty(roleName))
            {
                sql.AppendFormat("AND RoleName LIKE '%{0}%' ", roleName);
            }
            if (systemId >= 0)
            {
                sql.AppendFormat("AND SystemId={0} ", systemId);
            }
            if (status >= 0)
            {
                sql.AppendFormat("AND Status={0} ", status);
            }
            sql.AppendFormat(" ORDER BY RoleId ");
            return DapperHelper<SysRole>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
        /// <summary>
        /// 获取单个角色信息
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>NULL未获取到角色信息，其他返回相应的角色信息</returns>
        public SysRole GetRoleInfo(int roleId)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "SELECT TOP 1 RoleId,RoleName,SystemId,Status,Remark FROM SysRole WITH(NOLOCK) WHERE RoleId=@RoleId";
                return conn.Query<SysRole>(sql, new { RoleId = roleId }).SingleOrDefault<SysRole>();
            }
        }

        public int UpdateRole(SysRole model)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "UPDATE SysRole SET RoleName=@RoleName,SystemId=@SystemId,Status=@Status,Remark=@Remark WHERE RoleId=@RoleId";
                return conn.Execute(sql,
                    new
                    {
                        RoleId = model.RoleId,
                        RoleName = model.RoleName,
                        SystemId = model.SystemId,
                        Status = model.Status,
                        Remark = model.Remark
                    });
            }
        }
        public int AddRole(SysRole model)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "INSERT INTO SysRole(RoleName,SystemId,Status,Remark) VALUES(@RoleName,@SystemId,@Status,@Remark)";
                return conn.Execute(sql,
                    new
                    {
                        RoleName = model.RoleName,
                        SystemId = model.SystemId,
                        Status = model.Status,
                        Remark = model.Remark
                    });
            }
        }

        public int DeleteRoleList(List<int> ids)
        {
            string id = string.Empty;
            if (ids == null || ids.Count <= 0) { return 0; }
            for (int index = 0; index < ids.Count; index++)
            {
                id += ids[index] + ",";
            }
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = string.Format("DELETE SysRole WHERE RoleId IN ({0})", id.Trim(','));
                return conn.Execute(sql);
            }
        }
    }
}
