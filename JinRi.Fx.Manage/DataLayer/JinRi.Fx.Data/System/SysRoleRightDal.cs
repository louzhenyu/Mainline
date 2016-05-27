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
    /// 系统角色权限数据访问类
    /// </summary>
    public class SysRoleRightDal
    {
        /// <summary>
        /// 获取一个系统角色权限列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="isAppId"></param>
        /// <param name="pageItem"></param>
        /// <returns></returns>
        public IEnumerable<SysRoleRight> GetRoleRightList(int roleId, bool isAppId = false, int userId = -1, PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            if (isAppId)
            {
                sql.Append("SELECT ID,RoleId,UserId,(SELECT TOP 1 MenuId FROM dbo.SysMenu WHERE MenuName = '" + AppSettingsHelper.ConfigCenterMenuName + "') AS MenuId,AppId FROM SysRoleRight WHERE 1=1 ");
            }
            else
            {
                sql.Append("SELECT ID,RoleId,UserId,MenuId,AppId FROM SysRoleRight WHERE 1=1 ");
            }
            if (roleId >= 0)
            {
                sql.AppendFormat("and RoleId={0} ", roleId);
            }
            if (isAppId)
            {
                sql.Append("and MenuId = (SELECT TOP 1 MenuId FROM SysMenu WHERE MenuName = '" + AppSettingsHelper.ConfigCenterMenuName + "') ");
                sql.Append("and AppId > 0 and UserId > 0 ");
            }
            else
            {
                sql.Append("and AppId = 0 and UserId = 0 ");
            }
            if (userId > 0)
            {
                sql.AppendFormat("and UserId = {0} ", userId);
            }
            sql.AppendFormat("ORDER BY ID ");
            return DapperHelper<SysRoleRight>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isMenuRight">true表示只求各菜单访问权限；false表示只求配置中心的各应用的访问权限</param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <param name="menuId"></param>
        /// <param name="menuName"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public SysRoleRight GetRoleRight(bool isMenuRight, int roleId, int userId, int menuId, string menuName, int appId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT TOP 1 ID, RoleId, UserId, MenuId, AppId FROM SysRoleRight WHERE 1 = 1 ");
            if (isMenuRight)
            {
                sql.Append("AND (UserId = 0 AND AppId = 0) ");
            }
            else
            {
                sql.Append("AND (UserId > 0 AND AppId > 0) ");
            }
            if (roleId > 0)
            {
                sql.AppendFormat("AND RoleId = {0} ", roleId);
            }
            if(userId > 0)
            {
                sql.AppendFormat("AND UserId = {0} ", userId);
            }            
            if (menuId > 0)
            {
                sql.AppendFormat("AND MenuId = {0} ", menuId);
            }            
            if (!string.IsNullOrWhiteSpace(menuName))
            {
                sql.AppendFormat("AND MenuId = (SELECT TOP 1 MenuId FROM SysMenu WHERE MenuName = '{0}') ", menuName);
            }
            if (appId > 0)
            {
                sql.AppendFormat("AND AppId = {0} ", appId);
            }
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Query<SysRoleRight>(sql.ToString()).SingleOrDefault<SysRoleRight>();
            }
        }

        public int UpdateRoleRight(SysRoleRight model)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "UPDATE SysRoleRight SET RoleId=@RoleId,MenuId=@MenuId WHERE Id=@Id";
                return conn.Execute(sql,
                    new
                    {
                        Id = model.ID,
                        RoleId = model.RoleId,
                        MenuId = model.MenuId
                    });
            }
        }

        public int AddRoleRight(SysRoleRight model)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "INSERT INTO SysRoleRight(RoleId,MenuId) VALUES(@RoleId,@MenuId)";
                return conn.Execute(sql,
                    new
                    {
                        RoleId = model.RoleId,
                        MenuId = model.MenuId
                    });
            }
        }

        public int DeleteRoleRightList(List<int> ids)
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
                string sql = "DELETE SysRoleRight WHERE RoleId IN (@RoleId)";
                return conn.Execute(sql, new { RoleId = id.Trim(',') });
            }
        }


        public bool SaveRoleRight(int roleId, List<SysRoleRight> list, bool isAppId = false, int userId = 0)
        {
            StringBuilder command = new StringBuilder();
            if (isAppId) //true表示重新设置配置中心权限，false表示重新设置角色权限
            {
                if (userId > 0)
                {
                    command.AppendFormat("DELETE SysRoleRight WHERE RoleId = {0} AND MenuId = (SELECT TOP 1 MenuId FROM dbo.SysMenu WHERE MenuName = '{1}') AND AppId > 0 AND UserId = {2} ", roleId, AppSettingsHelper.ConfigCenterMenuName, userId);
                }
            }
            else
            {
                command.AppendFormat("DELETE SysRoleRight WHERE AppId = 0 AND UserId = 0 AND RoleId = {0} ", roleId);
            }

            for (int index = 0; index < list.Count; index++)
            {
                command.AppendFormat("INSERT INTO SysRoleRight(RoleId,UserId,MenuId,AppId) VALUES({0},{1},{2},{3})", roleId, userId, list[index].MenuId, list[index].AppId);
            }

            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(command.ToString()) > 0;
            }
        }
    }
}
