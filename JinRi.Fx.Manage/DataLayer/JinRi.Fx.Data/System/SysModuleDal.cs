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
    /// 系统模块数据访问类
    /// </summary>
    public class SysModuleDal
    {
        /// <summary>
        /// 获取一个系统模块列表
        /// </summary>
        /// <param name="systemId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <returns></returns>
        public IEnumerable<SysModule> GetModuleList(string moduleName, int systemId, int status, PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ModuleId,ModuleName,SystemId,ModuleUrl,ImageUrl,Sort,Status,Remark FROM SysModule WHERE 1=1 ");
            if (!string.IsNullOrEmpty(moduleName))
            {
                sql.AppendFormat("AND ModuleName LIKE '%{0}%' ", moduleName);
            }
            if (systemId >= 0)
            {
                sql.AppendFormat("AND SystemId={0} ", systemId);
            }
            if (status >= 0)
            {
                sql.AppendFormat("AND Status={0} ", status);
            }
            sql.AppendFormat(" ORDER BY SORT ");
            return DapperHelper<SysModule>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
        /// <summary>
        /// 获取单个模块信息
        /// </summary>
        /// <param name="moduleId">模块编号</param>
        /// <returns>NULL未获取到菜单信息，其他返回相应的菜单信息</returns>
        public SysModule GetModuleInfo(int moduleId)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "SELECT TOP 1 ModuleId,ModuleName,SystemId,ModuleUrl,ImageUrl,Sort,Status,Remark FROM SysModule WITH(NOLOCK) WHERE ModuleId=@ModuleId";
                return conn.Query<SysModule>(sql, new { ModuleId = moduleId }).SingleOrDefault<SysModule>();
            }
        }

        public int UpdateModule(SysModule model)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "UPDATE SysModule SET ModuleName=@ModuleName,SystemId=@SystemId,ModuleUrl=@ModuleUrl,ImageUrl=@ImageUrl,Sort=@Sort,Status=@Status,Remark=@Remark WHERE ModuleId=@ModuleId";
                return conn.Execute(sql,
                    new
                    {
                        ModuleId = model.ModuleId,
                        ModuleName = model.ModuleName,
                        SystemId = model.SystemId,
                        ModuleUrl = model.ModuleUrl,
                        ImageUrl = model.ImageUrl,
                        Sort = model.Sort,
                        Status = model.Status,
                        Remark = model.Remark
                    });
            }
        }
        public int AddModule(SysModule model)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "INSERT INTO SysModule(ModuleName,SystemId,ModuleUrl,ImageUrl,Sort,Status,Remark) VALUES(@ModuleName,@SystemId,@ModuleUrl,@ImageUrl,@Sort,@Status,@Remark)";
                return conn.Execute(sql,
                    new
                    {
                        ModuleName = model.ModuleName,
                        SystemId = model.SystemId,
                        ModuleUrl = model.ModuleUrl,
                        ImageUrl = model.ImageUrl,
                        Sort = model.Sort,
                        Status = model.Status,
                        Remark = model.Remark
                    });
            }
        }

        public int DeleteModuleList(List<int> ids)
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
                string sql = string.Format("DELETE SysModule WHERE ModuleId IN ({0})", id.Trim(','));
                return conn.Execute(sql);
            }
        }
    }
}
