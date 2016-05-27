using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JetermEntity.Parser
{
    [Serializable]
    public class AVH : ParserBase<JetermEntity.Request.AVH, CommandResult<JetermEntity.Response.AVH>>
    {
        #region 成员变量

        private JetermEntity.Request.AVH _request = null;

        private CommandResult<JetermEntity.Response.AVH> _response = null;

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.AVH> Response { get { return this._response; } }

        #endregion

        public AVH(string config, string officeNo)
        {
            _response = new CommandResult<JetermEntity.Response.AVH>();
            _response.result = new JetermEntity.Response.AVH();
            _response.config = config;
            _response.OfficeNo = officeNo;
        }

        /// <summary>
        /// 获得AVH指令返回结果
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回AVH指令（即：【AVH/{出发城市三字码}{到达城市三字码}/{起飞日期}/{航司}】指令）；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.AVH request)
        {
            if (string.IsNullOrWhiteSpace(request.Airline) && !string.IsNullOrWhiteSpace(request.FlightNo) && request.FlightNo.Length > 2)
            {
                request.Airline = request.FlightNo.ToUpper().Trim().Substring(0, 2);
            }
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }

            string dtStr = ParserHelper.ConvertDateTimeToString(request.DepDate);            
            return string.Format("AVH/{0}{1}/{2}{3}/D", request.SCity, request.ECity, dtStr.Substring(0, 5), (string.IsNullOrWhiteSpace(request.Airline) ? string.Empty : string.Format("/{0}", request.Airline)));
        }

        /// <summary>
        /// 解析AVH指令（即：【AVH/{出发城市三字码}{到达城市三字码}/{起飞日期}/{航司}】指令）返回结果
        /// </summary>
        /// <param name="cmdResult">AVH指令（即：【AVH/{出发城市三字码}{到达城市三字码}/{起飞日期}/{航司}】指令）返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.AVH> ParseCmdResult(string cmdResult)
        {
            string originalCmdResult = cmdResult;

            if (!ValidRequest())
            {
                _response.error.CmdResultBag = originalCmdResult;
                return _response;
            }

            // 获得AVH指令原始返回结果：
            _response.result.ResultBag = originalCmdResult;

            // 获得起飞日期：
            _response.result.DepDate = _request.DepDate;

            cmdResult = cmdResult.ToUpper();

            if (!ValidCmdResult(cmdResult))
            {
                _response.error.CmdResultBag = originalCmdResult;
                return _response;
            }

            string regExpression = string.Empty;
            // 格式如：25AUG15
            string dtStr = ParserHelper.ConvertDateTimeToString(_request.DepDate);
            // 格式如：25AUG
            string dtStr1 = dtStr.Substring(0, 5);

            // 匹配出发时间、到达时间和机型这样的信息（如：【2105   2240   73L 0^   E】）：            
            string regExpression1 = @"(\d{4,4}|\d{4,4}\+1)\s+(\d{4,4}|\d{4,4}\+1)\s+(\S+)\s+(\S+)(\s\S)*\s+(E)";
            // 匹配出发航站楼、到达航站楼和飞行总时长：            
            string regExpression2 = @"\s(\S\S)\s(\S\S)\s+(\d+:\d+)";
            // 匹配出发城市三字码、到达城市三字码：
            string regExpression3 = @"\s(\S{6,6})\s";

            cmdResult = cmdResult.Replace("\r", "\r\n");

            try
            {
                // 去每页的头：               
                // 正则：09JUN.+DIRECT ONLY\s+
                regExpression = string.Format(@"{0}.+DIRECT ONLY\s+", dtStr1);
                cmdResult = Regex.Replace(cmdResult, regExpression, string.Empty);

                // 去每页的尾：
                regExpression = @"\S\*{2,2}.+";
                cmdResult = Regex.Replace(cmdResult, regExpression, string.Empty);

                // 去除DS#或DS!或『AS#』：
                regExpression = @"\S{0,1}\SS#\S{0,1}|\S{0,1}\SS!\S{0,1}";
                cmdResult = Regex.Replace(cmdResult, regExpression, string.Empty);

                // 航班号正则，格式如：1-  *KN5988，但不能匹配【** M1A H1A K13 】中的【1A K13】这种字符串
                regExpression = @"\d\-{0,1}\s+\*{0,1}[A-Z]{2,2}\d{2,}|\d\-{0,1}\s+\*{0,1}\d[A-Z]\d{2,}|\d\-{0,1}\s+\*{0,1}[A-Z]\d\d{2,}|\d\+{0,1}\s+\*{0,1}[A-Z]{2,2}\d{2,}|\d\+{0,1}\s+\*{0,1}\d[A-Z]\d{2,}|\d\+{0,1}\s+\*{0,1}[A-Z]\d\d{2,}";
                string[] recordList = Regex.Split(cmdResult, regExpression);
                if (recordList == null || recordList.Length < 2)
                {
                    _response.error = new Error(EtermCommand.ERROR.INVALID_AVH_RESULT);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                MatchCollection mc = Regex.Matches(cmdResult, regExpression);
                for (int i = 1; i < recordList.Length; ++i)
                {
                    if ((i - 1) >= mc.Count)
                    {
                        _response.error = new Error(EtermCommand.ERROR.INVALID_AVH_RESULT);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }
                    JetermEntity.Response.AVHSingle avhSingle = new JetermEntity.Response.AVHSingle();

                    // 获得是否是共享航班：
                    string flightNo = mc[i - 1].Value;
                    if (flightNo.IndexOf("*") > -1)
                    {
                        avhSingle.ShareFlight = true;
                    }

                    // 获得序号：
                    int n = -1;
                    int.TryParse(Regex.Match(flightNo, @"^\d").Value, out n);
                    avhSingle.Number = n;

                    // 获得航班号：
                    avhSingle.FlightNo = Regex.Replace(flightNo, @"^\d\-{0,1}\s+\*{0,1}|^\d\+{0,1}\s+\*{0,1}", string.Empty);

                    string record = recordList[i];
                    regExpression = string.Format("{0}{1}", @"\d\-{0,1}\s+\*{0,1}", avhSingle.FlightNo);
                    record = Regex.Replace(record, regExpression, string.Empty);
                    regExpression = string.Format("{0}{1}", @"\d\+{0,1}\s+\*{0,1}", avhSingle.FlightNo);
                    record = Regex.Replace(record, regExpression, string.Empty);

                    // 获得共享航班号列表
                    regExpression = @"[A-Z]{2,2}\d{2,}|\d[A-Z]\d{2,}|[A-Z]\d\d{2,}";
                    MatchCollection shareFLMatchList = Regex.Matches(record, regExpression);
                    if (shareFLMatchList != null && shareFLMatchList.Count > 0)
                    {
                        foreach (Match m in shareFLMatchList)
                        {
                            avhSingle.ShareFltNoList.Add(m.Value);
                        }
                    }
                    record = Regex.Replace(record, regExpression, string.Empty);

                    // 获得出发时间：
                    // 获得到达时间：
                    // 获得机型：
                    Regex reg = new Regex(regExpression1);
                    Match match = reg.Match(record);
                    if (match != null && match.Groups != null && match.Groups.Count > 0)
                    {
                        avhSingle.STime = match.Groups[1].Value;
                        avhSingle.ETime = match.Groups[2].Value;
                        avhSingle.FlightModel = match.Groups[3].Value;
                    }

                    record = Regex.Replace(record, regExpression1, string.Empty);

                    // 获得出发航站楼：
                    // 获得到达航站楼：
                    // 获得飞行时长：
                    reg = new Regex(regExpression2);
                    match = reg.Match(record);
                    if (match != null && match.Groups != null && match.Groups.Count > 0)
                    {
                        avhSingle.DepTerminal = match.Groups[1].Value;
                        avhSingle.ArrTerminal = match.Groups[2].Value;
                        avhSingle.FltDuration = match.Groups[3].Value;
                    }

                    record = Regex.Replace(record, regExpression2, string.Empty);

                    // 获得出发城市三字码：
                    // 获得到达城市三字码：
                    reg = new Regex(regExpression3);
                    match = reg.Match(record);
                    if (match != null && match.Groups != null && match.Groups.Count > 0)
                    {
                        string v = match.Groups[1].Value;
                        avhSingle.SCity = v.Substring(0, 3);
                        avhSingle.ECity = v.Substring(3);
                    }

                    record = Regex.Replace(record, regExpression3, string.Empty);

                    _response.result.AVHList.Add(avhSingle);

                    // 获得舱位剩余可订数：

                    MatchCollection mcAVNumber = Regex.Matches(record, @"(\S{2,3}\s)");
                    if (mcAVNumber == null || mcAVNumber.Count < 1)
                    {
                        //if (i == 1)
                        //{
                        //    _response.error = new Error(EtermCommand.ERROR.NO_PERMISSION_CHECK_AVNUMBER);
                        //    _response.error.CmdResultBag = originalCmdResult;
                        //    return _response;
                        //}
                        //else
                        //{
                        continue;
                        //}
                    }

                    for (int j = 0; j < mcAVNumber.Count; ++j)
                    {
                        string value = mcAVNumber[j].Value.Trim();
                        if (string.IsNullOrWhiteSpace(value) || value.Contains("**") || Regex.IsMatch(value, @"\d+\+|\d+-") || (value.Length != 2 && value.Length != 3))
                        {
                            continue;
                        }

                        CarbinNumber carbinNumber = new CarbinNumber();
                        if (value.Length == 2)
                        {
                            carbinNumber.Cabin = value.Substring(0, 1);
                            carbinNumber.NumTag = value.Substring(1);
                        }
                        else if (value.Length == 3)
                        {
                            carbinNumber.Cabin = value.Substring(0, 2);
                            carbinNumber.NumTag = value.Substring(2);
                        }

                        carbinNumber.NumStr = "0";
                        if (carbinNumber.NumTag.Equals("A"))
                        {
                            carbinNumber.NumStr = carbinNumber.NumTag;
                        }
                        else
                        {
                            int number = 0;
                            if (int.TryParse(carbinNumber.NumTag, out number))
                            {
                                carbinNumber.NumStr = number.ToString();
                            }
                        }

                        avhSingle.CarbinNumList.Add(carbinNumber);
                    }
                }
            }
            catch
            {
                _response.error = new Error(EtermCommand.ERROR.PARSE_FAIL);
                _response.error.CmdResultBag = originalCmdResult;
                return _response;
            }

            if (_response.result.AVHList.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(_request.FlightNo))
                {
                    _response.result.AVHList = _response.result.AVHList.Where<AVHSingle>(avh => avh.FlightNo.Equals(_request.FlightNo)).ToList<AVHSingle>();
                }

                if (!string.IsNullOrWhiteSpace(_request.Carbin) && _response.result.AVHList.Count > 0)
                {
                    foreach (AVHSingle avh in _response.result.AVHList)
                    {
                        avh.CarbinNumList = avh.CarbinNumList.Where<CarbinNumber>(carbin => carbin.Cabin.Equals(_request.Carbin)).ToList<CarbinNumber>();
                    }
                }
            }            

            _response.state = true;
            return _response;
        }

        protected internal override bool ValidRequest()
        {
            if (_request == null
                || string.IsNullOrWhiteSpace(_request.SCity)
                || string.IsNullOrWhiteSpace(_request.ECity)
                || _request.DepDate == DateTime.MinValue.Date
               )
            {
                _response.error = new Error(EtermCommand.ERROR.EMPTY_REQUEST_PARAM);
                return false;
            }

            // 假设当前日期是2015年7月8日，则只能查到起飞日期在2015年7月7日到2016年7月6日期间的舱位可订数
            DateTime nowDt = DateTime.Now.Date;
            DateTime startDt = nowDt.AddDays(-1);
            DateTime endDt = nowDt.AddDays(-2).AddYears(1);
            if (startDt > _request.DepDate.Date)
            {
                _response.error = new Error(EtermCommand.ERROR.NO_CHECK_HISTORY_AVNUMBER);
                return false;
            }
            if (endDt < _request.DepDate.Date)
            {
                _response.error = new Error(EtermCommand.ERROR.ONLY_CHECK_WITHINONEYEAR_AVNUMBER);
                return false;
            }

            _request.SCity = Regex.Replace(_request.SCity, @"\s", string.Empty).Trim().ToUpper();
            _request.ECity = Regex.Replace(_request.ECity, @"\s", string.Empty).Trim().ToUpper();
            if (!string.IsNullOrWhiteSpace(_request.Airline))
            {
                _request.Airline = Regex.Replace(_request.Airline, @"\s", string.Empty).Trim().ToUpper();
            }

            if (_request.SCity.Length != 3 || _request.ECity.Length != 3 || (!string.IsNullOrWhiteSpace(_request.Airline) && _request.Airline.Length != 2))
            {
                _response.error = new Error(EtermCommand.ERROR.INVALID_AVH_REQUEST_PARAM);
                return false;
            }

            string dtStr = ParserHelper.ConvertDateTimeToString(_request.DepDate);
            if (string.IsNullOrWhiteSpace(dtStr))
            {
                _response.error = new Error(EtermCommand.ERROR.INVALID_AVH_REQUEST_PARAM);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(_request.FlightNo))
            {
                _request.FlightNo = Regex.Replace(_request.FlightNo, @"\s", string.Empty).Trim().ToUpper();
            }

            if (!string.IsNullOrWhiteSpace(_request.Carbin))
            {
                _request.Carbin = Regex.Replace(_request.Carbin, @"\s", string.Empty).Trim().ToUpper();
            }

            return true;
        }

        protected internal override bool ValidCmdResult(string cmdResult)
        {
            if (string.IsNullOrWhiteSpace(cmdResult))
            {
                _response.error = new Error(EtermCommand.ERROR.COMMAND_EMPTY);
                return false;
            }

            if (cmdResult.Contains("FORMAT"))
            {
                _response.error = new Error(EtermCommand.ERROR.INVALID_AVH_REQUEST_PARAM);
                return false;
            }

            if (!cmdResult.Contains("1- ") && !cmdResult.Contains("1+ "))
            {
                _response.error = new Error(EtermCommand.ERROR.INVALID_AVH_RESULT);
                return false;
            }

            return true;
        }
    }
}
