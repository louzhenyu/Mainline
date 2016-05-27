using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JetermEntity.Parser
{
    /// <summary>
    /// 解析订位SS指令
    /// </summary>
    [Serializable]
    public class Booking : ParserBase<JetermEntity.Request.Booking, CommandResult<JetermEntity.Response.Booking>>
    {
        #region 成员变量

        private JetermEntity.Request.Booking _request = null;

        private CommandResult<JetermEntity.Response.Booking> _response = null;

        /// <summary>
        /// 请求对象
        /// </summary>
        public override JetermEntity.Request.Booking Request { get { return this._request; } }

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.Booking> Response { get { return this._response; } }

        #endregion

        /// <summary>
        /// 构造订位SS指令返回对象
        /// </summary>
        public Booking()
        {
            _response = new CommandResult<JetermEntity.Response.Booking>();

            _response.result = new JetermEntity.Response.Booking();
        }

        public Booking(string config, string officeNo)
            : this()
        {
            _response.config = config;
            _response.OfficeNo = officeNo;

            _response.result.OfficeNo = officeNo;
        }

        /// <summary>
        /// 解析请求对象
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回订位SS指令；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.Booking request)
        {
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }
            // 1、获得【起飞信息】和【返回信息】输入命令行：
            Flight sFlight = request.FlightList[0];
            string ssHeaderLine = GetHeadCommandLine(request, sFlight);
            List<string> commandLineList = new List<string>();
            GetSecondCommandLine(request, sFlight, commandLineList);
            // 2、获得预订人名输入命令行：
            string namesLine = commandLineList[0];
            // 4、获得以【SSR FOID YY HK/NI】或【SSR FOID {0} HK/NI】开头的那些命令行：
            string cardNumberLine = commandLineList[1];

            string airLine = sFlight.FlightNo.Substring(0, 2); // 航司

            // 5、若为儿童订位，则出现【SSR OTHS {航司如MU} ADULT PNR IS {成人PNR如HF4X80}】命令行：
            string adultPnrLine = string.Empty;
            if (!string.IsNullOrWhiteSpace(commandLineList[3]))
            {
                adultPnrLine = string.Format("SSR OTHS {0} ADULT PNR IS {1}[RN]", airLine, request.Pnr);
            }

            // 6、若为儿童订位，则出现【SSR CHLD {航司如MU} HK1/{儿童出生日期如10JUL12}/P{第几位儿童乘客如1}】那些命令行：
            string childrenBirthdayLine = commandLineList[3];

            // 8、获得可能出现的以【OSI HU CKIN SSAC/S{0}】开头的那些命令行：
            string osiHUCkinLine = commandLineList[2];

            // 3、获得以TKTL开头的命令行：
            DateTime dt = DateTime.Now.AddMinutes(60);
            string saveTime = dt.ToString("HHmm"); //保留时间
            string saveDay = dt.ToString("dd");  //保留日期
            string saveMonth = dt.ToString("MM"); //保留月份
            string saveExp = saveTime + "/" + saveDay + ParserHelper.GetEnMonth(saveMonth); //保留时限
            string tktlLine = string.Format("TKTL{0}/{1}[RN]", saveExp, request.OfficeNo);
            // 7、获得以OSI开头的命令行：
            string osiLine = string.Format("OSI {0} CTCT {1}[RN]", airLine, request.Mobile);
            string osiZHLine = string.Empty;
            if (airLine.Equals("ZH") && !string.IsNullOrWhiteSpace(request.PhoneNo))
            {
                osiZHLine = string.Format("OSI {0} CTCT{1}[RN]", airLine, request.PhoneNo);
            }
            // 9、获得可能出现的以【OSI PN CABIN{0}】开头的命令行：
            // 如果是西部航空，则会出现以【OSI PN CABIN{0}】开头的命令行：
            string osiPNCabinLine = string.Empty;
            string pnCabin = string.Empty;
            if (airLine.Equals("PN"))
            {
                foreach (Flight flight in request.FlightList)
                {
                    pnCabin += string.Format("/{0}", flight.Cabin);
                }

                osiPNCabinLine = string.Format("OSI PN CABIN{0}[RN]", pnCabin);
            }
            // 10、获得以【RMK TJ AUTH】开头的那些命令行：            
            string rmkAuthLine = string.Empty;
            if (request.RMKOfficeNoList != null && request.RMKOfficeNoList.Any())
            {
                foreach (string rmkOfficeNo in request.RMKOfficeNoList)
                {
                    if (string.IsNullOrWhiteSpace(rmkOfficeNo))
                    {
                        continue;
                    }
                    rmkAuthLine += string.Format("RMK TJ AUTH {0}[RN]", rmkOfficeNo);
                }
            }
            // 11、获得可能出现的以【RMK】开头的命令行：
            string rmkRemarkLine = string.Empty;
            if (!string.IsNullOrWhiteSpace(request.RMKRemark))
            {
                rmkRemarkLine = string.Format("RMK {0}[RN]", request.RMKRemark);
            }
            // 12、最终获得订位SS指令：
            string bookCommand = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}\\", ssHeaderLine, namesLine, tktlLine, cardNumberLine, adultPnrLine, childrenBirthdayLine, osiLine, osiZHLine, osiHUCkinLine, osiPNCabinLine, rmkAuthLine, rmkRemarkLine);
            // 获得订位SS指令
            _response.result.Command = bookCommand.Replace("[RN]", Environment.NewLine);

            return _response.result.Command;
        }

        /// <summary>
        /// 解析订位指令返回结果
        /// </summary>
        /// <param name="cmdResult">订位指令返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.Booking> ParseCmdResult(string cmdResult)
        {
            _response.result.ResultBag = cmdResult;

            _response.result.BookingState = EtermCommand.BookingState.BookingFail;

            // 订位指令返回结果验证：
            if (!ValidCmdResult(cmdResult))
            {
                _response.error.CmdResultBag = cmdResult;
                return _response;
            }

            string regexPNumber = @"[1-9](\d*)";
            string regHKResult = string.Format(@"\sHK{0}\s", regexPNumber);
            string regDKResult = string.Format(@"\sDK{0}\s", regexPNumber);
            string regKKResult = string.Format(@"\sKK{0}\s", regexPNumber);
            string regVirtualPosition = string.Empty;
            if (
                Regex.IsMatch(cmdResult, regHKResult)
                || Regex.IsMatch(cmdResult, regDKResult)
                || (Regex.IsMatch(cmdResult, regKKResult) && Regex.IsMatch(cmdResult, @"\s*CZ\s*(\d+)\s"))
               )
            {
                _response.result.BookingState = EtermCommand.BookingState.BookingSuccess;
                regVirtualPosition = string.Format(@"\s*HL{0}\s|\s*DW{0}\s|\s*KL{0}\s", regexPNumber);
                if (Regex.IsMatch(cmdResult, regVirtualPosition))
                {
                    _response.result.BookingState = EtermCommand.BookingState.WaitBooking;
                }
                regVirtualPosition = string.Format(@"\s*HN{0}\s|\s*NN{0}\s", regexPNumber);
                if (Regex.IsMatch(cmdResult, regVirtualPosition))
                {
                    _response.result.BookingState = EtermCommand.BookingState.BookingFail;
                }

                bool isValidBook = ValidBookResult(cmdResult);
                // 若验证通过，则获得Pnr。
                if (isValidBook)
                {
                    _response.state = true;
                    return _response;
                }
                // 返回error：很抱歉，订位失败，请重新预订
                _response.error = new Error(EtermCommand.ERROR.BOOKING_FAIL);
                _response.error.CmdResultBag = cmdResult;
                return _response;
            }

            regVirtualPosition = string.Format(@"\s*HN{0}\s|\s*HL{0}\s|\s*DW{0}\s|\s*KL{0}\s|\s*NN{0}\s", regexPNumber);
            if (!Regex.IsMatch(cmdResult, regVirtualPosition))
            {
                // 返回error：很抱歉，订位失败，请重新预订
                _response.error = new Error(EtermCommand.ERROR.BOOKING_FAIL);
                _response.error.CmdResultBag = cmdResult;
                return _response;
            }
            // 若验证通过，则获得Pnr。
            if (ValidBookResult(cmdResult))
            {
                _response.state = true;
                return _response;
            }
            // 返回error：很抱歉，订位失败，请重新预订
            _response.error = new Error(EtermCommand.ERROR.BOOKING_FAIL);
            _response.error.CmdResultBag = cmdResult;
            return _response;
        }

        #region Helper

        protected internal override bool ValidRequest()
        {
            if (_request == null
                || (_request.FlightList == null || !_request.FlightList.Any())
                || (_request.PassengerList == null || !_request.PassengerList.Any())
                || string.IsNullOrWhiteSpace(_request.OfficeNo)
                || string.IsNullOrWhiteSpace(_request.Mobile)
                )
            {
                // 返回false的原因：没有传必须传的请求值
                _response.error = new Error(EtermCommand.ERROR.EMPTY_REQUEST_PARAM);
                return false;
            }

            string originalValue = string.Empty;

            // start：以下代码段实现的是：验证关于航班信息的所有输入参数的合法性

            string selfDefinedErrorMessage = string.Empty;
            for (int f = 0; f < _request.FlightList.Count; ++f)
            {
                int fNumber = f + 1; // 第几航程
                Flight flight = _request.FlightList[f];

                if (string.IsNullOrWhiteSpace(flight.FlightNo) || string.IsNullOrWhiteSpace(flight.Cabin)
                    || flight.DepDate.Date == DateTime.MinValue || string.IsNullOrWhiteSpace(flight.SCity) || string.IsNullOrWhiteSpace(flight.ECity))
                {
                    selfDefinedErrorMessage += string.Format("{0}没有给第{1}段航程传入必须传的请求参数值，如：航班号FlightNo、舱位Cabin、出发时间DepDate、出发城市三字码SCity、到达城市三字码ECity。", string.IsNullOrWhiteSpace(selfDefinedErrorMessage) ? string.Empty : Environment.NewLine, fNumber);
                }

                if (!string.IsNullOrWhiteSpace(flight.FlightNo))
                {
                    originalValue = flight.FlightNo;
                    flight.FlightNo = Regex.Replace(flight.FlightNo, @"\s", string.Empty).Trim().ToUpper();
                    if (flight.FlightNo.Length < 2)
                    {
                        selfDefinedErrorMessage += string.Format("{0}第{1}段航程的航班号【{2}】输入格式不正确，正确格式如：CZ380。", string.IsNullOrWhiteSpace(selfDefinedErrorMessage) ? string.Empty : Environment.NewLine, fNumber, originalValue);
                    }
                }

                if (!string.IsNullOrWhiteSpace(flight.Cabin))
                {
                    flight.Cabin = Regex.Replace(flight.Cabin, @"\s", string.Empty).Trim().ToUpper();
                }

                if (!string.IsNullOrWhiteSpace(flight.SCity))
                {
                    flight.SCity = Regex.Replace(flight.SCity, @"\s", string.Empty).Trim().ToUpper();
                }

                if (!string.IsNullOrWhiteSpace(flight.ECity))
                {
                    flight.ECity = Regex.Replace(flight.ECity, @"\s", string.Empty).Trim().ToUpper();
                }
            }

            if (!string.IsNullOrWhiteSpace(selfDefinedErrorMessage))
            {
                // 返回false的原因：航班信息请求参数验证没有通过
                _response.error = new Error(EtermCommand.ERROR.SELFDEFINE_ERROR_MESSAGE);
                _response.error.ErrorMessage = selfDefinedErrorMessage;
                return false;
            }

            // end：以上代码段实现的是：验证关于航班信息的所有输入参数的合法性   

            // start：以下代码段实现的是：验证各预订人输入参数的合法性

            selfDefinedErrorMessage = string.Empty;

            // 用于计算儿童年龄
            DateTime nowDate = DateTime.Now.Date;
            Flight sFlight = _request.FlightList[0];
            int nowMonth = sFlight.DepDate.Month;
            if (nowMonth < nowDate.Month)
            {
                nowDate = nowDate.AddYears(1);
            }
            nowDate = Convert.ToDateTime(string.Format("{0}-{1}-{2}", nowDate.Year, nowMonth, sFlight.DepDate.Day)).Date;

            for (int p = 0; p < _request.PassengerList.Count; ++p)
            {
                int pNumber = p + 1; // 第几位乘机人
                Passenger passenger = _request.PassengerList[p];

                if (passenger.PassType.GetHashCode() == EtermCommand.PassengerType.NotSet.GetHashCode()
                    || string.IsNullOrWhiteSpace(passenger.name)
                    || passenger.idtype.GetHashCode() == EtermCommand.IDtype.NotSet.GetHashCode()
                    || string.IsNullOrWhiteSpace(passenger.cardno))
                {
                    selfDefinedErrorMessage += string.Format("{0}没有给第{1}位乘客传入必须传的请求参数值，如：乘客类型PassType、姓名name、ID号cardno、ID卡类型idtype。", string.IsNullOrWhiteSpace(selfDefinedErrorMessage) ? string.Empty : Environment.NewLine, pNumber);
                }

                if (passenger.PassType.GetHashCode() == EtermCommand.PassengerType.Children.GetHashCode())
                {
                    if (passenger.ChildBirthday.Date == DateTime.MinValue)
                    {
                        selfDefinedErrorMessage += string.Format("{0}必须给第{1}位儿童乘客{2}传出生日期参数。", string.IsNullOrWhiteSpace(selfDefinedErrorMessage) ? string.Empty : Environment.NewLine, pNumber, passenger.name);
                    }

                    // 判断该儿童乘客的年龄是否处于儿童年龄范围之内
                    if (passenger.ChildBirthday.Date != DateTime.MinValue)
                    {
                        int childAge = ParserHelper.GetAge(passenger.ChildBirthday.Date, nowDate);
                        if (childAge > 12 || childAge < 2)
                        {
                            selfDefinedErrorMessage += string.Format("{0}第{1}位儿童乘客{2}年龄不符：不在2岁到12岁之间。", string.IsNullOrWhiteSpace(selfDefinedErrorMessage) ? string.Empty : Environment.NewLine, pNumber, passenger.name);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(passenger.name))
                {
                    originalValue = passenger.name;

                    string userName = ParserHelper.FilterSqlSpecial(passenger.name);
                    userName = Regex.Replace(userName, @"\s", string.Empty);
                    passenger.name = Regex.Replace(passenger.name, @"\s", string.Empty).ToUpper();
                    if (userName.Length != passenger.name.Length)
                    {
                        selfDefinedErrorMessage += string.Format("{0}第{1}位乘客名字【{2}】不合法。", string.IsNullOrWhiteSpace(selfDefinedErrorMessage) ? string.Empty : Environment.NewLine, pNumber, originalValue);
                    }

                    if (passenger.PassType.GetHashCode() == EtermCommand.PassengerType.Children.GetHashCode())
                    {
                        passenger.name = ParserHelper.TrimCHD(passenger.name);
                        passenger.name = string.Format("{0} {1}", passenger.name, "CHD");
                    }
                }

                passenger.cardno = Regex.Replace(passenger.cardno, @"\s", string.Empty).Trim().ToUpper();
            }

            if (!string.IsNullOrWhiteSpace(selfDefinedErrorMessage))
            {
                // 返回false的原因：乘客请求参数验证没有通过
                _response.error = new Error(EtermCommand.ERROR.SELFDEFINE_ERROR_MESSAGE);
                _response.error.ErrorMessage = selfDefinedErrorMessage;
                return false;
            }

            // end：以上代码段实现的是：验证各预订人输入参数的合法性

            if (string.IsNullOrWhiteSpace(_request.Pnr))
            {
                foreach (Passenger passenger in _request.PassengerList)
                {
                    if (passenger.PassType == EtermCommand.PassengerType.Children)
                    {
                        // 返回false的原因：给儿童订位时，成人PNR请求参数必传
                        _response.error = new Error(EtermCommand.ERROR.EMPTY_REQUEST_ADULT_PNR);
                        return false;
                    }
                }
            }

            // 为保留时限项的Office号去除空格
            _request.OfficeNo = Regex.Replace(_request.OfficeNo, @"\s", string.Empty).Trim().ToUpper();

            // 以下代码段实现的是：验证输入参数mobile的合法性

            _request.Mobile = ParserHelper.GetValidBookSeatMobile(_request.Mobile.Trim());
            if (string.IsNullOrWhiteSpace(_request.Mobile))
            {
                _response.error = new Error(EtermCommand.ERROR.EMPTY_OR_INVALID_MOBILE);
                return false;
            }

            // 以上代码段实现的是：验证输入参数mobile的合法性

            if (!string.IsNullOrWhiteSpace(_request.PhoneNo))
            {
                _request.PhoneNo = Regex.Replace(_request.PhoneNo, @"\s", string.Empty).Replace("-", string.Empty).Trim();
                Regex reg = new Regex(@"\d+");               
                if (!reg.IsMatch(_request.PhoneNo))
                {
                    _response.error = new Error(EtermCommand.ERROR.INVALID_PHONENO);
                    return false;
                }
            }

            // 为授权Office号去除空格
            if (_request.RMKOfficeNoList != null && _request.RMKOfficeNoList.Any())
            {
                for (int r = 0; r < _request.RMKOfficeNoList.Count; ++r)
                {
                    _request.RMKOfficeNoList[r] = Regex.Replace(_request.RMKOfficeNoList[r], @"\s", string.Empty).Trim().ToUpper();
                }
            }

            // 为授权备注去除前后空格
            if (!string.IsNullOrWhiteSpace(_request.RMKRemark))
            {
                _request.RMKRemark = _request.RMKRemark.Trim();
            }

            return true;
        }

        protected internal override bool ValidCmdResult(string cmdResult)
        {
            // 根据判断条件设置等待订位标识
            SetWaitState(cmdResult);

            if (string.IsNullOrWhiteSpace(cmdResult))
            {
                _response.error = new Error(EtermCommand.ERROR.ORDER_COMMAND_EMPTY);
                return false;
            }

            if (cmdResult.Equals("err") || cmdResult.ToLower().Contains("error"))
            {
                // 返回false的原因：订位系统出现故障，暂时无法进行订位
                _response.error = new Error(EtermCommand.ERROR.BOOKING_REMOTE_ERROR);
                return false;
            }

            if (Regex.IsMatch(cmdResult, @"\d{1,}\.") || cmdResult.Contains("UNABLE TO"))
            {
                // 返回false的原因：很抱歉，订位失败，请重新预订
                _response.error = new Error(EtermCommand.ERROR.BOOKING_FAIL);
                return false;
            }

            if (!Regex.IsMatch(cmdResult, @"[A-Za-z0-9]{1,}"))
            {
                // 返回false的原因：很抱歉，订位失败，请重新预订
                _response.error = new Error(EtermCommand.ERROR.BOOKING_FAIL);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获得【起飞信息】和【返回信息】输入命令行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sFlight">起飞信息</param>     
        /// <returns></returns>
        private string GetHeadCommandLine(JetermEntity.Request.Booking request, Flight sFlight)
        {
            sFlight = request.FlightList[0];

            // 获得航段信息输入命令行：
            string ssHeaderLine = string.Empty;
            int passengerNumber = request.PassengerList.Count;
            foreach (Flight flight in request.FlightList)
            {
                ssHeaderLine += string.Format("SS: {0}/{1}/{2}/{3}{4}/{5}[RN]", flight.FlightNo, flight.Cabin.Substring(0, 1), string.Format("{0:dd}{1}", flight.DepDate, ((ParserHelper.Month)flight.DepDate.Month).ToString()), flight.SCity, flight.ECity, passengerNumber);
            }

            return ssHeaderLine;
        }

        /// <summary>
        /// 获得以下输入命令行：
        /// 1、预订人名输入命令行
        /// 2、以【SSR FOID YY HK/NI】或【SSR FOID {0} HK/NI】开头的那些命令行       
        /// 3、可能出现的以【OSI HU CKIN SSAC/S{0}】开头的那些命令行
        /// 4、如果为儿童订票，则出现以【SSR CHLD {航司如MU} HK1/{儿童出生日期如10JUL12}/P{第几位儿童乘客如1}】那些命令行
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="sFlight">返程信息</param>
        /// <param name="commandLineList"></param>
        private void GetSecondCommandLine(JetermEntity.Request.Booking request, Flight sFlight, List<string> commandLineList)
        {
            // 获得预订人名输入命令行：
            string namesLine = "NM ";
            // 获得以【SSR FOID YY HK/NI】或【SSR FOID {0} HK/NI】开头的那些命令行：
            string cardNumberLine = string.Empty;
            // 如果为儿童订票，则获得以【SSR CHLD {航司如MU} HK1/{儿童出生日期如10JUL12}/P{第几位儿童乘客如1}】那些命令行
            string childrenBirthdayLine = string.Empty;
            // 获得可能出现的以【OSI HU CKIN SSAC/S{0}】开头的那些命令行：
            string osiHUCkinLine = string.Empty;

            IEnumerable<string> passengerNameList = request.PassengerList.Select(p => p.name);
            string[] sortedPassengerNameList = ParserHelper.SortChinese(passengerNameList).ToArray();
            int length = 3; // 原有逻辑：初始值为3
            string airLine = sFlight.FlightNo.Substring(0, 2); // 航司
            for (int i = 1; i <= sortedPassengerNameList.Length; i++)
            {
                foreach (Passenger passenger in request.PassengerList)
                {
                    if (!sortedPassengerNameList[i - 1].Equals(passenger.name))
                    {
                        continue;
                    }

                    #region 获得预订人名输入命令行

                    namesLine += GetCurrentName(passenger.name, ref length);

                    #endregion

                    #region 获得以【SSR FOID YY HK/NI】或【SSR FOID {0} HK/NI】开头的那些命令行

                    if (passenger.PassType.GetHashCode() == EtermCommand.PassengerType.Children.GetHashCode())
                    {
                        cardNumberLine += string.Format("SSR FOID {0} HK/NI{1}/P{2}[RN]", airLine, passenger.cardno, i);
                    }
                    else
                    {
                        switch (passenger.idtype)
                        {
                            case EtermCommand.IDtype.Other:     // 证件类型为其他时 FOID项的航司固定为“YY”
                                cardNumberLine += string.Format("SSR FOID YY HK/NI{0}/P{1}[RN]", passenger.cardno, i);
                                break;
                            default:
                                cardNumberLine += string.Format("SSR FOID {0} HK/NI{1}/P{2}[RN]", airLine, passenger.cardno, i);
                                break;
                        }
                    }

                    #endregion

                    #region  如果为儿童订票，则出现以【SSR CHLD {航司如MU} HK1/{儿童出生日期如10JUL12}/P{第几位儿童乘客如1}】那些命令行

                    if (passenger.PassType == EtermCommand.PassengerType.Children)
                    {
                        string day = passenger.ChildBirthday.ToString("dd");
                        string month = ParserHelper.GetEnMonth(passenger.ChildBirthday.ToString("MM"));
                        string year = passenger.ChildBirthday.ToString("yy");
                        string childBirthdayStr = string.Format("{0}{1}{2}", day, month, year);
                        childrenBirthdayLine += string.Format("SSR CHLD {0} HK1/{1}/P{2}[RN]", airLine, childBirthdayStr, i);
                    }

                    #endregion

                    #region 获得可能出现的以【OSI HU CKIN SSAC/S{0}】开头的那些命令行

                    if (airLine.Equals("HU") && sFlight.Cabin.Equals("A"))
                    {
                        osiHUCkinLine += string.Format("OSI HU CKIN SSAC/S{0}[RN]", i);
                    }

                    #endregion
                }
            }
            namesLine += "[RN]";

            commandLineList.Add(namesLine);
            commandLineList.Add(cardNumberLine);
            commandLineList.Add(osiHUCkinLine);
            commandLineList.Add(childrenBirthdayLine);
        }

        /// <summary>
        /// 获得单个姓名输入命令
        /// </summary>
        /// <param name="name">当前乘客姓名</param>
        /// <param name="length">用于决定预订名命令行是否换行的度量器</param>
        /// <returns></returns>
        private string GetCurrentName(string name, ref int length)
        {
            int currentNameLength = ParserHelper.GetLengthOfString(name);
            string currentName = string.Empty;
            // 前台黑屏定位时，名字多到一定长度时，需要换行；而换行时需要加【-】。
            if (currentNameLength + length < 71)
            {
                currentName = "1" + name;
                length += currentNameLength + 1;
            }
            else
            {
                currentName = "[RN]-1" + name;
                length = currentNameLength + 2;
            }

            return currentName;
        }

        /// <summary>
        /// 验证预订命令。若验证通过，则获得Pnr。
        /// </summary>
        /// <param name="cmdResult">预订命令返回结果</param>      
        /// <returns></returns>
        private bool ValidBookResult(string cmdResult)
        {
            string newPnr = string.Empty;
            bool isValid = ParserHelper.GetNewPnr(cmdResult, ref newPnr);
            _response.result.Pnr = newPnr;
            return isValid;
        }

        /// <summary>
        /// 根据判断条件设置等待订位标识
        /// </summary>
        /// <param name="cmdResult"></param>
        private void SetWaitState(string cmdResult)
        {
            _response.result.BookingState = EtermCommand.BookingState.WaitBooking;
            if (cmdResult.Contains("没有座位")
                || Regex.IsMatch(cmdResult, @"\s*NN[1-9](\d*)\s")
                || Regex.IsMatch(cmdResult, @"\s*HN[1-9](\d*)\s")
                || cmdResult.Contains("SEATS")
                || cmdResult.Contains("PLS NM1XXXX/XXXXXX")
                || cmdResult.Contains("CHECK TKT TIME")
                || cmdResult.Contains("NAMES")
                || cmdResult.Contains("INVALID CHAR")
                || cmdResult.Contains("TIME")
                || cmdResult.Contains("FORMAT")
                || cmdResult.Contains("NAME LENGTH"))
            {
                _response.result.BookingState = EtermCommand.BookingState.BookingFail; ;
            }
        }

        #endregion
    }
}
