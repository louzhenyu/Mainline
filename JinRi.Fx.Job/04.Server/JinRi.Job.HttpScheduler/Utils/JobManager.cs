using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JinRi.Job.HttpScheduler.Utils
{
    public class JobManager
    {
        private static JobManager m_instance = null;
        private JobManager() { }

        public static JobManager Instance()
        {
            if (m_instance == null)
            {
                m_instance = new JobManager();
            }
            return m_instance;
        }

        public List<JobInfo> GetJobInfoList()
        {
            List<JobInfo> returnValue = new List<JobInfo>();
            string commandText = "SELECT JobHttpSchedulerID,JobName,GroupName,RequestURL,RequestType,StartTime,JobDescription,TriggerType,RepeatCount,RepeatInterval,CronExpression FROM JobHttpScheduler WITH(NOLOCK) WHERE JobStatus=0 ";
            DataSet ds = SqlHelper.ExecuteDataSet(SqlHelper.FxDBConnectionStr, System.Data.CommandType.Text, commandText, null);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
            {
                DataRow row = null;
                for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                {
                    row = ds.Tables[0].Rows[index];
                    JobInfo job = new JobInfo();
                    job.JobHttpSchedulerID = Convert.ToInt32(row["JobHttpSchedulerID"].ToString());
                    job.Name = row["JobName"].ToString();
                    job.GroupName = row["GroupName"].ToString();
                    job.RequestURL = row["RequestURL"].ToString();
                    job.RequestType = row["RequestType"].ToString() == "0" ? RequestType.Get : RequestType.Post;
                    job.JobDescription = row["JobDescription"].ToString();
                    job.StartTime = Convert.ToDateTime(row["StartTime"].ToString());
                    job.TriggerType = row["TriggerType"].ToString() == "0" ? JobInfoTriggerType.SimpleTrigger : JobInfoTriggerType.CronTrigger;
                    job.RepeatCount = Convert.ToInt32(row["RepeatCount"].ToString());
                    job.RepeatInterval = Convert.ToInt32(row["RepeatInterval"].ToString());
                    job.CronExpression = row["CronExpression"].ToString();
                    returnValue.Add(job);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 记录JOB运行日志
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="executeTime">执行时间</param>
        /// <param name="success">是否成功</param>
        /// <returns></returns>
        public int WriteExecuteLog(int jobId, DateTime executeTime, bool success)
        {
            int result = 0;
            try
            {
                string sql = string.Format("INSERT INTO JobExecuteLog(JobId,ExecuteTime,Success)VALUES({0},'{1}',{2})", jobId, executeTime, success ? 0 : 1);
                result = SqlHelper.ExecuteNonQuery(SqlHelper.FxDBConnectionStr, System.Data.CommandType.Text, sql, null);
            }
            catch (Exception ex) { }
            return result;
        }
    }
}
