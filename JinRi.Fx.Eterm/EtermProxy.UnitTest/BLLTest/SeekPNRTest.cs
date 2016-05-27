using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JetermEntity;
using EtermProxy.Utility;
using System.Text.RegularExpressions;
using JetermEntity.Response;

namespace EtermProxy.UnitTest
{
    [TestClass]
    public class SeekPNRTest
    {
        #region Enum Value

        /*        
             /// <summary>
        /// 证件类型
        /// </summary>
        public enum IDtype
        {
            /// <summary>
            /// 没有设置
            /// </summary>
            NotSet = -1,
            /// <summary>
            /// 身份证
            /// </summary>
            IDcard,
            /// <summary>
            /// 其他
            /// </summary>
            Other
        }

        /// <summary>
        /// 乘客类型
        /// </summary>
        public enum PassengerType
        {
            /// <summary>
            /// 没有设置
            /// </summary>
            NotSet = -1,
            /// <summary>
            /// 成人
            /// </summary>
            Adult = 0,
            /// <summary>
            /// 儿童
            /// </summary>
            Children = 1,
            /// <summary>
            /// 婴儿
            /// </summary>
            Baby = 2
        }


public enum FlightType
        {
            O, //  单程
            F, // 往返
            T // 联程
            
        }

public enum ToggleAnswer
        {
            NotSet,
            Yes,
            No
        }
             */

        #endregion

        [TestMethod]
        public void ParseRT()
        {
            string rt = @"1.卢海新 PXHEKT/HU                                                             
 2.  HU7873 U   WE27APR  XIYSZX HK1   1610 1905      E T2T3                     
 3.021-60727777                                                                 
 4.TL/0940/20APR/HKK532                                                         
 5.SSR FOID HU HK1 NI450924198503024718/P1                                      
 6.SSR ADTK CA BY HKK20APR16/1140 OR CXL HU7873 U27APR                          
 7.OSI HU CTCT18923832158                                                       
 8.OSI HU CTCT02160727777                                                       
 9.HKK532[price]>PAT:A                                                                          
01 U FARE:CNY740.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:790.00                     
SFC:01   SFN:01[eTerm:o78783b1]";
            var rtResult = new JetermEntity.Parser.SeekPNR().ParseCmdResult(rt);
        }

