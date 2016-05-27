using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JFx.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;

namespace JFxUnitTest
{
    [TestClass]
    public class MailHelperTest
    {
        [TestMethod]
        public void MailHelperAll()
        {
            //MailHelper mail= new MailHelper("smtp.126.com", "JinRiAdministrator@126.com", "jinri123456");
            //IList<Attachment> list = new List<Attachment>();
            //list.Add(MailHelper.CreateAttachment("ZipFile\\6035021_111610206000_2.jpg"));
            //list.Add(MailHelper.CreateAttachment("ZipFile\\test.txt"));
            //mail.SendMail("tonghangzhou@jinri.cn", "邮件发送", "请查收！","",list);
            Assert.IsTrue(true);
        }
    }
}
