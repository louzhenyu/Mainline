using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    public class MetricMeter : CommonMetric
    {
        public int MeterID { get; set; }

        public long YAxisValueForSUM { get; set; }

        public long YAxisValueForCOUNT { get; set; }       

        public double YAxisValueForAVG { get; set; }

        public long YAxisValueForMIN { get; set; }

        public long YAxisValueForMAX { get; set; }
    }
}
