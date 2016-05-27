using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JinRi.Fx.RequestDTO;

namespace JinRi.Fx.WebUI.Models
{
    public static class ModelExtensions
    {
        /// <summary>
        /// 将监控搜索条件Model转换DTO。
        /// </summary>
        public static MetricSearchRequestDTO ToDTO(this MetricSearchArgs model)
        {
            if (model == null)
            {
                return new MetricSearchRequestDTO();
            }
            MetricSearchRequestDTO dto = null;
            try
            {
                dto = new MetricSearchRequestDTO
                   {
                       MetricName = model.MetricName.Trim(),
                       AppID = model.AppID,
                       HostIP = string.IsNullOrWhiteSpace(model.HostIP) ? string.Empty : model.HostIP.Trim(),
                       StartTime = model.StartTime,
                       EndTime = model.EndTime,
                       Interval = model.Interval,
                       IntervalUnit = model.IntervalUnit,
                       AggregationWay = model.AggregationWay,
                       GroupBy = model.GroupBy
                   };
            }
            catch (Exception ex)
            {

            }
            return dto;
        }
    }
}