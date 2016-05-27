using JetermClient.BLL;
using JetermEntity;
using JetermEntity.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JEtermClientDemo
{
    /// <summary>
    /// 采用直连方式调用的Demo
    /// </summary>
    public partial class HttpPostDemo
    {
        // 返回结果：{"state":true,"error":null,"config":"TFP007","OfficeNo":"tt","result":{"PassengerList":[{"name":"赵南","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7842180572716"}],"FlightList":[{"FlightNo":"CZ3476","Airline":"CZ","Cabin":"T","SubCabin":"","SCity":"KWE","ECity":"CGO","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1435638000000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":330.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":380.00},"TicketStatus":1},"reqtime":"\/Date(1433226034274+0800)\/","SaveTime":1800}
        public static void HttpPostDemo_TicketByBigPnr()
        {
            // 设置URL：
            // url如：http://120.132.136.91:18082/format=json&language=CSharp&method=TicketByBigPnr
            string url = string.Format("{0}&language=CSharp&method={1}", ConfigurationManager.AppSettings["EtermHttpPostURL"], "TicketByBigPnr");                
            
            // 设置请求参数：
            JetermEntity.Request.TicketByBigPnr request = new JetermEntity.Request.TicketByBigPnr();     
            request.BigPnr = "PWF62P";
            request.FlightNo = "CZ3476";
            request.SCity = "KWE";
            request.ECity = "CGO";
            string strParams = JsonConvert.SerializeObject(request);

            EtermClient client = new EtermClient();        
            string str = string.Empty;
            try
            {
                str = client.Invoke(-1, url, strParams, EtermCommand.ServerSource.EtermServer, TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("JEtermClient抛出异常，异常信息为：{0}", ex.Message));
            }

            Console.WriteLine("解析结果：" + Environment.NewLine + str);

            CommandResult<TicketByBigPnr> result = JsonConvert.DeserializeObject<CommandResult<TicketByBigPnr>>(str);

            Console.ReadLine();
        }
    }
}
