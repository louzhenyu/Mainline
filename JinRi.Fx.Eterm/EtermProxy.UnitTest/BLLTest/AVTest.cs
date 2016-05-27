using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using JetermEntity.Response;

namespace EtermProxy.UnitTest.BLLTest
{
    [TestClass]
    public class AVTest
    {
        [TestMethod]
        public void AVTest_ParseCmdResult1()
        {
            JetermEntity.Request.AV request = new JetermEntity.Request.AV();

            // 没有共享航班的，即没有OPE（有2个航线）
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"MU2325","DepDate":"\/Date(1444320000000+0800)\/","TotalJourneyTime":"4:30","AVList":[{"SCity":"LHW","ECity":"XIY","STime":"0740","ETime":"0840","EWeek":"FRI","FltDuration":"1:00","Ground":"1:00","STerminal":"T2","ETerminal":"T3","FlightModel":"320","Meal":null,"Distance":"518","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"4","NumStr":"4"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"2","NumStr":"2"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"5","NumStr":"5"},{"Cabin":"E","NumTag":"5","NumStr":"5"},{"Cabin":"H","NumTag":"5","NumStr":"5"},{"Cabin":"K","NumTag":"5","NumStr":"5"},{"Cabin":"L","NumTag":"5","NumStr":"5"},{"Cabin":"N","NumTag":"5","NumStr":"5"},{"Cabin":"R","NumTag":"5","NumStr":"5"},{"Cabin":"S","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"G","NumTag":"S","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"Q","NumTag":"Q","NumStr":"0"}]},{"SCity":"XIY","ECity":"SZX","STime":"0940","ETime":"1210","EWeek":"FRI","FltDuration":"2:30","Ground":null,"STerminal":"T3","ETerminal":"T3","FlightModel":"320","Meal":"L","Distance":"1402","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"4","NumStr":"4"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"2","NumStr":"2"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"S","NumStr":"0"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"S","NumStr":"0"},{"Cabin":"E","NumTag":"S","NumStr":"0"},{"Cabin":"H","NumTag":"S","NumStr":"0"},{"Cabin":"K","NumTag":"S","NumStr":"0"},{"Cabin":"L","NumTag":"S","NumStr":"0"},{"Cabin":"N","NumTag":"S","NumStr":"0"},{"Cabin":"R","NumTag":"S","NumStr":"0"},{"Cabin":"S","NumTag":"S","NumStr":"0"},{"Cabin":"V","NumTag":"S","NumStr":"0"},{"Cabin":"T","NumTag":"S","NumStr":"0"},{"Cabin":"G","NumTag":"S","NumStr":"0"},{"Cabin":"Z","NumTag":"S","NumStr":"0"},{"Cabin":"Q","NumTag":"S","NumStr":"0"}]}],"ResultBag":"\r\n AV:MU2325/09OCT                                                                \r\nDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE               \r\nLHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518                    \r\nXIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402                   \r\nTOTAL JOURNEY TIME   4:30                                                       \r\nLHWXIY UC F4 PQ J2 CQ DQ IQ WQ YA BA M5 E5 H5 K5 L5 N5 R5 SQ VQ                 \r\n       TQ GS ZQ QQ                                                              \r\nXIYSZX UC F4 PQ J2 CQ DQ IQ WQ YS BA MS ES HS KS LS NS RS SS VS                 \r\n       TS GS ZS QS                                                              \r\nMEMBER OF SKYTEAM\r\n"},"reqtime":"\/Date(1440404801177+0800)\/","SaveTime":1800,"ServerUrl":null}
            //request.FlightNo = "MU2325";
            //request.DepDate = Convert.ToDateTime("2015-10-09");
            //// 查询条件：            
            ////request.SCity = "XIY";
            ////request.ECity = "SZX";
            ////request.Carbin = "F"; 
            //string cmdResult =
            string cmdResult1 =
@"
 AV:MU2325/09OCT                                                                
DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE               
LHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518                    
XIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402                   
TOTAL JOURNEY TIME   4:30                                                       
LHWXIY UC F4 PQ J2 CQ DQ IQ WQ YA BA M5 E5 H5 K5 L5 N5 R5 SQ VQ                 
       TQ GS ZQ QQ                                                              
XIYSZX UC F4 PQ J2 CQ DQ IQ WQ YS BA MS ES HS KS LS NS RS SS VS                 
       TS GS ZS QS                                                              
