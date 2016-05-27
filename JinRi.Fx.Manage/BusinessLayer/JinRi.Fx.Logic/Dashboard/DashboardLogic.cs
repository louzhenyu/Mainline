using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using JinRi.Fx.Data;
using JinRi.Fx.RequestDTO;
using JinRi.Fx.ResponseDTO;

namespace JinRi.Fx.Logic
{
    public class DashboardLogic
    {
        DashboardDal dal = new DashboardDal();
        public MetricSearchResponseDTO GetHistogramList(MetricSearchRequestDTO dto)
        {
            MetricSearchResponseDTO responseDTO = null;
            List<MetricHistogram> selectResult = null;
            Dictionary<string, Series> dic = new Dictionary<string, Series>();

            List<MetricHistogram> list = dal.GetHistogramList(dto);
            if (list != null && list.Count > 0)
            {
                responseDTO = new MetricSearchResponseDTO();

                responseDTO.MetricUnit = list[0].MetricUnit;
                //所有数据列名称
                List<string> seriesName = list.Select(x => x.SeriesName).Distinct().ToList<string>();
                //DateTime xAxisDateTime = dto.StartTime;
                DateTime xAxisDateTime = CommonHelper.GetDateByUnit(dto.IntervalUnit, dto.StartTime);
                DateTime endTimeSearchCondtion = CommonHelper.GetDateByUnit(dto.IntervalUnit, dto.EndTime);
                bool firstTime = true;
                //获取所有间隔时间的数据
                //while (xAxisDateTime < dto.EndTime || firstTime)
                while (xAxisDateTime < endTimeSearchCondtion || firstTime)
                {
                    firstTime = false;
                    responseDTO.XAxisValueList.Add(CommonHelper.ConvertDateToStringByUnit(dto.IntervalUnit, xAxisDateTime));
                    DateTime endTime = CommonHelper.GetNextTime(xAxisDateTime, dto.Interval, dto.IntervalUnit);
                    selectResult = list.Where(c => c.AddTime >= xAxisDateTime && c.AddTime < endTime).ToList<MetricHistogram>();

                    for (int index = 0; index < seriesName.Count; index++)
                    {
                        if (!dic.ContainsKey(seriesName[index]))
                        {
                            dic.Add(seriesName[index], new Series() { Name = seriesName[index] });
                        }
                        dic[seriesName[index]].YAxisValueList.Add(Math.Round(GetSeriesValue(selectResult, seriesName[index], dto.AggregationWay), 2));
                    }
                    xAxisDateTime = endTime;
                }
                responseDTO.SerieList = dic.Values.ToList<Series>();
            }
            return responseDTO;
        }

        public MetricSearchResponseDTO GetMeterList(MetricSearchRequestDTO dto)
        {
            List<MetricMeter> meterList = dal.GetMeterList(dto);
            if (meterList == null || meterList.Count < 1)
            {
                return null;
            }

            MetricSearchResponseDTO responseDTO = new MetricSearchResponseDTO();

            responseDTO.MetricUnit = meterList[0].MetricUnit;

            #region 分别求 responseDTO.XAxisValueList 和 responseDTO.SerieList

            // 分别求 responseDTO.XAxisValueList 和 responseDTO.SerieList：
            DateTime xAxisDateTime = CommonHelper.GetDateByUnit(dto.IntervalUnit, dto.StartTime);
            DateTime endTimeSearchCondtion = CommonHelper.GetDateByUnit(dto.IntervalUnit, dto.EndTime);
            List<MetricMeter> selectResult = null;
            List<string> seriesNameList = meterList.Select(x => x.SeriesName).Distinct().ToList<string>();
            // value表示Series Name，key表示yAxisValueList：
            Dictionary<string, Series> dictionary = new Dictionary<string, Series>();

            if (xAxisDateTime == endTimeSearchCondtion) // start: if-else
            {
                responseDTO.XAxisValueList.Add(CommonHelper.ConvertDateToStringByUnit(dto.IntervalUnit, xAxisDateTime));
                DateTime endTime = CommonHelper.GetNextTime(xAxisDateTime, dto.Interval, dto.IntervalUnit);
                selectResult = meterList.Where(m => m.XAxisValue >= xAxisDateTime && m.XAxisValue < endTime).ToList<MetricMeter>();

                GetYAxisValueDictionary(seriesNameList, selectResult, dto.AggregationWay, dictionary);
            }
            else
            {
                while (xAxisDateTime < endTimeSearchCondtion)
                { // start: while                
                    responseDTO.XAxisValueList.Add(CommonHelper.ConvertDateToStringByUnit(dto.IntervalUnit, xAxisDateTime));
                    DateTime endTime = CommonHelper.GetNextTime(xAxisDateTime, dto.Interval, dto.IntervalUnit);
                    selectResult = meterList.Where(m => m.XAxisValue >= xAxisDateTime && m.XAxisValue < endTime).ToList<MetricMeter>();
                    xAxisDateTime = endTime;

                    // 每次外循环结束后，就能为每个series获得一个YAxisValue：
                    GetYAxisValueDictionary(seriesNameList, selectResult, dto.AggregationWay, dictionary);

                }// end: while
            } // end: if-else
            responseDTO.SerieList = dictionary.Values.ToList<Series>();

            #endregion

            return responseDTO;
        }

