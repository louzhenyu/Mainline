using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 计划任务实体类
    /// </summary>
    public class JobHttpScheduler
    {

        private int jobHttpSchedulerID = -1;
        private string jobName = "";
        private string groupName = "DefaultGroup";
        private string requestURL = "";
        private int requestType = 0;
        private string jobDescription = "";
        private DateTime startTime = DateTime.Now;
        private int triggerType = 0;
        private int repeatCount = -1;
        private int repeatInterval = 1;
        private string cronExpression = "-";
        private int jobStatus = 0;
        private DateTime addTime = DateTime.Now;

        /// <summary>
        /// JobId
        /// </summary>
        public int JobHttpSchedulerID
        {
            get
            {
                return jobHttpSchedulerID;
            }
            set
            {
                jobHttpSchedulerID = value;
            }
        }
        /// <summary>
        /// 名称(唯一)
        /// </summary>
        [Display(Name="任务名称")]
        [Required(ErrorMessage = "任务名称不能为空")]
        public string JobName
        {
            get
            {
                return jobName;
            }
            set
            {
                jobName = value;
            }
        }
        /// <summary>
        /// 组名
        /// </summary>
        [Display(Name = "组名")]
        [Required(ErrorMessage = "组名不能为空")]
        public string GroupName
        {
            get
            {
                return groupName;
            }
            set
            {
                groupName = value;
            }
        }
        /// <summary>
        /// 请求地址
        /// </summary>
        [StringLength(150, MinimumLength = 4,ErrorMessage="请求地址长度不符")]　
        [Required(ErrorMessage = "请求地址不能为空")]
        [Display(Name = "请求地址")]
        public string RequestURL
        {
            get
            {
                return requestURL;
            }
            set
            {
                requestURL = value;
            }
        }
        /// <summary>
        /// 请求类型(0:GET,1:POST)
        /// </summary>
        public int RequestType
        {
            get
            {
                return requestType;
            }
            set
            {
                requestType = value;
            }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string JobDescription
        {
            get
            {
                return jobDescription;
            }
            set
            {
                jobDescription = value;
            }
        }
        /// <summary>
        /// 启动时间
        /// </summary>
        [DataType(DataType.DateTime)]
        [Display(Name = "任务启动时间")]
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
            }
        }
        /// <summary>
        /// Trigger类型，0：SimpleTrigger，1：CronTrigger
        /// </summary>
        public int TriggerType
        {
            get
            {
                return triggerType;
            }
            set
            {
                triggerType = value;
            }
        }
        /// <summary>
        /// 重复次数(-1表示不限制)
        /// </summary>
        public int RepeatCount
        {
            get
            {
                return repeatCount;
            }
            set
            {
                repeatCount = value;
            }
        }
        /// <summary>
        /// 间隔时间，单位分钟
        /// </summary>
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "间隔时间格式错误，必须为正整数")]
        public int RepeatInterval
        {
            get
            {
                return repeatInterval;
            }
            set
            {
                repeatInterval = value;
            }
        }
        /// <summary>
        /// Cron-Like 表达式
        /// </summary>
        public string CronExpression
        {
            get
            {
                return cronExpression;
            }
            set
            {
                cronExpression = value;
            }
        }
        /// <summary>
        /// 状态(0开启，1暂停)
        /// </summary>
        public int JobStatus
        {
            get
            {
                return jobStatus;
            }
            set
            {
                jobStatus = value;
            }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get
            {
                return addTime;
            }
            set
            {
                addTime = value;
            }
        }
    }
}