MEMBER OF SKYTEAM
";

            // 没有共享航班的，即没有OPE（只有1个航线）
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"FM9220","DepDate":"\/Date(1471276800000+0800)\/","TotalJourneyTime":"4:40","AVList":[{"SCity":"URC","ECity":"SHA","STime":"0930","ETime":"1410","EWeek":"SAT","FltDuration":"4:40","Ground":null,"STerminal":"T2","ETerminal":"T2","FlightModel":"738","Meal":null,"Distance":"3271","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"8","NumStr":"8"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"C","NumStr":"0"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"Q","NumStr":"0"},{"Cabin":"E","NumTag":"Q","NumStr":"0"},{"Cabin":"H","NumTag":"Q","NumStr":"0"},{"Cabin":"K","NumTag":"Q","NumStr":"0"},{"Cabin":"L","NumTag":"Q","NumStr":"0"},{"Cabin":"N","NumTag":"Q","NumStr":"0"},{"Cabin":"R","NumTag":"Q","NumStr":"0"},{"Cabin":"S","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"G","NumTag":"Q","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"Q","NumTag":"Q","NumStr":"0"}]}],"ResultBag":"\r\n AV:FM9220/20AUG16                                                              \r\nDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE               \r\nURC 0930   SHA 1410   SAT  4:40         T2/T2 738        3271                   \r\nTOTAL JOURNEY TIME   4:40                                                       \r\nURCSHA UC F8 PQ JC CQ DQ IQ WQ YA BA MQ EQ HQ KQ LQ NQ RQ SQ VQ                 \r\n       TQ GQ ZQ QQ                                                              \r\nMEMBER OF SKYTEAM\r\n"},"reqtime":"\/Date(1440415178146+0800)\/","SaveTime":1800,"ServerUrl":null}
            //request.FlightNo = "FM9220";
            //request.DepDate = Convert.ToDateTime("2016-08-16");
            ////// 查询条件：            
            ////request.SCity = "URC";
            ////request.ECity = "SHA";
            ////request.Carbin = "G"; 
            //string cmdResult =
            string cmdResult222 =
@"
 AV:FM9220/20AUG16                                                              
DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE               
URC 0930   SHA 1410   SAT  4:40         T2/T2 738        3271                   
TOTAL JOURNEY TIME   4:40                                                       
URCSHA UC F8 PQ JC CQ DQ IQ WQ YA BA MQ EQ HQ KQ LQ NQ RQ SQ VQ                 
       TQ GQ ZQ QQ                                                              
MEMBER OF SKYTEAM
";

            // 有共享航班的，即有OPE（有2个航线）
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"CZ9104","DepDate":"\/Date(1444320000000+0800)\/","TotalJourneyTime":"4:30","AVList":[{"SCity":"LHW","ECity":"XIY","STime":"0740","ETime":"0840","EWeek":"FRI","FltDuration":"1:00","Ground":"1:00","STerminal":"T2","ETerminal":"T3","FlightModel":"320","Meal":null,"Distance":"518","ShareFlight":true,"ShareFltNo":"MU2325","CarbinNumList":[{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"5","NumStr":"5"},{"Cabin":"M","NumTag":"5","NumStr":"5"},{"Cabin":"U","NumTag":"5","NumStr":"5"},{"Cabin":"L","NumTag":"5","NumStr":"5"},{"Cabin":"E","NumTag":"5","NumStr":"5"}]},{"SCity":"XIY","ECity":"SZX","STime":"0940","ETime":"1210","EWeek":"FRI","FltDuration":"2:30","Ground":null,"STerminal":"T3","ETerminal":"T3","FlightModel":"320","Meal":"L","Distance":"1402","ShareFlight":true,"ShareFltNo":"MU2325","CarbinNumList":[{"Cabin":"Y","NumTag":"S","NumStr":"0"},{"Cabin":"B","NumTag":"S","NumStr":"0"},{"Cabin":"M","NumTag":"S","NumStr":"0"},{"Cabin":"U","NumTag":"S","NumStr":"0"},{"Cabin":"L","NumTag":"S","NumStr":"0"},{"Cabin":"E","NumTag":"S","NumStr":"0"}]}],"ResultBag":"\r\n AV:CZ9104/09OCT                                                                \r\nDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE OPE           \r\nLHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518      MU2325        \r\nXIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402     MU2325        \r\nTOTAL JOURNEY TIME   4:30                                                       \r\nLHWXIY YA B5 M5 U5 L5 E5                                                        \r\nXIYSZX YS BS MS US LS ES                                                        \r\nMEMBER OF SKYTEAM\r\n"},"reqtime":"\/Date(1440415394141+0800)\/","SaveTime":1800,"ServerUrl":null}
            //request.FlightNo = "CZ9104";
            //request.DepDate = Convert.ToDateTime("2015-10-09");
            ////// 查询条件：            
            ////request.SCity = "LHW";
            ////request.ECity = "XIY";
            ////request.Carbin = "E";
            //string cmdResult =
            string cmdResult3 =
@"
 AV:CZ9104/09OCT                                                                
DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE OPE           
LHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518      MU2325        
XIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402     MU2325        
TOTAL JOURNEY TIME   4:30                                                       
LHWXIY YA B5 M5 U5 L5 E5                                                        
XIYSZX YS BS MS US LS ES                                                        
MEMBER OF SKYTEAM
";

            // 有共享航班的，即有OPE（只有1个航线）            
            // string cmdResult =
            string cmdResult4 =
