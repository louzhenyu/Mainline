using System;
using System.Configuration;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Loggers;
using Messages;
using System.Threading;

namespace Subscriber
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string mqHost = ConfigurationManager.AppSettings["RabbitMQHost"];
            using (IBus bus = RabbitHutch.CreateBus(mqHost, reg => reg.Register<IEasyNetQLogger>(log => new NullLogger())))
            {
                bus.Subscribe<DtoOrderWQ>("wq", HandleMessage);
                Console.WriteLine("正在监听消息.");
                Console.ReadLine();
            }
        }

        private static void HandleMessage(DtoOrderWQ dtoOrder)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Thread.Sleep(1000);
            Console.WriteLine("收到消息: {0}", dtoOrder.Name);
            Console.ResetColor();
        }
    }
}