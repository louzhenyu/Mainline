using JinRi.Fx.Entity;
using JinRi.Fx.Logic;
using JinRi.Fx.Utility;
using JinRi.Fx.Web;
using JinRi.Fx.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace JinRi.Fx.WebUI.Controllers.Job
{
    public class JobController : ControllerBaseAdmin
    {
        JobHttpSchedulerLogic logic = new JobHttpSchedulerLogic();
        //
        // GET: /Job/

        [UserAuthentication]
        public ActionResult Index(string jobName = "", int jobStatus = -1, int pageIndex = 1)
        {
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };
            List<JobHttpScheduler> jobList = logic.GetJobPageList(new JobHttpScheduler { JobName = jobName, JobStatus = jobStatus }, pageItem).ToList<JobHttpScheduler>();
            ViewBag.JobList = new PagedList<JobHttpScheduler>(jobList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View();
        }

        //
        // GET: /Job/Create

        [UserAuthentication]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Job/Create

        [HttpPost]
        [UserAuthentication]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                JobHttpScheduler model = new JobHttpScheduler();
                model.JobName = collection["JobName"];
                model.GroupName = string.IsNullOrEmpty(collection["GroupName"]) ? "DefaultGroup" : collection["GroupName"];
                model.RequestURL = collection["RequestURL"];
                model.RequestType = Convert.ToInt32(collection["RequestType"]);
                model.JobDescription = collection["JobDescription"];
                model.StartTime = Convert.ToDateTime(collection["StartTime"]);
                model.TriggerType = Convert.ToInt32(collection["TriggerType"]);
                model.RepeatCount = Convert.ToInt32(collection["RepeatCount"]);
                model.RepeatInterval = Convert.ToInt32(collection["RepeatInterval"]);
                model.CronExpression = collection["CronExpression"].Trim();
                model.JobStatus = Convert.ToInt32(collection["JobStatus"]);
                if (string.IsNullOrEmpty(model.JobName))
                {
                    return this.Back("请输入任务名称。");
                }
                if (model.TriggerType == 1 && string.IsNullOrEmpty(model.CronExpression))
                {
                    return this.Back("请输入正确的Cron-Like表达式。");
                }
                if (model.TriggerType == 0 && model.RepeatInterval <= 0)
                {
                    return this.Back("执行计划间隔时间输入错误。");
                }
                if (logic.GetJobInfo(model.JobName) == null)
                {
                    logic.AddJob(model);
                    return this.RefreshParent();
                }
                else
                {
                    return this.Back("执行计划名称重复。");
                }
            }
            catch (Exception ex)
            {
                return this.Back("新增Job发生异常。" + ex.Message);
            }
        }

        //
        // GET: /Job/Edit/5

        [UserAuthentication]
        public ActionResult Edit(int id)
        {
            var model = logic.GetJobInfo(id);
            if (model == null)
            {
                return this.Back("数据异常。");
            }
            return View(model);
        }

        //
        // POST: /Job/Edit/5

        [HttpPost]
        [UserAuthentication]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                JobHttpScheduler model = new JobHttpScheduler();
                model.JobHttpSchedulerID = id;
                model.JobName = collection["JobName"];
                model.GroupName = collection["GroupName"];
                model.RequestURL = collection["RequestURL"];
                model.RequestType = Convert.ToInt32(collection["RequestType"]);
                model.JobDescription = collection["JobDescription"] ?? "";
                model.StartTime = Convert.ToDateTime(collection["StartTime"]);
                model.TriggerType = Convert.ToInt32(collection["TriggerType"]);
                model.RepeatCount = Convert.ToInt32(collection["RepeatCount"]);
                model.RepeatInterval = Convert.ToInt32(collection["RepeatInterval"]);
                model.CronExpression = collection["CronExpression"].Trim();
                model.JobStatus = Convert.ToInt32(collection["JobStatus"]);
                if (model.TriggerType == 1 && string.IsNullOrEmpty(model.CronExpression))
                {
                    return this.Back("请输入正确的Cron-Like表达式。");
                }
                if (model.TriggerType == 0 && model.RepeatInterval <= 0)
                {
                    return this.Back("执行计划间隔时间输入错误。");
                }
                logic.UpdateJob(model);
                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                return this.Back("修改任务发生异常。" + ex.Message);
            }
        }
        //
        // POST: /Job/Delete/

        [HttpPost]
        [UserAuthentication]
        public ActionResult Delete(List<int> ids)
        {
            logic.DeleteJobList(ids);
            return RedirectToAction("Index");
        }

        public ActionResult Log(int id, string startTime, string endTime, int pageIndex = 1)
        {
            JobLogModel model = new JobLogModel() { JobId = id };
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                model.StartTime = DateTime.Parse(startTime);
                model.EndTime = DateTime.Parse(endTime);
            }
            JobExecuteLogLogic logLogic = new JobExecuteLogLogic();
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };
            List<JobExecuteLog> list = logLogic.GetJobExecuteLogList(model.JobId, model.StartTime, model.EndTime, pageItem).ToList<JobExecuteLog>();
            PagedList<JobExecuteLog> pageList = new PagedList<JobExecuteLog>(list, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            ViewBag.DataSource = new PagedList<JobExecuteLog>(list, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View(model);
        }
    }
}