        public List<MetricsKey> GetMetricsKeys()
        {
            return dal.GetMetricsKeys();
        }
        /// <summary>
        /// 聚合SeriesName数据
        /// </summary>
        /// <param name="selectResult">所有记录数</param>
        /// <param name="seriesName"></param>
        /// <param name="aggregationWay">聚合方式</param>
        /// <returns>Result</returns>
        private double GetSeriesValue(List<MetricHistogram> selectResult, string seriesName, AggregationWay aggregationWay)
        {
            double num = 0;
            if (selectResult != null)
            {
                IEnumerable<MetricHistogram> enumInfo = selectResult.Where(m => m.SeriesName == seriesName);
                if (enumInfo != null && enumInfo.Any())
                {
                    switch (aggregationWay)
                    {
                        case AggregationWay.COUNT: num = enumInfo.Sum(y => y.ValueCount); break;
                        case AggregationWay.SUM: num = enumInfo.Sum(y => y.ValueSum); break;
                        case AggregationWay.MAX: num = enumInfo.Max(y => y.ValueMax); break;
                        case AggregationWay.MIN: num = enumInfo.Min(y => y.ValueMin); break;
                        case AggregationWay.AVG:
                            num = enumInfo.Sum(y => y.ValueCount);
                            num = num == 0 ? num : 1.00 * enumInfo.Sum(y => y.ValueSum) / num;
                            break;
                    }
                }
            }
            return num;
        }

        private void GetYAxisValueDictionary(List<string> seriesNameList, List<MetricMeter> selectResult, AggregationWay aggregationWay, Dictionary<string, Series> dictionary)
        {
            foreach (string seriesName in seriesNameList)
            { // start: foreach (string seriesName in seriesNameList)
                if (!dictionary.ContainsKey(seriesName))
                {
                    dictionary.Add(seriesName, new Series() { Name = seriesName });
                }

                double yAxisValue = 0;
                if (selectResult == null || selectResult.Count < 1)
                {
                    dictionary[seriesName].YAxisValueList.Add(Math.Round(yAxisValue, 2));
                    continue;
                }

                // if(selectResult != null && selectResult.Count > 0):
                IEnumerable<MetricMeter> info = selectResult.Where<MetricMeter>(m => m.SeriesName == seriesName);
                if (info != null && info.Any())
                {
                    switch (aggregationWay)
                    {
                        case AggregationWay.COUNT:
                            yAxisValue = info.Sum(y => y.YAxisValueForCOUNT);
                            break;
                        case AggregationWay.SUM:
                            yAxisValue = info.Sum(y => y.YAxisValueForSUM);
                            break;
                        case AggregationWay.MAX:
                            yAxisValue = info.Max(y => y.YAxisValueForMAX);
                            break;
                        case AggregationWay.MIN:
                            yAxisValue = info.Min(y => y.YAxisValueForMIN);
                            break;
                        case AggregationWay.AVG:
                            yAxisValue = info.Sum(y => y.YAxisValueForCOUNT);
                            yAxisValue = yAxisValue == 0 ? yAxisValue : 1.00 * info.Sum(y => y.YAxisValueForSUM) / yAxisValue;
                            break;
                    }
                }
                dictionary[seriesName].YAxisValueList.Add(Math.Round(yAxisValue, 2));
            } // end: foreach (string seriesName in seriesNameList)
        }
    }
}
