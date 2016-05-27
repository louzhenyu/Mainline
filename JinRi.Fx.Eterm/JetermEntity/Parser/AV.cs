using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using JetermEntity.Response;

namespace JetermEntity.Parser
{
    [Serializable]
    public class AV : ParserBase<JetermEntity.Request.AV, CommandResult<JetermEntity.Response.AV>>
    {
        #region 成员变量

        private JetermEntity.Request.AV _request = null;

        private CommandResult<JetermEntity.Response.AV> _response = null;

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.AV> Response { get { return this._response; } }

        #endregion

        public AV(string config, string officeNo)
        {
            _response = new CommandResult<JetermEntity.Response.AV>();
            _response.result = new JetermEntity.Response.AV();
            _response.config = config;
            _response.OfficeNo = officeNo;
        }

        /// <summary>
        /// 获得AV指令返回结果
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回AV指令（即：【AV:{航班号}/{起飞日期}】指令）；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.AV request)
        {
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }

            string dtStr = ParserHelper.ConvertDateTimeToString(_request.DepDate);
            return string.Format("AV:{0}/{1}", request.FlightNo, dtStr.Substring(0, 5));
        }

        /// <summary>
        /// 解析AV指令（即：【AV:{航班号}/{起飞日期}】指令）返回结果
        /// </summary>
        /// <param name="cmdResult">AV指令（即：【AV:{航班号}/{起飞日期}】指令）返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.AV> ParseCmdResult(string cmdResult)
        {
            string originalCmdResult = cmdResult;

            if (!ValidRequest())
            {
                _response.error.CmdResultBag = originalCmdResult;
                return _response;
            }

            // 获得AV指令原始返回结果：
            _response.result.ResultBag = originalCmdResult;

            // 获得起飞日期：
            _response.result.FlightNo = _request.FlightNo;

            // 获得起飞日期：
            _response.result.DepDate = _request.DepDate;

            cmdResult = cmdResult.ToUpper();

            if (!ValidCmdResult(cmdResult))
            {
                _response.error.CmdResultBag = originalCmdResult;
                return _response;
            }

            string regExpression = @"TOTAL JOURNEY TIME\s+\d*:\d+";
            // 获得总飞行时长：
            _response.result.TotalJourneyTime = Regex.Match(cmdResult, regExpression).Value.Replace("TOTAL JOURNEY TIME", string.Empty).Trim();          

            try
            {
                string[] recordList = Regex.Split(cmdResult, regExpression);
                if (recordList == null || recordList.Length != 2)
                {
                    _response.error = new Error(EtermCommand.ERROR.INVALID_AV_RESULT);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                // 以下逻辑实现各航线信息的获得，最终结果存入_response.result.AVList：

                if (Regex.IsMatch(cmdResult, @"\s+OPE\s*"))
                {
                    regExpression = @"DEP.*\s+OPE\s*";
                }
                else if (Regex.IsMatch(cmdResult, @"\s+DISTANCE\s+NWST\s*"))
                {
                    regExpression = @"DEP.*\s+DISTANCE\s+NWST\s*";
                }
                else if (Regex.IsMatch(cmdResult, @"\s+DISTANCE\s*"))
                {
                    regExpression = @"DEP.*\s+DISTANCE\s*";
                }
                else if (Regex.IsMatch(cmdResult, @"\s+MEAL\s*"))
                {
                    regExpression = @"DEP.*\s+MEAL\s*";
                }
                string flightInfo = recordList[0].Trim();
                flightInfo = Regex.Replace(flightInfo, regExpression, string.Empty);               
                string[] flightInfoArr = flightInfo.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (flightInfoArr == null || flightInfoArr.Length < 2)
                {
                    _response.error = new Error(EtermCommand.ERROR.INVALID_AV_RESULT);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                for (int i = 1; i < flightInfoArr.Length; ++i)
                {
                    string flight = flightInfoArr[i].Trim();
                    string[] flightArr = flight.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (flightArr == null || flightArr.Length < 5)
                    {
                        _response.error = new Error(EtermCommand.ERROR.INVALID_AV_RESULT);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }

                    AVSingle av = new AVSingle();
                    // 获得出发城市三字码：
                    av.SCity = flightArr[0];
                    // 获得出发时间：
                    av.STime = flightArr[1];
                    // 获得到达城市三字码：
                    av.ECity = flightArr[2];
                    // 获得到达时间：
                    av.ETime = flightArr[3];
                    // 获得到达时星期几：
                    av.EWeek = flightArr[4];

                    flight = flight.Replace(av.SCity, string.Empty)
                                    .Replace(av.STime, string.Empty)
                                    .Replace(av.ECity, string.Empty)
                                    .Replace(av.ETime, string.Empty)
                                    .Replace(av.EWeek, string.Empty)
                                    .Trim();

                    regExpression = @"\d*:\d+";
                    MatchCollection mc = Regex.Matches(flight, regExpression);
                    if (mc != null && mc.Count > 0)
                    {
                        // 获得该航线的飞行时长：
                        av.FltDuration = mc[0].Value;
                        flight = flight.Replace(av.FltDuration, string.Empty).Trim();

                        // 获得该航线的停飞总时长（可能会没有值）：                        
                        if (mc.Count > 1)
                        {
                            av.Ground = mc[1].Value;
                            flight = flight.Replace(av.Ground, string.Empty).Trim();
                        }
                    }

                    if (string.IsNullOrWhiteSpace(flight))
                    {
                        continue;
                    }

                    // 获得出发航站楼、到达航站楼（可能都有值，或其中1个没有值）：
                    regExpression = @"\S{0,2}/\S{0,2}";
                    string terminal = Regex.Match(flight, regExpression).Value;
                    if (!string.IsNullOrWhiteSpace(terminal))
                    {
                        string[] terminalArr = terminal.Split(new char[] { '/' });
                        av.STerminal = terminalArr[0];
                        av.ETerminal = terminalArr[1];

                        flight = flight.Replace(terminal, string.Empty).Trim();
                    }

                    if (string.IsNullOrWhiteSpace(flight))
                    {
                        continue;
                    }

                    // 获得共享航班号（可能会没有值）：
                    regExpression = @"[A-Z]{2,2}\d{2,}|\d[A-Z]\d{2,}|[A-Z]\d\d{2,}";
                    av.ShareFltNo = Regex.Match(flight, regExpression).Value;
                    if (!string.IsNullOrWhiteSpace(av.ShareFltNo))
                    {
                        av.ShareFlight = true;
                        flight = flight.Replace(av.ShareFltNo, string.Empty).Trim();
                    }

                    if (string.IsNullOrWhiteSpace(flight))
                    {
                        continue;
                    }

                    flightArr = flight.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (flightArr != null && flightArr.Length > 0)
                    {
                        // 获得机型：
                        av.FlightModel = flightArr[0];
                        // 获得Meal（可能会没有值）、飞行总距离（可能会没有值）：
                        switch (flightArr.Length)
                        {
                            case 2:
                                if (Regex.IsMatch(flightArr[1], @"\S") && !Regex.IsMatch(flightArr[1], @"\d"))
                                {
                                    av.Meal = flightArr[1];
                                }
                                else
                                {
                                    av.Distance = flightArr[1];
                                }
                                break;
                            case 3:
                                av.Meal = flightArr[1];
                                av.Distance = flightArr[2];
                                break;
                            default:
                                break;
                        }
                    }

                    _response.result.AVList.Add(av);
                }

                // 以上逻辑实现各航线信息的获得，最终结果存入_response.result.AVList

                // 以下逻辑实现各航线的舱位剩余可订数的获得，最终结果存入AVSingle的CarbinNumList：

                string carbinNumberInfo = recordList[1].Trim();           
                carbinNumberInfo = Regex.Replace(carbinNumberInfo, @"MEMBER\s+OF\s+.*", string.Empty).Trim();

                regExpression = @"\s*\S{6,6}\s*";
                string[] cbNumberInfoArr = Regex.Split(carbinNumberInfo, regExpression);
                if (cbNumberInfoArr == null || cbNumberInfoArr.Length < 1)
                {
                    _response.error = new Error(EtermCommand.ERROR.NOSHOW_CARBIN_NUMBER);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }
                if ((cbNumberInfoArr.Length - 1) != _response.result.AVList.Count)
                {
                    _response.error = new Error(EtermCommand.ERROR.INVALID_AV_RESULT);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                int j = 0;
                foreach (string cbNumberInfo in cbNumberInfoArr)
                {
                    if (string.IsNullOrWhiteSpace(cbNumberInfo))
                    {
                        ++j;
                    }
                }
                if (j == cbNumberInfoArr.Length)
                {
                    _response.error = new Error(EtermCommand.ERROR.NOSHOW_CARBIN_NUMBER);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                for (int k = 1; k < cbNumberInfoArr.Length; ++k)
                {
                    AVSingle avSingle = _response.result.AVList[k - 1];

                    string cb = cbNumberInfoArr[k];
                    cb = Regex.Replace(cb, @"\r|\n", " ").Trim();
                    string[] cbNumberArr = cb.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string cbNum in cbNumberArr)
                    {
                        if (cbNum.Length < 2)
                        {
                            _response.error = new Error(EtermCommand.ERROR.INVALID_AV_RESULT);
                            _response.error.CmdResultBag = originalCmdResult;
                            return _response;
                        }

                        AVCarbinNumber avCBNumber = new AVCarbinNumber();
                        avCBNumber.Cabin = cbNum.Substring(0, 1);
                        avCBNumber.NumTag = cbNum.Substring(1);
                        avCBNumber.NumStr = "0";
                        if (avCBNumber.NumTag.Equals("A"))
                        {
                            avCBNumber.NumStr = avCBNumber.NumTag;
                        }
                        else
                        {
                            int number = 0;
                            if (int.TryParse(avCBNumber.NumTag, out number))
                            {
                                avCBNumber.NumStr = number.ToString();
                            }
                        }

                        avSingle.CarbinNumList.Add(avCBNumber);
                    }
                }

                // 以上逻辑实现各航线的舱位剩余可订数的获得，最终结果存入AVSingle的CarbinNumList

                if (_response.result.AVList == null || _response.result.AVList.Count < 1)
                {
                    _response.error = new Error(EtermCommand.ERROR.INVALID_AV_RESULT);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                int l = 0;
                foreach (AVSingle av in _response.result.AVList)
                {                    
                    if (av.CarbinNumList == null || av.CarbinNumList.Count < 1)
                    {
                        ++l;
                    }
                }
                if (l == _response.result.AVList.Count)
                {
                    _response.error = new Error(EtermCommand.ERROR.NOSHOW_CARBIN_NUMBER);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                } 
            }
            catch (Exception ex)
            {
                _response.error = new Error(EtermCommand.ERROR.SELFDEFINE_ERROR_MESSAGE);
                _response.error.ErrorMessage = string.Format("解析失败，异常信息为：{0}", ex.ToString());
                _response.error.CmdResultBag = originalCmdResult;
                return _response;
            }

            /*
            if (_response.result.AVList.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(_request.SCity))
                {
                    _response.result.AVList = _response.result.AVList.Where<AVSingle>(av => av.SCity.Equals(_request.SCity)).ToList<AVSingle>();

                    if (_response.result.AVList == null || _response.result.AVList.Count < 1)
                    {
                        _response.error = new Error(EtermCommand.ERROR.NO_QUERY_AV_RESULT);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }
                }                

                if (!string.IsNullOrWhiteSpace(_request.ECity))
                {
                    _response.result.AVList = _response.result.AVList.Where<AVSingle>(av => av.ECity.Equals(_request.ECity)).ToList<AVSingle>();

                    if (_response.result.AVList == null || _response.result.AVList.Count < 1)
                    {
                        _response.error = new Error(EtermCommand.ERROR.NO_QUERY_AV_RESULT);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }
                }                

                if (!string.IsNullOrWhiteSpace(_request.Carbin))
                {
                    string carbin = _request.Carbin;
                    if (carbin.Length > 1)
                    {
                        carbin = carbin.Substring(0, 1);
                    }

                    int l = 0;
                    foreach (AVSingle av in _response.result.AVList)
                    {                        
                        av.CarbinNumList = av.CarbinNumList.Where<AVCarbinNumber>(c => c.Cabin.Equals(carbin)).ToList<AVCarbinNumber>();
                        if (av.CarbinNumList == null || av.CarbinNumList.Count < 1)
                        {
                            ++l;
                        }
                    }
                    if (l == _response.result.AVList.Count)
                    {
                        _response.error = new Error(EtermCommand.ERROR.NO_QUERY_AV_RESULT);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }                    
                }
            }
            */

            _response.state = true;
            return _response;
        }

        protected internal override bool ValidRequest()
        {
            if (_request == null
                || string.IsNullOrWhiteSpace(_request.FlightNo)
                || _request.DepDate == DateTime.MinValue.Date
               )
            {
                _response.error = new Error(EtermCommand.ERROR.EMPTY_REQUEST_PARAM);
                return false;
            }

            // 假设当前日期是2015年8月22日，则只能查到起飞日期在2015年8月21日到2016年8月20日期间的舱位可订数
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

            _request.FlightNo = Regex.Replace(_request.FlightNo, @"\s", string.Empty).Trim().ToUpper();

            /*
            if (!string.IsNullOrWhiteSpace(_request.SCity))
            {
                _request.SCity = Regex.Replace(_request.SCity, @"\s", string.Empty).Trim().ToUpper();
            }

            if (!string.IsNullOrWhiteSpace(_request.ECity))
            {
                _request.ECity = Regex.Replace(_request.ECity, @"\s", string.Empty).Trim().ToUpper();
            }

            if (!string.IsNullOrWhiteSpace(_request.Carbin))
            {
                _request.Carbin = Regex.Replace(_request.Carbin, @"\s", string.Empty).Trim().ToUpper();
            }
            */

            return true;
        }

        protected internal override bool ValidCmdResult(string cmdResult)
        {
            if (string.IsNullOrWhiteSpace(cmdResult))
            {
                _response.error = new Error(EtermCommand.ERROR.COMMAND_EMPTY);
                return false;
            }

            if (!Regex.IsMatch(cmdResult, @"TOTAL JOURNEY TIME\s+\d*:\d+"))
            {
                _response.error = new Error(EtermCommand.ERROR.INVALID_AV_RESULT);
                return false;
            }

            return true;
        }
    }
}
