using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetermEntity.Response;
using System.Text.RegularExpressions;

namespace JetermEntity.Parser
{
    /// <summary>
    /// 解析获取价格PAT指令
    /// </summary>
    [Serializable]
    public class GetPrice : ParserBase<JetermEntity.Request.GetPrice, CommandResult<JetermEntity.Response.GetPrice>>
    {
        #region 成员变量

        private JetermEntity.Request.GetPrice _request = null;

        private CommandResult<JetermEntity.Response.GetPrice> _response = null;

        /// <summary>
        /// 请求对象
        /// </summary>
        public override JetermEntity.Request.GetPrice Request { get { return this._request; } }

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.GetPrice> Response { get { return this._response; } }

        #endregion

        /// <summary>
        /// 构造获取价格PAT指令返回对象
        /// </summary>
        public GetPrice()
        {
            _response = new CommandResult<JetermEntity.Response.GetPrice>();

            _response.result = new JetermEntity.Response.GetPrice();
        }

        public GetPrice(string config, string officeNo)
            : this()
        {
            _response.config = config;
            _response.OfficeNo = officeNo;
        }

        /// <summary>
        /// 解析请求对象
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回获取价格PAT指令；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.GetPrice request)
        {
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }

            string cmd = string.Empty;
            switch (request.PassengerType)
            {
                case EtermCommand.PassengerType.Adult:
                    cmd = "PAT:A";
                    break;
                case EtermCommand.PassengerType.Children:
                    cmd = "PAT:A*CH";
                    break;
                case EtermCommand.PassengerType.Baby:
                    cmd = "PAT:A*IN";
                    break;
            }

