using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JetermEntity.Response;

namespace EtermProxy.UnitTest.BLLTest
{
    [TestClass]
    public class RmkTest
    {
        /*
        测试案例：
        测试案例1：返回结果包含【-】
        测试案例2：返回结果不包含【-】
        测试案例3：返回结果为err
        （未找到案例）测试案例4：返回结果中包含\d{1,}\.
        测试案例5：返回结果中包含UNABLE TO
         */

        [TestMethod]
        public void Test_BusinessDispose()
        {
            JetermEntity.Request.Rmk request = new JetermEntity.Request.Rmk();
            // 用于测试案例1的pnr参数：
            //request.Pnr = "KWSWH9";
            // 用于测试案例2的pnr参数：
            //request.Pnr = "JV9787";
            // 以下在EtermServer中运行过：KHN117授权给SHA327：
            request.Pnr = "JXGPMY";
            // 【rt HPDL7J】在EtermServer中运行过：
            // 用于测试案例3和5：
            //request.Pnr = "HPDL7J";
            //request.Pnr = "HVQ8K9";

            // 以下参数，在EtermServer中没有运行过：
            request.RmkOfficeNoList.Add("SHA123");
            request.RmkOfficeNoList.Add("SHA456");

            EtermProxy.BLL.Rmk logic = new EtermProxy.BLL.Rmk(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Rmk> result = logic.BusinessDispose(request);

            // 运行结果：
            // 测试案例1的运行结果：
            // {"IsSuccess":true}
            // 测试案例2的运行结果：
            // {"IsSuccess":true}
            // 测试案例3的运行结果：
            // {"IsSuccess":false}
            // 测试案例5的运行结果：
            // {"IsSuccess":false}
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
