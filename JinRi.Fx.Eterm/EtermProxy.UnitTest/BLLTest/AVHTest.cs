using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JetermEntity.Response;
using JetermEntity.Request;
using JetermEntity;
using Newtonsoft.Json;
using JetermClient.Utility;

namespace EtermProxy.UnitTest.BLLTest
{
    [TestClass]
    public class AVHTest
    {
        [TestMethod]
        public void AVHTest_ParseCmdResult1()
        {
            //List<string> vec = new List<string>();
            //for (int i = 0; i < 1000; i++)
            //{
            //    string guid = Guid.NewGuid().ToString("N");

            //    if (guid.Length == 32)
            //        vec.Add(guid);
            //}
            JetermEntity.Request.AVH request = new JetermEntity.Request.AVH();
            //request.SCity = "SHE";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-08-25");
            //request.Airline = "CA"; 

            // 不测
            // 没权限查询AVH指令：
            // 请求url：http://114.80.79.152:5555/
            // 指令：system("AVH/SHEPEK/25AUG/CA");
            // 返回结果：
            // 很抱歉，您没有权限查看舱位剩余可订数
            //string cmdResult =
            string cmdResult1 =
@"
 25AUG(TUE) SHEBJS VIA CA                                                      
1-  CA1602  DS#                                SHEPEK 0930   1110   738 0^N  E  
>                                                                   -- T3  1:40
2   CA1652  DS#                                SHEPEK 1100   1245   738 0^   E  
>                                                                   -- T3  1:45
3   CA1658  DS#                                SHEPEK 1545   1720   738 0^   E  
>                                                                   -- T3  1:35
4   CA1636  DS#                                SHEPEK 1910   2040   738 0^   E  
>                                                                   -- T3  1:30
5   CA1626  DS#                                SHEPEK 2105   2240   73L 0^   E  
>                                                                   -- T3  1:35
6+  CA4186  DS#                                SHECTU 1210   1555   321 0^D  E  
>                                                                   -- T2  3:45
    CA4117  DS#                                   PEK 1700   1940   321 0^D  E  
>                                                                   T2 T3  7:30
『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   
『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』 
";

            // 将测
            // 已测
            // 没权限查询AVH指令：
            // 请求url：http://114.80.79.152:5555/
            // 指令：system("AVH/SHEPEK/25AUG/CA/D");
            // 返回结果：
            // 很抱歉，您没有权限查看舱位剩余可订数
            // {"DepDate":"\/Date(1440432000000+0800)\/","AVHList":[{"Number":1,"ShareFlight":false,"FlightNo":"CA1602","ShareFlightNoList":[],"FlightModel":"330","SCity":"SHE","ECity":"PEK","STime":"0930","ETime":"1110","DepTerminal":"--","ArrTerminal":"T3","FlightDuration":"1:40","CarbinNumberList":[]},{"Number":2,"ShareFlight":false,"FlightNo":"CA1652","ShareFlightNoList":[],"FlightModel":"738","SCity":"SHE","ECity":"PEK","STime":"1100","ETime":"1245","DepTerminal":"--","ArrTerminal":"T3","FlightDuration":"1:45","CarbinNumberList":[]},{"Number":3,"ShareFlight":false,"FlightNo":"CA1658","ShareFlightNoList":[],"FlightModel":"73K","SCity":"SHE","ECity":"PEK","STime":"1545","ETime":"1720","DepTerminal":"--","ArrTerminal":"T3","FlightDuration":"1:35","CarbinNumberList":[]},{"Number":4,"ShareFlight":false,"FlightNo":"CA1636","ShareFlightNoList":[],"FlightModel":"738","SCity":"SHE","ECity":"PEK","STime":"1910","ETime":"2040","DepTerminal":"--","ArrTerminal":"T3","FlightDuration":"1:30","CarbinNumberList":[]},{"Number":5,"ShareFlight":false,"FlightNo":"CA1626","ShareFlightNoList":[],"FlightModel":"73L","SCity":"SHE","ECity":"PEK","STime":"2105","ETime":"2240","DepTerminal":"--","ArrTerminal":"T3","FlightDuration":"1:35","CarbinNumberList":[]}],"ResultBag":"\r\n 25AUG(TUE) SHEBJS VIA CA DIRECT ONLY                                          \r\n1-  CA1602  DS#                                SHEPEK 0930   1110   330 0^N  E  \r\n>                                                                   -- T3  1:40\r\n2   CA1652  DS#                                SHEPEK 1100   1245   738 0^   E  \r\n>                                                                   -- T3  1:45\r\n3   CA1658  DS#                                SHEPEK 1545   1720   73K 0^   E  \r\n>                                                                   -- T3  1:35\r\n4   CA1636  DS#                                SHEPEK 1910   2040   738 0^   E  \r\n>                                                                   -- T3  1:30\r\n5+  CA1626  DS#                                SHEPEK 2105   2240   73L 0^   E  \r\n>                                                                   -- T3  1:35\r\n『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   \r\n『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』 \r\n"}
            request.SCity = "SHE";
            request.ECity = "PEK";
            request.DepDate = Convert.ToDateTime("2015-08-25");
            request.Airline = "CA"; 
            string cmdResult =
            //string cmdResult1_1 =
@"
 25AUG(TUE) SHEBJS VIA CA DIRECT ONLY                                          
1-  CA1602  DS#                                SHEPEK 0930   1110   330 0^N  E  
>                                                                   -- T3  1:40
2   CA1652  DS#                                SHEPEK 1100   1245   738 0^   E  
>                                                                   -- T3  1:45
3   CA1658  DS#                                SHEPEK 1545   1720   73K 0^   E  
>                                                                   -- T3  1:35
4   CA1636  DS#                                SHEPEK 1910   2040   738 0^   E  
>                                                                   -- T3  1:30
5+  CA1626  DS#                                SHEPEK 2105   2240   73L 0^   E  
>                                                                   -- T3  1:35
『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   
『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』 
";

            // 不测
            // 有权限查询AVH指令，但舱位数都为0：
            // 请求url：http://114.80.79.152:6666/
            // 指令：system("AVH/SHEPEK/25AUG/CA");
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"AVHList":[{"FlightNo":"CA1602","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"A","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"B","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"C","AVNumberString":"0"}]},{"FlightNo":"CA1652","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"A","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"B","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"C","AVNumberString":"0"}]},{"FlightNo":"CA1658","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"A","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"B","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"C","AVNumberString":"0"}]},{"FlightNo":"CA1636","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"A","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"B","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"C","AVNumberString":"0"}]},{"FlightNo":"CA1626","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"A","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"B","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"C","AVNumberString":"0"}]}],"ResultBag":"\r\n 25AUG(TUE) SHEBJS VIA CA                                                      \r\n1-  CA1602  DS# FC AC YC BC MC HC KC LC QC GC  SHEPEK 0930   1110   738 0^N  E  \r\n>               SC NC VC UC TC EC                                   -- T3  1:40\r\n               ** M1C H1C K1C L1C Q1C V1C                                      \r\n2   CA1652  DS# FC AC YC BC MC HC KC LC QC GC  SHEPEK 1100   1245   738 0^   E  \r\n>               SC NC VC UC TC EC                                   -- T3  1:45\r\n               ** M1C H1C K1C L1C Q1C V1C                                      \r\n3   CA1658  DS# FC AC YC BC MC HC KC LC QC GC  SHEPEK 1545   1720   738 0^   E  \r\n>               SC NC VC UC TC EC                                   -- T3  1:35\r\n               ** M1C H1C K1C L1C Q1C V1C                                      \r\n4   CA1636  DS# FC AC YC BC MC HC KC LC QC GC  SHEPEK 1910   2040   738 0^   E  \r\n>               SC NC VC UC TC EC                                   -- T3  1:30\r\n               ** M1C H1C K1C L1C Q1C V1C                                      \r\n5+  CA1626  DS# FC AC YC BC MC HC KC LC QC GC  SHEPEK 2105   2240   73L 0^   E  \r\n>               SC NC VC UC TC EC                                   -- T3  1:35\r\n               ** M1C H1C K1C L1C Q1C V1C                                      \r\n『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   \r\n『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』 \r\n"},"reqtime":"\/Date(1436236515218+0800)\/","SaveTime":1800}
            //string cmdResult =
            string cmdResult222 =
@"
 25AUG(TUE) SHEBJS VIA CA                                                      
1-  CA1602  DS# FC AC YC BC MC HC KC LC QC GC  SHEPEK 0930   1110   738 0^N  E  
>               SC NC VC UC TC EC                                   -- T3  1:40
               ** M1C H1C K1C L1C Q1C V1C                                      
2   CA1652  DS# FC AC YC BC MC HC KC LC QC GC  SHEPEK 1100   1245   738 0^   E  
>               SC NC VC UC TC EC                                   -- T3  1:45
               ** M1C H1C K1C L1C Q1C V1C                                      
3   CA1658  DS# FC AC YC BC MC HC KC LC QC GC  SHEPEK 1545   1720   738 0^   E  
>               SC NC VC UC TC EC                                   -- T3  1:35
               ** M1C H1C K1C L1C Q1C V1C                                      
4   CA1636  DS# FC AC YC BC MC HC KC LC QC GC  SHEPEK 1910   2040   738 0^   E  
>               SC NC VC UC TC EC                                   -- T3  1:30
               ** M1C H1C K1C L1C Q1C V1C                                      
5+  CA1626  DS# FC AC YC BC MC HC KC LC QC GC  SHEPEK 2105   2240   73L 0^   E  
>               SC NC VC UC TC EC                                   -- T3  1:35
               ** M1C H1C K1C L1C Q1C V1C                                      
『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   
『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』 
";

            // 不测
            // 有权限查询AVH指令，且有舱位数：
            // 请求url：http://114.80.69.243:18082/
            // 指令：
            // system("AVH/SHEPEK/25AUG/CA");  
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"AVHList":[{"FlightNo":"CA1602","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CA1652","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CA1658","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CA1636","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CA1626","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"4","AVNumberString":"4"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H1","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K1","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L1","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]}],"ResultBag":"\r\n 25AUG(TUE) SHEBJS VIA CA                                                      \r\n1-  CA1602  DS# F2 A2 YA BS MS HS KS LS QS GS  SHEPEK 0930   1110   738 0^N  E  \r\n>               SS NS VS US TS ES                                   -- T3  1:40\r\n               ** M1S H1S K1S L1S Q1S V1S                                      \r\n2   CA1652  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1100   1245   738 0^   E  \r\n>               SA NS VS US TS ES                                   -- T3  1:45\r\n               ** M1S H1S K1S L1S Q1S V1S                                      \r\n3   CA1658  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1545   1720   738 0^   E  \r\n>               SA NS VS US TS ES                                   -- T3  1:35\r\n               ** M1S H1S K1S L1S Q1S V1S                                      \r\n4   CA1636  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1910   2040   738 0^   E  \r\n>               SA NS VS US TS ES                                   -- T3  1:30\r\n               ** M1S H1S K1S L1S Q1S V1S                                      \r\n5+  CA1626  DS# FA A4 YA BA MA HA KA LA QS GS  SHEPEK 2105   2240   73L 0^   E  \r\n>               S8 NS VS US TS ES                                   -- T3  1:35\r\n               ** M1A H1A K1A L1A Q1S V1S                                      \r\n『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   \r\n『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』 \r\n"},"reqtime":"\/Date(1436235761204+0800)\/","SaveTime":1800}
            //string cmdResult =
            string cmdResult3 =
@"
 25AUG(TUE) SHEBJS VIA CA                                                      
1-  CA1602  DS# F2 A2 YA BS MS HS KS LS QS GS  SHEPEK 0930   1110   738 0^N  E  
>               SS NS VS US TS ES                                   -- T3  1:40
               ** M1S H1S K1S L1S Q1S V1S                                      
2   CA1652  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1100   1245   738 0^   E  
>               SA NS VS US TS ES                                   -- T3  1:45
               ** M1S H1S K1S L1S Q1S V1S                                      
3   CA1658  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1545   1720   738 0^   E  
>               SA NS VS US TS ES                                   -- T3  1:35
               ** M1S H1S K1S L1S Q1S V1S                                      
4   CA1636  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1910   2040   738 0^   E  
>               SA NS VS US TS ES                                   -- T3  1:30
               ** M1S H1S K1S L1S Q1S V1S                                      
5+  CA1626  DS# FA A4 YA BA MA HA KA LA QS GS  SHEPEK 2105   2240   73L 0^   E  
>               S8 NS VS US TS ES                                   -- T3  1:35
               ** M1A H1A K1A L1A Q1S V1S                                      
『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   
『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』 
";
            // 将测
            // 有权限查询AVH指令，且有舱位数：
            // 请求url：http://114.80.69.243:18082/
            // 指令：
            // system("AVH/PEKPVG/09JUN/MU/D");
            // 返回结果：
            string cmdResult3_1 =
@"
 09JUN16(THU) BJSPVG VIA MU DIRECT ONLY                                        
1-  MU5183  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKPVG 0735   0950   321 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T1  2:15
2   MU271   DS# UC FC PC JA CQ DQ IQ WQ YA B1  PEKPVG 1355   1555   321 0^   E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:00
3   MU563   DS# UC FC PC JA CQ DQ IQ WQ YA BA  PEKPVG 1620   1840   321 0^   E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:20
4+  MU5130  DS# UC F8 PQ JC CQ DQ IQ WQ YA BA  PEKPVG 1900   2120   320 0^D  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QQ              T2 T1  2:20
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』 
";

            // 未测，但测了cmdResult3_2_1这个替代例子
            // 将测
            // 有权限查询AVH指令，且有舱位数：
            // 重点测：
            // 请求url：http://114.80.69.243:18082/
            // 指令：
            //system("AVH/PEKSHA/09JUN/MU/D");
            //system("PN");
            //system("PN");
            // 返回结果：
            /*
            
         */
            //request.SCity = "PEK";
            //request.ECity = "SHA";
            //request.DepDate = Convert.ToDateTime("2016-06-09");
            //request.Airline = "MU"; 
            //string cmdResult =
            string cmdResult3_2 =
@"
 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        
1-  MU5138  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0700   0910   333 0^S  E  
>               MA EA HA KA LA NS RA SQ VQ TQ GQ ZS QS              T2 T2  2:10
2   MU5183  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKPVG 0735   0950   321 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T1  2:15
3   MU5102  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0800   0955   333 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  1:55
4   MU5104  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0900   1110   333 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
5   MU5106  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1000   1210   333 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
6   MU5108  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1100   1310   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
7   MU5152  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1130   1340   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
8   MU5110  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1200   1410   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QQ              T2 T2  2:10
9+  MU5112  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1300   1510   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10                           
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         

 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        
1-  MU5154  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1335   1545   763 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
2   MU271   DS# UC FC PC JA CQ DQ IQ WQ YA B1  PEKPVG 1355   1555   321 0^   E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:00
3   MU5114  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1400   1620   333 0^   E  
>               MS ES HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:20
4   MU5116  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1500   1710   333 0^   E  
>               MS ES HS KS LS NS RS SQ VQ TQ GQ ZS QS              T2 T2  2:10
5   MU563   DS# UC FC PC JA CQ DQ IQ WQ YA BA  PEKPVG 1620   1840   321 0^   E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:20
6   MU5120  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1700   1915   333 0^D  E  
>               MS ES HS KS LS NS RS SQ VQ TQ GQ ZS QS              T2 T2  2:15
7   MU5122  DS# UC FA PQ JC CC DC IC WQ YA BA  PEKSHA 1800   2010   333 0^D  E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10
8   MU5156  DS# UC FA P8 JC CQ DQ IQ WQ YA BA  PEKSHA 1830   2045   333 0^   E  
>               MA EA HA KQ LQ NQ RQ SQ VQ TQ GQ ZA QA              T2 T2  2:15
9+  MU5124  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1900   2110   333 0^D  E  
>               MS E3 HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:10                           
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         

 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        
1-  MU5130  DS# UC F8 PQ JC CQ DQ IQ WQ YA BA  PEKPVG 1900   2120   320 0^D  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QQ              T2 T1  2:20
2   MU5126  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 2000   2210   333 0^   E  
>               MS E3 HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:10
3   MU5128  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 2100   2315   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:15
4   MU5158  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 2130   2340   763 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
5+  MU5160  DS# UC FA PA JC CC DC IC W4 YA BA  PEKSHA 2200   2355   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  1:55
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』 
";

            // 已测
            //request.SCity = "PEK";
            //request.ECity = "SHA";
            //request.DepDate = Convert.ToDateTime("2016-06-09");
            //request.Airline = "MU";
            // 返回结果：
            // {"DepDate":"\/Date(1465401600000+0800)\/","AVHList":[{"FlightNo":"MU5138","FlightModel":"333","ShareFlight":false,"SCity":"PEK","ECity":"SHA","STime":"0700","ETime":"0910","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:10","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"MU5183","FlightModel":"321","ShareFlight":false,"SCity":"PEK","ECity":"PVG","STime":"0735","ETime":"0950","DepTerminal":"T2","ArrTerminal":"T1","FlightDuration":"2:15","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"}]},{"FlightNo":"MU5112","FlightModel":"333","ShareFlight":false,"SCity":"PEK","ECity":"SHA","STime":"1300","ETime":"1510","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:10","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"}]},{"FlightNo":"MU5154","FlightModel":"763","ShareFlight":false,"SCity":"PEK","ECity":"SHA","STime":"1335","ETime":"1545","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:10","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"}]},{"FlightNo":"MU5156","FlightModel":"333","ShareFlight":false,"SCity":"PEK","ECity":"SHA","STime":"1830","ETime":"2045","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:15","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"R","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"}]},{"FlightNo":"MU5124","FlightModel":"333","ShareFlight":false,"SCity":"PEK","ECity":"SHA","STime":"1900","ETime":"2110","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:10","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"3","AVNumberString":"3"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"R","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"Q","AVNumberString":"0"}]}],"ResultBag":"\r\n 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        \r\n1-  MU5138  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0700   0910   333 0^S  E  \r\n>               MA EA HA KA LA NS RA SQ VQ TQ GQ ZS QS              T2 T2  2:10\r\n2   MU5183  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKPVG 0735   0950   321 0^S  E  \r\n>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T1  2:15\r\n3+  MU5112  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1300   1510   333 0^L  E  \r\n>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10                           \r\n『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      \r\n『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         \r\n\r\n 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        \r\n1-  MU5154  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1335   1545   763 0^   E  \r\n>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10\r\n2   MU5156  DS# UC FA P8 JC CQ DQ IQ WQ YA BA  PEKSHA 1830   2045   333 0^   E  \r\n>               MA EA HA KQ LQ NQ RQ SQ VQ TQ GQ ZA QA              T2 T2  2:15\r\n3+  MU5124  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1900   2110   333 0^D  E  \r\n>               MS E3 HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:10                           \r\n『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      \r\n『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』        \r\n\r\n"}
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"DepDate":"\/Date(1465401600000+0800)\/","AVHList":[{"ShareFlight":false,"FlightNo":"MU5138","ShareFlightNoList":[],"FlightModel":"333","SCity":"PEK","ECity":"SHA","STime":"0700","ETime":"0910","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:10","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"MU5183","ShareFlightNoList":[],"FlightModel":"321","SCity":"PEK","ECity":"PVG","STime":"0735","ETime":"0950","DepTerminal":"T2","ArrTerminal":"T1","FlightDuration":"2:15","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"}]},{"ShareFlight":false,"FlightNo":"MU5112","ShareFlightNoList":[],"FlightModel":"333","SCity":"PEK","ECity":"SHA","STime":"1300","ETime":"1510","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:10","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"}]},{"ShareFlight":false,"FlightNo":"MU5154","ShareFlightNoList":[],"FlightModel":"763","SCity":"PEK","ECity":"SHA","STime":"1335","ETime":"1545","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:10","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"}]},{"ShareFlight":false,"FlightNo":"MU5156","ShareFlightNoList":[],"FlightModel":"333","SCity":"PEK","ECity":"SHA","STime":"1830","ETime":"2045","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:15","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"R","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"}]},{"ShareFlight":false,"FlightNo":"MU5124","ShareFlightNoList":[],"FlightModel":"333","SCity":"PEK","ECity":"SHA","STime":"1900","ETime":"2110","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:10","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"3","AVNumberString":"3"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"R","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"Q","AVNumberString":"0"}]}],"ResultBag":"\r\n 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        \r\n1-  MU5138  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0700   0910   333 0^S  E  \r\n>               MA EA HA KA LA NS RA SQ VQ TQ GQ ZS QS              T2 T2  2:10\r\n2   MU5183  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKPVG 0735   0950   321 0^S  E  \r\n>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T1  2:15\r\n3+  MU5112  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1300   1510   333 0^L  E  \r\n>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10                           \r\n『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      \r\n『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         \r\n\r\n 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        \r\n1-  MU5154  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1335   1545   763 0^   E  \r\n>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10\r\n2   MU5156  DS# UC FA P8 JC CQ DQ IQ WQ YA BA  PEKSHA 1830   2045   333 0^   E  \r\n>               MA EA HA KQ LQ NQ RQ SQ VQ TQ GQ ZA QA              T2 T2  2:15\r\n3+  MU5124  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1900   2110   333 0^D  E  \r\n>               MS E3 HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:10                           \r\n『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      \r\n『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』        \r\n\r\n"},"reqtime":"\/Date(1436421199091+0800)\/","SaveTime":1800}
            //string cmdResult =
                string cmdResult3_2_1 =
