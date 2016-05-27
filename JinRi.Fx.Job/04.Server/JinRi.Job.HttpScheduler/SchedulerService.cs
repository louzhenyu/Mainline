using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using JinRi.Job.HttpScheduler.Utils;
using Quartz;
using Quartz.Impl;

namespace JinRi.Job.HttpScheduler
{
    public partial class SchedulerService : ServiceBase
    {
        #region Field
        IScheduler scheduler;
        bool isStart = false;
        object syncRoot = new object();
        System.Threading.Timer refreshTimer;
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SchedulerService));

        /// <summary>
        /// 所有配置的任务列表
        /// </summary>
        List<JobInfo> jobList = new List<JobInfo>();
        #endregion

        #region Event
        protected override void OnStart(string[] args)
        {
            try
            {
                isStart = true;
                logger.Info("Quartz服务启动中...");
                foreach (JobInfo job in jobList)
                {
                    logger.Info(string.Format("{0} TriggerType:{1} StartTime:{2} CronExpression:[{3}] RepeatCount:{4} RepeatInterval:{5} URL:{6}",
                        job.Name, job.TriggerType.ToString(), job.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        job.CronExpression, job.RepeatCount, job.RepeatInterval, job.RequestURL));
                }
                scheduler.Start();
                logger.Info("Quartz服务成功启动");
            }
            catch (Exception ex)
            {
                logger.Error("服务启动报错：" + ex.ToString());
                throw;
            }
        }
        protected override void OnStop()
        {
            isStart = false;
            scheduler.Shutdown(false);
            logger.Info("Quartz服务成功终止");
        }
        protected override void OnPause()
        {
            isStart = false;
            scheduler.PauseAll();
        }
        protected override void OnContinue()
        {
            isStart = true;
            scheduler.ResumeAll();
        }
        #endregion

        #region Method
        public SchedulerService()
        {
            InitializeComponent();
            try
            {
                if (!ConfigManager.UseLocalSwitch)
                {
                    ZKManager.Client.Init();
                }
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                scheduler = schedulerFactory.GetScheduler();
                //获取Job数据
                RefreshJobList(scheduler);

                refreshTimer = new System.Threading.Timer(delegate
                {
                    if (isStart) { RefreshJobList(scheduler); }
                }, null, ConfigManager.RefreshTime, ConfigManager.RefreshTime);
            }
            catch (Exception ex)
            {
                logger.Error("scheduler初始化报错：" + ex.ToString());
                throw;
            }
        }
        /// <summary>
        /// 刷新Job列表
        /// </summary>
        /// <param name="sched"></param>
        private void RefreshJobList(IScheduler sched)
        {
            try
            {
                lock (syncRoot)
                {
                    logger.Info("更新Job列表...");
                    jobList = JobManager.Instance().GetJobInfoList();
                    sched.Clear();
                    foreach (JobInfo jobInfo in jobList)
                    {
                        try
                        {
                            JobDataMap dataMap = new JobDataMap();
                            dataMap.Add("JobInfo", jobInfo);
                            IJobDetail job = JobBuilder.Create<JinRi.Job.HttpScheduler.SchedulerJob>().SetJobData(dataMap).WithIdentity(jobInfo.Name, "Job_" + jobInfo.GroupName).WithDescription(jobInfo.JobDescription).Build();

                            ITrigger trigger = null;
                            switch (jobInfo.TriggerType)
                            {
                                case JobInfoTriggerType.SimpleTrigger:
                                    trigger = (ISimpleTrigger)TriggerBuilder.Create().WithIdentity("Trigger_" + jobInfo.Name, "Trigger_" + jobInfo.GroupName)
                                        .StartAt(new DateTimeOffset(jobInfo.StartTime)).WithSimpleSchedule(x => x.WithIntervalInMinutes(jobInfo.RepeatInterval).WithRepeatCount(jobInfo.RepeatCount)).Build();
                                    break;
                                case JobInfoTriggerType.CronTrigger:
                                    trigger = (ICronTrigger)TriggerBuilder.Create().WithIdentity("Trigger_" + jobInfo.Name, "Trigger_" + jobInfo.GroupName)
                                        .WithCronSchedule(jobInfo.CronExpression).Build();
                                    break;
                                default:
                                    break;
                            }
                            if (trigger != null)
                            {
                                sched.ScheduleJob(job, trigger);
                            }
                            else
                            {
                                logger.Info(string.Format("{0} trigger is null.", jobInfo.Name));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(string.Format("加载JOB信息报错{0}：{1}", jobInfo.Name, ex.ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("SchedulerService.LoadJobList." + ex.ToString());
            }
        }
        #endregion
    }
}
