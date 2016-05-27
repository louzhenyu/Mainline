using System.Collections.Generic;

namespace JSOA.Redis
{
    public class RedisData
    {
        public byte[] Data { get; set; }

        public List<RedisData> Children { get; set; } 
    }
}