        [TestMethod]
        public void Test_BusinessDispose()
        {
            #region Enum Value

            /*        
             /// <summary>
        /// 证件类型
        /// </summary>
        public enum IDtype
        {
            /// <summary>
            /// 没有设置
            /// </summary>
            NotSet = -1,
            /// <summary>
            /// 身份证
            /// </summary>
            IDcard,
            /// <summary>
            /// 其他
            /// </summary>
            Other
        }

        /// <summary>
        /// 乘客类型
        /// </summary>
        public enum PassengerType
        {
            /// <summary>
            /// 没有设置
            /// </summary>
            NotSet = -1,
            /// <summary>
            /// 成人
            /// </summary>
            Adult = 0,
            /// <summary>
            /// 儿童
            /// </summary>
            Children = 1,
            /// <summary>
            /// 婴儿
            /// </summary>
            Baby = 2
        }


public enum FlightType
        {
            O, //  单程
            F, // 往返
            T // 联程
            
        }

public enum ToggleAnswer
        {
            NotSet,
            Yes,
            No
        }
             */

            #endregion

            JetermEntity.Request.SeekPNR request = new JetermEntity.Request.SeekPNR();

            #region 设置请求参数

            // 例1_2：乘客类型为成人（只有1人），执行rt后，没有显示【ELECTRONIC TICKET PNR】，没有分页
            //request.Pnr = "KWSWH9";
            //request.PassengerType = EtermCommand.PassengerType.Adult;
            //request.GetPrice = true;

            // （未测）例2：乘客类型为成人（只有1人），执行rt后，有显示【ELECTRONIC TICKET PNR】，有分页
            //request.Pnr = "KW9CL2";
            //request.PassengerType = EtermCommand.PassengerType.Adult;
            //request.GetPrice = true;

            // 例3：乘客类型为成人（多人），执行rt后，没有显示【ELECTRONIC TICKET PNR】，有分页
            //request.Pnr = "KN0SNL";
            //request.PassengerType = EtermCommand.PassengerType.Adult;
            //request.GetPrice = true;

            // （未测）例4：乘客类型为成人（多人），执行rt后，有显示【ELECTRONIC TICKET PNR】，有分页
            //request.Pnr = "JPEB2H";
            //request.PassengerType = EtermCommand.PassengerType.Adult;
            //request.GetPrice = true;

            // 例5：乘客类型为儿童（只有1人），执行rt后，没有显示【ELECTRONIC TICKET PNR】，没有分页
            //request.Pnr = "HPDL7J";
            //request.PassengerType = EtermCommand.PassengerType.Children;
            //request.GetPrice = true;

            // 例6：乘客类型为儿童（只有1人），执行rt后，有显示【ELECTRONIC TICKET PNR】，有分页（表中有数据，但我没找到能运行出结果的测试案例）
            //request.Pnr = "HSNH3Q";
            //request.PassengerType = EtermCommand.PassengerType.Children;
            //request.GetPrice = true;

            // 例8、乘客类型为儿童（多人），执行rt后，有显示【ELECTRONIC TICKET PNR】，有分页（还没在数据库表中开始找测试案例）

            // 例10_1：乘客类型为婴儿（多人），执行rt后，有显示【ELECTRONIC TICKET PNR】，且没有婴儿出生日期信息，有分页（在数据库表[JinRiLogger].[dbo].[PnrInfoLog]有，但我没在Eterm Server中运行过）
            //request.Pnr = "JVWNH9";
            //request.PassengerType = EtermCommand.PassengerType.Baby;
            //request.GetPrice = true;

            // 例10_2：乘客类型为婴儿（多人），执行rt后，有显示【ELECTRONIC TICKET PNR】，且包含有婴儿出生日期信息，有分页（在数据库表[JinRiLogger].[dbo].[PnrInfoLog]有，但我没在Eterm Server中运行过）
            //request.Pnr = "HSZ42G";
            //request.PassengerType = EtermCommand.PassengerType.Baby;
            //request.GetPrice = true;

            //request.Pnr = "JNN6CN";
            //request.Pnr = "HE19HL";
            // JX0K91
            request.Pnr = "JX0K91";
            request.PassengerType = EtermCommand.PassengerType.Adult;
            request.GetPrice = true;

            request.Pnr = "KPHXV6";
            request.PassengerType = EtermCommand.PassengerType.Adult;
            request.GetPrice = true;

            #endregion

            EtermProxy.BLL.SeekPNR logic = new EtermProxy.BLL.SeekPNR(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.SeekPNR> result = logic.BusinessDispose(request);

            if (result == null || result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            if (!result.state)
            {
                if (result.error != null)
                {
                    // 获得的结果，如：  
                    /*
                     // 例5：
                     此记录第1段航班信息的编号位置被“NO”掉了，请检查您的PNR记录编号
                     // 例10_1：
                     成人编码不含婴儿航段，请做入后再导入，或选择预订创单！
                     */
                    Console.WriteLine(string.Format("返回失败，失败原因：{0}", result.error.ErrorMessage));
                    //Console.ReadLine();
                    return;
                }

                Console.WriteLine("返回失败");
                //Console.ReadLine();
                return;
            }

            // 获得的结果，如：               
            /*
            // 例1_2：
            {"PassengerList":[{"name":"沈燕彬","idtype":0,"cardno":"NI513322197103192519","PassType":0,"Ename":"","BirthDayString":"","TicketNo":""}],"PNR":"KWSWH9","FlightList":[{"FlightNo":"CZ3461","Cabin":"Y","SCity":"CSX","ECity":"CTU","SDate":"28DEC1425","EDate":"28DEC1635"}],"ShareFlight":1,"FlightType":0,"RMKOfficeNoList":["CSX141"],"BigPNR":"NBV0F1","Mobile":"13548746642","PriceList":[{"FacePrice":910.00,"Tax":50.00,"Fuel":60.00,"TotalPrice":1020.00}]}
            // 例3：
            {"PassengerList":[{"name":"王亚敏","idtype":0,"cardno":"NI20150115010019112398","PassType":0,"Ename":"","BirthDayString":"","TicketNo":""},{"name":"杨璐","idtype":0,"cardno":"NI20150115010019129398","PassType":0,"Ename":"","BirthDayString":"","TicketNo":""},{"name":"资星","idtype":0,"cardno":"NI20150115010019129328","PassType":0,"Ename":"","BirthDayString":"","TicketNo":""}],"PNR":"KN0SNL","FlightList":[{"FlightNo":"CZ6120","Cabin":"Y","SCity":"CTU","ECity":"PEK","SDate":"19FEB0855","EDate":"19FEB1135"}],"ShareFlight":1,"FlightType":0,"RMKOfficeNoList":["SHA333","SHA123"],"BigPNR":"MVBPYW","Mobile":"13764246613","PriceList":[{"FacePrice":1580.00,"Tax":50.00,"Fuel":30.00,"TotalPrice":1660.00}]}
            // 例6：
            {"PassengerList":[{"name":"张越CHD","idtype":0,"cardno":"NI150102200804245116","PassType":1,"Ename":"ZHANGYUE","BirthDayString":"","TicketNo":"9992124675454"}],"PNR":"HSNH3Q","FlightList":[{"FlightNo":"CA1345","Cabin":"Y","SCity":"PEK","ECity":"SYX","SDate":"03FEB1450","EDate":"03FEB1900"}],"ShareFlight":1,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","PriceList":[{"FacePrice":0.0,"Tax":0.0,"Fuel":60.00,"TotalPrice":60.00}]}
            // 例10_2：                
           {"PassengerList":[{"name":"王耀荣","idtype":0,"cardno":"NI130921198911264063","PassType":0,"Ename":"","BirthDayString":"","TicketNo":"99923476297319992347629732"},{"name":"谢雪晴","idtype":0,"cardno":"NI13092119870104402X","PassType":0,"Ename":"","BirthDayString":"","TicketNo":""},{"name":"谢依然","idtype":-1,"cardno":"","PassType":2,"Ename":"XIE/YIRAN ","BirthDayString":"12OCT13","TicketNo":""}],"PNR":"HSZ42G","FlightList":[{"FlightNo":"CA4174","Cabin":"G","SCity":"PEK","ECity":"KMG","SDate":"24OCT2120","EDate":"25OCT0035"}],"ShareFlight":1,"FlightType":0,"RMKOfficeNoList":["BJS407","SIA302"],"BigPNR":"MX5428","Mobile":"13833736118","PriceList":[{"FacePrice":180.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":180.00}]}
             */
            Console.WriteLine("返回结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
            //Console.ReadLine();
        }

        [TestMethod]
        public void Test_EtermProxy_1000()
        {
            //string strPost = "{\"ClassName\" : \"SeekPNR\", \"Config\" : \"o72fd431\",  \"OfficeNo\" : \"SHA243\" }";
            string strPost = "{\"ClassName\" : \"SeekPNR\", \"Config\" : \"O77124B1\",  \"OfficeNo\" : \"SHA243\" }";
            //string ss = "{\"FlightList\":[{\"FlightNo\":\"MU5137\",\"Cabin\":\"H\",\"SCity\":\"SHA\",\"ECity\":\"PEK\",\"DepDate\":\"\\/Date(1430064000000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"MU5156\",\"Cabin\":\"B\",\"SCity\":\"PEK\",\"ECity\":\"SHA\",\"DepDate\":\"\\/Date(1430323200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"干园\",\"idtype\":0,\"cardno\":\"650121199412242866\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"},{\"name\":\"张杰\",\"idtype\":0,\"cardno\":\"140525198401186312\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"13472634765\",\"RMKOfficeNoList\":[],\"RMKRemark\":null,\"Pnr\":null}";
            //string ss = "{\"Pnr\":\"HX88FQ\",\"PassengerType\":0,\"GetPrice\":true}";
            string ss = "{\"Pnr\":\"KTN35E\",\"PassengerType\":0,\"GetPrice\":true}";

            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：{"state":true,"error":null,"config":"o72fd431","OfficeNo":"SHA243","result":{"PassengerList":[{"name":"张龙","idtype":0,"cardno":"NI610103197010032517","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HX88FQ","FlightList":[{"FlightNo":"CZ6178","Airline":"","Cabin":"Y","SCity":"CGQ","ECity":"CSX","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1435124400000+0800)\/","ArrDate":"\/Date(1435141800000+0800)\/"},{"FlightNo":"CZ3937","Airline":"","Cabin":"M","SCity":"CSX","ECity":"CGQ","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1435210800000+0800)\/","ArrDate":"\/Date(1435228800000+0800)\/"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["CGQ203"],"BigPNR":"PV17TB","Mobile":"18101810679","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":3380.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":3480.00},{"FacePrice":3380.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":3480.00}],"ResultBag":" 1.张龙 HX88FQ                                                                   \r 2.  CZ6178 Y   WE24JUN  CGQCSX HK1   1340 1830          E                     \r 3.  CZ3937 M   TH25JUN  CSXCGQ HK1   1340 1840          E                     \r 4.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     \r     ABCDEFG                                                                   \r 5.TL/1518/07MAY/SHA888                                                        \r 6.SSR FOID CZ HK1 NI610103197010032517/P1                                     \r 7.SSR ADTK 1E BY SHA17MAY15/1419 OR CXL CZ BOOKING                            \r 8.OSI CZ CTCT18101810679                                                      \r 9.RMK TJ AUTH CGQ203                                                          \r10.RMK CA/PV17TB                                                               \r11.SHA243                                                                      \r\r\n\r\n>PAT:A                                                                         \r01 Y+M FARE:CNY3380.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:3480.00               \r?SFC:01   ?SFN:01/01   ?SFN:01/02                                              \r02 Y+M FARE:CNY3380.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:3480.00               \r?SFC:02   ?SFN:02/01   ?SFN:02/02                                              \r"},"reqtime":"\/Date(1430979973493+0800)\/","SaveTime":1800}
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, ss);
            Console.WriteLine("返回结果：" + sret);
        }

        [TestMethod]
        public void Test_EtermProxy_1001()
        {
            string strPost = "{\"ClassName\" : \"SeekPNR\", \"Config\" : \"o72fd3c1\",  \"OfficeNo\" : \"SHA243\" }";
            string ss = "{\"Pnr\":\"HWYDV3\",\"PassengerType\":0,\"GetPrice\":true}";

            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：
            // {"state":false,"error":{"ErrorCode":68,"ErrorMessage":"此记录编号未添加证件号码，加入后请重新导入PNR记录编号","CmdResultBag":" 1.朱柯红 HWYDV3                                                                  \r 2.  CZ3188 U   WE14OCT15PEKCGD『HX1』  1750 2030          E T2--                \r 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     \r     ABCDEFG                                                                   \r 4.T                                                                           \r 5.SSR OTHS 1E CHG FLT                                                         \r 6.SSR ADTK 1E BY SHA12OCT15/0036 OR CXL CZ BOOKING                            \r 7.OSI CZ CTCT13705842196                                                      \r 8.RMK TJ TSN201                                                               \r 9.RMK TJ AUTH TSN201                                                          \r10.RMK AUTOMATIC FARE QUOTE                                                    \r11.FN/A/FCNY830.00/SCNY830.00/C0.00/XCNY50.00/TCNY50.00CN/TEXEMPTYQ/ACNY880.00 +\r\r\n12.TN/784-9737553358/P1                                                        -\r13.FP/CASH,CNY                                                                 \r14.SHA243                                                                      \r\r\n\r\n>PAT:A                                                                         \r01 V FARE:CNY560.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:610.00                    \r?SFC:01   ?SFN:01                                                              \r02 YV FARE:CNY560.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:610.00                   \r?SFC:02   ?SFN:02                                                              \r","InnerDetailedErrorMessage":null},"config":"o72fd3c1","OfficeNo":"SHA243","result":{"PassengerList":[{"name":"朱柯红","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"0001-01-01T00:00:00","ChildBirthday":"0001-01-01T00:00:00","TicketNo":""}],"PNR":"HWYDV3","FlightList":[{"FlightNo":"","Airline":"","Cabin":"","SubCabin":"","SCity":null,"ECity":null,"DepTerminal":null,"ArrTerminal":null,"DepDate":"0001-01-01T00:00:00","ArrDate":"0001-01-01T00:00:00","PNRState":"『HX1』"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["TSN201"],"BigPNR":"","Mobile":"13705842196","PhoneNo":null,"OfficeNo":"SHA243","AdultPnr":null,"PriceList":[],"ResultBag":" 1.朱柯红 HWYDV3                                                                  \r 2.  CZ3188 U   WE14OCT15PEKCGD『HX1』  1750 2030          E T2--                \r 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     \r     ABCDEFG                                                                   \r 4.T                                                                           \r 5.SSR OTHS 1E CHG FLT                                                         \r 6.SSR ADTK 1E BY SHA12OCT15/0036 OR CXL CZ BOOKING                            \r 7.OSI CZ CTCT13705842196                                                      \r 8.RMK TJ TSN201                                                               \r 9.RMK TJ AUTH TSN201                                                          \r10.RMK AUTOMATIC FARE QUOTE                                                    \r11.FN/A/FCNY830.00/SCNY830.00/C0.00/XCNY50.00/TCNY50.00CN/TEXEMPTYQ/ACNY880.00 +\r\r\n12.TN/784-9737553358/P1                                                        -\r13.FP/CASH,CNY                                                                 \r14.SHA243                                                                      \r\r\n\r\n>PAT:A                                                                         \r01 V FARE:CNY560.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:610.00                    \r?SFC:01   ?SFN:01                                                              \r02 YV FARE:CNY560.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:610.00                   \r?SFC:02   ?SFN:02                                                              \r"},"reqtime":"2015-10-16T12:10:46.7605239+08:00","SaveTime":1800,"ServerUrl":null}
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, ss);
            Console.WriteLine("返回结果：" + sret);
        }

        //===============added by Li Yang, April 13th, 2015================================
        [TestMethod]
        public void Test()
        {
            // 成人往返票测试
            // 返回结果：
            // {"PassengerList":[{"name":"雷雷","idtype":0,"cardno":"NI370402198411026921","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"李银生","idtype":0,"cardno":"NI410105195301033831","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"屠光君","idtype":0,"cardno":"NI330227197712286828","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"JN5Y9G","FlightList":[{"FlightNo":"MU2504","Airline":"","Cabin":"L","SCity":"PVG","ECity":"WUH","DepTerminal":"T1","ArrTerminal":"T2","DepDate":"\/Date(1430090100000+0800)\/","ArrDate":"\/Date(1430097300000+0800)\/","PNRState":"HK3"},{"FlightNo":"MU517","Airline":"","Cabin":"R","SCity":"WUH","ECity":"PVG","DepTerminal":"T2","ArrTerminal":"T1","DepDate":"\/Date(1430264700000+0800)\/","ArrDate":"\/Date(1430270100000+0800)\/","PNRState":"HK3"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["SHA255","SZX585"],"BigPNR":"MWCFGZ","Mobile":"15692182199","OfficeNo":"TPE567","AdultPnr":null,"PriceList":[{"FacePrice":1500.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":1600.00},{"FacePrice":1670.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":1770.00}],"ResultBag":"RT JN5Y9G\r\n1.雷雷 2.李银生 3.屠光君 JN5Y9G                                                       \r\n 4.  MU2504 L   MO27APR  PVGWUH HK3   0715 0915          E T1T2                \r\n 5.  MU517  R   WE29APR  WUHPVG HK3   0745 0915          E T2T1                \r\n 6.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG      \r\n 7.TL/1527/15APR/SHA888                                                        \r\n 8.SSR FOID MU HK1 NI330227197712286828/P3                                     \r\n 9.SSR FOID MU HK1 NI410105195301033831/P2                                     \r\n10.SSR FOID MU HK1 NI370402198411026921/P1                                     \r\n11.SSR CKIN MU                                                                 \r\n12.SSR FQTV MU HK1 PVGWUH 2504 L27APR MU660283483239/P3                        \r\n13.SSR FQTV MU HK1 WUHPVG 517 R29APR MU660283483239/P3                         \r\n14.SSR ADTK 1E BY TPE17APR15/1427 OR CXL MU2504 L27APR                         +\r\n\r\n15.OSI MU CTCT15692182199                                                      -\r\n16.RMK TJ AUTH SHA255                                                          \r\n17.RMK TJ AUTH SZX585                                                          \r\n18.RMK CA/MWCFGZ                                                               \r\n19.TPE567                                                                      \r\n\r\nPAT:A\r\n>PAT:A*CH                                                                         \r\n01 RT/L+RT/R FARE:CNY1500.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:1600.00         \r\n?SFC:01   ?SFN:01/01   ?SFN:01/02                                              \r\n02 L+R FARE:CNY1670.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:1770.00               \r\n?SFC:02   ?SFN:02/01   ?SFN:02/02"}
            string cmdResult1 =
@"RT JN5Y9G
1.雷雷 2.李银生 3.屠光君 JN5Y9G                                                       
 4.  MU2504 L   MO27APR  PVGWUH HK3   0715 0915          E T1T2                
 5.  MU517  R   WE29APR  WUHPVG HK3   0745 0915          E T2T1                
 6.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG      
 7.TL/1527/15APR/SHA888                                                        
 8.SSR FOID MU HK1 NI330227197712286828/P3                                     
 9.SSR FOID MU HK1 NI410105195301033831/P2                                     
10.SSR FOID MU HK1 NI370402198411026921/P1                                     
11.SSR CKIN MU                                                                 
12.SSR FQTV MU HK1 PVGWUH 2504 L27APR MU660283483239/P3                        
13.SSR FQTV MU HK1 WUHPVG 517 R29APR MU660283483239/P3                         
14.SSR ADTK 1E BY TPE17APR15/1427 OR CXL MU2504 L27APR                         +

15.OSI MU CTCT15692182199                                                      -
16.RMK TJ AUTH SHA255                                                          
17.RMK TJ AUTH SZX585                                                          
18.RMK CA/MWCFGZ                                                               
19.TPE567                                                                      

PAT:A
>PAT:A*CH                                                                         
01 RT/L+RT/R FARE:CNY1500.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:1600.00         
?SFC:01   ?SFN:01/01   ?SFN:01/02                                              
02 L+R FARE:CNY1670.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:1770.00               
?SFC:02   ?SFN:02/01   ?SFN:02/02";

            // 成人单程票测试
            // 返回结果：
            // {"PassengerList":[{"name":"李娜","idtype":0,"cardno":"NI32091114","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"万旋","idtype":0,"cardno":"NI22222222","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"JWQHCG","FlightList":[{"FlightNo":"CZ3194","Airline":"","Cabin":"F","SCity":"PEK","ECity":"SZX","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1435623000000+0800)\/","ArrDate":"\/Date(1435634400000+0800)\/","PNRState":"HK2"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["AAA123","BBB123"],"BigPNR":"MY37KJ","Mobile":"13764246613","OfficeNo":"TPE672","AdultPnr":null,"PriceList":[{"FacePrice":5250.00,"Tax":50.00,"Fuel":120.00,"TotalPrice":5420.00}],"ResultBag":" 1.李娜 2.万旋 JWQHCG     3.  CZ3194 F   MO30JUN  PEKSZX HK2   0810 1120          E T2T3       -CA-MY37KJ   4.TPE/T TPE/T0265159958/JU HSIN TRAVEL SERVICE CO LTD/HSIAO CHING MING       ABCDEFG  5.TL/1857/10JUN/SHA888   6.SSR FOID CZ HK1 NI32091114/P1  7.SSR FOID CZ HK1 NI22222222/P2  8.OSI CZ CTCT13764246613     9.RMK TJ AUTH AAA123    10.RMK TJ AUTH BBB123    11.RMK CA/MY37KJ                                                               +  \u001e12.TPE672                                                                      -  \u001e[price]>PAT:A   01 F FARE:CNY5250.00 TAX:CNY50.00 YQ:CNY120.00  TOTAL:5420.00    \u001eSFC:01  \u001e[eTerm:o765a2b1]"}
            string cmdResult3 =
            //string cmdResult =
@" 1.李娜 2.万旋 JWQHCG     3.  CZ3194 F   MO30JUN  PEKSZX HK2   0810 1120          E T2T3       -CA-MY37KJ   4.TPE/T TPE/T0265159958/JU HSIN TRAVEL SERVICE CO LTD/HSIAO CHING MING       ABCDEFG  5.TL/1857/10JUN/SHA888   6.SSR FOID CZ HK1 NI32091114/P1  7.SSR FOID CZ HK1 NI22222222/P2  8.OSI CZ CTCT13764246613     9.RMK TJ AUTH AAA123    10.RMK TJ AUTH BBB123    11.RMK CA/MY37KJ                                                               +  12.TPE672                                                                      -  [price]>PAT:A   01 F FARE:CNY5250.00 TAX:CNY50.00 YQ:CNY120.00  TOTAL:5420.00    SFC:01  [eTerm:o765a2b1]";

            // 儿童单程票测试
            // 返回结果：{"PassengerList":[{"name":"李哲慧CHD","idtype":0,"cardno":"NI20080808","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1218124800000+0800)\/","TicketNo":""}],"PNR":"HRTE37","FlightList":[{"FlightNo":"HO1279","Airline":"","Cabin":"Y","SCity":"BFJ","ECity":"KMG","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1440489900000+0800)\/","ArrDate":"\/Date(1440492900000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"NKL9Z2","Mobile":"18801932793","OfficeNo":"TPE672","AdultPnr":"JVF15R","PriceList":[{"FacePrice":350.00,"Tax":0.0,"Fuel":30.00,"TotalPrice":380.00}],"ResultBag":"\u0010RTHRTE37                                                                          1.李哲慧CHD HRTE37                                                                2.  HO1279 Y   MO25AUG  BFJKMG HK1   1605 1655          E                         3.TPE/T TPE/T0265159958/JU HSIN TRAVEL SERVICE CO LTD/HSIAO CHING MING                ABCDEFG                                                                       4.TL/1734/26JUN/BJS579                                                            5.SSR FOID HO HK1 NI20080808/P1                                                   6.SSR OTHS HO ADULT PNR IS JVF15R                                                 7.SSR ADTK 1E BY TPE28JUN14/1634 OR CXL HO1279 Y25AUG                             8.SSR CHLD HO HK1 08AUG08/P1                                                      9.OSI HO CTCT18801932793                                                         10.RMK CA/NKL9Z2                                                                  11.TPE672                                                                         \u0010                                                                                                                                                                                                                                                     \u0010PAT:A*CH                                                                         >PAT:A*CH                                                                         01 YCH FARE:CNY350.00 TAX:TEXEMPTCN YQ:CNY30.00  TOTAL:380.00                     \u0010SFC:01"}
            string cmdResult4 =
            //string cmdResult =
@"RTHRTE37                                                                          1.李哲慧CHD HRTE37                                                                2.  HO1279 Y   MO25AUG  BFJKMG HK1   1605 1655          E                         3.TPE/T TPE/T0265159958/JU HSIN TRAVEL SERVICE CO LTD/HSIAO CHING MING                ABCDEFG                                                                       4.TL/1734/26JUN/BJS579                                                            5.SSR FOID HO HK1 NI20080808/P1                                                   6.SSR OTHS HO ADULT PNR IS JVF15R                                                 7.SSR ADTK 1E BY TPE28JUN14/1634 OR CXL HO1279 Y25AUG                             8.SSR CHLD HO HK1 08AUG08/P1                                                      9.OSI HO CTCT18801932793                                                         10.RMK CA/NKL9Z2                                                                  11.TPE672                                                                                                                                                                                                                                                                                                                              PAT:A*CH                                                                         >PAT:A*CH                                                                         01 YCH FARE:CNY350.00 TAX:TEXEMPTCN YQ:CNY30.00  TOTAL:380.00                     SFC:01";

            // 婴儿往返票测试
            // 返回结果：
            // {"PassengerList":[{"name":"万春华","idtype":0,"cardno":"NI360122197805084211","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"熊焕兵","idtype":0,"cardno":"NI360122197803114237","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"熊魁","idtype":0,"cardno":"NI360122197603026312","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"熊贤财","idtype":0,"cardno":"NI360122197512034210","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"张丽","idtype":0,"cardno":"NI522501197504021622","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"ZI/XIN","idtype":-1,"cardno":"","PassType":2,"Ename":"ZI/XIN ","BabyBirthday":"\/Date(1398528000000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HG7BWL","FlightList":[{"FlightNo":"CZ3691","Airline":"","Cabin":"M","SCity":"KWE","ECity":"CAN","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1436157600000+0800)\/","ArrDate":"\/Date(1436163300000+0800)\/","PNRState":"RR5"},{"FlightNo":"CZ3692","Airline":"","Cabin":"M","SCity":"CAN","ECity":"KWE","DepTerminal":"","ArrTerminal":"T2","DepDate":"\/Date(1436512500000+0800)\/","ArrDate":"\/Date(1436518200000+0800)\/","PNRState":"RR5"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["CAN445","SHA255"],"BigPNR":"NYD502","Mobile":"18913987688","OfficeNo":"TPE672","AdultPnr":null,"PriceList":[{"FacePrice":200.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":200.00}],"ResultBag":"\u0010RTHG7BWL                                                                           **ELECTRONIC TICKET PNR**                                                        1.万春华 2.熊焕兵 3.熊魁 4.熊贤财 5.张丽 HG7BWL                                   6.  CZ3691 M   SU06JUL  KWECAN RR5   1240 1415          E T2--                    7.  CZ3692 M   TH10JUL  CANKWE RR5   1515 1650          E --T2                    8.TPE/T TPE/T0265159958/JU HSIN TRAVEL SERVICE CO LTD/HSIAO CHING MING                ABCDEFG                                                                       9.T/0000000000000                                                                10.SSR FOID CZ HK1 NI360122197805084211/P1                                        11.SSR FOID CZ HK1 NI360122197803114237/P2                                        12.SSR FOID CZ HK1 NI360122197603026312/P3                                        13.SSR FOID CZ HK1 NI522501197504021622/P5                                        14.SSR FOID CZ HK1 NI360122197512034210/P4                                        \u0010                                                                                                                                                                                                                                                     \u0010PN                                                                               15.SSR ADTK 1E BY TPE12JUN14/1317 OR CXL CZ BOOKING                            -  16.SSR TKNE CZ HK1 KWECAN 3691 M06JUL 7842133168147/1/P1                          17.SSR TKNE CZ HK1 KWECAN 3691 M06JUL 7842133168148/1/P2                          18.SSR TKNE CZ HK1 KWECAN 3691 M06JUL 7842133168149/1/P3                          19.SSR TKNE CZ HK1 KWECAN 3691 M06JUL 7842133168150/1/P4                          20.SSR TKNE CZ HK1 KWECAN 3691 M06JUL 7842133168151/1/P5                          21.SSR TKNE CZ HK1 CANKWE 3692 M10JUL 7842133168147/2/P1                          22.SSR TKNE CZ HK1 CANKWE 3692 M10JUL 7842133168148/2/P2                          23.SSR TKNE CZ HK1 CANKWE 3692 M10JUL 7842133168149/2/P3                          24.SSR TKNE CZ HK1 CANKWE 3692 M10JUL 7842133168150/2/P4                          25.SSR TKNE CZ HK1 CANKWE 3692 M10JUL 7842133168151/2/P5                          26.SSR INFT CZ  KK1 KWECAN 3691 M06JUL ZI/XIN 27APR14/P1                          \u0010                                                                                                                                                                                                                                                     \u0010PN                                                                               27.SSR INFT CZ  KK1 CANKWE 3692 M10JUL ZI/XIN 27APR14/P1                       -  28.OSI CZ CTCT18913987688                                                         29.OSI YY 1INF ZI/XININF/P1                                                       30.RMK TJ AUTH CAN445                                                             31.RMK CA/NYD502                                                                  32.RMK TJ AUTH SHA255                                                             33.XN/IN/ZI/XININF(APR14)/P1                                                      34.TPE672                                                                         \u0010                                                                                                                                                                                                                                                     \u0010PAT:A*IN                                                                         >PAT:A*IN                                                                         01 YIN YIN FARE:CNY200.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:200.00                \u0010SFC:01                                                                           \u0010                              ,"}
            string cmdResult5 =
            //string cmdResult =
@"RTHG7BWL                                                                           **ELECTRONIC TICKET PNR**                                                        1.万春华 2.熊焕兵 3.熊魁 4.熊贤财 5.张丽 HG7BWL                                   6.  CZ3691 M   SU06JUL  KWECAN RR5   1240 1415          E T2--                    7.  CZ3692 M   TH10JUL  CANKWE RR5   1515 1650          E --T2                    8.TPE/T TPE/T0265159958/JU HSIN TRAVEL SERVICE CO LTD/HSIAO CHING MING                ABCDEFG                                                                       9.T/0000000000000                                                                10.SSR FOID CZ HK1 NI360122197805084211/P1                                        11.SSR FOID CZ HK1 NI360122197803114237/P2                                        12.SSR FOID CZ HK1 NI360122197603026312/P3                                        13.SSR FOID CZ HK1 NI522501197504021622/P5                                        14.SSR FOID CZ HK1 NI360122197512034210/P4                                                                                                                                                                                                                                                                                             PN                                                                               15.SSR ADTK 1E BY TPE12JUN14/1317 OR CXL CZ BOOKING                            -  16.SSR TKNE CZ HK1 KWECAN 3691 M06JUL 7842133168147/1/P1                          17.SSR TKNE CZ HK1 KWECAN 3691 M06JUL 7842133168148/1/P2                          18.SSR TKNE CZ HK1 KWECAN 3691 M06JUL 7842133168149/1/P3                          19.SSR TKNE CZ HK1 KWECAN 3691 M06JUL 7842133168150/1/P4                          20.SSR TKNE CZ HK1 KWECAN 3691 M06JUL 7842133168151/1/P5                          21.SSR TKNE CZ HK1 CANKWE 3692 M10JUL 7842133168147/2/P1                          22.SSR TKNE CZ HK1 CANKWE 3692 M10JUL 7842133168148/2/P2                          23.SSR TKNE CZ HK1 CANKWE 3692 M10JUL 7842133168149/2/P3                          24.SSR TKNE CZ HK1 CANKWE 3692 M10JUL 7842133168150/2/P4                          25.SSR TKNE CZ HK1 CANKWE 3692 M10JUL 7842133168151/2/P5                          26.SSR INFT CZ  KK1 KWECAN 3691 M06JUL ZI/XIN 27APR14/P1                                                                                                                                                                                                                                                                               PN                                                                               27.SSR INFT CZ  KK1 CANKWE 3692 M10JUL ZI/XIN 27APR14/P1                       -  28.OSI CZ CTCT18913987688                                                         29.OSI YY 1INF ZI/XININF/P1                                                       30.RMK TJ AUTH CAN445                                                             31.RMK CA/NYD502                                                                  32.RMK TJ AUTH SHA255                                                             33.XN/IN/ZI/XININF(APR14)/P1                                                      34.TPE672                                                                                                                                                                                                                                                                                                                              PAT:A*IN                                                                         >PAT:A*IN                                                                         01 YIN YIN FARE:CNY200.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:200.00                SFC:01                                                                                                         ,";

            string cmdResult6 =
@"
   *THIS PNR WAS ENTIRELY CANCELLED*                                           
005     HDQCA 9983 1155 15MAY /RLC4                                            
     X1.林忠秀(001) JD438Q                                                        
001 X2.  CA1603 Y   WE20MAY  PEKHRB XX1   0655 0900          E T3--            
       NN(001)  DK(001)  HK(001)  XX(004)                                      
001 X3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG  
        ABCDEFG                                                                
001 X4.TL/2054/15MAY/SHA888                                                    
001 X5.SSR FOID CA XX1 NI134541                                                
       HK(001)   XX(004)   XX(004)                                             
003 X6.SSR ADTK 1E BY SHA15MAY15/2154 OR CXL CA ALL SEGS                       
001 X7.OSI CA CTCT13641601096                                                  +

PNR CANCELLED  
";

            string cmdResult7 =
            //string cmdResult =
@"
 1.武玉泉 MGJQN3  2.  CA1567 T   WE25JUN  PEKWNZ NO1   0740 1015      E T3--   3.T KHN/KHN/T0796-8571656/JIZHOU DISTRICT OF JI AN CITY STAR     4.T KHN/HEART TOURISM CONSULTING CO  5.SSR FOID CA HK1 NI410223201301012826/P1    6.SSR OTHS 1E CA BKG CXLD DUE TO TKT TIME EXPIRED    7.SSR TKTL CA SS/ SHA 1254/23JUN14   8.OSI CA CTCT    9.PEK1E/HTX1XG/KHN117   
";

            // 返回结果：
            // {"PassengerList":[{"name":"张亮亮","idtype":0,"cardno":"NI232303198403141331","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KPHXV6","FlightList":[{"FlightNo":"CZ6411","Airline":"","Cabin":"Y","SubCabin":"","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T2","DepDate":"\/Date(1432218600000+0800)\/","ArrDate":"\/Date(1432226100000+0800)\/","PNRState":"HK1"},{"FlightNo":"CZ6412","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"SHA","DepTerminal":"T2","ArrTerminal":"T2","DepDate":"\/Date(1432420500000+0800)\/","ArrDate":"\/Date(1432428000000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["BJS410"],"BigPNR":"","Mobile":"18101810679","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[],"ResultBag":"\r\n1.张亮亮 KPHXV6                                                                  \r 2.  CZ6411 Y   TH21MAY  SHAPEK HK1   2230 0035+1        E T2T2                \r 3.  CZ6412 Y   SU24MAY  PEKSHA HK1   0635 0840          E T2T2                \r 4.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     \r     ABCDEFG                                                                   \r 5.TL/1113/21MAY/SHA888                                                        \r 6.SSR FOID CZ HK1 NI232303198403141331/P1                                     \r 7.OSI CZ CTCT18101810679                                                      \r 8.RMK TJ AUTH BJS410                                                          \r 9.SHA243                                                                      \r\n"}
            //string cmdResult8 =
            string cmdResult =
@"
1.张亮亮 KPHXV6                                                                  
 2.  CZ6411 Y   TH21MAY  SHAPEK HK1   2230 0035+1        E T2T2                
 3.  CZ6412 Y   SU24MAY  PEKSHA HK1   0635 0840          E T2T2                
 4.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     
     ABCDEFG                                                                   
 5.TL/1113/21MAY/SHA888                                                        
 6.SSR FOID CZ HK1 NI232303198403141331/P1                                     
 7.OSI CZ CTCT18101810679                                                      
 8.RMK TJ AUTH BJS410                                                          
 9.SHA243                                                                      
";

            //string[] list = null;
            //list = cmdResult.Split(new string[] { ">PAT:A" }, StringSplitOptions.RemoveEmptyEntries);

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();

            JetermEntity.Request.SeekPNR request = new JetermEntity.Request.SeekPNR();
            request.Pnr = "JWQHCG";
            request.PassengerType = EtermCommand.PassengerType.Adult;
            request.GetPrice = true;
            seekPNR.ParseCmd(request);
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            //Error error = new Error(EtermCommand.ERROR.SELFDEFINE_ERROR_MESSAGE);
            //error.ErrorMessage = string.Format("{0}_{1}", "123", "456");

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果为：" + Environment.NewLine + parseResult);
        }

#warning code here：补测--1、儿童往返票价格列表。

        //================================================================================


        //========================为帮助小波组测试而写的代码================================
        [TestMethod]
        public void Test_BusinessDispose1()
        { 
            JetermEntity.Request.SeekPNR request = new JetermEntity.Request.SeekPNR();

            #region 设置请求参数

            request.Pnr = "HE1B3G";
            request.PassengerType = EtermCommand.PassengerType.Children;
            request.GetPrice = true;

            #endregion

            EtermProxy.BLL.SeekPNR logic = new EtermProxy.BLL.SeekPNR(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.SeekPNR> result = logic.BusinessDispose(request);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}SeekPNR指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            /*
解析结果：
{"PassengerList":[{"name":"朱伟是CHD","idtype":0,"cardno":"NI20101010","PassType":1,"Ename":"","BirthDayString":"","ChildBirthDayDate":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"朱伟是CHD","idtype":-1,"cardno":"","PassType":1,"Ename":"","BirthDayString":"","ChildBirthDayDate":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HE1B3G","FlightList":[{"FlightNo":"MU5112","Airline":"","Cabin":"Y","SCity":"PEK","ECity":"SHA","DepTerminal":"T2","ArrTerminal":"T2","DepDate":"\/Date(1430283600000+0800)\/","ArrDate":"\/Date(1430291400000+0800)\/"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["KHN117"],"BigPNR":"PDQZTQ","Mobile":"13647125256","OfficeNo":"TPE566","AdultPnr":"KS6332","PriceList":[{"FacePrice":620.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":620.00}],"ResultBag":" 1.朱伟是CHD HE1B3G                                                               \r 2.  MU5112 Y   WE29APR  PEKSHA『NO1』  1300 1510          E T2T2                \r 3.TPE/T TPE/T0285692294/HUNG DA TRAVEL SERVICE CO LTD/SUN YUNG HUNG ABCDEFG   \r 4.TL/1818/22APR/KHN117                                                        \r 5.SSR FOID MU HK1 NI20101010/P1                                               \r 6.SSR OTHS 1E CNL DUE TO TL                                                   \r 7.SSR OTHS MU ADULT PNR IS KS6332                                             \r 8.SSR ADTK 1E BY TPE23APR15/1718 OR CXL MU5112 Y29APR                         \r 9.SSR CHLD MU HK1 01JAN10/P1                                                  \r10.OSI MU CTCT13647125256                                                      \r11.RMK TJ AUTH KHN117                                                          \r12.RMK 1212                                                                    +\r\r\n13.RMK CA/PDQZTQ                                                               -\r14.TPE566                                                                      \r\r\n\r\n>PAT:A*CH                                                                      \r01 YCH FARE:CNY620.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:620.00                 \r?SFC:01   ?SFN:01                                                              \r"}
             */
        }
        
        // 查询含婴儿的，航班信息
        [TestMethod]
        public void Test_SeekPNR_Baby_ParseCmdResult32()
        {
            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            string cmdResult1 =
@"
1.庞赞旺 2.郑炳勇 JD48H3                                                       
 3.  CZ3152 M   TH27MAR  PEKSZX RR2   1300 1605          E T2T3                 
 4.HKG/T HKG/T-21160677/CEPA TRAVEL (HK) COMPANY LIMITED/FRANKIE ABCDEFG        
 5.REM 0326 1324 JIANKONG                                                       
 6.T/00000000000                                                                
 7.SSR FOID CZ HK1 NI362330198712015898/P2                                      
 8.SSR FOID CZ HK1 NI422325197806093214/P1                                      
 9.SSR ADTK 1E BY HKG26MAR14/1525 OR CXL CZ BOOKING                             
10.SSR INFT CZ  KK1 PEKSZX 3152 M27MAR LI/SI 10OCT13/P2                         
11.SSR INFT CZ  KK1 PEKSZX 3152 M27MAR ZHANG/SAN 10OCT13/P1                     
12.OSI CZ CTCT18109251220                                                       
13.OSI YY 1INF ZHANGSANINF/P1                                                  +
                                                                               
                                                                                
pat:a*in                                                                       
>PAT:A*IN                                                                       
01 YIN FARE:CNY180.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:180.00                  
SFC:01  
 ";

            // 返回结果：{"PassengerList":[{"name":"元永梅","idtype":0,"cardno":"NI220582198002144227","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363241969"},{"name":"金泰润","idtype":-1,"cardno":"","PassType":2,"Ename":"JIN/TAIRUN ","BabyBirthday":"\/Date(1384704000000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"JV24GY","FlightList":[{"FlightNo":"CA1666","Airline":"","Cabin":"V1","SCity":"YNJ","ECity":"TSN","DepTerminal":"V1","ArrTerminal":null,"DepDate":"\/Date(1437906300000+0800)\/","ArrDate":"\/Date(1437914100000+0800)\/"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["LXA103","SIA302"],"BigPNR":"MES4HQ","Mobile":"15104338360","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":120.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":120.00}],"ResultBag":"\r\n\u0010RTJV24GY                                                                           **ELECTRONIC TICKET PNR**                                                        1.元永梅 JV24GY                                                                   2.  CA1666 V   SU26JUL  YNJTSN RR1   1825 2035          E      V1                 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG             ABCDEFG                                                                       4.T/WC/30APR/PEK001                                                               5.SSR FOID CA HK1 NI220582198002144227/P1                                         6.SSR OTHS 1E 1 PNR RR AND PRINTED                                                7.SSR OTHS 1E 1 CAAIRLINES ET PNR                                                 8.SSR ADTK 1E BY SHA30APR15/1402 OR CXL CA ALL SEGS                               9.SSR TKNE CA HK1 YNJTSN 1666 V26JUL INF9998900143749/1/P1                       10.SSR TKNE CA HK1 YNJTSN 1666 V26JUL 9992363241969/1/P1                          \u0010                                                                                                                                                                                                                                                     \u0010PN                                                                               11.SSR INFT CA  KK1 YNJTSN 1666 V26JUL JIN/TAIRUN 18NOV13/P1                   -  12.OSI CA CTCT15104338360                                                         13.OSI 1E CAET TN/9992363241969                                                   14.OSI YY 1INF JINTAIRUNINF/P1                                                    15.RMK TJ SIA302                                                                  16.RMK TJ AUTH LXA103                                                             17.RMK CA/MES4HQ                                                                  18.RMK TJ AUTH SIA302                                                             19.RMK AUTOMATIC FARE QUOTE                                                       20.FN/A/IN/FCNY120.00/SCNY120.00/C0.00/TEXEMPTCN/TEXEMPTYQ/ACNY120.00             21.TN/IN/999-8900143749/P1                                                        22.FP/IN/CASH,CNY                                                                 \u0010                                                                                                                                                                                                                                                     \u0010PN                                                                               23.XN/IN/金泰润INF(NOV13)/P1                                               -24.S  HA243                                                                             \u0010                                                                                                                                                                                                                                                     \u0010PAT:A*IN                                                                         >PAT:A*IN                                                                         01 YIN90 FARE:CNY120.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:120.00                  \u0010SFC:01   \u0010SFN:01   ,\r\n"}
            string cmdResult22 =
@"
RTJV24GY                                                                           **ELECTRONIC TICKET PNR**                                                        1.元永梅 JV24GY                                                                   2.  CA1666 V   SU26JUL  YNJTSN RR1   1825 2035          E      V1                 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG             ABCDEFG                                                                       4.T/WC/30APR/PEK001                                                               5.SSR FOID CA HK1 NI220582198002144227/P1                                         6.SSR OTHS 1E 1 PNR RR AND PRINTED                                                7.SSR OTHS 1E 1 CAAIRLINES ET PNR                                                 8.SSR ADTK 1E BY SHA30APR15/1402 OR CXL CA ALL SEGS                               9.SSR TKNE CA HK1 YNJTSN 1666 V26JUL INF9998900143749/1/P1                       10.SSR TKNE CA HK1 YNJTSN 1666 V26JUL 9992363241969/1/P1                                                                                                                                                                                                                                                                               PN                                                                               11.SSR INFT CA  KK1 YNJTSN 1666 V26JUL JIN/TAIRUN 18NOV13/P1                   -  12.OSI CA CTCT15104338360                                                         13.OSI 1E CAET TN/9992363241969                                                   14.OSI YY 1INF JINTAIRUNINF/P1                                                    15.RMK TJ SIA302                                                                  16.RMK TJ AUTH LXA103                                                             17.RMK CA/MES4HQ                                                                  18.RMK TJ AUTH SIA302                                                             19.RMK AUTOMATIC FARE QUOTE                                                       20.FN/A/IN/FCNY120.00/SCNY120.00/C0.00/TEXEMPTCN/TEXEMPTYQ/ACNY120.00             21.TN/IN/999-8900143749/P1                                                        22.FP/IN/CASH,CNY                                                                                                                                                                                                                                                                                                                      PN                                                                               23.XN/IN/金泰润INF(NOV13)/P1                                               -24.S  HA243                                                                                                                                                                                                                                                                                                                                  PAT:A*IN                                                                         >PAT:A*IN                                                                         01 YIN90 FARE:CNY120.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:120.00                  SFC:01   SFN:01   ,
";

            // 由彭珊给：
            // 返回结果：{"PassengerList":[{"name":"元永梅","idtype":0,"cardno":"NI220582198002144227","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363241969"},{"name":"金泰润","idtype":-1,"cardno":"","PassType":2,"Ename":"JIN/TAIRUN ","BabyBirthday":"\/Date(1384704000000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"JV24GY","FlightList":[{"FlightNo":"CA1666","Airline":"","Cabin":"V1","SCity":"YNJ","ECity":"TSN","DepTerminal":"V1","ArrTerminal":null,"DepDate":"\/Date(1437906300000+0800)\/","ArrDate":"\/Date(1437914100000+0800)\/"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["LXA103","SIA302"],"BigPNR":"MES4HQ","Mobile":"15104338360","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":120.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":120.00}],"ResultBag":"\r\n\u0010rtJV24GY                                                                       \r\n  **ELECTRONIC TICKET PNR**                                                     \r\n 1.元永梅 JV24GY                                                                \r\n 2.  CA1666 V   SU26JUL  YNJTSN RR1   1825 2035          E      V1              \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 4.T/WC/30APR/PEK001                                                            \r\n 5.SSR FOID CA HK1 NI220582198002144227/P1                                      \r\n 6.SSR OTHS 1E 1 PNR RR AND PRINTED                                             \r\n 7.SSR OTHS 1E 1 CAAIRLINES ET PNR                                              \r\n 8.SSR ADTK 1E BY SHA30APR15/1402 OR CXL CA ALL SEGS                            \r\n 9.SSR TKNE CA HK1 YNJTSN 1666 V26JUL INF9998900143749/1/P1                     \r\n10.SSR TKNE CA HK1 YNJTSN 1666 V26JUL 9992363241969/1/P1                       +\r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pn                                                                             \r\n11.SSR INFT CA  KK1 YNJTSN 1666 V26JUL JIN/TAIRUN 18NOV13/P1                   -\r\n12.OSI CA CTCT15104338360                                                       \r\n13.OSI 1E CAET TN/9992363241969                                                 \r\n14.OSI YY 1INF JINTAIRUNINF/P1                                                  \r\n15.RMK TJ SIA302                                                                \r\n16.RMK TJ AUTH LXA103                                                           \r\n17.RMK CA/MES4HQ                                                                \r\n18.RMK TJ AUTH SIA302                                                           \r\n19.RMK AUTOMATIC FARE QUOTE                                                     \r\n20.FN/A/IN/FCNY120.00/SCNY120.00/C0.00/TEXEMPTCN/TEXEMPTYQ/ACNY120.00           \r\n21.TN/IN/999-8900143749/P1                                                      \r\n22.FP/IN/CASH,CNY                                                              +\r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pn                                                                             \r\n23.XN/IN/金泰润INF(NOV13)/P1                                               -24.S\r\nHA243                                                                           \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat:a*in                                                                       \r\n>PAT:A*IN                                                                       \r\n01 YIN90 FARE:CNY120.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:120.00                \r\n\u0010SFC:01   \u0010SFN:01   \r\n"}
            string cmdResult =
@"
rtJV24GY                                                                       
  **ELECTRONIC TICKET PNR**                                                     
 1.元永梅 JV24GY                                                                
 2.  CA1666 V   SU26JUL  YNJTSN RR1   1825 2035          E      V1              
 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      
     ABCDEFG                                                                    
 4.T/WC/30APR/PEK001                                                            
 5.SSR FOID CA HK1 NI220582198002144227/P1                                      
 6.SSR OTHS 1E 1 PNR RR AND PRINTED                                             
 7.SSR OTHS 1E 1 CAAIRLINES ET PNR                                              
 8.SSR ADTK 1E BY SHA30APR15/1402 OR CXL CA ALL SEGS                            
 9.SSR TKNE CA HK1 YNJTSN 1666 V26JUL INF9998900143749/1/P1                     
10.SSR TKNE CA HK1 YNJTSN 1666 V26JUL 9992363241969/1/P1                       +
                                                                               
                                                                                
                                                                                
pn                                                                             
11.SSR INFT CA  KK1 YNJTSN 1666 V26JUL JIN/TAIRUN 18NOV13/P1                   -
12.OSI CA CTCT15104338360                                                       
13.OSI 1E CAET TN/9992363241969                                                 
14.OSI YY 1INF JINTAIRUNINF/P1                                                  
15.RMK TJ SIA302                                                                
16.RMK TJ AUTH LXA103                                                           
17.RMK CA/MES4HQ                                                                
18.RMK TJ AUTH SIA302                                                           
19.RMK AUTOMATIC FARE QUOTE                                                     
20.FN/A/IN/FCNY120.00/SCNY120.00/C0.00/TEXEMPTCN/TEXEMPTYQ/ACNY120.00           
21.TN/IN/999-8900143749/P1                                                      
22.FP/IN/CASH,CNY                                                              +
                                                                               
                                                                                
                                                                                
pn                                                                             
23.XN/IN/金泰润INF(NOV13)/P1                                               -24.S
HA243                                                                           
                                                                               
                                                                                
                                                                                
pat:a*in                                                                       
>PAT:A*IN                                                                       
01 YIN90 FARE:CNY120.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:120.00                
SFC:01   SFN:01   
";

            // 返回信息：{"PassengerList":[{"name":"王耀荣","idtype":0,"cardno":"NI130921198911264063","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"99923476297319992347629732"},{"name":"谢雪晴","idtype":0,"cardno":"NI13092119870104402X","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"谢依然","idtype":-1,"cardno":"","PassType":2,"Ename":"XIE/YIRAN ","BabyBirthday":"\/Date(1381507200000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HSZ42G","FlightList":[{"FlightNo":"CA4174","Airline":"","Cabin":"G","SCity":"PEK","ECity":"KMG","DepTerminal":"E","ArrTerminal":null,"DepDate":"\/Date(1445692800000+0800)\/","ArrDate":"\/Date(1445704500000+0800)\/"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["BJS407","SIA302"],"BigPNR":"MX5428","Mobile":"13833736118","OfficeNo":"KHN117","AdultPnr":null,"PriceList":[{"FacePrice":180.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":180.00}],"ResultBag":"\r\n**ELECTRONIC TICKET PNR**                                                        \r\n1.王耀荣 2.谢雪晴 HSZ42G                                                          \r\n3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035 1        E T3--                    \r\n4.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM            CONSULTING CO ABCDEFG                                                         \r\n5.TL/1156/22OCT/SHA888                                                            \r\n6.SSR FOID CA HK1 NI13092119870104402X/P2                                         \r\n7.SSR FOID CA HK1 NI130921198911264063/P1                                         \r\n8.SSR OTHS 1E 1 PNR RR AND PRINTED                                                \r\n9.SSR OTHS 1E 1 CAAIRLINES ET PNR                                                \r\n10.SSR ADTK 1E BY KHN22OCT14/1200 OR CXL CA ALL SEGS                              \r\n11.SSR TKNE CA HK1 PEKKMG 4174 G24OCT 9992347629731/1/P1                          \u0010                                                                                                                                                                                                                                                     \u0010PN                                                                               \r\n12.SSR TKNE CA HK1 PEKKMG 4174 G24OCT 9992347629732/1/P2                       -  \r\n13.SSR INFT CA  KK1 PEKKMG 4174 G24OCT XIE/YIRAN 12OCT13/P1                       \r\n14.OSI CA CTCT13833736118                                                         \r\n15.OSI 1E CAET TN/9992347629731-9992347629732                                     \r\n16.OSI YY 1INF XIEYIRANINF/P1                                                     \r\n17.RMK TJ AUTH BJS407                                                             \r\n18.RMK CA/MX5428                                                                  \r\n19.RMK TJ AUTH SIA302                                                             20.XN/IN/谢依然INF(OCT13)/P1                                                      21.KHN117                                                                         \u0010                                                                                                                                                                                                                                                     \u0010PAT:A*IN                                                                         >PAT:A*IN                                                                         \r\n01 YIN90 FARE:CNY180.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:180.00                  \r\n\u0010SFC:01, \r\n"}
            string cmdResult44 =
@"
**ELECTRONIC TICKET PNR**                                                        
1.王耀荣 2.谢雪晴 HSZ42G                                                          
3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035 1        E T3--                    
4.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM            CONSULTING CO ABCDEFG                                                         
5.TL/1156/22OCT/SHA888                                                            
6.SSR FOID CA HK1 NI13092119870104402X/P2                                         
7.SSR FOID CA HK1 NI130921198911264063/P1                                         
8.SSR OTHS 1E 1 PNR RR AND PRINTED                                                
9.SSR OTHS 1E 1 CAAIRLINES ET PNR                                                
10.SSR ADTK 1E BY KHN22OCT14/1200 OR CXL CA ALL SEGS                              
11.SSR TKNE CA HK1 PEKKMG 4174 G24OCT 9992347629731/1/P1                                                                                                                                                                                                                                                                               PN                                                                               
12.SSR TKNE CA HK1 PEKKMG 4174 G24OCT 9992347629732/1/P2                       -  
13.SSR INFT CA  KK1 PEKKMG 4174 G24OCT XIE/YIRAN 12OCT13/P1                       
14.OSI CA CTCT13833736118                                                         
15.OSI 1E CAET TN/9992347629731-9992347629732                                     
16.OSI YY 1INF XIEYIRANINF/P1                                                     
17.RMK TJ AUTH BJS407                                                             
18.RMK CA/MX5428                                                                  
19.RMK TJ AUTH SIA302                                                             20.XN/IN/谢依然INF(OCT13)/P1                                                      21.KHN117                                                                                                                                                                                                                                                                                                                              PAT:A*IN                                                                         >PAT:A*IN                                                                         
01 YIN90 FARE:CNY180.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:180.00                  
SFC:01, 
";

            // 返回结果：{"PassengerList":[{"name":"王耀荣","idtype":0,"cardno":"NI130921198911264063","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"99923476297319992347629732"},{"name":"谢雪晴","idtype":0,"cardno":"NI13092119870104402X","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"谢依然","idtype":-1,"cardno":"","PassType":2,"Ename":"XIE/YIRAN ","BabyBirthday":"\/Date(1381507200000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HSZ42G","FlightList":[{"FlightNo":"CA4174","Airline":"","Cabin":"G","SCity":"PEK","ECity":"KMG","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1445692800000+0800)\/","ArrDate":"\/Date(1445704500000+0800)\/"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["BJS407","SIA302"],"BigPNR":"MX5428","Mobile":"13833736118","OfficeNo":"KHN117","AdultPnr":null,"PriceList":[{"FacePrice":180.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":180.00}],"ResultBag":"\r\n**ELECTRONIC TICKET PNR**                                                        \r\n1.王耀荣 2.谢雪晴 HSZ42G                                                          \r\n3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035         E T3--                    \r\n4.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM            CONSULTING CO ABCDEFG                                                         \r\n5.TL/1156/22OCT/SHA888                                                            \r\n6.SSR FOID CA HK1 NI13092119870104402X/P2                                         \r\n7.SSR FOID CA HK1 NI130921198911264063/P1                                         \r\n8.SSR OTHS 1E 1 PNR RR AND PRINTED                                                \r\n9.SSR OTHS 1E 1 CAAIRLINES ET PNR                                                \r\n10.SSR ADTK 1E BY KHN22OCT14/1200 OR CXL CA ALL SEGS                              \r\n11.SSR TKNE CA HK1 PEKKMG 4174 G24OCT 9992347629731/1/P1                          \u0010                                                                                                                                                                                                                                                     \u0010PN                                                                               \r\n12.SSR TKNE CA HK1 PEKKMG 4174 G24OCT 9992347629732/1/P2                       -  \r\n13.SSR INFT CA  KK1 PEKKMG 4174 G24OCT XIE/YIRAN 12OCT13/P1                       \r\n14.OSI CA CTCT13833736118                                                         \r\n15.OSI 1E CAET TN/9992347629731-9992347629732                                     \r\n16.OSI YY 1INF XIEYIRANINF/P1                                                     \r\n17.RMK TJ AUTH BJS407                                                             \r\n18.RMK CA/MX5428                                                                  \r\n19.RMK TJ AUTH SIA302                                                             20.XN/IN/谢依然INF(OCT13)/P1                                                      21.KHN117                                                                         \u0010                                                                                                                                                                                                                                                     \u0010PAT:A*IN                                                                         >PAT:A*IN                                                                         \r\n01 YIN90 FARE:CNY180.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:180.00                  \r\n\u0010SFC:01, \r\n"}
            string cmdResult55 =
@"
**ELECTRONIC TICKET PNR**                                                        
1.王耀荣 2.谢雪晴 HSZ42G                                                          
3.  CA4174 G   FR24OCT  PEKKMG RR2   2120 0035         E T3--                    
4.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM            CONSULTING CO ABCDEFG                                                         
5.TL/1156/22OCT/SHA888                                                            
6.SSR FOID CA HK1 NI13092119870104402X/P2                                         
7.SSR FOID CA HK1 NI130921198911264063/P1                                         
8.SSR OTHS 1E 1 PNR RR AND PRINTED                                                
9.SSR OTHS 1E 1 CAAIRLINES ET PNR                                                
10.SSR ADTK 1E BY KHN22OCT14/1200 OR CXL CA ALL SEGS                              
11.SSR TKNE CA HK1 PEKKMG 4174 G24OCT 9992347629731/1/P1                                                                                                                                                                                                                                                                               PN                                                                               
12.SSR TKNE CA HK1 PEKKMG 4174 G24OCT 9992347629732/1/P2                       -  
13.SSR INFT CA  KK1 PEKKMG 4174 G24OCT XIE/YIRAN 12OCT13/P1                       
14.OSI CA CTCT13833736118                                                         
15.OSI 1E CAET TN/9992347629731-9992347629732                                     
16.OSI YY 1INF XIEYIRANINF/P1                                                     
17.RMK TJ AUTH BJS407                                                             
18.RMK CA/MX5428                                                                  
19.RMK TJ AUTH SIA302                                                             20.XN/IN/谢依然INF(OCT13)/P1                                                      21.KHN117                                                                                                                                                                                                                                                                                                                              PAT:A*IN                                                                         >PAT:A*IN                                                                         
01 YIN90 FARE:CNY180.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:180.00                  
SFC:01, 
";


            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }

        // 测试不应返回多航段的问题
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult()
        {
            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            //string cmdResult =
            string cmdResult1 =
@"
RTJE2ZY4                                                                       
 1.陈晓庆 JE2ZY4                                                                
 2.  CA1947 M   MO25MAY  PVGCTU HK1   0745 1050          E T2T2 M1              
 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      
     ABCDEFG                                                                    
 4.REM 0519 1326 JK002                                                          
 5.TL/0000/19MAY/SHA243                                                         
 6.SSR FOID CA HK1 NI320911198911224646/P1                                      
 7.SSR ADTK 1E BY SHA19MAY15/1527 OR CXL CA ALL SEGS                            
 8.OSI CA CTCT18917588289                                                       
 9.RMK CA/PTBJWJ                                                                
10.SHA243                                                                       
                                                                               
                                                                                
                                                                                
PAT:A                                                                          
>PAT:A                                                                          
01 M1 FARE:CNY1500.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1550.00                  
SFC:01   SFN:01                                                               
02 M FARE:CNY1550.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1600.00                   
SFC:02   SFN:02
 
 ";

            string cmdResult22 =
            //string cmdResult =
@"
RTKX7J31                                                                      
 1.张永华 KX7J31                                                                
 2.  CZ6624 Z   TU26MAY  CGQNKG HK1   0810 1030          E                      
 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       
 4.REM 0521 1056 CP041                                                          
 5.TL/1141/21MAY/BJS349                                                         
 6.SSR FOID                                                                     
 7.SSR ADTK 1E BY TPE21MAY15/1856 OR CXL CZ BOOKING                             
 8.OSI CZ CTCT18962298250                                                       
 9.RMK TJ AUTH CGQ203                                                           
10.RMK CA/NT1PB3                                                                
11.RMK TJ AUTH SHA255                                                           
12.TPE567                                                                       
                                                                               
                                                                                
                                                                                
PAT A                                                                          
>PAT:A                                                                          
01 Z FARE:CNY580.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:630.00                     
SFC:01   SFN:01

";

            // 返回结果：
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KW1F70","FlightList":[{"FlightNo":"CA1858","Airline":"","Cabin":"Y","SubCabin":"","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1432598100000+0800)\/","ArrDate":"\/Date(1432606500000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1689","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"HRB","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1432770300000+0800)\/","ArrDate":"\/Date(1432777800000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1678","Airline":"","Cabin":"Y","SubCabin":"","SCity":"HRB","ECity":"TSN","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432856700000+0800)\/","ArrDate":"\/Date(1432864800000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":2,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"MBSLFM","Mobile":"13127584465","OfficeNo":"TPE567","AdultPnr":null,"PriceList":[{"FacePrice":3370.00,"Tax":150.00,"Fuel":0.0,"TotalPrice":3520.00}],"ResultBag":"\r\n\u0010rtKW1F70                                                                       \r\n 1.ZI/XING KW1F70                                                               \r\n 2.  CA1858 Y   TU26MAY  SHAPEK HK1   0755 1015          E T2T3                 \r\n 3.  CA1689 Y   TH28MAY  PEKHRB HK1   0745 0950          E T3--                 \r\n 4.  CA1678 Y   FR29MAY  HRBTSN HK1   0745 1000          E                      \r\n 5.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       \r\n 6.REM 0525 1924 ZIXIN                                                          \r\n 7.13127584465                                                                  \r\n 8.TL/1058/25MAY/SHA888                                                         \r\n 9.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n10.SSR ADTK 1E BY TPE25MAY15/2124 OR CXL CA ALL SEGS                            \r\n11.OSI YY CTCT13127584465                                                       \r\n12.OSI CA CTCT02151812332                                                      +\r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pn                                                                             \r\n13.RMK TJ AUTH SHA255                                                          -\r\n14.RMK TJ AUTH SHA836                                                           \r\n15.RMK CA/MBSLFM                                                                \r\n16.TPE567                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 Y+Y+Y FARE:CNY3370.00 TAX:CNY150.00 YQ:TEXEMPTYQ  TOTAL:3520.00              \r\n\u0010SFC:01   \u0010SFN:01/01   \u0010SFN:01/02   \u0010SFN:01/03                                  \r\n\u0010                \r\n"}
            string cmdResult3 =
            //string cmdResult =
@"
rtKW1F70                                                                       
 1.ZI/XING KW1F70                                                               
 2.  CA1858 Y   TU26MAY  SHAPEK HK1   0755 1015          E T2T3                 
 3.  CA1689 Y   TH28MAY  PEKHRB HK1   0745 0950          E T3--                 
 4.  CA1678 Y   FR29MAY  HRBTSN HK1   0745 1000          E                      
 5.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       
 6.REM 0525 1924 ZIXIN                                                          
 7.13127584465                                                                  
 8.TL/1058/25MAY/SHA888                                                         
 9.SSR FOID CA HK1 NI320681198610238019/P1                                      
10.SSR ADTK 1E BY TPE25MAY15/2124 OR CXL CA ALL SEGS                            
11.OSI YY CTCT13127584465                                                       
12.OSI CA CTCT02151812332                                                      +
                                                                               
                                                                                
                                                                                
pn                                                                             
13.RMK TJ AUTH SHA255                                                          -
14.RMK TJ AUTH SHA836                                                           
15.RMK CA/MBSLFM                                                                
16.TPE567                                                                       
                                                                               
                                                                                
                                                                                
pat:a                                                                          
>PAT:A                                                                          
01 Y+Y+Y FARE:CNY3370.00 TAX:CNY150.00 YQ:TEXEMPTYQ  TOTAL:3520.00              
SFC:01   SFN:01/01   SFN:01/02   SFN:01/03                                  
                
";

            // 2015-06-12(5)tested:
            // 测试返回多航段的情况（航站楼格式为T3，包含【>PAT:A】）
            // 测试价格解析不出来的情况（解析不出来的原因：航站楼格式写成了T3，而不是T3--或--T3）
            // 返回结果：
            // {"PassengerList":[{"name":"张磊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KERP42","FlightList":[{"FlightNo":"HU7048","Airline":"","Cabin":"Q","SubCabin":"","SCity":"SHE","ECity":"HAK","DepTerminal":"T3","ArrTerminal":null,"DepDate":"\/Date(1434090300000+0800)\/","ArrDate":"\/Date(1434112200000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA243"],"BigPNR":"NGV27E","Mobile":"13518822261","OfficeNo":"HAK227","AdultPnr":null,"PriceList":[{"FacePrice":1520.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1570.00}],"ResultBag":"\r\nRT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 \r\n"}
            //string cmdResult =
                string cmdResult4 =
@"
RT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 
";

                string cmdResult =
                    //string cmdResult4_2 =
    @"
RT KNZDB6 1.白小菊 2.槐笑萱 3.槐洋 4.李嘉 KNZDB6 5. HU7280 A SU26JUL SYXPEK HK4 1645 2040 E T1 6.BJS/T PEK/T 010-65081681/HUAXIA AIR SERVICE CO.WWW.HUAXIAHANGKONG.COM/YUE DIAN WEI ABCDEFG 7.* 8.TL/1445/26JUL/BJS182 9.SSR FOID 10.SSR FOID 11.SSR FOID 12.SSR FOID 13.SSR ADTK 1E BY BJS12JUN15/1302CXL HU7280 A26JUL 14.SSR CHLD HU HK1 07AUG06/P2 PN 15.OSI CTC - 16.OSI HU CKIN SSAC/A 17.RMK 18.RMK 19.BJS182 PAT A >PAT:A 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01
";

            // 测试返回多航段的情况（航站楼格式为T3--或--T3，包含【>PAT:A】）
            // 测试价格解析不出来的情况（解析不出来的原因：航站楼格式写成了T3，而不是T3--或--T3）
            // 返回结果：
                // {"PassengerList":[{"name":"张磊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KERP42","FlightList":[{"FlightNo":"HU7048","Airline":"","Cabin":"Q","SubCabin":"","SCity":"SHE","ECity":"HAK","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1434090300000+0800)\/","ArrDate":"\/Date(1434112200000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA243"],"BigPNR":"NGV27E","Mobile":"13518822261","OfficeNo":"HAK227","AdultPnr":null,"PriceList":[{"FacePrice":1520.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1570.00}],"ResultBag":"\r\nRT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3-- 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 \r\n"}
                //string cmdResult =
                string cmdResult5 =
@"
RT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3-- 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 
";

                //string cmdResult =
                string cmdResult6 =
@"
RT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E --T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 
";

                // 测试返回多航段的情况（航站楼格式为T3，不包含【>PAT:A】，但包含【PAT:A】）
                // 测试价格解析不出来的情况（解析不出来的原因：航站楼格式写成了T3，而不是T3--或--T3）
                //string cmdResult =
                string cmdResult7 =
@"
RT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 
";

                //string cmdResult =
                    string cmdResult7_2 =
    @"
RT KNZDB6 1.白小菊 2.槐笑萱 3.槐洋 4.李嘉 KNZDB6 5. HU7280 A SU26JUL SYXPEK HK4 1645 2040 E T1 6.BJS/T PEK/T 010-65081681/HUAXIA AIR SERVICE CO.WWW.HUAXIAHANGKONG.COM/YUE DIAN WEI ABCDEFG 7.* 8.TL/1445/26JUL/BJS182 9.SSR FOID 10.SSR FOID 11.SSR FOID 12.SSR FOID 13.SSR ADTK 1E BY BJS12JUN15/1302CXL HU7280 A26JUL 14.SSR CHLD HU HK1 07AUG06/P2 PN 15.OSI CTC - 16.OSI HU CKIN SSAC/A 17.RMK 18.RMK 19.BJS182 PAT A PAT:A 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01
";


            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }              

        // 测试外宾证件号是否能被解析出
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult3()
        {
            // 返回结果：
            // {"PassengerList":[{"name":"BI/FENGSHENGADA","idtype":0,"cardno":"NIBA669428","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"高伟峰","idtype":0,"cardno":"NI440902197012152024","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"何燕华","idtype":0,"cardno":"NI132404197803280027","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"林柔红","idtype":0,"cardno":"NI440102197711084426","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"LIU/KATHERINEXUNYU","idtype":0,"cardno":"NIBA679910","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"LIU/SOPHIEZIYU","idtype":0,"cardno":"NIBA679909","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"李焱","idtype":0,"cardno":"NI430102197205153028","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"肖克平","idtype":0,"cardno":"NI440822195910081839","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HPR8TZ","FlightList":[{"FlightNo":"MU5732","Airline":"","Cabin":"E","SubCabin":"","SCity":"CAN","ECity":"KMG","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1437188100000+0800)\/","ArrDate":"\/Date(1437197400000+0800)\/","PNRState":"HK8"},{"FlightNo":"MU5741","Airline":"","Cabin":"E","SubCabin":"","SCity":"KMG","ECity":"CAN","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1437886200000+0800)\/","ArrDate":"\/Date(1437894300000+0800)\/","PNRState":"HK8"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["SHA712"],"BigPNR":"MXB5JL","Mobile":"13761876330","OfficeNo":"SHA666","AdultPnr":null,"PriceList":[{"FacePrice":2000.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2100.00}],"ResultBag":"\r\nrtHPR8TZ                                                                       \r\n 1.BI/FENGSHENGADA 2.高伟峰 3.何燕华 4.林柔红 5.LIU/KATHERINEXUNYU   6.LIU/SOPHI\r\nEZIYU 7.李焱 8.肖克平 HPR8TZ\r\n 9.  MU5732 E   SA18JUL  CANKMG HK8   1055 1330          E  \r\n10.  MU5741 E   SU26JUL  KMGCAN HK8   1250 1505          E  \r\n11.SHA/T SHA/T 021-52909090/SHANGHAI GUANGFA AIR-TICKET SERVICE CO.LTD/ \r\n    /ZHAOHONG ABCDEFG   \r\n12.TL/0855/18JUL/SHA666 \r\n13.SSR FOID MU HK1 NIBA679909/P6\r\n14.SSR FOID MU HK1 NIBA679910/P5\r\n15.SSR FOID MU HK1 NIBA669428/P1\r\n16.SSR FOID MU HK1 NI132404197803280027/P3  \r\n17.SSR FOID MU HK1 NI440102197711084426/P4                                     +\r\npn                                                                             \r\n18.SSR FOID MU HK1 NI440902197012152024/P2                                     -\r\n19.SSR FOID MU HK1 NI440822195910081839/P8  \r\n20.SSR FOID MU HK1 NI430102197205153028/P7  \r\n21.SSR ADTK 1E BY SHA28MAY15/1359 OR CXL MU5732 E18JUL  \r\n22.OSI MU CTCT13761876330   \r\n23.RMK CA/MXB5JL\r\n24.RMK TJ AUTH SHA712   \r\n25.SHA666   \r\npat:a                                                                          \r\n>PAT:A  \r\n01 RT/E+RT/E FARE:CNY2000.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2100.00\r\n"}
            string cmdResult =
@"
rtHPR8TZ                                                                       
 1.BI/FENGSHENGADA 2.高伟峰 3.何燕华 4.林柔红 5.LIU/KATHERINEXUNYU   6.LIU/SOPHI
EZIYU 7.李焱 8.肖克平 HPR8TZ
 9.  MU5732 E   SA18JUL  CANKMG HK8   1055 1330          E  
10.  MU5741 E   SU26JUL  KMGCAN HK8   1250 1505          E  
11.SHA/T SHA/T 021-52909090/SHANGHAI GUANGFA AIR-TICKET SERVICE CO.LTD/ 
    /ZHAOHONG ABCDEFG   
12.TL/0855/18JUL/SHA666 
13.SSR FOID MU HK1 NIBA679909/P6
14.SSR FOID MU HK1 NIBA679910/P5
15.SSR FOID MU HK1 NIBA669428/P1
16.SSR FOID MU HK1 NI132404197803280027/P3  
17.SSR FOID MU HK1 NI440102197711084426/P4                                     +
pn                                                                             
18.SSR FOID MU HK1 NI440902197012152024/P2                                     -
19.SSR FOID MU HK1 NI440822195910081839/P8  
20.SSR FOID MU HK1 NI430102197205153028/P7  
21.SSR ADTK 1E BY SHA28MAY15/1359 OR CXL MU5732 E18JUL  
22.OSI MU CTCT13761876330   
23.RMK CA/MXB5JL
24.RMK TJ AUTH SHA712   
25.SHA666   
pat:a                                                                          
>PAT:A  
01 RT/E+RT/E FARE:CNY2000.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2100.00
";

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }

        // 测试【时间和城市三字码取值取不到】的问题
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult4()
        {           
            // 返回结果：
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI430481198706145364","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"JG2QSR","FlightList":[{"FlightNo":"CZ3323","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CAN","ECity":"ZHA","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1433030100000+0800)\/","ArrDate":"\/Date(1433034000000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"NDKGYW","Mobile":"13564922652","OfficeNo":"KHN117","AdultPnr":null,"PriceList":[{"FacePrice":970.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1020.00}],"ResultBag":"\r\n RTJG2QSR                                                                       \r\n 1.ZI/XING JG2QSR                                                               \r\n 2.  CZ3323 Y   SU31MAY  CANZHA HK1   0755 0900          E                      \r\n 3.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM     \r\n     CONSULTING CO ABCDEFG                                                      \r\n 4.REM 0416 1447 ZIXIN                                                          \r\n 5.TL/2300/16APR/SHA255                                                         \r\n 6.SSR FOID CZ HK1 NI430481198706145364/P1                                      \r\n 7.SSR ADTK 1E BY KHN19APR15/1448 OR CXL CZ BOOKING                             \r\n 8.OSI CZ CTCT13564922652                                                       \r\n 9.RMK CA/NDKGYW                                                                \r\n10.KHN117                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 Y FARE:CNY970.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1020.00                    \r\n SFC:01    SFN:01 \r\n"}
            // 
            //string cmdResult =
                string cmdResult1 =
@"
 RTJG2QSR                                                                       
 1.ZI/XING JG2QSR                                                               
 2.  CZ3323 Y   SU31may  CANZHA HK1   0755 0900          E                      
 3.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM     
     CONSULTING CO ABCDEFG                                                      
 4.REM 0416 1447 ZIXIN                                                          
 5.TL/2300/16APR/SHA255                                                         
 6.SSR FOID CZ HK1 NI430481198706145364/P1                                      
 7.SSR ADTK 1E BY KHN19APR15/1448 OR CXL CZ BOOKING                             
 8.OSI CZ CTCT13564922652                                                       
 9.RMK CA/NDKGYW                                                                
10.KHN117                                                                       
                                                                                
                                                                                
                                                                                
 PAT:A                                                                          
>PAT:A                                                                          
01 Y FARE:CNY970.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1020.00                    
 SFC:01    SFN:01 
";

            // 返回结果：
                // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KS7DGH","FlightList":[{"FlightNo":"CZ3678","Airline":"","Cabin":"Y","SubCabin":"","SCity":"KMG","ECity":"PVG","DepTerminal":"","ArrTerminal":"T2","DepDate":"\/Date(1432958700000+0800)\/","ArrDate":"\/Date(1432968600000+0800)\/","PNRState":"HK1"},{"FlightNo":"CZ6799","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PVG","ECity":"KMG","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1432971300000+0800)\/","ArrDate":"\/Date(1432983300000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":[],"BigPNR":"MYGR5H","Mobile":"13127584465","OfficeNo":"KHN117","AdultPnr":null,"PriceList":[{"FacePrice":4180.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":4280.00}],"ResultBag":"\r\nRTKS7DGH \r\n 1.ZI/XING KS7DGH \r\n 2. CZ3678 Y SA30MAY KMGPVG HK1 1205 1450 E --T2 \r\n 3. CZ6799 Y SA30MAY PVGKMG HK1 1535 1855 E T2-- \r\n 4.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM \r\n     CONSULTING CO ABCDEFG \r\n 5.REM 0417 1151 ZIXIN \r\n 6.13127584465 \r\n 7.TL/1058/17APR/SHA888 \r\n 8.SSR FOID CZ HK1 NI320681198610238019/P1 \r\n 9.SSR ADTK 1E BY KHN20APR15/1151 OR CXL CZ BOOKING \r\n10.OSI YY CTCT13127584465 \r\n11.OSI CZ CTCT02151812332 + \r\n                                                                                \r\n                                                                                 \r\n                                                                                 \r\nPN \r\n12.RMK CA/MYGR5H - \r\n13.KHN117 \r\n                                                                                \r\n                                                                                 \r\n                                                                                 \r\nPAT:A \r\n>PAT:A \r\n01 Y+Y FARE:CNY4180.00 TAX:CNY100.00 YQ:TEXEMPTYQ TOTAL:4280.00 \r\nSFC:01 SFN:01/01 SFN:01/02\r\n"}
            string cmdResult =
            //string cmdResult22 =
@"
rtKS7DGH 
 1.ZI/XING KS7DGH 
 2. CZ3678 Y sa30may KMGPVG HK1 1205 1450 E --T2 
 3. CZ6799 Y sa30may PVGKMG HK1 1535 1855 E T2-- 
 4.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM 
     CONSULTING CO ABCDEFG 
 5.REM 0417 1151 ZIXIN 
 6.13127584465 
 7.TL/1058/17APR/SHA888 
 8.SSR FOID CZ HK1 NI320681198610238019/P1 
 9.SSR ADTK 1E BY KHN20APR15/1151 OR CXL CZ BOOKING 
10.OSI YY CTCT13127584465 
11.OSI CZ CTCT02151812332 + 
                                                                                
                                                                                 
                                                                                 
pn 
12.RMK CA/MYGR5H - 
13.KHN117 
                                                                                
                                                                                 
                                                                                 
pat:a 
>PAT:A 
01 Y+Y FARE:CNY4180.00 TAX:CNY100.00 YQ:TEXEMPTYQ TOTAL:4280.00 
SFC:01 SFN:01/01 SFN:01/02
";

            cmdResult = cmdResult.ToUpper();

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }

        // 测试【解析不到证件号】的问题
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult5()
        {
            // 返回结果：
            // {"PassengerList":[{"name":"张永华","idtype":0,"cardno":"NI4221356","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KX7J31","FlightList":[{"FlightNo":"CZ6624","Airline":"","Cabin":"Z","SubCabin":"","SCity":"CGQ","ECity":"NKG","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432599000000+0800)\/","ArrDate":"\/Date(1432607400000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["CGQ203","SHA255"],"BigPNR":"NT1PB3","Mobile":"18962298250","OfficeNo":"TPE567","AdultPnr":null,"PriceList":[{"FacePrice":580.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":630.00}],"ResultBag":"\r\n RTKX7J31                                                                      \r\n 1.张永华 KX7J31                                                                \r\n 2.  CZ6624 Z   TU26MAY  CGQNKG HK1   0810 1030          E                      \r\n 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       \r\n 4.REM 0521 1056 CP041                                                          \r\n 5.TL/1141/21MAY/BJS349                                                         \r\n 6.SSR FOID CZ HK NI4221356/P1                                                                     \r\n 7.SSR ADTK 1E BY TPE21MAY15/1856 OR CXL CZ BOOKING                             \r\n 8.OSI CZ CTCT18962298250                                                       \r\n 9.RMK TJ AUTH CGQ203                                                           \r\n10.RMK CA/NT1PB3                                                                \r\n11.RMK TJ AUTH SHA255                                                           \r\n12.TPE567                                                                       \r\n                                                                               \r\n                                                                                \r\n                                                                                \r\nPAT A                                                                          \r\n>PAT:A                                                                          \r\n01 Z FARE:CNY580.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:630.00                     \r\nSFC:01   SFN:01 \r\n"}
            // 
            string cmdResult =
                //string cmdResult1 =
@"
 RTKX7J31                                                                      
 1.张永华 KX7J31                                                                
 2.  CZ6624 Z   TU26MAY  CGQNKG HK1   0810 1030          E                      
 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       
 4.REM 0521 1056 CP041                                                          
 5.TL/1141/21MAY/BJS349                                                         
 6.SSR FOID cz hk ni4221356/p1                                                                     
 7.SSR ADTK 1E BY TPE21MAY15/1856 OR CXL CZ BOOKING                             
 8.OSI CZ CTCT18962298250                                                       
 9.RMK TJ AUTH CGQ203                                                           
10.RMK CA/NT1PB3                                                                
11.RMK TJ AUTH SHA255                                                           
12.TPE567                                                                       
                                                                               
                                                                                
                                                                                
PAT A                                                                          
>PAT:A                                                                          
01 Z FARE:CNY580.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:630.00                     
SFC:01   SFN:01 
";

            cmdResult = cmdResult.ToUpper();

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }     

        // 测试时间没解析到的问题
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult6()
        {
            // 返回结果：
            // 
            // 
            string cmdResult =
                //string cmdResult1 =
@"
RT KNZDB6 1.白小菊 2.槐笑萱 3.槐洋 4.李嘉 KNZDB6 5. HU7280 A SU26JUL SYXPEK HK4 1645 2040 E --T1 6.BJS/T PEK/T 010-65081681/HUAXIA AIR SERVICE CO.WWW.HUAXIAHANGKONG.COM/YUE DIAN WEI ABCDEFG 7.* 8.TL/1445/26JUL/BJS182 9.SSR FOID 10.SSR FOID 11.SSR FOID 12.SSR FOID 13.SSR ADTK 1E BY BJS12JUN15/1302 OR CXL HU7280 A26JUL 14.SSR CHLD HU HK1 07AUG06/P2 PAT A >PAT:A 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01
";

            cmdResult = cmdResult.ToUpper();

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }     
        
        // 测试时间是否能解析出来
        // = 测试为什么会返回“传入的日期和星期不匹配”
        // 测试为什么会返回“此记录编号暂时无法导入创建订单，请联系我们021-60727501”
        // 测试不完整的成人PNR信息
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult101()
        {
            JetermEntity.Request.SeekPNR request = new JetermEntity.Request.SeekPNR();

            
            // 测试时间是否能解析出来
            // = 测试为什么会返回“传入的日期和星期不匹配”
            // 返回结果：
            // 此记录第1段航班信息的编号位置被“NO”掉了，请检查您的PNR记录编号
            //string cmdResult =
                string cmdResult1 =
@"
rt KX2KWF                                                                      
 1.林玉珊 KX2KWF                                                                
 2.  CZ6912 U   TH11JUN  PEKURC NO1   2125 0130+1        E T2T3                 
 3.BJS/T PEK/T-65906699/BEIJING CHINA EXPRESS INTERNATIONAL AIR SERVICE         
     CO.,LTD/WEI CHONG ABCDEFG                                                  
 4.*                                                                            
 5.TL/2100/11JUN/BJS187                                                         
 6.SSR FOID                                                                     
 7.SSR OTHS 1E CZ BKG CXLD DUE ATTL EXPIRED WITHOUT TKNE                        
 8.OSI YY CTCT84045714                                                          
 9.OSI CZ CTCT13601289859                                                       
10.RMK                                                                          
11.RMK                                                                         +
                                                                               
                                                                                
                                                                                
pn                                                                             
12.BJS187                                                                      -
                                                                               
                                                                                
                                                                                
pat:a                                                                          
>PAT:A                                                                          
01 U FARE:CNY1690.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1740.00                   
SFC:01   SFN:01                                                               
   

";

            // 返回结果：
                // {"PassengerList":[{"name":"ZI/XING CHD","idtype":0,"cardno":"NI20090123","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1232640000000+0800)\/","TicketNo":""}],"PNR":"HMZFD7","FlightList":[{"FlightNo":"HO1252","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1434235800000+0800)\/","ArrDate":"\/Date(1434243900000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"MZ2P8D","Mobile":"13641601096","OfficeNo":"TPE567","AdultPnr":"JN2FYD","PriceList":[{"FacePrice":620.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":620.00}],"ResultBag":"\r\n\u0010rtHMZFD7                                                                       \r\n 1.ZI/XING CHD HMZFD7                                                           \r\n 2.  HO1252 Y   SU14jun  PEKSHA HK1   0650 0905          E T3T2                 \r\n 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       \r\n 4.TL/1912/17APR/BJS579                                                         \r\n 5.SSR FOID HO HK1 NI20090123/P1                                                \r\n 6.SSR OTHS HO ADULT PNR IS JN2FYD                                              \r\n 7.SSR ADTK 1E BY TPE17APR15/1912 OR CXL HO1252 Y19APR                          \r\n 8.SSR CHLD HO HK1 23JAN09/P1                                                   \r\n 9.OSI HO CTCT13641601096                                                       \r\n10.RMK CA/MZ2P8D                                                                \r\n11.TPE567                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat:a*ch                                                                       \r\n>PAT:A*CH                                                                       \r\n01 YCH FARE:CNY620.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:620.00                  \r\n\u0010SFC:01   \u0010SFN:01                                                               \r\n\u0010                       \r\n"}
            // 测试为什么会返回“此记录编号暂时无法导入创建订单，请联系我们021-60727501”
            //string cmdResult =
            string cmdResult222 =
@"
rtHMZFD7                                                                       
 1.ZI/XING CHD HMZFD7                                                           
 2.  HO1252 Y   SU14jun  PEKSHA HK1   0650 0905          E T3T2                 
 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       
 4.TL/1912/17APR/BJS579                                                         
 5.SSR FOID HO HK1 NI20090123/P1                                                
 6.SSR OTHS HO ADULT PNR IS JN2FYD                                              
 7.SSR ADTK 1E BY TPE17APR15/1912 OR CXL HO1252 Y19APR                          
 8.SSR CHLD HO HK1 23JAN09/P1                                                   
 9.OSI HO CTCT13641601096                                                       
10.RMK CA/MZ2P8D                                                                
11.TPE567                                                                       
                                                                               
                                                                                
                                                                                
pat:a*ch                                                                       
>PAT:A*CH                                                                       
01 YCH FARE:CNY620.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:620.00                  
SFC:01   SFN:01                                                               
                       
";
            // 测试不完整的成人PNR信息
            string cmdResult =
@"
rtJR75QD                                                                       
  **ELECTRONIC TICKET PNR**                                                     
 1.武正安 JR75QD                                                                
 2.  MF8347 Y   WE17JUN  FOCCAN RR1   1440 1605          E                      
 3.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM     
     CONSULTING CO ABCDEFG                                                      
 4.T/0000000000000                                                              
 5.SSR FOID MF HK1 NI342201196603262136/P1                                      
 6.SSR OTHS 1E 1 PNR RR AND PRINTED                                             
 7.SSR OTHS 1E 1 MFAIRLINES ET PNR                                              
 8.SSR FQTV MF HK1 FOCCAN 8347 Y17JUN MF1111136051/C/P1                         
 9.SSR ADTK 1E BY KHN15JUN15/1559 OR CXL MF8347 Y17JUN                          
10.SSR TKNE MF HK1 FOCCAN 8347 Y17JUN 7312396431136/1/P1                       +
                                                                               
                                                                                

     
pat:a*in                                                                       
>PAT:A*IN                                                                       
01 YIN FARE:CNY180.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:180.00                  
SFC:01 
";


//            // 返回结果：
//            // {"PassengerList":[{"name":"ZI/XING CHD","idtype":0,"cardno":"NI20090123","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1232640000000+0800)\/","TicketNo":""}],"PNR":"HMZFD7","FlightList":[{"FlightNo":"HO1252","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1434235800000+0800)\/","ArrDate":"\/Date(1434243900000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"MZ2P8D","Mobile":"13641601096","OfficeNo":"TPE567","AdultPnr":"JN2FYD","PriceList":[{"FacePrice":620.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":620.00}],"ResultBag":"\r\n\u0010rtHMZFD7                                                                       \r\n 1.ZI/XING CHD HMZFD7                                                           \r\n 2.  HO1252 Y   SU14JUN  PEKSHA HK1   0650 0905          E T3T2                 \r\n 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       \r\n 4.TL/1912/17APR/BJS579                                                         \r\n 5.SSR FOID HO HK1 NI20090123/P1                                                \r\n 6.SSR OTHS HO ADULT PNR IS JN2FYD                                              \r\n 7.SSR ADTK 1E BY TPE17APR15/1912 OR CXL HO1252 Y19APR                          \r\n 8.SSR CHLD HO HK1 23JAN09/P1                                                   \r\n 9.OSI HO CTCT13641601096                                                       \r\n10.RMK CA/MZ2P8D                                                                \r\n11.TPE567                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat:a*ch                                                                       \r\n>PAT:A*CH                                                                       \r\n01 YCH FARE:CNY620.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:620.00                  \r\n\u0010SFC:01   \u0010SFN:01                                                               \r\n\u0010                       \r\n\r\n"}
//            //string cmdResult3 =
//               string cmdResult =
//@"
//rtHMZFD7                                                                       
// 1.ZI/XING CHD HMZFD7                                                           
// 2.  HO1252 Y   SU14JUN  PEKSHA HK1   0650 0905          E T3T2                 
// 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       
// 4.TL/1912/17APR/BJS579                                                         
// 5.SSR FOID HO HK1 NI20090123/P1                                                
// 6.SSR OTHS HO ADULT PNR IS JN2FYD                                              
// 7.SSR ADTK 1E BY TPE17APR15/1912 OR CXL HO1252 Y19APR                          
// 8.SSR CHLD HO HK1 23JAN09/P1                                                   
// 9.OSI HO CTCT13641601096                                                       
//10.RMK CA/MZ2P8D                                                                
//11.TPE567                                                                       
//                                                                               
//                                                                                
//                                                                                
//pat:a*ch                                                                       
//>PAT:A*CH                                                                       
//01 YCH FARE:CNY620.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:620.00                  
//SFC:01   SFN:01                                                               
//                       
//
//";

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            Console.ReadLine();

            #endregion
        }

        // 测试票号的问题
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult102()
        {
            // 没有票号的情况：
            // 返回结果：
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"沈燕彬","idtype":0,"cardno":"NI513322197103192519","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KWSWH9","FlightList":[{"FlightNo":"CZ3461","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CSX","ECity":"CTU","DepTerminal":"","ArrTerminal":"T2","DepDate":"\/Date(1451283900000+0800)\/","ArrDate":"\/Date(1451291700000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["CSX141"],"BigPNR":"NBV0F1","Mobile":"13548746642","OfficeNo":"KHN117","AdultPnr":null,"PriceList":[{"FacePrice":910.00,"Tax":50.00,"Fuel":60.00,"TotalPrice":1020.00}],"ResultBag":"\r\nrtKWSWH9                                                                       \r\n 1.沈燕彬 KWSWH9                                                                \r\n 2.  CZ3461 Y   SU28DEC  CSXCTU HK1   1425 1635          E --T2                 \r\n 3.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM     \r\n     CONSULTING CO ABCDEFG                                                      \r\n 4.REM 1223 1558 ROCK                                                           \r\n 5.TL/1116/23DEC/SHA888                                                         \r\n 6.SSR FOID CZ HK1 NI513322197103192519/P1                                      \r\n 7.SSR ADTK 1E BY KHN24DEC14/0355 OR CXL CZ BOOKING                             \r\n 8.OSI CZ CTCT13548746642                                                       \r\n 9.RMK TJ AUTH CSX141                                                           \r\n10.RMK CA/NBV0F1                                                                \r\n11.KHN117                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 Y FARE:CNY910.00 TAX:CNY50.00 YQ:CNY60.00  TOTAL:1020.00                     \r\n\u0010SFC:01   \u0010SFN:01\r\n"},"reqtime":"\/Date(1435139075827+0800)\/","SaveTime":1800}
            //string cmdResult =
          string cmdResult1 =
@"
rtKWSWH9                                                                       
 1.沈燕彬 KWSWH9                                                                
 2.  CZ3461 Y   SU28DEC  CSXCTU HK1   1425 1635          E --T2                 
 3.KHN/T KHN/T0796-8571656/JIZHOU DISTRICT OF JI'AN CITY STAR HEART TOURISM     
     CONSULTING CO ABCDEFG                                                      
 4.REM 1223 1558 ROCK                                                           
 5.TL/1116/23DEC/SHA888                                                         
 6.SSR FOID CZ HK1 NI513322197103192519/P1                                      
 7.SSR ADTK 1E BY KHN24DEC14/0355 OR CXL CZ BOOKING                             
 8.OSI CZ CTCT13548746642                                                       
 9.RMK TJ AUTH CSX141                                                           
10.RMK CA/NBV0F1                                                                
11.KHN117                                                                       
                                                                               
                                                                                
                                                                                
PAT:A                                                                          
>PAT:A                                                                          
01 Y FARE:CNY910.00 TAX:CNY50.00 YQ:CNY60.00  TOTAL:1020.00                     
SFC:01   SFN:01
";

            // 有【SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787182/1/P1】和【OSI 1E CAET TN/9992375787182-9992375787183】这两行命令：
            // 返回结果：
          // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"李YUE","idtype":0,"cardno":"NI620102198111104620","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992375787182"},{"name":"张蓉","idtype":0,"cardno":"NI610302198301140568","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992375787183"}],"PNR":"KF1QET","FlightList":[{"FlightNo":"CA4172","Airline":"","Cabin":"Q","SubCabin":"","SCity":"PEK","ECity":"KMG","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1434599400000+0800)\/","ArrDate":"\/Date(1434612000000+0800)\/","PNRState":"RR2"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"MJYPC2","Mobile":"63154444","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":1310.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1360.00}],"ResultBag":"\r\n  **ELECTRONIC TICKET PNR** \r\n 1.李YUE 2.张蓉 KF1QET  \r\n 3.  CA4172 Q   TH18JUN  PEKKMG RR2   1150 1520      E T3-- \r\n 4.T KMG/KMG/T0871-65162666/EAST COAST AVIATION SERVICES CO.\r\n 5.T KMG/LTD IN YUNNAN/LUO WENQIANG \r\n 6.T\r\n 7.SSR FOID CA HK1 NI610302198301140568/P2  \r\n 8.SSR FOID CA HK1 NI620102198111104620/P1  \r\n 9.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787183/1/P2\r\n10.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787182/1/P1\r\n11.SSR OTHS 1E 1 CAAIRLINES ET PNR  \r\n12.SSR OTHS 1E 1 PNR RR AND PRINTED                                            +\r\n\r\n\u001e13.SSR OTHS 1E CA BKG CXLD DUE TO TKT TIME EXPIRED                             -\r\n\r\n14.SSR TKTL CA XX/ KMG 1900/17JUN15 \r\n15.SSR FQTV CA HK1 PEKKMG 4172 Q18JUN CA008334377481/C/P2   \r\n16.OSI CA CTC 63154444  \r\n17.OSI YY CTCT63154444  \r\n18.OSI CA CTCT1388*****49   \r\n19.OSI 1E CAET TN/9992375787182-9992375787183   \r\n20.RMK B2BPLATFORM WEB IMPORT   \r\n21.FN/M/FCNY1310.00/SCNY1310.00/C6.00/XCNY50.00/TCNY50.00CN/TEXEMPTYQ/  \r\n    ACNY1360.00 \r\n22.TN/999-2375787182/P1 \r\n23.TN/999-2375787183/P2                                                        +\r\n\r\n\u001e24.FP/CC/Y1                                                                    -\r\n\r\n25.PEK1E/KF1QET/KMG168  \r\n\u001e[price]>PAT:A  \r\n01 Q FARE:CNY1310.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1360.00   \r\n\u001eSFC:01   \u001eSFN:01   \r\n\u001e[eTerm:caa01] RMK CA/MJYPC2\r\n"},"reqtime":"\/Date(1435139178478+0800)\/","SaveTime":1800}
            string cmdResult222 =
           //string cmdResult =
@"
  **ELECTRONIC TICKET PNR** 
 1.李YUE 2.张蓉 KF1QET  
 3.  CA4172 Q   TH18JUN  PEKKMG RR2   1150 1520      E T3-- 
 4.T KMG/KMG/T0871-65162666/EAST COAST AVIATION SERVICES CO.
 5.T KMG/LTD IN YUNNAN/LUO WENQIANG 
 6.T
 7.SSR FOID CA HK1 NI610302198301140568/P2  
 8.SSR FOID CA HK1 NI620102198111104620/P1  
 9.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787183/1/P2
10.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787182/1/P1
11.SSR OTHS 1E 1 CAAIRLINES ET PNR  
12.SSR OTHS 1E 1 PNR RR AND PRINTED                                            +

13.SSR OTHS 1E CA BKG CXLD DUE TO TKT TIME EXPIRED                             -

14.SSR TKTL CA XX/ KMG 1900/17JUN15 
15.SSR FQTV CA HK1 PEKKMG 4172 Q18JUN CA008334377481/C/P2   
16.OSI CA CTC 63154444  
17.OSI YY CTCT63154444  
18.OSI CA CTCT1388*****49   
19.OSI 1E CAET TN/9992375787182-9992375787183   
20.RMK B2BPLATFORM WEB IMPORT   
21.FN/M/FCNY1310.00/SCNY1310.00/C6.00/XCNY50.00/TCNY50.00CN/TEXEMPTYQ/  
    ACNY1360.00 
22.TN/999-2375787182/P1 
23.TN/999-2375787183/P2                                                        +

24.FP/CC/Y1                                                                    -

25.PEK1E/KF1QET/KMG168  
[price]>PAT:A  
01 Q FARE:CNY1310.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1360.00   
SFC:01   SFN:01   
[eTerm:caa01] RMK CA/MJYPC2
";

            // 只有【SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508902/1/P3】这行命令：
            // 返回结果：
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"DANAHER/MAE XIANG","idtype":0,"cardno":"NI478536670","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7842178508900"},{"name":"PERRY/KYLIE HONGSHUN","idtype":0,"cardno":"NI517925439","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7842178508901"},{"name":"PERRY/REBECCA SUE","idtype":0,"cardno":"NI518926188","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7842178508902"}],"PNR":"KXN58Y","FlightList":[{"FlightNo":"CZ6670","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CTU","ECity":"KWL","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1434863100000+0800)\/","ArrDate":"\/Date(1434869400000+0800)\/","PNRState":"UN3"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"NWEXF0","Mobile":"18001367952","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":980.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1030.00}],"ResultBag":"\r\n 1.DANAHER/MAE XIANG 2.PERRY/KYLIE HONGSHUN 3.PERRY/REBECCA SUE KXN58Y  \r\n 4.  CZ6670 Y   SU21JUN  CTUKWL UN3   1305 1450      E T2-- \r\n 5.T BJS/PEK/T-65906699/BEIJING CHINA EXPRESS INTERNATIONAL AI  \r\n 6.T BJS/R SERVICE CO. LTD/WEI CHONG\r\n 7.T/APT\r\n 8.BA/GCP ALDY PAM TOT CNY  \r\n 9.SSR FOID CZ HK1 NI517925439/P2   \r\n10.SSR FOID CZ HK1 NI518926188/P3   \r\n11.SSR FOID CZ HK1 NI478536670/P1   \r\n12.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508902/1/P3\r\n13.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508901/1/P2\r\n14.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508900/1/P1                       + 15.SSR TKTL CA SS/ PEK 1200/01JUN15                                            -16.OSI CA CTC CHELI \r\n17.OSI CZ CTCT18001367952   \r\n18.OSI YY RLOC PEK1EJQPSW6  \r\n19.OSI CZ LSH GCP020150511370151\r\n20.PEK1E/KXN58Y/PEK587  \r\n [price]>PAT:A  \r\n01 Y FARE:CNY980.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1030.00\r\n SFC:01    SFN:01   \r\n  RMK CA/NWEXF0\r\n"},"reqtime":"\/Date(1435139468901+0800)\/","SaveTime":1800}
            //string cmdResult3 =
            string cmdResult =
@"
 1.DANAHER/MAE XIANG 2.PERRY/KYLIE HONGSHUN 3.PERRY/REBECCA SUE KXN58Y  
 4.  CZ6670 Y   SU21JUN  CTUKWL UN3   1305 1450      E T2-- 
 5.T BJS/PEK/T-65906699/BEIJING CHINA EXPRESS INTERNATIONAL AI  
 6.T BJS/R SERVICE CO. LTD/WEI CHONG
 7.T/APT
 8.BA/GCP ALDY PAM TOT CNY  
 9.SSR FOID CZ HK1 NI517925439/P2   
10.SSR FOID CZ HK1 NI518926188/P3   
11.SSR FOID CZ HK1 NI478536670/P1   
12.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508902/1/P3
13.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508901/1/P2
14.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508900/1/P1                       + 15.SSR TKTL CA SS/ PEK 1200/01JUN15                                            -16.OSI CA CTC CHELI 
17.OSI CZ CTCT18001367952   
18.OSI YY RLOC PEK1EJQPSW6  
19.OSI CZ LSH GCP020150511370151
20.PEK1E/KXN58Y/PEK587  
 [price]>PAT:A  
01 Y FARE:CNY980.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1030.00
 SFC:01    SFN:01   
  RMK CA/NWEXF0
";

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);          

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            Console.ReadLine();         
        }

        // 测试名字是否包含有pnr
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult103()
        {
            // 返回结果：
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"JAYET/ANTOINE XAVIER MARIE","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KGR8QY","FlightList":[{"FlightNo":"FM9341","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PVG","ECity":"DYG","DepTerminal":"T1","ArrTerminal":"","DepDate":"\/Date(1435318500000+0800)\/","ArrDate":"\/Date(1435326300000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"MX6JYE","Mobile":"18621174350","OfficeNo":"SHA476","AdultPnr":null,"PriceList":[{"FacePrice":1400.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1450.00}],"ResultBag":"\r\nrtKGR8QY                                                                       \r\n 1.JAYET/ANTOINE XAVIER MARIE KGR8QY                                            \r\n 2.  FM9341 Y   FR26JUN  PVGDYG HK1   1935 2145          E T1--                 \r\n 3.SHA/T SHA/T 021-55666666/SHA SHENG JIE TOUR TRADE CO.,LTD/NI QI ABCDEFG      \r\n 4.REM 0625 1525 QIANCHENG6 021-32506034                                        \r\n 5.TL/1735/26JUN/SHA476                                                         \r\n 6.SSR FOID FM HK1 NI15FV00324/P1                                               \r\n 7.SSR ADTK 1E BY SHA25JUN15/1825 OR CXL FM9341 Y26JUN                          \r\n 8.OSI FM CTCT18621174350                                                       \r\n 9.RMK CA/MX6JYE                                                                \r\n10.SHA476                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 Y FARE:CNY1400.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1450.00                   \r\n\u0010SFC:01   \u0010SFN:01\r\n\r\n"},"reqtime":"\/Date(1435232602673+0800)\/","SaveTime":1800}
            //string cmdResult =
            string cmdResult1 =
@"
rtKGR8QY                                                                       
 1.JAYET/ANTOINE XAVIER MARIE KGR8QY                                            
 2.  FM9341 Y   FR26JUN  PVGDYG HK1   1935 2145          E T1--                 
 3.SHA/T SHA/T 021-55666666/SHA SHENG JIE TOUR TRADE CO.,LTD/NI QI ABCDEFG      
 4.REM 0625 1525 QIANCHENG6 021-32506034                                        
 5.TL/1735/26JUN/SHA476                                                         
 6.SSR FOID FM HK1 NI15FV00324/P1                                               
 7.SSR ADTK 1E BY SHA25JUN15/1825 OR CXL FM9341 Y26JUN                          
 8.OSI FM CTCT18621174350                                                       
 9.RMK CA/MX6JYE                                                                
10.SHA476                                                                       
                                                                               
                                                                                
                                                                                
PAT:A                                                                          
>PAT:A                                                                          
01 Y FARE:CNY1400.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1450.00                   
SFC:01   SFN:01

";
            // 返回结果：
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"李YUE","idtype":0,"cardno":"NI620102198111104620","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992375787182"},{"name":"张蓉","idtype":0,"cardno":"NI610302198301140568","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992375787183"}],"PNR":"KF1QET","FlightList":[{"FlightNo":"CA4172","Airline":"","Cabin":"Q","SubCabin":"","SCity":"PEK","ECity":"KMG","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1434599400000+0800)\/","ArrDate":"\/Date(1434612000000+0800)\/","PNRState":"RR2"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"MJYPC2","Mobile":"63154444","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":1310.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1360.00}],"ResultBag":"\r\n  **ELECTRONIC TICKET PNR** \r\n 1.李YUE 2.张蓉 KF1QET  \r\n 3.  CA4172 Q   TH18JUN  PEKKMG RR2   1150 1520      E T3-- \r\n 4.T KMG/KMG/T0871-65162666/EAST COAST AVIATION SERVICES CO.\r\n 5.T KMG/LTD IN YUNNAN/LUO WENQIANG \r\n 6.T\r\n 7.SSR FOID CA HK1 NI610302198301140568/P2  \r\n 8.SSR FOID CA HK1 NI620102198111104620/P1  \r\n 9.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787183/1/P2\r\n10.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787182/1/P1\r\n11.SSR OTHS 1E 1 CAAIRLINES ET PNR  \r\n12.SSR OTHS 1E 1 PNR RR AND PRINTED                                            +\r\n\r\n\u001e13.SSR OTHS 1E CA BKG CXLD DUE TO TKT TIME EXPIRED                             -\r\n\r\n14.SSR TKTL CA XX/ KMG 1900/17JUN15 \r\n15.SSR FQTV CA HK1 PEKKMG 4172 Q18JUN CA008334377481/C/P2   \r\n16.OSI CA CTC 63154444  \r\n17.OSI YY CTCT63154444  \r\n18.OSI CA CTCT1388*****49   \r\n19.OSI 1E CAET TN/9992375787182-9992375787183   \r\n20.RMK B2BPLATFORM WEB IMPORT   \r\n21.FN/M/FCNY1310.00/SCNY1310.00/C6.00/XCNY50.00/TCNY50.00CN/TEXEMPTYQ/  \r\n    ACNY1360.00 \r\n22.TN/999-2375787182/P1 \r\n23.TN/999-2375787183/P2                                                        +\r\n\r\n\u001e24.FP/CC/Y1                                                                    -\r\n\r\n25.PEK1E/KF1QET/KMG168  \r\n\u001e[price]>PAT:A  \r\n01 Q FARE:CNY1310.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1360.00   \r\n\u001eSFC:01   \u001eSFN:01   \r\n\u001e[eTerm:caa01] RMK CA/MJYPC2\r\n"},"reqtime":"\/Date(1435232851841+0800)\/","SaveTime":1800}
            string cmdResult22 =
            //string cmdResult =
@"
  **ELECTRONIC TICKET PNR** 
 1.李YUE 2.张蓉 KF1QET  
 3.  CA4172 Q   TH18JUN  PEKKMG RR2   1150 1520      E T3-- 
 4.T KMG/KMG/T0871-65162666/EAST COAST AVIATION SERVICES CO.
 5.T KMG/LTD IN YUNNAN/LUO WENQIANG 
 6.T
 7.SSR FOID CA HK1 NI610302198301140568/P2  
 8.SSR FOID CA HK1 NI620102198111104620/P1  
 9.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787183/1/P2
10.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787182/1/P1
11.SSR OTHS 1E 1 CAAIRLINES ET PNR  
12.SSR OTHS 1E 1 PNR RR AND PRINTED                                            +

13.SSR OTHS 1E CA BKG CXLD DUE TO TKT TIME EXPIRED                             -

14.SSR TKTL CA XX/ KMG 1900/17JUN15 
15.SSR FQTV CA HK1 PEKKMG 4172 Q18JUN CA008334377481/C/P2   
16.OSI CA CTC 63154444  
17.OSI YY CTCT63154444  
18.OSI CA CTCT1388*****49   
19.OSI 1E CAET TN/9992375787182-9992375787183   
20.RMK B2BPLATFORM WEB IMPORT   
21.FN/M/FCNY1310.00/SCNY1310.00/C6.00/XCNY50.00/TCNY50.00CN/TEXEMPTYQ/  
    ACNY1360.00 
22.TN/999-2375787182/P1 
23.TN/999-2375787183/P2                                                        +

24.FP/CC/Y1                                                                    -

25.PEK1E/KF1QET/KMG168  
[price]>PAT:A  
01 Q FARE:CNY1310.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1360.00   
SFC:01   SFN:01   
[eTerm:caa01] RMK CA/MJYPC2
";

