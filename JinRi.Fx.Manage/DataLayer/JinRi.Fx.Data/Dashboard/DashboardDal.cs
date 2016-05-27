using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using JinRi.Fx.RequestDTO;
using System.Data.SqlClient;
using Dapper;

namespace JinRi.Fx.Data
{
    public class DashboardDal
    {
        public List<MetricHistogram> GetHistogramList(MetricSearchRequestDTO dto)
        {
            if (dto == null) { return null; }
            StringBuilder commandText = new StringBuilder();

            string dateMethod = CommonHelper.ConvertDateMethod("AddTime", dto.IntervalUnit);
            commandText.AppendLine(string.Format("SELECT {0} AS SeriesName, Sum(ValueCount) AS ValueCount,Sum(ValueSum) AS ValueSum,Min(ValueMin) AS ValueMin,Max(ValueMax) AS ValueMax,HistogramUnit AS MetricUnit,LEFT({1} + '00:00:00', 19) AS AddTime ", dto.GroupBy.GetHashCode() == GroupBy.NotSet.GetHashCode() ? "Name" : dto.GroupBy.ToString(), dateMethod));
            commandText.AppendLine("FROM MetsHistogram WITH(NOLOCK) ");
            commandText.AppendLine("WHERE 1=1 ");
            if (!string.IsNullOrWhiteSpace(dto.MetricName))
            {
                commandText.AppendLine(string.Format("AND Name='{0}' ", dto.MetricName));
            }
            if (dto.AppID > 0)
            {
                commandText.AppendLine(string.Format("AND AppId={0} ", dto.AppID));
            }
            if (!string.IsNullOrWhiteSpace(dto.HostIP))
            {
                commandText.AppendLine(string.Format("AND HostIP LIKE '{0}%' ", dto.HostIP));
            }
            commandText.AppendLine(string.Format("AND (AddTime>='{0}' ", dto.StartTime.ToString("yyyy-MM-dd HH:mm")));
            commandText.AppendLine(string.Format("AND AddTime<'{0}') ", dto.EndTime.AddMinutes(1).ToString("yyyy-MM-dd HH:mm")));
            commandText.AppendLine(string.Format("GROUP BY LEFT({0} + '00:00:00', 19), HistogramUnit, {1} ", dateMethod, dto.GroupBy.GetHashCode() == GroupBy.NotSet.GetHashCode() ? "Name" : dto.GroupBy.ToString()));
        
            return DapperHelper<MetricHistogram>.GetList(ConnectionStr.FxDb, commandText.ToString()).ToList<MetricHistogram>();
        }

        public List<MetricMeter> GetMeterList(MetricSearchRequestDTO dto)
        {
            if (dto == null)
            {
                return null;
            }

            StringBuilder sqlStringBuilder = new StringBuilder();            
            sqlStringBuilder.AppendLine("SELECT MeterUnit AS MetricUnit");
            if (dto.GroupBy.GetHashCode() == GroupBy.NotSet.GetHashCode())
            {
                sqlStringBuilder.AppendLine(", Name AS SeriesName");
            }
            else
            {
                sqlStringBuilder.AppendLine(string.Format(", {0} AS SeriesName", dto.GroupBy.ToString()));
            }
            string addTimeString = CommonHelper.ConvertDateMethod("AddTime", dto.IntervalUnit);           
            sqlStringBuilder.AppendLine(string.Format(", LEFT({0} + '00:00:00', 19) AS XAxisValue", addTimeString));
            sqlStringBuilder.AppendLine(", MIN(RequestCount) AS YAxisValueForMIN");
            sqlStringBuilder.AppendLine(", MAX(RequestCount) AS YAxisValueForMAX");
            sqlStringBuilder.AppendLine(", SUM(RequestCount) AS YAxisValueForSUM");
            sqlStringBuilder.AppendLine(", COUNT(RequestCount) AS YAxisValueForCOUNT ");
            sqlStringBuilder.AppendLine("FROM dbo.MetsMeter WITH(NOLOCK) ");
            sqlStringBuilder.AppendLine("WHERE 1 = 1");
            if (!string.IsNullOrWhiteSpace(dto.MetricName))
            {
                sqlStringBuilder.AppendLine(string.Format(" AND Name = '{0}'", dto.MetricName));
            }
            if (dto.AppID > 0)
            {
                sqlStringBuilder.AppendLine(string.Format(" AND AppID = {0}", dto.AppID));
            }
            if (!string.IsNullOrWhiteSpace(dto.HostIP))
            {
                sqlStringBuilder.AppendLine(string.Format(" AND HostIP LIKE '{0}%'", dto.HostIP));
            }
            sqlStringBuilder.AppendLine(string.Format(" AND (AddTime >= '{0}'", dto.StartTime.ToString("yyyy-MM-dd HH:mm")));
            sqlStringBuilder.AppendLine(string.Format(" AND AddTime < '{0}')", dto.EndTime.AddMinutes(1).ToString("yyyy-MM-dd HH:mm")));
            sqlStringBuilder.AppendLine(string.Format(" GROUP BY LEFT({0} + '00:00:00', 19), MeterUnit, {1} ", addTimeString, dto.GroupBy.GetHashCode() == GroupBy.NotSet.GetHashCode() ? "Name" : dto.GroupBy.ToString()));

            return DapperHelper<MetricMeter>.GetList(ConnectionStr.FxDb, sqlStringBuilder.ToString()).ToList<MetricMeter>();
        }


        public List<MetricsKey> GetMetricsKeys()
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = @"SELECT DISTINCT NAME AS [KEY],1 AS [Type] FROM MetsHistogram WITH(NOLOCK) 
UNION ALL
SELECT DISTINCT NAME,0 FROM MetsMeter WITH(NOLOCK) ";
                return conn.Query<MetricsKey>(sql).ToList<MetricsKey>();
            }
        }
    }
}
