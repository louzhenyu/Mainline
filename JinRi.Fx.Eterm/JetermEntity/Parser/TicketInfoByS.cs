using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JetermEntity.Parser
{
    /// <summary>
    /// 解析【DETR:TN/{票号},S】指令
    /// </summary>
    /// <remarks>
    // 命令：
    //system("DETR:TN/784-2158602564,S");
    //system("PN");
    // 返回结果：   
    //航空公司电子客票航程通知单                                                                  
    //电子客票票号       784-2158602564                                                    
    //后续客票号         NONE                                                             
    //出票航空公司       MIS CAAC                                                          
    //售票处信息         CHINA SOUTHERN AIRLINES WEB                                      
    //出票时间/地点      17DEC14/GUANGZHOU(11)<08685898>                                   
    //旅客姓名           张细志                                                             
    //身份识别号码       NONE                                                              
    //票价    货币       CNY  金额   640.00                                                
    //实付等值货币       CNY  金额   640.00    付款方式 CC                                       
    //税款   CNY 50.00CN  CNY 60.00YQ  CNY EXEMPTXT                                    +


    //付款总额           CNY  750.00                                                   -  

    //使用限制        BUDEQIANZHUAN不得签转/BIANGENGTUIPIAOSHOUFEI变更退票收洓溓┳靶畔皼潧鈲
    //瓲经停 起飞城市机场         日期  星期 时间 航班 舱位                    
    //    DEP  CSX-CHANGSHA      26DEC FRI 1425 CZ3461   UU          26DEC  26DEC 20K 
    //经停 到达城市机场         日期  星期 时间 机型 订座                                              
    //    ARR  CTU-CHENGDU       26DEC FRI               CONFIRMED                   
    //订座记录编号 HWP8PF/1E           7Q'FW4L,  USED/CLOSED                               
    //------------------------------------------------------------------------------- 
    //.                                                                              
    //VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   +
    /// </remarks>
    [Serializable]
    public class TicketInfoByS : ParserBase<JetermEntity.Request.TicketInfoByS, CommandResult<JetermEntity.Response.TicketInfoByS>>
    {
        #region 成员变量

        private JetermEntity.Request.TicketInfoByS _request = null;

        private CommandResult<JetermEntity.Response.TicketInfoByS> _response = null;

        /// <summary>
        /// 请求对象
        /// </summary>
        public override JetermEntity.Request.TicketInfoByS Request { get { return this._request; } }

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.TicketInfoByS> Response { get { return this._response; } }

        #endregion

        /// <summary>
        /// 构造【DETR:TN/{票号},S】指令返回对象
        /// </summary>
        public TicketInfoByS()
        {
            _response = new CommandResult<JetermEntity.Response.TicketInfoByS>();

            _response.result = new JetermEntity.Response.TicketInfoByS();
        }

        public TicketInfoByS(string config, string officeNo)
            : this()
        {
            _response.config = config;
            _response.OfficeNo = officeNo;
        }

        /// <summary>
        /// 解析请求对象
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回【DETR:TN/{票号},S】指令；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.TicketInfoByS request)
        {
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }

            return string.Format("DETR:TN/{0},S", request.TicketNo);
        }

        /// <summary>
        /// 解析【DETR:TN/{票号},S】指令返回结果
        /// </summary>
        /// <param name="cmdResult">获取【DETR:TN/{票号},S】指令返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.TicketInfoByS> ParseCmdResult(string cmdResult)
        {
            _response.result.ResultBag = cmdResult;

            if (!ValidCmdResult(cmdResult))
            {
                _response.error.CmdResultBag = cmdResult;
                return _response;
            }

            cmdResult = cmdResult.Replace("\r", "\r\n");

            // 1、获得票号/编码
            Regex reg = new Regex(@"电子客票票号\s*(\S*)");
            Match match = reg.Match(cmdResult);
            _response.result.TicketNo = (match.Groups[1].Value ?? string.Empty).Replace("-", "").Trim();

            // 2、获得乘客姓名
            reg = new Regex(@"旅客姓名(.*)");
            match = reg.Match(cmdResult);
            _response.result.PassengerName = match.Groups[1].Value.Trim();

            // 3、获得价格
            GetPrice(cmdResult);

            // 4、获得出发城市三字码、到达城市三字码、起飞日期、到达日期、航班号、航司和舱位
            GetFlight(cmdResult);

            // 5、获得客票状态
            // 6、根据客票状态来决定是否设置永久缓存
            _response.result.TicketStatus = EtermCommand.TicketStatus.NotSet;
            MatchCollection mc = Regex.Matches(cmdResult, @"订座记录编号(.*)");
            if (mc != null && mc.Count > 0)
            {
                reg = new Regex(@"订座记录编号(.*)");
                match = reg.Match(mc[mc.Count - 1].Value);
                if (match != null && match.Groups != null && match.Groups.Count > 0)
                {
                    string arr = match.Groups[1].Value.Trim();
                    if (!string.IsNullOrWhiteSpace(arr))
                    {
                        // 获得客票状态 以及 根据客票状态来决定是否设置永久缓存
                        EtermCommand.TicketStatus ticketStatusResult = EtermCommand.TicketStatus.NotSet;
                        _response.SaveTime = ParserHelper.GetTicketStatusAndSaveTime(arr, out ticketStatusResult);
                        _response.result.TicketStatus = ticketStatusResult;
                    }
                }
            }

            _response.state = true;
            return _response;
        }

        #region Helper

        protected internal override bool ValidRequest()
        {
            // 验证请求参数的合法性 + 为请求参数去除空格：
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
            if (cmdResult.Contains("TICKET NUMBER IS NOT EXIST"))
            {
                _response.error = new Error(EtermCommand.ERROR.NO_TICKET);
                return false;
            }

            // 验证文本是否符合规范
            if (!ParserHelper.CheckData(cmdResult, new List<string>() { "电子客票票号", "旅客姓名", "票价", "税款", "付款总额", "DEP", "ARR", "订座记录编号" }))
            {
                _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获得价格
        /// </summary>
        /// <param name="etermApiResult"></param>  
        private void GetPrice(string etermApiResult)
        {
            // A、获得票面价
            Regex reg = new Regex(@"票价.*金额\s*(\d+\.\d{2})");
            Match match = reg.Match(etermApiResult);
            _response.result.Price = new JetermEntity.Price();
            _response.result.Price.FacePrice = 0;
            if (match != null && match.Groups != null && match.Groups.Count > 0 && !string.IsNullOrWhiteSpace(match.Groups[1].Value.Trim()))
            {
                _response.result.Price.FacePrice = decimal.Parse(match.Groups[1].Value.Trim());
            }

            // 获得税和机建燃油费：
            reg = new Regex(@"税款[^\d]*(\d+\.\d{2})[^\d]*(\d+\.\d{2})*([\s|\S]*)付款总额");
            match = reg.Match(etermApiResult);

            // B、获得税
            string airportTaxStr = string.Empty;
            // C、获得机建燃油费
            string bunkerSurchargeStr = string.Empty;
            if (match != null && match.Groups != null && match.Groups.Count > 0)
            {
                airportTaxStr = match.Groups[1].Value.Trim();
                if (match.Groups.Count > 1)
                {
                    bunkerSurchargeStr = match.Groups[2].Value.Trim();
                }
            }

            // B、获得税
            _response.result.Price.Tax = 0;
            if (!string.IsNullOrWhiteSpace(airportTaxStr))
            {
                _response.result.Price.Tax = decimal.Parse(airportTaxStr);
            }

            // C、获得机建燃油费
            _response.result.Price.Fuel = 0;
            if (!string.IsNullOrWhiteSpace(bunkerSurchargeStr))
            {
                _response.result.Price.Fuel = decimal.Parse(bunkerSurchargeStr);
            }

            // D、获得总价
            reg = new Regex(@"付款总额[^\d]*(\d+\.\d{2})");
            match = reg.Match(etermApiResult);
            string totalAmountStr = string.Empty;
            if (match != null && match.Groups != null && match.Groups.Count > 0)
            {
                totalAmountStr = match.Groups[1].Value.Trim();
            }
            _response.result.Price.TotalPrice = 0;
            if (!string.IsNullOrWhiteSpace(totalAmountStr))
            {
                _response.result.Price.TotalPrice = decimal.Parse(totalAmountStr);
            }
        }

        /// <summary>
        /// 获得航班信息，包括出发城市三字码、到达城市三字码、出发日期、起飞日期、航班号、航司以及舱位
        /// </summary>
        /// <param name="etermApiResult"></param>     
        private void GetFlight(string etermApiResult)
        {
            JetermEntity.Response.TicketInfoByS result = _response.result;

            MatchCollection mc = Regex.Matches(etermApiResult, @"DEP(.*)");
            if (mc == null || mc.Count < 1)
            {
                return;
            }

            // 获得出发航班信息（列表），即出发城市三字码、出发日期、航班号、航司、舱位
            Flight flight = null;
            Regex reg = new Regex(@"DEP(.*)");
            Match match = null;
            for (int i = 0; i < mc.Count; i++)
            {
                match = reg.Match(mc[i].Value);
                if (match == null || match.Groups == null || match.Groups.Count < 1)
                {
                    break;
                }

                // 如：
                // CSX-CHANGSHA      26DEC FRI 1425 CZ3461   UU          26DEC  26DEC 20K
                // HGH-HANGZHOU      06MAY WED 1550 QW9792   ZZ                       20K
                // XUZ-XUZHOU        25FEB WED 2225 MF8870   KK                 09FEB 20K
                // DLC-DALIAN        21FEB SAT 0745 CZ6337   HYRT70      21FEB  21FEB 20K
                // HAK-HAIKOU        25FEB WED 2000 CZ8334   YYRT95      25FEB  25FEB 20K+
                string def = match.Groups[1].Value.Trim();
                if (string.IsNullOrWhiteSpace(def))
                {
                    continue;
                }

                IList<string> defList = def.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (defList == null || defList.Count < 1)
                {
                    break;
                }

                flight = new Flight();
                // A、获得出发城市三字码
                if (defList[0].Length > 3)
                {
                    flight.SCity = defList[0].Substring(0, 3);
                }

                // B、获得出发日期
                string sDateWithoutTime = defList.Count > 1 ? defList[1] : string.Empty;
                string sTime = defList.Count > 3 ? defList[3] : string.Empty;
                string sDate = string.Format("{0}{1}", sDateWithoutTime, sTime);
                if (!string.IsNullOrWhiteSpace(sDate) && sDate.Length == 9)
                {
                    // 传入格式如：24OCT2021
                    flight.DepDate = ParserHelper.ConvertStringToDateTime(sDate);
                }
                else if (!string.IsNullOrWhiteSpace(sDateWithoutTime) && sDateWithoutTime.Length == 5)
                {
                    // 传入格式如：24OCT
                    flight.DepDate = ParserHelper.ConvertStringToDateTime(sDateWithoutTime);
                }

                // C、获得航班号
                // D、获得航司
                flight.Airline = string.Empty;
                if (defList.Count > 2)
                {
                    flight.FlightNo = defList[4];
                    flight.Airline = defList[4].Length > 3 ? defList[4].Substring(0, 2) : string.Empty;
                }
                if (def.Contains("OPEN"))
                {
                    reg = new Regex(@"([A-Z0-9]{2})OPEN");
                    match = reg.Match(def);
                    flight.Airline = match.Groups[1].Value.Trim();
                }

                // E、获得舱位
                if (def.Contains("OPEN"))
                {
                    reg = new Regex(flight.Airline + @"OPEN\s+([A-Z])");
                    match = reg.Match(def);
                    flight.Cabin = match.Groups[1].Value.Trim();
                }
                else
                {
                    if (defList.Count > 5)
                    {
                        if (defList[4].Length == 2)
                        {
                            if (defList.Count > 6)
                            {
                                flight.Cabin = defList[6].Length > 0 ? defList[6].Substring(0, 1) : string.Empty;
                            }
                        }
                        else
                        {
                            // 如：CSX-CHANGSHA      26DEC FRI 1425 CZ3461   UU          26DEC  26DEC 20K
                            flight.Cabin = defList[5].Length > 0 ? defList[5].Substring(0, 1) : string.Empty;
                        }
                    }
                }

                result.FlightList.Add(flight);
            }

            // 获得到达城市三字码、到达日期（列表）：
            mc = Regex.Matches(etermApiResult, @"ARR(.*)");
            if (mc == null || mc.Count < 1)
            {
                return;
            }

            if (result.FlightList == null || !result.FlightList.Any() || mc.Count != result.FlightList.Count)
            {
                return;
            }

            reg = new Regex(@"ARR(.*)");
            match = null;
            for (int i = 0; i < mc.Count; i++)
            {
                match = reg.Match(mc[i].Value);
                if (match == null || match.Groups == null || match.Groups.Count < 1)
                {
                    break;
                }

                // 如：
                // CTU-CHENGDU       26DEC FRI      (320)    CONFIRMED
                // TAO-QINGDAO       06MAY WED 1740 (320)    CONFIRMED             
                // HAK-HAIKOU        21FEB SAT      (320)    CONFIRMED de           
                string arr = match.Groups[1].Value.Trim();
                if (string.IsNullOrWhiteSpace(arr))
                {
                    continue;
                }

                IList<string> arrList = arr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (arrList != null && arrList.Count > 0 && arrList[0].Length > 3)
                {
                    flight = result.FlightList[i];

                    // F、获得到达城市三字码
                    if (arrList[0].Length > 3)
                    {
                        flight.ECity = arrList[0].Substring(0, 3);
                    }

                    // G、获得到达日期
                    string eDateWithoutTime = arrList.Count > 1 ? arrList[1] : string.Empty;
                    string eTime = arrList.Count > 3 ? arrList[3] : string.Empty;
                    string eDate = string.Format("{0}{1}", eDateWithoutTime, eTime);
                    if (!string.IsNullOrWhiteSpace(eDate) && eDate.Length == 9)
                    {
                        // 传入格式如：24OCT2021
                        flight.ArrDate = ParserHelper.ConvertStringToDateTime(eDate);

                        if (flight.DepDate.Date != DateTime.MinValue && flight.DepDate > flight.ArrDate)
                        {
                            flight.ArrDate = flight.ArrDate.AddDays(1);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(eDateWithoutTime) && eDateWithoutTime.Length == 5)
                    {
                        // 传入格式如：24OCT
                        flight.ArrDate = ParserHelper.ConvertStringToDateTime(eDateWithoutTime);

                        if (flight.DepDate.Date != DateTime.MinValue && flight.DepDate.Date > flight.ArrDate.Date)
                        {
                            flight.ArrDate = flight.ArrDate.AddDays(1);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
