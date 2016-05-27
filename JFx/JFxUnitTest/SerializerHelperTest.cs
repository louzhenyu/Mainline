using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JFx.Utils;

namespace JFxUnitTest
{
    [TestClass]
    public class SerializerHelperTest
    {
        UserInfo user = new UserInfo() { UserId = 1234, UserName = "RANEN.TONG", RegDate = DateTime.Now.AddYears(-100) };

        [TestMethod]
        public void XmlSerialize()
        {
            string xml = SerializerHelper.XmlSerialize(user);
            Console.WriteLine(xml);
            Assert.IsTrue(!string.IsNullOrEmpty(xml));
            UserInfo user2 = SerializerHelper.XmlDeserialize(xml, typeof(UserInfo)) as UserInfo;
            Assert.IsNotNull(user2);
            Console.WriteLine("XML反序列化结果：");
            Console.WriteLine(string.Format("UserId:{0}", user2.UserId));
            Console.WriteLine(string.Format("UserName:{0}", user2.UserName));
            Console.WriteLine(string.Format("RegDate:{0}", user2.RegDate));

        }
        [TestMethod]
        public void JsonSerialize()
        {
            string json = SerializerHelper.JsonSerializer(user, "yyyy-MM-dd HH:mm:ss");
            Assert.IsTrue(!string.IsNullOrEmpty(json));
            Console.WriteLine(json);

            UserInfo user2 = SerializerHelper.JsonDeserialize<UserInfo>(json);
            Assert.IsNotNull(user2);
            Console.WriteLine("JSON反序列化结果：");
            Console.WriteLine(string.Format("UserId:{0}", user2.UserId));
            Console.WriteLine(string.Format("UserName:{0}", user2.UserName));
            Console.WriteLine(string.Format("RegDate:{0}", user2.RegDate));
        }


        public class UserInfo
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public DateTime RegDate { get; set; }
        }
    }
}