            // 返回结果：
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"陈振海","idtype":0,"cardno":"NI420102197410112012","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"胡丽娅","idtype":0,"cardno":"NI420700197903301620","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"阮红艳","idtype":0,"cardno":"NI420103196701160841","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"孙巍","idtype":0,"cardno":"NI420104196212271635","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"吴天文","idtype":0,"cardno":"NI420123197601011726","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"夏伟平","idtype":0,"cardno":"NI420103196312280874","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"杨本平","idtype":0,"cardno":"NI420103195505291233","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"杨玲","idtype":0,"cardno":"NI420102196804113320","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KZ06LE","FlightList":[{"FlightNo":"TV9878","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CKG","ECity":"LZY","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1435703400000+0800)\/","ArrDate":"\/Date(1435712100000+0800)\/","PNRState":"HK8"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["HGH157"],"BigPNR":"MH3TXL","Mobile":"18971552053","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":1510.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1560.00}],"ResultBag":"\r\n 1.陈振海 2.胡丽娅 3.阮红艳 4.孙巍 5.吴天文 6.夏伟平                                            \r\n 7.杨本平 8.杨玲 KZ06LE                                                             \r\n 9.  TV9878 Y   WE01JUL  CKGLZY HK8   0630 0855          E T2--                \r\n10.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     \r\n     ABCDEFG                                                                   \r\n11.TL/1613/05JUN/SHA888                                                        \r\n12.SSR FOID TV HK1 NI420102196804113320/P8                                     \r\n13.SSR FOID TV HK1 NI420103195505291233/P7                                     \r\n14.SSR FOID TV HK1 NI420103196312280874/P6                                     \r\n15.SSR FOID TV HK1 NI420123197601011726/P5                                     \r\n16.SSR FOID TV HK1 NI420104196212271635/P4                                     \r\n17.SSR FOID TV HK1 NI420103196701160841/P3                                     +\r\n\r\n18.SSR FOID TV HK1 NI420700197903301620/P2                                     -\r\n19.SSR FOID TV HK1 NI420102197410112012/P1                                     \r\n20.SSR ADTK 1E BY SHA05JUN15/1613 OR CXL TV9878 Y01JUL                         \r\n21.OSI TV CTCT18971552053                                                      \r\n22.RMK TJ AUTH HGH157                                                          \r\n23.RMK CA/MH3TXL                                                               \r\n24.SHA243                                                                      \r\n\r\n>PAT:A                                                                         \r\n01 Y FARE:CNY1510.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1560.00                  \r\n?SFC:01   ?SFN:01\r\n"},"reqtime":"\/Date(1435232960572+0800)\/","SaveTime":1800}
            //string cmdResult3 =
            string cmdResult =
@"
 1.陈振海 2.胡丽娅 3.阮红艳 4.孙巍 5.吴天文 6.夏伟平                                            
 7.杨本平 8.杨玲 KZ06LE                                                             
 9.  TV9878 Y   WE01JUL  CKGLZY HK8   0630 0855          E T2--                
10.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     
     ABCDEFG                                                                   
11.TL/1613/05JUN/SHA888                                                        
12.SSR FOID TV HK1 NI420102196804113320/P8                                     
13.SSR FOID TV HK1 NI420103195505291233/P7                                     
14.SSR FOID TV HK1 NI420103196312280874/P6                                     
15.SSR FOID TV HK1 NI420123197601011726/P5                                     
16.SSR FOID TV HK1 NI420104196212271635/P4                                     
17.SSR FOID TV HK1 NI420103196701160841/P3                                     +

18.SSR FOID TV HK1 NI420700197903301620/P2                                     -
19.SSR FOID TV HK1 NI420102197410112012/P1                                     
20.SSR ADTK 1E BY SHA05JUN15/1613 OR CXL TV9878 Y01JUL                         
21.OSI TV CTCT18971552053                                                      
22.RMK TJ AUTH HGH157                                                          
23.RMK CA/MH3TXL                                                               
24.SHA243                                                                      

>PAT:A                                                                         
01 Y FARE:CNY1510.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1560.00                  
?SFC:01   ?SFN:01
";

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            Console.ReadLine();
        }

        // 测试单程票的主、子舱价格的问题
        // 测试价格不出来的问题
        // 测试时间出现+1的情况   
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult2()
        {
            // 单程票--有子舱：
            // 返回结果：
            // {"PassengerList":[{"name":"陈晓庆","idtype":0,"cardno":"NI320911198911224646","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNRN19","FlightList":[{"FlightNo":"CA1389","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"HGH","ECity":"SYX","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432093200000+0800)\/","ArrDate":"\/Date(1432104300000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"PCH4YT","Mobile":"18917588289","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":760.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":810.00}],"ResultBag":"\r\nRTKNRN19                                                                       \r\n 1.陈晓庆 KNRN19                                                                \r\n 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E      V1              \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 4.REM 0518 1334 JK002                                                          \r\n 5.TL/0000/18MAY/SHA243                                                         \r\n 6.SSR FOID CA HK1 NI320911198911224646/P1                                      \r\n 7.SSR ADTK 1E BY SHA18MAY15/1434 OR CXL CA ALL SEGS                            \r\n 8.OSI CA CTCT18917588289                                                       \r\n 9.RMK CA/PCH4YT                                                                \r\n10.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02 \r\n"}
            // {"PassengerList":[{"name":"陈晓庆","idtype":0,"cardno":"NI320911198911224646","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNRN19","FlightList":[{"FlightNo":"CA1389","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"HGH","ECity":"SYX","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432093200000+0800)\/","ArrDate":"\/Date(1432104300000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"PCH4YT","Mobile":"18917588289","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":760.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":810.00}],"ResultBag":"\r\nRTKNRN19                                                                       \r\n 1.陈晓庆 KNRN19                                                                \r\n 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E      V1              \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 4.REM 0518 1334 JK002                                                          \r\n 5.TL/0000/18MAY/SHA243                                                         \r\n 6.SSR FOID CA HK1 NI320911198911224646/P1                                      \r\n 7.SSR ADTK 1E BY SHA18MAY15/1434 OR CXL CA ALL SEGS                            \r\n 8.OSI CA CTCT18917588289                                                       \r\n 9.RMK CA/PCH4YT                                                                \r\n10.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02 \r\n"}
            // finished tested
            // {"PassengerList":[{"name":"陈晓庆","idtype":0,"cardno":"NI320911198911224646","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNRN19","FlightList":[{"FlightNo":"CA1389","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"HGH","ECity":"SYX","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432093200000+0800)\/","ArrDate":"\/Date(1432104300000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"PCH4YT","Mobile":"18917588289","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":760.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":810.00}],"ResultBag":"\r\nRTKNRN19                                                                       \r\n 1.陈晓庆 KNRN19                                                                \r\n 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E      V1              \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 4.REM 0518 1334 JK002                                                          \r\n 5.TL/0000/18MAY/SHA243                                                         \r\n 6.SSR FOID CA HK1 NI320911198911224646/P1                                      \r\n 7.SSR ADTK 1E BY SHA18MAY15/1434 OR CXL CA ALL SEGS                            \r\n 8.OSI CA CTCT18917588289                                                       \r\n 9.RMK CA/PCH4YT                                                                \r\n10.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02 \r\n"}
            // {"PassengerList":[{"name":"陈晓庆","idtype":0,"cardno":"NI320911198911224646","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNRN19","FlightList":[{"FlightNo":"CA1389","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"HGH","ECity":"SYX","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432093200000+0800)\/","ArrDate":"\/Date(1432104300000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"PCH4YT","Mobile":"18917588289","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":760.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":810.00,"Tag":"V1"}],"ResultBag":"\r\nRTKNRN19                                                                       \r\n 1.陈晓庆 KNRN19                                                                \r\n 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E      V1              \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 4.REM 0518 1334 JK002                                                          \r\n 5.TL/0000/18MAY/SHA243                                                         \r\n 6.SSR FOID CA HK1 NI320911198911224646/P1                                      \r\n 7.SSR ADTK 1E BY SHA18MAY15/1434 OR CXL CA ALL SEGS                            \r\n 8.OSI CA CTCT18917588289                                                       \r\n 9.RMK CA/PCH4YT                                                                \r\n10.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02 \r\n"}
            //string cmdResult =
            string cmdResult1 =
@"
RTKNRN19                                                                       
 1.陈晓庆 KNRN19                                                                
 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E      V1              
 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      
     ABCDEFG                                                                    
 4.REM 0518 1334 JK002                                                          
 5.TL/0000/18MAY/SHA243                                                         
 6.SSR FOID CA HK1 NI320911198911224646/P1                                      
 7.SSR ADTK 1E BY SHA18MAY15/1434 OR CXL CA ALL SEGS                            
 8.OSI CA CTCT18917588289                                                       
 9.RMK CA/PCH4YT                                                                
10.SHA243                                                                       
                                                                                
                                                                                
                                                                                
 PAT:A                                                                          
>PAT:A                                                                          
01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    
 SFC:01    SFN:01                                                               
02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     
 SFC:02    SFN:02 
";

            // 单程票--没有子舱（没找到测试案例，自己去除了V1）：
            // 返回结果：
            // {"PassengerList":[{"name":"陈晓庆","idtype":0,"cardno":"NI320911198911224646","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNRN19","FlightList":[{"FlightNo":"CA1389","Airline":"","Cabin":"V","SubCabin":"","SCity":"HGH","ECity":"SYX","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432093200000+0800)\/","ArrDate":"\/Date(1432104300000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"PCH4YT","Mobile":"18917588289","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":860.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":910.00}],"ResultBag":"\r\nRTKNRN19                                                                       \r\n 1.陈晓庆 KNRN19                                                                \r\n 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E                    \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 4.REM 0518 1334 JK002                                                          \r\n 5.TL/0000/18MAY/SHA243                                                         \r\n 6.SSR FOID CA HK1 NI320911198911224646/P1                                      \r\n 7.SSR ADTK 1E BY SHA18MAY15/1434 OR CXL CA ALL SEGS                            \r\n 8.OSI CA CTCT18917588289                                                       \r\n 9.RMK CA/PCH4YT                                                                \r\n10.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02 \r\n"}
            // {"PassengerList":[{"name":"陈晓庆","idtype":0,"cardno":"NI320911198911224646","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNRN19","FlightList":[{"FlightNo":"CA1389","Airline":"","Cabin":"V","SubCabin":"","SCity":"HGH","ECity":"SYX","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432093200000+0800)\/","ArrDate":"\/Date(1432104300000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"PCH4YT","Mobile":"18917588289","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":860.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":910.00}],"ResultBag":"\r\nRTKNRN19                                                                       \r\n 1.陈晓庆 KNRN19                                                                \r\n 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E                    \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 4.REM 0518 1334 JK002                                                          \r\n 5.TL/0000/18MAY/SHA243                                                         \r\n 6.SSR FOID CA HK1 NI320911198911224646/P1                                      \r\n 7.SSR ADTK 1E BY SHA18MAY15/1434 OR CXL CA ALL SEGS                            \r\n 8.OSI CA CTCT18917588289                                                       \r\n 9.RMK CA/PCH4YT                                                                \r\n10.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02 \r\n"}
            // finished tested
            // {"PassengerList":[{"name":"陈晓庆","idtype":0,"cardno":"NI320911198911224646","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNRN19","FlightList":[{"FlightNo":"CA1389","Airline":"","Cabin":"V","SubCabin":"","SCity":"HGH","ECity":"SYX","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432093200000+0800)\/","ArrDate":"\/Date(1432104300000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"PCH4YT","Mobile":"18917588289","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":760.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":810.00},{"FacePrice":860.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":910.00}],"ResultBag":"\r\nRTKNRN19                                                                       \r\n 1.陈晓庆 KNRN19                                                                \r\n 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E                    \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 4.REM 0518 1334 JK002                                                          \r\n 5.TL/0000/18MAY/SHA243                                                         \r\n 6.SSR FOID CA HK1 NI320911198911224646/P1                                      \r\n 7.SSR ADTK 1E BY SHA18MAY15/1434 OR CXL CA ALL SEGS                            \r\n 8.OSI CA CTCT18917588289                                                       \r\n 9.RMK CA/PCH4YT                                                                \r\n10.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02 \r\n"}
            // {"PassengerList":[{"name":"陈晓庆","idtype":0,"cardno":"NI320911198911224646","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNRN19","FlightList":[{"FlightNo":"CA1389","Airline":"","Cabin":"V","SubCabin":"","SCity":"HGH","ECity":"SYX","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432093200000+0800)\/","ArrDate":"\/Date(1432104300000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"PCH4YT","Mobile":"18917588289","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":760.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":810.00,"Tag":"V1"},{"FacePrice":860.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":910.00,"Tag":"V"}],"ResultBag":"\r\nRTKNRN19                                                                       \r\n 1.陈晓庆 KNRN19                                                                \r\n 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E                    \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 4.REM 0518 1334 JK002                                                          \r\n 5.TL/0000/18MAY/SHA243                                                         \r\n 6.SSR FOID CA HK1 NI320911198911224646/P1                                      \r\n 7.SSR ADTK 1E BY SHA18MAY15/1434 OR CXL CA ALL SEGS                            \r\n 8.OSI CA CTCT18917588289                                                       \r\n 9.RMK CA/PCH4YT                                                                \r\n10.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02 \r\n"}
            string cmdResult22 =
                //string cmdResult =
@"
RTKNRN19                                                                       
 1.陈晓庆 KNRN19                                                                
 2.  CA1389 V   WE20MAY  HGHSYX HK1   1140 1445          E                    
 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      
     ABCDEFG                                                                    
 4.REM 0518 1334 JK002                                                          
 5.TL/0000/18MAY/SHA243                                                         
 6.SSR FOID CA HK1 NI320911198911224646/P1                                      
 7.SSR ADTK 1E BY SHA18MAY15/1434 OR CXL CA ALL SEGS                            
 8.OSI CA CTCT18917588289                                                       
 9.RMK CA/PCH4YT                                                                
10.SHA243                                                                       
                                                                                
                                                                                
                                                                                
 PAT:A                                                                          
>PAT:A                                                                          
01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    
 SFC:01    SFN:01                                                               
02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     
 SFC:02    SFN:02 
";

            // 往返票--有子舱
            // 测试+1的情况：
            // 返回结果：
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KS7N0B","FlightList":[{"FlightNo":"CA1831","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1434583800000+0800)\/","ArrDate":"\/Date(1434591600000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1886","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434722100000+0800)\/","ArrDate":"\/Date(1434730500000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"","Mobile":"13127584465","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":2100.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2200.00},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00},{"FacePrice":2180.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2280.00}],"ResultBag":"\r\nrtKS7N0B                                                                       \r\n 1.ZI/XING KS7N0B                                                               \r\n 2.  CA1831 M   TH18JUN  PEKSHA HK1   0730 0940          E T3T2 M1              \r\n 3.  CA1886 M   FR19JUN  SHAPEK HK1   2155 0015+1        E T2T3 M1              \r\n 4.TPE/T TPE/T0285692294/HUNG DA TRAVEL SERVICE CO LTD/SUN YUNG HUNG ABCDEFG    \r\n 5.REM 0519 0959 ZIXIN                                                          \r\n 6.13127584465                                                                  \r\n 7.TL/1058/19MAY/SHA888                                                         \r\n 8.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n 9.OSI YY CTCT13127584465                                                       \r\n10.OSI CA CTCT02151812332                                                       \r\n11.RMK TJ AUTH SHA255                                                           \r\n12.RMK TJ AUTH SHA836                                                          +\r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 M1+M1 FARE:CNY2100.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2200.00              \r\n SFC:01    SFN:01/01    SFN:01/02                                               \r\n02 M+M1 FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:02    SFN:02/01    SFN:02/02                                               \r\n03 M1+M FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:03    SFN:03/01    SFN:03/02                                               \r\n04 M+M FARE:CNY2180.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2280.00                \r\n SFC:04    SFN:04/01    SFN:04/02\u0010\r\n"}
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KS7N0B","FlightList":[{"FlightNo":"CA1831","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1434583800000+0800)\/","ArrDate":"\/Date(1434591600000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1886","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434722100000+0800)\/","ArrDate":"\/Date(1434730500000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"","Mobile":"13127584465","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":2100.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2200.00},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00},{"FacePrice":2180.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2280.00}],"ResultBag":"\r\nrtKS7N0B                                                                       \r\n 1.ZI/XING KS7N0B                                                               \r\n 2.  CA1831 M   TH18JUN  PEKSHA HK1   0730 0940          E T3T2 M1              \r\n 3.  CA1886 M   FR19JUN  SHAPEK HK1   2155 0015+1        E T2T3 M1              \r\n 4.TPE/T TPE/T0285692294/HUNG DA TRAVEL SERVICE CO LTD/SUN YUNG HUNG ABCDEFG    \r\n 5.REM 0519 0959 ZIXIN                                                          \r\n 6.13127584465                                                                  \r\n 7.TL/1058/19MAY/SHA888                                                         \r\n 8.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n 9.OSI YY CTCT13127584465                                                       \r\n10.OSI CA CTCT02151812332                                                       \r\n11.RMK TJ AUTH SHA255                                                           \r\n12.RMK TJ AUTH SHA836                                                          +\r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 M1+M1 FARE:CNY2100.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2200.00              \r\n SFC:01    SFN:01/01    SFN:01/02                                               \r\n02 M+M1 FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:02    SFN:02/01    SFN:02/02                                               \r\n03 M1+M FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:03    SFN:03/01    SFN:03/02                                               \r\n04 M+M FARE:CNY2180.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2280.00                \r\n SFC:04    SFN:04/01    SFN:04/02\u0010\r\n"}
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KS7N0B","FlightList":[{"FlightNo":"CA1831","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1434583800000+0800)\/","ArrDate":"\/Date(1434591600000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1886","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434722100000+0800)\/","ArrDate":"\/Date(1434730500000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"","Mobile":"13127584465","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":2100.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2200.00,"Tag":"M1+M1"},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00,"Tag":"M+M1"},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00,"Tag":"M1+M"},{"FacePrice":2180.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2280.00,"Tag":"M+M"}],"ResultBag":"\r\nrtKS7N0B                                                                       \r\n 1.ZI/XING KS7N0B                                                               \r\n 2.  CA1831 M   TH18JUN  PEKSHA HK1   0730 0940          E T3T2 M1              \r\n 3.  CA1886 M   FR19JUN  SHAPEK HK1   2155 0015+1        E T2T3 M1              \r\n 4.TPE/T TPE/T0285692294/HUNG DA TRAVEL SERVICE CO LTD/SUN YUNG HUNG ABCDEFG    \r\n 5.REM 0519 0959 ZIXIN                                                          \r\n 6.13127584465                                                                  \r\n 7.TL/1058/19MAY/SHA888                                                         \r\n 8.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n 9.OSI YY CTCT13127584465                                                       \r\n10.OSI CA CTCT02151812332                                                       \r\n11.RMK TJ AUTH SHA255                                                           \r\n12.RMK TJ AUTH SHA836                                                          +\r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 M1+M1 FARE:CNY2100.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2200.00              \r\n SFC:01    SFN:01/01    SFN:01/02                                               \r\n02 M+M1 FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:02    SFN:02/01    SFN:02/02                                               \r\n03 M1+M FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:03    SFN:03/01    SFN:03/02                                               \r\n04 M+M FARE:CNY2180.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2280.00                \r\n SFC:04    SFN:04/01    SFN:04/02\u0010\r\n"}
            string cmdResult3 =
                //string cmdResult =
