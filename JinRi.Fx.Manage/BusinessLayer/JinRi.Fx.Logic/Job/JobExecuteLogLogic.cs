using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using JinRi.Fx.Entity;
using JinRi.Fx.Data;
using JinRi.Fx.Utility;
using System;

namespace JinRi.Fx.Logic
{
    /// <summary>
    /// Job运行日志
    /// <para>编码：RANEN.TONG 2015-12-10</para>
    /// </summary>
    public class JobExecuteLogLogic
    {
        // 数据访问对象
        JobExecuteLogDAL jobExecuteLogDAL = new JobExecuteLogDAL();

        /// <summary>
        /// 获取Job运行日志列表
        /// </summary>
        public IEnumerable<JobExecuteLog> GetJobExecuteLogList(int jobId, DateTime startTime, DateTime endTime, PageItem pageItem)
        {
            return jobExecuteLogDAL.GetJobExecuteLogList(jobId, startTime, endTime, pageItem);
        }

        /// <summary>
        /// 获取Job运行日志信息
        /// </summary>
        /// <param name="iD">编号</param>
        /// <returns>Job运行日志实体</returns>
        public JobExecuteLog GetJobExecuteLog(int iD)
        {
            return jobExecuteLogDAL.GetJobExecuteLog(iD);
        }

        /// <summary>
        /// 修改Job运行日志信息
        /// </summary>
        public int UpdateJobExecuteLog(JobExecuteLog model)
        {
            return jobExecuteLogDAL.UpdateJobExecuteLog(model);
        }

        /// <summary>
        /// 添加Job运行日志信息
        /// </summary>
        public int AddJobExecuteLog(JobExecuteLog model)
        {
            return jobExecuteLogDAL.AddJobExecuteLog(model);
        }

        /// <summary>
        /// 删除Job运行日志信息
        /// </summary>
        /// <param name="iD">编号</param>
        /// <returns>受影响的行数</returns>
        public int DeleteJobExecuteLog(int iD)
        {
            return jobExecuteLogDAL.DeleteJobExecuteLog(iD);
        }

        /// <summary>
        /// 删除Job运行日志信息
        /// </summary>
        /// <param name="iD">编号</param>
        /// <returns>受影响的行数</returns>
        public int DeleteJobExecuteLog(List<int> listId)
        {
            return jobExecuteLogDAL.DeleteJobExecuteLog(listId);
        }
    }
}
