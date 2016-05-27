using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using EasyNetQ;
using EasyNetQ.Loggers;
using FX.CTI.ConfigHelper;
using FX.CTI.DB.FxDB;
using FX.CTI.DBEntity;
using FX.CTI.Entity.DTO;
using FX.CTI.SMSWS;
using log4net;


namespace FX.CTI.Business
{
    public class SMSSender
    {
        private ILog loger = LogManager.GetLogger(typeof(SMSSender));
        /// <summary>
        /// 开始从RabbitMQ收消息,收到后发送到短信服务器
        /// </summary>
        public void Start()
        {
            //创建到RabbitMQ服务器的连接
            CreateConn();
            loger.Info("短信windows服务已启动.");
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
            loger.Info("短信windows服务已停止");
        }

        /// <summary>
        /// 收到RabbitMQ消息的回调
        /// </summary>
        /// <param name="smsDto"></param>
        private void HandleMessage(SMSDTO smsDto)
        {
            try
            {
                SendSMS(smsDto);
            }
            catch (Exception ex)
            {
                loger.Error("短信发送异常--SMSId:{0}--Exception:{1}".FormatWith(new object[] { smsDto.Id, ex }));
            }
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
                throw new Exception("RabbitMQ服务器连接失败");
            }
            _bus.Subscribe<SMSDTO>("Main", HandleMessage);
        }

        /// <summary>
        /// 依据SMSDTO对象内容发送短信
        /// </summary>
        /// <param name="smsDto">SMSDTO对象</param>
        private void SendSMS(SMSDTO smsDto)
        {
            var smsId = smsDto.Id;
            loger.Info("开始发送--SMSId:{0}".FormatWith(smsId));
            var sms = new SMS
            {
                SMSId = smsDto.Id,
                AppId = smsDto.AppId,
                SMSContent = smsDto.Content,
                SMSErrMsg = "",
                SMSMobile = smsDto.Mobile,
                SMSReceivedTime = DateTime.Now,
                SMSSentTime = DateTime.Now,
                SMSStatus = SMSStatus.Received.ToString()
            };
            var smsFacade = new SMSFacade();
            //发送短信
            bool result = false;
            string mobile = smsDto.Mobile;
            string content = smsDto.Content;

            //申讯短信内容需加签名
            if (ConfigMgr.SmsChannel == 2)
            {
                if (smsDto.AppId == "100204")//OEM项目，负责人：张业华
                {
                    content += "【登机宝】";
                }
                else
                {
                    content += "【今日天下通】";
                }
            }

            var sender = SMSSenderFactory.CreateSender(ConfigMgr.SmsChannel);
            try
            {
                result = sender.SendSMS(mobile, content);
            }
            catch (Exception ex)
            {
                sms.SMSErrMsg = ex.Message;
                loger.Error("短信发送异常--SMSId:{0}--Exception:{1}".FormatWith(new object[] {smsId, ex}));
            }
            if (result)
            {
                sms.SMSErrMsg = "";
            }
            //保存数据库
            sms.SMSStatus = result ? SMSStatus.Sent.ToString() : SMSStatus.Fail.ToString();
            sms.SMSSentTime = DateTime.Now;
            try
            {
                smsFacade.UpdateSMS(sms);
            }
            catch (Exception ex)
            {
                loger.Error("数据库更新SMS记录失败--SMSId:{0}--Exception:{1}".FormatWith(new object[] { smsId, ex }));
            }
            loger.Info("结束发送--SMSId:{0}--发送结果{1}".FormatWith(new object[] {smsId, result? "成功":"失败"}));
        }

        /// <summary>
        /// RabbitMQ连接字符串
        /// </summary>
        private string _mqHost;
        /// <summary>
        /// EasyNetQ连接
        /// </summary>
        private IBus _bus;
    }
}
