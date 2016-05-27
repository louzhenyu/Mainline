using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientApp.TestSMS;
using ClientApp.TestEmail;
using SMSRequest = ClientApp.TestSMS.SMSRequest;
using SMSServiceClient = ClientApp.TestSMS.SMSServiceClient;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var smsClient = new ClientApp.LocalService.SMSServiceClient();
            //var smsRequest = new ClientApp.LocalService.SMSRequest
            //{
            //    AppId = "802",
            //    Content = "我特喜欢天津这个市民社会，到处都是一团和气。去买肉，售货员先问怎么吃？开头纳闷，你管我怎么吃？后来才知道人家要给你切。开车违章，警察开罢罚单说受累您给把罚款交了。没见过一言不合打架的。昨天一位爷说他在天津乘公交车，没留神踩了人家的脚。到站了，人家问：大哥你还踩吗？不踩我下车了。",
            //    Mobile = "13482767468"
            //};
            //var smsResult = smsClient.SendSMS(smsRequest);
            //Console.WriteLine("SMSId:{0}", smsResult.SMSId);
            //Console.WriteLine("Success:{0}", smsResult.Success);
            //Console.WriteLine("ErrMsg:{0}", smsResult.ErrMsg);
            //Console.ReadKey();

            //var emailClient = new EmailServiceClient();
            //var smsClient = new SMSServiceClient();
            //var emailRequest = new EmailRequest
            //{
            //    AppId = "801",
            //    CC = "",
            //    Content = "测试123",
            //    Subject = "测试123",
            //    ToAddr = "louzhenyu@jinri.cn,tonghangzhou@jinri.cn"
            //};
            //var emailResult = emailClient.SendEmail(emailRequest);
            //Console.WriteLine("EmailId:{0}", emailResult.EmailId);
            //Console.WriteLine("Success:{0}", emailResult.Success);
            //Console.WriteLine("ErrMsg:{0}", emailResult.ErrMsg);
            var smsClient = new SMSServiceClient();
            var smsRequest = new SMSRequest
            {
                AppId = "100307",
                Content = "您好,杜聪明/MU5459/2015-10-28/上海虹桥上海虹桥机场07:15—呼和浩特10:25,T2航站楼请准时登机！祝您旅途愉快！",
                Mobile = "13482767468"
            };
            var smsResult = smsClient.SendSMS(smsRequest);
            Console.WriteLine("SMSId:{0}", smsResult.SMSId);
            Console.WriteLine("Success:{0}", smsResult.Success);
            Console.WriteLine("ErrMsg:{0}", smsResult.ErrMsg);
            Console.ReadKey();
        }
    }
}
