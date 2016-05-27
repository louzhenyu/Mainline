using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Loggers;
using FX.CTI.ConfigHelper;
using FX.CTI.DB.FxDB;
using FX.CTI.DBEntity;
using FX.CTI.Entity.DTO;
using FX.CTI.MQBusMgr;
using log4net;

namespace FX.CTI.Business
{
    /// <summary>
    /// 邮件转发器
    /// </summary>
    public class EmailRepeater
    {
        private EmailResponse _eResponse = new EmailResponse();
        private ILog loger = LogManager.GetLogger(typeof (EmailRepeater));
        /// <summary>
        /// 发送邮件请求到MQ
        /// </summary>
        /// <param name="eRequest"></param>
        /// <returns></returns>
        public EmailResponse SendEmail(EmailRequest eRequest)
        {
            if (!CheckParas(eRequest))
            {
                return _eResponse;
            }
            var emailId = Guid.NewGuid().ToString("N");
            var emailFacade = new EmailFacade();
            try
            {
                while (emailFacade.IsEmailIdExist(emailId))
                {
                    emailId = Guid.NewGuid().ToString("N");
                }
            }
            catch (Exception ex)
            {
                loger.Error("数据库查询EmailId是否存在失败--EmailId:{0}--Exception:{1}".FormatWith(new object[] { emailId, ex }));
            }
            var emailDto = ConverToCommonDTO(emailId, eRequest);
            _eResponse.Success = true;
            _eResponse.EmailId = emailId;
            _eResponse.ErrMsg = "";
            loger.Info("开始转发--EmailId:{0}".FormatWith(emailId));
            var bus = MqBusMgr.GetInstance();
            if (!bus.IsConnected || bus.IsNull())
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = "无法连接RabbitMQ";
                loger.Error("无法连接RabbitMQ--EmailId:{0}".FormatWith(emailId));
            }
            else
            {
                try
                {
                    bus.PublishAsync(emailDto).ContinueWith(task =>
                    {
                        if (!(task.IsCompleted && !task.IsFaulted))
                        {
                            _eResponse.Success = false;
                            _eResponse.ErrMsg = string.Format("发送RabbitMQ失败");
                            loger.Error("发送RabbitMQ失败--EmailId:{0}--Exception:{1}".FormatWith(new object[] { emailId, task.Exception }));
                        }
                    }).Wait();
                }
                catch (Exception ex)
                {
                    _eResponse.Success = false;
                    _eResponse.ErrMsg = string.Format("发送RabbitMQ失败");
                    loger.Error("发送RabbitMQ失败--EmailId:{0}--Exception:{1}".FormatWith(new object[] { emailId, ex }));
                }
            }
            //保存数据库
            var email = new Email
            {
                EmailId = emailDto.Id,
                AppId = emailDto.AppId,
                EmailCC = emailDto.CC,
                EmailContent = emailDto.Content,
                EmailErrMsg = _eResponse.ErrMsg,
                EmailReceivedTime = DateTime.Now,
                EmailSentTime = DateTime.Now,
                EmailStatus = EmailStatus.Received.ToString(),
                EmailSubject = emailDto.Subject,
                EmailToAddr = emailDto.ToAddr
            };
            try
            {
                emailFacade.AddEmail(email);
            }
            catch (Exception ex)
            {
                loger.Error("数据库增加Email记录失败--EmailId:{0}--Exception:{1}".FormatWith(new object[] { emailId, ex }));
            }
            loger.Info("结束转发--EmailId:{0}--结果:{1}".FormatWith(new object[]{emailId, _eResponse.Success}));
            return _eResponse;
        }

        /// <summary>
        /// 参数检查
        /// </summary>
        /// <param name="eRequest">EmailRequest对象</param>
        /// <returns>参数合法，返回true;参数非法，返回false</returns>
        private bool CheckParas(EmailRequest eRequest)
        {
            if (eRequest == null)
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = "EmailRequest对象不能为null.";
                return false;
            }
            if (string.IsNullOrEmpty(eRequest.AppId))
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = "AppId不能为null或空.";
                return false;
            }
            if (string.IsNullOrEmpty(eRequest.ToAddr))
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = "ToAddr不能为null或空.";
                return false;
            }
            if (string.IsNullOrEmpty(eRequest.CC))
            {
                eRequest.CC = "";
            }
            if (string.IsNullOrEmpty(eRequest.Subject))
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = "Subject不能为null或空.";
                return false;
            }
            if (string.IsNullOrEmpty(eRequest.Content))
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = "Content不能为null或空.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// EmailRequest对象转换成类型为EmailDTO的数据传输对象
        /// </summary>
        /// <param name="emailId">邮件编号</param>
        /// <param name="eRequest">EmailRequest对象</param>
        /// <returns>类型为EmailDTO的数据传输对象</returns>
        private EmailDTO ConverToCommonDTO(string emailId, EmailRequest eRequest)
        {
            return new EmailDTO
            {
                Id = emailId,
                AppId =  eRequest.AppId,
                ToAddr = eRequest.ToAddr,
                CC = eRequest.CC,
                Subject = eRequest.Subject,
                Content = eRequest.Content
            };
        }
    }
}