@"
 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        
1-  MU5138  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0700   0910   333 0^S  E  
>               MA EA HA KA LA NS RA SQ VQ TQ GQ ZS QS              T2 T2  2:10
2   MU5183  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKPVG 0735   0950   321 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T1  2:15
3+  MU5112  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1300   1510   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10                           
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         

 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        
1-  MU5154  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1335   1545   763 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
2   MU5156  DS# UC FA P8 JC CQ DQ IQ WQ YA BA  PEKSHA 1830   2045   333 0^   E  
>               MA EA HA KQ LQ NQ RQ SQ VQ TQ GQ ZA QA              T2 T2  2:15
3+  MU5124  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1900   2110   333 0^D  E  
>               MS E3 HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:10                           
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』        

";

            // 将测
            // 已测
            // 有权限查询AVH指令，且有舱位数：
            // 请求url：http://114.80.69.243:18082/
            // 指令：
            //system("AVH/SHAPEK/10OCT/CA/D");
            //system("PN");
            //system("PN");
            // 返回结果：
            // {"DepDate":"\/Date(1444406400000+0800)\/","AVHList":[{"ShareFlight":false,"FlightNo":"CA1858","ShareFlightNoList":[],"FlightModel":"773","SCity":"SHA","ECity":"PEK","STime":"0755","ETime":"1015","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"5","AVNumberString":"5"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1590","ShareFlightNoList":[],"FlightModel":"773","SCity":"SHA","ECity":"PEK","STime":"0855","ETime":"1115","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1832","ShareFlightNoList":[],"FlightModel":"744","SCity":"SHA","ECity":"PEK","STime":"1055","ETime":"1315","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1502","ShareFlightNoList":[],"FlightModel":"747","SCity":"SHA","ECity":"PEK","STime":"1155","ETime":"1415","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"W","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"3","AVNumberString":"3"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1520","ShareFlightNoList":[],"FlightModel":"773","SCity":"SHA","ECity":"PEK","STime":"1255","ETime":"1515","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1532","ShareFlightNoList":[],"FlightModel":"773","SCity":"SHA","ECity":"PEK","STime":"1355","ETime":"1615","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1558","ShareFlightNoList":[],"FlightModel":"744","SCity":"SHA","ECity":"PEK","STime":"1455","ETime":"1720","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:25","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1884","ShareFlightNoList":[],"FlightModel":"330","SCity":"PVG","ECity":"PEK","STime":"1615","ETime":"1840","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:25","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1518","ShareFlightNoList":[],"FlightModel":"772","SCity":"SHA","ECity":"PEK","STime":"1655","ETime":"1915","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"W","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1522","ShareFlightNoList":[],"FlightModel":"772","SCity":"SHA","ECity":"PEK","STime":"1755","ETime":"2015","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"W","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1516","ShareFlightNoList":[],"FlightModel":"747","SCity":"SHA","ECity":"PEK","STime":"1855","ETime":"2115","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"W","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1550","ShareFlightNoList":[],"FlightModel":"773","SCity":"SHA","ECity":"PEK","STime":"1955","ETime":"2215","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1856","ShareFlightNoList":[],"FlightModel":"33A","SCity":"SHA","ECity":"PEK","STime":"2055","ETime":"2315","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"W","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":true,"FlightNo":"CA3201","ShareFlightNoList":["HO1251"],"FlightModel":"321","SCity":"SHA","ECity":"PEK","STime":"2150","ETime":"0020+1","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:30","CarbinNumberList":[{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1886","ShareFlightNoList":[],"FlightModel":"330","SCity":"SHA","ECity":"PEK","STime":"2155","ETime":"0015+1","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]}],"ResultBag":"\r\n 10OCT(SAT) SHABJS VIA CA DIRECT ONLY                                          \r\n1-  CA1858  DS# P8 FA A2 YA B5 MS HS KS LS QS  SHAPEK 0755   1015   773 0^B  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n2   CA1590  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 0855   1115   773 0^R  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n3   CA1832  DS# PA FA AS YA BS MS HS KS LS QS  SHAPEK 1055   1315   744 0^M  E  \r\n>               GS SS NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n4   CA1502  DS# PA FA A2 WA YA B3 MS HS KS LS  SHAPEK 1155   1415   747 0^M  E  \r\n>               QS GS SA NS VS US TS ES                             T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n5   CA1520  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1255   1515   773 0^M  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n6+  CA1532  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1355   1615   773 0^R  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n\r\n 10OCT(SAT) SHABJS VIA CA DIRECT ONLY                                          \r\n1-  CA1558  DS# PA FA AS YA BS MS HS KS LS QS  SHAPEK 1455   1720   744 0^R  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:25\r\n               ** M1S V1S                                                      \r\n2   CA1884  DS# FA A2 YA BS MS HS KS LS QS GS  PVGPEK 1615   1840   330 0^M  E  \r\n>               SA NS VS US TS ES                                   T2 T3  2:25\r\n               ** M1S V1S                                                      \r\n3   CA1518  DS# FA A2 WA YA BS MS HS KS LS QS  SHAPEK 1655   1915   772 0^M  E  \r\n>               GS SS NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n4   CA1522  DS# FA A2 WA YA BS MS HS KS LS QS  SHAPEK 1755   2015   772 0^M  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n5   CA1516  DS# PA FA A2 WA YA BS MS HS KS LS  SHAPEK 1855   2115   747 0^M  E  \r\n>               QS GS SA NS VS US TS ES                             T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n6+  CA1550  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1955   2215   773 0^R  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n\r\n 10OCT(SAT) SHABJS VIA CA DIRECT ONLY                                          \r\n1-  CA1856  DS# FA A2 WA YA BA MS HS KS LS QS  SHAPEK 2055   2315   33A 0^R  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n2  *CA3201  DS# YA BS HS KS LS QS              SHAPEK 2150   0020+1 321 0^S  E  \r\n>   HO1251                                                          T2 T3  2:30\r\n3+  CA1886  DS# FA A2 YA BA MS HS KS LS QS GS  SHAPEK 2155   0015+1 330 0^   E  \r\n>               SA NS VS US TS ES                                   T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n"}
            //request.SCity = "SHA";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-10-10");
            //request.Airline = "CA";
            //string cmdResult =
            string cmdResult3_3 =
@"
 10OCT(SAT) SHABJS VIA CA DIRECT ONLY                                          
1-  CA1858  DS# P8 FA A2 YA B5 MS HS KS LS QS  SHAPEK 0755   1015   773 0^B  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
2   CA1590  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 0855   1115   773 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
3   CA1832  DS# PA FA AS YA BS MS HS KS LS QS  SHAPEK 1055   1315   744 0^M  E  
>               GS SS NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
4   CA1502  DS# PA FA A2 WA YA B3 MS HS KS LS  SHAPEK 1155   1415   747 0^M  E  
>               QS GS SA NS VS US TS ES                             T2 T3  2:20
               ** M1S V1S                                                      
5   CA1520  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1255   1515   773 0^M  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
6+  CA1532  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1355   1615   773 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      

 10OCT(SAT) SHABJS VIA CA DIRECT ONLY                                          
1-  CA1558  DS# PA FA AS YA BS MS HS KS LS QS  SHAPEK 1455   1720   744 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:25
               ** M1S V1S                                                      
2   CA1884  DS# FA A2 YA BS MS HS KS LS QS GS  PVGPEK 1615   1840   330 0^M  E  
>               SA NS VS US TS ES                                   T2 T3  2:25
               ** M1S V1S                                                      
3   CA1518  DS# FA A2 WA YA BS MS HS KS LS QS  SHAPEK 1655   1915   772 0^M  E  
>               GS SS NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
4   CA1522  DS# FA A2 WA YA BS MS HS KS LS QS  SHAPEK 1755   2015   772 0^M  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
5   CA1516  DS# PA FA A2 WA YA BS MS HS KS LS  SHAPEK 1855   2115   747 0^M  E  
>               QS GS SA NS VS US TS ES                             T2 T3  2:20
               ** M1S V1S                                                      
6+  CA1550  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1955   2215   773 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      

 10OCT(SAT) SHABJS VIA CA DIRECT ONLY                                          
1-  CA1856  DS# FA A2 WA YA BA MS HS KS LS QS  SHAPEK 2055   2315   33A 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
2  *CA3201  DS# YA BS HS KS LS QS              SHAPEK 2150   0020+1 321 0^S  E  
>   HO1251                                                          T2 T3  2:30
3+  CA1886  DS# FA A2 YA BA MS HS KS LS QS GS  SHAPEK 2155   0015+1 330 0^   E  
>               SA NS VS US TS ES                                   T2 T3  2:20
               ** M1S V1S                                                      
";

            // 不测
            //request.SCity = "SHE";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-08-25");            
            // 有权限查询AVH指令，且有舱位数：
            // 请求url：http://114.80.69.243:18082/
            // 指令：
            // system("AVH/SHEPEK/25AUG");
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"AVHList":[{"FlightNo":"CZ6101","CarbinNumberList":[{"Cabin":"J","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"C","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"D","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"O","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"3","AVNumberString":"3"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"K","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"U","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"L","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"E","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"R","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"X","AVNumberTag":"C","AVNumberString":"0"}]},{"FlightNo":"MF1849","CarbinNumberList":[{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"3","AVNumberString":"3"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"K","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"Q","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"1","AVNumberString":"1"}]},{"FlightNo":"CZ6101","CarbinNumberList":[]},{"FlightNo":"CA1602","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"ZH1602","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CA1602","CarbinNumberList":[{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CZ6103","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"4","AVNumberString":"4"},{"Cabin":"W","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"U","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"R","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"X","AVNumberTag":"C","AVNumberString":"0"}]},{"FlightNo":"MF1851","CarbinNumberList":[{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"1","AVNumberString":"1"}]},{"FlightNo":"CZ6103","CarbinNumberList":[]},{"FlightNo":"ZH1652","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"}]},{"FlightNo":"CA1652","CarbinNumberList":[{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"}]}],"ResultBag":"\r\n 25AUG(TUE) SHEBJS                                                             \r\n1-  CZ6101  DS# J8 C1 D1 IQ OQ WS SQ YA B3 MA  SHEPEK 0800   0940   320 0^C  E  \r\n>               H1 K2 U1 LQ Q1 EQ VQ ZQ TQ N1 RQ GQ XC              -- T2  1:40\r\n2  *MF1849  DS# YA B3 MA L1 K1 QQ VQ TQ S1     SHEPEK 0800   0940   320 0^C  E  \r\n>   CZ6101                                                          T3 T2  1:40\r\n3   CA1602  DS# F2 A2 YA BS MS HS KS LS QS GS  SHEPEK 0930   1110   738 0^N  E  \r\n>               SS NS VS US TS ES                                   -- T3  1:40\r\n               ** M1S H1S K1S L1S Q1S V1S                                      \r\n4  *ZH1602  DS# F2 YA BS MS HS KS LS QS GS SS  SHEPEK 0930   1110   738 0^S  E  \r\n>   CA1602      VS US TS ES                                         -- T3  1:40\r\n5   CZ6103 『AS#』FA P4 WS SQ YA BA MA HA KA UA  SHEPEK 1010   1150   330 0^C  E  \r\n>               LQ QA EQ VQ ZQ TQ N1 RQ GQ XC                       -- T2  1:40\r\n6  *MF1851 『AS#』YA BA MA LA KA QQ VQ TQ S1     SHEPEK 1010   1150   330 0^C  E  \r\n>   CZ6103                                                          T3 T2  1:40\r\n7+ *ZH1652  DS# F8 YA BS MS HS KS LS QS GS SA  SHEPEK 1100   1245   738 0^S  E  \r\n>   CA1652      VS US TS ES                                         -- T3  1:45\r\n『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   \r\n『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』\r\n"},"reqtime":"\/Date(1436235214780+0800)\/","SaveTime":1800}
            //string cmdResult =
            string cmdResult4 =
