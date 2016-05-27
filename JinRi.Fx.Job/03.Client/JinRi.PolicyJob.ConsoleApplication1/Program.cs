using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Quartz;
using Quartz.Impl;

namespace JinRi.PolicyJob.ConsoleApplication1
{
    class Program
    {
        private static object syncRoot = new object();
        private static List<JobInfo> JobList = new List<JobInfo>();
        private static IScheduler scheduler;

        static void Main(string[] args)
        {
            #region 方式一   通过代码实现
            //初始化调度器工厂
            ISchedulerFactory schedFactory = new StdSchedulerFactory();
            //创建调度器
            IScheduler sched = schedFactory.GetScheduler();
            //创建任务
            IJobDetail job = JobBuilder.Create<JinRi.Policy.Jobs.PolicySyncJob>().WithIdentity("PolicySyncJob", "PolicyGroup").WithDescription("描述").Build();

            //创建任务触发器
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create().WithIdentity("PolicySyncTrigger", "PolicyTriggerGroup").WithCronSchedule("0/5 * * * * ?").Build();
            sched.ScheduleJob(job, trigger);
            sched.Start();
            #endregion

            #region 方式二   Job、Trigger通过配置文件配置，系统配置通过程序设置
            //NameValueCollection properties = new NameValueCollection();
            //properties["quartz.scheduler.instanceName"] = "XmlConfiguredInstance";

            //// set thread pool info
            //properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //properties["quartz.threadPool.threadCount"] = "5";
            //properties["quartz.threadPool.threadPriority"] = "Normal";

            //// job initialization plugin handles our xml reading, without it defaults are used
            //properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
            //properties["quartz.plugin.xml.fileNames"] = "Config/quartz_jobs.xml";

            //ISchedulerFactory schedulerFactory = new StdSchedulerFactory(properties);
            //IScheduler scheduler = schedulerFactory.GetScheduler();
            //scheduler.Start();
            #endregion

            #region 方式三   通过配置文件读取
            //ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            //IScheduler scheduler = schedulerFactory.GetScheduler();
            //scheduler.Start();
            #endregion

            Console.WriteLine(string.Format("[{0}] Job启动完成。", DateTime.Now.ToString("HH:mm:ss")));
            Console.ReadKey();
        }

        private static void RefreshJobList(IScheduler sched)
        {
            Console.WriteLine("重新加载Job...");
            lock (syncRoot)
            {
                sched.Clear();
                JobList.Clear();
                JobList.Add(new JobInfo { Name = "PolicySyncPage", RequestType = RequestType.Get, CronExpression = "5/10 * * * * ?", RequestURL = "http://127.0.0.1:5080/PolicySyncPage.aspx" });
                JobList.Add(new JobInfo { Name = "WebForm1", RequestType = RequestType.Get, CronExpression = "0/10 * * * * ?", RequestURL = "http://127.0.0.1:5080/WebForm1.aspx" });

                foreach (JobInfo jobInfo in JobList)
                {
                    JobDataMap dataMap = new JobDataMap();
                    dataMap.Add("JobInfo", jobInfo);
                    IJobDetail job = JobBuilder.Create<JinRi.PolicyJob.ConsoleApplication1.SchedulerJob>().SetJobData(dataMap).WithIdentity(jobInfo.Name, "Job_" + jobInfo.GroupName).WithDescription(jobInfo.Description).Build();
                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create().WithIdentity("Trigger_" + jobInfo.Name, "Trigger_" + jobInfo.GroupName).WithCronSchedule(jobInfo.CronExpression).Build();
                    sched.ScheduleJob(job, trigger);
                }
            }
        }
    }
}
