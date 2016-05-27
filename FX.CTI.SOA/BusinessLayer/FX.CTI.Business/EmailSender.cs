using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using EasyNetQ;
using EasyNetQ.Loggers;
using FX.CTI.ConfigHelper;
using FX.CTI.Entity.DTO;
using FX.CTI.DB.FxDB;
using FX.CTI.DBEntity;
using JFx.Utils;
using log4net;

namespace FX.CTI.Business
{
    /// <summary>
    /// Email发送器
    /// </summary>
    public class EmailSender
    {
        private ILog loger = LogManager.GetLogger(typeof(EmailSender));
        /// <summary>
        /// 开始从RabbitMQ收消息,收到后发送到邮件服务器
        /// </summary>
        public void Start()
        {
            //创建到RabbitMQ服务器的连接
            CreateConn();
            //创建MailHelper的对象_mailHelper
            InitializeMailHelper();
            loger.Info("邮件windows服务已启动.");
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (!_bus.IsNull())
            {
                _bus.Dispose();
            }
            loger.Info("邮件windows服务已停止");
        }

        /// <summary>
        /// 收到RabbitMQ消息的回调
        /// </summary>
        /// <param name="emailDto"></param>
        private void HandleMessage(EmailDTO emailDto)
        {
            SendMail(emailDto);
        }

        /// <summary>
        /// 创建RabbitMQ服务器的连接
        /// </summary>
        private void CreateConn()
        {
            _mqHost = ConfigurationManager.AppSettings["RabbitMQHost"]; 
            _bus = RabbitHutch.CreateBus(_mqHost, reg => reg.Register<IEasyNetQLogger>(log => new NullLogger()));
            if (!_bus.IsConnected || _bus.IsNull())
            {
                loger.Error("连接RabbitMQ失败");
                throw new Exception("连接RabbitMQ失败");
            }
            _bus.Subscribe<EmailDTO>("Main", HandleMessage);
        }

        /// <summary>
        /// 创建MailHelper的对象_mailHelper
        /// </summary>
        private void InitializeMailHelper()
        {
            _smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            _emailSenderAddr = ConfigurationManager.AppSettings["EmailSenderAddr"];
            _emailSenderPwd = ConfigurationManager.AppSettings["EmailSenderPwd"];
            try
            {
                _mailHelper = new MailHelper(_smtpHost, _emailSenderAddr, _emailSenderPwd);
            }
            catch (Exception ex)
            {
                loger.Error("创建MailHelper对象失败--Exception:{0}".FormatWith(ex));
                throw new Exception("MailHelper对象初始化失败");
            }
            
        }

        /// <summary>
        /// 依据EmailDTO对象内容发送邮件
        /// </summary>
        /// <param name="emailDto">EmailDTO对象</param>
        private void SendMail(EmailDTO emailDto)
        {
            var emailId = emailDto.Id;
            loger.Info("开始发送--EmailId:{0}".FormatWith(emailId));
            var email = new Email
            {
                EmailId = emailDto.Id,
                AppId = emailDto.AppId,
                EmailCC = emailDto.CC,
                EmailContent = emailDto.Content,
                EmailErrMsg = "",
                EmailReceivedTime = DateTime.Now,
                EmailSentTime = DateTime.Now,
                EmailStatus = EmailStatus.Received.ToString(),
                EmailSubject = emailDto.Subject,
                EmailToAddr = emailDto.ToAddr
            };
            var emailFacade = new EmailFacade();
            bool result = true;
            string sendTo = emailDto.ToAddr;
            string title = emailDto.Subject;
            string content = emailDto.Content;
            string cc = emailDto.CC;
            //发送邮件
            try
            {
                result = _mailHelper.SendMail(sendTo, title, content, cc);
                if (!result)
                {
                    email.EmailErrMsg = "发送Email到Smtp Server失败";
                    loger.Error("发送Email到Smtp Server失败--EmailId:{0}".FormatWith(emailId));
                }
            }
            catch (Exception ex)
            {

                result = false;
                email.EmailErrMsg = "发送Email到Smtp Server异常";
                loger.Error("发送Email到Smtp Server异常--EmailId:{0}--Exception:{1}".FormatWith(new object[] { emailId, ex }));
            }

            //保存数据库
            email.EmailStatus = result ? EmailStatus.Sent.ToString() : email.EmailStatus = EmailStatus.Fail.ToString();
            email.EmailSentTime = DateTime.Now;
            try
            {
                emailFacade.UpdateEmail(email);
            }
            catch (Exception ex)
            {
                loger.Error("数据库更新Email记录失败--EmailId:{0}--Exception Msg:{1}".FormatWith(new object[] { emailId, ex }));
            }
            loger.Info("结束发送--EmailId:{0}".FormatWith(emailId));
        }

        /// <summary>
        /// RabbitMQ连接字符串
        /// </summary>
        private string _mqHost;
        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        private string _smtpHost;
        /// <summary>
        /// 邮件发送者地址
        /// </summary>
        private string _emailSenderAddr;
        /// <summary>
        /// 邮件发送者密码
        /// </summary>
        private string _emailSenderPwd;
        /// <summary>
        /// EasyNetQ连接
        /// </summary>
        private IBus _bus;
        /// <summary>
        /// 发送邮件辅助对象
        /// </summary>
        private MailHelper _mailHelper;
    }
}
