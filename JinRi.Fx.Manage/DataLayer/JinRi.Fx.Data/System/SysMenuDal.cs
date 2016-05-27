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
    /// 系统菜单数据访问类
    /// </summary>
    public class SysMenuDal
    {
        /// <summary>
        /// 获取一个系统菜单列表
        /// </summary>
        /// <param name="menuName">负数表示不限制</param>
        /// <param name="moduleId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <param name="pageItem">分页信息</param>
        /// <returns></returns>
        public IEnumerable<SysMenu> GetMenuList(string menuName, int moduleId, int status, PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT MenuId,MenuName,MenuUrl,ImageUrl,ModuleId,Sort,Status,Remark FROM SysMenu WHERE 1=1 ");
            if (!string.IsNullOrEmpty(menuName))
            {
                sql.AppendFormat("and MenuName LIKE '%{0}%' ", menuName);
            }
            if (moduleId >= 0)
            {
                sql.AppendFormat("and ModuleId={0} ", moduleId);
            }
            if (status >= 0)
            {
                sql.AppendFormat("and Status={0} ", status);
            }
            sql.AppendFormat("ORDER BY SORT ");
            return DapperHelper<SysMenu>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
        public IEnumerable<SysMenu> GetUserMenuList(int userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT C.MenuId,C.MenuName,C.MenuUrl,C.ImageUrl,C.ModuleId,C.Sort,C.Status,C.Remark ");
            sql.AppendLine("FROM SysUser A ");
            sql.AppendLine("INNER JOIN SysRoleRight B ON A.RoleId=B.RoleId ");
            sql.AppendLine("INNER JOIN SysMenu C ON B.MenuId=C.MenuId ");
            sql.AppendFormat("WHERE C.Status=0 AND A.UserId={0} ", userId);
            sql.AppendLine("ORDER BY C.Sort ");

            return DapperHelper<SysMenu>.GetPageList(ConnectionStr.FxDb, sql.ToString(), null);
        }

        /// <summary>
        /// 获取单个菜单信息
        /// </summary>
        /// <param name="menuId">菜单编号</param>
        /// <returns>NULL未获取到菜单信息，其他返回相应的菜单信息</returns>
        public SysMenu GetMenuInfo(int menuId)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "SELECT TOP 1 MenuId,MenuName,MenuUrl,ImageUrl,ModuleId,Sort,Status,Remark FROM SysMenu WITH(NOLOCK) WHERE MenuId=@MenuId";
                return conn.Query<SysMenu>(sql, new { MenuId = menuId }).SingleOrDefault<SysMenu>();
            }
        }

        public int UpdateMenu(SysMenu model)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "UPDATE SysMenu SET MenuName=@MenuName,MenuUrl=@MenuUrl,ImageUrl=@ImageUrl,ModuleId=@ModuleId,Sort=@Sort,Status=@Status,Remark=@Remark WHERE MenuId=@MenuId";
                return conn.Execute(sql,
                    new
                    {
                        MenuId = model.MenuId,
                        MenuName = model.MenuName,
                        MenuUrl = model.MenuUrl,
                        ImageUrl = model.ImageUrl,
                        ModuleId = model.ModuleId,
                        Sort = model.Sort,
                        Status = model.Status,
                        Remark = model.Remark
                    });
            }
        }
        public int AddMenu(SysMenu model)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "INSERT INTO SysMenu(MenuName,MenuUrl,ImageUrl,ModuleId,Sort,Status,Remark) VALUES(@MenuName,@MenuUrl,@ImageUrl,@ModuleId,@Sort,@Status,@Remark)";
                return conn.Execute(sql,
                    new
                    {
                        MenuName = model.MenuName,
                        MenuUrl = model.MenuUrl,
                        ImageUrl = model.ImageUrl,
                        ModuleId = model.ModuleId,
                        Sort = model.Sort,
                        Status = model.Status,
                        Remark = model.Remark
                    });
            }
        }

        public int DeleteMenuList(List<int> ids)
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
                string sql = string.Format("DELETE SysMenu WHERE MenuId IN ({0})", id.Trim(','));
                return conn.Execute(sql);
            }
        }
    }
}
