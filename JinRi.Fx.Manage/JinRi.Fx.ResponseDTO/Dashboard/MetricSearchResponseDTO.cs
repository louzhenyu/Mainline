using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Fx.Entity;

namespace JinRi.Fx.ResponseDTO
{
    public class MetricSearchResponseDTO
    {
        /// <summary>
        /// 计数器单位(用于显示在Y轴上的单位)
        /// </summary>
        public string MetricUnit { get; set; }

        private List<string> xAxisValueList = new List<string>();
        /// <summary>
        /// X轴显示的文字列表
        /// </summary>
        public List<string> XAxisValueList
        {
            get { return xAxisValueList; }
            set { xAxisValueList = value; }
        }

        private List<Series> seriesList = new List<Series>();
        /// <summary>
        /// 所有数据列以及相应的数据集合
        /// </summary>
        public List<Series> SerieList
        {
            get { return seriesList; }
            set { seriesList = value; }
        }
    }
}
