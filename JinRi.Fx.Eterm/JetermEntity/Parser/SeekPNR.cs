using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JetermEntity.Parser
{
    /// <summary>
    /// 解析提取编码RT指令
    /// </summary>
    [Serializable]
    public class SeekPNR : ParserBase<JetermEntity.Request.SeekPNR, CommandResult<JetermEntity.Response.SeekPNR>>
    {
        #region 成员变量

        private int guestNumber = 100;

        private string pnr = string.Empty;

        private bool isGetPrice = false;

        private EtermCommand.PassengerType passengerType = EtermCommand.PassengerType.NotSet;

        private JetermEntity.Request.SeekPNR _request = null;

        private CommandResult<JetermEntity.Response.SeekPNR> _response = null;

        private static Regex pnrRegex = new Regex(@"([\s+][A-Z]{1}[A-Z0-9]{5}[\s/]+)");
         

        /// <summary>
        /// 请求对象
        /// </summary>
        public override JetermEntity.Request.SeekPNR Request { get { return this._request; } }

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.SeekPNR> Response { get { return this._response; } }

        #endregion

        /// <summary>
        /// 构造提取编码RT指令返回对象
        /// </summary>
        public SeekPNR()
        {
            _response = new CommandResult<JetermEntity.Response.SeekPNR>();
            _response.result = new JetermEntity.Response.SeekPNR();
        }

        public SeekPNR(string config, string officeNo)
            : this()
        {
            _response.config = config;
            _response.OfficeNo = officeNo;
        }

        /// <summary>
        /// 解析请求对象
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回提取编码RT指令；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.SeekPNR request)
        {
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }

            return string.Format("RT {0}", request.Pnr);
        }

        /// <summary>
        /// 解析RT指令返回结果
        /// </summary>
        /// <param name="cmdResult">RT指令返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.SeekPNR> ParseCmdResult(string cmdResult)
        {
            if (string.IsNullOrWhiteSpace(cmdResult))
            {
                _response.error = new Error(EtermCommand.ERROR.COMMAND_EMPTY);
                return _response;
            }

            string originalCmdResult = cmdResult;

            // 获得RT指令原始返回结果：
            _response.result.ResultBag = originalCmdResult;

            cmdResult = cmdResult.ToUpper();

            isGetPrice = cmdResult.Contains(">PAT:A");
            int patIndex = -1;
            string rtResult = cmdResult;
            string priceCmdResult = string.Empty;
            if (!isGetPrice && cmdResult.Contains("PAT:A"))
            {
                patIndex = cmdResult.IndexOf("PAT:A");
                rtResult = cmdResult.Substring(0, patIndex);
                priceCmdResult = cmdResult.Substring(patIndex);
                isGetPrice = true;
            }
            else if (isGetPrice)
            {
                patIndex = cmdResult.IndexOf(">PAT:A");
                rtResult = cmdResult.Substring(0, patIndex);
                priceCmdResult = cmdResult.Substring(patIndex);
            }

            rtResult = Regex.Replace(rtResult, @"\r|\n", string.Empty).Trim();
            rtResult = rtResult.Replace("<li>", string.Empty).Replace("</li>", string.Empty);

            // RT指令返回结果验证：
            if (!ValidCmdResult(rtResult))
            {
                _response.error.CmdResultBag = originalCmdResult;
                return _response;
            }

            passengerType = EtermCommand.PassengerType.Adult;
            if (rtResult.Contains("INFT"))
            {
                passengerType = EtermCommand.PassengerType.Baby;
            }
            else if (rtResult.Contains("ADULT PNR IS"))
            {
                passengerType = EtermCommand.PassengerType.Children;
            }

            // 获得PNR：          
            //MatchCollection mc = Regex.Matches(rtResult, @"([\s+][A-Z0-9]{6}[\s+])|([\s+][0-9A-Z]{6}[\s+])");
            MatchCollection mc = pnrRegex.Matches(rtResult);
            if (mc == null || mc.Count < 1)
            {
                _response.error = new Error(EtermCommand.ERROR.NOT_EXIST_PNR);
                _response.error.CmdResultBag = originalCmdResult;
                return _response;
            }

            pnr = mc[0].Value.Trim().TrimEnd('/');
            if (pnr.Equals("TICKET", StringComparison.InvariantCultureIgnoreCase))
            {
                pnr = mc[1].Value.Trim();
            }
            _response.result.PNR = pnr;

            try
            {
                // start：获得乘机人类型、成人/儿童乘机人姓名、成人/儿童乘机人的ID号、成人/儿童乘机人的ID类型、儿童出生日期（日期类型）、婴儿姓名、婴儿姓名拼音以及婴儿出生日期（string类型）

#warning: 问题：数据库表中没有团队的例子，所以没有测过。能否给个这样的例子？
                // 判断是否是团队，并设置guestNumber初始值
                bool isTeam = false;
                ParserHelper.IsTeam(rtResult, ref guestNumber, ref isTeam);                

                // 获得成人/儿童乘客姓名，并设置guestNumber：
                GetAdultOrChdName(rtResult);

                // 获得婴儿乘客姓名、姓名拼音、出生日期以及婴儿总数：
                //int infCount = 0; // 婴儿数
                //if (!GetBabyInfo(rtResult, ref infCount))
                if (!GetBabyInfo(rtResult))
                {
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                // 获得各成人/儿童乘客的ID号、ID类型、票号以及姓名拼音：
                GetAdultInfo(rtResult);

                // 获得儿童出生日期
                GetChildBirthday(rtResult);

                // end：获得乘机人类型、成人/儿童乘机人姓名、成人/儿童乘机人的ID号、成人/儿童乘机人的ID类型、儿童出生日期（日期类型）、婴儿姓名、婴儿姓名拼音以及婴儿出生日期（string类型）

                // 获得手机号和可能有的座机号：                
                GetMobileAndPhone(rtResult);

                // 获得Office号：
                if (Regex.IsMatch(rtResult, @"[0-9]{1,2}\.[A-Z]{3}[0-9]{3}"))
                {
                    _response.result.OfficeNo = Regex.Match(rtResult, @"[0-9]{1,2}\.([A-Z]{3}[0-9]{3})").Groups[1].Value;
                }

                // 获得大编码号：
                //_response.result.BigPNR = ParserHelper.GetBigPNR(rtResult);
                // 把参数由rtResult改成cmdResult的原因：由大系统返回的RTPAT信息，大编码号这行命令（如：pnr=KF1QET&airline=CA返回的命令中的【[eTerm:caa01] RMK CA/MJYPC2】）位于最后一行
                _response.result.BigPNR = ParserHelper.GetBigPNR(cmdResult);

                // 获得RMK Office No List：
                GetRMKOfficeNoList(rtResult);

                // 获得儿童监管人的PNR：
                if (passengerType == EtermCommand.PassengerType.Children)
                {
                    Regex reg = new Regex(@"ADULT PNR IS\s*(\S{6})");
                    Match match = reg.Match(rtResult);
                    if (match != null && match.Groups != null && match.Groups.Count > 0)
                    {
                        _response.result.AdultPnr = match.Groups[1].Value.Trim();
                    }
                }

                #region 获得航班信息

                #region 获得第1段航班信息，即获得航班号、舱位、出发日期和时间、到达日期和时间、出发城市、到达城市、航站楼以及航班是否为共享航班这些信息

                int num01 = guestNumber + 1; // 目前的guestNumber等于成人乘机人数
                int num03 = rtResult.IndexOf(num01.ToString() + "."); // 相当于【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035 1        E T3--】这行的行号
                int num02 = guestNumber + 2;
                int num04;
                // airInfo存的是相当于如下格式之一的行内容：
                // 3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035 1        E T3--
                //  2.  CA1666 V   SU26JUL  YNJTSN RR1   1825 2035          E      V1 
                //  3.  MU5138 Y   FR01MAY  PEKSHA HK2   0700 0910          E T2T2                
                // 4. *MU4009 Y   SU10MAY  SHAXIY HK2   0805 1040          E T2T3 OP-HO1217 
                string airInfo = string.Empty;
                try
                {
                    num04 = rtResult.IndexOf(num02.ToString() + ".");
                    // 格式如：  CA1666 V   SU26JUL  YNJTSN RR1   1825 2035          E      V1 
                    airInfo = rtResult.Substring(num03, num04 - num03).Replace(num01.ToString() + ".", string.Empty);
                }
                catch
                {
#warning 这种情况什么时候会出现
                    num04 = rtResult.IndexOf("+");
                    airInfo = rtResult.Substring(num03, num04 - num03).Replace(num01.ToString() + ".", string.Empty);
                }
                airInfo = airInfo.Replace(" ", "|").Replace("|||", "|").Replace("||", "|").Replace("||", "|");
                // 格式如：|CA1666|V|SU26JUL|YNJTSN|RR1|1825|2035|E|V1||
                airInfo = ParserHelper.RepalceYear(airInfo);
                // 检查航班各信息是否过长
                string[] checkInfo = airInfo.Split('|');
                if (checkInfo[1].Replace("*", string.Empty).Length > 8 && checkInfo[2].Length > 2)
                {
                    airInfo = airInfo.Insert(8, "|");
                }
                else if (checkInfo[3].Length == 15)
                {
                    airInfo = airInfo.Replace(checkInfo[3].Substring(0, 9), checkInfo[3].Substring(0, 7) + "|");
                }
                else if (checkInfo[4].Length > 16)
                {
                    // SHAPEKHK108551115
                    airInfo = airInfo.Replace(checkInfo[4].Substring(0, 6), checkInfo[4].Substring(0, 6) + "|");
                    airInfo = airInfo.Replace(checkInfo[4].Substring(6, 3), checkInfo[4].Substring(6, 3) + "|");
                    airInfo = airInfo.Replace(checkInfo[4].Substring(9, 4), checkInfo[4].Substring(9, 4) + "|");
                    airInfo = airInfo.Replace(checkInfo[4].Substring(13, 4), checkInfo[4].Substring(13, 4) + "|");
                }

                string[] cutInfo = airInfo.Split('|');
                //cutInfo = cutInfo.Where<string>(s => !string.IsNullOrWhiteSpace(s)).ToArray<string>();

                Flight flight = new Flight();

                #region 根据编码状态，验证RT命令返回结果的有效性

                string pnrState = cutInfo[4].Length > 6 ? cutInfo[4].Substring(6, cutInfo[4].Length - 6) : cutInfo[5]; // 【pnrState = cutInfo[5];】时，记录编号状态， NO，HL  
                flight.PNRState = pnrState;
                _response.result.FlightList.Add(flight);
                if (!isTeam)
                {
                    // 判断记录编号是否证件号码
                    if (!rtResult.Contains("FOID"))
                    {
                        // 错误信息：此记录编号未添加证件号码，加入后请重新导入PNR记录编号
                        _response.error = new Error(EtermCommand.ERROR.NO_CARDNO);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }
                }
                if (isGetPrice && passengerType.GetHashCode() == EtermCommand.PassengerType.Baby.GetHashCode())
                {
                    // 返回【成人编码不是RR状态，无法创单，请成人出票后再创单！】错误信息：
                    if (!pnrState.Contains("RR"))
                    {
                        _response.error = new Error(EtermCommand.ERROR.NO_ADULT_TICKET);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }
                }

                // 判断记录是否被NO掉了
                if (pnrState.Contains("NO"))
                {
                    _response.error = new Error(EtermCommand.ERROR.FIRST_SEGMENT_STATE_NO);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }
                // 判断记录是否为HL状态
                if (pnrState.Contains("HL"))
                {
                    _response.error = new Error(EtermCommand.ERROR.FIRST_SEGMENT_STATE_HL);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }
                // 判断记录是否为TN状态
                if (pnrState.Contains("TN"))
                {
                    _response.error = new Error(EtermCommand.ERROR.FIRST_SEGMENT_STATE_TN);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }
                // 判断记录是否为HN状态
                if (pnrState.Contains("HN"))
                {
                    _response.error = new Error(EtermCommand.ERROR.FIRST_SEGMENT_STATE_HN);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }
                // 判断记录是否为HN状态
                if (pnrState.Contains("HX"))
                {
                    _response.error = new Error(EtermCommand.ERROR.FIRST_SEGMENT_STATE_HX);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }
                // 判断记录是否为SA状态
                if (pnrState.Contains("SA"))
                {
                    _response.error = new Error(EtermCommand.ERROR.FIRST_SEGMENT_STATE_SA);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                #endregion

                // 获得航班号：
                flight.FlightNo = cutInfo[1].Replace("*", string.Empty);

                // 根据航班号，验证成人、儿童、婴儿：
                if (!IsChild(rtResult, flight.FlightNo))
                {
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                // 获得舱位代码：
                flight.Cabin = cutInfo[2];
                if ((airInfo.IndexOf("|" + flight.Cabin + "1|") != -1 || airInfo.IndexOf("|" + flight.Cabin + "2|") != -1) && flight.Cabin != "T")
                {
                    string regex = "\\|" + flight.Cabin + "[0-9]{1}";
                    flight.Cabin = Regex.Match(airInfo, regex).Value.Remove(0, 1);
                }

                if (flight.FlightNo.StartsWith("HU") && flight.Cabin.Equals("A") && !rtResult.Contains("OSI HU CKIN SSAC"))
                {
                    _response.error = new Error(EtermCommand.ERROR.HU_CABIN_OSI);
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                // 获得出发日期时间和到达日期时间，格式如：24OCT2021               
                GetSEDatetime(cutInfo, flight);

                // 获得出发城市三字代码
                flight.SCity = cutInfo[4].Substring(0, 3);

                // 获得到达城市三字代码
                flight.ECity = cutInfo[4].Substring(3, 3);

                // 获得航站楼
                // 格式如：
                // 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035 1        E T3--】
                // 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035+1        E T3--】
                // 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 2235        E T3--】
                // 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 2235        E T3】
                // 【2.  PN6253 Y   SA19jun  CKGCAN HK1   0650 0850          E 2B-- Y1】
                // 【 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E      V1】
                // 【2. CZ3237 E WE17JUN NNGSZX RR1  1835 1950     E  】 
                //if (cutInfo.Length > 9 && !string.IsNullOrWhiteSpace(cutInfo[9]) && (cutInfo[9].StartsWith("T") || cutInfo[9].StartsWith("--")))
                if (cutInfo.Length > 9 && !string.IsNullOrWhiteSpace(cutInfo[9]) && (cutInfo[9].StartsWith("T") || cutInfo[9].StartsWith("--") || cutInfo[9].StartsWith("-") || cutInfo[9].Contains("A") || cutInfo[9].Contains("B")))
                {
                    ParserHelper.GetTerminal(cutInfo[9], flight);
                    // 解决航班信息内容【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035 1        E T3--】为这种的问题
                    if (!string.IsNullOrWhiteSpace(flight.DepTerminal)
                        && flight.DepTerminal.Equals("E")
                        && cutInfo.Length > 10)
                    {
                        ParserHelper.GetTerminal(cutInfo[10], flight);
                    }
                }

                // 获得子舱位
                for (int i = cutInfo.Length - 1; i >= 0; --i)
                {
                    string subCabin = cutInfo[i];
                    if (string.IsNullOrWhiteSpace(subCabin))
                    {
                        continue;
                    }

                    // 解决航班信息内容【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035+1        E T3】
                    // 、【PN6253 Y   SA19jun  CKGCAN HK1   0650 0850          E B-】
                    //、【PN6253 Y   SA19jun  CKGCAN HK1   0650 0850          E B2】这3种问题
                    if (Regex.IsMatch(subCabin, @"^\S[0-9]{1}$") && !Regex.IsMatch(subCabin, @"^T[0-9]{1}$") && !Regex.IsMatch(subCabin, @"^(-\S)|(\S-)$"))
                    {
                        flight.SubCabin = subCabin;
                        if ((i - 1) > -1 && cutInfo[i - 1].Equals("E") && subCabin.Equals(flight.DepTerminal))
                        {
                            flight.SubCabin = string.Empty;
                        }
                    }
                    break;
                }

                //// 获得可能有的航站楼和可能有的子舱位信息
                //// 格式如：
                //// 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035 1        E T3--】
                //// 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035+1        E T3--】
                //// 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 2235        E T3--】
                //// 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 2235        E T3】
                //// 【2.  PN6253 Y   SA19jun  CKGCAN HK1   0650 0850          E 2B-- Y1】
                //if (cutInfo.Length > 9)
                //{
                //    for (int i = cutInfo.Length - 1; i >= 0; --i)
                //    {
                //        string value = cutInfo[i];
                //        if (string.IsNullOrWhiteSpace(value))
                //        {
                //            continue;
                //        }

                //        if (value.Equals("E"))
                //        {
                //            // 获得可能有的航站楼信息
                //            if ((i + 1) < cutInfo.Length && !string.IsNullOrWhiteSpace(cutInfo[i + 1]))
                //            {
                //                ParserHelper.GetTerminal(cutInfo[i + 1], flight);
                //            }

                //            // 获得可能有的子舱位信息
                //            if ((i + 2) < cutInfo.Length && !string.IsNullOrWhiteSpace(cutInfo[i + 2]))
                //            {
                //                flight.SubCabin = cutInfo[i + 2];   
                //            }

                //            break;
                //        }
                //    }
                //}                            

                // 判断航班是否为共享航班
                if (cutInfo[1].Contains("*"))
                {
                    _response.result.ShareFlight = true;
                }

                #endregion

                #region 获得第2段航班信息，即获得航班号、舱位、出发日期和时间、到达日期和时间、出发城市、到达城市、航站楼、航班是否为共享航班以及Flight Type这些信息

                int num05 = guestNumber + 3;
                int num06;
                int num11;
                string reInfo;
                try
                {
                    num06 = rtResult.IndexOf(num05.ToString() + ".");
                    reInfo = rtResult.Substring(num04, num06 - num04).Replace(num02.ToString() + ".", string.Empty);
                    if (Regex.Split(reInfo, @"\s{1,}")[1].Trim().Equals("ARNK"))
                    {
                        num11 = rtResult.IndexOf(num05.ToString() + ".");
                        num06 = rtResult.IndexOf(Convert.ToString(num05 + 1) + ".");
                        reInfo = rtResult.Substring(num11, num06 - num11).Replace(num05.ToString() + ".", string.Empty);
                    }
                }
                catch
                {
                    num06 = rtResult.IndexOf("+");
                    reInfo = rtResult.Substring(num04, num06 - num04).Replace(num02.ToString() + ".", string.Empty);
                }
                reInfo = reInfo.Replace(" ", "|").Replace("|||", "|").Replace("||", "|").Replace("||", "|");
                reInfo = ParserHelper.RepalceYear(reInfo);

                string[] cutRInfo = reInfo.Split('|');
                //cutRInfo = cutRInfo.Where<string>(s => !string.IsNullOrWhiteSpace(s)).ToArray<string>();

                string pnrState2 = string.Empty;
                Flight prevFlight = flight;
                flight = new Flight();
                if (cutRInfo.Length > 1 && Regex.IsMatch(cutRInfo[1], @"^\*?\w{2}[0-9]{2,5}$") && !Regex.IsMatch(cutRInfo[1], @"^\*?[0-9]{2,5}$"))
                {
                    #region 根据编码状态，验证RT命令返回结果的有效性

                    pnrState2 = cutRInfo[4].Length > 6 ? cutRInfo[4].Substring(6, cutRInfo[4].Length - 6) : cutRInfo[5]; // 【pnrState2 = cutRInfo[5];】时，记录编号状态， NO，HL
                    flight.PNRState = pnrState2;
                    _response.result.FlightList.Add(flight);
                    // 判断记录是否被NO掉了
                    if (pnrState2.Contains("NO"))
                    {
                        _response.error = new Error(EtermCommand.ERROR.SECOND_SEGMENT_STATE_NO);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }

                    // 判断记录是否为HL状态
                    if (pnrState2.Contains("HL"))
                    {
                        _response.error = new Error(EtermCommand.ERROR.SECOND_SEGMENT_STATE_HL);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }

                    // 判断记录是否为HN状态
                    if (pnrState2.Contains("HN"))
                    {
                        _response.error = new Error(EtermCommand.ERROR.SECOND_SEGMENT_STATE_HN);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }

                    #endregion

                    // 获得航班号、舱位代码、出发日期时间、到达日期时间、出发城市三字代码、到达城市三字代码以及航站楼这些航班信息：
                    // 获得航班号
                    flight.FlightNo = cutRInfo[1].Replace("*", string.Empty);
                    if (!prevFlight.FlightNo.Substring(0, 2).Equals(flight.FlightNo.Substring(0, 2)))
                    {
                        _response.error = new Error(EtermCommand.ERROR.PNR_AIRCOM_NO_INCLUDE);
                        _response.error.CmdResultBag = originalCmdResult;
                        return _response;
                    }
                    GetFlightInfo(cutRInfo, reInfo, flight);

                    // 判断航班是否为共享航班
                    if (cutRInfo[1].Contains("*"))
                    {
                        _response.result.ShareFlight = true;
                    }

                    #region 获得Flight Type

                    _response.result.FlightType = EtermCommand.FlightType.F; // 往返

                    // 判断记录是否为往返程
                    string multiCityA = "SHA/PVG";
                    string multiCityB = "PEK/NAY";
                    if (
                        (!prevFlight.SCity.Equals(flight.ECity) &&
                            (
                                (multiCityA.Contains(prevFlight.SCity) && multiCityA.Contains(flight.ECity))
                                || (multiCityB.Contains(prevFlight.SCity) && multiCityB.Contains(flight.ECity))
                            )
                         )
                         && (!prevFlight.ECity.Equals(flight.SCity)
                              && (
                                    (multiCityA.Contains(prevFlight.ECity) && multiCityA.Contains(flight.SCity))
                                     || (multiCityB.Contains(prevFlight.ECity) && multiCityB.Contains(flight.SCity))
                                  )
                             )
                       )
                    {
                        _response.result.FlightType = EtermCommand.FlightType.F; // 往返
                    }

                    bool multiA = false;
                    bool multiB = false;
                    if (multiCityA.IndexOf(prevFlight.SCity) != -1 && multiCityA.IndexOf(flight.ECity) != -1)
                    {
                        multiA = true;
                    }
                    if (multiCityB.IndexOf(prevFlight.ECity) != -1 && multiCityB.IndexOf(flight.SCity) != -1)
                    {
                        multiB = true;
                    }
                    if ((!prevFlight.SCity.Equals(flight.ECity) && multiA.Equals(false)) || (!prevFlight.ECity.Equals(flight.SCity) && multiB.Equals(false)))
                    {
                        _response.result.FlightType = EtermCommand.FlightType.T; // 联程
                    }

                    #endregion
                }

                #endregion

#warning will test
                // 获得第3段和第4段航班信息，即获得航班号、舱位、出发日期和时间、到达日期和时间、出发城市、到达城市、航站楼、航班是否为共享航班以及Flight Type这些信息：
                if (!SetLianCheng(rtResult, num04, num05, num06))
                {
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                #endregion                
            }
            catch
            {
                _response.error = new Error(EtermCommand.ERROR.PNR_NO_CREATE_ORDER);
                _response.error.CmdResultBag = originalCmdResult;
                return _response;
            }

            // 解析价格
            if (isGetPrice)
            {
                JetermEntity.Parser.GetPrice getPrice = new JetermEntity.Parser.GetPrice();

                // 初始化PAT价格请求参数
                Request.GetPrice getPriceRequest = new Request.GetPrice();
                getPriceRequest.PassengerType = passengerType;
                getPriceRequest.FlightType = _response.result.FlightType;
                getPriceRequest.FlightList = _response.result.FlightList;
                // 验证PAT价格请求参数
                getPrice.ParseCmd(getPriceRequest);

                CommandResult<JetermEntity.Response.GetPrice> getPriceResponse = getPrice.ParseCmdResult(priceCmdResult);
                if (!getPriceResponse.state)
                {
                    _response.error = getPriceResponse.error;
                    _response.error.CmdResultBag = originalCmdResult;
                    return _response;
                }

                _response.result.PriceList.AddRange(getPriceResponse.result.PriceList);
            }

            _response.state = true;
            return _response;
        }

        #region Helper

        protected internal override bool ValidRequest()
        {
            if (_request == null || string.IsNullOrWhiteSpace(_request.Pnr) || _request.PassengerType.GetHashCode() == EtermCommand.PassengerType.NotSet.GetHashCode())
            {
                _response.error = new Error(EtermCommand.ERROR.EMPTY_REQUEST_PARAM);
                return false;
            }

            _request.Pnr = Regex.Replace(_request.Pnr, @"\s", string.Empty).Trim().ToUpper();
            if (!Regex.IsMatch(_request.Pnr, @"^[A-Za-z0-9]{6}$"))
            {
                _response.error = new Error(EtermCommand.ERROR.NOT_EXIST_PNR);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(_request.Airline))
            {
                _request.Airline = Regex.Replace(_request.Airline, @"\s", string.Empty).Trim().ToUpper();
            }

            return true;
        }

        protected internal override bool ValidCmdResult(string cmdResult)
        {
            _response.error = ParserHelper.ValidRTResult(cmdResult);

            if (_response.error == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获得成人/儿童乘客姓名，并设置guestNumber
        /// </summary>
        /// <param name="rtResult"></param>   
        private void GetAdultOrChdName(string rtResult)
        {
            // 所有乘客姓名是否为中文名字：
            //bool isChineseName = true;

            Passenger passenger = null;

            // 获得成人/儿童乘机人姓名：
            for (int i = 1; i <= guestNumber; i++)
            {
                string str01 = i.ToString() + ".";
                int n = rtResult.IndexOf(str01); // 当前处理的乘客名所在开始位置

                int j = i + 1;
                string str02 = j.ToString() + ".";
                int m = rtResult.IndexOf(str02); // 下一个乘客名所在开始位置

                string guestName = string.Empty;
                guestName = rtResult.Substring(n, m - n).Replace(str01, string.Empty).Trim();

                if (!guestName.Contains(pnr))
                {
                    passenger = new Passenger();
                    passenger.name = guestName;
                    passenger.PassType = passenger.name.EndsWith("CHD") ? EtermCommand.PassengerType.Children : EtermCommand.PassengerType.Adult;
                    _response.result.PassengerList.Add(passenger);
                    continue;
                }

                // 处理【JAYET/ANTOINE XAVIER MARIE KGR8QY】的情况
                string[] name = Regex.Split(guestName, @"\s+"); 
                if (name != null && name.Length > 1 && !name[name.Length - 1].Equals(pnr))
                {
                    pnr = name[name.Length - 1];
                    if (pnr.Length > 6)
                    {
                        pnr = pnr.Substring(0, 6);
                    }
                    _response.result.PNR = pnr;
                }

                passenger = new Passenger();
                passenger.name = guestName.Substring(0, guestName.IndexOf(pnr)).Trim();
                passenger.PassType = passenger.name.EndsWith("CHD") ? EtermCommand.PassengerType.Children : EtermCommand.PassengerType.Adult;
                _response.result.PassengerList.Add(passenger);

                guestNumber = i;
                break;
            }
        }

        /// <summary>
        /// 获得婴儿乘客姓名、姓名拼音、出生日期以及婴儿总数
        /// </summary>
        /// <param name="rtResult"></param>   
        /// <returns></returns>     
        private bool GetBabyInfo(string rtResult)
        {
            if (passengerType.GetHashCode() != EtermCommand.PassengerType.Baby.GetHashCode())
            {
                return true;
            }

            #region 获得婴儿乘客姓名

            Regex reg = new Regex(@"XN/IN/(?<Name>(\w|/|\s*)+)INF\((?<Month>[A-Z]{3})(?<Year>\d{2})\)/P\d{1,3}");
            MatchCollection match = reg.Matches(rtResult);
            if (match == null || match.Count < 1)
            {
                _response.error = new Error(EtermCommand.ERROR.AUDLT_NO_BABY);
                return false;
            }
            // 判断婴儿人数是否超过成人人数              
            if (match.Count > guestNumber)
            {
                _response.error = new Error(EtermCommand.ERROR.ONE_PASSENGER_ON_BABY);
                return false;
            }

            for (int i = 0; i < match.Count; i++)
            {
                Passenger passenger = new Passenger();
                passenger.name = match[i].Groups["Name"].Value;
                passenger.PassType = EtermCommand.PassengerType.Baby;
                _response.result.PassengerList.Add(passenger);
            }

            #endregion

            //infCount = match.Count; // 获得婴儿数  

            #region 获得婴儿姓名拼音以及其出生日期

            List<Passenger> babyPassengerList = new List<Passenger>();
            if (_response.result.PassengerList != null && _response.result.PassengerList.Any())
            {
                IEnumerable<Passenger> tempBabyList = _response.result.PassengerList.Where<Passenger>(p => p.PassType.GetHashCode() == EtermCommand.PassengerType.Baby.GetHashCode());
                if (tempBabyList != null && tempBabyList.Count() > 0)
                {
                    babyPassengerList.AddRange(tempBabyList.ToList());
                }
            }

            reg = new Regex(@"\d{1,3}\.SSR\s*INFT\s*[A-Z]{2} \s*(?<Status>[A-Z]{2})\d{1,2}\s*[A-Z]{6}\s*\d{4} \s*[A-Z]{1}\d{2}[A-Z]{3}\s*(?<Name>[\w/ ]*)\s*(?<Date>\d{2}[A-Z]{3}\d{2})/P\d{1,3}");
            match = reg.Matches(rtResult);
            if (match != null && match.Count > 0)
            {
                for (int i = 0; i < match.Count; i++)
                {
                    string status = match[i].Groups["Status"].Value;
                    if (!status.Contains("KK") && !status.Contains("HK"))
                    {
                        // 婴儿航段状态不对，请检查！
                        _response.error = new Error(EtermCommand.ERROR.INCORRECT_BABY_FLIGHTINFO);
                        return false;
                    }

                    // date获得的格式如：12OCT13
                    string date = match[i].Groups["Date"].Value;
                    string nowyear = date.Substring(5, 2);
                    string nowmonth = ParserHelper.GetNumMonth(date.Substring(2, 3));
                    DateTime nowdate = Convert.ToDateTime(nowyear + "-" + nowmonth + "-" + date.Substring(0, 2));
                    if (nowdate.AddYears(2) < DateTime.Now || nowdate.AddDays(14) > DateTime.Now)
                    {
                        // 出生年月非婴儿，请核实！
                        _response.error = new Error(EtermCommand.ERROR.INCORRECT_BABY_BIRTHDAY);
                        return false;
                    }

                    if (babyPassengerList == null || !babyPassengerList.Any()) // 不会出现这种情况，因为成人编码必须包含婴儿航段
                    {
                        continue;
                    }

                    if (i < babyPassengerList.Count)
                    {
                        foreach (Passenger p in _response.result.PassengerList)
                        {
                            if (p.PassType.GetHashCode() != EtermCommand.PassengerType.Baby.GetHashCode())
                            {
                                continue;
                            }
                            if (!p.name.Equals(babyPassengerList[i].name))
                            {
                                continue;
                            }

                            p.Ename = match[i].Groups["Name"].Value;
                            p.BabyBirthday = ParserHelper.ConvertStringToDateTime(date);
                            break;
                        }
                    }
                }
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 获得各成人/儿童乘客的ID号、ID类型、票号以及姓名拼音
        /// </summary>
        /// <param name="rtResult"></param>
        private void GetAdultInfo(string rtResult)
        {
            if (_response.result.PassengerList == null || !_response.result.PassengerList.Any())
            {
                return;
            }

            try
            {
                #region 获得各成人/儿童乘客的ID号、ID类型

                Dictionary<string, string> dic = new Dictionary<string, string>();
                MatchCollection mcCard = null;
                if (Regex.IsMatch(rtResult, @"NI[a-zA-Z]{0,}[\d]+[a-zA-Z]{0,}/P"))
                {
                    mcCard = Regex.Matches(rtResult, @"NI[a-zA-Z]{0,}[\d]+[a-zA-Z]{0,}/P");
                }
                if (Regex.IsMatch(rtResult, @"ID[a-zA-Z]{0,}[\d]+[a-zA-Z]{0,}/P"))
                {
                    mcCard = Regex.Matches(rtResult, @"ID[a-zA-Z]{0,}[\d]+[a-zA-Z]{0,}/P");
                }
                if (Regex.IsMatch(rtResult, @"PP[a-zA-Z]{0,}[\d]+[a-zA-Z]{0,}/P"))
                {
                    mcCard = Regex.Matches(rtResult, @"PP[a-zA-Z]{0,}[\d]+[a-zA-Z]{0,}/P");
                }
                if (mcCard != null)
                {
                    //result.state = true;
                    foreach (Match mc in mcCard)
                    {
                        // nString和nInt分别表示第几位成人乘客：
                        string nString = rtResult.Substring(rtResult.IndexOf(mc.Value), mc.Value.Length + 1).Replace(mc.Value, string.Empty);
                        int nInt = -1;
                        // 获得的mc.Value格式如: NI13092119870104402X/P
                        string idCardNo = mc.Value.Replace("/P", string.Empty);
                        if (int.TryParse(nString, out nInt) && nInt > 0)
                        {
                            _response.result.PassengerList[nInt - 1].cardno = idCardNo;
                            if (idCardNo.StartsWith("NI"))
                            {
                                _response.result.PassengerList[nInt - 1].idtype = EtermCommand.IDtype.IDcard;
                                continue;
                            }
                            if (idCardNo.StartsWith("ID"))
                            {
                                _response.result.PassengerList[nInt - 1].idtype = EtermCommand.IDtype.Other;
                                continue;
                            }
                            if (idCardNo.StartsWith("PP"))
                            {
                                _response.result.PassengerList[nInt - 1].idtype = EtermCommand.IDtype.Other;
                                continue;
                            }
                        }
                    }
                }

                #endregion

                // 获得各成人/儿童乘客的票号、姓名拼音
                GetTicketNoAndEName();
            }
            catch
            {
                // do nothing
            }
        }

        /// <summary>
        /// 获得儿童出生日期
        /// </summary>
        /// <param name="rtResult"></param>
        private void GetChildBirthday(string rtResult)
        {
            if (passengerType.GetHashCode() != EtermCommand.PassengerType.Children.GetHashCode())
            {
                return;
            }

            if (_response.result.PassengerList == null || !_response.result.PassengerList.Any())
            {
                return;
            }

            MatchCollection mc = Regex.Matches(rtResult, @"SSR\s*CHLD\s*\S{2}\s*\S{2}\d{1}\s*(\S+)/P(\d{1,})");
            if (mc == null || mc.Count < 1)
            {
                return;
            }

            Match match = null;
            for (int index = 0; index < mc.Count; index++)
            {
                match = mc[index];
                if (match == null || match.Groups.Count < 2)
                {
                    continue;
                }

                int i = -1;
                int.TryParse(mc[index].Groups[2].Value.Trim(), out i);
                if (i < 1 || i > _response.result.PassengerList.Count)
                {
                    continue;
                }
                // 传入格式如：11DEC14
                _response.result.PassengerList[i - 1].ChildBirthday = ParserHelper.ConvertStringToDateTime(mc[index].Groups[1].Value.Trim());
            }
        }

        /// <summary>
        /// 获得各成人/儿童乘客的票号、姓名拼音
        /// </summary>        
        private void GetTicketNoAndEName()
        {
            List<Passenger> passengerList = _response.result.PassengerList;
            int length = passengerList.Count;

            Regex reg = new Regex(@"OSI\s1E\s\S+ET\sTN/(?<TicketNo>\d+[-]*\d+)(?<PinYinName>\s1\S+)*");
            MatchCollection match = reg.Matches(_response.result.ResultBag);
            if (match != null && match.Count > 0)
            {
                for (int i = 0; i < match.Count; i++)
                {
                    if (i > (length - 1))
                    {
                        return;
                    }

                    if (passengerList[i].PassType.GetHashCode() == EtermCommand.PassengerType.Baby.GetHashCode())
                    {
                        continue;
                    }
                    passengerList[i].TicketNo = match[i].Groups["TicketNo"].Value.Replace("-", string.Empty).Trim();
                    passengerList[i].Ename = match[i].Groups["PinYinName"].Value.Replace(" 1", string.Empty).Trim();
                }
            }

            reg = new Regex(@"SSR\sTKNE\s\S+\s\S{2}1\s\S{6}\s\S+\s\S+\s(?<TicketNo>\d+[-]*\d+)/1/P(?<PassengerNumber>\d+)\s*");
            match = reg.Matches(_response.result.ResultBag);
            if (match == null || match.Count < 1)
            {
                return;
            }

            for (int i = 0; i < match.Count; i++)
            {
                int passengerNumber = -1;
                if (!int.TryParse(match[i].Groups["PassengerNumber"].Value.Trim(), out passengerNumber))
                {
                    continue;
                }
                if (passengerNumber < 1)
                {
                    continue;
                }
                    
                if (passengerNumber > length)
                {
                    return;
                }

                if (passengerList[passengerNumber - 1].PassType.GetHashCode() == EtermCommand.PassengerType.Baby.GetHashCode())
                {
                    continue;
                }
                passengerList[passengerNumber - 1].TicketNo = match[i].Groups["TicketNo"].Value.Replace("-", string.Empty).Trim();
            }            
        }

        /// <summary>
        /// 成人、儿童、婴儿验证
        /// </summary>
        /// <param name="rtResult"></param>
        /// <param name="flightNo">航班号</param>      
        /// <returns></returns>       
        private bool IsChild(string rtResult, string flightNo)
        {
            if (_response == null || _response.result.PassengerList == null || !_response.result.PassengerList.Any())
            {
                return true;
            }

            bool res = true;

            for (int i = 0; i < _response.result.PassengerList.Count; ++i)
            {
                Passenger passenger = _response.result.PassengerList[i];

                switch (passengerType)
                {
                    case EtermCommand.PassengerType.Adult:
                        if (passenger.name.EndsWith("CHD") || (rtResult.Contains("SSR CHLD") && flightNo.StartsWith("CZ")))
                        {
                            _response.error = new Error(EtermCommand.ERROR.PNR_USE_CHILD);
                            res = false;
                            break;
                        }
                        break;
                    case EtermCommand.PassengerType.Baby:
                        if (!isGetPrice)
                        {
                            _response.error = new Error(EtermCommand.ERROR.NEED_PAT);
                            res = false;
                            break;
                        }
                        break;
                    case EtermCommand.PassengerType.Children:
                        if (flightNo.StartsWith("CZ") && !rtResult.Contains("SSR CHLD"))
                        {
                            _response.error = new Error(EtermCommand.ERROR.CZ_CHILD_NEED_SSR);
                            res = false;
                            break;
                        }

                        if (!passenger.name.EndsWith("CHD"))
                        {
                            _response.error = new Error(EtermCommand.ERROR.PNR_CONTACT_AUDLT);
                            res = false;
                            break;
                        }
                        break;
                }
            }

            return res;
        }

        /// <summary>
        /// 获得起飞日期时间和到达日期时间，格式如：24OCT2021
        /// </summary>
        /// <param name="cutInfo"></param>
        /// <param name="flight"></param>    
        private void GetSEDatetime(string[] cutInfo, JetermEntity.Flight flight)
        {
            if (cutInfo == null || cutInfo.Length < 4)
            {
                return;
            }

            string sDate = cutInfo[3]; //出发日期
            if (!string.IsNullOrWhiteSpace(sDate) && sDate.Length > 6)
            {
                // 获得格式如：24OCT
                sDate = sDate.Substring(2, 5);
            }

            // 起飞时间、到达时间
            string sTime = string.Empty, eTime = string.Empty;
            if (cutInfo[4].Length > 6)
            {
                sTime = ParserHelper.CheckFilghtTime(cutInfo[5]); //起飞时间
                eTime = ParserHelper.CheckFilghtTime(cutInfo[6].Substring(0, 4)); //到达时间
            }
            else
            {
                sTime = ParserHelper.CheckFilghtTime(cutInfo[6]); //起飞时间
                eTime = ParserHelper.CheckFilghtTime(cutInfo[7].Substring(0, 4)); //到达时间
            }

            // 获得出发日期时间和到达日期时间           
            string sDateWithoutTime = sDate;
            sDate = string.Format("{0}{1}", sDate, sTime);
            string eDate = string.Format("{0}{1}", sDateWithoutTime, eTime);
            if ((!string.IsNullOrWhiteSpace(sDate) && sDate.Length == 9)
                &&
                (!string.IsNullOrWhiteSpace(eDate) && eDate.Length == 9))
            {
                // 传入格式如：24OCT2021
                DateTime sDateTime = ParserHelper.ConvertStringToDateTime(sDate);
                DateTime eDateTime = ParserHelper.ConvertStringToDateTime(eDate);
                if (sDateTime > eDateTime)
                {
                    eDateTime = eDateTime.AddDays(1);
                }

                flight.DepDate = sDateTime;
                flight.ArrDate = eDateTime;
            }
        }

        /// <summary>
        /// 获得航班号、舱位代码、出发日期时间、到达日期时间、出发城市三字代码、到达城市三字代码以及航站楼这些航班信息
        /// </summary>
        /// <param name="cutInfo"></param>
        /// <param name="info"></param>
        /// <param name="flight"></param>    
        private void GetFlightInfo(string[] cutInfo, string info, JetermEntity.Flight flight)
        {
            // 获得航班号
            flight.FlightNo = cutInfo[1].Replace("*", string.Empty);

            // 获得舱位代码、子舱位代码
            flight.Cabin = cutInfo[2];
            if (flight.FlightNo.StartsWith("CA") && info.Contains("|" + flight.Cabin + "1"))
            {
                flight.Cabin = flight.Cabin + "1";
                flight.SubCabin = flight.Cabin;
            }

            // 获得出发日期时间和到达日期时间，格式如：24OCT2021         
            GetSEDatetime(cutInfo, flight);

            // 获得出发城市三字代码
            flight.SCity = cutInfo[4].Substring(0, 3);

            // 获得到达城市三字代码  
            flight.ECity = cutInfo[4].Substring(3, 3);

            // 获得航站楼
            // 格式如：
            // 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035 1        E T3--】
            // 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035+1        E T3--】
            // 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 2235        E T3--】
            // 【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 2235        E T3】
            // 【2.  PN6253 Y   SA19jun  CKGCAN HK1   0650 0850          E 2B-- Y1】
            // 【 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E      V1】
            // 【2. CZ3237 E WE17JUN NNGSZX RR1  1835 1950     E  】          
            if (cutInfo.Length > 9 && !string.IsNullOrWhiteSpace(cutInfo[9]) && (cutInfo[9].StartsWith("T") || cutInfo[9].StartsWith("--") || cutInfo[9].StartsWith("-") || cutInfo[9].Contains("A") || cutInfo[9].Contains("B")))
            {
                ParserHelper.GetTerminal(cutInfo[9], flight);
                // 解决航班信息内容【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035 1        E T3--】为这种的问题
                if (!string.IsNullOrWhiteSpace(flight.DepTerminal)
                    && flight.DepTerminal.Equals("E")
                    && cutInfo.Length > 10)
                {
                    ParserHelper.GetTerminal(cutInfo[10], flight);
                }
            }

            // 获得子舱位
            if (!flight.FlightNo.StartsWith("CA"))
            {
                for (int i = cutInfo.Length - 1; i >= 0; --i)
                {
                    string subCabin = cutInfo[i];
                    if (string.IsNullOrWhiteSpace(subCabin))
                    {
                        continue;
                    }               
                   
                    // 解决航班信息内容【3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035+1        E T3】
                    // 、【PN6253 Y   SA19jun  CKGCAN HK1   0650 0850          E B-】
                    //、【PN6253 Y   SA19jun  CKGCAN HK1   0650 0850          E B2】这3种问题
                    if (Regex.IsMatch(subCabin, @"^\S[0-9]{1}$") && !Regex.IsMatch(subCabin, @"^T[0-9]{1}$") && !Regex.IsMatch(subCabin, @"^(-\S)|(\S-)$"))
                    {
                        flight.SubCabin = subCabin;
                        if ((i - 1) > -1 && cutInfo[i - 1].Equals("E") && subCabin.Equals(flight.DepTerminal))
                        {
                            flight.SubCabin = string.Empty;
                        }
                    }
                    break;
                }
            }

            //// 获得可能有的航站楼信息和可能有的子舱位信息
            //if (cutInfo.Length > 9)
            //{
            //    for (int i = cutInfo.Length - 1; i >= 0; --i)
            //    {
            //        string value = cutInfo[i];
            //        if (string.IsNullOrWhiteSpace(value))
            //        {
            //            continue;
            //        }

            //        if (value.Equals("E"))
            //        {
            //            // 获得可能有的航站楼信息
            //            if ((i + 1) < cutInfo.Length && !string.IsNullOrWhiteSpace(cutInfo[i + 1]))
            //            {
            //                ParserHelper.GetTerminal(cutInfo[i + 1], flight);
            //            }

            //            // 获得可能有的子舱位信息
            //            if (!flight.FlightNo.StartsWith("CA") && (i + 2) < cutInfo.Length && !string.IsNullOrWhiteSpace(cutInfo[i + 2]))
            //            {
            //                flight.SubCabin = cutInfo[i + 2];
            //            }

            //            break;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 分别获得第3段和第4段航班信息
        /// </summary>
        /// <param name="rtResult"></param>
        /// <param name="num04"></param>
        /// <param name="num05"></param>
        /// <param name="num06"></param>    
        /// <returns></returns>
        private bool SetLianCheng(string rtResult, int num04, int num05, int num06)
        {
            try
            {
                string[] cutRinfo;
                int Tnum05 = guestNumber + 4;
                int Tnum06;
                int Tnum11;
                string pnrState3;
                string Treinfo;
                try
                {
                    Tnum06 = rtResult.IndexOf(Tnum05.ToString() + ".");
                    Treinfo = rtResult.Substring(num06, Tnum06 - num06).Replace(num04.ToString() + ".", string.Empty);
                    if (Regex.Split(Treinfo, @"\s{1,}")[1].Trim().Equals("ARNK"))
                    {
                        Tnum11 = rtResult.IndexOf(Tnum05.ToString() + ".");
                        Tnum06 = rtResult.IndexOf(Convert.ToString(num05 + 1) + ".");
                        Treinfo = rtResult.Substring(Tnum11, Tnum06 - Tnum11).Replace(Tnum05.ToString() + ".", string.Empty);
                    }
                }
                catch
                {
                    Tnum06 = rtResult.IndexOf("+");
                    Treinfo = rtResult.Substring(num06, Tnum06 - num06).Replace(num04.ToString() + ".", string.Empty);
                }
                Treinfo = Treinfo.Replace(" ", "|").Replace("|||", "|").Replace("||", "|").Replace("||", "|");
                Treinfo = ParserHelper.RepalceYear(Treinfo);

                cutRinfo = Treinfo.Split('|');

                if (cutRinfo.Length > 1 && Regex.IsMatch(cutRinfo[1], @"^\*?\w{2}[0-9]{2,5}$") && !Regex.IsMatch(cutRinfo[1], @"^\*?[0-9]{2,5}$")) // 有第三段航班
                {
                    #region 获得第3段航班信息，即获得航班号、舱位、出发日期和时间、到达日期和时间、出发城市、到达城市、航站楼以及航班是否为共享航班这些信息

                    Flight flight = new Flight();

                    #region 根据编码状态，验证RT命令返回结果的有效性

                    pnrState3 = cutRinfo[4].Length > 6 ? cutRinfo[4].Substring(6, cutRinfo[4].Length - 6) : cutRinfo[5]; // 【pnrState3 = cutRinfo[5]】时，记录编号状态， NO，HL
                    if (string.IsNullOrWhiteSpace(pnrState3))
                    {
                        return true;
                    }
                    flight.PNRState = pnrState3;
                    _response.result.FlightList.Add(flight);
                    // 判断记录是否被NO掉
                    if (pnrState3.Contains("NO"))
                    {
                        _response.error = new Error(EtermCommand.ERROR.THIRD_SEGMENT_STATE_NO);
                        return false;
                    }

                    // 判断记录是否为HL状态
                    if (pnrState3.Contains("HL"))
                    {
                        _response.error = new Error(EtermCommand.ERROR.THIRD_SEGMENT_STATE_HL);
                        return false;
                    }

                    // 判断记录是否为HN状态
                    if (pnrState3.Contains("HN"))
                    {
                        _response.error = new Error(EtermCommand.ERROR.THIRD_SEGMENT_STATE_HN);
                        return false;
                    }

                    #endregion

                    // 获得航班号、舱位代码、出发日期时间、到达日期时间、出发城市三字代码、到达城市三字代码以及航站楼这些航班信息
                    GetFlightInfo(cutRinfo, Treinfo, flight);

                    // 判断第3段航班是否为共享航班
                    if (cutRinfo[1].Contains("*"))
                    {
                        _response.result.ShareFlight = true;
                    }

                    #endregion

                    #region 获得第4段航班信息，即获得航班号、舱位、出发日期和时间、到达日期和时间、出发城市、到达城市、航站楼、航班是否为共享航班以及Flight Type这些信息

                    int Tnum07 = guestNumber + 5;
                    int Tnum08;
                    int Tnum13;
                    string pnrState4;
                    try
                    {
                        Tnum08 = rtResult.IndexOf(Tnum07.ToString() + ".");
                        Treinfo = rtResult.Substring(Tnum06, Tnum08 - Tnum06).Replace(num06.ToString() + ".", string.Empty);
                        if (Regex.Split(Treinfo, @"\s{1,}")[1].Trim().Equals("ARNK"))
                        {
                            Tnum13 = rtResult.IndexOf(Tnum07.ToString() + ".");
                            Tnum08 = rtResult.IndexOf(Convert.ToString(Tnum05 + 1) + ".");
                            Treinfo = rtResult.Substring(Tnum13, Tnum08 - Tnum13).Replace(Tnum07.ToString() + ".", string.Empty);
                        }
                    }
                    catch
                    {
                        Tnum08 = rtResult.IndexOf(Tnum07.ToString() + ".");
                        Treinfo = rtResult.Substring(Tnum06, Tnum08 - Tnum06).Replace(num06.ToString() + ".", string.Empty);
                    }
                    Treinfo = Treinfo.Replace(" ", "|").Replace("|||", "|").Replace("||", "|").Replace("||", "|");
                    Treinfo = ParserHelper.RepalceYear(Treinfo);

                    cutRinfo = Treinfo.Split('|');

                    flight = new Flight();

                    if (cutRinfo.Length > 1 && Regex.IsMatch(cutRinfo[1], @"^\*?\w{2}[0-9]{2,5}$") && !Regex.IsMatch(cutRinfo[1], @"^\*?[0-9]{2,5}$")) // 有第四段航班
                    {
                        #region 根据编码状态，验证RT命令返回结果的有效性

                        pnrState4 = cutRinfo[4].Length > 6 ? cutRinfo[4].Substring(6, cutRinfo[4].Length - 6) : cutRinfo[5]; // 【pnrState4 = cutRinfo[5]】时，记录编号状态， NO，HL
                        if (string.IsNullOrWhiteSpace(pnrState4))
                        {
                            return true;
                        }
                        flight.PNRState = pnrState4;
                        _response.result.FlightList.Add(flight);
                        // 判断记录是否被NO掉了
                        if (pnrState4.Contains("NO"))
                        {
                            _response.error = new Error(EtermCommand.ERROR.FORTH_SEGMENT_STATE_NO);
                            return false;
                        }

                        //判断记录是否为HL状态
                        if (pnrState4.Contains("HL"))
                        {
                            _response.error = new Error(EtermCommand.ERROR.FORTH_SEGMENT_STATE_HL);
                            return false;
                        }

                        //判断记录是否为HN状态
                        if (pnrState4.Contains("HN"))
                        {
                            _response.error = new Error(EtermCommand.ERROR.FORTH_SEGMENT_STATE_HN);
                            return false;
                        }

                        #endregion

                        // 获得航班号、舱位代码、出发日期时间、到达日期时间、出发城市三字代码、到达城市三字代码以及航站楼这些航班信息                        
                        GetFlightInfo(cutRinfo, Treinfo, flight);

                        // 判断航班是否为共享航班
                        if (cutRinfo[1].Contains("*"))
                        {
                            _response.result.ShareFlight = true;
                        }

                        // 获得Flight Type
                        _response.result.FlightType = EtermCommand.FlightType.T; //往返
                    }

                    #endregion

                    _response.result.FlightType = EtermCommand.FlightType.T; // 联程
                }
            }
            catch
            {
                return true;
            }

            return true;
        }

        private void GetMobileAndPhone(string rtResult)
        {
            MatchCollection mc = Regex.Matches(rtResult, @"OSI\s+\S+\s+CTCT\S+");           
            if (mc == null || mc.Count < 1)
            {
                return;
            }

            _response.result.Mobile = Regex.Replace(mc[0].Value, @"OSI\s+\S+\s+CTCT\s*", string.Empty).Trim();
            if (mc.Count > 1)
            {
                _response.result.PhoneNo = Regex.Replace(mc[1].Value, @"OSI\s+\S+\s+CTCT\s*", string.Empty).Trim();
            }
        }

        /// <summary>
        /// 获取授权OFFICE号（可能有多个）
        /// </summary>
        /// <param name="rtResult"></param>    
        private void GetRMKOfficeNoList(string rtResult)
        {
            MatchCollection mc = Regex.Matches(rtResult, @"RMK\s+TJ\s+AUTH\s*\S+");
            if (mc == null || mc.Count < 1)
            {
                return;
            }

            for (int index = 0; index < mc.Count; index++)
            {
                string rmkOfficeNo = Regex.Replace(mc[index].Value, @"RMK\s+TJ\s+AUTH\s*", string.Empty).Trim();

                _response.result.RMKOfficeNoList.Add(rmkOfficeNo);
            }
        }

        #endregion
    }
}
