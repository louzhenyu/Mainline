using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JetermEntity.Parser
{
    /// <summary>
    /// 解析根据大编码号获取票号指令（即：解析【DETR:CN/{大编码号},C】指令）
    /// </summary>
    [Serializable]
    public class TicketByBigPnr : ParserBase<JetermEntity.Request.TicketByBigPnr, CommandResult<JetermEntity.Response.TicketByBigPnr>>
    {
        #region 成员变量

        private JetermEntity.Request.TicketByBigPnr _request = null;

        private CommandResult<JetermEntity.Response.TicketByBigPnr> _response = null;

        /// <summary>
        /// 请求对象
        /// </summary>
        public override JetermEntity.Request.TicketByBigPnr Request { get { return this._request; } }

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.TicketByBigPnr> Response { get { return this._response; } }

        /// <summary>
        /// 航班查询条件
        /// </summary>
        public string FlightNo { get; set; }

        /// <summary>
        /// 出发城市三字码查询条件
        /// </summary>
        public string SCity { get; set; }

        /// <summary>
        /// 到达cha
        /// </summary>
        public string ECity { get; set; }

        #endregion

        /// <summary>
        /// 构造根据大编码号获取票号指令（即：解析【DETR:CN/{大编码号},C】指令）返回对象
        /// </summary>
        public TicketByBigPnr()
        {
            _response = new CommandResult<JetermEntity.Response.TicketByBigPnr>();
            _response.result = new JetermEntity.Response.TicketByBigPnr();
        }

        public TicketByBigPnr(string config, string officeNo)
            : this()
        {
            _response.config = config;
            _response.OfficeNo = officeNo;
        }

        /// <summary>
        /// 解析请求对象
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回根据大编码号获取票号指令（即：解析【DETR:CN/{大编码号},C】指令）；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.TicketByBigPnr request)
        {
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }

            // 返回前设置查询条件
            FlightNo = _request.FlightNo;
            SCity = _request.SCity;
            ECity = _request.ECity;

            return string.Format("DETR:CN/{0},C", request.BigPnr);
        }

        /// <summary>
        /// 解析根据大编码号获取票号指令（即：解析【DETR:CN/{大编码号},C】指令）返回结果
        /// </summary>
        /// <param name="cmdResult">获取根据大编码号获取票号指令（即：解析【DETR:CN/{大编码号},C】指令）返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.TicketByBigPnr> ParseCmdResult(string cmdResult)
        {
            _response.result.ResultBag = cmdResult;

            if (string.IsNullOrWhiteSpace(FlightNo) || string.IsNullOrWhiteSpace(SCity) || string.IsNullOrWhiteSpace(ECity))
            {
                // 请先设置查询条件：航班号FlightNo、出发城市SCity以及到达城市ECity
                _response.error = new Error(EtermCommand.ERROR.NO_QUERY_CONDITION_OF_TICKETBYBIGPNR);
                return _response;
            }

            FlightNo = Regex.Replace(FlightNo, @"\s", string.Empty).Trim().ToUpper();
            SCity = Regex.Replace(SCity, @"\s", string.Empty).Trim().ToUpper();
            ECity = Regex.Replace(ECity, @"\s", string.Empty).Trim().ToUpper();

            string originalCmdResult = cmdResult;

            // 根据大编码号获取票号指令（即：解析【DETR:CN/{0},C】指令）返回结果验证。
            // 若验证通过，对于第2种返回结果而言，还返回了航班信息。
            if (!ValidCmdResult(cmdResult))
            {
                _response.error.CmdResultBag = originalCmdResult;
                return _response;
            }

            // 结果：
            // 例3：DETR:CN/NK9Y8G,CDETR:TN/781-2180622791NAME:斯坎迪尔穆提拉FOID:NI653121199401031919MU5633/10JAN15/URCKHGOPENDETR:TN/999-1952037851NAME:徐速FOID:RP4668562276CA1303/09DEC14/PEKSZXFLOW
            // 例1：DETR:CN/PCZ0SX,CISSUEDBY:HAINANAIRLINESORG/DST:SIA/NKGARL-DE/R:不得签转TOURCODE:PASSENGER:茅威涛EXCH:CONJTKT:OFM:1XIYHU7639M31DEC1340OKM20KOPENFORUSET2--RL:PCZ0SX/TO:NKGFC:31DEC14XIYHUNKG760.00CNY760.00ENDFARE:CNY760.00|FOP:CASH(CNY)TAX:CNY50.00CN|OI:TAX:CNY60.00YQ|-TOTAL:CNY870.00|TKTN:880-2323065499           
            string data = Regex.Replace(cmdResult, @"\r|\n|\s+|\+", string.Empty).Trim();

            string findData = string.Empty;
            JetermEntity.Passenger passenger;
            _response.result.Price = new JetermEntity.Price();
            _response.result.Price.FacePrice = 0;
            _response.result.Price.Tax = 0;
            _response.result.Price.Fuel = 0;
            _response.result.Price.TotalPrice = 0;

            #region 解析第1种情况的命令返回

            if (data.IndexOf(@"DETR:TN/") != -1)
            {
                /*
                结果，如：
                recordList[0]--DETR:CN/NK9Y8G,C                                                              

                recordList[1]--999-2370208996 	             NAME: 齐珂                                 
    FOID:FFCA002609609114/C                 CA1254 /23FEB15/URCPEK FLOW        

                recordList[2]--999-2370208995 	             NAME: 黄凯                                 
    FOID:RP1162487782                       CA1254 /23FEB15/URCPEK FLOW   
                 */

                cmdResult = cmdResult.Replace("?", string.Empty).Replace("▪", string.Empty);

                // 获得航班信息：
                MatchFlightInfo(cmdResult);          

                // 获得获得客票状态、根据客票状态来决定是否设置永久缓存、乘机人的身份证号：

                // pattern放的是如：FOID:\s*(\S*)\s*MU5767\s*/\s*(\S+)\s*/\s*KMGNNG\s*(\S+)
                string pattern = string.Empty;
                pattern = string.Format(@"FOID:\s*(?'CardNo'\S*)\s*{0}\s*/\s*(?'DepDate'\S+)\s*/\s*{1}{2}\s*(?'TicketStatus'\S+)", FlightNo, SCity, ECity);
                Regex regex = new Regex(pattern);
                MatchCollection matchs = regex.Matches(cmdResult);
                if (matchs == null || matchs.Count < 1)
                {
                    _response.error = new Error(EtermCommand.ERROR.NO_MATCHED_RECORD);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                _response.result.PassengerList = new List<JetermEntity.Passenger>();
              
                foreach (Match match in matchs)
                {
                    // 获得身份证号
                    passenger = new JetermEntity.Passenger();

                    string cardNo = match.Groups["CardNo"].Captures[0].Value;
                    Regex regex1 = new Regex(@"NI\w{1,}");
                    if (regex1.IsMatch(cardNo))
                    {
                        passenger.idtype = EtermCommand.IDtype.IDcard;
                        passenger.cardno = Regex.Replace(regex1.Match(cardNo).Value, "NI", string.Empty).Trim();
                    }

                    _response.result.PassengerList.Add(passenger);                   

                    // 获得客票状态 以及 根据客票状态来决定是否设置永久缓存
                    if (_response.SaveTime != EtermCommand.CacheTime.infinite)
                    {
                        string ticketStatus = match.Groups["TicketStatus"].Captures[0].Value;
                        EtermCommand.TicketStatus ticketStatusResult = EtermCommand.TicketStatus.NotSet;
                        _response.SaveTime = ParserHelper.GetTicketStatusAndSaveTime(ticketStatus, out ticketStatusResult);
                        _response.result.TicketStatus = ticketStatusResult;
                    }
                }

                if (_response.result.PassengerList.Count < 1)
                {
                    _response.error = new Error(EtermCommand.ERROR.NO_MATCHED_RECORD);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }
             
                // 获得乘机人的姓名和其票号：
                string[] recordList = Regex.Split(cmdResult, @"DETR:TN/");            

                int i = -1;
                foreach (string record in recordList)
                {
                    /*
record记录的值如：
781-2191996969 ?             NAME: 肖苏城                                
FOID:NI450221197705161938               MU5757 /20MAY15/DLUKMG OPEN        
FOID:NI450221197705161938               MU5767 /20MAY15/KMGNNG OPEN        
?
                     
或如：
826-9288796767 ▪             NAME: 李石山                                
    FOID:PF13760312136                      GS7489 /02JUN15/URCKRL OPEN        
▶
     */
                    if (string.IsNullOrWhiteSpace(record))
                    {
                        continue;
                    }                    
                  
                    matchs = regex.Matches(record);
                    if (matchs == null || matchs.Count < 1)
                    {
                        continue;
                    }

                    ++i;
                    if (i < _response.result.PassengerList.Count)
                    {
                        pattern = @"(\d+-*\d+)\s*\S*\s*NAME:\s*(\S*)";
                        Match match = Regex.Match(record, pattern);
                        // 获得票号
                        _response.result.PassengerList[i].TicketNo = match.Groups[1].Value.Replace("-", string.Empty);
                        // 获得乘机人姓名
                        _response.result.PassengerList[i].name = match.Groups[2].Value;  
                    }              
                }

                _response.state = true;
                return _response;
            }

            #endregion

            #region 解析第2种情况的命令返回

            if (data.IndexOf("PASSENGER:") != -1)
            {
                string originalData = cmdResult;

                // 思路：把文本转换为key/value键值对格式，一对键值对为一行            
                cmdResult = cmdResult.Replace("ORG/DST:", "\r\nORG/DST:");
                cmdResult = cmdResult.Replace("CONJ TKT:", "\r\nCONJ TKT:");
                cmdResult = cmdResult.Replace("BG:", "\r\nBG:");
                cmdResult = cmdResult.Replace("BN:", "\r\nBN:");
                cmdResult = cmdResult.Replace("|FOP:", "\r\nFOP:");
                cmdResult = cmdResult.Replace("|OI:", "\r\nOI:");
                cmdResult = cmdResult.Replace("|TKTN:", "\r\nTKTN:");
                // 客票类型比较特殊：返回的文本中ISI（客票类型）不是键值对形式，故通过正则表达式，将ISI转换到单独的一行
                Regex reg = new Regex(@"(ORG/DST:\s*[\S]*\s*)([\S]*)");
                cmdResult = reg.Replace(cmdResult, "$1\r\nISI:$2");
                // 转换以后的文本，就是一行为一对键值对，根据换行符分割
                IList<string> lines = cmdResult.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                string[] dic = null;
                string key = string.Empty;
                string value = string.Empty;
                IList<string> valueList;

                // start: 解析Eterm命令返回结果                

                passenger = new JetermEntity.Passenger();
                string priceValue = string.Empty;

                foreach (string line in lines)
                {
                    if (!line.Contains(":"))// 检查该行是否是有效键值对
                    {
                        continue;
                    }

                    dic = line.Split(':');
                    if (dic.Length != 2)
                    {
                        continue;
                    }
                    key = dic[0].Trim();
                    value = dic[1].Trim();

                    if (key.Equals("PASSENGER") || key.Contains("SSENGER")) // 获得乘客姓名
                    {
                        passenger.name = value ?? string.Empty;
                        continue;
                    }                    

                    if (key.Equals("FARE") && !string.IsNullOrWhiteSpace(value)) // 获得票面价格（单位：CNY）
                    {
                        priceValue = Regex.Match(value, @"\d+\.\d{2}").Value.Trim();
                        if (!string.IsNullOrWhiteSpace(priceValue))
                        {
                            _response.result.Price.FacePrice = decimal.Parse(priceValue);
                        }

                        continue;
                    }

                    if (key.EndsWith("FARE PD") && !string.IsNullOrWhiteSpace(value) && !value.StartsWith("CNY")) // 获得票面价格（单位：非CNY）
                    {
                        priceValue = Regex.Match(value, @"\d+\.\d{2}").Value.Trim();
                        if (!string.IsNullOrWhiteSpace(priceValue))
                        {
                            _response.result.Price.FacePrice = decimal.Parse(priceValue);
                        }

                        continue;
                    }

                    if (key.Equals("TAX") && !string.IsNullOrWhiteSpace(value)) // 获得税或燃油费
                    {
                        priceValue = Regex.Match(value, @"\d+\.\d{2}").Value.Trim();
                        if (!string.IsNullOrWhiteSpace(priceValue))
                        {
                            if (value.Contains("YQ"))
                            {
                                _response.result.Price.Fuel = decimal.Parse(priceValue); // 获得燃油费
                                continue;
                            }

                            _response.result.Price.Tax = decimal.Parse(priceValue);  // 获得税                        
                        }

                        continue;
                    }

                    if (key.Equals("TOTAL") && !string.IsNullOrWhiteSpace(value)) // 获得总价
                    {
                        priceValue = Regex.Match(value, @"\d+\.\d{2}").Value.Trim();
                        if (!string.IsNullOrWhiteSpace(priceValue))
                        {
                            _response.result.Price.TotalPrice = decimal.Parse(priceValue);
                        }

                        continue;
                    }

                    if (key.Equals("TKTN")) // 获得票号/编码
                    {
                        passenger.TicketNo = (value ?? string.Empty).Replace("-", string.Empty);
                        continue;
                    }
                }

                _response.result.PassengerList = new List<JetermEntity.Passenger>();
                _response.result.PassengerList.Add(passenger);

                // end: 解析Eterm命令返回结果

                _response.state = true;
                return _response;
            }

            #endregion

            return _response;
        }

        #region Helper

        protected internal override bool ValidRequest()
        {
            if (_request == null || string.IsNullOrWhiteSpace(_request.BigPnr) || string.IsNullOrWhiteSpace(_request.FlightNo) || string.IsNullOrWhiteSpace(_request.SCity) || string.IsNullOrWhiteSpace(_request.ECity))
            {
                _response.error = new Error(EtermCommand.ERROR.EMPTY_REQUEST_PARAM);
                return false;
            }

            _request.BigPnr = Regex.Replace(_request.BigPnr, @"\s", string.Empty).Trim().ToUpper();
            _request.FlightNo = Regex.Replace(_request.FlightNo, @"\s", string.Empty).Trim().ToUpper();
            _request.SCity = Regex.Replace(_request.SCity, @"\s", string.Empty).Trim().ToUpper();
            _request.ECity = Regex.Replace(_request.ECity, @"\s", string.Empty).Trim().ToUpper();

            return true;
        }

        protected internal override bool ValidCmdResult(string cmdResult)
        {
            if (string.IsNullOrWhiteSpace(cmdResult))
            {
                _response.error = new Error(EtermCommand.ERROR.DETR_CN_C_COMMAND_RESULT_EMPTY);
                return false;
            }

            if (cmdResult.Contains("TICKET NOT FOUND"))
            {
                _response.error = new Error(EtermCommand.ERROR.TICKET_NOT_FOUND);
                return false;
            }

            // 结果：
            // 例3：DETR:CN/NK9Y8G,CDETR:TN/781-2180622791NAME:斯坎迪尔穆提拉FOID:NI653121199401031919MU5633/10JAN15/URCKHGOPENDETR:TN/999-1952037851NAME:徐速FOID:RP4668562276CA1303/09DEC14/PEKSZXFLOW
            // 例1：DETR:CN/PCZ0SX,CISSUEDBY:HAINANAIRLINESORG/DST:SIA/NKGARL-DE/R:不得签转TOURCODE:PASSENGER:茅威涛EXCH:CONJTKT:OFM:1XIYHU7639M31DEC1340OKM20KOPENFORUSET2--RL:PCZ0SX/TO:NKGFC:31DEC14XIYHUNKG760.00CNY760.00ENDFARE:CNY760.00|FOP:CASH(CNY)TAX:CNY50.00CN|OI:TAX:CNY60.00YQ|-TOTAL:CNY870.00|TKTN:880-2323065499           
            string data = Regex.Replace(cmdResult, @"\r|\n|\s+|\+", string.Empty).Trim();

            // 验证第1种指令返回结果
            if (data.IndexOf(@"DETR:TN/") != -1)
            {
                string[] recordList = Regex.Split(cmdResult, @"DETR:TN/");
                if (recordList == null || !recordList.Any())
                {
                    _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT);
                    return false;
                }

                return true;
            }

            // 验证第2种指令返回结果
            if (data.IndexOf("PASSENGER:") != -1)
            {
                // start: 检查Eterm命令返回结果cmdResult，是否分别与航班号、出发城市三字码以及到达城市三字码这3个请求参数相同 + 获得航班信息

                // 思路：把文本转换为key/value键值对格式，一对键值对为一行            
                cmdResult = cmdResult.Replace("ORG/DST:", "\r\nORG/DST:");
                cmdResult = cmdResult.Replace("CONJ TKT:", "\r\nCONJ TKT:");
                cmdResult = cmdResult.Replace("BG:", "\r\nBG:");
                cmdResult = cmdResult.Replace("BN:", "\r\nBN:");
                cmdResult = cmdResult.Replace("|FOP:", "\r\nFOP:");
                cmdResult = cmdResult.Replace("|OI:", "\r\nOI:");
                cmdResult = cmdResult.Replace("|TKTN:", "\r\nTKTN:");
                // 客票类型比较特殊：返回的文本中ISI（客票类型）不是键值对形式，故通过正则表达式，将ISI转换到单独的一行
                Regex reg = new Regex(@"(ORG/DST:\s*[\S]*\s*)([\S]*)");
                cmdResult = reg.Replace(cmdResult, "$1\r\nISI:$2");
                // 转换以后的文本，就是一行为一对键值对，根据换行符分割
                IList<string> lines = cmdResult.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                string[] dic = null;
                string key = string.Empty;
                string value = string.Empty;
                IList<string> valueList;

                string fm = string.Empty;
                try
                {           
                    fm = lines.Where<string>(line => line.Contains("FM:")).SingleOrDefault();                    
                }
                catch
                {
                    _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT);
                    return false;
                }

                dic = fm.Split(':');
                key = dic[0].Trim();
                value = dic[1].Trim();

                if (string.IsNullOrWhiteSpace(value))
                {
                    _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT_FM);
                    return false;
                }

                valueList = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (valueList.Count < 3)
                {
                    _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT);
                    return false;
                }

                if (valueList[0].Length < 3)
                {
                    _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT_SCITY);
                    return false;
                }

                string to = string.Empty;
                try
                {
                    to = lines.Where<string>(line => line.Trim().StartsWith("TO:")).SingleOrDefault();
                }
                catch
                {
                    _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT);
                    return false;
                }

                dic = to.Split(':');
                key = dic[0].Trim();
                value = dic[1].Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT_TO);
                    return false;
                }                

                List<string> nextFmList = lines.Where<string>(line => !line.Trim().StartsWith("TO:") && line.Contains("TO:")).ToList<string>();
                if (nextFmList.Count > 0)
                {
                    for (int i = 0; i < nextFmList.Count; ++i)
                    {
                        string line = nextFmList[i];

                        dic = line.Split(':');
                        key = dic[0].Trim();
                        value = dic[1].Trim();

                        if (string.IsNullOrWhiteSpace(value))
                        {
                            _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT_FM);
                            return false;
                        }

                        valueList = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (valueList.Count < 3)
                        {
                            _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT);
                            return false;
                        }
                        if (valueList[0].Length < 3)
                        {
                            _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT_SCITY);
                            return false;
                        }
                    }
                }

                List<string> rlList = lines.Where<string>(line => line.Contains("RL:")).ToList<string>();
                if (rlList.Count < 1)
                {
                    _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT_RL);
                    return false;
                }
                if (rlList.Count != (1 + nextFmList.Count))
                {
                    _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT_RL);
                    return false;
                }
               
                dic = fm.Split(':');
                key = dic[0].Trim();
                value = dic[1].Trim();
                valueList = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                string[] toDic = null;
                string toKey = string.Empty;
                string toValue = string.Empty;

                string[] nextFmDic = null;
                string nextFmKey = string.Empty;
                string nextFmValue = string.Empty;
                IList<string> nextFmValueList = null;

                EtermCommand.TicketStatus ticketStatusResult = EtermCommand.TicketStatus.NotSet;

                Flight flight = null;
                _response.result.FlightList = new List<Flight>();

                if (valueList[0].Substring(1).Equals(SCity) && string.Format("{0}{1}", valueList[1], valueList[2]).Equals(FlightNo))
                {
                    // 是否符合到达城市三字码查询条件 

                    if (nextFmList.Count == 0)
                    {
                        toDic = to.Split(':');                      
                        toValue = toDic[1].Trim();
                        if (!toValue.Equals(ECity))
                        {
                            _response.error = new Error(EtermCommand.ERROR.NO_MATCHED_RECORD);
                            return false;
                        }

                        // 获得客票状态 以及 根据客票状态来决定是否设置永久缓存  
                        ticketStatusResult = EtermCommand.TicketStatus.NotSet;
                        _response.SaveTime = ParserHelper.GetTicketStatusAndSaveTime(value, out ticketStatusResult);
                        _response.result.TicketStatus = ticketStatusResult;

                        // 获得航班信息：出发城市三字码、航司、航班号、舱位、起飞日期、到达城市三字码、出发航站楼、到达航站楼
                        flight = new Flight();                        
                        SetFlightInfo(value, flight, toValue, rlList[0]);
                        _response.result.FlightList.Add(flight);                        

                        return true;
                    }

                    nextFmDic = nextFmList[0].Split(':');
                    nextFmValue = nextFmDic[1].Trim();
                    nextFmValueList = nextFmValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (!nextFmValueList[0].Substring(1).Equals(ECity))
                    {
                        _response.error = new Error(EtermCommand.ERROR.NO_MATCHED_RECORD);
                        return false;
                    }

                    // 获得客票状态 以及 根据客票状态来决定是否设置永久缓存
                    ticketStatusResult = EtermCommand.TicketStatus.NotSet;
                    _response.SaveTime = ParserHelper.GetTicketStatusAndSaveTime(value, out ticketStatusResult);
                    _response.result.TicketStatus = ticketStatusResult;

                    // 获得航班信息：出发城市三字码、航司、航班号、舱位、起飞日期、到达城市三字码、出发航站楼、到达航站楼
                    flight = new Flight();                    
                    SetFlightInfo(value, flight, nextFmValueList[0].Substring(1), rlList[0]);
                    _response.result.FlightList.Add(flight);

                    return true;
                }

                if (nextFmList.Count == 0)
                {
                    _response.error = new Error(EtermCommand.ERROR.NO_MATCHED_RECORD);
                    return false;
                }

                for (int i = 0; i < nextFmList.Count; ++i)
                {
                    string line = nextFmList[i];

                    dic = line.Split(':');
                    key = dic[0].Trim();
                    value = dic[1].Trim();
                    valueList = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                   
                    if (valueList[0].Substring(1).Equals(SCity) && string.Format("{0}{1}", valueList[1], valueList[2]).Equals(FlightNo))
                    {
                        if (i == nextFmList.Count - 1)
                        {                          
                            toDic = to.Split(':');
                            toValue = toDic[1].Trim();
                            if (!toValue.Equals(ECity))
                            {
                                _response.error = new Error(EtermCommand.ERROR.NO_MATCHED_RECORD);
                                return false;
                            }

                            // 获得客票状态 以及 根据客票状态来决定是否设置永久缓存
                            ticketStatusResult = EtermCommand.TicketStatus.NotSet;
                            _response.SaveTime = ParserHelper.GetTicketStatusAndSaveTime(value, out ticketStatusResult);
                            _response.result.TicketStatus = ticketStatusResult;

                            // 获得航班信息：出发城市三字码、航司、航班号、舱位、起飞日期、到达城市三字码、出发航站楼、到达航站楼
                            flight = new Flight();
                            SetFlightInfo(value, flight, toValue, rlList[i + 1]);
                            _response.result.FlightList.Add(flight);

                            return true;
                        }

                        nextFmDic = nextFmList[i + 1].Split(':');
                        nextFmValue = nextFmDic[1].Trim();
                        nextFmValueList = nextFmValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (!nextFmValueList[0].Substring(1).Equals(ECity))
                        {
                            _response.error = new Error(EtermCommand.ERROR.NO_MATCHED_RECORD);
                            return false;
                        }

                        // 获得客票状态 以及 根据客票状态来决定是否设置永久缓存
                        ticketStatusResult = EtermCommand.TicketStatus.NotSet;
                        _response.SaveTime = ParserHelper.GetTicketStatusAndSaveTime(value, out ticketStatusResult);
                        _response.result.TicketStatus = ticketStatusResult;

                        // 获得航班信息：出发城市三字码、航司、航班号、舱位、起飞日期、到达城市三字码、出发航站楼、到达航站楼
                        flight = new Flight();
                        SetFlightInfo(value, flight, nextFmValueList[0].Substring(1), rlList[i + 1]);
                        _response.result.FlightList.Add(flight);

                        return true;
                    }
                }

                _response.error = new Error(EtermCommand.ERROR.NO_MATCHED_RECORD);
                return false;

                // end: 检查Eterm命令返回结果cmdResult，是否分别与航班号、出发城市三字码以及到达城市三字码这3个请求参数相同 + 获得航班信息            
            }

            _response.error = new Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT);
            return false;
        }

        /// <summary>
        /// 获得第1种返回结果中的航班信息
        /// </summary>
        /// <param name="detrInfo"></param>   
        /// <returns></returns>
        private void MatchFlightInfo(string detrInfo)
        {
            Regex regex = new Regex(@"FOID:\S+\s*(?'FlightNo'\S+)\s*/(?'Date'\S+)/(?'StartCity'[A-Z]{3})(?'EndCity'[A-Z]{3})");
            var matchs = regex.Matches(detrInfo);
            List<JetermEntity.Flight> flightList = new List<JetermEntity.Flight>();
            JetermEntity.Flight flight;
            foreach (Match match in matchs)
            {
                string flightNo = match.Groups["FlightNo"].Captures[0].Value;
                string sCity = match.Groups["StartCity"].Captures[0].Value;
                string eCity = match.Groups["EndCity"].Captures[0].Value;

                if (!flightNo.Equals(FlightNo) || !sCity.Equals(SCity) || !eCity.Equals(ECity))
                {
                    continue;
                }

                flight = new JetermEntity.Flight();
                flight.FlightNo = match.Groups["FlightNo"].Captures[0].Value;
                if (!string.IsNullOrWhiteSpace(flight.FlightNo) && flight.FlightNo.Length > 2)
                {
                    flight.Airline = flight.FlightNo.Substring(0, 2);
                }
                // 传入格式如：12DEC14
                flight.DepDate = ParserHelper.ConvertStringToDateTime(match.Groups["Date"].Captures[0].Value);
                flight.SCity = match.Groups["StartCity"].Captures[0].Value;
                flight.ECity = match.Groups["EndCity"].Captures[0].Value;
                flightList.Add(flight);

                _response.result.FlightList = flightList;
                return;
            }

            return;
        }

        private void SetFlightInfo(string value, Flight flight)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            string[] v = Regex.Split(value, @"\s+");

            // 获得出发城市三字码
            flight.SCity = v[0].Length != 3 ? v[0].Substring(1) : v[0];

            // 获得航司
            flight.Airline = v.Length > 1 ? v[1] : string.Empty;

            // 获得航班号
            flight.FlightNo = v.Length > 2 ? string.Format("{0}{1}", v[1], v[2]) : string.Empty;

            // 获得舱位
            flight.Cabin = v.Length > 3 ? v[3] : string.Empty;

            // 获得起飞日期
            string depStr = string.Empty;
            string timeStr = string.Empty;
            depStr = v.Length > 4 ? v[4] : string.Empty;
            if (!string.IsNullOrWhiteSpace(depStr))
            {
                timeStr = v.Length > 5 ? v[5] : string.Empty;
                depStr = string.Format("{0}{1}", depStr, timeStr);
            }
            flight.DepDate = ParserHelper.ConvertStringToDateTime(depStr);
        }

        private void SetFlightInfo(string value, Flight flight, string eCity, string rlListValue)
        {
            // 获得航班信息：出发城市三字码、航司、航班号、舱位、起飞日期
            SetFlightInfo(value, flight);

            // 获得到达城市三字码
            flight.ECity = eCity;

            // 获得出发航站楼、到达航站楼
            string[] dic = rlListValue.Split(':');
            string key = dic[0].Trim();
            string[] k = Regex.Split(key, @"\s+");
            if (k.Length > 1)
            {
                ParserHelper.GetTerminal(k[0], flight);
            }
        }

        #endregion
    }
}