            return cmd;
        }

        /// <summary>
        /// 解析获取价格PAT指令返回结果
        /// </summary>
        /// <param name="cmdResult">获取价格PAT指令返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.GetPrice> ParseCmdResult(string cmdResult)
        {
            if (_request == null || _request.PassengerType.GetHashCode() == EtermCommand.PassengerType.NotSet.GetHashCode())
            {
                // 请先初始化请求对象
                _response.error = new Error(EtermCommand.ERROR.FIRST_INITIALISE_REQUEST);
                return _response;
            }

            Flight firstFlight = null;
            if (_request.FlightList == null || !_request.FlightList.Any())
            {
                firstFlight = new Flight();
            }
            else
            {
                firstFlight = _request.FlightList[0];
            }              

            // 获取价格PAT指令返回结果验证：
            if (!ValidCmdResult(cmdResult))
            {
                _response.error.CmdResultBag = cmdResult;
                return _response;
            }

            string priceResult = cmdResult.Trim();

            // 获得的结果，如：>PAT:A|01|Y|FARE:CNY1580.00|TAX:CNY50.00|YQ:CNY30.00|TOTAL:1660.00|SFC:01|SFN:01|
            priceResult = Regex.Replace(priceResult, @"\r|\n|\r\n|\s+", "|");

            string[] cutPrice = priceResult.Split('|');            
            //string priceRegexExpression = @"FARE:CNY[0-9]{1,}\.[0-9]{1,}\s+TAX:CNY([0-9]{1,}\.[0-9]{1,}|[A-Z]{1,})\s+YQ:CNY([0-9]{1,}\.[0-9]{1,}|[A-Z]{1,})\s+TOTAL:[0-9]{1,}\.[0-9]{1,}";
            //int j = -1;
            string getTax = string.Empty;
            string getFuel = string.Empty;
            string valueString = string.Empty;
            //decimal valueDecimal = 0;
            Price price = new Price();
            _response.result.PriceList = new List<Price>();

            // Step1、获得儿童票/成人票的票面价（列表）
           
            // 获得票面价（列表）
            // 如：>PAT:A*CH 01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:810.00 SFC:01 SFN:01 02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:910.00 SFC:02 SFN:02
            string tempPriceResult = Regex.Replace(cmdResult.Trim(), @"\r|\n|\r\n|\s+", " ");
            //GetFacePriceList(priceResult, firstFlight); 
            GetFacePriceList(tempPriceResult, firstFlight);

            // Step2、获得儿童往返票/儿童联程票/儿童单程票/成人往返票/成人联程票/成人单程票的机建费、燃油税、总价（列表）
            GetTaxFuelTotal(tempPriceResult, firstFlight);

//            switch (_request.PassengerType)
//            {
//                case EtermCommand.PassengerType.Children: // A、开始：情况1：儿童票
//                    {
//                        //if (_request.FlightType.GetHashCode() != EtermCommand.FlightType.O.GetHashCode())
//                        //{
//                        //    // ErrorMessage：当前暂不支持往返儿童票，请将往返儿童票拆成两个单程编码导入
//                        //    _response.error = new Error(EtermCommand.ERROR.NOT_SUPPORT_ROUNDTRIP_CHILDTICKET);
//                        //    _response.error.CmdResultBag = cmdResult;
//                        //    return _response;
//                        //}

//                        // Step2、获得儿童往返票/儿童联程票/儿童单程票的机建费、燃油税、总价（列表）

//                        //GetTaxFuelTotal(cutPrice);
//                        GetTaxFuelTotal(tempPriceResult, firstFlight);                                                              

//                        //switch (_request.FlightType)
//                        //{
//                        //    case EtermCommand.FlightType.F: // 开始：A2、A3、儿童往返票或儿童联程票或儿童单程票
//                        //    case EtermCommand.FlightType.T:                            
//                        //        {
//                        //            GetTaxFuelTotal(cutPrice);
//                        //        }// 结束：A2、A3、儿童往返票或儿童联程票或儿童单程票
//                        //        break;
//                        //}
                        
//                        #region Step2、或 获得儿童单程票的机建费、燃油税、总价

//                        /*
//                        if (_request.FlightType.GetHashCode() != EtermCommand.FlightType.O.GetHashCode())
//                        {
//                            break;
//                        }

//                        // 为下面求机建费、燃油税、总价作准备
//                        if (_response.result.PriceList.Count > 0)
//                        {
//                            price = _response.result.PriceList[0];
//                        }

//                        switch (cutPrice[1].Equals("01"))
//                        {
//                            case false: // 开始：A1、儿童单程票之!cutPrice[1].Equals("01")的情况
//                                {
//                                    try
//                                    {
//#warning will be tested
//                                        getTax = ReTax(cutPrice, "CN/");
//                                        if (!getTax.Contains("TEXEMPT"))
//                                        {
//                                            if (decimal.TryParse(getTax, out valueDecimal))
//                                            {
//                                                price.Tax = valueDecimal;
//                                            }
//                                        }

//                                        getFuel = ReTax(cutPrice, "YQ");
//                                        if (!getFuel.Contains("-") && !getFuel.Contains("TEXEMPT"))
//                                        {
//                                            if (decimal.TryParse(getFuel, out valueDecimal))
//                                            {
//                                                price.Fuel = valueDecimal;
//                                            }
//                                        }
//                                    }
//                                    catch
//                                    {
//                                        GetTaxFuelTotal(cutPrice[13], cutPrice[14], price);
//                                    }
//                                } // 结束：A1、儿童单程票之!cutPrice[1].Equals("01")的情况
//                                break;
//                            case true: // 开始：A1、儿童单程票之cutPrice[1].Equals("01")的情况
//                                {
//                                    //DisPrice = Regex.Match(cutprice[3], @"[0-9]{1,}").Value;

//                                    if (Regex.IsMatch(cutPrice[4], @"[0-9]{1,}\.[0-9]{1,}"))
//                                    {
//                                        price.Tax = Convert.ToDecimal(Regex.Match(cutPrice[4], @"[0-9]{1,}\.[0-9]{1,}").Value);
//                                    }

//                                    if (Regex.IsMatch(cutPrice[5], @"[0-9]{1,}\.[0-9]{1,}"))
//                                    {
//                                        price.Fuel = Convert.ToDecimal(Regex.Match(cutPrice[5], @"[0-9]{1,}\.[0-9]{1,}").Value);
//                                    }
//                                } // 结束：A1、儿童单程票之cutPrice[1].Equals("01")的情况
//                                break;
//                        }

//                        //Tax = Convert.ToString(Airport + Fuel);
//                        price.TotalPrice = price.FacePrice + price.Tax + price.Fuel;
//                        */
                         
//                        #endregion                        
//                    } // A、结束：情况1：儿童票
//                    break;
//                case EtermCommand.PassengerType.Adult: // B、开始：情况2：成人票
//                case EtermCommand.PassengerType.Baby:
//                    {
//                        //#region Step1、获得成人票的票面价（列表）

//                        //// 获得票面价（列表）
//                        //GetFacePriceList(priceResult);

//                        //#endregion

//                        #region Step2、或 获得成人往返票/成人联程票/成人单程票的机建费、燃油税、总价（列表）

//                        switch (_request.FlightType)
//                        {
//                            case EtermCommand.FlightType.F: // 开始：B2、B3、成人往返票或成人联程票或成人单程票
//                            case EtermCommand.FlightType.T: 
//                            case EtermCommand.FlightType.O:
//                                {
//                                    //GetTaxFuelTotal(cutPrice);
//                                    GetTaxFuelTotal(tempPriceResult, firstFlight); 
//                                }// 结束：B2、B3、成人往返票或成人联程票或成人单程票
//                                break;
//                        }

//                        #endregion

//                        #region Step2、或 获得成人单程票的机建费、燃油税、总价

////                        if (_request.FlightType.GetHashCode() != EtermCommand.FlightType.O.GetHashCode())
////                        {
////                            break;
////                        }

////                        GetTaxFuelTotal(tempPriceResult, firstFlight); 

////                        if (_response.result.PriceList.Count > 1)
////                        {
////                            break;
////                        }
                       
////                        // 为下面求机建费、燃油税、总价作准备                        
////                        if (_response.result.PriceList.Count == 1)
////                        {
////                            price = _response.result.PriceList[0];
////                        }                        

////                        // 开始：B1、成人单程票
////                        switch (cutPrice[1].Equals("01"))
////                        {
////                            case false: // 开始：B1、成人单程票之!cutPrice[1].Equals("01")的情况
////                                {
////                                    #region 获得成人单程票的机建费、燃油税、总价

////                                    GetTaxFuelTotal(cutPrice[13], cutPrice[14], price);

////                                    #endregion
////                                } // 结束：B1、成人单程票之!cutPrice[1].Equals("01")的情况
////                                break;
////                            case true: // 开始：B1、成人单程票之cutPrice[1].Equals("01")的情况
////                                {
////                                    #region 针对几家航空公司特定舱位来获得成人单程票的机建费、燃油税、总价

////#warning will be tested
////                                    if ( // 情况1、判断上航（FM）P舱、上航X舱、东航（MU）X舱，区分不同年龄段的价格
////                                        (
////                                            (firstFlight.Cabin.Equals("P") && firstFlight.FlightNo.StartsWith("FM"))
////                                            || (firstFlight.Cabin.Equals("X") && firstFlight.FlightNo.StartsWith("FM"))
////                                            || (firstFlight.Cabin.Equals("X") && firstFlight.FlightNo.StartsWith("MU"))
////                                         )
////                                         && Regex.IsMatch(priceResult, string.Format(@"Y[XP]{1}[0-9M]{2}\s+{0}", priceRegexExpression))
////                                        )
////                                    {
////                                        priceRegexExpression = @"Y[XP]{1}[0-9M]{2}\s+" + priceRegexExpression;
////                                        GetAirportAndFuelPrice(priceResult, priceRegexExpression, 0, price);
////                                    }
////#warning will be tested
////                                    else if (firstFlight.Cabin.Equals("W") // 情况2、判断深航（CZ）W舱
////                                                && firstFlight.FlightNo.StartsWith("CZ")
////                                                && Regex.IsMatch(priceResult, string.Format(@"W[0-9]{0,1}\s+{0}", priceRegexExpression))
////                                            )
////                                    {
////                                        priceRegexExpression = @"W[0-9]{0,1}\s+" + priceRegexExpression;
////                                        GetAirportAndFuelPrice(priceResult, priceRegexExpression, -1, price);
////                                    }
////                                    else // 情况3、如果有第2条价格记录，则第1条价格记录中的机建费和燃油税分别取的是第2条的；否则，分别取的是第1条的
////                                    {
////                                        j = 1;
////                                        if (firstFlight.Cabin.Equals("Y") || firstFlight.Cabin.Equals("C") || firstFlight.Cabin.Equals("F"))
////                                        {
////                                            try
////                                            {
////                                                j = ReNum(cutPrice, "02", true); // 如果有第2条价格记录，则j获得的是第2条记录的开始位置
////                                                if (j == 0)
////                                                {
////                                                    j = 1;
////                                                }
////                                            }
////                                            catch
////                                            {
////                                                j = 1;
////                                            }
////                                        }

////                                        //DisPrice = cutPrice[j + 2].Replace("FARE:CNY", string.Empty);
////                                        GetTaxFuelTotal(cutPrice[j + 3], cutPrice[j + 4], price);
////                                    }

////                                    // Tax = Convert.ToString(Airport + Fuel);
////                                    price.TotalPrice = price.FacePrice + price.Tax + price.Fuel;

////                                    #endregion
////                                } // 结束：B1、成人单程票之cutPrice[1].Equals("01")的情况
////                                break;
////                        } // 结束：B1、成人单程票                        

//                        #endregion
//                    } // B、结束：情况2：成人票
//                    break;
//            }

            //if (_response.result.PriceList.Count == 0)
            //{
            //    _response.result.PriceList.Add(price);
            //}
            _response.state = true;
            return _response;
        }

        #region Helper
      
        protected internal override bool ValidRequest()
        {
            if (_request == null || _request.PassengerType.GetHashCode() == EtermCommand.PassengerType.NotSet.GetHashCode() || _request.FlightList == null || !_request.FlightList.Any())
            {
                _response.error = new Error(EtermCommand.ERROR.EMPTY_PAT_REQUEST_PARAM);
                return false;
            }

            string originalValue = string.Empty;

            string selfDefinedErrorMessage = string.Empty;
            for (int f = 0; f < _request.FlightList.Count; ++f)
            {
                int fNumber = f + 1; // 第几航程
                Flight flight = _request.FlightList[f];

                if (string.IsNullOrWhiteSpace(flight.Cabin) || string.IsNullOrWhiteSpace(flight.FlightNo))
                {
                    selfDefinedErrorMessage += string.Format("{0}没有给第{1}段航程传入必须传的PAT请求参数值，缺少航班号FlightNo或舱位Cabin。", string.IsNullOrWhiteSpace(selfDefinedErrorMessage) ? string.Empty : Environment.NewLine, fNumber);
                }

                if (!string.IsNullOrWhiteSpace(flight.FlightNo))
                {
                    originalValue = flight.FlightNo;
                    flight.FlightNo = Regex.Replace(flight.FlightNo, @"\s", string.Empty).Trim().ToUpper();
                    if (flight.FlightNo.Length < 2)
                    {
                        // 返回false的原因：输入格式应如CZ380                
                        _response.error = new Error(EtermCommand.ERROR.INVALID_FLIGHTNO);
                        return false;

                        selfDefinedErrorMessage += string.Format("{0}PAT请求参数-第{1}段航程的航班号【{2}】-输入格式不正确，正确格式如：CZ380。", string.IsNullOrWhiteSpace(selfDefinedErrorMessage) ? string.Empty : Environment.NewLine, fNumber, originalValue);
                    }
                }

                if (!string.IsNullOrWhiteSpace(flight.Cabin))
                {
                    flight.Cabin = Regex.Replace(flight.Cabin, @"\s", string.Empty).Trim().ToUpper();
                }
            }

            if (!string.IsNullOrWhiteSpace(selfDefinedErrorMessage))
            {
                // 返回false的原因：航班信息请求参数验证没有通过
                _response.error = new Error(EtermCommand.ERROR.SELFDEFINE_ERROR_MESSAGE);
                _response.error.ErrorMessage = selfDefinedErrorMessage;
                return false;
            }

            return true;
        }

        protected internal override bool ValidCmdResult(string cmdResult)
        {
            if (string.IsNullOrWhiteSpace(cmdResult))
            {
                _response.error = new Error(EtermCommand.ERROR.PATCOMMAND_NO_RESULT);
                return false;
            }

            if (!cmdResult.Contains("PAT:A"))
            {
                _response.error = new Error(EtermCommand.ERROR.PATCOMMAND_RESULT_FORMAT_INCORRECT);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取儿童票/成人票的票面价（列表）
        /// </summary>
        /// <param name="priceResult"></param>  
        /// <param name="firstFlight"></param>  
        /// <returns></returns>
        private void GetFacePriceList(string priceResult, Flight firstFlight)
        {
            List<JetermEntity.Price> priceList = _response.result.PriceList;
           
            string regExpression = @"\d+\s+\S+\s+FARE:CNY[0-9]{1,}\.[0-9]{1,}";
            MatchCollection mc = Regex.Matches(priceResult, regExpression);
                     
            if (_request.FlightType == EtermCommand.FlightType.O && !string.IsNullOrWhiteSpace(firstFlight.SubCabin))
            {
                regExpression = @"\d+\s+" + firstFlight.SubCabin + @"(CH\d*|IN\d*)*\s+FARE:CNY[0-9]{1,}\.[0-9]{1,}";
                mc = Regex.Matches(priceResult, regExpression);
                // 考虑到以下几种情况：
                // 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01
                // 折上折：01 YWB/CA1Y143234 FARE:CNY1110.00 TAX:CNY100.00 YQ:TEXEMPTYQ TOTAL:1210.00 SFC:01
                if (mc == null || mc.Count < 1)
                {
                    regExpression = @"FARE:CNY[0-9]{1,}\.[0-9]{1,}";
                    mc = Regex.Matches(priceResult, regExpression);
                }
            }     
            if (mc == null || mc.Count < 1)
            {                
                return;
            }

            Price price = null;
            for (int index = 0; index < mc.Count; index++)
            {
                price = new Price();

                string fareLine = mc[index].Value;

                string facePriceString = string.Empty;
                facePriceString = Regex.Match(fareLine, @"[0-9]{1,}\.[0-9]{1,}").Value.Trim();                
               
                decimal facePrice = 0;
                if (decimal.TryParse(facePriceString, out facePrice))
                {
                    price.FacePrice = facePrice;

                    regExpression = @"\d+\s+(\S+)\s+FARE:CNY[0-9]{1,}\.[0-9]{1,}";
                    Regex reg = new Regex(regExpression);
                    Match match = reg.Match(fareLine);
                    if (match != null && match.Groups != null && match.Groups.Count > 0)
                    {
                        price.Tag = match.Groups[1].Value.Trim();
                    }
                }

                priceList.Add(price);
            }
        }

        /// <summary>
        /// 获得儿童往返票/儿童联程票/儿童单程票/成人往返票/成人联程票/成人单程票的机建费、燃油税和总价（列表）
        /// </summary>
        /// <param name="priceCmdResult"></param>
        /// <param name="firstFlight"></param>
        private void GetTaxFuelTotal(string priceCmdResult, Flight firstFlight)
        {
            List<JetermEntity.Price> priceList = _response.result.PriceList;
            if (priceList == null || priceList.Count < 1)
            {
                return;
            }

            string expression = @"(?<Tax>TAX:\S+)\s*(?<Fuel>YQ:\S+)\s*(?<Total>TOTAL:\S+)";
            MatchCollection mc = Regex.Matches(priceCmdResult, expression);
                
            if (_request.FlightType == EtermCommand.FlightType.O && !string.IsNullOrWhiteSpace(firstFlight.SubCabin))
            {
                expression = @"\d+\s+" + firstFlight.SubCabin + @"(CH\d*|IN\d*)*\s+FARE:\S+\s*(?<Tax>TAX:\S+)\s*(?<Fuel>YQ:\S+)\s*(?<Total>TOTAL:\S+)";
                mc = Regex.Matches(priceCmdResult, expression);
                // 考虑到以下几种情况：
                // 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01
                // 折上折：01 YWB/CA1Y143234 FARE:CNY1110.00 TAX:CNY100.00 YQ:TEXEMPTYQ TOTAL:1210.00 SFC:01
                if (mc == null || mc.Count < 1)
                {
                    expression = @"(?<Tax>TAX:\S+)\s*(?<Fuel>YQ:\S+)\s*(?<Total>TOTAL:\S+)";
                    mc = Regex.Matches(priceCmdResult, expression);
                }
            }
           
            if (mc == null || mc.Count < 1)
            {
                return;
            }

            if (priceList.Count != mc.Count)
            {
                return;
            }

            Match match = null;
            string valueString = string.Empty;
            decimal valueDecimal = 0;
            for (int index = 0; index < mc.Count; index++)
            {
                match = mc[index];
                if (match == null || match.Groups.Count < 1)
                {
                    continue;
                }

                // 获得机建费列表
                string currentTax = mc[index].Groups["Tax"].Value.Trim();               
                if (!currentTax.Contains("TEXEMPT"))
                {
                    valueString = currentTax.Replace("TAX:CNY", string.Empty);
                    if (decimal.TryParse(valueString, out valueDecimal))
                    {
                        priceList[index].Tax = valueDecimal;
                    }
                }

                // 获得燃油税列表
                string currentFuel = mc[index].Groups["Fuel"].Value.Trim();
                if (!currentFuel.Contains("-") && !currentFuel.Contains("TEXEMPT"))
                {
                    valueString = currentFuel.Replace("YQ:CNY", string.Empty);
                    if (decimal.TryParse(valueString, out valueDecimal))
                    {
                        priceList[index].Fuel = valueDecimal;
                    }
                }

                // 获得总价列表
                string currentTotal = mc[index].Groups["Total"].Value.Trim();
                if (!currentTotal.Contains("TEXEMPT"))
                {
                    valueString = currentTotal.Replace("TOTAL:", string.Empty);
                    if (decimal.TryParse(valueString, out valueDecimal))
                    {
                        priceList[index].TotalPrice = valueDecimal;
                    }
                }
            }
        }    

        /// <summary>
        /// 获得ReTax
        /// </summary>
        /// <param name="str"></param>
        /// <param name="seaStr"></param>
        /// <returns></returns>
        private string ReTax(string[] str, string seaStr)
        {
            string s = string.Empty;

            for (int i = 0; i < str.Length; i++)
            {
                string cutstr = str[i];
                if (!cutstr.Contains(seaStr))
                {
                    continue;
                }

                int n = cutstr.IndexOf(seaStr);
                s = cutstr.Substring(0, n).Replace("TCNY", string.Empty).Trim();
                break;
            }

            return s;
        }

        /// <summary>
        /// 获得成人单程票的机建费、燃油税和总价
        /// </summary>
        /// <param name="currentTax"></param>
        /// <param name="currentFuel"></param>
        /// <param name="price"></param>
        private void GetTaxFuelTotal(string currentTax, string currentFuel, Price price)
        {
            string valueString = string.Empty;
            decimal valueDecimal = 0;

            if (!currentTax.Contains("TEXEMPT"))
            {
                valueString = currentTax.Replace("TAX:CNY", string.Empty);
                if (decimal.TryParse(valueString, out valueDecimal))
                {
                    price.Tax = valueDecimal;
                }
            }

            if (!currentFuel.Contains("-") && !currentFuel.Contains("TEXEMPT"))
            {
                valueString = currentFuel.Replace("YQ:CNY", string.Empty);
                if (decimal.TryParse(valueString, out valueDecimal))
                {
                    price.Fuel = valueDecimal;
                }
            }

            //Tax = Convert.ToString(Airport + Fuel);
            price.TotalPrice = price.FacePrice + price.Tax + price.Fuel;
        }

        /// <summary>
        /// 获得成人单程票的机建费和燃油税
        /// </summary>
        /// <param name="priceResult"></param>
        /// <param name="priceRegexExpression"></param>
        /// <param name="index"></param>
        /// <param name="price"></param>
        private static void GetAirportAndFuelPrice(string priceResult, string priceRegexExpression, int index, JetermEntity.Price price)
        {
            MatchCollection mc = Regex.Matches(priceResult, priceRegexExpression);
            if (mc == null || mc.Count < 1)
            {
                return;
            }

            if (index != 0)
            {
                index = mc.Count - 1;
            }

            string valueString = string.Empty;
            decimal valueDecimal = 0;

            //DisPrice = Regex.Match(mc[index].Value, @"FARE:CNY[0-9]{1,}\.[0-9]{1,}").Value.Replace("FARE:CNY", string.Empty);

            if (Regex.IsMatch(mc[index].Value, @"TAX:CNY[0-9]{1,}\.[0-9]{1,}"))
            {
                valueString = Regex.Match(mc[index].Value, @"TAX:CNY[0-9]{1,}\.[0-9]{1,}").Value.Replace("TAX:CNY", string.Empty);
                if (decimal.TryParse(valueString, out valueDecimal))
                {
                    price.Tax = valueDecimal;
                }
            }

            if (Regex.IsMatch(mc[index].Value, @"YQ:CNY[0-9]{1,}\.[0-9]{1,}"))
            {
                valueString = Regex.Match(mc[index].Value, @"YQ:CNY[0-9]{1,}\.[0-9]{1,}").Value.Replace("YQ:CNY", string.Empty);
                if (decimal.TryParse(valueString, out valueDecimal))
                {
                    price.Fuel = valueDecimal;
                }
            }
        }

        /// <summary>
        /// 获得第2条价格记录的开始位置
        /// </summary>
        /// <param name="str"></param>
        /// <param name="seaStr"></param>
        /// <param name="decide">true为等于、false为包含</param>
        /// <returns></returns>
        private static int ReNum(string[] str, string seaStr, bool decide)
        {
            int n = 0;

            for (int i = 0; i < str.Length; i++)
            {
                string cutstr = str[i];

                if (decide)
                {
                    if (cutstr == seaStr)
                    {
                        n = i;
                        break;
                    }
                    continue;
                }

                if (cutstr.Contains(seaStr))
                {
                    n = i;
                    break;
                }
                continue;
            }

            return n;
        }

        #endregion
    }
}
