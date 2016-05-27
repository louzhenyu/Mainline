using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Job.HttpScheduler.Utils;
using Quartz;

namespace JinRi.Job.HttpScheduler
{
    public class SchedulerJob : IJob
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SchedulerJob));

        public SchedulerJob() { }

        public void Execute(IJobExecutionContext context)
        {
            if (!ConfigManager.UseLocalSwitch)
            {
                if (!ZKManager.Client.IsMaster)
                {
                    return;
                }
            }
            JobInfo jobInfo = context.JobDetail.JobDataMap["JobInfo"] as JobInfo;
            if (jobInfo != null)
            {
                if (string.IsNullOrEmpty(jobInfo.RequestURL))
                {
                    logger.Info(string.Format("Job {0} requestUrl is empty. ", jobInfo.Name));
                    return;
                }
                bool success = false;
                DateTime executeTime = DateTime.Now;
                try
                {
                    switch (jobInfo.RequestType)
                    {
                        case RequestType.Get:
                            HttpHelper.HttpGet(jobInfo.RequestURL, 30000); break;
                        case RequestType.Post:
                            HttpHelper.HttpPost(jobInfo.RequestURL, "", 30000); break;
                        default: break;
                    }
                    logger.Info(string.Format("JobName:{0} request {1} complete.", jobInfo.Name, jobInfo.RequestURL));
                    success = true;
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format("JobName:{0} URL:{1} exception.\r\n{2}", jobInfo.Name, jobInfo.RequestURL, ex.ToString()));
                }
                JobManager.Instance().WriteExecuteLog(jobInfo.JobHttpSchedulerID, executeTime, success);
            }
            else
            {
                logger.Info("SchedulerJob.jobInfo is null.");
            }
        }
    }
}