@"
rtKS7N0B                                                                       
 1.ZI/XING KS7N0B                                                               
 2.  CA1831 M   TH18JUN  PEKSHA HK1   0730 0940          E T3T2 M1              
 3.  CA1886 M   FR19JUN  SHAPEK HK1   2155 0015+1        E T2T3 M1              
 4.TPE/T TPE/T0285692294/HUNG DA TRAVEL SERVICE CO LTD/SUN YUNG HUNG ABCDEFG    
 5.REM 0519 0959 ZIXIN                                                          
 6.13127584465                                                                  
 7.TL/1058/19MAY/SHA888                                                         
 8.SSR FOID CA HK1 NI320681198610238019/P1                                      
 9.OSI YY CTCT13127584465                                                       
10.OSI CA CTCT02151812332                                                       
11.RMK TJ AUTH SHA255                                                           
12.RMK TJ AUTH SHA836                                                          +
                                                                                
                                                                                
                                                                                
 pat:a                                                                          
>PAT:A                                                                          
01 M1+M1 FARE:CNY2100.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2200.00              
 SFC:01    SFN:01/01    SFN:01/02                                               
02 M+M1 FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               
 SFC:02    SFN:02/01    SFN:02/02                                               
03 M1+M FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               
 SFC:03    SFN:03/01    SFN:03/02                                               