@"";

            // 没有DISTANCE（有2个航线）
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"GS7544","DepDate":"\/Date(1444320000000+0800)\/","TotalJourneyTime":"2:00","AVList":[{"SCity":"LHW","ECity":"IQN","STime":"1450","ETime":"1530","EWeek":"FRI","FltDuration":"0:40","Ground":"0:45","STerminal":"T2","ETerminal":"T1","FlightModel":"190","Meal":null,"Distance":null,"ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"Q","NumStr":"0"},{"Cabin":"H","NumTag":"Q","NumStr":"0"},{"Cabin":"K","NumTag":"Q","NumStr":"0"},{"Cabin":"L","NumTag":"Q","NumStr":"0"},{"Cabin":"M","NumTag":"Q","NumStr":"0"},{"Cabin":"Q","NumTag":"Q","NumStr":"0"},{"Cabin":"X","NumTag":"Q","NumStr":"0"},{"Cabin":"U","NumTag":"Q","NumStr":"0"},{"Cabin":"E","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"Q","NumStr":"0"},{"Cabin":"R","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"O","NumTag":"Q","NumStr":"0"},{"Cabin":"S","NumTag":"2","NumStr":"2"},{"Cabin":"G","NumTag":"Q","NumStr":"0"}]},{"SCity":"IQN","ECity":"XIY","STime":"1615","ETime":"1650","EWeek":"FRI","FltDuration":"0:35","Ground":null,"STerminal":"T1","ETerminal":"T2","FlightModel":"190","Meal":null,"Distance":null,"ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"Q","NumStr":"0"},{"Cabin":"H","NumTag":"Q","NumStr":"0"},{"Cabin":"K","NumTag":"Q","NumStr":"0"},{"Cabin":"L","NumTag":"Q","NumStr":"0"},{"Cabin":"M","NumTag":"Q","NumStr":"0"},{"Cabin":"Q","NumTag":"Q","NumStr":"0"},{"Cabin":"X","NumTag":"Q","NumStr":"0"},{"Cabin":"U","NumTag":"Q","NumStr":"0"},{"Cabin":"E","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"Q","NumStr":"0"},{"Cabin":"R","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"O","NumTag":"Q","NumStr":"0"},{"Cabin":"S","NumTag":"2","NumStr":"2"},{"Cabin":"G","NumTag":"Q","NumStr":"0"}]}],"ResultBag":"\r\n AV:GS7544/09OCT                                                                \r\nDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL                         \r\nLHW 1450   IQN 1530   FRI  0:40  0:45   T2/T1 190                               \r\nIQN 1615   XIY 1650   FRI  0:35         T1/T2 190                               \r\nTOTAL JOURNEY TIME   2:00                                                       \r\nLHWIQN YA BQ HQ KQ LQ MQ QQ XQ UQ EQ TQ ZQ VQ RQ WQ JQ DQ OQ S2                 \r\n       GQ                                                                       \r\nIQNXIY YA BQ HQ KQ LQ MQ QQ XQ UQ EQ TQ ZQ VQ RQ WQ JQ DQ OQ S2                 \r\n       GQ\r\n"},"reqtime":"\/Date(1440415617502+0800)\/","SaveTime":1800,"ServerUrl":null}
            //request.FlightNo = "GS7544";
            //request.DepDate = Convert.ToDateTime("2015-10-09");
            ////// 查询条件：            
            ////request.SCity = "IQN";
            ////request.ECity = "XIY";
            ////request.Carbin = "S";
            //string cmdResult =
            string cmdResult5 =
@"
 AV:GS7544/09OCT                                                                
DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL                         
LHW 1450   IQN 1530   FRI  0:40  0:45   T2/T1 190                               
IQN 1615   XIY 1650   FRI  0:35         T1/T2 190                               
TOTAL JOURNEY TIME   2:00                                                       
LHWIQN YA BQ HQ KQ LQ MQ QQ XQ UQ EQ TQ ZQ VQ RQ WQ JQ DQ OQ S2                 
       GQ                                                                       
IQNXIY YA BQ HQ KQ LQ MQ QQ XQ UQ EQ TQ ZQ VQ RQ WQ JQ DQ OQ S2                 
       GQ
";

            // （不测，因为没有找到测试案例）没有DISTANCE（只有1个航线）
            // string cmdResult =
            string cmdResult6 =
@"";

            // 一个舱位都没有（有2个航线）
            // 返回结果：很抱歉，指令返回结果中没有显示舱位可订数
            // {"state":false,"error":{"ErrorCode":82,"ErrorMessage":"很抱歉，指令返回结果中没有显示舱位可订数","CmdResultBag":"\r\n AV:MU2325/24AUG                                                               \r\nDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              \r\nLHW 0740   XIY 0840   THU  1:00  1:00   T2/T3 320        518                   \r\nXIY 0940   SZX 1210   THU  2:30         T3/T3 320  L     1402                  \r\nTOTAL JOURNEY TIME   4:30                                                      \r\nLHWXIY                                                                         \r\nXIYSZX                                                                         \r\nMEMBER OF SKYTEAM\r\n"},"config":"","OfficeNo":"","result":{"FlightNo":"MU2325","DepDate":"\/Date(1440345600000+0800)\/","TotalJourneyTime":"4:30","AVList":[{"SCity":"LHW","ECity":"XIY","STime":"0740","ETime":"0840","EWeek":"THU","FltDuration":"1:00","Ground":"1:00","STerminal":"T2","ETerminal":"T3","FlightModel":"320","Meal":null,"Distance":"518","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[]},{"SCity":"XIY","ECity":"SZX","STime":"0940","ETime":"1210","EWeek":"THU","FltDuration":"2:30","Ground":null,"STerminal":"T3","ETerminal":"T3","FlightModel":"320","Meal":"L","Distance":"1402","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[]}],"ResultBag":"\r\n AV:MU2325/24AUG                                                               \r\nDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              \r\nLHW 0740   XIY 0840   THU  1:00  1:00   T2/T3 320        518                   \r\nXIY 0940   SZX 1210   THU  2:30         T3/T3 320  L     1402                  \r\nTOTAL JOURNEY TIME   4:30                                                      \r\nLHWXIY                                                                         \r\nXIYSZX                                                                         \r\nMEMBER OF SKYTEAM\r\n"},"reqtime":"\/Date(1440415954955+0800)\/","SaveTime":1800,"ServerUrl":null}
            request.FlightNo = "MU2325";
            request.DepDate = Convert.ToDateTime("2015-08-24");
            //// 查询条件：            
            //request.SCity = "LHW";
            //request.ECity = "XIY";
            //request.Carbin = "S";
            string cmdResult =
            //string cmdResult7 =
@"
 AV:MU2325/24AUG                                                               
DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              
LHW 0740   XIY 0840   THU  1:00  1:00   T2/T3 320        518                   
XIY 0940   SZX 1210   THU  2:30         T3/T3 320  L     1402                  
TOTAL JOURNEY TIME   4:30                                                      
LHWXIY                                                                         
XIYSZX                                                                         
MEMBER OF SKYTEAM
";

            // 一个舱位都没有（只有1个航线）
            // string cmdResult =
            string cmdResult8 =
