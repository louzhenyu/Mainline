using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.PinYinConverter;
using JetermEntity.Response;

namespace JetermEntity.Parser
{
    public class ParserHelper
    {
        #region Enum Values

        /// <summary>
        /// 月份枚举
        /// </summary>
        protected internal enum Month
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

        /// <summary>
        /// 获得大编码号
        /// </summary>
        /// <param name="rtResult"></param> 
        /// <return></return>
        public static string GetBigPNR(string rtResult)
        {
            string bigPNR = string.Empty;

            bigPNR = Regex.Match(rtResult, @"RMK CA/[0-9A-Z]{5,6}").Value.Replace("RMK CA/", string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(bigPNR))
            {
                bigPNR = Regex.Replace(Regex.Match(rtResult, @"RMK\s+CA/[0-9A-Z]{5,6}").Value, @"RMK\s+CA/", string.Empty).Trim();
            }
            if (string.IsNullOrWhiteSpace(bigPNR))
            {
                bigPNR = Regex.Match(rtResult, @"-CA-[0-9A-Z]{5,6}\s{1,}").Value.Replace("-CA-", string.Empty).Trim();
                if (bigPNR.ToUpper() == "HOSTRL")
                {
                    bigPNR = string.Empty;
                }
            }

            return bigPNR;
        }

        /// <summary>
        /// 验证RT命令返回结果
        /// </summary>
        /// <param name="data"></param>    
        /// <param name="pnrInput"></param> 
        /// <param name="passengeType"></param>
        /// <returns></returns>
        public static Error ValidRTResult(string data, string pnrInput = "", EtermCommand.PassengerType passengeType = EtermCommand.PassengerType.NotSet)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return new Error(EtermCommand.ERROR.RT_COMMAND_EMPTY);
            }

            // 大系统有可能会返回“err”：
            if (data.ToUpper().Equals("ERR") || data.ToUpper().Contains("ERROR"))
            {
                return new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }

            // 大系统有可能会返回“AUTHORITY”：
            if (data.IndexOf("需要授权") != -1 || data.ToUpper().IndexOf("AUTHORITY") != -1)
            {
                return new Error(EtermCommand.ERROR.REMARK_OFFICE);
            }

            if (data.ToUpper().Contains("NO PNR") || (!string.IsNullOrWhiteSpace(pnrInput) && data.IndexOf(pnrInput) == -1))
            {
                return new Error(EtermCommand.ERROR.NOT_EXIST_PNR);
            }

            // 判断记录编号是否被擦除
            if (data.ToUpper().Contains("CANCELLED"))
            {
                return new Error(EtermCommand.ERROR.CANCELLED);
            }

            if (passengeType.GetHashCode() == EtermCommand.PassengerType.Children.GetHashCode())
            {
                // 判断未将成人编码输入到儿童编码里
                if (!data.Contains("SSR OTHS"))
                {
                    return new Error(EtermCommand.ERROR.NO_AUDLT_PNR);
                }
            }

            if (passengeType.GetHashCode() == EtermCommand.PassengerType.Baby.GetHashCode())
            {
                // 成人编码不含婴儿航段，请做入后再导入，或选择预订创单！
                if (!data.Contains("INFT"))
                {
                    return new Error(EtermCommand.ERROR.AUDLT_NO_BABY);
                }
            }

