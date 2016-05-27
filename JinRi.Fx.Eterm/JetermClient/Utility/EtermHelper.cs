using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JetermClient.Utility
{
    public class EtermHelper
    {
        /// <summary>
        /// 格式化处理
        /// </summary>
        /// <param name="EtermDate"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string EtermDate)
        {
            if (string.IsNullOrWhiteSpace(EtermDate)) return DateTime.MinValue;
            EtermDate=EtermDate.Trim();
            if (EtermDate.Length != 9) return DateTime.MinValue;
            string day = EtermDate.Substring(0, 2);
            string month = EtermDate.Substring(2, 3);
            string hour = EtermDate.Substring(5, 2);
            string min = EtermDate.Substring(7, 2);
            return new DateTime(DateTime.Now.Year, (int)Enum.Parse(typeof(Month), month), Convert.ToInt32(day), Convert.ToInt32(hour), Convert.ToInt32(min), 0);
        }

        #region Enum Values

        /// <summary>
        /// 月份枚举
        /// </summary>
        public enum Month
        {
            /// <summary>
            /// 一月
            /// </summary>
            JAN = 1,
            /// <summary>
            /// 二月
            /// </summary>
            FEB = 2,
            /// <summary>
            /// 三月
            /// </summary>
            MAR = 3,
            /// <summary>
            /// 四月
            /// </summary>
            APR = 4,
            /// <summary>
            /// 五月
            /// </summary>
            MAY = 5,
            /// <summary>
            /// 六月
            /// </summary>
            JUN = 6,
            /// <summary>
            /// 七月
            /// </summary>
            JUL = 7,
            /// <summary>
            /// 八月
            /// </summary>
            AUG = 8,
            /// <summary>
            /// 九月
            /// </summary>
            SEP = 9,
            /// <summary>
            /// 十月
            /// </summary>
            OCT = 10,
            /// <summary>
            /// 十一月
            /// </summary>
            NOV = 11,
            /// <summary>
            /// 十二月
            /// </summary>
            DEC = 12
        }        

        #endregion
    }
}
