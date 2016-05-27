using Metrics.Sampling;
using Metrics.Utils;

namespace Metrics
{
    /// <summary>
    /// A Histogram measures the distribution of values in a stream of data: e.g., the number of results returned by a search.
    /// 最后修改：RANEN.TONG 20141215 注释掉了userValue相关的方法
    /// </summary>
    public interface Histogram : ResetableMetric, Utils.IHideObjectMembers
    {
        /// <summary>
        /// Records a value.
        /// </summary>
        /// <param name="value">Value to be added to the histogram.</param>
        /// <param name="userValue">A custom user value that will be associated to the results.
        /// Useful for tracking (for example) for which id the max or min value was recorded.
        /// </param>
        void Update(long value);
    }
}
