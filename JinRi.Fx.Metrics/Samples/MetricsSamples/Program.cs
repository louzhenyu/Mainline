using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.Diagnostics;
using Metrics;

namespace MetricsSamples
{
    class Program
    {        
        private readonly Meter meter = JMetric.Meter("Metrics.Test.Meter", Unit.Custom("单"));
        private readonly Histogram histogram = JMetric.Histogram("Metrics.Test.Histogram", Unit.Custom("ms"));    

        static void Main(string[] args)
        {
            Program client = new Program();
            client.CreateOrderExample();
            client.SearchFlightExample();

            Console.WriteLine(string.Format("[{0}] done setting things up", DateTime.Now.ToString("HH:mm:ss")));
            Console.ReadKey();
        }

        private void CreateOrderExample()
        {
            new Thread(() =>
            {
                while (true)
                {
                    // 调用创建订单方法：
                    CreateOrder();
                    Random ran = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
                    Thread.Sleep(ran.Next(500, 5000));
                }
            }).Start();
        }

        // 创建订单方法：
        void CreateOrder()
        {
            // 省略创建订单业务逻辑代码：
            // ...
            // 创建订单方法调用一次，就记录一次：
            meter.Mark(); 
        }

        private void SearchFlightExample()
        {
            new Thread(() =>
            {
                while (true)
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    // 调用航班查询引擎方法：
                    SearchFlight();
                    stopWatch.Stop();
                    // 将航班查询引擎方法调用耗时写入Histogram度量器：
                    histogram.Update(stopWatch.ElapsedMilliseconds);
                }
            }).Start();
        }
        
        // 航班查询引擎方法：
        void SearchFlight()
        {
            // 模拟航班查询引擎业务逻辑：
            Random ran = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            Thread.Sleep(ran.Next(500, 5000));
        }
    }    
}
