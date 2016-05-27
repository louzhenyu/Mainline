using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FX.CTI.Business;
using FX.CTI.Entity.DTO;

namespace Test_EmailRepeater
{
    class Program
    {
        static void Main(string[] args)
        {
            var repeater = new EmailRepeater();
            var emailRequest = new EmailRequest
            {
                AppId = "801",
                CC = "",
                Content = "测试123",
                Subject = "测试123",
                ToAddr = "louzhenyu@jinri.cn"
            };
            var emailResult = repeater.SendEmail(emailRequest);
            Console.WriteLine("EmailId:{0}", emailResult.EmailId);
            Console.WriteLine("Success:{0}", emailResult.Success);
            Console.WriteLine("ErrMsg:{0}", emailResult.ErrMsg);
            Console.ReadKey();
        }
    }
}
