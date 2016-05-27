using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JetermEntity.Request;
using JetermEntity;
using JetermEntity.Response;
using JetermClient.BLL;
using JetermClient.Utility;

namespace JetermClient.Test
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void TestAV()
        {
            string av =
@"AV:CA1885/15MAY                                                               " + Environment.NewLine + "DEP TIME   ARR TIME   WEEK FLY   GROUND TERM  TYPE MEAL  DISTANCE NWST         " + Environment.NewLine + "PEK 1830   SHA 2040   SUN  2:10         T3/T2 330  DDD   1076      W           " + Environment.NewLine + "TOTAL JOURNEY TIME   2:10                                                      " + Environment.NewLine + "PEKSHA FA AA YA BQ MQ UQ HQ QQ VQ WQ SQ TS LQ N3 KQ                            " + Environment.NewLine + "MEMBER OF STAR ALLIANCE                                                        " + Environment.NewLine;
            CommandResult<JetermEntity.Response.AV> response = new JetermEntity.Parser.AV("", "").ParseCmdResult(av);
            Console.ReadLine();
        }


        public static void TestRT()
        {

            

//            string pnrText = @"  1.张洋城 JQF0B7
//             2.  CZ3691 Y   FR17APR  KWECAN HK1   1245 1420          E T2-- 
//                 -CA-PLG89Z 
//             3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG   
//             4.TL/1802/16APR/SHA888 
//             5.SSR FOID CZ HK1 NI420700198103020999/P1  
//             6.SSR ADTK 1E BY TPE16APR15/1902 OR CXL CZ BOOKING 
//             7.OSI CZ CTCT18210003200   
//             8.RMK TJ AUTH KWE122   
//             9.RMK CA/PLG89Z
//            10.TPE567   
//            [price]>PAT:A  
//            01 Y FARE:CNY960.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1010.00
//            SFC:01   SFN:01   
//            [eTerm:o77a6491]";
            string pnrText =
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
            CommandResult<JetermEntity.Response.SeekPNR> response = new JetermEntity.Parser.SeekPNR().ParseCmdResult(pnrText);
            Console.ReadLine();
        }
    }
}
