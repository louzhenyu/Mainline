using Dapper;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Data
{
    /// <summary>
    /// Job运行日志
    /// <para>2015-12-10</para>
    /// </summary>
    public class JobExecuteLogDAL
    {
        /// <summary>
        /// 获取Job运行日志列表
        /// </summary>
        public IEnumerable<JobExecuteLog> GetJobExecuteLogList(int jobId, DateTime startTime, DateTime endTime, PageItem pageItem)
        {
            string sql = string.Format("SELECT ID,JobId,ExecuteTime,Success FROM JobExecuteLog WITH(NOLOCK) WHERE JobId={0} AND ExecuteTime BETWEEN '{1}' AND '{2}' ORDER BY ID DESC ", jobId, startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return DapperHelper<JobExecuteLog>.GetPageList(ConnectionStr.FxDb, sql, pageItem);
        }

        /// <summary>
        /// 获取Job运行日志信息
        /// </summary>
        /// <param name="iD">编号</param>
        /// <returns>Job运行日志实体</returns>
        public JobExecuteLog GetJobExecuteLog(int iD)
        {
            string sql = "SELECT ID,JobId,ExecuteTime,Success FROM JobExecuteLog WITH(NOLOCK)  WHERE ID=@ID";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Query<JobExecuteLog>(sql, new { ID = iD }).SingleOrDefault<JobExecuteLog>();
            }
        }

        /// <summary>
        /// 修改Job运行日志信息
        /// </summary>
        public int UpdateJobExecuteLog(JobExecuteLog model)
        {
            string sql = "UPDATE JobExecuteLog SET JobId=@JobId,ExecuteTime=@ExecuteTime,Success=@Success WHERE ID=@ID";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }

        /// <summary>
        /// 添加Job运行日志信息
        /// </summary>
        public int AddJobExecuteLog(JobExecuteLog model)
        {
            string sql = "INSERT INTO JobExecuteLog (JobId,ExecuteTime,Success) VALUES(@JobId,@ExecuteTime,@Success)";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }

        /// <summary>
        /// 删除Job运行日志信息
        /// </summary>
        /// <param name="iD">编号</param>
        /// <returns>受影响的行数</returns>
        public int DeleteJobExecuteLog(int iD)
        {
            string sql = "UPDATE JobExecuteLog SET JobId=@JobId,ExecuteTime=@ExecuteTime,Success=@Success WHERE ID=@ID";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, new { ID = iD });
            }
        }

        /// <summary>
        /// 删除Job运行日志信息
        /// </summary>
        /// <param name="iD">编号</param>
        /// <returns>受影响的行数</returns>
        public int DeleteJobExecuteLog(List<int> listId)
        {
            if (listId == null || listId.Count <= 0) { return 0; }
            string ids = string.Join("','", listId);
            string sql = string.Format("DELETE JobExecuteLog WHERE ID IN ('{0}')", ids);
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql);
            }
        }
    }
}
