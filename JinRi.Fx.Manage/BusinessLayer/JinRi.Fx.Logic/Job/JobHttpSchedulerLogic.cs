using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Logic
{
    public class JobHttpSchedulerLogic
    {
        JobHttpSchedulerDal dal = new JobHttpSchedulerDal();
        /// <summary>
        /// 获取单个Job信息
        /// </summary>
        /// <param name="jobId">编号</param>
        /// <returns>NULL未获取到Job信息，其他返回相应的Job信息</returns>
        public JobHttpScheduler GetJobInfo(int jobId)
        {
            return dal.GetJobInfo(jobId);
        }
        /// <summary>
        /// 获取单个Job信息
        /// </summary>
        /// <param name="jobName">名称</param>
        /// <returns>NULL未获取到Job信息，其他返回相应的Job信息</returns>
        public JobHttpScheduler GetJobInfo(string jobName)
        {
            return dal.GetJobInfo(jobName);
        }


        /// <summary>
        /// 新增一条计划任务记录
        /// </summary>
        /// <param name="item">要新增的计划任务实例</param>
        /// <returns>返回影响行数</returns>
        public int AddJob(JobHttpScheduler item)
        {
            return dal.AddJob(item);
        }

        /// <summary>
        /// 更新一条计划任务记录
        /// </summary>
        /// <param name="item">要更新的计划任务实例</param>
        /// <returns>返回影响行数</returns>
        public int UpdateJob(JobHttpScheduler item)
        {
            return dal.UpdateJob(item);
        }

        public int DeleteJobList(List<int> ids)
        {
            return dal.DeleteJobList(ids);
        }

        public IEnumerable<JobHttpScheduler> GetJobPageList(JobHttpScheduler searchCondition, PageItem pageItem)
        {
            return dal.GetJobList(searchCondition, pageItem);
        }
    }
}
