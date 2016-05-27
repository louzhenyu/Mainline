using System;
using System.Diagnostics;
using Metrics;

namespace JetermClient.Common
{
    public static class JMetricsHelper
    {
        #region ModuleName
        /// <summary>
        /// 统计访问次数
        /// </summary>
        public static string JetermCount = "Fx.Jeterm.{0}Count";

        /// <summary>
        /// 统计错误次数
        /// </summary>
        public static string JetermErrCount = "Fx.Jeterm.{0}ErrCount";

        /// <summary>
        /// 统计执行指令耗时
        /// </summary>
        public static string JetermExecTime = "Fx.Jeterm.{0}Time";

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="unitName"></param>
        /// <returns></returns>
        private static Meter GetMeter(this string moduleName, string unitName = "单")
        {
            try
            {
                return JMetric.Meter(moduleName, Unit.Custom(unitName));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="unitName"></param>
        /// <returns></returns>
        private static Histogram GetHistogram(this string moduleName, string unitName)
        {
            try
            {
                return JMetric.Histogram(moduleName, Unit.Custom(unitName));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="unitName"></param>
        public static void MeterMark(this string moduleName, string unitName)
        {
            Meter meter = moduleName.GetMeter(unitName);
            if (meter != null)
            {
                try
                {
                    meter.Mark();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="execFunc"></param>
        public static T HistogramUpdate<T>(this string moduleName, Func<T> execFunc)
        {
            Histogram histogram = moduleName.GetHistogram("ms");
            if (histogram != null)
            {
                try
                {
                    return histogram.HistogramUpdate(execFunc);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }                
            }
            return execFunc();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="histogram"></param>
        /// <param name="execFunc"></param>
        private static T HistogramUpdate<T>(this Histogram histogram, Func<T> execFunc)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                return execFunc();
            }
            finally
            {
                stopWatch.Stop();
                histogram.Update(stopWatch.ElapsedMilliseconds);
            }
        }
    }
}
