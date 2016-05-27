using System;
using System.Configuration;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Loggers;
using Messages;

namespace SubscriberA
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string mqHost = ConfigurationManager.AppSettings["RabbitMQHost"];
            using (IBus bus = RabbitHutch.CreateBus(mqHost, reg => reg.Register<IEasyNetQLogger>(log => new NullLogger())))
            {
                const string topic = "shanghai.*";
                bus.Subscribe<DtoOrderTopic>("topic", HandleMessage, x=>x.WithTopic(topic));
                Console.WriteLine("接收者A正在监听主题为shanghai.*的消息,按Enter退出.");
                Console.ReadLine();
            }
        }

        private static void HandleMessage(DtoOrderTopic dtoOrder)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("主题为shanghai.*的消费者收到消息: {0}", dtoOrder.Name);
            Console.ResetColor();
        }
    }
}