            return null;
        }              

        /// <summary>
        /// 验证订位指令返回结果。
        /// </summary>
        /// <param name="bookingCmdResult">订位指令返回结果</param>
        /// <param name="newPnr">若验证通过，则获得订位后的PNR。</param>
        /// <returns></returns>
        public static bool GetNewPnr(string bookingCmdResult, ref string newPnr)
        {
            if (bookingCmdResult.Contains("-"))
            {
                newPnr = Regex.Replace(Regex.Match(bookingCmdResult, @"[A-Z0-9]{6}[\s]*-").Value, @"[\s]*-", string.Empty).Trim();
                return true;
            }

            string temp = Regex.Replace(bookingCmdResult, @"\[\S*\]", string.Empty);
            MatchCollection mc = Regex.Matches(temp, "[0-9A-Z]{6}");
            if (mc == null || mc.Count < 2)
            {
                return false;
            }

            newPnr = mc[mc.Count - 1].Value;

            return true;
        }

        /// <summary>
        /// 日期类型转化为string类型，格式如05AUG15
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDateTimeToString(DateTime dt)
        {
            if (dt == null || dt.Date == DateTime.MinValue.Date)
            {
                return string.Empty;
            }

            string monthStr = GetEnMonth(dt.Month);

            int day = dt.Day;
            string dayStr = string.Empty;
            switch (day)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    dayStr = string.Format("0{0}", day);
                    break;
                default:
                    dayStr = day.ToString();
                    break;
            }

            string yearStr = dt.Year.ToString().Substring(2, 2);
            string result = string.Format("{0}{1}{2}", dayStr, monthStr, yearStr);
            if (string.IsNullOrWhiteSpace(result))
            {
                return string.Empty;
            }
            if (result.Length != 7)
            {
                return string.Empty;
            }
            return result;
        }

        /// <summary>
        /// 计算实际年龄
        /// </summary>
        /// <param name="birthday"></param>
        /// <param name="nowTime"></param>
        /// <returns>实际年龄</returns>
        protected internal static int GetAge(DateTime birthday, DateTime nowTime)
        {
            int age = nowTime.Year - birthday.Year;
            if (age < 0)
            {
                return age;
            }

            if (birthday > nowTime.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        /// <summary>
        /// 去除儿童姓名中包含的“CHD”      
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected internal static string TrimCHD(string userName)
        {
            string strResult = userName.Trim();
            int length = strResult.Length;
            if (length > 2 && strResult.Substring(length - 3, 3).ToUpper() == "CHD")
            {
                strResult = strResult.Substring(0, length - 3);
            }
            return strResult;
        }

        /// <summary>
        /// 过滤SQL敏感字符
        /// </summary>
        /// <param name="inputText">要特殊过滤的字符串</param>
        /// <returns>过滤后的字符串</returns>
        protected internal static string FilterSqlSpecial(string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText)) //如果字符串为空，直接返回。
            {
                return inputText;
            }

            inputText = inputText.Trim();
            string oldText = inputText;
            inputText = inputText.ToLower();

            inputText = inputText.Replace("and ", string.Empty);
            inputText = inputText.Replace("exec ", string.Empty);
            inputText = inputText.Replace("insert ", string.Empty);
            inputText = inputText.Replace("select ", string.Empty);
            inputText = inputText.Replace("delete ", string.Empty);
            inputText = inputText.Replace("update ", string.Empty);
            inputText = inputText.Replace(" and", string.Empty);
            inputText = inputText.Replace(" exec", string.Empty);
            inputText = inputText.Replace(" insert", string.Empty);
            inputText = inputText.Replace(" select", string.Empty);
            inputText = inputText.Replace(" delete", string.Empty);
            inputText = inputText.Replace(" update ", string.Empty);
            inputText = inputText.Replace("chr ", string.Empty);
            inputText = inputText.Replace("mid ", string.Empty);
            inputText = inputText.Replace(" chr", string.Empty);
            inputText = inputText.Replace(" mid", string.Empty);
            inputText = inputText.Replace("master ", string.Empty);
            inputText = inputText.Replace(" master", string.Empty);
            inputText = inputText.Replace("or ", string.Empty);
            inputText = inputText.Replace(" or", string.Empty);
            inputText = inputText.Replace("truncate ", string.Empty);
            inputText = inputText.Replace("char ", string.Empty);
            inputText = inputText.Replace("declare ", string.Empty);
            inputText = inputText.Replace("join ", string.Empty);
            inputText = inputText.Replace("union ", string.Empty);
            inputText = inputText.Replace("truncate ", string.Empty);
            inputText = inputText.Replace(" char", string.Empty);
            inputText = inputText.Replace(" declare", string.Empty);
            inputText = inputText.Replace(" join", string.Empty);
            inputText = inputText.Replace(" union", string.Empty);
            inputText = inputText.Replace("'", string.Empty);
            inputText = inputText.Replace("<", string.Empty);
            inputText = inputText.Replace(">", string.Empty);
            inputText = inputText.Replace("%", string.Empty);
            inputText = inputText.Replace("'delete", string.Empty);
            inputText = inputText.Replace("''", string.Empty);
            inputText = inputText.Replace("\"\"", string.Empty);
            inputText = inputText.Replace(",", string.Empty);
            inputText = inputText.Replace(">=", string.Empty);
            inputText = inputText.Replace("=<", string.Empty);
            inputText = inputText.Replace("--", string.Empty);
            //InText = InText.Replace("_", string.Empty);
            inputText = inputText.Replace(";", string.Empty);
            inputText = inputText.Replace("||", string.Empty);
            //InText = InText.Replace("[", string.Empty);
            //InText = InText.Replace("]", string.Empty);
            //InText = InText.Replace("&", string.Empty);
            //InText = InText.Replace("/", string.Empty);
            //InText = InText.Replace("?", string.Empty);
            inputText = inputText.Replace(">?", string.Empty);
            inputText = inputText.Replace("?<", string.Empty);

            return (inputText.Equals(oldText.ToLower()) ? oldText : inputText);
        }

        /// <summary>
        /// 获得符合Eterm订位指令格式要求的Mobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        protected internal static string GetValidBookSeatMobile(string mobile)
        {
            mobile = Regex.Replace(mobile, @"\s", string.Empty).Trim();

            Regex reg = new Regex(@"[\d]{3,4}[-]?[\d]+");
            string regPhone = string.Empty;
            if (reg.IsMatch(mobile))
            {
                MatchCollection mc = reg.Matches(mobile);
                regPhone = mc[0].Value.Split('/')[0].Replace("-", string.Empty).Trim();
            }

            if (string.IsNullOrWhiteSpace(regPhone) || !Regex.IsMatch(regPhone, @"\d+"))
            {
                return string.Empty;
            }

            return regPhone;
        }

        /// <summary>
        /// 对中文进行排序
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        protected internal static IEnumerable<string> SortChinese(IEnumerable<string> strs)
        {
            List<ChinesePinYin> list = new List<ChinesePinYin>();
            if (strs == null)
            {
                return list.Select(p => p.Key);
            }

            foreach (string str in strs)
            {
                list.Add(new ChinesePinYin(str, ConvertToPinYin(str)));
            }
            list.Sort((x, y) => x.Value.CompareTo(y.Value));

            return list.Select(p => p.Key);
        }

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected internal static int GetLengthOfString(string str)
        {
            char[] code = str.ToCharArray();
            int stringLength = str.Length;
            foreach (char item in code)
            {
                if (Convert.ToInt32(item) > 255)
                {
                    stringLength++;
                }
            }

            return stringLength;
        }

        /// <summary>
        /// 获取英文月份名
        /// </summary>
        /// <param name="monthNumber">由数字字符组成的月份名</param>
        /// <returns></returns>
        protected internal static string GetEnMonth(string monthNumber)
        {
            switch (monthNumber)
            {
                case "01":
                    return "JAN";
                case "02":
                    return "FEB";
                case "03":
                    return "MAR";
                case "04":
                    return "APR";
                case "05":
                    return "MAY";
                case "06":
                    return "JUN";
                case "07":
                    return "JUL";
                case "08":
                    return "AUG";
                case "09":
                    return "SEP";
                case "10":
                    return "OCT";
                case "11":
                    return "NOV";
                case "12":
                    return "DEC";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 获取英文月份名
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        protected internal static string GetEnMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "JAN";
                case 2:
                    return "FEB";
                case 3:
                    return "MAR";
                case 4:
                    return "APR";
                case 5:
                    return "MAY";
                case 6:
                    return "JUN";
                case 7:
                    return "JUL";
                case 8:
                    return "AUG";
                case 9:
                    return "SEP";
                case 10:
                    return "OCT";
                case 11:
                    return "NOV";
                case 12:
                    return "DEC";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 获取数字字符月份
        /// </summary>
        /// <param name="monthString">英文型月份</param>
        /// <returns>返回数字字符月份</returns>
        protected internal static string GetNumMonth(string monthString)
        {
            string s = string.Empty;

            switch (monthString)
            {
                case "JAN":
                    s = "01";
                    break;
                case "FEB":
                    s = "02";
                    break;
                case "MAR":
                    s = "03";
                    break;
                case "APR":
                    s = "04";
                    break;
                case "MAY":
                    s = "05";
                    break;
                case "JUN":
                    s = "06";
                    break;
                case "JUL":
                    s = "07";
                    break;
                case "AUG":
                    s = "08";
                    break;
                case "SEP":
                    s = "09";
                    break;
                case "OCT":
                    s = "10";
                    break;
                case "NOV":
                    s = "11";
                    break;
                case "DEC":
                    s = "12";
                    break;
            }

            return s;
        }

        /// <summary>
        /// 修正航班起飞时间、到达时间
        /// <para>修正到达时间里面类似"120."的错误问题</para>   
        /// </summary>
        /// <param name="timeStr">格式：HHmm(有可能会有"120.","12"的错误格式)，长度必须是2、4位，否则不处理</param>
        /// <returns>正确的时间格式</returns>
        protected internal static string CheckFilghtTime(string timeStr)
        {
            if (string.IsNullOrWhiteSpace(timeStr))
            {
                return string.Empty;
            }

            if (timeStr.Length == 4 && timeStr.Trim().EndsWith("."))
            {
                timeStr = timeStr.Replace('.', '0');
                return timeStr;
            }

            if (timeStr.Length == 2)
            {
                timeStr = timeStr + "00";
                return timeStr;
            }

            return timeStr;
        }

        /// <summary>
        /// 格式化处理
        /// </summary>
        /// <param name="EtermDate"></param>     
        /// <returns></returns>
        protected internal static DateTime ConvertStringToDateTime(string EtermDate)
        {
            if (string.IsNullOrWhiteSpace(EtermDate))
            {
                return DateTime.MinValue;
            }
            EtermDate = EtermDate.Trim();

            // 格式如：11DEC2020
            if (EtermDate.Length == 9)
            {
                string day = EtermDate.Substring(0, 2);
                string month = EtermDate.Substring(2, 3);
                string hour = EtermDate.Substring(5, 2);
                string min = EtermDate.Substring(7, 2);
                return new DateTime(DateTime.Now.Year, (int)Enum.Parse(typeof(Month), month), Convert.ToInt32(day), Convert.ToInt32(hour), Convert.ToInt32(min), 0);
            }

            // 格式如：11DEC14
            if (EtermDate.Length == 7)
            {
                string day = EtermDate.Substring(0, 2);
                string month = EtermDate.Substring(2, 3);
                string year = string.Format("20{0}", EtermDate.Substring(5, 2));

                return new DateTime(Convert.ToInt32(year), (int)Enum.Parse(typeof(Month), month), Convert.ToInt32(day), 0, 0, 0);
            }

            // 格式如：11DEC
            if (EtermDate.Length == 5)
            {
                string day = EtermDate.Substring(0, 2);
                string month = EtermDate.Substring(2, 3);
                return new DateTime(DateTime.Now.Year, (int)Enum.Parse(typeof(Month), month), Convert.ToInt32(day), 0, 0, 0);
            }

            return DateTime.MinValue;
        }        

        /// <summary>
        /// 获得客票状态 以及 根据客票状态来决定是否设置永久缓存
        /// </summary>
        /// <param name="value"></param>     
        /// <param name="ticketStatusResult"></param>
        protected internal static JetermEntity.EtermCommand.CacheTime GetTicketStatusAndSaveTime(string value, out EtermCommand.TicketStatus ticketStatusResult)
        {
            string TicketStatus = "OPEN FOR USE,VOID,REFUNDED,CHECKED-IN,CHECKED,CHECKED IN,USED/FLOWN,SUSPENDED,PRINT/EXCH,EXCHANGED,LIFT/BOARDED,FIM EXCH,AIRP CNTL/YY,CPN NOTE,USED/CLOSED";

            ticketStatusResult = EtermCommand.TicketStatus.NotSet;

            IList<string> ticketStatusList = TicketStatus.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string ticketStatus in ticketStatusList)
            {
                //if (!ticketStatus.Contains(value)) 
                if (!ticketStatus.Contains(value) && !value.Contains(ticketStatus))
                {
                    continue;
                }

                // 获得客票状态
                ticketStatusResult = ConvertStringToTicketStatusEnum(ticketStatus);
                // 根据客票状态来决定是否设置永久缓存
                if (ticketStatusResult.GetHashCode() == EtermCommand.TicketStatus.REFUNDED.GetHashCode()
                    || ticketStatusResult.GetHashCode() == EtermCommand.TicketStatus.USED_FLOWN.GetHashCode()
                    || ticketStatusResult.GetHashCode() == EtermCommand.TicketStatus.VOID.GetHashCode())
                {
                    return JetermEntity.EtermCommand.CacheTime.infinite;
                }

                break;
            }

            return JetermEntity.EtermCommand.CacheTime.min30;
        }

        /// <summary>
        /// 判断文本中是否包含特定关键字，用于判断文本是否符合规范。符合规范的文本才会被解析。
        /// </summary>
        /// <param name="data">文本内容</param>
        /// <param name="keys">关键字</param>
        /// <returns></returns>
        protected internal static bool CheckData(string data, IList<string> keys)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return false;
            }

            bool b = true;// 是否通过检测
            // 判断文档中是否包含
            foreach (string key in keys)
            {
                if (!data.Contains(key))
                {
                    b = false;
                    break;
                }
            }

            return b;
        }

        /// <summary>
        /// 判断是否是团队，并设置guestNumber初始值
        /// </summary>
        /// <param name="rtResult"></param>
        /// <param name="guestNumber"></param>
        ///<param name="isTeam"></param>
        protected internal static void IsTeam(string rtResult, ref int guestNumber, ref bool isTeam)
        {
            if (!rtResult.Contains("0."))
            {
                return;
            }

            try
            {
                string myStr = rtResult.Substring(0, rtResult.IndexOf("1.")).Trim().Replace(" ", "|");
                string[] cutStr = myStr.Split('|');

                for (int i = 0; i < cutStr.Length; i++)
                {
                    if (cutStr[i].ToUpper().StartsWith("NM"))
                    {
                        guestNumber = int.Parse(cutStr[i].Replace("NM", string.Empty));
                        isTeam = true;
                        break;
                    }
                }
            }
            catch
            {
                guestNumber = 100;
            }
        }

        /// <summary>
        /// 兼容 SA30MAR13PEKCSX 中带有年份的问题
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected internal static string RepalceYear(string str)
        {
            if (Regex.IsMatch(str, @"[\w]{7}[\d]{2}[\w]{6}"))
            {
                string tempStr = Regex.Match(str, @"[\w]{7}[\d]{2}[\w]{6}").Value;
                str = str.Replace(tempStr, tempStr.Replace(tempStr.Substring(7, 2), "|"));
            }
            return str;
        }

        /// <summary>
        /// 获得航站楼信息
        /// </summary>
        /// <param name="terminalInfo"></param>
        /// <param name="flight"></param>
        /// <returns></returns>
        protected internal static void GetTerminal(string terminalInfo, Flight flight)
        {
            if (terminalInfo.Length >= 4)
            {
                flight.DepTerminal = terminalInfo.Substring(0, 2) == "--" ? string.Empty : terminalInfo.Substring(0, 2);
                flight.ArrTerminal = terminalInfo.Substring(2, 2) == "--" ? string.Empty : terminalInfo.Substring(2, 2);
            }
            else if (terminalInfo.Length >= 2)
            {
                flight.DepTerminal = terminalInfo.Substring(0, 2);
            }
            else
            {
                flight.DepTerminal = terminalInfo;
            }
        }

        #region 对中文进行排序的辅助成员和辅助方法

        /// <summary>
        /// 内部类
        /// </summary>
        private class ChinesePinYin
        {
            /// <summary>
            /// 未转换前字符串
            /// </summary>
            public string Key { get; private set; }

            /// <summary>
            /// 转换后字符串
            /// </summary>
            public string Value { get; private set; }

            public ChinesePinYin(string key, string value)
            {
                this.Key = key;
                this.Value = value;
            }
        }

        /// <summary>
        /// 中文转换成拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ConvertToPinYin(string str)
        {
            IEnumerable<string> chineseList = GetChinese(str);
            foreach (string chinese in chineseList)
            {
                // 加入特殊字符防止出现连字后拼音相同的情况,如：万恶 和 哇呢
                str = str.Replace(chinese, string.Join(string.Empty, ConvertToPinYin(chinese.ToCharArray()).ToArray()));
            }

            return str;
        }

        /// <summary>
        /// 字符转换成拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static IList<string> ConvertToPinYin(Char[] chars)
        {
            IList<string> list = new List<string>();

            foreach (char c in chars)
            {
                ChineseChar cc = new ChineseChar(c);
                list.Add(cc.Pinyins.Count >= 1 ? cc.Pinyins[0].Substring(0, cc.Pinyins[0].Length - 1) : string.Empty);
            }

            return list;
        }

        /// <summary>
        /// 获取中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetChinese(string str)
        {
            IList<string> chineseList = new List<string>();

            MatchCollection matches = Regex.Matches(str, "[\u4E00-\u9FFF]{1}");
            foreach (Match match in matches)
            {
                if (!chineseList.Contains(match.Value))
                {
                    chineseList.Add(match.Value);
                }
            }

            return chineseList;
        }

        #endregion

        /// <summary>
        /// Ticket Status对应的字符串转成对应的Enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static EtermCommand.TicketStatus ConvertStringToTicketStatusEnum(string value)
        {
            switch (value)
            {
                case "OPEN FOR USE":
                    return EtermCommand.TicketStatus.OPEN_FOR_USE;
                case "VOID":
                    return EtermCommand.TicketStatus.VOID;
                case "REFUNDED":
                    return EtermCommand.TicketStatus.REFUNDED;
                case "CHECKED-IN":
                case "CHECKED IN":
                case "CHECKED":
                    return EtermCommand.TicketStatus.CHECKED_IN;                
                case "USED/FLOWN":
                    return EtermCommand.TicketStatus.USED_FLOWN;
                case "SUSPENDED":
                    return EtermCommand.TicketStatus.SUSPENDED;
                case "PRINT/EXCH":
                    return EtermCommand.TicketStatus.PRINT_EXCH;
                case "EXCHANGED":
                    return EtermCommand.TicketStatus.EXCHANGED;
                case "LIFT/BOARDED":
                    return EtermCommand.TicketStatus.LIFT_BOARDED;
                case "FIM EXCH":
                    return EtermCommand.TicketStatus.FIM_EXCH;
                case "AIRP CNTL/YY":
                    return EtermCommand.TicketStatus.AIRP_CNTL_YY;
                case "CPN NOTE":
                    return EtermCommand.TicketStatus.CPN_NOTE;
                case "USED/CLOSED":
                    return EtermCommand.TicketStatus.USED_CLOSED;
                default:
                    return EtermCommand.TicketStatus.NotSet;
            }
        }
    }
}
