using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using Quartz;
using Quartz.Impl;

namespace JinRi.PolicyJob.WindowsService
{
    partial class PolicySyncService : ServiceBase
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(PolicySyncService));
        IScheduler scheduler;
        public PolicySyncService()
        {
            InitializeComponent();
            try
            {
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                scheduler = schedulerFactory.GetScheduler();
            }
            catch (Exception ex)
            {
                logger.Error("scheduler初始化报错：" + ex.ToString());
                throw;
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                logger.Info("Quartz服务启动中...");
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
            scheduler.Shutdown(false);
            logger.Info("Quartz服务成功终止");
        }
        protected override void OnPause()
        {
            scheduler.PauseAll();
        }

        protected override void OnContinue()
        {
            scheduler.ResumeAll();
        }
    }
}
