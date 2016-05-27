using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metrics
{
    public static class JMetric
    {
        static JMetric()
        {
            int configMinutes = 1;
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Metrics.ServiceReport.TimeSpan"], out configMinutes);
            configMinutes = configMinutes < 1 ? 1 : configMinutes;
            Metric.Config.WithReporting(config =>
            {
                config.WithServiceReport(TimeSpan.FromMinutes(configMinutes));

                string TextFileReport = System.Configuration.ConfigurationManager.AppSettings["Metrics.TextFileReportPath"];
                if (!string.IsNullOrEmpty(TextFileReport))
                {
                    config.WithTextFileReport(TextFileReport, TimeSpan.FromMinutes(configMinutes));
                }
                string CSVReportsPath = System.Configuration.ConfigurationManager.AppSettings["Metrics.CSVReportsPath"];
                if (!string.IsNullOrEmpty(CSVReportsPath))
                {
                    config.WithCSVReports(CSVReportsPath, TimeSpan.FromMinutes(configMinutes));
                }
            });

            string httpEndpoint = System.Configuration.ConfigurationManager.AppSettings["Metrics.HttpEndpoint"];
            if (!string.IsNullOrEmpty(httpEndpoint))
            {
                Metric.Config.WithHttpEndpoint(httpEndpoint);
            }
        }
        public static Meter Meter(string name, Unit unit, TimeUnit rateUnit = TimeUnit.Seconds, MetricTags tags = default(MetricTags))
        {
            return Metric.Meter(name, unit, rateUnit, tags);
        }

        public static Histogram Histogram(string name, Unit unit, SamplingType samplingType = SamplingType.FavourRecent, MetricTags tags = default(MetricTags))
        {
            return Metric.Histogram(name, unit, samplingType, tags);
        }
    }
}
