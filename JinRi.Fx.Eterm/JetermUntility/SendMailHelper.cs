using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace JetermUntility
{
    public class SendMailHelper
    {
        public static string _FromEmail = "nagios1@21cn.com";
        public static string _FromPWD = "nagios1nagios1";
        public static string _FromServer = "smtp.21cn.com";
        
        /// <summary>
        /// 用户发送邮件
        /// </summary>
        public bool SendEmail(string from, string pass, string to, string subject, string content, string host, string port, bool isHtml, bool enableSSL, bool shiledHead)
        {
            MailAddress Efrom = new MailAddress(from);
            MailMessage mail = new MailMessage();
            #region//可屏蔽垃圾箱的功能
            if (shiledHead)
            {
                mail.Headers.Add("X-Mailer", "Tom");
                mail.Headers.Add("X-Priority", "3");
                mail.Headers.Add("X-MSMail-Priority", "Normal");
                mail.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");
                mail.Headers.Add("ReturnReceipt", "1");
            }
            #endregion
            //邮件主题
            mail.Subject = subject;
            //发件人
            mail.From = Efrom;
            //收件人
            mail.To.Add(new MailAddress(to));
            //邮件内容
            mail.Body = content;
            //设置邮件编码
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            //是否用HTML格式显示
            mail.IsBodyHtml = isHtml;
            //邮件的优先级(高|正常|低)
            mail.Priority = MailPriority.High;
            //邮件发送通知
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            //SMTP传输协议
            SmtpClient client = new SmtpClient();
            //设置用于SMTP事物的主机名称，填IP地址也可以
            client.Host = host;
            //设置用于SMTP事物的端口，默认就是25
            if (String.IsNullOrEmpty(port)) client.Port = 25;
            else client.Port = Int32.Parse(port);
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(from, pass);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = enableSSL;
            try
            {
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                return false;
            }
        }

        /// <summary>
        /// 利用SMTP发送邮件
        /// </summary>
        /// <param name="fromName">发件人名称</param>
        /// <param name="fromAddress">发件人邮箱地址</param>
        /// <param name="toAddressList">收件人邮箱地址集合</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="attachList">邮件附件集合，没有附件可为null（附件的文件地址为绝对路径）</param>
        /// <param name="loginName">用于发送邮件的邮箱的登录名</param>
        /// <param name="loginPwd">用于发送邮件的邮箱的登录密码</param>
        /// <param name="mh">用户发送邮件的邮箱的SMTP服务器(MailHost类中的一个属性)</param>
        private static void SendEMail(string fromName, string fromAddress, string[] toAddressList, string subject, string body, List<string> attachList, string loginName, string loginPwd, string mailHost)
        {
            MailMessage mail = new MailMessage();//实例化一封邮件
            mail.From = new MailAddress(fromAddress, fromName);//设置发件人名称和发件人邮箱地址
            foreach (string str in toAddressList)
            {
                mail.To.Add(str);//遍历添加收信人邮箱地址
            }
            mail.Subject = subject;//设置邮件主题
            mail.Body = body;//设置邮件正文
            mail.BodyEncoding = Encoding.UTF8;//设置正文编码方式
            mail.IsBodyHtml = true;//设置正文是否以html格式发送
            if (attachList != null && attachList.Count > 0 && attachList != null)//判断是否存在附件
            {
                foreach (string str in attachList)//遍历附件集合
                {
                    if (str != null && str != "" && File.Exists(str))//如果绝对路径不为空，且该路径下的文件存在
                    {
                        mail.Attachments.Add(new Attachment(str));//为该邮件添加一个附件
                    }
                }
            }

            SmtpClient smtp = new SmtpClient();//实例化一个用于发送邮件的SMTP客户端
            smtp.Host = mailHost;//设置SMTP服务器地址（用于发送邮件的邮箱的SMTP服务器地址）
            smtp.Credentials = new NetworkCredential(loginName, loginPwd);//用于发送邮件的邮箱的登录名和密码
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;//设置该邮件通过网络方式发送到SMTP服务器
            smtp.Send(mail);//发送
        }

        /// <summary>
        /// 利用今日邮件服务器发送SMTP发送邮件;SMTPD地址：smtp.jinri.cn
        /// </summary>
        /// <param name="send">发件人</param>
        /// <param name="toAddressList">收件人邮箱地址集合</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        public static void SendMail(string send, string subject, string body, string[] toAddressList)
        {

            //SendMailHelper.SendEMail("今日监控系统", send, toAddressList,
            //subject, body, null, "SysAutoOrderMonitor@jinri.cn", "jinri8888", "smtp.jinri.cn");
            SendMailHelper.SendEMail("今日监控系统", send, toAddressList,
            subject, body, null, _FromEmail, _FromPWD, _FromServer);

        }


        /// <summary>
        /// 利用今日邮件服务器发送SMTP发送邮件;SMTPD地址：smtp.jinri.cn
        /// </summary>
        /// <param name="toAddressList">收件人邮箱地址集合</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        public static void SendMail(string subject, string body, string[] toAddressList)
        {

            //SendMailHelper.SendEMail("今日监控系统", "SysAutoOrderMonitor@jinri.cn", toAddressList,
            //subject, body, null, "SysAutoOrderMonitor@jinri.cn", "jinri8888", "smtp.jinri.cn");
            SendMailHelper.SendEMail("今日监控系统", _FromEmail, toAddressList,
           subject, body, null, _FromEmail, _FromPWD, _FromServer);

        }
    }
}
