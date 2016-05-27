using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisDemo
{
    [Serializable]
    public class UserInfo
    {
       public int Id { set; get; }
       public string UserName { set; get; }
       public int Age { set; get; }
    }
}
