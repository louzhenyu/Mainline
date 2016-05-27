using Metrics;
using System.Collections.Generic;

namespace Flight.Product.Utility
{
    /// <summary>
    /// 统一度量器管理类
    /// </summary>
    public class MetricsManager
    {
        static object lockHistogram = new object();
        static object lockMeter = new object();

        static Dictionary<string, Histogram> histogramDictionary = new Dictionary<string, Histogram>();
        static Dictionary<string, Meter> meterDictionary = new Dictionary<string, Meter>();
        /// <summary>
        /// 统计单位时间内某一组数据的各项指标，包括：MAX、MIN、AVG、MEAN、SUM、COUNT
        /// </summary>
        /// <param name="name">Metrics Key</param>
        /// <param name="value">数值</param>
        /// <param name="unit">单位</param>
        public static void HistogramUpdate(string name, int value, Unit unit)
        {
            if (!histogramDictionary.ContainsKey(name))
            {
                lock (lockHistogram)
                {
                    if (!histogramDictionary.ContainsKey(name))
                    {
                        histogramDictionary.Add(name, JMetric.Histogram(name, unit));
                    }
                }
            }
            histogramDictionary[name].Update(value);
        }
        /// <summary>
        /// 统计单位时间内处理的Request数
        /// </summary>
        /// <param name="name">Metrics Key</param>
        /// <param name="unit">单位</param>
        public static void MeterMark(string name, Unit unit)
        {
            if (!meterDictionary.ContainsKey(name))
            {
                lock (lockMeter)
                {
                    if (!meterDictionary.ContainsKey(name))
                    {
                        meterDictionary.Add(name, JMetric.Meter(name, unit));
                    }
                }
            }
            meterDictionary[name].Mark();
        }
    }
}