04 M+M FARE:CNY2180.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2280.00                
 SFC:04    SFN:04/01    SFN:04/02
";

            // 往返票--没有子舱（没找到测试案例，自己去除了两个M1）
            // 返回结果：
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KS7N0B","FlightList":[{"FlightNo":"CA1831","Airline":"","Cabin":"M","SubCabin":"","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1434583800000+0800)\/","ArrDate":"\/Date(1434591600000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1886","Airline":"","Cabin":"M","SubCabin":"","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434722100000+0800)\/","ArrDate":"\/Date(1434730500000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"","Mobile":"13127584465","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":2100.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2200.00},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00},{"FacePrice":2180.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2280.00}],"ResultBag":"\r\nrtKS7N0B                                                                       \r\n 1.ZI/XING KS7N0B                                                               \r\n 2.  CA1831 M   TH18JUN  PEKSHA HK1   0730 0940          E T3T2               \r\n 3.  CA1886 M   FR19JUN  SHAPEK HK1   2155 0015+1        E T2T3               \r\n 4.TPE/T TPE/T0285692294/HUNG DA TRAVEL SERVICE CO LTD/SUN YUNG HUNG ABCDEFG    \r\n 5.REM 0519 0959 ZIXIN                                                          \r\n 6.13127584465                                                                  \r\n 7.TL/1058/19MAY/SHA888                                                         \r\n 8.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n 9.OSI YY CTCT13127584465                                                       \r\n10.OSI CA CTCT02151812332                                                       \r\n11.RMK TJ AUTH SHA255                                                           \r\n12.RMK TJ AUTH SHA836                                                          +\r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 M1+M1 FARE:CNY2100.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2200.00              \r\n SFC:01    SFN:01/01    SFN:01/02                                               \r\n02 M+M1 FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:02    SFN:02/01    SFN:02/02                                               \r\n03 M1+M FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:03    SFN:03/01    SFN:03/02                                               \r\n04 M+M FARE:CNY2180.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2280.00                \r\n SFC:04    SFN:04/01    SFN:04/02\u0010\r\n"}
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KS7N0B","FlightList":[{"FlightNo":"CA1831","Airline":"","Cabin":"M","SubCabin":"","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1434583800000+0800)\/","ArrDate":"\/Date(1434591600000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1886","Airline":"","Cabin":"M","SubCabin":"","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434722100000+0800)\/","ArrDate":"\/Date(1434730500000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"","Mobile":"13127584465","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":2100.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2200.00},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00},{"FacePrice":2180.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2280.00}],"ResultBag":"\r\nrtKS7N0B                                                                       \r\n 1.ZI/XING KS7N0B                                                               \r\n 2.  CA1831 M   TH18JUN  PEKSHA HK1   0730 0940          E T3T2               \r\n 3.  CA1886 M   FR19JUN  SHAPEK HK1   2155 0015+1        E T2T3               \r\n 4.TPE/T TPE/T0285692294/HUNG DA TRAVEL SERVICE CO LTD/SUN YUNG HUNG ABCDEFG    \r\n 5.REM 0519 0959 ZIXIN                                                          \r\n 6.13127584465                                                                  \r\n 7.TL/1058/19MAY/SHA888                                                         \r\n 8.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n 9.OSI YY CTCT13127584465                                                       \r\n10.OSI CA CTCT02151812332                                                       \r\n11.RMK TJ AUTH SHA255                                                           \r\n12.RMK TJ AUTH SHA836                                                          +\r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 M1+M1 FARE:CNY2100.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2200.00              \r\n SFC:01    SFN:01/01    SFN:01/02                                               \r\n02 M+M1 FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:02    SFN:02/01    SFN:02/02                                               \r\n03 M1+M FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:03    SFN:03/01    SFN:03/02                                               \r\n04 M+M FARE:CNY2180.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2280.00                \r\n SFC:04    SFN:04/01    SFN:04/02\u0010\r\n"}
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KS7N0B","FlightList":[{"FlightNo":"CA1831","Airline":"","Cabin":"M","SubCabin":"","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1434583800000+0800)\/","ArrDate":"\/Date(1434591600000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1886","Airline":"","Cabin":"M","SubCabin":"","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434722100000+0800)\/","ArrDate":"\/Date(1434730500000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"","Mobile":"13127584465","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":2100.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2200.00,"Tag":"M1+M1"},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00,"Tag":"M+M1"},{"FacePrice":2140.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2240.00,"Tag":"M1+M"},{"FacePrice":2180.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2280.00,"Tag":"M+M"}],"ResultBag":"\r\nrtKS7N0B                                                                       \r\n 1.ZI/XING KS7N0B                                                               \r\n 2.  CA1831 M   TH18JUN  PEKSHA HK1   0730 0940          E T3T2               \r\n 3.  CA1886 M   FR19JUN  SHAPEK HK1   2155 0015+1        E T2T3               \r\n 4.TPE/T TPE/T0285692294/HUNG DA TRAVEL SERVICE CO LTD/SUN YUNG HUNG ABCDEFG    \r\n 5.REM 0519 0959 ZIXIN                                                          \r\n 6.13127584465                                                                  \r\n 7.TL/1058/19MAY/SHA888                                                         \r\n 8.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n 9.OSI YY CTCT13127584465                                                       \r\n10.OSI CA CTCT02151812332                                                       \r\n11.RMK TJ AUTH SHA255                                                           \r\n12.RMK TJ AUTH SHA836                                                          +\r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 M1+M1 FARE:CNY2100.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2200.00              \r\n SFC:01    SFN:01/01    SFN:01/02                                               \r\n02 M+M1 FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:02    SFN:02/01    SFN:02/02                                               \r\n03 M1+M FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               \r\n SFC:03    SFN:03/01    SFN:03/02                                               \r\n04 M+M FARE:CNY2180.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2280.00                \r\n SFC:04    SFN:04/01    SFN:04/02\u0010\r\n"}
            string cmdResult4 =
                //string cmdResult =
