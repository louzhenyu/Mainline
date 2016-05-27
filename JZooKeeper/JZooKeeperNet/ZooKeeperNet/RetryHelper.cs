using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZooKeeperNet
{
    /// <summary>
    /// added this .cs file by Yang Li
    /// 由于这些API会抛Zookeeper的Exception，比如ConnectionLossException, 
    /// NoNodeException等，所以必须配合一堆try/catch的机制来catch错误，catch后再处理
    /// </summary>
    public class RetryHelper
    {
        private int retryDelay = 500;
        private long signal = 0;
        public Action IfErrorThen;
        public Action CreateNodeStructure;
        public Action FixConnectionLossAction;

        public static RetryHelper Make()
        {
            return new RetryHelper();
        }

        public void Execute(Action action)
        {
            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch (ZooKeeperNet.KeeperException.NoNodeException ex)
                {
                    if (CreateNodeStructure != null)
                        RetryHelper.Make().Execute(CreateNodeStructure);
                    continue;
                }
                catch (ZooKeeperNet.KeeperException.ConnectionLossException ex)
                {

                    long attempSignal = Interlocked.Read(ref signal);

                    while (Interlocked.Read(ref signal) > 0)
                        Thread.Sleep(retryDelay);

                    if (attempSignal == 0)
                    {
                        Interlocked.Increment(ref signal);

                        if (FixConnectionLossAction != null)
                            RetryHelper.Make().Execute(FixConnectionLossAction);

                        Interlocked.Decrement(ref signal);
                    }

                    continue;
                }
                catch (Exception ex)
                {
                    Thread.Sleep(retryDelay);
                    if (IfErrorThen != null)
                        IfErrorThen();
                    continue;
                }
            }
        }
    }
}
