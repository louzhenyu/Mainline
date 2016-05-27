using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace JinRi.Fx.Entity
{
    public class CommonMetric
    {
        public string MetricName { get; set; }

        public string AppID { get; set; }

        public string HostIP { get; set; }

        public string SeriesName { get; set; }

        public string ContextName { get; set; }  

        public string MetricUnit { get; set; }

        private DateTime addTime = DateTime.MinValue;   
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

        public DateTime XAxisValue { get; set; }

        public long YAxisValue { get; set; }
    }
}