@"
rtKS7N0B                                                                       
 1.ZI/XING KS7N0B                                                               
 2.  CA1831 M   TH18JUN  PEKSHA HK1   0730 0940          E T3T2               
 3.  CA1886 M   FR19JUN  SHAPEK HK1   2155 0015+1        E T2T3               
 4.TPE/T TPE/T0285692294/HUNG DA TRAVEL SERVICE CO LTD/SUN YUNG HUNG ABCDEFG    
 5.REM 0519 0959 ZIXIN                                                          
 6.13127584465                                                                  
 7.TL/1058/19MAY/SHA888                                                         
 8.SSR FOID CA HK1 NI320681198610238019/P1                                      
 9.OSI YY CTCT13127584465                                                       
10.OSI CA CTCT02151812332                                                       
11.RMK TJ AUTH SHA255                                                           
12.RMK TJ AUTH SHA836                                                          +
                                                                                
                                                                                
                                                                                
 pat:a                                                                          
>PAT:A                                                                          
01 M1+M1 FARE:CNY2100.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2200.00              
 SFC:01    SFN:01/01    SFN:01/02                                               
02 M+M1 FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               
 SFC:02    SFN:02/01    SFN:02/02                                               
03 M1+M FARE:CNY2140.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2240.00               
 SFC:03    SFN:03/01    SFN:03/02                                               
04 M+M FARE:CNY2180.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2280.00                
 SFC:04    SFN:04/01    SFN:04/02
";

            // 联程票--有子舱
            // 返回结果：
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KPFEM8","FlightList":[{"FlightNo":"CA1858","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1432338900000+0800)\/","ArrDate":"\/Date(1432347300000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1639","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"PEK","ECity":"HRB","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1432642200000+0800)\/","ArrDate":"\/Date(1432649400000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":2,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"MG2Y65","Mobile":"13127584465","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":1940.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2040.00},{"FacePrice":1970.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2070.00},{"FacePrice":1980.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2080.00},{"FacePrice":2010.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2110.00}],"ResultBag":"\r\nrtKPFEM8                                                                       \r\n 1.ZI/XING KPFEM8                                                               \r\n 2.  CA1858 M   SA23MAY  SHAPEK HK1   0755 1015          E T2T3 M1              \r\n 3.  CA1639 M   TU26MAY  PEKHRB HK1   2010 2210          E T3-- M1              \r\n 4.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 5.REM 0519 1037 ZIXIN                                                          \r\n 6.13127584465                                                                  \r\n 7.TL/1058/19MAY/SHA888                                                         \r\n 8.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n 9.OSI YY CTCT13127584465                                                       \r\n10.OSI CA CTCT02151812332                                                       \r\n11.RMK TJ AUTH SHA255                                                          +\r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pn                                                                             \r\n12.RMK TJ AUTH SHA836                                                          -\r\n13.RMK CA/MG2Y65                                                                \r\n14.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 M1+M1 FARE:CNY1940.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2040.00              \r\n SFC:01    SFN:01/01    SFN:01/02                                               \r\n02 M1+M FARE:CNY1970.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2070.00               \r\n SFC:02    SFN:02/01    SFN:02/02                                               \r\n03 M+M1 FARE:CNY1980.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2080.00               \r\n SFC:03    SFN:03/01    SFN:03/02                                               \r\n04 M+M FARE:CNY2010.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2110.00                \r\n SFC:04    SFN:04/01    SFN:04/02\r\n"}
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KPFEM8","FlightList":[{"FlightNo":"CA1858","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1432338900000+0800)\/","ArrDate":"\/Date(1432347300000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1639","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"PEK","ECity":"HRB","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1432642200000+0800)\/","ArrDate":"\/Date(1432649400000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":2,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"MG2Y65","Mobile":"13127584465","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":1940.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2040.00},{"FacePrice":1970.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2070.00},{"FacePrice":1980.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2080.00},{"FacePrice":2010.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2110.00}],"ResultBag":"\r\nrtKPFEM8                                                                       \r\n 1.ZI/XING KPFEM8                                                               \r\n 2.  CA1858 M   SA23MAY  SHAPEK HK1   0755 1015          E T2T3 M1              \r\n 3.  CA1639 M   TU26MAY  PEKHRB HK1   2010 2210          E T3-- M1              \r\n 4.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 5.REM 0519 1037 ZIXIN                                                          \r\n 6.13127584465                                                                  \r\n 7.TL/1058/19MAY/SHA888                                                         \r\n 8.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n 9.OSI YY CTCT13127584465                                                       \r\n10.OSI CA CTCT02151812332                                                       \r\n11.RMK TJ AUTH SHA255                                                          +\r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pn                                                                             \r\n12.RMK TJ AUTH SHA836                                                          -\r\n13.RMK CA/MG2Y65                                                                \r\n14.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 M1+M1 FARE:CNY1940.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2040.00              \r\n SFC:01    SFN:01/01    SFN:01/02                                               \r\n02 M1+M FARE:CNY1970.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2070.00               \r\n SFC:02    SFN:02/01    SFN:02/02                                               \r\n03 M+M1 FARE:CNY1980.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2080.00               \r\n SFC:03    SFN:03/01    SFN:03/02                                               \r\n04 M+M FARE:CNY2010.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2110.00                \r\n SFC:04    SFN:04/01    SFN:04/02\r\n"}
            // finished tested:
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KPFEM8","FlightList":[{"FlightNo":"CA1858","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1432338900000+0800)\/","ArrDate":"\/Date(1432347300000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1639","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"PEK","ECity":"HRB","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1432642200000+0800)\/","ArrDate":"\/Date(1432649400000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":2,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"MG2Y65","Mobile":"13127584465","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":1940.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2040.00},{"FacePrice":1970.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2070.00},{"FacePrice":1980.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2080.00},{"FacePrice":2010.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2110.00}],"ResultBag":"\r\nrtKPFEM8                                                                       \r\n 1.ZI/XING KPFEM8                                                               \r\n 2.  CA1858 M   SA23MAY  SHAPEK HK1   0755 1015          E T2T3 M1              \r\n 3.  CA1639 M   TU26MAY  PEKHRB HK1   2010 2210          E T3-- M1              \r\n 4.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 5.REM 0519 1037 ZIXIN                                                          \r\n 6.13127584465                                                                  \r\n 7.TL/1058/19MAY/SHA888                                                         \r\n 8.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n 9.OSI YY CTCT13127584465                                                       \r\n10.OSI CA CTCT02151812332                                                       \r\n11.RMK TJ AUTH SHA255                                                          +\r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pn                                                                             \r\n12.RMK TJ AUTH SHA836                                                          -\r\n13.RMK CA/MG2Y65                                                                \r\n14.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 M1+M1 FARE:CNY1940.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2040.00              \r\n SFC:01    SFN:01/01    SFN:01/02                                               \r\n02 M1+M FARE:CNY1970.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2070.00               \r\n SFC:02    SFN:02/01    SFN:02/02                                               \r\n03 M+M1 FARE:CNY1980.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2080.00               \r\n SFC:03    SFN:03/01    SFN:03/02                                               \r\n04 M+M FARE:CNY2010.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2110.00                \r\n SFC:04    SFN:04/01    SFN:04/02\r\n"}
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KPFEM8","FlightList":[{"FlightNo":"CA1858","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1432338900000+0800)\/","ArrDate":"\/Date(1432347300000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1639","Airline":"","Cabin":"M1","SubCabin":"M1","SCity":"PEK","ECity":"HRB","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1432642200000+0800)\/","ArrDate":"\/Date(1432649400000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":2,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"MG2Y65","Mobile":"13127584465","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":1940.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2040.00,"Tag":"M1+M1"},{"FacePrice":1970.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2070.00,"Tag":"M1+M"},{"FacePrice":1980.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2080.00,"Tag":"M+M1"},{"FacePrice":2010.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2110.00,"Tag":"M+M"}],"ResultBag":"\r\nrtKPFEM8                                                                       \r\n 1.ZI/XING KPFEM8                                                               \r\n 2.  CA1858 M   SA23MAY  SHAPEK HK1   0755 1015          E T2T3 M1              \r\n 3.  CA1639 M   TU26MAY  PEKHRB HK1   2010 2210          E T3-- M1              \r\n 4.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      \r\n     ABCDEFG                                                                    \r\n 5.REM 0519 1037 ZIXIN                                                          \r\n 6.13127584465                                                                  \r\n 7.TL/1058/19MAY/SHA888                                                         \r\n 8.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n 9.OSI YY CTCT13127584465                                                       \r\n10.OSI CA CTCT02151812332                                                       \r\n11.RMK TJ AUTH SHA255                                                          +\r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pn                                                                             \r\n12.RMK TJ AUTH SHA836                                                          -\r\n13.RMK CA/MG2Y65                                                                \r\n14.SHA243                                                                       \r\n                                                                                \r\n                                                                                \r\n                                                                                \r\n pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 M1+M1 FARE:CNY1940.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2040.00              \r\n SFC:01    SFN:01/01    SFN:01/02                                               \r\n02 M1+M FARE:CNY1970.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2070.00               \r\n SFC:02    SFN:02/01    SFN:02/02                                               \r\n03 M+M1 FARE:CNY1980.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2080.00               \r\n SFC:03    SFN:03/01    SFN:03/02                                               \r\n04 M+M FARE:CNY2010.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2110.00                \r\n SFC:04    SFN:04/01    SFN:04/02\r\n"}
            string cmdResult5 =
                //string cmdResult =
@"
rtKPFEM8                                                                       
 1.ZI/XING KPFEM8                                                               
 2.  CA1858 M   SA23MAY  SHAPEK HK1   0755 1015          E T2T3 M1              
 3.  CA1639 M   TU26MAY  PEKHRB HK1   2010 2210          E T3-- M1              
 4.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG      
     ABCDEFG                                                                    
 5.REM 0519 1037 ZIXIN                                                          
 6.13127584465                                                                  
 7.TL/1058/19MAY/SHA888                                                         
 8.SSR FOID CA HK1 NI320681198610238019/P1                                      
 9.OSI YY CTCT13127584465                                                       
10.OSI CA CTCT02151812332                                                       
11.RMK TJ AUTH SHA255                                                          +
                                                                                
                                                                                
                                                                                
 pn                                                                             
12.RMK TJ AUTH SHA836                                                          -
13.RMK CA/MG2Y65                                                                
14.SHA243                                                                       
                                                                                
                                                                                
                                                                                
 pat:a                                                                          
>PAT:A                                                                          
01 M1+M1 FARE:CNY1940.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2040.00              
 SFC:01    SFN:01/01    SFN:01/02                                               
02 M1+M FARE:CNY1970.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2070.00               
 SFC:02    SFN:02/01    SFN:02/02                                               
03 M+M1 FARE:CNY1980.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2080.00               
 SFC:03    SFN:03/01    SFN:03/02                                               
04 M+M FARE:CNY2010.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2110.00                
 SFC:04    SFN:04/01    SFN:04/02
";

            // 联程票--没有子舱
            // 返回结果：
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KW1F70","FlightList":[{"FlightNo":"CA1858","Airline":"","Cabin":"Y","SubCabin":"","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1432598100000+0800)\/","ArrDate":"\/Date(1432606500000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1689","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"HRB","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1432770300000+0800)\/","ArrDate":"\/Date(1432777800000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1678","Airline":"","Cabin":"Y","SubCabin":"","SCity":"HRB","ECity":"TSN","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432856700000+0800)\/","ArrDate":"\/Date(1432864800000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":2,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"MBSLFM","Mobile":"13127584465","OfficeNo":"TPE567","AdultPnr":null,"PriceList":[{"FacePrice":3370.00,"Tax":150.00,"Fuel":0.0,"TotalPrice":3520.00}],"ResultBag":"\r\nrtKW1F70                                                                       \r\n 1.ZI/XING KW1F70                                                               \r\n 2.  CA1858 Y   TU26MAY  SHAPEK HK1   0755 1015          E T2T3                 \r\n 3.  CA1689 Y   TH28MAY  PEKHRB HK1   0745 0950          E T3--                 \r\n 4.  CA1678 Y   FR29MAY  HRBTSN HK1   0745 1000          E                      \r\n 5.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       \r\n 6.REM 0525 1924 ZIXIN                                                          \r\n 7.13127584465                                                                  \r\n 8.TL/1058/25MAY/SHA888                                                         \r\n 9.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n10.SSR ADTK 1E BY TPE25MAY15/2124 OR CXL CA ALL SEGS                            \r\n11.OSI YY CTCT13127584465                                                       \r\n12.OSI CA CTCT02151812332                                                      +\r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pn                                                                             \r\n13.RMK TJ AUTH SHA255                                                          -\r\n14.RMK TJ AUTH SHA836                                                           \r\n15.RMK CA/MBSLFM                                                                \r\n16.TPE567                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 Y+Y+Y FARE:CNY3370.00 TAX:CNY150.00 YQ:TEXEMPTYQ  TOTAL:3520.00              \r\n\u0010SFC:01   \u0010SFN:01/01   \u0010SFN:01/02   \u0010SFN:01/03\r\n"}
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KW1F70","FlightList":[{"FlightNo":"CA1858","Airline":"","Cabin":"Y","SubCabin":"","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1432598100000+0800)\/","ArrDate":"\/Date(1432606500000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1689","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"HRB","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1432770300000+0800)\/","ArrDate":"\/Date(1432777800000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1678","Airline":"","Cabin":"Y","SubCabin":"","SCity":"HRB","ECity":"TSN","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432856700000+0800)\/","ArrDate":"\/Date(1432864800000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":2,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"MBSLFM","Mobile":"13127584465","OfficeNo":"TPE567","AdultPnr":null,"PriceList":[{"FacePrice":3370.00,"Tax":150.00,"Fuel":0.0,"TotalPrice":3520.00}],"ResultBag":"\r\nrtKW1F70                                                                       \r\n 1.ZI/XING KW1F70                                                               \r\n 2.  CA1858 Y   TU26MAY  SHAPEK HK1   0755 1015          E T2T3                 \r\n 3.  CA1689 Y   TH28MAY  PEKHRB HK1   0745 0950          E T3--                 \r\n 4.  CA1678 Y   FR29MAY  HRBTSN HK1   0745 1000          E                      \r\n 5.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       \r\n 6.REM 0525 1924 ZIXIN                                                          \r\n 7.13127584465                                                                  \r\n 8.TL/1058/25MAY/SHA888                                                         \r\n 9.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n10.SSR ADTK 1E BY TPE25MAY15/2124 OR CXL CA ALL SEGS                            \r\n11.OSI YY CTCT13127584465                                                       \r\n12.OSI CA CTCT02151812332                                                      +\r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pn                                                                             \r\n13.RMK TJ AUTH SHA255                                                          -\r\n14.RMK TJ AUTH SHA836                                                           \r\n15.RMK CA/MBSLFM                                                                \r\n16.TPE567                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 Y+Y+Y FARE:CNY3370.00 TAX:CNY150.00 YQ:TEXEMPTYQ  TOTAL:3520.00              \r\n\u0010SFC:01   \u0010SFN:01/01   \u0010SFN:01/02   \u0010SFN:01/03\r\n"}
            // {"PassengerList":[{"name":"ZI/XING","idtype":0,"cardno":"NI320681198610238019","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KW1F70","FlightList":[{"FlightNo":"CA1858","Airline":"","Cabin":"Y","SubCabin":"","SCity":"SHA","ECity":"PEK","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1432598100000+0800)\/","ArrDate":"\/Date(1432606500000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1689","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"HRB","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1432770300000+0800)\/","ArrDate":"\/Date(1432777800000+0800)\/","PNRState":"HK1"},{"FlightNo":"CA1678","Airline":"","Cabin":"Y","SubCabin":"","SCity":"HRB","ECity":"TSN","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432856700000+0800)\/","ArrDate":"\/Date(1432864800000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":2,"RMKOfficeNoList":["SHA255","SHA836"],"BigPNR":"MBSLFM","Mobile":"13127584465","OfficeNo":"TPE567","AdultPnr":null,"PriceList":[{"FacePrice":3370.00,"Tax":150.00,"Fuel":0.0,"TotalPrice":3520.00,"Tag":"Y+Y+Y"}],"ResultBag":"\r\nrtKW1F70                                                                       \r\n 1.ZI/XING KW1F70                                                               \r\n 2.  CA1858 Y   TU26MAY  SHAPEK HK1   0755 1015          E T2T3                 \r\n 3.  CA1689 Y   TH28MAY  PEKHRB HK1   0745 0950          E T3--                 \r\n 4.  CA1678 Y   FR29MAY  HRBTSN HK1   0745 1000          E                      \r\n 5.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       \r\n 6.REM 0525 1924 ZIXIN                                                          \r\n 7.13127584465                                                                  \r\n 8.TL/1058/25MAY/SHA888                                                         \r\n 9.SSR FOID CA HK1 NI320681198610238019/P1                                      \r\n10.SSR ADTK 1E BY TPE25MAY15/2124 OR CXL CA ALL SEGS                            \r\n11.OSI YY CTCT13127584465                                                       \r\n12.OSI CA CTCT02151812332                                                      +\r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pn                                                                             \r\n13.RMK TJ AUTH SHA255                                                          -\r\n14.RMK TJ AUTH SHA836                                                           \r\n15.RMK CA/MBSLFM                                                                \r\n16.TPE567                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 Y+Y+Y FARE:CNY3370.00 TAX:CNY150.00 YQ:TEXEMPTYQ  TOTAL:3520.00              \r\n\u0010SFC:01   \u0010SFN:01/01   \u0010SFN:01/02   \u0010SFN:01/03\r\n"}
            string cmdResult6 =
                //string cmdResult =
@"
rtKW1F70                                                                       
 1.ZI/XING KW1F70                                                               
 2.  CA1858 Y   TU26MAY  SHAPEK HK1   0755 1015          E T2T3                 
 3.  CA1689 Y   TH28MAY  PEKHRB HK1   0745 0950          E T3--                 
 4.  CA1678 Y   FR29MAY  HRBTSN HK1   0745 1000          E                      
 5.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       
 6.REM 0525 1924 ZIXIN                                                          
 7.13127584465                                                                  
 8.TL/1058/25MAY/SHA888                                                         
 9.SSR FOID CA HK1 NI320681198610238019/P1                                      
10.SSR ADTK 1E BY TPE25MAY15/2124 OR CXL CA ALL SEGS                            
11.OSI YY CTCT13127584465                                                       
12.OSI CA CTCT02151812332                                                      +
                                                                               
                                                                                
                                                                                
pn                                                                             
13.RMK TJ AUTH SHA255                                                          -
14.RMK TJ AUTH SHA836                                                           
15.RMK CA/MBSLFM                                                                
16.TPE567                                                                       
                                                                               
                                                                                
                                                                                
pat:a                                                                          
>PAT:A                                                                          
01 Y+Y+Y FARE:CNY3370.00 TAX:CNY150.00 YQ:TEXEMPTYQ  TOTAL:3520.00              
SFC:01   SFN:01/01   SFN:01/02   SFN:01/03
";

            // 儿童单程票，只有1个价格，无子舱
            // 返回结果：
            // {"PassengerList":[{"name":"张越CHD","idtype":0,"cardno":"NI150102200804245116","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1208966400000+0800)\/","TicketNo":"9992124675454"}],"PNR":"HSNH3Q","FlightList":[{"FlightNo":"CA1345","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"SYX","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1422946200000+0800)\/","ArrDate":"\/Date(1422961200000+0800)\/","PNRState":"RR1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"PEK888","AdultPnr":"JWD6G2","PriceList":[{"FacePrice":1160.00,"Tax":0.0,"Fuel":60.00,"TotalPrice":1220.00}],"ResultBag":"\r\n**ELECTRONIC TICKET PNR**   \r\n  1.张越CHD HSNH3Q     \r\n  2.  CA1345 Y   MO03FEB  PEKSYX RR1   1450 1900      E T3--   \r\n  3.NC     \r\n  4.NC     \r\n  5.T  \r\n  6.SSR FOID CA HK1 NI150102200804245116/P1    \r\n  7.SSR TKNE CA HK1 PEKSYX 1345 Y03FEB 9992124675454/1/P1  \r\n  8.SSR OTHS 1E 1 CAAIRLINES ET PNR    \r\n  9.SSR OTHS 1E 1 PNR RR AND PRINTED  \r\n  10.SSR OTHS CA ADULT PNR IS JWD6G2   \r\n  11.SSR TKTL CA XX/ BJS 2221/23JAN14                                              \r\n  12.SSR CHLD CA HK1 24APR08/P1                                                  -\r\n  13.OSI CA CTCT   \r\n  14.OSI 1E CAET TN/9992124675454  \r\n  15.FN/M/FCNY1160.00/XCNY60.00/TEXEMPTCN/TCNY60.00YQ/ACNY1220.00  \r\n  16.TN/999-2124675454/P1  \r\n  17.FP/CC/    \r\n  18.PEK888     \r\n  [price]\r\n  >PAT:A*CH    \r\n  01 YCH50 FARE:CNY1160.00 TAX:TEXEMPTCN YQ:CNY60.00  TOTAL:1220.00     \r\n  SFC:01    RMK CA/NWCMGX\r\n"}
            // {"PassengerList":[{"name":"张越CHD","idtype":0,"cardno":"NI150102200804245116","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1208966400000+0800)\/","TicketNo":"9992124675454"}],"PNR":"HSNH3Q","FlightList":[{"FlightNo":"CA1345","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"SYX","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1422946200000+0800)\/","ArrDate":"\/Date(1422961200000+0800)\/","PNRState":"RR1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"PEK888","AdultPnr":"JWD6G2","PriceList":[{"FacePrice":1160.00,"Tax":0.0,"Fuel":60.00,"TotalPrice":1220.00}],"ResultBag":"\r\n**ELECTRONIC TICKET PNR**   \r\n  1.张越CHD HSNH3Q     \r\n  2.  CA1345 Y   MO03FEB  PEKSYX RR1   1450 1900      E T3--   \r\n  3.NC     \r\n  4.NC     \r\n  5.T  \r\n  6.SSR FOID CA HK1 NI150102200804245116/P1    \r\n  7.SSR TKNE CA HK1 PEKSYX 1345 Y03FEB 9992124675454/1/P1  \r\n  8.SSR OTHS 1E 1 CAAIRLINES ET PNR    \r\n  9.SSR OTHS 1E 1 PNR RR AND PRINTED  \r\n  10.SSR OTHS CA ADULT PNR IS JWD6G2   \r\n  11.SSR TKTL CA XX/ BJS 2221/23JAN14                                              \r\n  12.SSR CHLD CA HK1 24APR08/P1                                                  -\r\n  13.OSI CA CTCT   \r\n  14.OSI 1E CAET TN/9992124675454  \r\n  15.FN/M/FCNY1160.00/XCNY60.00/TEXEMPTCN/TCNY60.00YQ/ACNY1220.00  \r\n  16.TN/999-2124675454/P1  \r\n  17.FP/CC/    \r\n  18.PEK888     \r\n  [price]\r\n  >PAT:A*CH    \r\n  01 YCH50 FARE:CNY1160.00 TAX:TEXEMPTCN YQ:CNY60.00  TOTAL:1220.00     \r\n  SFC:01    RMK CA/NWCMGX\r\n"}
            // finished tested
            // {"PassengerList":[{"name":"张越CHD","idtype":0,"cardno":"NI150102200804245116","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1208966400000+0800)\/","TicketNo":"9992124675454"}],"PNR":"HSNH3Q","FlightList":[{"FlightNo":"CA1345","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"SYX","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1422946200000+0800)\/","ArrDate":"\/Date(1422961200000+0800)\/","PNRState":"RR1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"PEK888","AdultPnr":"JWD6G2","PriceList":[{"FacePrice":1160.00,"Tax":0.0,"Fuel":60.00,"TotalPrice":1220.00}],"ResultBag":"\r\n**ELECTRONIC TICKET PNR**   \r\n  1.张越CHD HSNH3Q     \r\n  2.  CA1345 Y   MO03FEB  PEKSYX RR1   1450 1900      E T3--   \r\n  3.NC     \r\n  4.NC     \r\n  5.T  \r\n  6.SSR FOID CA HK1 NI150102200804245116/P1    \r\n  7.SSR TKNE CA HK1 PEKSYX 1345 Y03FEB 9992124675454/1/P1  \r\n  8.SSR OTHS 1E 1 CAAIRLINES ET PNR    \r\n  9.SSR OTHS 1E 1 PNR RR AND PRINTED  \r\n  10.SSR OTHS CA ADULT PNR IS JWD6G2   \r\n  11.SSR TKTL CA XX/ BJS 2221/23JAN14                                              \r\n  12.SSR CHLD CA HK1 24APR08/P1                                                  -\r\n  13.OSI CA CTCT   \r\n  14.OSI 1E CAET TN/9992124675454  \r\n  15.FN/M/FCNY1160.00/XCNY60.00/TEXEMPTCN/TCNY60.00YQ/ACNY1220.00  \r\n  16.TN/999-2124675454/P1  \r\n  17.FP/CC/    \r\n  18.PEK888     \r\n  [price]\r\n  >PAT:A*CH    \r\n  01 YCH50 FARE:CNY1160.00 TAX:TEXEMPTCN YQ:CNY60.00  TOTAL:1220.00     \r\n  SFC:01    RMK CA/NWCMGX\r\n"}
            // {"PassengerList":[{"name":"张越CHD","idtype":0,"cardno":"NI150102200804245116","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1208966400000+0800)\/","TicketNo":"9992124675454"}],"PNR":"HSNH3Q","FlightList":[{"FlightNo":"CA1345","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"SYX","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1422946200000+0800)\/","ArrDate":"\/Date(1422961200000+0800)\/","PNRState":"RR1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"NWCMGX","Mobile":"","OfficeNo":"PEK888","AdultPnr":"JWD6G2","PriceList":[{"FacePrice":1160.00,"Tax":0.0,"Fuel":60.00,"TotalPrice":1220.00,"Tag":"YCH50"}],"ResultBag":"\r\n**ELECTRONIC TICKET PNR**   \r\n  1.张越CHD HSNH3Q     \r\n  2.  CA1345 Y   MO03FEB  PEKSYX RR1   1450 1900      E T3--   \r\n  3.NC     \r\n  4.NC     \r\n  5.T  \r\n  6.SSR FOID CA HK1 NI150102200804245116/P1    \r\n  7.SSR TKNE CA HK1 PEKSYX 1345 Y03FEB 9992124675454/1/P1  \r\n  8.SSR OTHS 1E 1 CAAIRLINES ET PNR    \r\n  9.SSR OTHS 1E 1 PNR RR AND PRINTED  \r\n  10.SSR OTHS CA ADULT PNR IS JWD6G2   \r\n  11.SSR TKTL CA XX/ BJS 2221/23JAN14                                              \r\n  12.SSR CHLD CA HK1 24APR08/P1                                                  -\r\n  13.OSI CA CTCT   \r\n  14.OSI 1E CAET TN/9992124675454  \r\n  15.FN/M/FCNY1160.00/XCNY60.00/TEXEMPTCN/TCNY60.00YQ/ACNY1220.00  \r\n  16.TN/999-2124675454/P1  \r\n  17.FP/CC/    \r\n  18.PEK888     \r\n  [price]\r\n  >PAT:A*CH    \r\n  01 YCH50 FARE:CNY1160.00 TAX:TEXEMPTCN YQ:CNY60.00  TOTAL:1220.00     \r\n  SFC:01    RMK CA/NWCMGX\r\n"}
            string cmdResult7 =
                //string cmdResult =
