using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metrics;

namespace MetricsSamples
{
    /// <summary>
    /// 统一度量器管理类
    /// </summary>
    public class MetricsManager
    {
        static object lockHistogram = new object();
        static object lockMeter = new object();

        static Dictionary<string, Histogram> dictionaryHistogram = new Dictionary<string, Histogram>();
        static Dictionary<string, Meter> dictionaryMeter = new Dictionary<string, Meter>();
        public static void HistogramUpdate(string name, long num, Unit unit)
        {
            if (!dictionaryHistogram.ContainsKey(name))
            {
                lock (lockHistogram)
                {
                    if (!dictionaryHistogram.ContainsKey(name))
                    {
                        dictionaryHistogram.Add(name, JMetric.Histogram(name, unit));
                    }
                }
            }
            dictionaryHistogram[name].Update(num);
        }
        public static void MeterMark(string name, Unit unit)
        {
            if (!dictionaryMeter.ContainsKey(name))
            {
                lock (lockMeter)
                {
                    if (!dictionaryMeter.ContainsKey(name))
                    {
                        dictionaryMeter.Add(name, JMetric.Meter(name, unit));
                    }
                }
            }
            dictionaryMeter[name].Mark();
        }
    }
}
