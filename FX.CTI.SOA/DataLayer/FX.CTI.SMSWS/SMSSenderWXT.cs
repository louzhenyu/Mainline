using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using FX.CTI.ConfigHelper;
using FX.CTI.SMSWS.CTCSmsService;

namespace FX.CTI.SMSWS
{
    public class SMSSenderWXT : ISMSSender
    {
        public bool SendSMS(string mobile, string content)
        {
            bool result = false;
            string account = ConfigMgr.WXTAccount;
            string password = ConfigMgr.WXTPassword;
            string smsError = string.Empty;
            var smsService4XML = new SmsService4XMLClient();
            //短信参数设置
            string msgID = string.Empty;//该批短信编号(32位UUID)，需保证唯一，可空，建议为空
            string sign = string.Empty;//短信签名，该签名由服务端告知客户端，不可修改，为空时使用默认值
            string subCode = string.Empty;//扩展子号码，内容可空（验证格式和长度，不能超过20位）
            string sendTime = DateTime.Now.ToString("yyyyMMddHHmm");//发送时间,格式yyyyMMddHHmm,可空
            try
            {
                //构造短信XML串
                var builder = new StringBuilder();
                builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><message><account>");
                builder.Append(account);
                builder.Append("</account><password>");
                builder.Append(MD5(password));
                builder.Append("</password><msgid>");
                builder.Append(msgID);
                builder.Append("</msgid><phones>");
                builder.Append(mobile.Trim(','));
                builder.Append("</phones><content>");
                builder.Append(content + " 回复TD退订该短信");
                builder.Append("</content><sign>");
                builder.Append(sign);
                builder.Append("</sign><subcode>");
                builder.Append(subCode);
                builder.Append("</subcode><sendtime>");
                builder.Append(sendTime);
                builder.Append("</sendtime></message>");
                //通过无限通WEBSERVICE(XML）接口发送短信
                string smsResult = smsService4XML.submit(builder.ToString());
                //从返回XML中取发送结果
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(smsResult);
                XmlNodeList xmlNodes = xml.GetElementsByTagName("result");
                if (xmlNodes != null && xmlNodes.Count > 0 && int.Parse(xmlNodes[0].InnerText) == 0)
                {
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 计算字符串的MD5哈希值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        private string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5Result = md5.ComputeHash(Encoding.Default.GetBytes(str));
            return BitConverter.ToString(md5Result).Replace("-", "").ToLower();
        }
    }
}