@"
**ELECTRONIC TICKET PNR**   
  1.张越CHD HSNH3Q     
  2.  CA1345 Y   MO03FEB  PEKSYX RR1   1450 1900      E T3--   
  3.NC     
  4.NC     
  5.T  
  6.SSR FOID CA HK1 NI150102200804245116/P1    
  7.SSR TKNE CA HK1 PEKSYX 1345 Y03FEB 9992124675454/1/P1  
  8.SSR OTHS 1E 1 CAAIRLINES ET PNR    
  9.SSR OTHS 1E 1 PNR RR AND PRINTED  
  10.SSR OTHS CA ADULT PNR IS JWD6G2   
  11.SSR TKTL CA XX/ BJS 2221/23JAN14                                              
  12.SSR CHLD CA HK1 24APR08/P1                                                  -
  13.OSI CA CTCT   
  14.OSI 1E CAET TN/9992124675454  
  15.FN/M/FCNY1160.00/XCNY60.00/TEXEMPTCN/TCNY60.00YQ/ACNY1220.00  
  16.TN/999-2124675454/P1  
  17.FP/CC/    
  18.PEK888     
  [price]
  >PAT:A*CH    
  01 YCH50 FARE:CNY1160.00 TAX:TEXEMPTCN YQ:CNY60.00  TOTAL:1220.00     
  SFC:01    RMK CA/NWCMGX
";

            // 儿童单程票，有2个价格，无子舱（没测，没找到测试案例）

            // 儿童单程票，有2个价格，有子舱（自己编的案例：把Y舱改为V，然后加了个V1，然后价格全部改掉）
            // 返回结果：
            // {"PassengerList":[{"name":"张越CHD","idtype":0,"cardno":"NI150102200804245116","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1208966400000+0800)\/","TicketNo":"9992124675454"}],"PNR":"HSNH3Q","FlightList":[{"FlightNo":"CA1345","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"PEK","ECity":"SYX","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1422946200000+0800)\/","ArrDate":"\/Date(1422961200000+0800)\/","PNRState":"RR1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"PEK888","AdultPnr":"JWD6G2","PriceList":[{"FacePrice":760.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":810.00}],"ResultBag":"\r\n**ELECTRONIC TICKET PNR**   \r\n  1.张越CHD HSNH3Q     \r\n  2.  CA1345 V   MO03FEB  PEKSYX RR1   1450 1900      E T3--  V1\r\n  3.NC     \r\n  4.NC     \r\n  5.T  \r\n  6.SSR FOID CA HK1 NI150102200804245116/P1    \r\n  7.SSR TKNE CA HK1 PEKSYX 1345 Y03FEB 9992124675454/1/P1  \r\n  8.SSR OTHS 1E 1 CAAIRLINES ET PNR    \r\n  9.SSR OTHS 1E 1 PNR RR AND PRINTED  \r\n  10.SSR OTHS CA ADULT PNR IS JWD6G2   \r\n  11.SSR TKTL CA XX/ BJS 2221/23JAN14                                              \r\n  12.SSR CHLD CA HK1 24APR08/P1                                                  -\r\n  13.OSI CA CTCT   \r\n  14.OSI 1E CAET TN/9992124675454  \r\n  15.FN/M/FCNY1160.00/XCNY60.00/TEXEMPTCN/TCNY60.00YQ/ACNY1220.00  \r\n  16.TN/999-2124675454/P1  \r\n  17.FP/CC/    \r\n  18.PEK888     \r\n  [price]\r\n  >PAT:A*CH    \r\n 01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02\r\n"}
            // {"PassengerList":[{"name":"张越CHD","idtype":0,"cardno":"NI150102200804245116","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1208966400000+0800)\/","TicketNo":"9992124675454"}],"PNR":"HSNH3Q","FlightList":[{"FlightNo":"CA1345","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"PEK","ECity":"SYX","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1422946200000+0800)\/","ArrDate":"\/Date(1422961200000+0800)\/","PNRState":"RR1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"PEK888","AdultPnr":"JWD6G2","PriceList":[{"FacePrice":760.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":810.00}],"ResultBag":"\r\n**ELECTRONIC TICKET PNR**   \r\n  1.张越CHD HSNH3Q     \r\n  2.  CA1345 V   MO03FEB  PEKSYX RR1   1450 1900      E T3--  V1\r\n  3.NC     \r\n  4.NC     \r\n  5.T  \r\n  6.SSR FOID CA HK1 NI150102200804245116/P1    \r\n  7.SSR TKNE CA HK1 PEKSYX 1345 Y03FEB 9992124675454/1/P1  \r\n  8.SSR OTHS 1E 1 CAAIRLINES ET PNR    \r\n  9.SSR OTHS 1E 1 PNR RR AND PRINTED  \r\n  10.SSR OTHS CA ADULT PNR IS JWD6G2   \r\n  11.SSR TKTL CA XX/ BJS 2221/23JAN14                                              \r\n  12.SSR CHLD CA HK1 24APR08/P1                                                  -\r\n  13.OSI CA CTCT   \r\n  14.OSI 1E CAET TN/9992124675454  \r\n  15.FN/M/FCNY1160.00/XCNY60.00/TEXEMPTCN/TCNY60.00YQ/ACNY1220.00  \r\n  16.TN/999-2124675454/P1  \r\n  17.FP/CC/    \r\n  18.PEK888     \r\n  [price]\r\n  >PAT:A*CH    \r\n 01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02\r\n"}
            // finished tested:
            // {"PassengerList":[{"name":"张越CHD","idtype":0,"cardno":"NI150102200804245116","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1208966400000+0800)\/","TicketNo":"9992124675454"}],"PNR":"HSNH3Q","FlightList":[{"FlightNo":"CA1345","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"PEK","ECity":"SYX","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1422946200000+0800)\/","ArrDate":"\/Date(1422961200000+0800)\/","PNRState":"RR1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"PEK888","AdultPnr":"JWD6G2","PriceList":[{"FacePrice":760.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":810.00}],"ResultBag":"\r\n**ELECTRONIC TICKET PNR**   \r\n  1.张越CHD HSNH3Q     \r\n  2.  CA1345 V   MO03FEB  PEKSYX RR1   1450 1900      E T3--  V1\r\n  3.NC     \r\n  4.NC     \r\n  5.T  \r\n  6.SSR FOID CA HK1 NI150102200804245116/P1    \r\n  7.SSR TKNE CA HK1 PEKSYX 1345 Y03FEB 9992124675454/1/P1  \r\n  8.SSR OTHS 1E 1 CAAIRLINES ET PNR    \r\n  9.SSR OTHS 1E 1 PNR RR AND PRINTED  \r\n  10.SSR OTHS CA ADULT PNR IS JWD6G2   \r\n  11.SSR TKTL CA XX/ BJS 2221/23JAN14                                              \r\n  12.SSR CHLD CA HK1 24APR08/P1                                                  -\r\n  13.OSI CA CTCT   \r\n  14.OSI 1E CAET TN/9992124675454  \r\n  15.FN/M/FCNY1160.00/XCNY60.00/TEXEMPTCN/TCNY60.00YQ/ACNY1220.00  \r\n  16.TN/999-2124675454/P1  \r\n  17.FP/CC/    \r\n  18.PEK888     \r\n  [price]\r\n  >PAT:A*CH    \r\n 01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02\r\n"}
            // {"PassengerList":[{"name":"张越CHD","idtype":0,"cardno":"NI150102200804245116","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1208966400000+0800)\/","TicketNo":"9992124675454"}],"PNR":"HSNH3Q","FlightList":[{"FlightNo":"CA1345","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"PEK","ECity":"SYX","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1422946200000+0800)\/","ArrDate":"\/Date(1422961200000+0800)\/","PNRState":"RR1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"PEK888","AdultPnr":"JWD6G2","PriceList":[{"FacePrice":760.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":810.00,"Tag":"V1"}],"ResultBag":"\r\n**ELECTRONIC TICKET PNR**   \r\n  1.张越CHD HSNH3Q     \r\n  2.  CA1345 V   MO03FEB  PEKSYX RR1   1450 1900      E T3--  V1\r\n  3.NC     \r\n  4.NC     \r\n  5.T  \r\n  6.SSR FOID CA HK1 NI150102200804245116/P1    \r\n  7.SSR TKNE CA HK1 PEKSYX 1345 Y03FEB 9992124675454/1/P1  \r\n  8.SSR OTHS 1E 1 CAAIRLINES ET PNR    \r\n  9.SSR OTHS 1E 1 PNR RR AND PRINTED  \r\n  10.SSR OTHS CA ADULT PNR IS JWD6G2   \r\n  11.SSR TKTL CA XX/ BJS 2221/23JAN14                                              \r\n  12.SSR CHLD CA HK1 24APR08/P1                                                  -\r\n  13.OSI CA CTCT   \r\n  14.OSI 1E CAET TN/9992124675454  \r\n  15.FN/M/FCNY1160.00/XCNY60.00/TEXEMPTCN/TCNY60.00YQ/ACNY1220.00  \r\n  16.TN/999-2124675454/P1  \r\n  17.FP/CC/    \r\n  18.PEK888     \r\n  [price]\r\n  >PAT:A*CH    \r\n 01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    \r\n SFC:01    SFN:01                                                               \r\n02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     \r\n SFC:02    SFN:02\r\n"}
            string cmdResult8 =
                //string cmdResult =
@"
**ELECTRONIC TICKET PNR**   
  1.张越CHD HSNH3Q     
  2.  CA1345 V   MO03FEB  PEKSYX RR1   1450 1900      E T3--  V1
  3.NC     
  4.NC     
  5.T  
  6.SSR FOID CA HK1 NI150102200804245116/P1    
  7.SSR TKNE CA HK1 PEKSYX 1345 Y03FEB 9992124675454/1/P1  
  8.SSR OTHS 1E 1 CAAIRLINES ET PNR    
  9.SSR OTHS 1E 1 PNR RR AND PRINTED  
  10.SSR OTHS CA ADULT PNR IS JWD6G2   
  11.SSR TKTL CA XX/ BJS 2221/23JAN14                                              
  12.SSR CHLD CA HK1 24APR08/P1                                                  -
  13.OSI CA CTCT   
  14.OSI 1E CAET TN/9992124675454  
  15.FN/M/FCNY1160.00/XCNY60.00/TEXEMPTCN/TCNY60.00YQ/ACNY1220.00  
  16.TN/999-2124675454/P1  
  17.FP/CC/    
  18.PEK888     
  [price]
  >PAT:A*CH    
 01 V1 FARE:CNY760.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:810.00                    
 SFC:01    SFN:01                                                               
02 V FARE:CNY860.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:910.00                     
 SFC:02    SFN:02
";

            // 2015-06-12(5)tested:
            // 测试价格解析不出来的情况：经过分析，价格取不出来是因为航站楼格式弄成了T3，而不是T3--或--T3

            // 测试价格解析不出来的情况（航站楼格式为T3，包含【>PAT:A】）
            // finished tested
            // 返回结果：
            // {"PassengerList":[{"name":"张磊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KERP42","FlightList":[{"FlightNo":"HU7048","Airline":"","Cabin":"Q","SubCabin":"","SCity":"SHE","ECity":"HAK","DepTerminal":"T3","ArrTerminal":null,"DepDate":"\/Date(1434090300000+0800)\/","ArrDate":"\/Date(1434112200000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA243"],"BigPNR":"NGV27E","Mobile":"13518822261","OfficeNo":"HAK227","AdultPnr":null,"PriceList":[{"FacePrice":1520.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1570.00}],"ResultBag":"\r\nRT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 \r\n"}
            // {"PassengerList":[{"name":"张磊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KERP42","FlightList":[{"FlightNo":"HU7048","Airline":"","Cabin":"Q","SubCabin":"","SCity":"SHE","ECity":"HAK","DepTerminal":"T3","ArrTerminal":null,"DepDate":"\/Date(1434090300000+0800)\/","ArrDate":"\/Date(1434112200000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA243"],"BigPNR":"NGV27E","Mobile":"13518822261","OfficeNo":"HAK227","AdultPnr":null,"PriceList":[{"FacePrice":1520.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1570.00,"Tag":"Q"}],"ResultBag":"\r\nRT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 \r\n"}
            //string cmdResult =
            string cmdResult9 =
@"
RT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 
";

            // tested
            // 返回结果：
            // {"PassengerList":[{"name":"白小菊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"槐笑萱","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"槐洋","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"李嘉","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNZDB6","FlightList":[{"FlightNo":"HU7280","Airline":"","Cabin":"A","SubCabin":"","SCity":"SYX","ECity":"PEK","DepTerminal":"T1","ArrTerminal":null,"DepDate":"\/Date(1437900300000+0800)\/","ArrDate":"\/Date(1437914400000+0800)\/","PNRState":"HK4"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"BJS182","AdultPnr":null,"PriceList":[{"FacePrice":2780.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":2830.00}],"ResultBag":"\r\nRT KNZDB6 1.白小菊 2.槐笑萱 3.槐洋 4.李嘉 KNZDB6 5. HU7280 A SU26JUL SYXPEK HK4 1645 2040 E T1 6.BJS/T PEK/T 010-65081681/HUAXIA AIR SERVICE CO.WWW.HUAXIAHANGKONG.COM/YUE DIAN WEI ABCDEFG 7.* 8.TL/1445/26JUL/BJS182 9.SSR FOID 10.SSR FOID 11.SSR FOID 12.SSR FOID 13.SSR ADTK 1E BY BJS12JUN15/1302CXL HU7280 A26JUL 14.SSR CHLD HU HK1 07AUG06/P2 PN 15.OSI CTC - 16.OSI HU CKIN SSAC/A 17.RMK 18.RMK 19.BJS182 PAT A >PAT:A 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01\r\n"}
            // {"PassengerList":[{"name":"白小菊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"槐笑萱","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"槐洋","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"李嘉","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNZDB6","FlightList":[{"FlightNo":"HU7280","Airline":"","Cabin":"A","SubCabin":"","SCity":"SYX","ECity":"PEK","DepTerminal":"T1","ArrTerminal":null,"DepDate":"\/Date(1437900300000+0800)\/","ArrDate":"\/Date(1437914400000+0800)\/","PNRState":"HK4"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"BJS182","AdultPnr":null,"PriceList":[{"FacePrice":2780.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":2830.00,"Tag":"AY110S"}],"ResultBag":"\r\nRT KNZDB6 1.白小菊 2.槐笑萱 3.槐洋 4.李嘉 KNZDB6 5. HU7280 A SU26JUL SYXPEK HK4 1645 2040 E T1 6.BJS/T PEK/T 010-65081681/HUAXIA AIR SERVICE CO.WWW.HUAXIAHANGKONG.COM/YUE DIAN WEI ABCDEFG 7.* 8.TL/1445/26JUL/BJS182 9.SSR FOID 10.SSR FOID 11.SSR FOID 12.SSR FOID 13.SSR ADTK 1E BY BJS12JUN15/1302CXL HU7280 A26JUL 14.SSR CHLD HU HK1 07AUG06/P2 PN 15.OSI CTC - 16.OSI HU CKIN SSAC/A 17.RMK 18.RMK 19.BJS182 PAT A >PAT:A 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01\r\n"}
            //string cmdResult =
            string cmdResult9_2 =
@"
RT KNZDB6 1.白小菊 2.槐笑萱 3.槐洋 4.李嘉 KNZDB6 5. HU7280 A SU26JUL SYXPEK HK4 1645 2040 E T1 6.BJS/T PEK/T 010-65081681/HUAXIA AIR SERVICE CO.WWW.HUAXIAHANGKONG.COM/YUE DIAN WEI ABCDEFG 7.* 8.TL/1445/26JUL/BJS182 9.SSR FOID 10.SSR FOID 11.SSR FOID 12.SSR FOID 13.SSR ADTK 1E BY BJS12JUN15/1302CXL HU7280 A26JUL 14.SSR CHLD HU HK1 07AUG06/P2 PN 15.OSI CTC - 16.OSI HU CKIN SSAC/A 17.RMK 18.RMK 19.BJS182 PAT A >PAT:A 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01
";

            // 测试价格解析不出来的情况（航站楼格式为T3--或--T3，包含【>PAT:A】）
            // {"PassengerList":[{"name":"张磊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KERP42","FlightList":[{"FlightNo":"HU7048","Airline":"","Cabin":"Q","SubCabin":"","SCity":"SHE","ECity":"HAK","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1434090300000+0800)\/","ArrDate":"\/Date(1434112200000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA243"],"BigPNR":"NGV27E","Mobile":"13518822261","OfficeNo":"HAK227","AdultPnr":null,"PriceList":[{"FacePrice":1520.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1570.00,"Tag":"Q"}],"ResultBag":"\r\nRT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3-- 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 \r\n"}
            //string cmdResult =
            string cmdResult10 =
@"
RT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3-- 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 
";

            // finished tested
            // 返回结果：
            // {"PassengerList":[{"name":"张磊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KERP42","FlightList":[{"FlightNo":"HU7048","Airline":"","Cabin":"Q","SubCabin":"","SCity":"SHE","ECity":"HAK","DepTerminal":"","ArrTerminal":"T3","DepDate":"\/Date(1434090300000+0800)\/","ArrDate":"\/Date(1434112200000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA243"],"BigPNR":"NGV27E","Mobile":"13518822261","OfficeNo":"HAK227","AdultPnr":null,"PriceList":[{"FacePrice":1520.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1570.00}],"ResultBag":"\r\nRT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E --T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 \r\n"}
            // {"PassengerList":[{"name":"张磊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KERP42","FlightList":[{"FlightNo":"HU7048","Airline":"","Cabin":"Q","SubCabin":"","SCity":"SHE","ECity":"HAK","DepTerminal":"","ArrTerminal":"T3","DepDate":"\/Date(1434090300000+0800)\/","ArrDate":"\/Date(1434112200000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA243"],"BigPNR":"NGV27E","Mobile":"13518822261","OfficeNo":"HAK227","AdultPnr":null,"PriceList":[{"FacePrice":1520.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1570.00,"Tag":"Q"}],"ResultBag":"\r\nRT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E --T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 \r\n"}
            //string cmdResult =
            string cmdResult11 =
@"
RT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E --T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A >PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 
";

            // 测试价格解析不出来的情况（航站楼格式为T3，不包含【>PAT:A】，但包含【PAT:A】）
            // finished tested
            // 返回结果：
            // {"PassengerList":[{"name":"张磊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KERP42","FlightList":[{"FlightNo":"HU7048","Airline":"","Cabin":"Q","SubCabin":"","SCity":"SHE","ECity":"HAK","DepTerminal":"T3","ArrTerminal":null,"DepDate":"\/Date(1434090300000+0800)\/","ArrDate":"\/Date(1434112200000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA243"],"BigPNR":"NGV27E","Mobile":"13518822261","OfficeNo":"HAK227","AdultPnr":null,"PriceList":[{"FacePrice":1520.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1570.00}],"ResultBag":"\r\nRT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 \r\n"}
            // 未测
            //string cmdResult =
            string cmdResult12 =
@"
RT KERP42 1.张磊 KERP42 2. HU7048 Q FR12JUN SHEHAK HK1 1425 2030 E T3 3.HAK/T HAK/T 0898-66725259/HAK XIN DU AVATION BOOKING OFFICE/WEN FEI HUA ABCDERG 4.REM 0611 2013 HAK070 68581478 5.TL/2300/11JUN/HAK227 6.SSR FOID 7.SSR ADTK 1E BY HAK11JUN15/2013CXL HU7048 Q12JUN 8.OSI HU CTCT13518822261 9.RMK CA/NGV27E 10.RMK TJ AUTH SHA243 11.HAK227 PAT A PAT:A 01 Q FARE:CNY1520.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1570.00 SFC:01 SFN:01 
";

            // finished tested
            // 返回结果：
            // {"PassengerList":[{"name":"白小菊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"槐笑萱","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"槐洋","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"李嘉","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNZDB6","FlightList":[{"FlightNo":"HU7280","Airline":"","Cabin":"A","SubCabin":"","SCity":"SYX","ECity":"PEK","DepTerminal":"T1","ArrTerminal":null,"DepDate":"\/Date(1437900300000+0800)\/","ArrDate":"\/Date(1437914400000+0800)\/","PNRState":"HK4"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"BJS182","AdultPnr":null,"PriceList":[{"FacePrice":2780.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":2830.00}],"ResultBag":"\r\nRT KNZDB6 1.白小菊 2.槐笑萱 3.槐洋 4.李嘉 KNZDB6 5. HU7280 A SU26JUL SYXPEK HK4 1645 2040 E T1 6.BJS/T PEK/T 010-65081681/HUAXIA AIR SERVICE CO.WWW.HUAXIAHANGKONG.COM/YUE DIAN WEI ABCDEFG 7.* 8.TL/1445/26JUL/BJS182 9.SSR FOID 10.SSR FOID 11.SSR FOID 12.SSR FOID 13.SSR ADTK 1E BY BJS12JUN15/1302CXL HU7280 A26JUL 14.SSR CHLD HU HK1 07AUG06/P2 PN 15.OSI CTC - 16.OSI HU CKIN SSAC/A 17.RMK 18.RMK 19.BJS182 PAT A PAT:A 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01\r\n"}
            // 未测
            //string cmdResult =
            string cmdResult12_2 =
@"
RT KNZDB6 1.白小菊 2.槐笑萱 3.槐洋 4.李嘉 KNZDB6 5. HU7280 A SU26JUL SYXPEK HK4 1645 2040 E T1 6.BJS/T PEK/T 010-65081681/HUAXIA AIR SERVICE CO.WWW.HUAXIAHANGKONG.COM/YUE DIAN WEI ABCDEFG 7.* 8.TL/1445/26JUL/BJS182 9.SSR FOID 10.SSR FOID 11.SSR FOID 12.SSR FOID 13.SSR ADTK 1E BY BJS12JUN15/1302CXL HU7280 A26JUL 14.SSR CHLD HU HK1 07AUG06/P2 PN 15.OSI CTC - 16.OSI HU CKIN SSAC/A 17.RMK 18.RMK 19.BJS182 PAT A PAT:A 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01
";

            // finished tested
            // 返回结果：
            // {"PassengerList":[{"name":"白小菊","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"槐笑萱","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"槐洋","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"李嘉","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KNZDB6","FlightList":[{"FlightNo":"HU7280","Airline":"","Cabin":"A1","SubCabin":"A1","SCity":"SYX","ECity":"PEK","DepTerminal":"T1","ArrTerminal":null,"DepDate":"\/Date(1437900300000+0800)\/","ArrDate":"\/Date(1437914400000+0800)\/","PNRState":"HK4"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"","Mobile":"","OfficeNo":"BJS182","AdultPnr":null,"PriceList":[{"FacePrice":2780.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":2830.00}],"ResultBag":"\r\nRT KNZDB6 1.白小菊 2.槐笑萱 3.槐洋 4.李嘉 KNZDB6 5. HU7280 A SU26JUL SYXPEK HK4 1645 2040 E T1 A1 6.BJS/T PEK/T 010-65081681/HUAXIA AIR SERVICE CO.WWW.HUAXIAHANGKONG.COM/YUE DIAN WEI ABCDEFG 7.* 8.TL/1445/26JUL/BJS182 9.SSR FOID 10.SSR FOID 11.SSR FOID 12.SSR FOID 13.SSR ADTK 1E BY BJS12JUN15/1302CXL HU7280 A26JUL 14.SSR CHLD HU HK1 07AUG06/P2 PN 15.OSI CTC - 16.OSI HU CKIN SSAC/A 17.RMK 18.RMK 19.BJS182 PAT A PAT:A 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01\r\n"}
            // 未测
            //string cmdResult =
                string cmdResult12_3 =
@"
RT KNZDB6 1.白小菊 2.槐笑萱 3.槐洋 4.李嘉 KNZDB6 5. HU7280 A SU26JUL SYXPEK HK4 1645 2040 E T1 A1 6.BJS/T PEK/T 010-65081681/HUAXIA AIR SERVICE CO.WWW.HUAXIAHANGKONG.COM/YUE DIAN WEI ABCDEFG 7.* 8.TL/1445/26JUL/BJS182 9.SSR FOID 10.SSR FOID 11.SSR FOID 12.SSR FOID 13.SSR ADTK 1E BY BJS12JUN15/1302CXL HU7280 A26JUL 14.SSR CHLD HU HK1 07AUG06/P2 PN 15.OSI CTC - 16.OSI HU CKIN SSAC/A 17.RMK 18.RMK 19.BJS182 PAT A PAT:A 01 AY110S FARE:CNY2780.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:2830.00 SFC:01 SFN:01
";

            // 往返票--有子舱、价格属于折上折的情况
            // 测试价格不出来的问题
            // finished tested
            // 返回结果：
            // {"PassengerList":[{"name":"王秀文","idtype":0,"cardno":"NI130730198703010048","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"袁路瑶","idtype":0,"cardno":"NI130730199001073815","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HZ51XZ","FlightList":[{"FlightNo":"CA1311","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"PEK","ECity":"KWL","DepTerminal":"T3","ArrTerminal":null,"DepDate":"\/Date(1433542500000+0800)\/","ArrDate":"\/Date(1433553900000+0800)\/","PNRState":"HK2"},{"FlightNo":"CA1226","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"KWL","ECity":"PEK","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1433425200000+0800)\/","ArrDate":"\/Date(1433435700000+0800)\/","PNRState":"HK2"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":[],"BigPNR":"PF578X","Mobile":"13521308694","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":1110.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":1210.00}],"ResultBag":"\r\nRT HZ51XZ 1.王秀文 2.袁路瑶 HZ51XZ 3. CA1311 V Sa06jun PEKKWL HK2 0615 0925 E T3 V1 4. CA1226 V TH04JUN KWLPEK HK2 2140 0035 1 E T3 V1 5.BJS/T BJS/T-51668450/BJS HAI HUA AVIATION SERVICE LTD.CO/HUANG HUA ABCDEFG 6.REM 0529 1143 BJS860-30 7.TL/2000/30MAY/BJS860 8.SSR FOID CA HK1 NI130730198703010048/P1 9.SSR FOID CA HK1 NI130730199001073815/P2 10.SSR ADTK 1E BY BJS29MAY15/1243CXL CA ALL SEGS 11.OSI CA CTCT13521308694 12.OSI CA CTCM13031077381/P1 13.RMK CA/PF578X PAT:A >PAT:A 01 YWB/CA1Y143234 FARE:CNY1110.00 TAX:CNY100.00 YQ:TEXEMPTYQ TOTAL:1210.00 SFC:01\r\n"}
                // {"PassengerList":[{"name":"王秀文","idtype":0,"cardno":"NI130730198703010048","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"袁路瑶","idtype":0,"cardno":"NI130730199001073815","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HZ51XZ","FlightList":[{"FlightNo":"CA1311","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"PEK","ECity":"KWL","DepTerminal":"T3","ArrTerminal":null,"DepDate":"\/Date(1433542500000+0800)\/","ArrDate":"\/Date(1433553900000+0800)\/","PNRState":"HK2"},{"FlightNo":"CA1226","Airline":"","Cabin":"V1","SubCabin":"V1","SCity":"KWL","ECity":"PEK","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1433425200000+0800)\/","ArrDate":"\/Date(1433435700000+0800)\/","PNRState":"HK2"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":[],"BigPNR":"PF578X","Mobile":"13521308694","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":1110.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":1210.00,"Tag":"YWB/CA1Y143234"}],"ResultBag":"\r\nRT HZ51XZ 1.王秀文 2.袁路瑶 HZ51XZ 3. CA1311 V Sa06jun PEKKWL HK2 0615 0925 E T3 V1 4. CA1226 V TH04JUN KWLPEK HK2 2140 0035 1 E T3 V1 5.BJS/T BJS/T-51668450/BJS HAI HUA AVIATION SERVICE LTD.CO/HUANG HUA ABCDEFG 6.REM 0529 1143 BJS860-30 7.TL/2000/30MAY/BJS860 8.SSR FOID CA HK1 NI130730198703010048/P1 9.SSR FOID CA HK1 NI130730199001073815/P2 10.SSR ADTK 1E BY BJS29MAY15/1243CXL CA ALL SEGS 11.OSI CA CTCT13521308694 12.OSI CA CTCM13031077381/P1 13.RMK CA/PF578X PAT:A >PAT:A 01 YWB/CA1Y143234 FARE:CNY1110.00 TAX:CNY100.00 YQ:TEXEMPTYQ TOTAL:1210.00 SFC:01\r\n"}
            string cmdResult31 =
               // string cmdResult =
