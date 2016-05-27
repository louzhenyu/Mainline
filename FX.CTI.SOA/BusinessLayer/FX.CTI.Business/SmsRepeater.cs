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
using FX.CTI.MQBusMgr;
using log4net;

namespace FX.CTI.Business
{
    /// <summary>
    /// 短信转发器
    /// </summary>
    public class SmsRepeater
    {
        private SMSResponse _eResponse = new SMSResponse();
        private ILog loger = LogManager.GetLogger(typeof(SmsRepeater));
        /// <summary>
        /// 发送邮件请求到MQ
        /// </summary>
        /// <param name="eRequest"></param>
        /// <returns></returns>
        public SMSResponse SendSMS(SMSRequest eRequest)
        {
            if (!CheckParas(eRequest))
            {
                return _eResponse;
            }
            var smsId = Guid.NewGuid().ToString("N");
            var smsFacade = new SMSFacade();
            try
            {
                while (smsFacade.IsSMSIdExist(smsId))
                {
                    smsId = Guid.NewGuid().ToString("N");
                }
            }
            catch (Exception ex)
            {
                loger.Error("数据库查询SMSId是否存在失败--SMSId:{0}--Exception:{1}".FormatWith(new object[] { smsId, ex }));
            }
            var smsDto = ConverToCommonDTO(smsId, eRequest);
            _eResponse.Success = true;
            _eResponse.SMSId = smsId;
            _eResponse.ErrMsg = "";

            loger.Info("开始转发--SMSId:{0}".FormatWith(smsId));
            var bus = MqBusMgr.GetInstance();
            if (!bus.IsConnected || bus.IsNull())
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = "无法连接RabbitMQ";
                loger.Error("无法连接RabbitMQ--SMSId:{0}".FormatWith(smsId));
            }
            try
            {
                bus.PublishAsync(smsDto).ContinueWith(task =>
                {
                    if (!(task.IsCompleted && !task.IsFaulted))
                    {
                        _eResponse.Success = false;
                        _eResponse.ErrMsg = string.Format("发送RabbitMQ失败");
                        loger.Error("发送RabbitMQ失败--SMSId:{0}--Exception:{1}".FormatWith(new object[] { smsId, task.Exception }));
                    }
                }).Wait();
            }
            catch (Exception ex)
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = string.Format("发送RabbitMQ失败");
                loger.Error("发送RabbitMQ失败--SMSId:{0}--Exception:{1}".FormatWith(new object[] { smsId, ex }));
            }
            //保存数据库
            var sms = new SMS
            {
                SMSId = smsDto.Id,
                AppId = smsDto.AppId,
                SMSContent = smsDto.Content,
                SMSErrMsg = _eResponse.ErrMsg,
                SMSMobile = smsDto.Mobile,
                SMSReceivedTime = DateTime.Now,
                SMSSentTime = DateTime.Now,
                SMSStatus = SMSStatus.Received.ToString()
            };
            try
            {
                smsFacade.AddSMS(sms);
            }
            catch (Exception ex)
            {
                loger.Error("数据库增加SMS记录失败--SMSId:{0}--Exception:{1}".FormatWith(new object[] { smsId, ex }));
            }
            loger.Info("结束转发--SMSId:{0}--结果:{1}".FormatWith(new object[] { smsId, _eResponse.Success }));
            return _eResponse;
        }

        /// <summary>
        /// 参数检查
        /// </summary>
        /// <param name="eRequest">SMSRequest对象</param>
        /// <returns>参数合法，返回true;参数非法，返回false</returns>
        private bool CheckParas(SMSRequest eRequest)
        {
            if (eRequest == null)
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = "SMSRequest对象不能为null.";
                return false;
            }
            if (string.IsNullOrEmpty(eRequest.AppId))
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = "AppId不能为null或空.";
                return false;
            }
            if (string.IsNullOrEmpty(eRequest.Mobile))
            {
                _eResponse.Success = false;
                _eResponse.ErrMsg = "Mobile不能为null或空.";
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
        /// SMSRequest对象转换成类型为SMSDTO的数据传输对象
        /// </summary>
        /// <param name="smsId">SMS编号</param>
        /// <param name="eRequest">SMSRequest对象</param>
        /// <returns>类型为SMSDTO的数据传输对象</returns>
        private SMSDTO ConverToCommonDTO(string smsId, SMSRequest eRequest)
        {
            return new SMSDTO
            {
                Id = smsId,
                AppId = eRequest.AppId,
                Mobile = eRequest.Mobile,
                Content = eRequest.Content
            };
        }
    }
}
