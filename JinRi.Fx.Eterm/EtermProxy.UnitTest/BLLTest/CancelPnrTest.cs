using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JetermEntity.Response;

namespace EtermProxy.UnitTest
{
    [TestClass]
    public class CancelPnrTest
    {
        [TestMethod]
        public void Test_BusinessDispose()
        {
            JetermEntity.Request.CancelPnr request = new JetermEntity.Request.CancelPnr();
            /*
            //request.Pnr = "JXGPMY";
            request.Pnr = "HSZ42G";
            request.Pnr = "HPDL7J";
            //request.Pnr = "JGZ03K";
            //request.Pnr = "KN0SNL";            
            request.CancelOut = false;
             */
            //request.Pnr = "JDW8X8";
            //request.CancelOut = true;

            //request.Pnr = "JNN6CN";
            //request.CancelOut = true;

            request.Pnr = "JN344G";
            request.CancelOut = true;

            EtermProxy.BLL.CancelPnr logic = new EtermProxy.BLL.CancelPnr(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.CancelPnr> result = logic.BusinessDispose(request);

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
                    Console.WriteLine(string.Format("返回失败，失败原因：{0}", result.error.ErrorMessage));
                    //Console.ReadLine();
                    return;
                }

                Console.WriteLine("返回失败");
                //Console.ReadLine();
                return;
            }

            Console.WriteLine("返回结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
            //Console.ReadLine(); 
        }
    }
}
