using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Job.HttpScheduler
{
    /// <summary>
    /// 自定义Job实体
    /// </summary>
    public class JobInfo
    {
        private int jobHttpSchedulerID;

        public int JobHttpSchedulerID
        {
            get { return jobHttpSchedulerID; }
            set { jobHttpSchedulerID = value; }
        }
        private string name = string.Empty;
        /// <summary>
        /// Job名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string groupName = "DefaultGroup";

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        private string requestURL;
        /// <summary>
        /// 请求地址，完整的URL，例如：http://www.jinri.cn/Example.aspx
        /// </summary>
        public string RequestURL
        {
            get { return requestURL; }
            set { requestURL = value; }
        }

        private RequestType requestType = RequestType.Get;
        /// <summary>
        /// 请求类型
        /// </summary>
        public RequestType RequestType
        {
            get { return requestType; }
            set { requestType = value; }
        }

        private string jobDescription = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public string JobDescription
        {
            get { return jobDescription; }
            set { jobDescription = value; }
        }

        private JobInfoTriggerType triggerType= JobInfoTriggerType.SimpleTrigger;
        /// <summary>
        /// Trigger类型，0：SimpleTrigger，1：CronTrigger
        /// </summary>
        public JobInfoTriggerType TriggerType
        {
            get { return triggerType; }
            set { triggerType = value; }
        }

        private DateTime startTime = DateTime.Now;
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private int repeatCount = -1;
        /// <summary>
        /// SimpleTrigger重复执行次数，-1表示无限次，其他正整数表示具体重复的次数
        /// </summary>
        public int RepeatCount
        {
            get { return repeatCount; }
            set { repeatCount = value; }
        }
        private int repeatInterval = 0;
        /// <summary>
        /// SimpleTrigger重复执行间隔时间，单位：分钟
        /// </summary>
        public int RepeatInterval
        {
            get { return repeatInterval; }
            set { repeatInterval = value; }
        }

        private string cronExpression;
        /// <summary>
        /// 执行频率：Cron表达式
        /// </summary>
        public string CronExpression
        {
            get { return cronExpression; }
            set { cronExpression = value; }
        }
        private int jobStatus = 0;
        /// <summary>
        /// 0:开启，1：暂停
        /// </summary>
        public int JobStatus
        {
            get { return jobStatus; }
            set { jobStatus = value; }
        }

        private DateTime addTime = DateTime.Parse("1900-01-01");

        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
    }

    public enum RequestType
    {
        Get,
        Post
    }
    public enum JobInfoTriggerType
    {
        SimpleTrigger = 0,
        CronTrigger = 1
    }
}