@"
RT HZ51XZ 1.王秀文 2.袁路瑶 HZ51XZ 3. CA1311 V Sa06jun PEKKWL HK2 0615 0925 E T3 V1 4. CA1226 V TH04JUN KWLPEK HK2 2140 0035 1 E T3 V1 5.BJS/T BJS/T-51668450/BJS HAI HUA AVIATION SERVICE LTD.CO/HUANG HUA ABCDEFG 6.REM 0529 1143 BJS860-30 7.TL/2000/30MAY/BJS860 8.SSR FOID CA HK1 NI130730198703010048/P1 9.SSR FOID CA HK1 NI130730199001073815/P2 10.SSR ADTK 1E BY BJS29MAY15/1243CXL CA ALL SEGS 11.OSI CA CTCT13521308694 12.OSI CA CTCM13031077381/P1 13.RMK CA/PF578X PAT:A >PAT:A 01 YWB/CA1Y143234 FARE:CNY1110.00 TAX:CNY100.00 YQ:TEXEMPTYQ TOTAL:1210.00 SFC:01
";

            // finished tested
            // 测试第2条和第3条价格是否都取到的问题
            // 返回结果：
            // {"PassengerList":[{"name":"贺明友","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KQH1CP","FlightList":[{"FlightNo":"NS3246","Airline":"","Cabin":"V","SubCabin":"","SCity":"KWE","ECity":"XIY","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434171000000+0800)\/","ArrDate":"\/Date(1434177000000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["CAN106"],"BigPNR":"PY6SB0","Mobile":"13985197686","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":460.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":510.00}],"ResultBag":"\r\n\u0010RTKQH1CP                                                                       \r\n 1.贺明友 KQH1CP                                                                \r\n 2.  NS3246 V   SA13JUN  KWEXIY HK1   1250 1430          E T2T3                 \r\n 3.KWE/T KWE/T-0851-5870000/KWE HANG LIAN SERVICE AGENCY LTD.,CO./CUI WEI       \r\n     ABCDEFG                                                                    \r\n 4.REM 0611 1635 HANGLIAN31 0851-8288888                                        \r\n 5.TL/1900/11JUN/KWE164                                                         \r\n 6.SSR FOID                                                                     \r\n 7.SSR ADTK 1E BY KWE11JUN15/1735 OR CXL NS3246 V13JUN                          \r\n 8.OSI NS CTCT13985197686                                                       \r\n 9.RMK TJ AUTH CAN106                                                           \r\n10.RMK CA/PY6SB0                                                                \r\n11.RMK TLWBINSD                                                                +\r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010PAT A                                                                          \r\n>PAT:A                                                                          \r\n01 V FARE:CNY460.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:510.00                     \r\n\u0010SFC:01   \u0010SFN:01                                                               \r\n02 YV FARE:CNY410.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:460.00                    \r\n\u0010SFC:02   \u0010SFN:02                                                               \r\n03 V FARE:CNY460.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:510.00                     \r\n\u0010SFC:03   \u0010SFN:03\r\n"}
            // {"PassengerList":[{"name":"贺明友","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KQH1CP","FlightList":[{"FlightNo":"NS3246","Airline":"","Cabin":"V","SubCabin":"","SCity":"KWE","ECity":"XIY","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434171000000+0800)\/","ArrDate":"\/Date(1434177000000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["CAN106"],"BigPNR":"PY6SB0","Mobile":"13985197686","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":460.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":510.00},{"FacePrice":410.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":460.00},{"FacePrice":460.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":510.00}],"ResultBag":"\r\n\u0010rtKQH1CP                                                                       \r\n 1.贺明友 KQH1CP                                                                \r\n 2.  NS3246 V   SA13JUN  KWEXIY HK1   1250 1430          E T2T3                 \r\n 3.KWE/T KWE/T-0851-5870000/KWE HANG LIAN SERVICE AGENCY LTD.,CO./CUI WEI       \r\n     ABCDEFG                                                                    \r\n 4.REM 0611 1635 HANGLIAN31 0851-8288888                                        \r\n 5.TL/1900/11JUN/KWE164                                                         \r\n 6.SSR FOID                                                                     \r\n 7.SSR ADTK 1E BY KWE11JUN15/1735 OR CXL NS3246 V13JUN                          \r\n 8.OSI NS CTCT13985197686                                                       \r\n 9.RMK TJ AUTH CAN106                                                           \r\n10.RMK CA/PY6SB0                                                                \r\n11.RMK TLWBINSD                                                                +\r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat a                                                                          \r\n>PAT:A                                                                          \r\n01 V FARE:CNY460.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:510.00                     \r\n\u0010SFC:01   \u0010SFN:01                                                               \r\n02 YV FARE:CNY410.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:460.00                    \r\n\u0010SFC:02   \u0010SFN:02                                                               \r\n03 V FARE:CNY460.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:510.00                     \r\n\u0010SFC:03   \u0010SFN:03\r\n"}
            // {"PassengerList":[{"name":"贺明友","idtype":-1,"cardno":"","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KQH1CP","FlightList":[{"FlightNo":"NS3246","Airline":"","Cabin":"V","SubCabin":"","SCity":"KWE","ECity":"XIY","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434171000000+0800)\/","ArrDate":"\/Date(1434177000000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["CAN106"],"BigPNR":"PY6SB0","Mobile":"13985197686","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":460.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":510.00,"Tag":"V"},{"FacePrice":410.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":460.00,"Tag":"YV"},{"FacePrice":460.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":510.00,"Tag":"V"}],"ResultBag":"\r\n\u0010rtKQH1CP                                                                       \r\n 1.贺明友 KQH1CP                                                                \r\n 2.  NS3246 V   SA13JUN  KWEXIY HK1   1250 1430          E T2T3                 \r\n 3.KWE/T KWE/T-0851-5870000/KWE HANG LIAN SERVICE AGENCY LTD.,CO./CUI WEI       \r\n     ABCDEFG                                                                    \r\n 4.REM 0611 1635 HANGLIAN31 0851-8288888                                        \r\n 5.TL/1900/11JUN/KWE164                                                         \r\n 6.SSR FOID                                                                     \r\n 7.SSR ADTK 1E BY KWE11JUN15/1735 OR CXL NS3246 V13JUN                          \r\n 8.OSI NS CTCT13985197686                                                       \r\n 9.RMK TJ AUTH CAN106                                                           \r\n10.RMK CA/PY6SB0                                                                \r\n11.RMK TLWBINSD                                                                +\r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat a                                                                          \r\n>PAT:A                                                                          \r\n01 V FARE:CNY460.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:510.00                     \r\n\u0010SFC:01   \u0010SFN:01                                                               \r\n02 YV FARE:CNY410.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:460.00                    \r\n\u0010SFC:02   \u0010SFN:02                                                               \r\n03 V FARE:CNY460.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:510.00                     \r\n\u0010SFC:03   \u0010SFN:03\r\n"}
            //string cmdResult =
            string cmdResult13 =
@"
rtKQH1CP                                                                       
 1.贺明友 KQH1CP                                                                
 2.  NS3246 V   SA13JUN  KWEXIY HK1   1250 1430          E T2T3                 
 3.KWE/T KWE/T-0851-5870000/KWE HANG LIAN SERVICE AGENCY LTD.,CO./CUI WEI       
     ABCDEFG                                                                    
 4.REM 0611 1635 HANGLIAN31 0851-8288888                                        
 5.TL/1900/11JUN/KWE164                                                         
 6.SSR FOID                                                                     
 7.SSR ADTK 1E BY KWE11JUN15/1735 OR CXL NS3246 V13JUN                          
 8.OSI NS CTCT13985197686                                                       
 9.RMK TJ AUTH CAN106                                                           
10.RMK CA/PY6SB0                                                                
11.RMK TLWBINSD                                                                +
                                                                               
                                                                                
                                                                                
pat a                                                                          
>PAT:A                                                                          
01 V FARE:CNY460.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:510.00                     
SFC:01   SFN:01                                                               
02 YV FARE:CNY410.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:460.00                    
SFC:02   SFN:02                                                               
03 V FARE:CNY460.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:510.00                     
SFC:03   SFN:03
";

            // tested
            // 测试返回的价格*2的问题
            // 返回结果：
            // {"PassengerList":[{"name":"陈且云","idtype":0,"cardno":"NI372431197602071310","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"JNKP0L","FlightList":[{"FlightNo":"SC1187","Airline":"","Cabin":"Q","SubCabin":"","SCity":"TNA","ECity":"SZX","DepTerminal":"","ArrTerminal":"T3","DepDate":"\/Date(1434943200000+0800)\/","ArrDate":"\/Date(1434953700000+0800)\/","PNRState":"HK1"},{"FlightNo":"SC1188","Airline":"","Cabin":"V","SubCabin":"","SCity":"SZX","ECity":"TNA","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1435130700000+0800)\/","ArrDate":"\/Date(1435140600000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":[],"BigPNR":"ML9TYZ","Mobile":"18810983548","OfficeNo":"TNA138","AdultPnr":null,"PriceList":[{"FacePrice":1980.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2080.00},{"FacePrice":1800.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":1900.00}],"ResultBag":"\r\nRTJNKP0L                                                                       \r\n 1.陈且云 JNKP0L                                                                \r\n 2.  SC1187 Q   MO22JUN  TNASZX HK1   1120 1415          E --T3                 \r\n 3.  SC1188 V   WE24JUN  SZXTNA HK1   1525 1810          E T3--                 \r\n 4.TNA/T TNA/T0635-2112341/LIAOCHENG DONGCHANGFUOU CLOUD TICKETING CO., LTD/LI  \r\n    ZHENGXING ABCDEFG                                                           \r\n 5.TL/1741/12JUN/TNA138                                                         \r\n 6.SSR FOID SC HK1 NI372431197602071310/P1                                      \r\n 7.SSR ADTK 1E BY TNA15JUN15/1541 OR CXL SC1187 Q22JUN                          \r\n 8.OSI SC CTCT18810983548                                                       \r\n 9.OSI SC CTCT15865785000                                                       \r\n10.RMK CA/ML9TYZ                                                                \r\n11.TNA138                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010PAT:A                                                                          \r\n>PAT:A                                                                          \r\n01 Q+V FARE:CNY1980.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2080.00                \r\n\u0010SFC:01   \u0010SFN:01/01   \u0010SFN:01/02                                               \r\n02 Y/SC15U01+Y/SC15U01 FARE:CNY1800.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:1900.00\r\n\u0010SFC:02 \r\n"}
            // {"PassengerList":[{"name":"陈且云","idtype":0,"cardno":"NI372431197602071310","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"JNKP0L","FlightList":[{"FlightNo":"SC1187","Airline":"","Cabin":"Q","SubCabin":"","SCity":"TNA","ECity":"SZX","DepTerminal":"","ArrTerminal":"T3","DepDate":"\/Date(1434943200000+0800)\/","ArrDate":"\/Date(1434953700000+0800)\/","PNRState":"HK1"},{"FlightNo":"SC1188","Airline":"","Cabin":"V","SubCabin":"","SCity":"SZX","ECity":"TNA","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1435130700000+0800)\/","ArrDate":"\/Date(1435140600000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":1,"RMKOfficeNoList":[],"BigPNR":"ML9TYZ","Mobile":"18810983548","OfficeNo":"TNA138","AdultPnr":null,"PriceList":[{"FacePrice":1980.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":2080.00,"Tag":"Q+V"},{"FacePrice":1800.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":1900.00,"Tag":"Y/SC15U01+Y/SC15U01"}],"ResultBag":"\r\nrtJNKP0L                                                                       \r\n 1.陈且云 JNKP0L                                                                \r\n 2.  SC1187 Q   MO22JUN  TNASZX HK1   1120 1415          E --T3                 \r\n 3.  SC1188 V   WE24JUN  SZXTNA HK1   1525 1810          E T3--                 \r\n 4.TNA/T TNA/T0635-2112341/LIAOCHENG DONGCHANGFUOU CLOUD TICKETING CO., LTD/LI  \r\n    ZHENGXING ABCDEFG                                                           \r\n 5.TL/1741/12JUN/TNA138                                                         \r\n 6.SSR FOID SC HK1 NI372431197602071310/P1                                      \r\n 7.SSR ADTK 1E BY TNA15JUN15/1541 OR CXL SC1187 Q22JUN                          \r\n 8.OSI SC CTCT18810983548                                                       \r\n 9.OSI SC CTCT15865785000                                                       \r\n10.RMK CA/ML9TYZ                                                                \r\n11.TNA138                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat:a                                                                          \r\n>PAT:A                                                                          \r\n01 Q+V FARE:CNY1980.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2080.00                \r\n\u0010SFC:01   \u0010SFN:01/01   \u0010SFN:01/02                                               \r\n02 Y/SC15U01+Y/SC15U01 FARE:CNY1800.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:1900.00\r\n\u0010SFC:02 \r\n"}
            //string cmdResult =
            string cmdResult14 =
@"
rtJNKP0L                                                                       
 1.陈且云 JNKP0L                                                                
 2.  SC1187 Q   MO22JUN  TNASZX HK1   1120 1415          E --T3                 
 3.  SC1188 V   WE24JUN  SZXTNA HK1   1525 1810          E T3--                 
 4.TNA/T TNA/T0635-2112341/LIAOCHENG DONGCHANGFUOU CLOUD TICKETING CO., LTD/LI  
    ZHENGXING ABCDEFG                                                           
 5.TL/1741/12JUN/TNA138                                                         
 6.SSR FOID SC HK1 NI372431197602071310/P1                                      
 7.SSR ADTK 1E BY TNA15JUN15/1541 OR CXL SC1187 Q22JUN                          
 8.OSI SC CTCT18810983548                                                       
 9.OSI SC CTCT15865785000                                                       
10.RMK CA/ML9TYZ                                                                
11.TNA138                                                                       
                                                                               
                                                                                
                                                                                
pat:a                                                                          
>PAT:A                                                                          
01 Q+V FARE:CNY1980.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:2080.00                
SFC:01   SFN:01/01   SFN:01/02                                               
02 Y/SC15U01+Y/SC15U01 FARE:CNY1800.00 TAX:CNY100.00 YQ:TEXEMPTYQ  TOTAL:1900.00
SFC:02 
";

            // 测试“部分儿童导入不了”的问题
            // finished tested
            // 返回结果：
            // {"PassengerList":[{"name":"ZI/XING CHD","idtype":0,"cardno":"NI20090123","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1232640000000+0800)\/","TicketNo":""}],"PNR":"HMZFD7","FlightList":[{"FlightNo":"HO1252","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1434235800000+0800)\/","ArrDate":"\/Date(1434243900000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"MZ2P8D","Mobile":"13641601096","OfficeNo":"TPE567","AdultPnr":"JN2FYD","PriceList":[{"FacePrice":620.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":620.00}],"ResultBag":"\r\n\u0010rtHMZFD7                                                                       \r\n 1.ZI/XING CHD HMZFD7                                                           \r\n 2.  HO1252 Y   SU14JUN  PEKSHA HK1   0650 0905          E T3T2                 \r\n 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       \r\n 4.TL/1912/17APR/BJS579                                                         \r\n 5.SSR FOID HO HK1 NI20090123/P1                                                \r\n 6.SSR OTHS HO ADULT PNR IS JN2FYD                                              \r\n 7.SSR ADTK 1E BY TPE17APR15/1912 OR CXL HO1252 Y19APR                          \r\n 8.SSR CHLD HO HK1 23JAN09/P1                                                   \r\n 9.OSI HO CTCT13641601096                                                       \r\n10.RMK CA/MZ2P8D                                                                \r\n11.TPE567                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat:a*ch                                                                       \r\n>PAT:A*CH                                                                       \r\n01 YCH FARE:CNY620.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:620.00                  \r\n\u0010SFC:01   \u0010SFN:01                                                               \r\n\u0010                       \r\n\r\n"}
            // {"PassengerList":[{"name":"ZI/XING CHD","idtype":0,"cardno":"NI20090123","PassType":1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(1232640000000+0800)\/","TicketNo":""}],"PNR":"HMZFD7","FlightList":[{"FlightNo":"HO1252","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1434235800000+0800)\/","ArrDate":"\/Date(1434243900000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"MZ2P8D","Mobile":"13641601096","OfficeNo":"TPE567","AdultPnr":"JN2FYD","PriceList":[{"FacePrice":620.00,"Tax":0.0,"Fuel":0.0,"TotalPrice":620.00,"Tag":"YCH"}],"ResultBag":"\r\n\u0010rtHMZFD7                                                                       \r\n 1.ZI/XING CHD HMZFD7                                                           \r\n 2.  HO1252 Y   SU14JUN  PEKSHA HK1   0650 0905          E T3T2                 \r\n 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       \r\n 4.TL/1912/17APR/BJS579                                                         \r\n 5.SSR FOID HO HK1 NI20090123/P1                                                \r\n 6.SSR OTHS HO ADULT PNR IS JN2FYD                                              \r\n 7.SSR ADTK 1E BY TPE17APR15/1912 OR CXL HO1252 Y19APR                          \r\n 8.SSR CHLD HO HK1 23JAN09/P1                                                   \r\n 9.OSI HO CTCT13641601096                                                       \r\n10.RMK CA/MZ2P8D                                                                \r\n11.TPE567                                                                       \r\n\u0010                                                                               \r\n                                                                                \r\n                                                                                \r\n\u0010pat:a*ch                                                                       \r\n>PAT:A*CH                                                                       \r\n01 YCH FARE:CNY620.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:620.00                  \r\n\u0010SFC:01   \u0010SFN:01                                                               \r\n\u0010                       \r\n\r\n"}
            //string cmdResult =
            string cmdResult15 =
@"
rtHMZFD7                                                                       
 1.ZI/XING CHD HMZFD7                                                           
 2.  HO1252 Y   SU14JUN  PEKSHA HK1   0650 0905          E T3T2                 
 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG       
 4.TL/1912/17APR/BJS579                                                         
 5.SSR FOID HO HK1 NI20090123/P1                                                
 6.SSR OTHS HO ADULT PNR IS JN2FYD                                              
 7.SSR ADTK 1E BY TPE17APR15/1912 OR CXL HO1252 Y19APR                          
 8.SSR CHLD HO HK1 23JAN09/P1                                                   
 9.OSI HO CTCT13641601096                                                       
10.RMK CA/MZ2P8D                                                                
11.TPE567                                                                       
                                                                               
                                                                                
                                                                                
pat:a*ch                                                                       
>PAT:A*CH                                                                       
01 YCH FARE:CNY620.00 TAX:TEXEMPTCN YQ:TEXEMPTYQ  TOTAL:620.00                  
SFC:01   SFN:01                                                               
                       

";

            // 测试成人导入时，返回“解析失败”的问题：
            // finished tested
            // 返回结果：
            // {"PassengerList":[{"name":"朱玉妍","idtype":0,"cardno":"NI652301197211040828","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KTD5BF","FlightList":[{"FlightNo":"CZ6902","Airline":"","Cabin":"L","SubCabin":"","SCity":"PEK","ECity":"URC","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434264000000+0800)\/","ArrDate":"\/Date(1434279600000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA255"],"BigPNR":"PF9PSY","Mobile":"18973277876","OfficeNo":"CSX107","AdultPnr":null,"PriceList":[{"FacePrice":1580.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1630.00}],"ResultBag":"\r\nRTKTD5BF 1.朱玉妍 KTD5BF 2. CZ6902 L SU14JUN PEKURC HK1 1440 1900 E T2T3 3.CSX/T CSX/T0731-58525046/XIANGTAN PING AN BUSINESS SERVICES LIMITED/ZHOU WEI ABCDEFG 4.NA 5.TL/1240/14JUN/CSX107 6.SSR FOID CZ HK1 NI652301197211040828/P1 7.SSR ADTK 1E BY CSX12JUN15/2000 OR CXL CZ BOOKING 8.OSI CZ CTCT18973277876 9.RMK CA/PF9PSY 10.RMK TJ AUTH SHA255 11.CSX107 >PAT:A >PAT:A 01 L FARE:CNY1580.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1630.00 >SFC:01 >SFN:01\r\n"}
            // {"PassengerList":[{"name":"朱玉妍","idtype":0,"cardno":"NI652301197211040828","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KTD5BF","FlightList":[{"FlightNo":"CZ6902","Airline":"","Cabin":"L","SubCabin":"","SCity":"PEK","ECity":"URC","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1434264000000+0800)\/","ArrDate":"\/Date(1434279600000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA255"],"BigPNR":"PF9PSY","Mobile":"18973277876","OfficeNo":"CSX107","AdultPnr":null,"PriceList":[{"FacePrice":1580.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1630.00,"Tag":"L"}],"ResultBag":"\r\nRTKTD5BF 1.朱玉妍 KTD5BF 2. CZ6902 L SU14JUN PEKURC HK1 1440 1900 E T2T3 3.CSX/T CSX/T0731-58525046/XIANGTAN PING AN BUSINESS SERVICES LIMITED/ZHOU WEI ABCDEFG 4.NA 5.TL/1240/14JUN/CSX107 6.SSR FOID CZ HK1 NI652301197211040828/P1 7.SSR ADTK 1E BY CSX12JUN15/2000 OR CXL CZ BOOKING 8.OSI CZ CTCT18973277876 9.RMK CA/PF9PSY 10.RMK TJ AUTH SHA255 11.CSX107 >PAT:A >PAT:A 01 L FARE:CNY1580.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1630.00 >SFC:01 >SFN:01\r\n"}
            string cmdResult =
            //string cmdResult16 =
@"
RTKTD5BF 1.朱玉妍 KTD5BF 2. CZ6902 L SU14JUN PEKURC HK1 1440 1900 E T2T3 3.CSX/T CSX/T0731-58525046/XIANGTAN PING AN BUSINESS SERVICES LIMITED/ZHOU WEI ABCDEFG 4.NA 5.TL/1240/14JUN/CSX107 6.SSR FOID CZ HK1 NI652301197211040828/P1 7.SSR ADTK 1E BY CSX12JUN15/2000 OR CXL CZ BOOKING 8.OSI CZ CTCT18973277876 9.RMK CA/PF9PSY 10.RMK TJ AUTH SHA255 11.CSX107 >PAT:A >PAT:A 01 L FARE:CNY1580.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1630.00 >SFC:01 >SFN:01
";

            //cmdResult = cmdResult.ToUpper();

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }

        // 若为公务员，测试价格是否都显示出来
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult104()
        {
            // 返回结果：
            // {"PassengerList":[{"name":"林忠秀","idtype":0,"cardno":"NI230712198809180521","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HV7YGQ","FlightList":[{"FlightNo":"CA1831","Airline":"","Cabin":"Y","SubCabin":"","SCity":"PEK","ECity":"SHA","DepTerminal":"T3","ArrTerminal":"T2","DepDate":"\/Date(1450308600000+0800)\/","ArrDate":"\/Date(1450316400000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["BJS415"],"BigPNR":"MXWKSK","Mobile":"13641601096","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":1240.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1290.00,"Tag":"Y"},{"FacePrice":1090.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1140.00,"Tag":"Y88GP"}],"ResultBag":"\r\nrtHV7YGQ \r\n 1.林忠秀 HV7YGQ \r\n 2. CA1831 Y TH17DEC PEKSHA HK1 0730 0940 E T3T2 \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG \r\n     ABCDEFG \r\n 4.TL/1442/01JUL/SHA888 \r\n 5.SSR FOID CA HK1 NI230712198809180521/P1 \r\n 6.SSR ADTK 1E BY SHA01JUL15/1942 OR CXL CA ALL SEGS \r\n 7.OSI CA CTCT13641601096 \r\n 8.RMK TJ AUTH BJS415 \r\n 9.RMK CA/MXWKSK \r\n10.SHA243 \r\n                                                                                \r\n                                                                                 \r\n                                                                                 \r\nPAT:A#CGP/CC \r\n>PAT:A#CGP/CC \r\n01 Y FARE:CNY1240.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1290.00 \r\nSFC:01 SFN:01 \r\n02 Y88GP FARE:CNY1090.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1140.00 \r\nSFC:02 SFN:02 \r\n\r\n"}
            string cmdResult =
@"
rtHV7YGQ 
 1.林忠秀 HV7YGQ 
 2. CA1831 Y TH17DEC PEKSHA HK1 0730 0940 E T3T2 
 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG 
     ABCDEFG 
 4.TL/1442/01JUL/SHA888 
 5.SSR FOID CA HK1 NI230712198809180521/P1 
 6.SSR ADTK 1E BY SHA01JUL15/1942 OR CXL CA ALL SEGS 
 7.OSI CA CTCT13641601096 
 8.RMK TJ AUTH BJS415 
 9.RMK CA/MXWKSK 
10.SHA243 
                                                                                
                                                                                 
                                                                                 
PAT:A#CGP/CC 
>PAT:A#CGP/CC 
01 Y FARE:CNY1240.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1290.00 
SFC:01 SFN:01 
02 Y88GP FARE:CNY1090.00 TAX:CNY50.00 YQ:TEXEMPTYQ TOTAL:1140.00 
SFC:02 SFN:02 

";

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            Console.ReadLine();
        }

        // 2015-09-22（星期二）测试：测试是否能解析出PhoneNo
        [TestMethod]
        public void Test_SeekPNR_ParseCmdResult105()
        {
            // 返回结果：
            // {"PassengerList":[{"name":"王善","idtype":0,"cardno":"NI51032119721218050X","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"JV01ND","FlightList":[{"FlightNo":"ZH9557","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CGQ","ECity":"SZX","DepTerminal":"","ArrTerminal":"T3","DepDate":"\/Date(1443570900000+0800)\/","ArrDate":"\/Date(1443591900000+0800)\/","PNRState":"HK1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["BJS140"],"BigPNR":"NWK6H5","Mobile":"13917736789","PhoneNo":"02160727777","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[],"ResultBag":"\r\n 1.王善 JV01ND                                                                   \r\n 2.  ZH9557 Y   WE30SEP  CGQSZX HK1   0755 1345          E --T3                \r\n 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     \r\n     ABCDEFG                                                                   \r\n 4.TL/1605/22SEP/SHA888                                                        \r\n 5.SSR FOID ZH HK1 NI51032119721218050X/P1                                     \r\n 6.SSR ADTK 1E BY SHA22SEP15/2308 OR CXL ZH9557 Y30SEP                         \r\n 7.OSI ZH CTCT13917736789                                                      \r\n 8.OSI ZH CTCT02160727777                                                      \r\n 9.RMK TJ AUTH BJS140                                                          \r\n10.RMK CA/NWK6H5                                                               \r\n11.SHA243\r\n"}
            string cmdResult =
@"
 1.王善 JV01ND                                                                   
 2.  ZH9557 Y   WE30SEP  CGQSZX HK1   0755 1345          E --T3                
 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     
     ABCDEFG                                                                   
 4.TL/1605/22SEP/SHA888                                                        
 5.SSR FOID ZH HK1 NI51032119721218050X/P1                                     
 6.SSR ADTK 1E BY SHA22SEP15/2308 OR CXL ZH9557 Y30SEP                         
 7.OSI ZH CTCT13917736789                                                      
 8.OSI ZH CTCT02160727777                                                      
 9.RMK TJ AUTH BJS140                                                          
10.RMK CA/NWK6H5                                                               
11.SHA243
";

            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNR.ParseCmdResult(cmdResult);

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }
    }
}
