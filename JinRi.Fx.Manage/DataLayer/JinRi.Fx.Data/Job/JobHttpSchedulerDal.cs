using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using JinRi.Fx.Entity;
using Dapper;
using JinRi.Fx.Utility;

namespace JinRi.Fx.Data
{
    public class JobHttpSchedulerDal
    {
        /// <summary>
        /// 获取单个Job信息
        /// </summary>
        /// <param name="jobId">Job编号</param>
        /// <returns>NULL未获取到Job信息，其他返回相应的Job信息</returns>
        public JobHttpScheduler GetJobInfo(int jobId)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "SELECT TOP 1 JobHttpSchedulerID,JobName,GroupName,RequestURL,RequestType,JobDescription,StartTime,TriggerType,RepeatCount,RepeatInterval,CronExpression,JobStatus,AddTime FROM JobHttpScheduler WHERE JobHttpSchedulerID=@JobHttpSchedulerID";
                return conn.Query<JobHttpScheduler>(sql, new { JobHttpSchedulerID = jobId }).SingleOrDefault<JobHttpScheduler>();
            }
        }
        /// <summary>
        /// 获取单个Job信息
        /// </summary>
        /// <param name="jobId">Job编号</param>
        /// <returns>NULL未获取到Job信息，其他返回相应的Job信息</returns>
        public JobHttpScheduler GetJobInfo(string jobName)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "SELECT TOP 1 JobHttpSchedulerID,JobName,GroupName,RequestURL,RequestType,JobDescription,StartTime,TriggerType,RepeatCount,RepeatInterval,CronExpression,JobStatus,AddTime FROM JobHttpScheduler WHERE JobName=@JobName";
                return conn.Query<JobHttpScheduler>(sql, new { JobName = jobName }).SingleOrDefault<JobHttpScheduler>();
            }
        }


        /// <summary>
        /// 新增一条计划任务记录
        /// </summary>
        /// <param name="item">要新增的计划任务实例</param>
        /// <returns>返回影响行数</returns>
        public int AddJob(JobHttpScheduler item)
        {
            if (item == null)
            {
                return -1;
            }

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO dbo.JobHttpScheduler(JobName, GroupName, RequestURL, RequestType, JobDescription, StartTime, TriggerType, ");
            sql.AppendLine("RepeatCount, RepeatInterval, CronExpression, JobStatus, AddTime) ");
            sql.AppendLine("VALUES(@JobName, @GroupName, @RequestURL, @RequestType, @JobDescription, @StartTime, @TriggerType, ");
            sql.AppendLine("@RepeatCount, @RepeatInterval, @CronExpression, @JobStatus, GETDATE());");

            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql.ToString(),
                    new
                    {
                        JobName = item.JobName,
                        GroupName = item.GroupName,
                        RequestURL = item.RequestURL,
                        RequestType = item.RequestType,
                        JobDescription = item.JobDescription,
                        StartTime = item.StartTime,
                        TriggerType = item.TriggerType,
                        RepeatCount = item.RepeatCount,
                        RepeatInterval = item.RepeatInterval,
                        CronExpression = item.CronExpression,
                        JobStatus = item.JobStatus
                    });
            }
        }

        /// <summary>
        /// 更新一条计划任务记录
        /// </summary>
        /// <param name="item">要更新的计划任务实例</param>
        /// <returns>返回影响行数</returns>
        public int UpdateJob(JobHttpScheduler item)
        {
            if (item == null || item.JobHttpSchedulerID < 1)
            {
                return -1;
            }

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE dbo.JobHttpScheduler SET ");
            sql.AppendLine("JobName = @JobName,");
            sql.AppendLine("GroupName = @GroupName,");
            sql.AppendLine("RequestURL = @RequestURL,");
            sql.AppendLine("RequestType = @RequestType,");
            sql.AppendLine("JobDescription = @JobDescription,");
            sql.AppendLine("StartTime = @StartTime,");
            sql.AppendLine("TriggerType = @TriggerType,");
            sql.AppendLine("RepeatCount = @RepeatCount,");
            sql.AppendLine("RepeatInterval = @RepeatInterval,");
            sql.AppendLine("CronExpression = @CronExpression,");
            sql.AppendLine("JobStatus = @JobStatus ");
            sql.AppendLine("WHERE JobHttpSchedulerID = @JobHttpSchedulerID");

            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql.ToString(),
                    new
                    {
                        JobHttpSchedulerID = item.JobHttpSchedulerID,
                        JobName = item.JobName,
                        GroupName = item.GroupName,
                        RequestURL = item.RequestURL,
                        RequestType = item.RequestType,
                        JobDescription = item.JobDescription,
                        StartTime = item.StartTime,
                        TriggerType = item.TriggerType,
                        RepeatCount = item.RepeatCount,
                        RepeatInterval = item.RepeatInterval,
                        CronExpression = item.CronExpression,
                        JobStatus = item.JobStatus
                    });
            }
        }

        public int DeleteJobList(List<int> ids)
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
                string sql = string.Format("DELETE FROM dbo.JobHttpScheduler WHERE JobHttpSchedulerID IN ({0})", id.Trim(','));
                return conn.Execute(sql);
            }
        }

        public IEnumerable<JobHttpScheduler> GetJobList(JobHttpScheduler searchCondition, PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT JobHttpSchedulerID,JobName,GroupName,RequestURL,RequestType, ");
            sql.AppendLine("JobDescription,StartTime,TriggerType,RepeatCount,RepeatInterval,CronExpression,JobStatus,AddTime ");
            sql.AppendLine("FROM JobHttpScheduler WITH(NOLOCK) WHERE 1=1 ");
            if (searchCondition != null)
            {
                if (searchCondition.JobHttpSchedulerID > 0)
                {
                    sql.AppendLine(" AND JobHttpSchedulerID = " + searchCondition.JobHttpSchedulerID);
                }
                if (!string.IsNullOrWhiteSpace(searchCondition.JobName))
                {
                    sql.AppendLine(" AND JobName LIKE '%" + searchCondition.JobName + "%'");
                }
                if (searchCondition.JobStatus > -1)
                {
                    sql.AppendLine(" AND JobStatus = " + searchCondition.JobStatus);
                }
            }
            return DapperHelper<JobHttpScheduler>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
    }
}
