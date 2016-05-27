using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JinRi.Fx.WebUI.Models
{
    public partial class MetricSearchArgs
    {
        private MetricType metricType = MetricType.Meters;
        public MetricType MetricType
        {
            get
            {
                return metricType;
            }
            set
            {
                metricType = value;
            }
        }

        public string MetricName { get; set; }
                
        public int AppID { get; set; }

        public string AppIDString
        {
            get
            {
                if (AppID < 1)
                {
                    return string.Empty;
                }

                return AppID.ToString();
            }
        }

        public string HostIP { get; set; }

        private DateTime startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
        [DataType(DataType.DateTime)]
        [Display(Name = "开始时间")]
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

        private DateTime endTime =DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
        [DataType(DataType.DateTime)]
        [Display(Name = "结束时间")]
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
            }
        }

        private int interval = 1;
        public int Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
            }
        }

        private IntervalUnit intervalUnit = IntervalUnit.Hour;
        public IntervalUnit IntervalUnit
        {
            get
            {
                return intervalUnit;
            }
            set
            {
                intervalUnit = value;
            }
        }

        private AggregationWay aggregationWay = AggregationWay.SUM;
        public AggregationWay AggregationWay
        {
            get
            {
                return aggregationWay;
            }
            set
            {
                aggregationWay = value;
            }
        }

        private GroupBy groupBy = GroupBy.NotSet;
        public GroupBy GroupBy
        {
            get
            {
                return groupBy;
            }
            set
            {
                groupBy = value;
            }
        }
        private DateTime compareStartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00"));
        [DataType(DataType.DateTime)]
        [Display(Name = "对比开始时间")]
        public DateTime CompareStartTime
        {
            get
            {
                return compareStartTime;
            }
            set
            {
                compareStartTime = value;
            }
        }

        private DateTime compareEndTime = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00"));
        [DataType(DataType.DateTime)]
        [Display(Name = "对比结束时间")]
        public DateTime CompareEndTime
        {
            get
            {
                return compareEndTime;
            }
            set
            {
                compareEndTime = value;
            }
        }
    }
}