@"
 25AUG(TUE) SHEBJS                                                             
1-  CZ6101  DS# J8 C1 D1 IQ OQ WS SQ YA B3 MA  SHEPEK 0800   0940   320 0^C  E  
>               H1 K2 U1 LQ Q1 EQ VQ ZQ TQ N1 RQ GQ XC              -- T2  1:40
2  *MF1849  DS# YA B3 MA L1 K1 QQ VQ TQ S1     SHEPEK 0800   0940   320 0^C  E  
>   CZ6101                                                          T3 T2  1:40
3   CA1602  DS# F2 A2 YA BS MS HS KS LS QS GS  SHEPEK 0930   1110   738 0^N  E  
>               SS NS VS US TS ES                                   -- T3  1:40
               ** M1S H1S K1S L1S Q1S V1S                                      
4  *ZH1602  DS# F2 YA BS MS HS KS LS QS GS SS  SHEPEK 0930   1110   738 0^S  E  
>   CA1602      VS US TS ES                                         -- T3  1:40
5   CZ6103 『AS#』FA P4 WS SQ YA BA MA HA KA UA  SHEPEK 1010   1150   330 0^C  E  
>               LQ QA EQ VQ ZQ TQ N1 RQ GQ XC                       -- T2  1:40
6  *MF1851 『AS#』YA BA MA LA KA QQ VQ TQ S1     SHEPEK 1010   1150   330 0^C  E  
>   CZ6103                                                          T3 T2  1:40
7+ *ZH1652  DS# F8 YA BS MS HS KS LS QS GS SA  SHEPEK 1100   1245   738 0^S  E  
>   CA1652      VS US TS ES                                         -- T3  1:45
『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   
『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』
";

            // 将测
            // 有权限查询AVH指令，且有舱位数：
            // 请求url：http://114.80.69.243:18082/
            // 指令：
            //system("AVH/PEKPVG/09JUN/D");         
            // 返回结果：
            string cmdResult4_1 =
@"
 09JUN16(THU) BJSPVG DIRECT ONLY                                               
1-  MU5183  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKPVG 0735   0950   321 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T1  2:15
2   CA1835  DS# FZ AZ OZ GZ EZ YA BS MS US HS  PEKPVG 0800   1015   333 0^S  E  
>               QS VS WS SS TA LS NS KS                             T3 T2  2:15
               ** M1S V1S                                                      
3   MU271   DS# UC FC PC JA CQ DQ IQ WQ YA B1  PEKPVG 1355   1555   321 0^   E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:00
4   MU563   DS# UC FC PC JA CQ DQ IQ WQ YA BA  PEKPVG 1620   1840   321 0^   E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:20
5   MU5130  DS# UC F8 PQ JC CQ DQ IQ WQ YA BA  PEKPVG 1900   2120   320 0^D  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QQ              T2 T1  2:20
6+  CA1883  DS# FA A2 YA BS MS US HS QS VS WS  PEKPVG 2010   2225   330 0^B  E  
>               SS TA LS NS KS                                      T3 T2  2:15
               ** M1S V1S                                                      
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』  
";

            // 将测
            // 未测，但测了cmdResult3_2_1这个替代例子
            // 有权限查询AVH指令，且有舱位数：
            // 请求url：http://114.80.69.243:18082/
            // 指令：
            //system("AVH/PEKSHA/09JUN/D");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            // 返回结果：
            string cmdResult4_2 =
@"
 09JUN16(THU) BJSSHA DIRECT ONLY                                               
1-  MU5138  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0700   0910   333 0^S  E  
>               MA EA HA KA LA NS RA SQ VQ TQ GQ ZS QS              T2 T2  2:10
2   FM9108  DS# UC F8 P8 JC CQ DQ IQ WQ YA BA  PEKSHA 0730   0940   738 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
3   CA1831  DS# PA FA AA YA BS MS US HS QS VS  PEKSHA 0730   0940   744 0^B  E  
>               WS SS TA LS NS KS                                   T3 T2  2:10
               ** M1S V1S                                                      
4   MU5183  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKPVG 0735   0950   321 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T1  2:15
5   MU5102  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0800   0955   333 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  1:55
6   CA1835  DS# FZ AZ OZ GZ EZ YA BS MS US HS  PEKPVG 0800   1015   333 0^S  E  
>               QS VS WS SS TA LS NS KS                             T3 T2  2:15
               ** M1S V1S                                                      
7   HU7605  DS# F8 ZQ PQ AQ YA BQ HQ KQ LQ MQ  PEKSHA 0825   1030   738 0^   E  
>               QQ XQ UQ EQ TQ WQ VQ GQ OQ SQ                       T1 T2  2:05
8+  CA1501  DS# PA FA AA YA BA MA UA HA QA VS  PEKSHA 0830   1040   773 0^B  E  
>               WS SS TA LS NS KS                                   T3 T2  2:10
               ** M1A V1S                                                      
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』                                       
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         

 09JUN16(THU) BJSSHA DIRECT ONLY                                               
1- *HA3822  DS# FA JA PA CA AA YA WA XA QA VA  PEKSHA 0830   1040   773 0    E  
>   CA1501      BA SA NA MA IA HA GA KA LA ZA OA                    T3 T2  2:10
2   CZ3907  DS# AC FC PQ JA CQ DQ IQ OQ WA SQ  PEKSHA 0840   1045   321 0^C  E  
>               YA BQ MQ HQ KA UQ LQ QA EQ VQ ZQ TQ N2 RQ GQ XC     T2 T2  2:05
3   MU5104  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0900   1110   333 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
4  *HA3824  DS# FA JA PA CA AA YA WA XA QA VA  PEKSHA 0930   1140   773 0    E  
>   CA1519      BA SA NA MA IA HA GA KA LA ZA OA                    T3 T2  2:10
5   CA1519  DS# PA FA AA YA BA MA UA HA QA VS  PEKSHA 0930   1140   773 0^S  E  
>               WS SS TA LS NS KS                                   T3 T2  2:10
               ** M1A V1S                                                      
6   MU5106  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1000   1210   333 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
7   CA1531  DS# PA FA A2 YA BS MS US HS QS VS  PEKSHA 1030   1235   773 0^S  E  
>               WS SS TA LS NS KS                                   T3 T2  2:05
               ** M1S V1S                                                      
8+  MU5108  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1100   1310   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』                                                                  
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         

 09JUN16(THU) BJSSHA DIRECT ONLY                                               
1-  MU5152  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1130   1340   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
2   CA1557  DS# PA FA A2 YA BS MS US HS QS VS  PEKSHA 1130   1340   744 0^L  E  
>               WS SS TA LS NS KS                                   T3 T2  2:10
               ** M1S V1S                                                      
3   MU5110  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1200   1410   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QQ              T2 T2  2:10
4   MU5112  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1300   1510   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
5   CA1517  DS# FA A2 GA ES YA BS MS US HS QS  PEKSHA 1330   1540   772 0^L  E  
>               VS WS SS TA LS NS KS                                T3 T2  2:10
               ** M1S V1S                                                      
6   MU5154  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1335   1545   763 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
7   MU271   DS# UC FC PC JA CQ DQ IQ WQ YA B1  PEKPVG 1355   1555   321 0^   E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:00
8+  MU5114  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1400   1620   333 0^   E  
>               MS ES HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:20
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』                                                                  
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         

 09JUN16(THU) BJSSHA DIRECT ONLY                                               
1-  CA1521  DS# FA A2 GA ES YA BS MS US HS QS  PEKSHA 1430   1640   772 0^S  E  
>               VS WS SS TA LS NS KS                                T3 T2  2:10
               ** M1S V1S                                                      
2   MU5116  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1500   1710   333 0^   E  
>               MS ES HS KS LS NS RS SQ VQ TQ GQ ZS QS              T2 T2  2:10
3   CA1515  DS# PA FA A2 YA BS MS US HS QS VS  PEKSHA 1530   1740   773 0^S  E  
>               WS SS TA LS NS KS                                   T3 T2  2:10
               ** M1S V1S                                                      
4   MU563   DS# UC FC PC JA CQ DQ IQ WQ YA BA  PEKPVG 1620   1840   321 0^   E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:20
5   CA1549  DS# PA FA A2 YA BS MS US HS QS VS  PEKSHA 1630   1840   773 0^S  E  
>               WS SS TA LS NS KS                                   T3 T2  2:10
               ** M1S V1S                                                      
6   MU5120  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1700   1915   333 0^D  E  
>               MS ES HS KS LS NS RS SQ VQ TQ GQ ZS QS              T2 T2  2:15
7+  HU7601  DS# F8 ZQ PQ AQ YA BQ HQ KQ LQ MQ  PEKSHA 1705   1915   738 0^   E  
>               QQ XQ UQ EQ TQ WQ VQ GQ OQ SQ                       T1 T2  2:10
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         

 09JUN16(THU) BJSSHA DIRECT ONLY                                               
1-  CA1855  DS# FA A2 GA ES YA BS MS US HS QS  PEKSHA 1730   1940   33A 0^D  E  
>               VS WS SS TA LS NS KS                                T3 T2  2:10
               ** M1S V1S                                                      
2   MU5122  DS# UC FA PQ JC CC DC IC WQ YA BA  PEKSHA 1800   2010   333 0^D  E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10
3   CA1885  DS# FA A2 YA BS MS US HS QS VS WS  PEKSHA 1830   2040   330 0^S  E  
>               SS TA LS NS KS                                      T3 T2  2:10
               ** M1S V1S                                                      
4   MU5156  DS# UC FA P8 JC CQ DQ IQ WQ YA BA  PEKSHA 1830   2045   333 0^   E  
>               MA EA HA KQ LQ NQ RQ SQ VQ TQ GQ ZA QA              T2 T2  2:15
5   MU5124  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1900   2110   333 0^D  E  
>               MS E3 HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:10
6   MU5130  DS# UC F8 PQ JC CQ DQ IQ WQ YA BA  PEKPVG 1900   2120   320 0^D  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QQ              T2 T1  2:20
7   CA1857  DS# PA FA A2 YA BS MS US HS QS VS  PEKSHA 1930   2155   744 0^D  E  
>               WS SS TA LS NS KS                                   T3 T2  2:25
               ** M1S V1S                                                      
8+  MU5126  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 2000   2210   333 0^   E  
>               MS E3 HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:10
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』                                       
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         

 09JUN16(THU) BJSSHA DIRECT ONLY                                               
1-  CA1883  DS# FA A2 YA BS MS US HS QS VS WS  PEKPVG 2010   2225   330 0^B  E  
>               SS TA LS NS KS                                      T3 T2  2:15
               ** M1S V1S                                                      
2   CA1589  DS# PA FA A2 GA ES YA BS MS US HS  PEKSHA 2030   2240   747 0^S  E  
>               QS VS WS SS TA LS NS KS                             T3 T2  2:10
               ** M1S V1S                                                      
3   FM9106  DS# UC F8 P8 JC CQ DQ IQ WQ YA BA  PEKSHA 2030   2245   73E 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:15
4   HU7603  DS# F8 ZQ PQ AQ YA BQ HQ KQ LQ MQ  PEKSHA 2050   2255   738 0^   E  
>               QQ XQ UQ EQ TQ WQ VQ GQ OQ SQ                       T1 T2  2:05
5   MU5128  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 2100   2315   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:15
6   MU5158  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 2130   2340   763 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
7+  MU5160  DS# UC FA PA JC CC DC IC W4 YA BA  PEKSHA 2200   2355   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  1:55
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         
";

            // 将测
            // 没测，但测了cmdResult4_3_1和cmdResult4_3_2这两个替代案例
            // 有权限查询AVH指令，且有舱位数：
            // 重点测：
            // 请求url：http://114.80.69.243:18082/
            // 指令：
            //system("AVH/SHAPEK/10OCT/D");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            // 返回结果：
            //request.SCity = "SHA";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-10-10");           
            //string cmdResult =
            string cmdResult4_3 =
@"
 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  KN5988  DS# JS CQ DQ IQ W8 OQ YA BA MQ EQ  PVGNAY 0710   0910   73E 0^   E  
>               HQ KQ LQ NQ RA GQ QQ XQ                             T1 --  2:00
2  *MU3812  DS# JS CQ DQ IQ W8 OQ YA BA MQ EQ  PVGNAY 0710   0910   73E 0^   E  
>   KN5988      HQ KQ LQ NQ RA SQ VQ GQ QQ XQ                       T1 --  2:00
3  *ZH1858  DS# FA YA B5 MS HS KS LS QS GS SA  SHAPEK 0755   1015   773 0^B  E  
>   CA1858      VS US TS ES                                         T2 T3  2:20
4   CA1858  DS# P8 FA A2 YA B5 MS HS KS LS QS  SHAPEK 0755   1015   773 0^B  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
5  *CZ9270  DS# YA BA MA UA LQ EQ              SHAPEK 0800   1020   333 0^S  E  
>   MU5101                                                          T2 T2  2:20
6   MU5101  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 0800   1020   333 0^S  E  
>               MA EA HA KA LA NA RQ SQ VQ TQ GQ ZA QA              T2 T2  2:20
7   MU5151  DS# UC FA P8 JL CQ DQ IQ WQ YA BA  SHAPEK 0830   1035   321 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:05
8  *CZ9310  DS# YA BA MA UA LA EQ              SHAPEK 0830   1035   321 0^S  E  
>   MU5151                                                          T2 T2  2:05
9+  HU7604  DS# F8 Z4 P2 AQ YA BA HA KA LA MA  SHAPEK 0830   1035   738 0^B  E  
>               QA XQ UQ EQ TQ V5 WQ GQ OQ S6                       T2 T1  2:05         

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1- *ZH1590  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 0855   1115   773 0^B  E  
>   CA1590      VS US TS ES                                         T2 T3  2:20
2   CA1590  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 0855   1115   773 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
3  *CZ9272  DS# YA BA MA UA LA EQ              SHAPEK 0900   1120   333 0^   E  
>   MU5103                                                          T2 T2  2:20
4   MU5103  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 0900   1120   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:20
5  *QF4011  DS! J9 C9 D9 I9 Y9 B9 H9 K9 M9 L9  PVGPEK 0920   1155   320 0 N  E  
>   MU5129      V9 S9 N9 Q9 O9                                      T1 T2  2:35
6   MU5129  DS# UL F8 P6 JC CQ DQ IQ WQ YA BA  PVGPEK 0920   1155   320 0^L  E  
>               MA EA HA KA LA NA RS SQ VQ TQ GQ ZA QA              T1 T2  2:35
7  *CZ9298  DS# YA BA MA UA LS EQ              PVGPEK 0920   1155   320 0^L  E  
>   MU5129                                                          T1 T2  2:35
8  *CZ9312  DS# YA BA MA UA LA EQ              SHAPEK 0930   1155   763 0^   E  
>   MU5153                                                          T2 T2  2:25
9+  MU5153  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 0930   1155   763 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25         

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  MU5105  DS# UL FA PA JC CC DC IC WQ YA BA  SHAPEK 1000   1220   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:20
2  *CZ9274  DS# YA BA MA UA LA EQ              SHAPEK 1000   1220   333 0^   E  
>   MU5105                                                          T2 T2  2:20
3  *MU3810  DS# J7 CQ DQ IQ W7 OQ YA BA MQ EQ  PVGNAY 1010   1155   73E 0^   E  
>   KN5978      HQ KQ LQ NQ RA SQ VQ GQ QQ XQ                       T1 --  1:45
4   KN5978  DS# J7 CQ DQ IQ W7 OQ YA BA MQ EQ  PVGNAY 1010   1155   73E 0^   E  
>               HQ KQ LQ NQ RA GQ QQ XQ                             T1 --  1:45
5  *ZH1832  DS# FA YA BS MS HS KS LS QS GS SS  SHAPEK 1055   1315   744 0^S  E  
>   CA1832      VS US TS ES                                         T2 T3  2:20
6   CA1832  DS# PA FA AS YA BS MS HS KS LS QS  SHAPEK 1055   1315   744 0^M  E  
>               GS SS NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
7   MU5107  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1100   1320   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:20
8+ *CZ9276  DS# YA BA MA UA LA EQ              SHAPEK 1100   1320   333 0^L  E  
>   MU5107                                                          T2 T2  2:20

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  HU7606  DS# FA Z8 P4 A4 YA BA HA KA LA MA  SHAPEK 1130   1345   333 0^   E  
>               QA XQ UQ EQ TQ V5 WQ GQ OQ SA                       T2 T1  2:15
               ** M1A Q1A                                                      
