using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace JinRi.Fx.Data
{
    public class EtermScriptDal
    {
        /// <summary>
        /// 根据Eterm脚本ID号，获得单条Eterm脚本信息记录
        /// </summary>
        /// <param name="etermScriptId">Eterm脚本ID号</param>
        /// <returns></returns>
        public EtermScript GetEtermScript(int etermScriptId)
        {
            using (IDbConnection connection = DapperHelper<object>.OpenConnection())
            {                
                const string sql =
@"SELECT TOP 1 id AS EtermScriptID, method AS MethodName, script AS ScriptContent, remark AS Remark, updatetime AS UpdateTime
FROM dbo.EtermScripts WITH(NOLOCK)
WHERE id = @EtermScriptID";
                return connection.Query<EtermScript>(sql, new { EtermScriptID = etermScriptId }).SingleOrDefault<EtermScript>();
            }
        }

        /// <summary>
        /// 根据方法名，获得单条Eterm脚本信息记录
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public EtermScript GetEtermScript(string methodName)
        {
            using (IDbConnection connection = DapperHelper<object>.OpenConnection())
            {             
                const string sql =
@"SELECT TOP 1 id AS EtermScriptID, method AS MethodName, script AS ScriptContent, remark AS Remark, updatetime AS UpdateTime
FROM dbo.EtermScripts WITH(NOLOCK)
WHERE method = @MethodName";
                return connection.Query<EtermScript>(sql, new { MethodName = methodName }).SingleOrDefault<EtermScript>();
            }
        }

        /// <summary>
        /// 获得满足条件的Eterm脚本记录集。如果 PageItem 为null，则将获得所有满足条件的Eterm脚本记录集；如果 PageItem 不为null，则将获得某页的满足条件的Eterm脚本记录集。
        /// </summary>
        /// <param name="searchCondition">查询条件</param>
        /// <param name="pageItem">分页信息</param>
        /// <returns></returns>
        public IEnumerable<EtermScript> GetEtermScriptList(EtermScript searchCondition, PageItem pageItem = null)
        {
            string sql = 
@"SELECT id AS EtermScriptID, method AS MethodName, script AS ScriptContent, remark AS Remark, updatetime AS UpdateTime
FROM dbo.EtermScripts WITH(NOLOCK)
WHERE 1 = 1";

            StringBuilder whereBuilder = new StringBuilder();
            if(searchCondition != null)
            {
                if (!string.IsNullOrWhiteSpace(searchCondition.MethodName))
                {
                    whereBuilder.AppendLine(" AND method LIKE '" + searchCondition.MethodName + "%'");
                }
            }
            if (whereBuilder.Length > 0)
            {
                sql += whereBuilder.ToString();
            }
            sql += " ORDER BY updatetime DESC";

            return DapperHelper<EtermScript>.GetPageList(ConnectionStr.FxDb, sql, pageItem);
        }

        /// <summary>
        /// 新增一条Eterm脚本记录
        /// </summary>
        /// <param name="item">要新增的Eterm脚本实例</param>
        /// <returns>返回影响行数</returns>
        public int AddEtermScript(EtermScript item)
        {
            if (item == null)
            {
                return -1;
            }

            using (IDbConnection connection = DapperHelper<object>.OpenConnection())
            {
                const string sql =
@"INSERT INTO dbo.EtermScripts(method, script, remark, updatetime) VALUES(@MethodName, @ScriptContent, @Remark, GETDATE())";

                return connection.Execute(sql, item);
            }
        }

        /// <summary>
        /// 更新一条Eterm脚本记录
        /// </summary>
        /// <param name="item">要更新的Eterm脚本实例</param>
        /// <returns>返回影响行数</returns>
        public int UpdateEtermScript(EtermScript item)
        {
            if (item == null || item.EtermScriptID < 1)
            {
                return -1;
            }

            using (IDbConnection connection = DapperHelper<object>.OpenConnection())
            {
                const string sql =
@"UPDATE dbo.EtermScripts 
  SET method = @MethodName,
      script = @ScriptContent,
      remark = @Remark,
      updatetime = GETDATE()
  WHERE id = @EtermScriptID";

                return connection.Execute(sql, item);
            }
        }

        /// <summary>
        /// 删除多条Eterm脚本记录
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int DeleteEtermScriptList(IList<EtermScript> list)
        {
            using (IDbConnection connection = DapperHelper<object>.OpenConnection())
            {
                const string sql = @"DELETE FROM dbo.EtermScripts WHERE id = @EtermScriptID";
                return connection.Execute(sql, list);
            }
        }
    }
}
