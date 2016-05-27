using JinRi.Fx.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JinRi.Fx.Utility;

namespace JinRi.Fx.RequestDTO
{
    [Serializable]
    public class MetricSearchRequestDTO
    {
        public string MetricName { get; set; }

        public int AppID { get; set; }

        public string HostIP { get; set; }

        private DateTime startTime = DateTime.Now.AddHours(-1);      
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

        private DateTime endTime = DateTime.MinValue;     
        public DateTime EndTime
        {
            get
            {
                if (endTime.Date == DateTime.MinValue.Date)
                {
                    return startTime.AddMinutes(interval);
                }

                return endTime;
            }
            set
            {
                endTime = value;
            }
        }

        private int interval = 5;
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

        private IntervalUnit intervalUnit = IntervalUnit.Minute;
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
    }
}
