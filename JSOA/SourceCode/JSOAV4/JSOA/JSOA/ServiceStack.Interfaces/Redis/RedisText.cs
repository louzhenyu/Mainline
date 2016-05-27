using System.Collections.Generic;

namespace JSOA.Redis
{
    public class RedisText
    {
        public string Text { get; set; }

        public List<RedisText> Children { get; set; }
    }
}