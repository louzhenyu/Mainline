namespace JSOA.Redis
{
    public interface IHandleClientDispose
    {
        void DisposeClient(RedisNativeClient client);
    }
}