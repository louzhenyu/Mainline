using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JetermEntity.Parser
{
    /// <summary>
    /// 解析获取票号信息指令（即：解析【DETR:TN/{票号}】指令）
    /// </summary>
    [Serializable]
    public class TicketInfo : ParserBase<JetermEntity.Request.TicketInfo, CommandResult<JetermEntity.Response.TicketInfo>>
    {
        #region 成员变量

        private JetermEntity.Request.TicketInfo _request = null;

        private CommandResult<JetermEntity.Response.TicketInfo> _response = null;

        /// <summary>
        /// 请求对象
        /// </summary>
        public override JetermEntity.Request.TicketInfo Request { get { return this._request; } }

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.TicketInfo> Response { get { return this._response; } }

        #endregion

        /// <summary>
        /// 构造获取票号信息指令（即：解析【DETR:TN/{票号}】指令）返回对象
        /// </summary>
        public TicketInfo()
        {
            _response = new CommandResult<JetermEntity.Response.TicketInfo>();

            _response.result = new JetermEntity.Response.TicketInfo();
        }

        public TicketInfo(string config, string officeNo)
            : this()
        {
            _response.config = config;
            _response.OfficeNo = officeNo;
        }

        /// <summary>
        /// 解析请求对象
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回获取票号信息指令（即：解析【DETR:TN/{票号}】指令）；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.TicketInfo request)
        {
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }

            return string.Format("DETR:TN/{0}", request.TicketNo);
        }

        /// <summary>
        /// 解析获取票号信息指令（即：解析【DETR:TN/{票号}】指令）返回结果
        /// </summary>
        /// <param name="cmdResult">获取获取票号信息指令（即：解析【DETR:TN/{票号}】指令）返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.TicketInfo> ParseCmdResult(string cmdResult)
        {
            _response.result.ResultBag = cmdResult;

            if (!ValidCmdResult(cmdResult))
            {
                _response.error.CmdResultBag = cmdResult;
                return _response;
            }

            //// 获得联程票号：
            //_response.result.LianChengTicketList = AnalysizeLianChengTicket(cmdResult, _request.TicketNo);

            string originalCmdResult = cmdResult;
            string originalTicketNo = string.Empty;

            // 思路：把文本转换为key/value键值对格式，一对键值对为一行            
            cmdResult = cmdResult.Replace("ORG/DST:", "\r\nORG/DST:");
            cmdResult = cmdResult.Replace("CONJ TKT:", "\r\nCONJ TKT:");
            cmdResult = cmdResult.Replace("|FOP:", "\r\nFOP:");
            cmdResult = cmdResult.Replace("|OI:", "\r\nOI:");
            cmdResult = cmdResult.Replace("|TKTN:", "\r\nTKTN:");
            // 客票类型比较特殊：返回的文本中ISI（客票类型）不是键值对形式，故通过正则表达式，将ISI转换到单独的一行
            Regex reg = new Regex(@"(ORG/DST:\s*[\S]*\s*)([\S]*)");
            cmdResult = reg.Replace(cmdResult, "$1\r\nISI:$2");

            // 转换以后的文本，就是一行为一对键值对，根据换行符分割
            IList<string> lines = cmdResult.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string line in lines)
            {
                if (!line.Contains(":"))// 检查该行是否是有效键值对
                {
                    continue;
                }

                string[] dic = line.Split(':');
                if (dic.Length != 2)
                {
                    continue;
                }

                string key = dic[0].Trim();
                string value = dic[1].Trim();
               
                if (key.Equals("PASSENGER") || key.Contains("SSENGER")) // 获得乘客姓名
                {
                    _response.result.PassengerName = value ?? string.Empty;
                    continue;
                }
                if (key.EndsWith("FM")) // 获得客票状态、出发城市三字码、航空公司、舱位和起飞日期
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    _response.result.FlightList = new List<Flight>();

                    Flight flight = new Flight();

                    //获得客票状态
                    EtermCommand.TicketStatus ticketStatusResult = EtermCommand.TicketStatus.NotSet;                 
                    ParserHelper.GetTicketStatusAndSaveTime(value, out ticketStatusResult);
                    flight.TicketStatus = ticketStatusResult;

                    IList<string> valueList = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    // 获得出发城市三字码
                    if (valueList.Count > 0)
                    {
                        flight.SCity = valueList[0].Length > 3 ? valueList[0].Substring(1, 3) : valueList[0];
                    }
                    // 获得航空公司、舱位和起飞日期
                    if (valueList.Count > 4)
                    {
                        flight.Airline = valueList[1]; // 获得航空公司
                        flight.Cabin = valueList[3]; // 获得舱位
                        flight.DepDateString = valueList[4].Equals("OPEN") ? string.Empty : valueList[4]; // 获得起飞日期
                    }

                    _response.result.FlightList.Add(flight);

                    continue;
                }
                if (key.EndsWith("RL") && string.IsNullOrWhiteSpace(_response.result.BigPnr))// 获得大编码
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    string[] codes = value.Split('/');
                    if (codes.Length == 2)
                    {
                        _response.result.BigPnr = codes[0].Trim();
                    }

                    continue;
                }
                if (key.EndsWith("TO") && !key.Equals("TO"))
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    Flight flight = new Flight();

                    //获得客票状态
                    EtermCommand.TicketStatus ticketStatusResult = EtermCommand.TicketStatus.NotSet;                    
                    ParserHelper.GetTicketStatusAndSaveTime(value, out ticketStatusResult);
                    flight.TicketStatus = ticketStatusResult;  

                    IList<string> valueList = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    // 获得出发城市三字码
                    if (valueList.Count > 0)
                    {
                        int length = _response.result.FlightList.Count;
                        Flight oldFlight = _response.result.FlightList[length - 1];
                        oldFlight.ECity = valueList[0].Length > 3 ? valueList[0].Substring(1, 3) : valueList[0];

                        flight.SCity = oldFlight.ECity;
                    }
                    // 获得航空公司、舱位和起飞日期
                    if (valueList.Count > 4)
                    {
                        flight.Airline = valueList[1]; // 获得航空公司
                        flight.Cabin = valueList[3]; // 获得舱位
                        flight.DepDateString = valueList[4].Equals("OPEN") ? string.Empty : valueList[4]; // 获得起飞日期
                    }

                    _response.result.FlightList.Add(flight);

                    continue;
                }
                if (key.Equals("TO")) // 获得到达城市三字码
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    int length = _response.result.FlightList.Count;                    
                    _response.result.FlightList[length - 1].ECity = value;

                    continue;
                }
                if (key.Equals("TKTN")) // 获得票号/编码
                {
                    originalTicketNo = value;
                    _response.result.TicketNo = (value ?? string.Empty).Replace("-", string.Empty);
                    continue;
                }
            }

            // 根据客票状态来决定是否设置永久缓存
            if (_response.result.FlightList != null && _response.result.FlightList.Count > 0)
            {
                _response.SaveTime = JetermEntity.EtermCommand.CacheTime.min30;

                int count = 0;
                foreach(Flight flight in _response.result.FlightList)
                {
                    if (flight.TicketStatus != EtermCommand.TicketStatus.REFUNDED
                        && flight.TicketStatus != EtermCommand.TicketStatus.USED_FLOWN
                        && flight.TicketStatus != EtermCommand.TicketStatus.VOID)
                    {
                        continue;
                    }

                    ++count;                    
                }

                if (count == _response.result.FlightList.Count)
                {
                    _response.SaveTime = JetermEntity.EtermCommand.CacheTime.infinite;
                }
            }

            // 获得联程票号：
            _response.result.LianChengTicketList = AnalysizeLianChengTicket(cmdResult, originalTicketNo);

            _response.state = true;
            return _response;
        }

        #region Helper

        protected internal override bool ValidRequest()
        {
            if (_request == null || string.IsNullOrWhiteSpace(_request.TicketNo))
            {
                _response.error = new Error(EtermCommand.ERROR.EMPTY_REQUEST_PARAM);
                return false;
            }

            _request.TicketNo = Regex.Replace(_request.TicketNo, @"\s", string.Empty).Trim().ToUpper();

            return true;
        }

        protected internal override bool ValidCmdResult(string cmdResult)
        {
            // 验证文本是否符合规范
            if (!ParserHelper.CheckData(cmdResult, new List<string>() { "ORG/DST:", "FM:", "RL:", "TKTN:" }))
            {
                _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 解析联程票号
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ticketNo"></param>
        /// <returns></returns>
        private List<string> AnalysizeLianChengTicket(string data, string ticketNo)
        {
            List<string> lianChengTicketList = new List<string>();
            if (!Regex.IsMatch(data, ticketNo + @"(([\/]{1}|[-]{1})[\d]+)+"))
            {
                return lianChengTicketList;
            }

            data = Regex.Replace(Regex.Match(data, ticketNo + @"(([\/]{1}|[-]{1})[\d]+)+").Value, ticketNo + @"([\/]{1}|[-]{1})+", string.Empty);
            if (!string.IsNullOrWhiteSpace(data))
            {
                string[] arr = Regex.Split(data, @"([\/]{1}|[-]{1})+");
                string tempTicketno = ticketNo;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(arr[i]) && !Regex.IsMatch(arr[i], @"([\/]{1}|[-]{1})+"))
                    {
                        lianChengTicketList.Add(tempTicketno.Replace(tempTicketno.Substring(tempTicketno.Length - arr[i].Length, arr[i].Length), arr[i]));
                    }
                }
            }

            return lianChengTicketList;
        }

        #endregion
    }
}
