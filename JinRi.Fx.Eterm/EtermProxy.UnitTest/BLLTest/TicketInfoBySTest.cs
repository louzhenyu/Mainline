using JetermEntity.Request;
using JetermEntity.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EtermProxy.UnitTest
{
    [TestClass]
    public class TicketInfoBySTest
    {
        #region Enum Value

        ///// <summary>
        ///// 客票状态类型
        ///// </summary>
        //public enum TicketStatus
        //{
        //    NotSet,
        //    OPEN_FOR_USE,
        //    VOID,
        //    REFUNDED,
        //    CHECKED_IN,
        //    USED_FLOWN,
        //    SUSPENDED,
        //    PRINT_EXCH,
        //    EXCHANGED,
        //    LIFT_BOARDED,
        //    FIM_EXCH,
        //    AIRP_CNTL_YY,
        //    CPN_NOTE,
        //    USED_CLOSED
        //}

        #endregion

        [TestMethod]
        public void Test_BusinessDispose()
        {
#warning 有几点未测到：1、当返回结果中DEP这行，包含有OPEN的，测是否能分别解析到正确的航司和舱位信息；2、当返回结果中DEP这行，不含有OPEN，且仓位位于第7个位置，而不是第6个位置。测当遇到此种情况时，是否能解析到正确的仓位
            JetermEntity.Request.TicketInfoByS request = new JetermEntity.Request.TicketInfoByS();        
            request.TicketNo = "784-2158602564";
            //request.TicketNo = "7842133192747";
            //request.TicketNo = "7842130024027";

            EtermProxy.BLL.TicketInfoByS getTickInfo = new EtermProxy.BLL.TicketInfoByS(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.TicketInfoByS> result = getTickInfo.BusinessDispose(request);            
            
            if (result != null && result.state)
            {
                // 运行结果，如：
                // {"TicketNo":"7842158602564","PassengerName":"张细志","Airline":"CZ","Price":{"FacePrice":640.00,"TotalPrice":750.00,"Tax":50.00,"Fuel":60.00},"SCity":"CSX","ECity":"CTU","Cabin":"U"}
                Console.WriteLine("运行结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
                return;
            }
            if (result.error != null)
            {
                Console.WriteLine("运行错误，错误信息：" + result.error.ErrorMessage);
            }            
        }

        [TestMethod]
        public void Test_TicketInfoBySDemo_ParseCmdResult1()
        {
            //JetermEntity.Request.TicketInfoByS request = new JetermEntity.Request.TicketInfoByS();

            JetermEntity.Parser.TicketInfoByS ticketInfoByS = new JetermEntity.Parser.TicketInfoByS();

            // 解析结果：{"TicketNo":"7842158602564","PassengerName":"张细志","Price":{"FacePrice":640.00,"Tax":50.00,"Fuel":60.00,"TotalPrice":750.00},"FlightList":[{"FlightNo":"CZ3461","Airline":"CZ","Cabin":"U","SCity":"CSX","ECity":"CTU","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1451111100000+0800)\/","ArrDate":"\/Date(1451059200000+0800)\/"}],"TicketStatus":13}
            //string cmdResult =
            string cmdResult11 =
@"
                                                                      26DEC14   
航空公司电子客票航程通知单                                                                  
电子客票票号       784-2158602564                                                    
后续客票号         NONE                                                             
出票航空公司       MIS CAAC                                                          
售票处信息         CHINA SOUTHERN AIRLINES WEB                                      
出票时间/地点      17DEC14/GUANGZHOU(11)<08685898>                                   
旅客姓名           张细志                                                             
身份识别号码       NONE                                                              
票价    货币       CNY  金额   640.00                                                
实付等值货币       CNY  金额   640.00    付款方式 CC                                       
税款   CNY 50.00CN  CNY 60.00YQ  CNY EXEMPTXT                                    +
                                                                               

■pn
付款总额           CNY  750.00                                                   -  
                                                                               
使用限制        BUDEQIANZHUAN不得签转/BIANGENGTUIPIAOSHOUFEI变更退票收洓溓┳靶畔皼潧鈲
瓲经停 起飞城市机场         日期  星期 时间 航班 舱位                    
    DEP  CSX-CHANGSHA      26DEC FRI 1425 CZ3461   UU          26DEC  26DEC 20K 
经停 到达城市机场         日期  星期 时间 机型 订座                                              
    ARR  CTU-CHENGDU       26DEC FRI      (320)    CONFIRMED                   
订座记录编号 HWP8PF/1E           7Q'FW4L,  USED/CLOSED                             
------------------------------------------------------------------------------- 
.                                                                              
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   +

■pn
.                                                                              -
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   
------------------------------------------------------------------------------- 
.                                                                              
 张细志 CSX CTU CZ3461  U 26DEC  784-2158602564      CNY  750.00                  
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   
.                                                                              
 张细志 CSX CTU CZ3461  U 26DEC  784-2158602564      CNY  750.00                  
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   
.                                                                              
.                          --- WWW.TRAVELSKY.COM ---                           +

■pn
.                                                                              -
";

            // 解析结果：{"TicketNo":"9122340227002","PassengerName":"沈含笑","Price":{"FacePrice":360.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":410.00},"FlightList":[{"FlightNo":"QW9792","Airline":"QW","Cabin":"Z","SCity":"HGH","ECity":"TAO","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1430898600000+0800)\/","ArrDate":"\/Date(1430905200000+0800)\/"}],"TicketStatus":1}
            //string cmdResult =
            string cmdResult22 =
@"
                                                                      05MAY15   
航空公司电子客票航程通知单                                                                  
电子客票票号       912-2340227002                                                    
后续客票号         NONE                                                             
出票航空公司       AIRLINE                                                           
售票处信息         QINGDAO                                                          
                   TAO104                                                      
                   DEV-02                                                      
出票时间/地点      04MAY15/QINGDAO(104)<08695009>                                    
旅客姓名           沈含笑                                                             
身份识别号码       NI330283199508276729                                              
票价    货币       CNY  金额   360.00                                                +
                                                                               

实付等值货币       CNY  金额   360.00    付款方式 CC                                 -     
税款           CNY 50.00CN   EXEMPTYQ                                            
付款总额           CNY  410.00                                                     
使用限制        不得签转                                                               
签注信息                                                                           
------------------------------------------------------------------------------- 
经停 起飞城市机场         日期  星期 时间 航班 舱位                                              
    DEP  HGH-HANGZHOU      06MAY WED 1550 QW9792   ZZ                       20K 
经停 到达城市机场         日期  星期 时间 机型 订座                                              
    ARR  TAO-QINGDAO       06MAY WED 1740 (320)    CONFIRMED                   
订座记录编号                     客票状态  OPEN FOR USE                                  
-------------------------------------------------------------------------------+

.                                                                              -
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   
.                                                                              
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   
------------------------------------------------------------------------------- 
.                                                                              
 沈含笑 HGH TAO QW9792  Z 06MAY  912-2340227002      CNY  410.00                  
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   
.                                                                              
 沈含笑 HGH TAO QW9792  Z 06MAY  912-2340227002      CNY  410.00                  
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   +

.                                                                              -
.                          --- WWW.TRAVELSKY.COM ---                           
. 
";
            // 解析结果：{"TicketNo":"7312393853675","PassengerName":"张毛亚","Price":{"FacePrice":780.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":830.00},"FlightList":[{"FlightNo":"MF8870","Airline":"MF","Cabin":"K","SCity":"XUZ","ECity":"FOC","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1424874300000+0800)\/","ArrDate":"\/Date(1424880600000+0800)\/"}],"TicketStatus":1}
            //string cmdResult =
            string cmdResult33 =
@"
detr tn/731-2393853675,s                                                       
                                                                      10FEB15   
航空公司电子客票航程通知单                                                      
电子客票票号       731-2393853675                                               
后续客票号         NONE                                                         
出票航空公司       AIRLINE                                                      
售票处信息         MF                                                           
                   XMN042                                                       
                   DEV-07                                                       
出票时间/地点      09FEB15/XIAMEN(42)<08673111>                                 
旅客姓名           张毛亚                                                       
身份识别号码       NI41148119891006127X                                         
票价    货币       CNY  金额   780.00                                           
     +                                                                          
                                                                               
                                                                                
                                                                                
pn                                                                             
实付等值货币       CNY  金额   780.00    付款方式 CA                            
     -                                                                          
税款           CNY 50.00CN   EXEMPTYQ                                           
付款总额           CNY  830.00                                                  
使用限制        变更退票收费不得签转                                            
签注信息                                                                        
------------------------------------------------------------------------------- 
经停 起飞城市机场         日期  星期 时间 航班 舱位                             
    DEP  XUZ-XUZHOU        25FEB WED 2225 MF8870   KK                 09FEB 20K 
经停 到达城市机场         日期  星期 时间 机型 订座                             
    ARR  FOC-FUZHOU        26FEB THU 0010 (737)    CONFIRMED                    
订座记录编号                     客票状态  OPEN FOR USE                         
-------------------------------------------------------------------------------+
                                                                               
                                                                                
                                                                                
pn                                                                             
.                                                                              -
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                    
.                                                                               
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                    
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                    
------------------------------------------------------------------------------- 
.                                                                               
 张毛亚 XUZ FOC MF8870  K 25FEB  731-2393853675      CNY  830.00                
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                    
.                                                                               
 张毛亚 XUZ FOC MF8870  K 25FEB  731-2393853675      CNY  830.00                
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   +
                                                                               
                                                                                
                                                                                
.                                                                              -
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                    
.                                                                               
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                    
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                    
------------------------------------------------------------------------------- 
.                                                                               
 张毛亚 XUZ FOC MF8870  K 25FEB  731-2393853675      CNY  830.00                
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                    
.                                                                               
 张毛亚 XUZ FOC MF8870  K 25FEB  731-2393853675      CNY  830.00                
VOID VOID VOID VOID VOID VOID VOID VOID VOID                                   +
           
";
            // 解析结果：{"TicketNo":"7846762540170","PassengerName":"齐峰","Price":{"FacePrice":3980.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":4080.00},"FlightList":[{"FlightNo":"CZ6337","Airline":"CZ","Cabin":"H","SCity":"DLC","ECity":"HAK","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1424475900000+0800)\/","ArrDate":"\/Date(1424448000000+0800)\/"},{"FlightNo":"CZ8334","Airline":"CZ","Cabin":"Y","SCity":"HAK","ECity":"DLC","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1424865600000+0800)\/","ArrDate":"\/Date(1424793600000+0800)\/"}],"TicketStatus":1}
            string cmdResult =
            //string cmdResult44 =
@"
detr tn/784-6762540170,s                                                       
                                                                                
                                                                      10FEB15   
航空公司电子客票航程通知单                                                      
电子客票票号       784-6762540170                                               
后续客票号         NONE                                                         
出票航空公司       MIS CAAC                                                     
售票处信息         CHINA SOUTHERN AIRLINES                                      
出票时间/地点      09FEB15/SHANGHAI(666)<08039721>                              
旅客姓名           齐峰                                                         
身份识别号码       NONE                                                         
票价    货币       CNY  金额   3980.00                                          
实付等值货币       CNY  金额   3980.00   付款方式 CA CASH(CNY)                  
税款           CNY 100.00CN  CNY EXEMPTYQ                                       
 +                                                                              
                                                                               
                                                                                
                                                                                
pn                                                                             
付款总额           CNY 4080.00                                                  
 -                                                                              
使用限制        不得签转/变更退票收费                                           
签注信息                                                                        
------------------------------------------------------------------------------- 
经停 起飞城市机场         日期  星期 时间 航班 舱位                             
    DEP  DLC-DALIAN        21FEB SAT 0745 CZ6337   HYRT70      21FEB  21FEB 20K 
经停 到达城市机场         日期  星期 时间 机型 订座                             
    ARR  HAK-HAIKOU        21FEB SAT      (320)    CONFIRMED de                 
订座记录编号 HRN1PD/1E           客票状态  OPEN FOR USE                         
------------------------------------------------------------------------------- 
经停 起飞城市机场         日期  星期 时间 航班 舱位                             
    DEP  HAK-HAIKOU        25FEB WED 2000 CZ8334   YYRT95      25FEB  25FEB 20K+
                                                                               
                                                                                
                                                                                
pn                                                                             
经停 到达城市机场         日期  星期 时间 机型 订座                             
            -*{                                                                 
    ARR  DLC-DALIAN        25FEB WED      (320)    CONFIRMED                    
订座记录编号 HRN1PD/1E           客票状态  OPEN FOR USE                         
------------------------------------------------------------------------------- 
.                                                                               
 齐峰     DLC HAK CZ6337  H 21FEB  784-6762540170      CNY 4080.00              
 齐峰     HAK DLC CZ8334  Y 25FEB  784-6762540170      CNY 4080.00              
.                                                                               
 齐峰     DLC HAK CZ6337  H 21FEB  784-6762540170      CNY 4080.00              
 齐峰     HAK DLC CZ8334  Y 25FEB  784-6762540170      CNY 4080.00              
.                                                                               
.                          --- WWW.TRAVELSKY.COM ---                           +
                                                                               
                                                                                
                                                                                
                                                                               
                                                                                
pn                                                                              
.                                                                              -
              
";

            CommandResult<JetermEntity.Response.TicketInfoByS> result = ticketInfoByS.ParseCmdResult(cmdResult);

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
    
        [TestMethod]
        public void Test_EtermProxy_1000()
        {
            string strPost = "{\"ClassName\" : \"TicketInfoByS\", \"Config\" : \"O77124B1\",  \"OfficeNo\" : \"SHA243\" }";
            //string ss = "{\"FlightList\":[{\"FlightNo\":\"MU5137\",\"Cabin\":\"H\",\"SCity\":\"SHA\",\"ECity\":\"PEK\",\"DepDate\":\"\\/Date(1430064000000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"MU5156\",\"Cabin\":\"B\",\"SCity\":\"PEK\",\"ECity\":\"SHA\",\"DepDate\":\"\\/Date(1430323200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"干园\",\"idtype\":0,\"cardno\":\"650121199412242866\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"},{\"name\":\"张杰\",\"idtype\":0,\"cardno\":\"140525198401186312\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"13472634765\",\"RMKOfficeNoList\":[],\"RMKRemark\":null,\"Pnr\":null}";
            // 返回结果：{"state":true,"error":null,"config":"O77124B1","OfficeNo":"SHA243","result":{"TicketNo":"9122340227002","PassengerName":"沈含笑","Price":{"FacePrice":360.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":410.00},"FlightList":[{"FlightNo":"QW9792","Airline":"QW","Cabin":"Z","SCity":"HGH","ECity":"TAO","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1430898600000+0800)\/","ArrDate":"\/Date(1430905200000+0800)\/"}],"TicketStatus":13},"reqtime":"\/Date(1430894442030+0800)\/","SaveTime":1800}
            //string ss = "{\"TicketNo\":\"912-2340227002\"}";
            // 返回结果：{"state":true,"error":null,"config":"O77124B1","OfficeNo":"SHA243","result":{"TicketNo":"9122340227002","PassengerName":"沈含笑","Price":{"FacePrice":360.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":410.00},"FlightList":[{"FlightNo":"QW9792","Airline":"QW","Cabin":"Z","SCity":"HGH","ECity":"TAO","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1430898600000+0800)\/","ArrDate":"\/Date(1430905200000+0800)\/"}],"TicketStatus":13},"reqtime":"\/Date(1430893082366+0800)\/","SaveTime":1800}
            //string ss = "{\"TicketNo\":\"9122340227002\"}";
            // 返回结果：{"state":true,"error":null,"config":"O77124B1","OfficeNo":"SHA243","result":{"TicketNo":"7846762540170","PassengerName":"齐峰","Price":{"FacePrice":3980.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":4080.00},"FlightList":[{"FlightNo":"CZ6337","Airline":"CZ","Cabin":"H","SCity":"DLC","ECity":"HAK","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1424475900000+0800)\/","ArrDate":"\/Date(1424448000000+0800)\/"},{"FlightNo":"CZ8334","Airline":"CZ","Cabin":"Y","SCity":"HAK","ECity":"DLC","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1424865600000+0800)\/","ArrDate":"\/Date(1424793600000+0800)\/"}],"TicketStatus":13},"reqtime":"\/Date(1430893724259+0800)\/","SaveTime":1800}
            //string ss = "{\"TicketNo\":\"784-6762540170\"}";
            // 返回结果：{"state":true,"error":null,"config":"O77124B1","OfficeNo":"SHA243","result":{"TicketNo":"7842158602564","PassengerName":"张细志","Price":{"FacePrice":640.00,"Tax":50.00,"Fuel":60.00,"TotalPrice":750.00},"FlightList":[{"FlightNo":"CZ3461","Airline":"CZ","Cabin":"U","SCity":"CSX","ECity":"CTU","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1451111100000+0800)\/","ArrDate":"\/Date(1451059200000+0800)\/"}],"TicketStatus":13},"reqtime":"\/Date(1430895806350+0800)\/","SaveTime":1800}
            string ss = "{\"TicketNo\":\"784-2158602564\"}";           

            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, ss);
            Console.WriteLine("返回结果：" + sret);
        }
    }
}