2   CZ3908  DS# FA PQ WA SQ YA BQ MQ HQ KA UQ  SHAPEK 1150   1415   330 0^L  E  
>               LQ QA EQ VQ ZQ TQ N1 RS GQ XC                       T2 T2  2:25
3  *MU3492  DS# YA MQ HQ KQ RQ SQ              SHAPEK 1150   1415   330 0^L  E  
>   CZ3908                                                          T2 T2  2:25
4  *MF1764  DS# YA BQ MQ LQ KQ QQ VQ TQ S1     SHAPEK 1150   1415   330 0^L  E  
>   CZ3908                                                          T2 T2  2:25
5  *HO1902  DS# YA B3 MS TS ES VS              SHAPEK 1155   1415   747 0^L  E  
>   CA1502                                                          T2 T3  2:20
6  *ZH1502  DS# FA YA B3 MS HS KS LS QS GS SA  SHAPEK 1155   1415   747 0^L  E  
>   CA1502      VS US TS ES                                         T2 T3  2:20
7   CA1502  DS# PA FA A2 WA YA B3 MS HS KS LS  SHAPEK 1155   1415   747 0^M  E  
>               QS GS SA NS VS US TS ES                             T2 T3  2:20
               ** M1S V1S                                                      
8+ *CZ9278  DS# YA BA MA UA LA EQ              SHAPEK 1200   1425   333 0^L  E  
>   MU5109                                                          T2 T2  2:25

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  MU5109  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1200   1425   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25
2   HU7608  DS# FA ZA P4 A8 YA BA HA KA LA MA  SHAPEK 1220   1435   787 0^L  E  
>               QA XQ UQ EQ TQ V5 WQ GQ OQ SA                       T2 T1  2:15
               ** M1A Q1A                                                      
3  *QF4013  DS! J9 C9 D9 I9 Y9 B9 H9 K9 M9 L9  PVGPEK 1225   1445   321 0 L  E  
>   MU564       V9 S9 NL QL OL                                      T1 T2  2:20
4   MU564   DS# UL FL PL JA CQ DQ IQ WC YA BA  PVGPEK 1225   1445   321 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QQ              T1 T2  2:20
5  *ZH1520  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1255   1515   773 0^L  E  
>   CA1520      VS US TS ES                                         T2 T3  2:20
6   CA1520  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1255   1515   773 0^M  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
7  *CZ9280  DS# YA BA MA UA LA EQ              SHAPEK 1300   1520   333 0^L  E  
>   MU5111                                                          T2 T2  2:20
8+  MU5111  DS# UC FA PA JL CL DL IL WQ YA BA  SHAPEK 1300   1520   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:20

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  FM9103  DS# UC F8 P8 JL CQ DQ IQ WQ YA BA  SHAPEK 1330   1555   73E 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25
2  *MU9103  DS# UC F8 P8 A3 JL CQ DQ IQ OQ YA  SHAPEK 1330   1555   73E 0^   E  
>   FM9103      BA MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA X5        T2 T2  2:25
3   MU272   DS# UC FC PC JA CQ DQ IQ WL YA BA  PVGPEK 1340   1620   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA Q1              T1 T2  2:40
4   CA1532  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1355   1615   773 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
5  *ZH1532  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1355   1615   773 0^S  E  
>   CA1532      VS US TS ES                                         T2 T3  2:20
6  *CZ9282  DS# YA BA MA UA LA EQ              SHAPEK 1400   1625   333 0^   E  
>   MU5113                                                          T2 T2  2:25
7   MU5113  DS# UL FA PA JC CC DC IC WQ YA BA  SHAPEK 1400   1625   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25
8   MU5155  DS# UC FA P8 JL CQ DQ IQ WQ YA BA  SHAPEK 1430   1650   321 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:20
9+ *CZ9314  DS# YA BA MA UA LA EQ              SHAPEK 1430   1650   321 0^   E  
>   MU5155                                                          T2 T2  2:20         

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  CA1558  DS# PA FA AS YA BS MS HS KS LS QS  SHAPEK 1455   1720   744 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:25
               ** M1S V1S                                                      
2  *ZH1558  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1455   1720   744 0^S  E  
>   CA1558      VS US TS ES                                         T2 T3  2:25
3   MF8177 『AS#』                               SHAPEK 1500   1715   738 0^S  E  
>                                                                   T2 T2  2:15
4   MU5115  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1500   1715   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:15
5  *CZ9284  DS# YA BA MA UA LA EQ              SHAPEK 1500   1715   333 0^   E  
>   MU5115                                                          T2 T2  2:15
6  *MU8570 『AS#』YA MQ RQ SQ VQ                 SHAPEK 1500   1715   738 0^S  E  
>   MF8177                                                          T2 T2  2:15
7  *NS8177 『AS#』YA BQ MQ LQ KQ NQ QQ VQ TQ     SHAPEK 1500   1715   738 0^S  E  
>   MF8177                                                          T2 T2  2:15
8  *CZ5137 『AS#』YA BQ MQ HQ UQ LQ EQ ZQ N5     SHAPEK 1500   1715   738 0^S  E  
>   MF8177                                                          T2 T2  2:15
9+  MU5117  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1600   1825   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25         

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1- *CZ9286  DS# YA BA MA UA LA EQ              SHAPEK 1600   1825   333 0^L  E  
>   MU5117                                                          T2 T2  2:25
2  *ZH1884  DS# FA YA BS MS HS KS LS QS GS SA  PVGPEK 1615   1840   330 0^S  E  
>   CA1884      VS US TS ES                                         T2 T3  2:25
3   CA1884  DS# FA A2 YA BS MS HS KS LS QS GS  PVGPEK 1615   1840   330 0^M  E  
>               SA NS VS US TS ES                                   T2 T3  2:25
               ** M1S V1S                                                      
4  *ZH1518  DS# FA YA BS MS HS KS LS QS GS SS  SHAPEK 1655   1915   772 0^S  E  
>   CA1518      VS US TS ES                                         T2 T3  2:20
5  *HO1904  DS# YA BS MS TS ES VS              SHAPEK 1655   1915   772 0^S  E  
>   CA1518                                                          T2 T3  2:20
6   CA1518  DS# FA A2 WA YA BS MS HS KS LS QS  SHAPEK 1655   1915   772 0^M  E  
>               GS SS NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
7  *CZ9288  DS# YA BA MA UA LA EQ              SHAPEK 1700   1925   333 0^D  E  
>   MU5119                                                          T2 T2  2:25
8+  MU5119  DS# UC FA PA JL CQ DQ IQ WQ YA BA  SHAPEK 1700   1925   333 0^D  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1- *CZ9316  DS# YA BA MA UA LA EA              SHAPEK 1730   1950   763 0^D  E  
>   MU5157                                                          T2 T2  2:20
2   MU5157  DS# UL FA P8 JC CQ DQ IQ WQ YA BA  SHAPEK 1730   1950   763 0^D  E  
>               MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA              T2 T2  2:20
3   CA1522  DS# FA A2 WA YA BS MS HS KS LS QS  SHAPEK 1755   2015   772 0^M  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
4  *ZH1522  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1755   2015   772 0^D  E  
>   CA1522      VS US TS ES                                         T2 T3  2:20
5   MU5121  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1800   2020   333 0^D  E  
>               MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA              T2 T2  2:20
6  *CZ9290  DS# YA BA MA UA LA EA              SHAPEK 1800   2020   333 0^D  E  
>   MU5121                                                          T2 T2  2:20
7   KN5956  DS# JQ CQ DQ IQ W8 OQ YA BA MA EA  SHANAY 1830   2035   73E 0^   E  
>               HA KA LA NA RA GQ QQ XQ                             T2 --  2:05
8+ *MU3724  DS# JQ CQ DQ IQ W8 OQ YA BA MA EA  SHANAY 1830   2035   73E 0^   E  
>   KN5956      HA KA LA NA RA SA VA GQ QQ XQ                       T2 --  2:05

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  CA1516  DS# PA FA A2 WA YA BS MS HS KS LS  SHAPEK 1855   2115   747 0^M  E  
>               QS GS SA NS VS US TS ES                             T2 T3  2:20
               ** M1S V1S                                                      
2  *ZH1516  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1855   2115   747 0^D  E  
>   CA1516      VS US TS ES                                         T2 T3  2:20
3   MU5123  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1900   2125   333 0^D  E  
>               MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA              T2 T2  2:25
4  *CZ9292  DS# YA BA MA UA LA EA              SHAPEK 1900   2125   333 0^D  E  
>   MU5123                                                          T2 T2  2:25
5   CA1550  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1955   2215   773 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
6  *HA3823  DS! J7 P7 C7 A7 Y0 W0 X0 Q0 V0 B0  SHAPEK 1955   2215   773 0    E  
>   CA1550      S0 N0 M0 I0 H0 G0 K0 L0 Z0 O0                       T2 T3  2:20
7  *ZH1550  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1955   2215   773 0^D  E  
>   CA1550      VS US TS ES                                         T2 T3  2:20
8+  HU7602  DS# FA Z8 P4 A4 YA BA HA KA LA MA  SHAPEK 2020   2235   767 0^   E  
>               QA XA UQ EQ TQ V5 WQ GQ OQ SA                       T2 T1  2:15
               ** M1A Q1A                                                      

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1- *ZH1856  DS# FA YA BA MS HS KS LS QS GS SA  SHAPEK 2055   2315   33A 0^S  E  
>   CA1856      VS US TS ES                                         T2 T3  2:20
2   CA1856  DS# FA A2 WA YA BA MS HS KS LS QS  SHAPEK 2055   2315   33A 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
3  *QF4009  DS! J9 C9 D9 I9 Y9 B9 H9 K9 M9 L9  PVGPEK 2110   2340   321 0 N  E  
>   MU5186      VL SL N9 Q9 O9                                      T1 T2  2:30
4   MU5186  DS# UC FA PA JL CQ DQ IQ WQ YA BA  PVGPEK 2110   2340   321 0^   E  
>               MA EA HA KQ LQ NQ RQ SQ VQ TQ GQ ZS QA              T1 T2  2:30
5  *MU9107  DS# UC F8 P8 A3 JL CQ DQ IQ OQ YA  SHAPEK 2130   2345   738 0^   E  
>   FM9107      BA MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA X5        T2 T2  2:15
6   FM9107  DS# UC F8 P8 JL CQ DQ IQ WQ YA BA  SHAPEK 2130   2345   738 0^   E  
>               MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA              T2 T2  2:15
7  *CA3201  DS# YA BS HS KS LS QS              SHAPEK 2150   0020+1 321 0^S  E  
>   HO1251                                                          T2 T3  2:30
8  *MU3925  DS# YA MS ES HS                    SHAPEK 2150   0020+1 321 0^S  E  
>   HO1251                                                          T2 T2  2:30
9+  HO1251  DS# FA AS JS YA BS LS MS TS ES HS  SHAPEK 2150   0020+1 321 0^S  E  
>               VS KS WS RS QS ZS PS XS GS SS IS NS                 T2 T3  2:30         

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  CA1886  DS# FA A2 YA BA MS HS KS LS QS GS  SHAPEK 2155   0015+1 330 0^   E  
>               SA NS VS US TS ES                                   T2 T3  2:20
               ** M1S V1S                                                      
2  *ZH1886  DS# FA YA BA MS HS KS LS QS GS SA  SHAPEK 2155   0015+1 330 0^S  E  
>   CA1886      VS US TS ES                                         T2 T3  2:20
3  *MU3657  DS# YA MQ HQ KQ RQ SQ              SHAPEK 2230   0035+1 321 0^C  E  
>   CZ6411                                                          T2 T2  2:05
4   CZ6411  DS# JA CQ DQ IQ OQ WA SQ YA BQ MQ  SHAPEK 2230   0035+1 321 0^C  E  
>               HQ KA UQ LQ QA EQ VQ ZQ TQ N2 RQ GQ XC              T2 T2  2:05
5  *MF4737  DS# YA BQ MQ LQ KQ QQ VQ TQ S2     SHAPEK 2230   0035+1 321 0^C  E  
>   CZ6411                                                          T2 T2  2:05
6+  HU7610  DS# FA ZA P4 A8 YA BA HA KA LA MA  PVGPEK 2230   0100+1 331 0^   E  
>               QA XA UA EA TQ V5 WQ GQ OQ SA                       T2 T1  2:30
               ** M1A Q1A                                                      
";

            // 已测
            // 返回结果：
            // {"DepDate":"\/Date(1444406400000+0800)\/","AVHList":[{"ShareFlight":false,"FlightNo":"KN5988","ShareFlightNoList":[],"FlightModel":"73E","SCity":"PVG","ECity":"NAY","STime":"0710","ETime":"0910","DepTerminal":"T1","ArrTerminal":"--","FlightDuration":"2:00","CarbinNumberList":[{"Cabin":"J","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"O","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"X","AVNumberTag":"Q","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1858","ShareFlightNoList":[],"FlightModel":"773","SCity":"SHA","ECity":"PEK","STime":"0755","ETime":"1015","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"5","AVNumberString":"5"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":true,"FlightNo":"CZ9270","ShareFlightNoList":["MU5101"],"FlightModel":"333","SCity":"SHA","ECity":"PEK","STime":"0800+1","ETime":"1020","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"U","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"Q","AVNumberString":"0"}]},{"ShareFlight":true,"FlightNo":"CZ9310","ShareFlightNoList":["MU5151"],"FlightModel":"321","SCity":"SHA","ECity":"PEK","STime":"0830","ETime":"1035","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:05","CarbinNumberList":[{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"U","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"Q","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"HU7604","ShareFlightNoList":[],"FlightModel":"738","SCity":"SHA","ECity":"PEK","STime":"0830","ETime":"1035","DepTerminal":"T2","ArrTerminal":"T1","FlightDuration":"2:05","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"Z","AVNumberTag":"4","AVNumberString":"4"},{"Cabin":"P","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"A","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"X","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"5","AVNumberString":"5"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"O","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"6","AVNumberString":"6"}]},{"ShareFlight":true,"FlightNo":"ZH1590","ShareFlightNoList":["MU564"],"FlightModel":"773","SCity":"SHA","ECity":"PEK","STime":"0855","ETime":"1115","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1590","ShareFlightNoList":[],"FlightModel":"773","SCity":"SHA","ECity":"PEK","STime":"0855","ETime":"1115+1","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":true,"FlightNo":"QF4013","ShareFlightNoList":["MU564"],"FlightModel":"","SCity":"PVG","ECity":"PEK","STime":"","ETime":"","DepTerminal":"T1","ArrTerminal":"T2","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"J","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"C","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"D","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"I","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"Y","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"B","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"H","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"K","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"M","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"L","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"22","AVNumberTag":"5","AVNumberString":"5"},{"Cabin":"44","AVNumberTag":"5","AVNumberString":"5"},{"Cabin":"32","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"V","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"S","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"N","AVNumberTag":"L","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"L","AVNumberString":"0"},{"Cabin":"O","AVNumberTag":"L","AVNumberString":"0"}]},{"ShareFlight":true,"FlightNo":"MU8570","ShareFlightNoList":["MF8177"],"FlightModel":"738","SCity":"SHA","ECity":"PEK","STime":"1500","ETime":"1715","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:15","CarbinNumberList":[{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"R","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"MU5153","ShareFlightNoList":[],"FlightModel":"763","SCity":"SHA","ECity":"PEK","STime":"0930","ETime":"1155","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:25","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"L","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"}]},{"ShareFlight":false,"FlightNo":"CA1558","ShareFlightNoList":[],"FlightModel":"744","SCity":"SHA","ECity":"PEK","STime":"1455","ETime":"1720","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:25","CarbinNumberList":[{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":true,"FlightNo":"ZH1558","ShareFlightNoList":["CA1558"],"FlightModel":"744","SCity":"SHA","ECity":"PEK","STime":"1455","ETime":"1720","DepTerminal":"T2","ArrTerminal":"T3","FlightDuration":"2:25","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":true,"FlightNo":"CZ5137","ShareFlightNoList":["MF8177"],"FlightModel":"738","SCity":"SHA","ECity":"PEK","STime":"1500","ETime":"1715","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:15","CarbinNumberList":[{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"5","AVNumberString":"5"}]},{"ShareFlight":false,"FlightNo":"MU5117","ShareFlightNoList":[],"FlightModel":"333","SCity":"SHA","ECity":"PEK","STime":"1600","ETime":"1825","DepTerminal":"T2","ArrTerminal":"T2","FlightDuration":"2:25","CarbinNumberList":[{"Cabin":"U","AVNumberTag":"L","AVNumberString":"0"},{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"J","AVNumberTag":"C","AVNumberString":"0"},{"Cabin":"C","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"D","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"R","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"}]}],"ResultBag":"\r\n 10OCT(SAT) SHABJS DIRECT ONLY                                                 \r\n1-  KN5988  DS# JS CQ DQ IQ W8 OQ YA BA MQ EQ  PVGNAY 0710   0910   73E 0^   E  \r\n>               HQ KQ LQ NQ RA GQ QQ XQ                             T1 --  2:00\r\n2   CA1858  DS# P8 FA A2 YA B5 MS HS KS LS QS  SHAPEK 0755   1015   773 0^B  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S                                                      \r\n3  *CZ9270  DS# YA BA MA UA LQ EQ              SHAPEK 0800+1   1020   333 0^S  E  \r\n>   MU5101                                                          T2 T2  2:20\r\n4  *CZ9310  DS# YA BA MA UA LA EQ              SHAPEK 0830   1035   321 0^S  E  \r\n>   MU5151                                                          T2 T2  2:05\r\n5+  HU7604  DS# F8 Z4 P2 AQ YA BA HA KA LA MA  SHAPEK 0830   1035   738 0^B  E  \r\n>               QA XQ UQ EQ TQ V5 WQ GQ OQ S6                       T2 T1  2:05         \r\n\r\n 10OCT(SAT) SHABJS DIRECT ONLY                                                 \r\n1- *ZH1590  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 0855   1115   773 0^B  E  \r\n>   MU564       VS US TS ES                                         T2 T3  2:20\r\n2   CA1590  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 0855   1115+1   773 0^R  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:20\r\n               ** M1S V1S    \r\n3  *QF4013  DS! J9 C9 D9 I9 Y9 B9 H9 K9 M9 L9  PVGPEK 1225   1445   321 0 L  E  \r\n>   MU564       V9 S9 NL QL OL                                      T1 T2  2:20\t\t\r\n4  *MU8570 『AS#』YA MQ RQ SQ VQ                 SHAPEK 1500   1715   738 0^S  E  \r\n>   MF8177                                                          T2 T2  2:15\t   \r\n5+  MU5153  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 0930   1155   763 0^   E  \r\n>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25\r\n\r\n 10OCT(SAT) SHABJS DIRECT ONLY                                                 \r\n1-  CA1558  DS# PA FA AS YA BS MS HS KS LS QS  SHAPEK 1455   1720   744 0^R  E  \r\n>               GS SA NS VS US TS ES                                T2 T3  2:25\r\n               ** M1S V1S                                                      \r\n2  *ZH1558  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1455   1720   744 0^S  E  \r\n>   CA1558      VS US TS ES                                         T2 T3  2:25\r\n3  *CZ5137 『AS#』YA BQ MQ HQ UQ LQ EQ ZQ N5     SHAPEK 1500   1715   738 0^S  E  \r\n>   MF8177                                                          T2 T2  2:15\r\n4+  MU5117  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1600   1825   333 0^L  E  \r\n>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25 \r\n"}
            //request.SCity = "SHA";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-10-10"); 
            //string cmdResult =
            string cmdResult4_3_1 =
