using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    public class MetricHistogram : CommonMetric
    {
        public int HistogramID { get; set; }
        //public int AppId { get; set; }
        //public string HostIP { get; set; }
        //public string Name { get; set; }
        //public string ContextName { get; set; }
        public long ValueCount { get; set; }
        public long ValueSum { get; set; }
        public double ValueAvg { get; set; }
        public long ValueMin { get; set; }
        public long ValueMax { get; set; }
        public double ValueMedian { get; set; }
        //public string HistogramUnit { get; set; }
        //public DateTime AddTime { get; set; }

        //public string LineName { get; set; }
    }
}