@"";

            JetermEntity.Parser.AV av = new JetermEntity.Parser.AV(string.Empty, string.Empty);
            av.ParseCmd(request);
            CommandResult<JetermEntity.Response.AV> result = av.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string parseResult22 = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}AV指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);
        }

        [TestMethod]
        public void AVTest_EtermProxy1()
        {
            string strPost = "{\"ClassName\" : \"AV\", \"Config\" : \"\",  \"OfficeNo\" : \"SHA243\" }";
            //string ss = "{\"FlightList\":[{\"FlightNo\":\"MU5137\",\"Cabin\":\"H\",\"SCity\":\"SHA\",\"ECity\":\"PEK\",\"DepDate\":\"\\/Date(1430064000000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"MU5156\",\"Cabin\":\"B\",\"SCity\":\"PEK\",\"ECity\":\"SHA\",\"DepDate\":\"\\/Date(1430323200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"干园\",\"idtype\":0,\"cardno\":\"650121199412242866\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"},{\"name\":\"张杰\",\"idtype\":0,\"cardno\":\"140525198401186312\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"13472634765\",\"RMKOfficeNoList\":[],\"RMKRemark\":null,\"Pnr\":null}";

            // 设置请求参数：
            JetermEntity.Request.AV request = new JetermEntity.Request.AV();
            // 测试案例1：没有共享航班的，即没有OPE（有2个航线）           
            // 指令：            
            /*
指令返回结果：
 AV:MU2325/09OCT                                                               
DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              
LHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518                   
XIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402                  
TOTAL JOURNEY TIME   4:30                                                      
LHWXIY UC F4 PQ J2 CQ DQ IQ WQ YA BA M5 E5 H5 K5 L5 N5 R4 SQ VQ                
       TQ GS ZQ QQ                                                             
XIYSZX UC F4 PQ J2 CQ DQ IQ WQ YS BA MS ES HS KS LS NS RS SS VS                
       TS GS ZS QS                                                             
MEMBER OF SKYTEAM
             */
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"MU2325","DepDate":"\/Date(1444320000000+0800)\/","TotalJourneyTime":"4:30","AVList":[{"SCity":"LHW","ECity":"XIY","STime":"0740","ETime":"0840","EWeek":"FRI","FltDuration":"1:00","Ground":"1:00","STerminal":"T2","ETerminal":"T3","FlightModel":"320","Meal":null,"Distance":"518","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"4","NumStr":"4"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"2","NumStr":"2"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"5","NumStr":"5"},{"Cabin":"E","NumTag":"5","NumStr":"5"},{"Cabin":"H","NumTag":"5","NumStr":"5"},{"Cabin":"K","NumTag":"5","NumStr":"5"},{"Cabin":"L","NumTag":"5","NumStr":"5"},{"Cabin":"N","NumTag":"5","NumStr":"5"},{"Cabin":"R","NumTag":"4","NumStr":"4"},{"Cabin":"S","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"G","NumTag":"S","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"Q","NumTag":"Q","NumStr":"0"}]},{"SCity":"XIY","ECity":"SZX","STime":"0940","ETime":"1210","EWeek":"FRI","FltDuration":"2:30","Ground":null,"STerminal":"T3","ETerminal":"T3","FlightModel":"320","Meal":"L","Distance":"1402","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"4","NumStr":"4"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"2","NumStr":"2"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"S","NumStr":"0"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"S","NumStr":"0"},{"Cabin":"E","NumTag":"S","NumStr":"0"},{"Cabin":"H","NumTag":"S","NumStr":"0"},{"Cabin":"K","NumTag":"S","NumStr":"0"},{"Cabin":"L","NumTag":"S","NumStr":"0"},{"Cabin":"N","NumTag":"S","NumStr":"0"},{"Cabin":"R","NumTag":"S","NumStr":"0"},{"Cabin":"S","NumTag":"S","NumStr":"0"},{"Cabin":"V","NumTag":"S","NumStr":"0"},{"Cabin":"T","NumTag":"S","NumStr":"0"},{"Cabin":"G","NumTag":"S","NumStr":"0"},{"Cabin":"Z","NumTag":"S","NumStr":"0"},{"Cabin":"Q","NumTag":"S","NumStr":"0"}]}],"ResultBag":" AV:MU2325/09OCT                                                               \rDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              \rLHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518                   \rXIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402                  \rTOTAL JOURNEY TIME   4:30                                                      \rLHWXIY UC F4 PQ J2 CQ DQ IQ WQ YA BA M5 E5 H5 K5 L5 N5 R4 SQ VQ                \r       TQ GS ZQ QQ                                                             \rXIYSZX UC F4 PQ J2 CQ DQ IQ WQ YS BA MS ES HS KS LS NS RS SS VS                \r       TS GS ZS QS                                                             \rMEMBER OF SKYTEAM                                                              \r"},"reqtime":"\/Date(1441006748156+0800)\/","SaveTime":1800,"ServerUrl":null}
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"MU2325","DepDate":"\/Date(1444320000000+0800)\/","TotalJourneyTime":"4:30","AVList":[{"SCity":"LHW","ECity":"XIY","STime":"0740","ETime":"0840","EWeek":"FRI","FltDuration":"1:00","Ground":"1:00","STerminal":"T2","ETerminal":"T3","FlightModel":"320","Meal":null,"Distance":"518","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"4","NumStr":"4"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"2","NumStr":"2"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"5","NumStr":"5"},{"Cabin":"E","NumTag":"5","NumStr":"5"},{"Cabin":"H","NumTag":"5","NumStr":"5"},{"Cabin":"K","NumTag":"5","NumStr":"5"},{"Cabin":"L","NumTag":"5","NumStr":"5"},{"Cabin":"N","NumTag":"5","NumStr":"5"},{"Cabin":"R","NumTag":"4","NumStr":"4"},{"Cabin":"S","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"G","NumTag":"S","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"Q","NumTag":"Q","NumStr":"0"}]},{"SCity":"XIY","ECity":"SZX","STime":"0940","ETime":"1210","EWeek":"FRI","FltDuration":"2:30","Ground":null,"STerminal":"T3","ETerminal":"T3","FlightModel":"320","Meal":"L","Distance":"1402","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"4","NumStr":"4"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"2","NumStr":"2"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"S","NumStr":"0"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"S","NumStr":"0"},{"Cabin":"E","NumTag":"S","NumStr":"0"},{"Cabin":"H","NumTag":"S","NumStr":"0"},{"Cabin":"K","NumTag":"S","NumStr":"0"},{"Cabin":"L","NumTag":"S","NumStr":"0"},{"Cabin":"N","NumTag":"S","NumStr":"0"},{"Cabin":"R","NumTag":"S","NumStr":"0"},{"Cabin":"S","NumTag":"S","NumStr":"0"},{"Cabin":"V","NumTag":"S","NumStr":"0"},{"Cabin":"T","NumTag":"S","NumStr":"0"},{"Cabin":"G","NumTag":"S","NumStr":"0"},{"Cabin":"Z","NumTag":"S","NumStr":"0"},{"Cabin":"Q","NumTag":"S","NumStr":"0"}]}],"ResultBag":" AV:MU2325/09OCT                                                               \rDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              \rLHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518                   \rXIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402                  \rTOTAL JOURNEY TIME   4:30                                                      \rLHWXIY UC F4 PQ J2 CQ DQ IQ WQ YA BA M5 E5 H5 K5 L5 N5 R4 SQ VQ                \r       TQ GS ZQ QQ                                                             \rXIYSZX UC F4 PQ J2 CQ DQ IQ WQ YS BA MS ES HS KS LS NS RS SS VS                \r       TS GS ZS QS                                                             \rMEMBER OF SKYTEAM                                                              \r"},"reqtime":"\/Date(1441169684244+0800)\/","SaveTime":1800,"ServerUrl":null}
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"MU2325","DepDate":"\/Date(1444320000000+0800)\/","TotalJourneyTime":"4:30","AVList":[{"SCity":"LHW","ECity":"XIY","STime":"0740","ETime":"0840","EWeek":"FRI","FltDuration":"1:00","Ground":"1:00","STerminal":"T2","ETerminal":"T3","FlightModel":"320","Meal":null,"Distance":"518","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"4","NumStr":"4"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"2","NumStr":"2"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"5","NumStr":"5"},{"Cabin":"E","NumTag":"5","NumStr":"5"},{"Cabin":"H","NumTag":"5","NumStr":"5"},{"Cabin":"K","NumTag":"5","NumStr":"5"},{"Cabin":"L","NumTag":"5","NumStr":"5"},{"Cabin":"N","NumTag":"5","NumStr":"5"},{"Cabin":"R","NumTag":"4","NumStr":"4"},{"Cabin":"S","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"G","NumTag":"S","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"Q","NumTag":"Q","NumStr":"0"}]},{"SCity":"XIY","ECity":"SZX","STime":"0940","ETime":"1210","EWeek":"FRI","FltDuration":"2:30","Ground":null,"STerminal":"T3","ETerminal":"T3","FlightModel":"320","Meal":"L","Distance":"1402","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"4","NumStr":"4"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"2","NumStr":"2"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"S","NumStr":"0"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"S","NumStr":"0"},{"Cabin":"E","NumTag":"S","NumStr":"0"},{"Cabin":"H","NumTag":"S","NumStr":"0"},{"Cabin":"K","NumTag":"S","NumStr":"0"},{"Cabin":"L","NumTag":"S","NumStr":"0"},{"Cabin":"N","NumTag":"S","NumStr":"0"},{"Cabin":"R","NumTag":"S","NumStr":"0"},{"Cabin":"S","NumTag":"S","NumStr":"0"},{"Cabin":"V","NumTag":"S","NumStr":"0"},{"Cabin":"T","NumTag":"S","NumStr":"0"},{"Cabin":"G","NumTag":"S","NumStr":"0"},{"Cabin":"Z","NumTag":"S","NumStr":"0"},{"Cabin":"Q","NumTag":"S","NumStr":"0"}]}],"ResultBag":" AV:MU2325/09OCT                                                               \rDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              \rLHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518                   \rXIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402                  \rTOTAL JOURNEY TIME   4:30                                                      \rLHWXIY UC F4 PQ J2 CQ DQ IQ WQ YA BA M5 E5 H5 K5 L5 N5 R4 SQ VQ                \r       TQ GS ZQ QQ                                                             \rXIYSZX UC F4 PQ J2 CQ DQ IQ WQ YS BA MS ES HS KS LS NS RS SS VS                \r       TS GS ZS QS                                                             \rMEMBER OF SKYTEAM                                                              \r"},"reqtime":"\/Date(1441169860563+0800)\/","SaveTime":1800,"ServerUrl":null}
            // {"state":true,"error":null,"config":"","OfficeNo":"SHA243","result":{"FlightNo":"MU2325","DepDate":"\/Date(1444320000000+0800)\/","TotalJourneyTime":"4:30","AVList":[{"SCity":"LHW","ECity":"XIY","STime":"0740","ETime":"0840","EWeek":"FRI","FltDuration":"1:00","Ground":"1:00","STerminal":"T2","ETerminal":"T3","FlightModel":"320","Meal":null,"Distance":"518","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"4","NumStr":"4"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"2","NumStr":"2"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"5","NumStr":"5"},{"Cabin":"E","NumTag":"5","NumStr":"5"},{"Cabin":"H","NumTag":"5","NumStr":"5"},{"Cabin":"K","NumTag":"5","NumStr":"5"},{"Cabin":"L","NumTag":"5","NumStr":"5"},{"Cabin":"N","NumTag":"5","NumStr":"5"},{"Cabin":"R","NumTag":"4","NumStr":"4"},{"Cabin":"S","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"G","NumTag":"S","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"Q","NumTag":"Q","NumStr":"0"}]},{"SCity":"XIY","ECity":"SZX","STime":"0940","ETime":"1210","EWeek":"FRI","FltDuration":"2:30","Ground":null,"STerminal":"T3","ETerminal":"T3","FlightModel":"320","Meal":"L","Distance":"1402","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"4","NumStr":"4"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"2","NumStr":"2"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"S","NumStr":"0"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"S","NumStr":"0"},{"Cabin":"E","NumTag":"S","NumStr":"0"},{"Cabin":"H","NumTag":"S","NumStr":"0"},{"Cabin":"K","NumTag":"S","NumStr":"0"},{"Cabin":"L","NumTag":"S","NumStr":"0"},{"Cabin":"N","NumTag":"S","NumStr":"0"},{"Cabin":"R","NumTag":"S","NumStr":"0"},{"Cabin":"S","NumTag":"S","NumStr":"0"},{"Cabin":"V","NumTag":"S","NumStr":"0"},{"Cabin":"T","NumTag":"S","NumStr":"0"},{"Cabin":"G","NumTag":"S","NumStr":"0"},{"Cabin":"Z","NumTag":"S","NumStr":"0"},{"Cabin":"Q","NumTag":"S","NumStr":"0"}]}],"ResultBag":" AV:MU2325/09OCT                                                               \rDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              \rLHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518                   \rXIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402                  \rTOTAL JOURNEY TIME   4:30                                                      \rLHWXIY UC F4 PQ J2 CQ DQ IQ WQ YA BA M5 E5 H5 K5 L5 N5 R4 SQ VQ                \r       TQ GS ZQ QQ                                                             \rXIYSZX UC F4 PQ J2 CQ DQ IQ WQ YS BA MS ES HS KS LS NS RS SS VS                \r       TS GS ZS QS                                                             \rMEMBER OF SKYTEAM                                                              \r"},"reqtime":"\/Date(1441170319880+0800)\/","SaveTime":1800,"ServerUrl":null}
            request.FlightNo = "MU2325";
            request.DepDate = Convert.ToDateTime("2015-10-09");

            // 测试案例2：没有共享航班的，即没有OPE（只有1个航线）
            // 指令：            
            /*
指令返回结果：
 AV:FM9220/16AUG16                                                             
DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              
URC 0930   SHA 1410   TUE  4:40         T2/T2 738        3271                  
TOTAL JOURNEY TIME   4:40                                                      
URCSHA UC F8 PQ JC CQ DQ IQ WQ YA BA MA EA HQ KQ LQ NQ RQ SQ VQ                
       TQ GQ ZQ QQ                                                             
MEMBER OF SKYTEAM
            */
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"FM9220","DepDate":"\/Date(1471276800000+0800)\/","TotalJourneyTime":"4:40","AVList":[{"SCity":"URC","ECity":"SHA","STime":"0930","ETime":"1410","EWeek":"TUE","FltDuration":"4:40","Ground":null,"STerminal":"T2","ETerminal":"T2","FlightModel":"738","Meal":null,"Distance":"3271","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"U","NumTag":"C","NumStr":"0"},{"Cabin":"F","NumTag":"8","NumStr":"8"},{"Cabin":"P","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"C","NumStr":"0"},{"Cabin":"C","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"I","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"A","NumStr":"A"},{"Cabin":"E","NumTag":"A","NumStr":"A"},{"Cabin":"H","NumTag":"Q","NumStr":"0"},{"Cabin":"K","NumTag":"Q","NumStr":"0"},{"Cabin":"L","NumTag":"Q","NumStr":"0"},{"Cabin":"N","NumTag":"Q","NumStr":"0"},{"Cabin":"R","NumTag":"Q","NumStr":"0"},{"Cabin":"S","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"G","NumTag":"Q","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"Q","NumTag":"Q","NumStr":"0"}]}],"ResultBag":" AV:FM9220/16AUG16                                                             \rDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              \rURC 0930   SHA 1410   TUE  4:40         T2/T2 738        3271                  \rTOTAL JOURNEY TIME   4:40                                                      \rURCSHA UC F8 PQ JC CQ DQ IQ WQ YA BA MA EA HQ KQ LQ NQ RQ SQ VQ                \r       TQ GQ ZQ QQ                                                             \rMEMBER OF SKYTEAM                                                              \r"},"reqtime":"\/Date(1441008050472+0800)\/","SaveTime":1800,"ServerUrl":null}
            //request.FlightNo = "FM9220";
            //request.DepDate = Convert.ToDateTime("2016-08-16");

            // 测试案例3：有共享航班的，即有OPE（有2个航线）
            // 指令：            
            /*
指令返回结果：
 AV:CZ9104/09OCT                                                               
DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE OPE          
LHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518      MU2325       
XIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402     MU2325       
TOTAL JOURNEY TIME   4:30                                                      
LHWXIY YA B5 M5 U5 L5 E4                                                       
XIYSZX YS BS MS US LS ES                                                       
MEMBER OF SKYTEAM 
            */
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"CZ9104","DepDate":"\/Date(1444320000000+0800)\/","TotalJourneyTime":"4:30","AVList":[{"SCity":"LHW","ECity":"XIY","STime":"0740","ETime":"0840","EWeek":"FRI","FltDuration":"1:00","Ground":"1:00","STerminal":"T2","ETerminal":"T3","FlightModel":"320","Meal":null,"Distance":"518","ShareFlight":true,"ShareFltNo":"MU2325","CarbinNumList":[{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"5","NumStr":"5"},{"Cabin":"M","NumTag":"5","NumStr":"5"},{"Cabin":"U","NumTag":"5","NumStr":"5"},{"Cabin":"L","NumTag":"5","NumStr":"5"},{"Cabin":"E","NumTag":"4","NumStr":"4"}]},{"SCity":"XIY","ECity":"SZX","STime":"0940","ETime":"1210","EWeek":"FRI","FltDuration":"2:30","Ground":null,"STerminal":"T3","ETerminal":"T3","FlightModel":"320","Meal":"L","Distance":"1402","ShareFlight":true,"ShareFltNo":"MU2325","CarbinNumList":[{"Cabin":"Y","NumTag":"S","NumStr":"0"},{"Cabin":"B","NumTag":"S","NumStr":"0"},{"Cabin":"M","NumTag":"S","NumStr":"0"},{"Cabin":"U","NumTag":"S","NumStr":"0"},{"Cabin":"L","NumTag":"S","NumStr":"0"},{"Cabin":"E","NumTag":"S","NumStr":"0"}]}],"ResultBag":" AV:CZ9104/09OCT                                                               \rDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE OPE          \rLHW 0740   XIY 0840   FRI  1:00  1:00   T2/T3 320        518      MU2325       \rXIY 0940   SZX 1210   FRI  2:30         T3/T3 320  L     1402     MU2325       \rTOTAL JOURNEY TIME   4:30                                                      \rLHWXIY YA B5 M5 U5 L5 E4                                                       \rXIYSZX YS BS MS US LS ES                                                       \rMEMBER OF SKYTEAM                                                              \r"},"reqtime":"\/Date(1441008658515+0800)\/","SaveTime":1800,"ServerUrl":null}
            //request.FlightNo = "CZ9104";
            //request.DepDate = Convert.ToDateTime("2015-10-09");

            // 测试案例4：有共享航班的，即有OPE（只有1个航线） 
            // 指令：            
            /*
指令返回结果：
            */
            // 返回结果：

            // 测试案例5：没有DISTANCE（有2个航线）
            // 指令：            
            /*
指令返回结果：
 AV:GS7544/09OCT                                                               
DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL                        
LHW 1450   IQN 1530   FRI  0:40  0:45   T2/T1 190                              
IQN 1615   XIY 1650   FRI  0:35         T1/T2 190                              
TOTAL JOURNEY TIME   2:00                                                      
LHWIQN YA BA HA KA LA MA QA XA UQ EQ TQ ZQ V3 RQ NQ WQ JQ DQ OQ                
       S2 GQ                                                                   
IQNXIY YA BA HA KA LA MA QA XA UA EA TQ ZQ V3 RQ NQ WQ JQ DQ OQ                
       S2 GQ
            */
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"GS7544","DepDate":"\/Date(1444320000000+0800)\/","TotalJourneyTime":"2:00","AVList":[{"SCity":"LHW","ECity":"IQN","STime":"1450","ETime":"1530","EWeek":"FRI","FltDuration":"0:40","Ground":"0:45","STerminal":"T2","ETerminal":"T1","FlightModel":"190","Meal":null,"Distance":null,"ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"H","NumTag":"A","NumStr":"A"},{"Cabin":"K","NumTag":"A","NumStr":"A"},{"Cabin":"L","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"A","NumStr":"A"},{"Cabin":"Q","NumTag":"A","NumStr":"A"},{"Cabin":"X","NumTag":"A","NumStr":"A"},{"Cabin":"U","NumTag":"Q","NumStr":"0"},{"Cabin":"E","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"3","NumStr":"3"},{"Cabin":"R","NumTag":"Q","NumStr":"0"},{"Cabin":"N","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"O","NumTag":"Q","NumStr":"0"},{"Cabin":"S","NumTag":"2","NumStr":"2"},{"Cabin":"G","NumTag":"Q","NumStr":"0"}]},{"SCity":"IQN","ECity":"XIY","STime":"1615","ETime":"1650","EWeek":"FRI","FltDuration":"0:35","Ground":null,"STerminal":"T1","ETerminal":"T2","FlightModel":"190","Meal":null,"Distance":null,"ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"H","NumTag":"A","NumStr":"A"},{"Cabin":"K","NumTag":"A","NumStr":"A"},{"Cabin":"L","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"A","NumStr":"A"},{"Cabin":"Q","NumTag":"A","NumStr":"A"},{"Cabin":"X","NumTag":"A","NumStr":"A"},{"Cabin":"U","NumTag":"A","NumStr":"A"},{"Cabin":"E","NumTag":"A","NumStr":"A"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"Z","NumTag":"Q","NumStr":"0"},{"Cabin":"V","NumTag":"3","NumStr":"3"},{"Cabin":"R","NumTag":"Q","NumStr":"0"},{"Cabin":"N","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"J","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"Q","NumStr":"0"},{"Cabin":"O","NumTag":"Q","NumStr":"0"},{"Cabin":"S","NumTag":"2","NumStr":"2"},{"Cabin":"G","NumTag":"Q","NumStr":"0"}]}],"ResultBag":" AV:GS7544/09OCT                                                               \rDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL                        \rLHW 1450   IQN 1530   FRI  0:40  0:45   T2/T1 190                              \rIQN 1615   XIY 1650   FRI  0:35         T1/T2 190                              \rTOTAL JOURNEY TIME   2:00                                                      \rLHWIQN YA BA HA KA LA MA QA XA UQ EQ TQ ZQ V3 RQ NQ WQ JQ DQ OQ                \r       S2 GQ                                                                   \rIQNXIY YA BA HA KA LA MA QA XA UA EA TQ ZQ V3 RQ NQ WQ JQ DQ OQ                \r       S2 GQ                                                                   \r"},"reqtime":"\/Date(1441009347855+0800)\/","SaveTime":1800,"ServerUrl":null}
            //request.FlightNo = "GS7544";
            //request.DepDate = Convert.ToDateTime("2015-10-09");

            // 测试案例6：（不测，因为没有找到测试案例）没有DISTANCE（只有1个航线）
            // 指令：            
            /*
指令返回结果：
            */
            // 返回结果：

            // 测试案例7：一个舱位都没有（有2个航线）
            // 指令：            
            /*
指令返回结果：
            */
            // 返回结果：
            // {"state":false,"error":{"ErrorCode":79,"ErrorMessage":"很抱歉，不能查询历史起飞日期的舱位剩余可订数","CmdResultBag":null},"config":"","OfficeNo":"","result":{"FlightNo":null,"DepDate":"\/Date(-62135596800000+0800)\/","TotalJourneyTime":null,"AVList":[],"ResultBag":null},"reqtime":"\/Date(1441011023500+0800)\/","SaveTime":1800,"ServerUrl":null}
            //request.FlightNo = "MU2325";
            //request.DepDate = Convert.ToDateTime("2015-08-24");

            // 测试案例8：一个舱位都没有（只有1个航线）
            // 指令：            
            /*
指令返回结果：
            */
            // 返回结果：

            // 返回结果中有【MEMBER OF STAR ALLIANCE】
            /*
            指令返回结果：
             AV:ZH9522/02SEP                                                               
            DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              
            CTU 0640   XIY 0755   WED  1:15         T2/T2 320        622                   
            TOTAL JOURNEY TIME   1:15                                                      
            CTUXIY F6 P1 A2 OQ DX YA BA MA HA KA LA JA QA ZA GA VQ WQ EQ TQ                
                   UA SQ X2 NQ                                                             
            MEMBER OF STAR ALLIANCE
             */
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"FlightNo":"ZH9522","DepDate":"\/Date(1441123200000+0800)\/","TotalJourneyTime":"1:15","AVList":[{"SCity":"CTU","ECity":"XIY","STime":"0640","ETime":"0755","EWeek":"WED","FltDuration":"1:15","Ground":null,"STerminal":"T2","ETerminal":"T2","FlightModel":"320","Meal":null,"Distance":"622","ShareFlight":false,"ShareFltNo":"","CarbinNumList":[{"Cabin":"F","NumTag":"6","NumStr":"6"},{"Cabin":"P","NumTag":"1","NumStr":"1"},{"Cabin":"A","NumTag":"2","NumStr":"2"},{"Cabin":"O","NumTag":"Q","NumStr":"0"},{"Cabin":"D","NumTag":"X","NumStr":"0"},{"Cabin":"Y","NumTag":"A","NumStr":"A"},{"Cabin":"B","NumTag":"A","NumStr":"A"},{"Cabin":"M","NumTag":"A","NumStr":"A"},{"Cabin":"H","NumTag":"A","NumStr":"A"},{"Cabin":"K","NumTag":"A","NumStr":"A"},{"Cabin":"L","NumTag":"A","NumStr":"A"},{"Cabin":"J","NumTag":"A","NumStr":"A"},{"Cabin":"Q","NumTag":"A","NumStr":"A"},{"Cabin":"Z","NumTag":"A","NumStr":"A"},{"Cabin":"G","NumTag":"A","NumStr":"A"},{"Cabin":"V","NumTag":"Q","NumStr":"0"},{"Cabin":"W","NumTag":"Q","NumStr":"0"},{"Cabin":"E","NumTag":"Q","NumStr":"0"},{"Cabin":"T","NumTag":"Q","NumStr":"0"},{"Cabin":"U","NumTag":"A","NumStr":"A"},{"Cabin":"S","NumTag":"Q","NumStr":"0"},{"Cabin":"X","NumTag":"2","NumStr":"2"},{"Cabin":"N","NumTag":"Q","NumStr":"0"}]}],"ResultBag":" AV:ZH9522/02SEP                                                               \rDEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE              \rCTU 0640   XIY 0755   WED  1:15         T2/T2 320        622                   \rTOTAL JOURNEY TIME   1:15                                                      \rCTUXIY F6 P1 A2 OQ DX YA BA MA HA KA LA JA QA ZA GA VQ WQ EQ TQ                \r       UA SQ X2 NQ                                                             \rMEMBER OF STAR ALLIANCE                                                        \r"},"reqtime":"\/Date(1441094089064+0800)\/","SaveTime":1800,"ServerUrl":null}
            //request.FlightNo = "ZH9522";
            //request.DepDate = Convert.ToDateTime("2015-09-02");

            string ss = JsonConvert.SerializeObject(request);
            //string ss = "{\"FlightList\":[{\"FlightNo\":\"CZ6178\",\"Airline\":\"\",\"Cabin\":\"Y\",\"SCity\":\"CGQ\",\"ECity\":\"CSX\",\"DepTerminal\":null,\"ArrTerminal\":null,\"DepDate\":\"\\/Date(1435075200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"CZ3937\",\"Airline\":\"\",\"Cabin\":\"M\",\"SCity\":\"CSX\",\"ECity\":\"CGQ\",\"DepTerminal\":null,\"ArrTerminal\":null,\"DepDate\":\"\\/Date(1435161600000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"张龙\",\"idtype\":0,\"cardno\":\"610103197010032517\",\"PassType\":0,\"Ename\":\"\",\"BabyBirthday\":\"\\/Date(-62135596800000+0800)\\/\",\"ChildBirthday\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"18101810679\",\"RMKOfficeNoList\":[\"CGQ203\"],\"RMKRemark\":null,\"Pnr\":null}";

            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, ss);
            Console.WriteLine("返回结果：" + sret);
        }
    }
}