@"
 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  KN5988  DS# JS CQ DQ IQ W8 OQ YA BA MQ EQ  PVGNAY 0710   0910   73E 0^   E  
>               HQ KQ LQ NQ RA GQ QQ XQ                             T1 --  2:00
2   CA1858  DS# P8 FA A2 YA B5 MS HS KS LS QS  SHAPEK 0755   1015   773 0^B  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
3  *CZ9270  DS# YA BA MA UA LQ EQ              SHAPEK 0800+1   1020   333 0^S  E  
>   MU5101                                                          T2 T2  2:20
4  *CZ9310  DS# YA BA MA UA LA EQ              SHAPEK 0830   1035   321 0^S  E  
>   MU5151                                                          T2 T2  2:05
5+  HU7604  DS# F8 Z4 P2 AQ YA BA HA KA LA MA  SHAPEK 0830   1035   738 0^B  E  
>               QA XQ UQ EQ TQ V5 WQ GQ OQ S6                       T2 T1  2:05         

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1- *ZH1590  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 0855   1115   773 0^B  E  
>   MU564       VS US TS ES                                         T2 T3  2:20
2   CA1590  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 0855   1115+1   773 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S    
3  *QF4013  DS! J9 C9 D9 I9 Y9 B9 H9 K9 M9 L9  PVGPEK 1225   1445   321 0 L  E  
>   MU564       V9 S9 NL QL OL                                      T1 T2  2:20		
4  *MU8570 『AS#』YA MQ RQ SQ VQ                 SHAPEK 1500   1715   738 0^S  E  
>   MF8177                                                          T2 T2  2:15	   
5+  MU5153  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 0930   1155   763 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  CA1558  DS# PA FA AS YA BS MS HS KS LS QS  SHAPEK 1455   1720   744 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:25
               ** M1S V1S                                                      
2  *ZH1558  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1455   1720   744 0^S  E  
>   CA1558      VS US TS ES                                         T2 T3  2:25
3  *CZ5137 『AS#』YA BQ MQ HQ UQ LQ EQ ZQ N5     SHAPEK 1500   1715   738 0^S  E  
>   MF8177                                                          T2 T2  2:15
4+  MU5117  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1600   1825   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25 
";

            // 已测
            // 返回结果：
            // {"DepDate":"\/Date(1444406400000+0800)\/","AVHList":[{"ShareFlight":true,"FlightNo":"QF4013","ShareFlightNoList":["MU564"],"FlightModel":"321","SCity":"PVG","ECity":"PEK","STime":"1225","ETime":"1445","DepTerminal":"T1","ArrTerminal":"T2","FlightDuration":"2:20","CarbinNumberList":[{"Cabin":"J","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"C","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"D","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"I","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"Y","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"B","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"H","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"K","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"M","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"L","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"V","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"S","AVNumberTag":"9","AVNumberString":"9"},{"Cabin":"N","AVNumberTag":"L","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"L","AVNumberString":"0"},{"Cabin":"O","AVNumberTag":"L","AVNumberString":"0"}]}],"ResultBag":"\r\n 10OCT(SAT) SHABJS DIRECT ONLY                                                 \r\n1-  *QF4013  DS! J9 C9 D9 I9 Y9 B9 H9 K9 M9 L9  PVGPEK 1225   1445   321 0 L  E  \r\n>   MU564       V9 S9 NL QL OL                                      T1 T2  2:20\t\t\r\n"}
            //request.SCity = "SHA";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-10-10"); 
            //string cmdResult =
            string cmdResult4_3_2 =
@"
 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  *QF4013  DS! J9 C9 D9 I9 Y9 B9 H9 K9 M9 L9  PVGPEK 1225   1445   321 0 L  E  
>   MU564       V9 S9 NL QL OL                                      T1 T2  2:20		
";

            // 将再测一遍
            // 已测
            //request.SCity = "SHE";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-08-25");
            //request.Airline = "C"; 
            // 有权限查询AVH指令，且有舱位数：
            // 请求url：http://114.80.69.243:18082/
            // 指令：
            //system("AVH/SHEPEK/25AUG/C"); 
            // 返回结果：
            // 请给AVH指令传具有正确格式的请求参数值，如：出发城市三字码SCity传入格式如SHE、到达城市三字码ECity传入格式如PEK、航司Airline传入格式如CA
            // 请给AVH指令传具有正确格式的请求参数值，如：出发城市三字码SCity传入格式如SHE、到达城市三字码ECity传入格式如PEK、航司Airline传入格式如CA
            //string cmdResult =
            string cmdResult5 =
@"
 25AUG(TUE) SHEBJS C                                                           
1-  CZ6101  DS# J8 C1 D1 IQ OQ WS SQ YA B3 MA  SHEPEK 0800   0940   320 0^C  E  
>               H1 K2 U1 LQ Q1 EQ VQ ZQ TQ N1 RQ GQ XC              -- T2  1:40
2   CZ6109  DS# JA C4 D3 IQ OQ WS SQ YA BQ MQ  SHEPEK 1815   1955   321 0^C  E  
>               HQ KA UQ LQ QA EQ VQ ZQ TQ NA RS GQ XC              -- T2  1:40
3   CZ6115  DS# JA C6 D4 IQ OQ WS SQ YA BA MA  SHEPEK 2030   2210   321 0^C  E  
>               HA KA UA LQ QA EQ VQ ZQ TQ NA RQ GQ XC              -- T2  1:40
4  *HA3827  DS! J7 P7 C7 A7 Y7 W7 X7 Q7 V7 B7  SHEPEK 2105   2240   738 0    E  
>   CA1626      S7 N7 M7 I7 H7 G7 K7 L7 Z7 O7                       -- T3  1:35
5   MU5602  DS# UL F6 P3 JC CQ DQ IQ WQ YA BA  SHEPVG 0735   0950   319 0^S  E  
>               MA EA HA KA LA NA RA SA VA TQ GQ ZS QA              T3 T1  2:15
    MU564   DS# UL FL PL JA C7 D5 IQ WL YA BA     PEK 1225   1510   321 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T1 T2  7:35
6+  MU5602  DS# UL F6 P3 JC CQ DQ IQ WQ YA BA  SHEPVG 0735   0950   319 0^S  E  
>               MA EA HA KA LA NA RA SA VA TQ GQ ZS QA              T3 T1  2:15
    MU272   DS# UC FC PC JA CA DA IA WL YA BA     PEK 1340   1620   333 0^   E  
>               MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA              T1 T2  8:45
『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   
『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』
";


            // 不测：
            //request.SCity = "SHE";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-08-25");
            //request.Airline = "CA"; 
            // 有权限查询AVH指令，且有舱位数：
            // 请求url：http://114.80.69.243:18082/
            // 指令：
            //system("AVH/SHEPEK/25AU/CA");
            string cmdResult6 =
@"
 03JUL(FRI) SHEBJS VIA CA U                                                    
1-  CA1636  DS# FA AS YA BQ MQ HQ KQ LQ QQ GQ  SHEPEK 1910   2040   73K 0^   E  
>               SQ NQ VQ UQ TQ EQ                                   -- T3  1:30
               ** M1Q H1Q K1Q L1Q Q1Q V1Q                                      
2+  CA1626  DS# FA AS YA BS MQ HQ KQ LQ QQ GQ  SHEPEK 2105   2240   32A 0^   E  
>               SA NA VQ UQ TQ EQ                                   -- T3  1:35
               ** M1Q H1Q K1Q L1Q Q1Q V1Q                                      
『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   
『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』   
";

            // 将再测一遍
            // 已测             
            // 执行错误的指令：
            // 请求url：http://114.80.69.243:18082/
            //system("AVH/SHPEK/25AUG/CA");
            // 返回结果：
            // 请给AVH指令传具有正确格式的请求参数值，如：出发城市三字码SCity传入格式如SHE、到达城市三字码ECity传入格式如PEK、航司Airline传入格式如CA
            // 请给AVH指令传具有正确格式的请求参数值，如：出发城市三字码SCity传入格式如SHE、到达城市三字码ECity传入格式如PEK、航司Airline传入格式如CA
            //request.SCity = "SH";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-08-25");
            //request.Airline = "CA";
            //string cmdResult =
            string cmdResult7 =
@"
*FORMAT**
";                     
            // 将测
            // 已测
            // 指令：system("AVH/PEKPVG/09JUN/D");
            // 返回结果：很抱歉，不能查询历史起飞日期的舱位剩余可订数
            //request.SCity = "PEK";
            //request.ECity = "PVG";
            //request.DepDate = Convert.ToDateTime("2015-06-09");            
            //string cmdResult =
            string cmdResult8 =
@"";

            // 将测
            // 已测
            // 指令：system("AVH/PEKPVG/09JUN/D");
            // 返回结果：很抱歉，只能查询到起飞日期在1年内的舱位剩余可订数
            //request.SCity = "PEK";
            //request.ECity = "PVG";
            //request.DepDate = Convert.ToDateTime("2016-10-09");            
            //string cmdResult =
            string cmdResult9 =
