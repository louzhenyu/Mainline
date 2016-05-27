using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace JetermUntility
{
    [Guid("7FA6FB44-086E-4587-92BE-EF0B46CE9845")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Communication:ICommunication
    {
        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="toAddressList">接受人集合</param>
        public void SendMail(string sEmail, string Pwd, string FormServer, string subject, string body,string[] toAddressList)
        {
            SendMailHelper._FromEmail = sEmail;
            SendMailHelper._FromPWD = Pwd;
            SendMailHelper._FromServer = FormServer;
            SendMailHelper.SendMail(subject, body, toAddressList);
        }
    }
}
