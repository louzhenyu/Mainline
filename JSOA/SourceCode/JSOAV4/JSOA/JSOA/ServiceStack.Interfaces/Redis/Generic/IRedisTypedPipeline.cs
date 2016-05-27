using JSOA.Redis.Pipeline;

namespace JSOA.Redis.Generic
{
    /// <summary>
    /// Interface to redis typed pipeline
    /// </summary>
    public interface IRedisTypedPipeline<T> : IRedisPipelineShared, IRedisTypedQueueableOperation<T>
    {
    }
}