@"";


            JetermEntity.Parser.AVH avh = new JetermEntity.Parser.AVH(string.Empty, string.Empty);
            avh.ParseCmd(request);
            CommandResult<JetermEntity.Response.AVH> result = avh.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}AVH指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
        
        //  不测这个方法，而是测AVHTest_EtermProxy1这个方法
        [TestMethod]
        public void AVHTest_BusinessDispose1()
        {
            // AVH请求对象
            Command<JetermEntity.Request.AVH> cmd = new Command<JetermEntity.Request.AVH>();
            //JetermEntity.Request.Booking cmd = new JetermEntity.Request.Booking();

            // 设置应用程序编号
            cmd.AppId = 100201;

            // 根据各自的业务需求，设置缓存返回结果时长
            //cmd.CacheTime = EtermCommand.CacheTime.min30;
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "KHN117";

            cmd.request = new JetermEntity.Request.AVH();

            #region AVH请求参数

            // 测试案例1：即传航司
            // 将再测一遍
            // 指令：
            //system("AVH/SHEPEK/25AUG/CA/D");           
            /*
命令返回结果：

             */
            // 返回结果：
            // {"state":true,"error":null,"config":"O77124B1","OfficeNo":"SHA243","result":{"AVHList":[{"FlightNo":"CA1602","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CA1652","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CA1658","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CA1636","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CA1626","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"4","AVNumberString":"4"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H1","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K1","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L1","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]}],"ResultBag":" 25AUG(TUE) SHEBJS VIA CA                                                      \r1-  CA1602  DS# FA AS YA BS MS HS KS LS QS GS  SHEPEK 0930   1110   330 0^N  E  \r>               SS NS VS US TS ES                                   -- T3  1:40\r               ** M1S H1S K1S L1S Q1S V1S                                      \r2   CA1652  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1100   1245   738 0^   E  \r>               SA NS VS US TS ES                                   -- T3  1:45\r               ** M1S H1S K1S L1S Q1S V1S                                      \r3   CA1658  DS# FA A2 YA BS MS HS KS LS QS GS  SHEPEK 1545   1720   73K 0^   E  \r>               SA NS VS US TS ES                                   -- T3  1:35\r               ** M1S H1S K1S L1S Q1S V1S                                      \r4   CA1636  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1910   2040   738 0^   E  \r>               SA NS VS US TS ES                                   -- T3  1:30\r               ** M1S H1S K1S L1S Q1S V1S                                      \r5+  CA1626  DS# FA A4 YA BA MA HA KA LA QS GS  SHEPEK 2105   2240   73L 0^   E  \r>               S8 NS VS US TS ES                                   -- T3  1:35\r               ** M1A H1A K1A L1A Q1S V1S                                      \r『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   \r『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』         \r"},"reqtime":"\/Date(1436247509982+0800)\/","SaveTime":1800}
            //cmd.request.SCity = "SHE";
            //cmd.request.ECity = "PEK";
            //cmd.request.DepDate = Convert.ToDateTime("2015-08-25");
            //cmd.request.Airline = "CA";

            // 将测
            // 测试案例1_1：即传航司
            // 指令：
            // system("AVH/PEKPVG/09JUN/MU/D");
            // 返回结果：
            //cmd.request.SCity = "PEK";
            //cmd.request.ECity = "PVG";
            //cmd.request.DepDate = Convert.ToDateTime("2015-06-09");
            //cmd.request.DepDate = Convert.ToDateTime("2016-06-09");
            //cmd.request.Airline = "MU";

            // 将测
            // 测试案例1_2：即传航司
            // 重点测：
            // 指令：
            //system("AVH/PEKSHA/09JUN/MU/D");
            //system("PN");
            //system("PN");
            // 返回结果：
            //cmd.request.SCity = "PEK";
            //cmd.request.ECity = "SHA";
            //cmd.request.DepDate = Convert.ToDateTime("2015-06-09");
            //cmd.request.DepDate = Convert.ToDateTime("2016-06-09");
            //cmd.request.Airline = "MU";

            // 将测
            // 测试案例1_3：即传航司
            // 指令：
            //system("AVH/SHAPEK/10OCT/CA/D");
            //system("PN");
            //system("PN");
            // 返回结果：
            //cmd.request.SCity = "SHA";
            //cmd.request.ECity = "PEK";
            //cmd.request.DepDate = Convert.ToDateTime("2015-10-10");         
            //cmd.request.Airline = "CA";

            // 测试案例2：即不传航司
            // 将再测一遍
            /*
命令返回结果：
 25AUG(TUE) SHEBJS                                                             
1-  CZ6101  DS# J8 C4 D3 IQ OQ WS SQ YA BA MA  SHEPEK 0800   0940   320 0^C  E  
>               HA KA UA LQ Q2 EQ VQ ZQ TQ N1 RQ GQ XC              -- T2  1:40
2  *MF1849  DS# YA BA MA LA KA QQ VQ TQ S1     SHEPEK 0800   0940   320 0^C  E  
>   CZ6101                                                          T3 T2  1:40
3   CA1602  DS# FA AS YA BS MS HS KS LS QS GS  SHEPEK 0930   1110   330 0^N  E  
>               SS NS VS US TS ES                                   -- T3  1:40
               ** M1S H1S K1S L1S Q1S V1S                                      
4  *ZH1602  DS# FA YA BS MS HS KS LS QS GS SS  SHEPEK 0930   1110   330 0^S  E  
>   CA1602      VS US TS ES                                         -- T3  1:40
5   CZ6103 『AS#』FA PA WS SQ YA BA MA HA KA UA  SHEPEK 1010   1150   330 0^C  E  
>               LQ QA EQ VQ ZQ TQ NA RQ GQ XC                       -- T2  1:40
6  *MF1851 『AS#』YA BA MA LA KA QQ VQ TQ SA     SHEPEK 1010   1150   330 0^C  E  
>   CZ6103                                                          T3 T2  1:40
7+ *ZH1652  DS# F8 YA BS MS HS KS LS QS GS SA  SHEPEK 1100   1245   738 0^S  E  
>   CA1652      VS US TS ES                                         -- T3  1:45
『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   
『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』   
             */
            // 返回结果：
            // {"state":true,"error":null,"config":"O77124B1","OfficeNo":"SHA243","result":{"AVHList":[{"FlightNo":"CZ6101","CarbinNumberList":[{"Cabin":"J","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"C","AVNumberTag":"4","AVNumberString":"4"},{"Cabin":"D","AVNumberTag":"3","AVNumberString":"3"},{"Cabin":"I","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"O","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"W","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"U","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"E","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"1","AVNumberString":"1"},{"Cabin":"R","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"X","AVNumberTag":"C","AVNumberString":"0"}]},{"FlightNo":"MF1849","CarbinNumberList":[{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"1","AVNumberString":"1"}]},{"FlightNo":"CZ6101","CarbinNumberList":[]},{"FlightNo":"CA1602","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"ZH1602","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CA1602","CarbinNumberList":[{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"}]},{"FlightNo":"CZ6103","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"P","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"W","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"U","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"E","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"Z","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"R","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"X","AVNumberTag":"C","AVNumberString":"0"}]},{"FlightNo":"MF1851","CarbinNumberList":[{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"L","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"Q","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"Q","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"}]},{"FlightNo":"CZ6103","CarbinNumberList":[]},{"FlightNo":"ZH1652","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"}]},{"FlightNo":"CA1652","CarbinNumberList":[{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"}]}],"ResultBag":" 25AUG(TUE) SHEBJS                                                             \r1-  CZ6101  DS# J8 C4 D3 IQ OQ WS SQ YA BA MA  SHEPEK 0800   0940   320 0^C  E  \r>               HA KA UA LQ Q2 EQ VQ ZQ TQ N1 RQ GQ XC              -- T2  1:40\r2  *MF1849  DS# YA BA MA LA KA QQ VQ TQ S1     SHEPEK 0800   0940   320 0^C  E  \r>   CZ6101                                                          T3 T2  1:40\r3   CA1602  DS# FA AS YA BS MS HS KS LS QS GS  SHEPEK 0930   1110   330 0^N  E  \r>               SS NS VS US TS ES                                   -- T3  1:40\r               ** M1S H1S K1S L1S Q1S V1S                                      \r4  *ZH1602  DS# FA YA BS MS HS KS LS QS GS SS  SHEPEK 0930   1110   330 0^S  E  \r>   CA1602      VS US TS ES                                         -- T3  1:40\r5   CZ6103 『AS#』FA PA WS SQ YA BA MA HA KA UA  SHEPEK 1010   1150   330 0^C  E  \r>               LQ QA EQ VQ ZQ TQ NA RQ GQ XC                       -- T2  1:40\r6  *MF1851 『AS#』YA BA MA LA KA QQ VQ TQ SA     SHEPEK 1010   1150   330 0^C  E  \r>   CZ6103                                                          T3 T2  1:40\r7+ *ZH1652  DS# F8 YA BS MS HS KS LS QS GS SA  SHEPEK 1100   1245   738 0^S  E  \r>   CA1652      VS US TS ES                                         -- T3  1:45\r『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   \r『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』         \r"},"reqtime":"\/Date(1436250428728+0800)\/","SaveTime":1800}
            cmd.request.SCity = "SHE";
            cmd.request.ECity = "PEK";
            cmd.request.DepDate = Convert.ToDateTime("2015-08-25");

            // 将测
            // 测试案例2_1：即不传航司
            // 指令：
            // system("AVH/PEKPVG/09JUN/D");
            // 返回结果：
            //cmd.request.SCity = "PEK";
            //cmd.request.ECity = "PVG";
            //cmd.request.DepDate = Convert.ToDateTime("2015-06-09");   
            //cmd.request.DepDate = Convert.ToDateTime("2016-06-09");     

            // 将测
            // 测试案例2_2：即不传航司
            // 指令：
            //system("AVH/PEKSHA/09JUN/D");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            // 返回结果：
            //cmd.request.SCity = "PEK";
            //cmd.request.ECity = "SHA";
            //cmd.request.DepDate = Convert.ToDateTime("2015-06-09");   
            //cmd.request.DepDate = Convert.ToDateTime("2016-06-09"); 

            // 将测
            // 测试案例2_3：即不传航司
            // 重点测：
            // 指令：
            //system("AVH/SHAPEK/10OCT/D");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            // 返回结果：
            //cmd.request.SCity = "SHA";
            //cmd.request.ECity = "PEK";
            //cmd.request.DepDate = Convert.ToDateTime("2015-10-10");          

            #endregion

            #region 调用Invoke以处理业务

            //EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.AVH> result = client.Invoke<JetermEntity.Request.AVH, JetermEntity.Response.AVH>(cmd);

            EtermProxy.BLL.AVH logic = new EtermProxy.BLL.AVH(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            logic.OfficeNo = "SHA243";
            logic.config = "O77124B1";
            CommandResult<JetermEntity.Response.AVH> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.AVH>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}AVH指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult + Environment.NewLine + "返回的OfficeNo：" + result.OfficeNo + "；返回的配置名：" + result.config);

            #endregion
        }

        // 2015-07-22（星期三），加一个功能：当传FlightNo和Carbin时，是否能返回正确结果：
        [TestMethod]
        public void AVHTest_BusinessDispose2()
        {
            // AVH请求对象
            Command<JetermEntity.Request.AVH> cmd = new Command<JetermEntity.Request.AVH>();
            //JetermEntity.Request.Booking cmd = new JetermEntity.Request.Booking();

            // 设置应用程序编号
            cmd.AppId = 100201;

            // 根据各自的业务需求，设置缓存返回结果时长
            //cmd.CacheTime = EtermCommand.CacheTime.min30;
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "KHN117";

            cmd.request = new JetermEntity.Request.AVH();

            // 返回结果：
            // {"state":true,"error":null,"config":"O77124B1","OfficeNo":"TPE567","result":{"DepDate":"\/Date(1437667200000+0800)\/","AVHList":[{"Number":5,"ShareFlight":false,"FlightNo":"MU5112","ShareFltNoList":[],"FlightModel":"333","SCity":"PEK","ECity":"SHA","STime":"1300","ETime":"1510","DepTerminal":"T2","ArrTerminal":"T2","FltDuration":"2:10","CarbinNumList":[{"Cabin":"Y","NumTag":"A","NumStr":"A"}]}],"ResultBag":" 24JUL(FRI) BJSSHA VIA MU DIRECT ONLY                                          \r1- *MU3658  DS# YA MA HA KA RA S8              PEKSHA 0635   0840   321 0^C  E  \r>   CZ6412                                                          T2 T2  2:05\r2  *MU3926  DS# YA MA EA HA                    PEKSHA 0650   0905   321 0^B  E  \r>   HO1252                                                          T2 T2  2:15\r3   MU5138  DS# UL FA PA J6 C4 DQ IQ WQ YA BA  PEKSHA 0700   0910   333 0^S  E  \r>               MA EA HA KA LA NA RA SS VQ TQ GQ ZQ QA              T2 T2  2:10\r4  *MU9108  DS# UC F1 PS AS JC CC DC IQ OQ YA  PEKSHA 0730   0940   738 0^S  E  \r>   FM9108      BA MA EA HA KA LA NA R8 SQ VQ TQ GQ ZQ Q2 XS        T2 T2  2:10\r5   MU5183  DS# UC FA PA JC CC DQ IQ WQ YA BA  PEKPVG 0735   0950   33E 0^S  E  \r>               MA EA HA KA LA NA RA SS VQ TQ GQ ZQ QA              T2 T1  2:15\r6   MU5102  DS# UL FA P4 J4 CQ DQ IQ WQ YA BA  PEKSHA 0800   1010   333 0^S  E  \r>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QA              T2 T2  2:10\r7  *MU3491 『AS#』YA MA HA KA RA SQ              PEKSHA 0820   1045   33B 0^C  E  \r>   CZ3907                                                          T2 T2  2:25\r8   MU5104  DS# UL F8 P1 J4 CQ DQ IQ WQ YA BA  PEKSHA 0900   1110   333 0^   E  \r>               MA EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QS              T2 T2  2:10\r9+ *MU3809  DS# JQ CQ DQ IQ W7 OQ YA BA MA EA  NAYPVG 0930   1130   737 0^   E  \r>   KN5977      HA KA LA N4 RS SS VS GQ QQ XQ                       -- T1  2:00                                         \r『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      \r『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         \r\r\n 24JUL(FRI) BJSSHA VIA MU DIRECT ONLY                                          \r1-  MU5106  DS# UL FA PS J4 CQ DQ IQ WQ YA BA  PEKSHA 1000   1210   333 0^   E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QS              T2 T2  2:10\r2   MU5108  DS# UL FA PS J5 CQ DQ IQ WQ YA BA  PEKSHA 1100   1310   333 0^L  E  \r>               MS EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10\r3   MU5152  DS# UC FA PS J3 CQ DQ IQ WQ YA BA  PEKSHA 1130   1340   321 0^L  E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10\r4   MU5110  DS# UL FA PS JC CQ DQ IQ WQ YA BA  PEKSHA 1200   1420   333 0^L  E  \r>               MS EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:20\r5   MU5112  DS# UL FA PQ J4 CQ DQ IQ WQ YA BA  PEKSHA 1300   1510   333 0^L  E  \r>               MS EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10\r6   MU5154  DS# UL FA PS J4 CQ DQ IQ WQ Y3 BA  PEKSHA 1335   1545   763 0^   E  \r>               MS EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10\r7   MU271   DS# UL FL PL JA CQ DQ IQ WC YA BA  PEKPVG 1400   1610   333 0^   E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:10\r8   MU5114  DS# UC FA PQ JC CQ DQ IQ WQ YS BA  PEKSHA 1400   1620   333 0^   E  \r>               MS EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:20\r9+  MU5116  DS# UL FA PQ JC CQ DQ IQ WQ YS BA  PEKSHA 1500   1710   333 0^   E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10                                         \r『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      \r『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         \r\r\n 24JUL(FRI) BJSSHA VIA MU DIRECT ONLY                                          \r1-  MU5200  DS# UL FA PS J3 CS DS IS WQ YS BA  PEKPVG 1545   1800   333 0^L  E  \r>               MS ES HS KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:15\r2   MU5118  DS# UL FA PQ JC CC DC IC WQ YS BA  PEKSHA 1600   1810   333 0 L  E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10\r3   MU563   DS# UC FC PC JL CQ DQ IQ WL YA BA  PEKPVG 1620   1840   320 0^   E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:20\r4   MU5120  DS# UL FA PQ JC CQ DQ IQ WQ YS BA  PEKSHA 1700   1915   333 0^D  E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:15\r5   MU5122  DS# UL FA PQ JC CC DC IC WQ YS BA  PEKSHA 1800   2010   333 0^D  E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10\r6  *MU8571 『AS#』YA MA RQ SQ VQ                 PEKSHA 1820   2045   738 0^D  E  \r>   MF8178                                                          T2 T2  2:25\r7  *MU3723  DS# JQ CQ DQ IQ W8 OQ YA BA MA EA  NAYSHA 1830   2035   73E 0^   E  \r>   KN5955      HA KS LQ NQ RQ SQ VQ GQ QQ XQ                       -- T2  2:05\r8   MU5156  DS# UC FA PQ JL CQ DQ IQ WQ YS BA  PEKSHA 1830   2045   321 0^D  E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:15\r9+  MU5124  DS# UC FA PS JL CQ DQ IQ WQ YS BA  PEKSHA 1900   2110   333 0^D  E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10                                         \r『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      \r『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         \r\r\n 24JUL(FRI) BJSSHA VIA MU DIRECT ONLY                                          \r1-  MU5130  DS# UL F5 PQ JC CQ DQ IQ WQ YS BA  PEKPVG 1900   2120   320 0^D  E  \r>               MS EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:20\r2   MU5126  DS# UL FA PQ JC CQ DQ IQ WQ YS BA  PEKSHA 2000   2210   333 0^   E  \r>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10\r3  *MU9106  DS# UC F5 PQ AQ JL CQ DQ IQ OQ YS  PEKSHA 2030   2235   73E 0^   E  \r>   FM9106      BA MS ES HS KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ XQ        T2 T2  2:05\r4   MU5128  DS# UL FA P2 J4 CQ DQ IQ WQ YS BA  PEKSHA 2100   2315   333 0^   E  \r>               MS ES HS KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:15\r5  *MU3811  DS# JQ CQ DQ IQ W8 OQ YA BA MA E5  NAYPVG 2130   2330   737 0^   E  \r>   KN5987      HS KS LS NQ RQ SQ VQ GQ QQ XQ                       -- T1  2:00\r6   MU5158  DS# UL FA PS J4 CQ DQ IQ WQ YA BA  PEKSHA 2130   2340   763 0^   E  \r>               MS ES HS KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10\r7+  MU5160  DS# UQ FA PS J5 CS DS IS WQ YA BA  PEKSHA 2200   2355   333 0^   E  \r>               M3 ES HS KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  1:55\r『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      \r『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         \r"},"reqtime":"\/Date(1437554693226+0800)\/","SaveTime":1800}
            cmd.request.Airline = "MU";
            cmd.request.SCity = "PEK";
            cmd.request.ECity = "SHA";
            cmd.request.DepDate = Convert.ToDateTime("2015-07-24");
            cmd.request.FlightNo = "MU5112";
            cmd.request.Carbin = "Y";

            #region 调用Invoke以处理业务

            //EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.AVH> result = client.Invoke<JetermEntity.Request.AVH, JetermEntity.Response.AVH>(cmd);

            EtermProxy.BLL.AVH logic = new EtermProxy.BLL.AVH(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            logic.OfficeNo = "TPE567";
            logic.config = "O77124B1";
            CommandResult<JetermEntity.Response.AVH> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.AVH>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}AVH指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult + Environment.NewLine + "返回的OfficeNo：" + result.OfficeNo + "；返回的配置名：" + result.config);

            #endregion
        }

        // 2015-08-05（星期三），帮助政策组寻找返回为null的原因
        [TestMethod]
        public void AVHTest_BusinessDispose3()
        {
            // AVH请求对象
            Command<JetermEntity.Request.AVH> cmd = new Command<JetermEntity.Request.AVH>();
            //JetermEntity.Request.Booking cmd = new JetermEntity.Request.Booking();

            // 设置应用程序编号
            //cmd.AppId = 100201;

            // 根据各自的业务需求，设置缓存返回结果时长
            //cmd.CacheTime = EtermCommand.CacheTime.min30;
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "KHN117";

            cmd.request = new JetermEntity.Request.AVH();

            cmd.AppId = 100203001;
            //AppId:100203001航司:HU起飞日期:2015/9/30 0:00:00出发城市PEK到达城市:PVG用户名:tester001舱位:X航班号:HU7609  
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"DepDate":"\/Date(1443542400000+0800)\/","AVHList":[{"Number":1,"ShareFlight":false,"FlightNo":"HU7609","ShareFltNoList":[],"FlightModel":"331","SCity":"PEK","ECity":"PVG","STime":"0705","ETime":"0915","DepTerminal":"T1","ArrTerminal":"T2","FltDuration":"2:10","CarbinNumList":[{"Cabin":"X","NumTag":"2","NumStr":"2"}]}],"ResultBag":" 30SEP(WED) BJSPVG VIA HU DIRECT ONLY                                          \r1+  HU7609  DS# FA ZA P4 A8 YA BA HA KA LA MA  PEKPVG 0705   0915   331 0^B  E  \r>               QA X2 UQ EQ TQ V5 WQ GQ OQ SA                       T1 T2  2:10\r               ** M1A Q1A                                                      \r『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      \r『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         \r"},"reqtime":"\/Date(1438758727002+0800)\/","SaveTime":1800}
            cmd.request.Airline = "HU";
            cmd.request.DepDate = Convert.ToDateTime("2015-09-30");
            cmd.request.SCity = "PEK";
            cmd.request.ECity = "PVG";
            cmd.request.Carbin = "X";
            cmd.request.FlightNo = "HU7609";

            // AppId:100203001航司:3U起飞日期:2015/9/16 0:00:00出发城市PEK到达城市:CKG用户名:tester001舱位:Y航班号:3U8516
            // 返回结果：
            // 
            //cmd.request.Airline = "3U";
            //cmd.request.DepDate = Convert.ToDateTime("2015-09-16");
            //cmd.request.SCity = "PEK";
            //cmd.request.ECity = "CKG";
            //cmd.request.Carbin = "Y";
            //cmd.request.FlightNo = "3U8516";

            #region 调用Invoke以处理业务

            //EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.AVH> result = client.Invoke<JetermEntity.Request.AVH, JetermEntity.Response.AVH>(cmd);

            EtermProxy.BLL.AVH logic = new EtermProxy.BLL.AVH(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            //logic.OfficeNo = "SHA243";
            //logic.config = "av09";
            CommandResult<JetermEntity.Response.AVH> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.AVH>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}AVH指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult + Environment.NewLine + "返回的OfficeNo：" + result.OfficeNo + "；返回的配置名：" + result.config);

            #endregion
        }

        // 将测（“重点测”都已测，剩下的有部分没测，只测了部分）
        [TestMethod]
        public void AVHTest_EtermProxy1()
        {
            string strPost = "{\"ClassName\" : \"AVH\", \"Config\" : \"O77124B1\",  \"OfficeNo\" : \"SHA243\" }";
            //string ss = "{\"FlightList\":[{\"FlightNo\":\"MU5137\",\"Cabin\":\"H\",\"SCity\":\"SHA\",\"ECity\":\"PEK\",\"DepDate\":\"\\/Date(1430064000000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"MU5156\",\"Cabin\":\"B\",\"SCity\":\"PEK\",\"ECity\":\"SHA\",\"DepDate\":\"\\/Date(1430323200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"干园\",\"idtype\":0,\"cardno\":\"650121199412242866\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"},{\"name\":\"张杰\",\"idtype\":0,\"cardno\":\"140525198401186312\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"13472634765\",\"RMKOfficeNoList\":[],\"RMKRemark\":null,\"Pnr\":null}";

            // 设置请求参数：
            JetermEntity.Request.AVH request = new JetermEntity.Request.AVH();
            // 测试案例1：即传航司
            // 已测
            // 指令：
            //system("AVH/SHEPEK/25AUG/CA/D");
            /*
指令返回结果：
 25AUG(TUE) SHEBJS VIA CA DIRECT ONLY                                          
1-  CA1602  DS# FA AS YA BS MS HS KS LS QS GS  SHEPEK 0930   1110   330 0^N  E  
>               SS NS VS US TS ES                                   -- T3  1:40
               ** M1S H1S K1S L1S Q1S V1S                                      
2   CA1652  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1100   1245   738 0^   E  
>               SA NS VS US TS ES                                   -- T3  1:45
               ** M1S H1S K1S L1S Q1S V1S                                      
3   CA1658  DS# FA A2 YA BS MS HS KS LS QS GS  SHEPEK 1545   1720   73K 0^   E  
>               SA NS VS US TS ES                                   -- T3  1:35
               ** M1S H1S K1S L1S Q1S V1S                                      
4   CA1636  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1910   2040   738 0^   E  
>               SA NS VS US TS ES                                   -- T3  1:30
               ** M1S H1S K1S L1S Q1S V1S                                      
5+  CA1626  DS# FA A4 YA BA MA HA K4 LS QS GS  SHEPEK 2105   2240   73L 0^   E  
>               S8 NS VS US TS ES                                   -- T3  1:35
               ** M1A H1A K14 L1S Q1S V1S                                      
『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   
『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』  
             */
            // 返回结果：
            // {"state":true,"error":null,"config":"O77124B1","OfficeNo":"SHA243","result":{"DepDate":"\/Date(1440432000000+0800)\/","AVHList":[{"ShareFlight":false,"FlightNo":"CA1602","ShareFlightNoList":[],"FlightModel":"330","SCity":"SHE","ECity":"PEK","STime":"0930","ETime":"1110","DepTerminal":"--","ArrTerminal":"T3","FlightDuration":"1:40","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1652","ShareFlightNoList":[],"FlightModel":"738","SCity":"SHE","ECity":"PEK","STime":"1100","ETime":"1245","DepTerminal":"--","ArrTerminal":"T3","FlightDuration":"1:45","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1658","ShareFlightNoList":[],"FlightModel":"73K","SCity":"SHE","ECity":"PEK","STime":"1545","ETime":"1720","DepTerminal":"--","ArrTerminal":"T3","FlightDuration":"1:35","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1636","ShareFlightNoList":[],"FlightModel":"738","SCity":"SHE","ECity":"PEK","STime":"1910","ETime":"2040","DepTerminal":"--","ArrTerminal":"T3","FlightDuration":"1:30","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"A","AVNumberTag":"2","AVNumberString":"2"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"H1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"K1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]},{"ShareFlight":false,"FlightNo":"CA1626","ShareFlightNoList":[],"FlightModel":"73L","SCity":"SHE","ECity":"PEK","STime":"2105","ETime":"2240","DepTerminal":"--","ArrTerminal":"T3","FlightDuration":"1:35","CarbinNumberList":[{"Cabin":"F","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"A","AVNumberTag":"4","AVNumberString":"4"},{"Cabin":"Y","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"B","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"M","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K","AVNumberTag":"3","AVNumberString":"3"},{"Cabin":"L","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"G","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"S","AVNumberTag":"8","AVNumberString":"8"},{"Cabin":"N","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"U","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"T","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"E","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"M1","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"H1","AVNumberTag":"A","AVNumberString":">9"},{"Cabin":"K1","AVNumberTag":"3","AVNumberString":"3"},{"Cabin":"L1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"Q1","AVNumberTag":"S","AVNumberString":"0"},{"Cabin":"V1","AVNumberTag":"S","AVNumberString":"0"}]}],"ResultBag":" 25AUG(TUE) SHEBJS VIA CA DIRECT ONLY                                          \r1-  CA1602  DS# FA AS YA BS MS HS KS LS QS GS  SHEPEK 0930   1110   330 0^N  E  \r>               SS NS VS US TS ES                                   -- T3  1:40\r               ** M1S H1S K1S L1S Q1S V1S                                      \r2   CA1652  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1100   1245   738 0^   E  \r>               SA NS VS US TS ES                                   -- T3  1:45\r               ** M1S H1S K1S L1S Q1S V1S                                      \r3   CA1658  DS# FA A2 YA BS MS HS KS LS QS GS  SHEPEK 1545   1720   73K 0^   E  \r>               SA NS VS US TS ES                                   -- T3  1:35\r               ** M1S H1S K1S L1S Q1S V1S                                      \r4   CA1636  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1910   2040   738 0^   E  \r>               SA NS VS US TS ES                                   -- T3  1:30\r               ** M1S H1S K1S L1S Q1S V1S                                      \r5+  CA1626  DS# FA A4 YA BA MA HA K3 LS QS GS  SHEPEK 2105   2240   73L 0^   E  \r>               S8 NS VS US TS ES                                   -- T3  1:35\r               ** M1A H1A K13 L1S Q1S V1S                                      \r『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   \r『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』         \r"},"reqtime":"\/Date(1436430710135+0800)\/","SaveTime":1800}
            //request.SCity = "SHE";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-08-25");
            //request.Airline = "CA";

            // 测试案例1_1：即传航司
            // 还未测
            // 指令：
            // system("AVH/PEKPVG/09JUN/MU/D");
            // 返回结果：
            //request.SCity = "PEK";
            //request.ECity = "PVG";
            //request.DepDate = Convert.ToDateTime("2015-06-09");
            //request.DepDate = Convert.ToDateTime("2016-06-09");
            //request.Airline = "MU";

            // 测试案例1_2：即传航司
            // 已测
            // 重点测：
            // 指令：
            //system("AVH/PEKSHA/09JUN/MU/D");
            //system("PN");
            //system("PN");
            /*
指令返回结果：
 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        
1-  MU5138  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0700   0910   333 0^S  E  
>               MA EA HA KA LA NS RA SQ VQ TQ GQ ZS QS              T2 T2  2:10
2   MU5183  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKPVG 0735   0950   321 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T1  2:15
3   MU5102  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0800   0955   333 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  1:55
4   MU5104  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 0900   1110   333 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
5   MU5106  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1000   1210   333 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
6   MU5108  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1100   1310   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
7   MU5152  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1130   1340   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
8   MU5110  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1200   1410   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QQ              T2 T2  2:10
9+  MU5112  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1300   1510   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10                                       
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         

 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        
1-  MU5154  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 1335   1545   763 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
2   MU271   DS# UC FC PC JA CQ DQ IQ WQ YA B1  PEKPVG 1355   1555   321 0^   E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:00
3   MU5114  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1400   1620   333 0^   E  
>               MS ES HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:20
4   MU5116  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1500   1710   333 0^   E  
>               MS ES HS KS LS NS RS SQ VQ TQ GQ ZS QS              T2 T2  2:10
5   MU563   DS# UC FC PC JA CQ DQ IQ WQ YA BA  PEKPVG 1620   1840   321 0^   E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T1  2:20
6   MU5120  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1700   1915   333 0^D  E  
>               MS ES HS KS LS NS RS SQ VQ TQ GQ ZS QS              T2 T2  2:15
7   MU5122  DS# UC FA PQ JC CC DC IC WQ YA BA  PEKSHA 1800   2010   333 0^D  E  
>               MQ EQ HQ KQ LQ NQ RQ SQ VQ TQ GQ ZQ QQ              T2 T2  2:10
8   MU5156  DS# UC FA P8 JC CQ DQ IQ WQ YA BA  PEKSHA 1830   2045   333 0^   E  
>               MA EA HA KQ LQ NQ RQ SQ VQ TQ GQ ZA QA              T2 T2  2:15
9+  MU5124  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 1900   2110   333 0^D  E  
>               MS E3 HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:10                                       
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』         

 09JUN16(THU) BJSSHA VIA MU DIRECT ONLY                                        
1-  MU5130  DS# UC F8 PQ JC CQ DQ IQ WQ YA BA  PEKPVG 1900   2120   320 0^D  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QQ              T2 T1  2:20
2   MU5126  DS# UC FA PQ JC CQ DQ IQ WQ YA BA  PEKSHA 2000   2210   333 0^   E  
>               MS E3 HS KS LS NS RS SQ VQ TQ GQ ZQ QQ              T2 T2  2:10
3   MU5128  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 2100   2315   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:15
4   MU5158  DS# UC FA PA JC CQ DQ IQ WQ YA BA  PEKSHA 2130   2340   763 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:10
5+  MU5160  DS# UC FA PA JC CC DC IC W4 YA BA  PEKSHA 2200   2355   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  1:55
『**  CZ3000/3100/6000 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT PEK』      
『**  HU7000-HU7899 PLEASE CHECK IN 30 MINUTES BEFORE DEPARTURE AT PEK』  

             */
            // 返回结果：
            // 
            //request.SCity = "PEK";
            //request.ECity = "SHA";
            ////request.DepDate = Convert.ToDateTime("2015-06-09");
            //request.DepDate = Convert.ToDateTime("2016-06-09");
            //request.Airline = "MU";

            // 测试案例1_3：即传航司
            // 还未测
            // 指令：
            //system("AVH/SHAPEK/10OCT/CA/D");
            //system("PN");
            //system("PN");
            // 返回结果：
            //request.SCity = "SHA";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-10-10");          
            //request.Airline = "CA";          

            // 测试案例2：即不传航司
            // 还未测
            // 返回结果：
            //request.SCity = "SHE";
            //request.ECity = "PEK";
            //request.DepDate = Convert.ToDateTime("2015-08-25");  

            // 测试案例2_1：即不传航司
            // 还未测
            // 指令：
            // system("AVH/PEKPVG/09JUN/D");
            // 返回结果：
            //request.SCity = "PEK";
            //request.ECity = "PVG";
            //request.DepDate = Convert.ToDateTime("2015-06-09");
            //request.DepDate = Convert.ToDateTime("2016-06-09");

            // 测试案例2_2：即不传航司
            // 还未测
            // 指令：
            //system("AVH/PEKSHA/09JUN/D");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            // 返回结果：
            //request.SCity = "PEK";
            //request.ECity = "SHA";
            //request.DepDate = Convert.ToDateTime("2015-06-09");
            //request.DepDate = Convert.ToDateTime("2016-06-09");
            
            // 测试案例2_3：即不传航司
            // 已测
            // 重点测：
            // 指令：
            //system("AVH/SHAPEK/10OCT/D");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            //system("PN");
            /*
命令返回结果：
 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  KN5988  DS# JS CQ DQ IQ W8 OQ YA BA MQ EQ  PVGNAY 0710   0910   73E 0^   E  
>               HQ KQ LQ NQ RA GQ QQ XQ                             T1 --  2:00
2  *MU3812  DS# JS CQ DQ IQ W8 OQ YA BA MQ EQ  PVGNAY 0710   0910   73E 0^   E  
>   KN5988      HQ KQ LQ NQ RA SQ VQ GQ QQ XQ                       T1 --  2:00
3  *ZH1858  DS# FA YA B5 MS HS KS LS QS GS SA  SHAPEK 0755   1015   773 0^B  E  
>   CA1858      VS US TS ES                                         T2 T3  2:20
4   CA1858  DS# P8 FA A2 YA B5 MS HS KS LS QS  SHAPEK 0755   1015   773 0^B  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
5  *CZ9270  DS# YA BA MA UA LQ EQ              SHAPEK 0800   1020   333 0^S  E  
>   MU5101                                                          T2 T2  2:20
6   MU5101  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 0800   1020   333 0^S  E  
>               MA EA HA KA LA NA RQ SQ VQ TQ GQ ZA QA              T2 T2  2:20
7  *CZ9310  DS# YA BA MA UA LA EQ              SHAPEK 0830   1035   321 0^S  E  
>   MU5151                                                          T2 T2  2:05
8   HU7604  DS# F8 Z4 P2 AQ YA BA HA KA LA MA  SHAPEK 0830   1035   738 0^B  E  
>               QA XQ UQ EQ TQ V5 WQ GQ OQ S6                       T2 T1  2:05
9+  MU5151  DS# UC FA P8 JL CQ DQ IQ WQ YA BA  SHAPEK 0830   1035   321 0^S  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:05                     

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  CA1590  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 0855   1115   773 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
2  *ZH1590  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 0855   1115   773 0^B  E  
>   CA1590      VS US TS ES                                         T2 T3  2:20
3   MU5103  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 0900   1120   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:20
4  *CZ9272  DS# YA BA MA UA LA EQ              SHAPEK 0900   1120   333 0^   E  
>   MU5103                                                          T2 T2  2:20
5   MU5129  DS# UL F8 P6 JC CQ DQ IQ WQ YA BA  PVGPEK 0920   1155   320 0^L  E  
>               MA EA HA KA LA NA RS SQ VQ TQ GQ ZA QA              T1 T2  2:35
6  *CZ9298  DS# YA BA MA UA LS EQ              PVGPEK 0920   1155   320 0^L  E  
>   MU5129                                                          T1 T2  2:35
7  *QF4011  DS! J9 C9 D9 I9 Y9 B9 H9 K9 M9 L9  PVGPEK 0920   1155   320 0 N  E  
>   MU5129      V9 S9 N9 Q9 O9                                      T1 T2  2:35
8   MU5153  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 0930   1155   763 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25
9+ *CZ9312  DS# YA BA MA UA LA EQ              SHAPEK 0930   1155   763 0^   E  
>   MU5153                                                          T2 T2  2:25                     

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  MU5105  DS# UL FA PA JC CC DC IC WQ YA BA  SHAPEK 1000   1220   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:20
2  *CZ9274  DS# YA BA MA UA LA EQ              SHAPEK 1000   1220   333 0^   E  
>   MU5105                                                          T2 T2  2:20
3  *MU3810  DS# J7 CQ DQ IQ W7 OQ YA BA MQ EQ  PVGNAY 1010   1155   73E 0^   E  
>   KN5978      HQ KQ LQ NQ RA SQ VQ GQ QQ XQ                       T1 --  1:45
4   KN5978  DS# J7 CQ DQ IQ W7 OQ YA BA MQ EQ  PVGNAY 1010   1155   73E 0^   E  
>               HQ KQ LQ NQ RA GQ QQ XQ                             T1 --  1:45
5  *ZH1832  DS# FA YA BS MS HS KS LS QS GS SS  SHAPEK 1055   1315   744 0^S  E  
>   CA1832      VS US TS ES                                         T2 T3  2:20
6   CA1832  DS# PA FA AS YA BS MS HS KS LS QS  SHAPEK 1055   1315   744 0^M  E  
>               GS SS NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
7   MU5107  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1100   1320   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:20
8+ *CZ9276  DS# YA BA MA UA LA EQ              SHAPEK 1100   1320   333 0^L  E  
>   MU5107                                                          T2 T2  2:20

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  HU7606  DS# FA Z8 P4 A4 YA BA HA KA LA MA  SHAPEK 1130   1345   333 0^   E  
>               QA XQ UQ EQ TQ V5 WQ GQ OQ SA                       T2 T1  2:15
               ** M1A Q1A                                                      
2  *MU3492  DS# YA MQ HQ KQ RQ SQ              SHAPEK 1150   1415   330 0^L  E  
>   CZ3908                                                          T2 T2  2:25
3  *MF1764  DS# YA BQ MQ LQ KQ QQ VQ TQ S1     SHAPEK 1150   1415   330 0^L  E  
>   CZ3908                                                          T2 T2  2:25
4   CZ3908  DS# FA PQ WA SQ YA BQ MQ HQ KA UQ  SHAPEK 1150   1415   330 0^L  E  
>               LQ QA EQ VQ ZQ TQ N1 RS GQ XC                       T2 T2  2:25
5  *HO1902  DS# YA B3 MS TS ES VS              SHAPEK 1155   1415   747 0^L  E  
>   CA1502                                                          T2 T3  2:20
6   CA1502  DS# PA FA A2 WA YA B3 MS HS KS LS  SHAPEK 1155   1415   747 0^M  E  
>               QS GS SA NS VS US TS ES                             T2 T3  2:20
               ** M1S V1S                                                      
7  *ZH1502  DS# FA YA B3 MS HS KS LS QS GS SA  SHAPEK 1155   1415   747 0^L  E  
>   CA1502      VS US TS ES                                         T2 T3  2:20
8+ *CZ9278  DS# YA BA MA UA LA EQ              SHAPEK 1200   1425   333 0^L  E  
>   MU5109                                                          T2 T2  2:25

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  MU5109  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1200   1425   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25
2   HU7608  DS# FA ZA P4 A8 YA BA HA KA LA MA  SHAPEK 1220   1435   787 0^L  E  
>               QA XQ UQ EQ TQ V5 WQ GQ OQ SA                       T2 T1  2:15
               ** M1A Q1A                                                      
3  *QF4013  DS! J9 C9 D9 I9 Y9 B9 H9 K9 M9 L9  PVGPEK 1225   1445   321 0 L  E  
>   MU564       V9 S9 NL QL OL                                      T1 T2  2:20
4   MU564   DS# UL FL PL JA CQ DQ IQ WC YA BA  PVGPEK 1225   1445   321 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZQ QQ              T1 T2  2:20
5  *ZH1520  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1255   1515   773 0^L  E  
>   CA1520      VS US TS ES                                         T2 T3  2:20
6   CA1520  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1255   1515   773 0^M  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
7  *CZ9280  DS# YA BA MA UA LA EQ              SHAPEK 1300   1520   333 0^L  E  
>   MU5111                                                          T2 T2  2:20
8+  MU5111  DS# UC FA PA JL CL DL IL WQ YA BA  SHAPEK 1300   1520   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:20

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  FM9103  DS# UC F8 P8 JL CQ DQ IQ WQ YA BA  SHAPEK 1330   1555   73E 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25
2  *MU9103  DS# UC F8 P8 A3 JL CQ DQ IQ OQ YA  SHAPEK 1330   1555   73E 0^   E  
>   FM9103      BA MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA X5        T2 T2  2:25
3   MU272   DS# UC FC PC JA CQ DQ IQ WL YA BA  PVGPEK 1340   1620   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA Q1              T1 T2  2:40
4   CA1532  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1355   1615   773 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
5  *ZH1532  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1355   1615   773 0^S  E  
>   CA1532      VS US TS ES                                         T2 T3  2:20
6  *CZ9282  DS# YA BA MA UA LA EQ              SHAPEK 1400   1625   333 0^   E  
>   MU5113                                                          T2 T2  2:25
7   MU5113  DS# UL FA PA JC CC DC IC WQ YA BA  SHAPEK 1400   1625   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25
8   MU5155  DS# UC FA P8 JL CQ DQ IQ WQ YA BA  SHAPEK 1430   1650   321 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:20
9+ *CZ9314  DS# YA BA MA UA LA EQ              SHAPEK 1430   1650   321 0^   E  
>   MU5155                                                          T2 T2  2:20                     

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  CA1558  DS# PA FA AS YA BS MS HS KS LS QS  SHAPEK 1455   1720   744 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:25
               ** M1S V1S                                                      
2  *ZH1558  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1455   1720   744 0^S  E  
>   CA1558      VS US TS ES                                         T2 T3  2:25
3   MU5115  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1500   1715   333 0^   E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:15
4  *CZ9284  DS# YA BA MA UA LA EQ              SHAPEK 1500   1715   333 0^   E  
>   MU5115                                                          T2 T2  2:15
5  *NS8177 『AS#』YA BQ MQ LQ KQ NQ QQ VQ TQ     SHAPEK 1500   1715   738 0^S  E  
>   MF8177                                                          T2 T2  2:15
6   MF8177 『AS#』                               SHAPEK 1500   1715   738 0^S  E  
>                                                                   T2 T2  2:15
7  *CZ5137 『AS#』YA BQ MQ HQ UQ LQ EQ ZQ N5     SHAPEK 1500   1715   738 0^S  E  
>   MF8177                                                          T2 T2  2:15
8  *MU8570 『AS#』YA MQ RQ SQ VQ                 SHAPEK 1500   1715   738 0^S  E  
>   MF8177                                                          T2 T2  2:15
9+  MU5117  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1600   1825   333 0^L  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25                     

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1- *CZ9286  DS# YA BA MA UA LA EQ              SHAPEK 1600   1825   333 0^L  E  
>   MU5117                                                          T2 T2  2:25
2   CA1884  DS# FA A2 YA BS MS HS KS LS QS GS  PVGPEK 1615   1840   330 0^M  E  
>               SA NS VS US TS ES                                   T2 T3  2:25
               ** M1S V1S                                                      
3  *ZH1884  DS# FA YA BS MS HS KS LS QS GS SA  PVGPEK 1615   1840   330 0^S  E  
>   CA1884      VS US TS ES                                         T2 T3  2:25
4   CA1518  DS# FA A2 WA YA BS MS HS KS LS QS  SHAPEK 1655   1915   772 0^M  E  
>               GS SS NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
5  *HO1904  DS# YA BS MS TS ES VS              SHAPEK 1655   1915   772 0^S  E  
>   CA1518                                                          T2 T3  2:20
6  *ZH1518  DS# FA YA BS MS HS KS LS QS GS SS  SHAPEK 1655   1915   772 0^S  E  
>   CA1518      VS US TS ES                                         T2 T3  2:20
7   MU5119  DS# UC FA PA JL CQ DQ IQ WQ YA BA  SHAPEK 1700   1925   333 0^D  E  
>               MA EA HA KA LA NA RA SQ VQ TQ GQ ZA QA              T2 T2  2:25
8+ *CZ9288  DS# YA BA MA UA LA EQ              SHAPEK 1700   1925   333 0^D  E  
>   MU5119                                                          T2 T2  2:25

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1- *CZ9316  DS# YA BA MA UA LA EA              SHAPEK 1730   1950   763 0^D  E  
>   MU5157                                                          T2 T2  2:20
2   MU5157  DS# UL FA P8 JC CQ DQ IQ WQ YA BA  SHAPEK 1730   1950   763 0^D  E  
>               MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA              T2 T2  2:20
3   CA1522  DS# FA A2 WA YA BS MS HS KS LS QS  SHAPEK 1755   2015   772 0^M  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
4  *ZH1522  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1755   2015   772 0^D  E  
>   CA1522      VS US TS ES                                         T2 T3  2:20
5   MU5121  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1800   2020   333 0^D  E  
>               MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA              T2 T2  2:20
6  *CZ9290  DS# YA BA MA UA LA EA              SHAPEK 1800   2020   333 0^D  E  
>   MU5121                                                          T2 T2  2:20
7   KN5956  DS# JQ CQ DQ IQ W8 OQ YA BA MA EA  SHANAY 1830   2035   73E 0^   E  
>               HA KA LA NA RA GQ QQ XQ                             T2 --  2:05
8+ *MU3724  DS# JQ CQ DQ IQ W8 OQ YA BA MA EA  SHANAY 1830   2035   73E 0^   E  
>   KN5956      HA KA LA NA RA SA VA GQ QQ XQ                       T2 --  2:05

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1- *ZH1516  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1855   2115   747 0^D  E  
>   CA1516      VS US TS ES                                         T2 T3  2:20
2   CA1516  DS# PA FA A2 WA YA BS MS HS KS LS  SHAPEK 1855   2115   747 0^M  E  
>               QS GS SA NS VS US TS ES                             T2 T3  2:20
               ** M1S V1S                                                      
3  *CZ9292  DS# YA BA MA UA LA EA              SHAPEK 1900   2125   333 0^D  E  
>   MU5123                                                          T2 T2  2:25
4   MU5123  DS# UL FA PA JC CQ DQ IQ WQ YA BA  SHAPEK 1900   2125   333 0^D  E  
>               MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA              T2 T2  2:25
5  *ZH1550  DS# FA YA BS MS HS KS LS QS GS SA  SHAPEK 1955   2215   773 0^D  E  
>   CA1550      VS US TS ES                                         T2 T3  2:20
6  *HA3823  DS! J7 P7 C7 A7 Y0 W0 X0 Q0 V0 B0  SHAPEK 1955   2215   773 0    E  
>   CA1550      S0 N0 M0 I0 H0 G0 K0 L0 Z0 O0                       T2 T3  2:20
7   CA1550  DS# P8 FA A2 YA BS MS HS KS LS QS  SHAPEK 1955   2215   773 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
8+  HU7602  DS# FA Z8 P4 A4 YA BA HA KA LA MA  SHAPEK 2020   2235   767 0^   E  
>               QA XA UQ EQ TQ V5 WQ GQ OQ SA                       T2 T1  2:15
               ** M1A Q1A                                                      

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1-  CA1856  DS# FA A2 WA YA BA MS HS KS LS QS  SHAPEK 2055   2315   33A 0^R  E  
>               GS SA NS VS US TS ES                                T2 T3  2:20
               ** M1S V1S                                                      
2  *ZH1856  DS# FA YA BA MS HS KS LS QS GS SA  SHAPEK 2055   2315   33A 0^S  E  
>   CA1856      VS US TS ES                                         T2 T3  2:20
3   MU5186  DS# UC FA PA JL CQ DQ IQ WQ YA BA  PVGPEK 2110   2340   321 0^   E  
>               MA EA HA KQ LQ NQ RQ SQ VQ TQ GQ ZS QA              T1 T2  2:30
4  *QF4009  DS! J9 C9 D9 I9 Y9 B9 H9 K9 M9 L9  PVGPEK 2110   2340   321 0 N  E  
>   MU5186      VL SL N9 Q9 O9                                      T1 T2  2:30
5   FM9107  DS# UC F8 P8 JL CQ DQ IQ WQ YA BA  SHAPEK 2130   2345   738 0^   E  
>               MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA              T2 T2  2:15
6  *MU9107  DS# UC F8 P8 A3 JL CQ DQ IQ OQ YA  SHAPEK 2130   2345   738 0^   E  
>   FM9107      BA MA EA HA KA LA NA RA SA VQ TQ GQ ZA QA X5        T2 T2  2:15
7  *MU3925  DS# YA MS ES HS                    SHAPEK 2150   0020+1 321 0^S  E  
>   HO1251                                                          T2 T2  2:30
8  *CA3201  DS# YA BS HS KS LS QS              SHAPEK 2150   0020+1 321 0^S  E  
>   HO1251                                                          T2 T3  2:30
9+  HO1251  DS# FA AS JS YA BS LS MS TS ES HS  SHAPEK 2150   0020+1 321 0^S  E  
>               VS KS WS RS QS ZS PS XS GS SS IS NS                 T2 T3  2:30                     

 10OCT(SAT) SHABJS DIRECT ONLY                                                 
1- *ZH1886  DS# FA YA BA MS HS KS LS QS GS SA  SHAPEK 2155   0015+1 330 0^S  E  
>   CA1886      VS US TS ES                                         T2 T3  2:20
2   CA1886  DS# FA A2 YA BA MS HS KS LS QS GS  SHAPEK 2155   0015+1 330 0^   E  
>               SA NS VS US TS ES                                   T2 T3  2:20
               ** M1S V1S                                                      
3  *MF4737  DS# YA BQ MQ LQ KQ QQ VQ TQ S2     SHAPEK 2230   0035+1 321 0^C  E  
>   CZ6411                                                          T2 T2  2:05
4  *MU3657  DS# YA MQ HQ KQ RQ SQ              SHAPEK 2230   0035+1 321 0^C  E  
>   CZ6411                                                          T2 T2  2:05
5   CZ6411  DS# JA CQ DQ IQ OQ WA SQ YA BQ MQ  SHAPEK 2230   0035+1 321 0^C  E  
>               HQ KA UQ LQ QA EQ VQ ZQ TQ N2 RQ GQ XC              T2 T2  2:05
6+  HU7610  DS# FA ZA P4 A8 YA BA HA KA LA MA  PVGPEK 2230   0100+1 331 0^   E  
>               QA XA UA EA TQ V5 WQ GQ OQ SA                       T2 T1  2:30
               ** M1A Q1A                                                              
             */
            // 返回结果（共99条航线）：
            // 
            request.SCity = "SHA";
            request.ECity = "PEK";
            request.DepDate = Convert.ToDateTime("2015-10-10");

            string ss = JsonConvert.SerializeObject(request);
            //string ss = "{\"FlightList\":[{\"FlightNo\":\"CZ6178\",\"Airline\":\"\",\"Cabin\":\"Y\",\"SCity\":\"CGQ\",\"ECity\":\"CSX\",\"DepTerminal\":null,\"ArrTerminal\":null,\"DepDate\":\"\\/Date(1435075200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"CZ3937\",\"Airline\":\"\",\"Cabin\":\"M\",\"SCity\":\"CSX\",\"ECity\":\"CGQ\",\"DepTerminal\":null,\"ArrTerminal\":null,\"DepDate\":\"\\/Date(1435161600000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"张龙\",\"idtype\":0,\"cardno\":\"610103197010032517\",\"PassType\":0,\"Ename\":\"\",\"BabyBirthday\":\"\\/Date(-62135596800000+0800)\\/\",\"ChildBirthday\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"18101810679\",\"RMKOfficeNoList\":[\"CGQ203\"],\"RMKRemark\":null,\"Pnr\":null}";

            EtermProxy.Proxy proxy = new EtermProxy.Proxy();            
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, ss);
            Console.WriteLine("返回结果：" + sret);
        }
    }
}
