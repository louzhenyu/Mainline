using JetermClient.BLL;
using JetermEntity;
using JetermEntity.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JetermClient.Test
{
    [TestClass]
    public class HttpPostTest
    {
        //public static void Main(string[] args)
        public static void Main33(string[] args)
        {
            


            // url如：http://120.132.136.91:18082/format=json&language=CSharp&method=TicketByBigPnr
            string url = string.Format("{0}&language=CSharp&method={1}", ConfigurationManager.AppSettings["HttpPostURL"], "TicketByBigPnr");          
            string strParams = "{\"BigPnr\":\"MJK958\",\"FlightNo\":\"JD5357\",\"SCity\":\"PEK\",\"ECity\":\"ERL\"}";
            //string strParams = "{\"BigPnr\":\"NVZSHM\",\"FlightNo\":\"MU5844\",\"SCity\":\"CTU\",\"ECity\":\"KMG\"}";
            url = "http://114.80.69.243:9999/format=json&language=CSharp&method=SeekPNR";
            strParams="{\"Pnr\":\"HYEHSM\",\"PassengerType\":0,\"GetPrice\":true}";

            EtermClient client = new EtermClient();

            try
            {
                client.Invoke(123, "format=json&method=SeekPNR&officeno=SHA243", "system(\"RT KZ9EZN\");\r\nreturn DATA;", new TimeSpan(0, 0, 50));
                //client.Invoke(123, "format=json&method=SeekPNR&officeno=SHA244", "system(\"RT KZ9EZN\");\r\nreturn DATA;", new TimeSpan(0, 0, 50));
                //client.Invoke(123, "format=json&method=SeekPNR&officeno=SHA243", "system(\"RT KZ9EZN\");\r\nreturn DATAss;", new TimeSpan(0, 0, 50));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return;


            CommandResult<JetermEntity.Response.SeekPNR> sk = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(new JetermEntity.Request.Command<JetermEntity.Request.SeekPNR>() { AppId = 0, officeNo="SHA243", request = new JetermEntity.Request.SeekPNR() { Pnr="HYEHSM", PassengerType= EtermCommand.PassengerType.Adult, GetPrice=true } });
            string str = string.Empty;
            try
            {
                str = client.Invoke(-1, url, strParams, EtermCommand.ServerSource.EtermServer, TimeSpan.FromSeconds(5));
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }               

            CommandResult<JetermEntity.Response.TicketByBigPnr> result = JsonConvert.DeserializeObject<CommandResult<JetermEntity.Response.TicketByBigPnr>>(str);

            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.TicketByBigPnr>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResultBag = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResultBag) ? string.Empty : string.Format("{0}TicketByBigPnr指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResultBag)));
                Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("解析结果：" + Environment.NewLine + str);

            Console.ReadLine();
        }
    }
}
