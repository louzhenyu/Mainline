using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;

namespace JinRi.PolicyJob.ConsoleApplication1
{
    public class SchedulerJob : IJob
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SchedulerJob));

        public SchedulerJob() { }

        public void Execute(IJobExecutionContext context)
        {
            JobInfo jobInfo = context.JobDetail.JobDataMap["JobInfo"] as JobInfo;
            if (jobInfo != null)
            {
                if (string.IsNullOrEmpty(jobInfo.RequestURL))
                {
                    logger.Info(string.Format("Job {0} requestUrl is empty. ", jobInfo.RequestURL));
                    return;
                }
                switch (jobInfo.RequestType)
                {
                    case RequestType.Get:
                        HttpHelper.HttpGet(jobInfo.RequestURL, 30000); break;
                    case RequestType.Post:
                        HttpHelper.HttpPost(jobInfo.RequestURL, "", 30000); break;
                    default: break;
                }
                logger.Info("request " + jobInfo.RequestURL);
            }
            else
            {
                logger.Info("SchedulerJob.jobInfo is null.");
            }
        }
    }
}
