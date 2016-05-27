using System;
namespace JSOA.Redis
{
    public interface IRedisSentinel : IDisposable
    {
        IRedisClientsManager Start();
    }
}
