using JSOA.Redis.Pipeline;

namespace JSOA.Redis
{
    /// <summary>
    /// Base transaction interface, shared by typed and non-typed transactions
    /// </summary>
    public interface IRedisTransactionBase : IRedisPipelineShared
    {
    }
}
