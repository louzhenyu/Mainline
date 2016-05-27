using System;
using Metrics.MetricData;
using Metrics.Sampling;
using Metrics.Logging;

namespace Metrics.Core
{
    public interface HistogramImplementation : Histogram, MetricValueProvider<HistogramValue> { }

    public sealed class HistogramMetric : HistogramImplementation
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();
        private readonly Reservoir reservoir;
        private UserValueWrapper last = new UserValueWrapper();

        public HistogramMetric()
            : this(new ExponentiallyDecayingReservoir()) { }

        public HistogramMetric(SamplingType samplingType)
            : this(SamplingTypeToReservoir(samplingType)) { }

        public HistogramMetric(Reservoir reservoir)
        {
            this.reservoir = reservoir;
        }
        /// <summary>
        /// 最后修改：RANEN.TONG 20141215 注释掉了userValue相关的方法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="userValue"></param>
        public void Update(long value)
        {
            try
            {
                string userValue = null;
                this.last = new UserValueWrapper(value, userValue);
                this.reservoir.Update(value, userValue);
            }
            catch (Exception ex)
            {
                log.ErrorException(ex.Message, ex);
            }
        }

        public HistogramValue GetValue(bool resetMetric = false)
        {
            var value = new HistogramValue(this.last.Value, this.last.UserValue, this.reservoir.GetSnapshot(resetMetric));
            if (resetMetric)
            {
                this.last = new UserValueWrapper();
            }
            return value;
        }

        public HistogramValue Value
        {
            get
            {
                return GetValue();
            }
        }

        public void Reset()
        {
            this.last = new UserValueWrapper();
            this.reservoir.Reset();
        }

        private static Reservoir SamplingTypeToReservoir(SamplingType samplingType)
        {
            switch (samplingType)
            {
                case SamplingType.FavourRecent: return new ExponentiallyDecayingReservoir();
                case SamplingType.LongTerm: return new UniformReservoir();
                case SamplingType.SlidingWindow: return new SlidingWindowReservoir();
            }
            throw new InvalidOperationException("Sampling type not implemented " + samplingType.ToString());
        }
    }
}
