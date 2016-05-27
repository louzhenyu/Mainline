using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Loggers;
using Messages;

namespace Subscriber
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string mqHost = ConfigurationManager.AppSettings["RabbitMQHost"];
            using (IBus bus = RabbitHutch.CreateBus(mqHost, reg => reg.Register<IEasyNetQLogger>(log => new NullLogger())))
            {
                bus.Subscribe<DtoOrder111>("p2p", HandleMessage);
                Console.WriteLine("正在监听消息.");
                Console.ReadLine();
            }
        }

        private static void HandleMessage(DtoOrder111 dtoOrder)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("收到消息: {0}", dtoOrder.Name);
            Console.ResetColor();

        }

        private static void Main1(string[] args)
        {
            string mqHost = ConfigurationManager.AppSettings["RabbitMQHost"];
            using (IBus bus = RabbitHutch.CreateBus(mqHost, reg => reg.Register<IEasyNetQLogger>(log => new NullLogger())))
            {
                bus.SubscribeAsync<DtoOrder111>("p2p", HandleMessageAsync);
                Console.WriteLine("正在监听消息.");
                Console.ReadLine();
            }
        }

        private static Task<DtoOrder111> HandleMessageAsync(DtoOrder111 dtoOrder)
        {
            var tLongTimeWork = new Task<DtoOrder111>(LongTimeWork, dtoOrder);
            tLongTimeWork.Start();
            tLongTimeWork.ContinueWith<DtoOrder111>(ContinueTask);
            return tLongTimeWork;
        }

        private static DtoOrder111 LongTimeWork(object dtoOrder)
        {
            Thread.Sleep(10000);
            return (DtoOrder111)dtoOrder;
        }

        private static DtoOrder111 ContinueTask(Task<DtoOrder111> T)
        {
            var dtoOrder = T.Result;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("收到消息: {0}", dtoOrder.Name);
            Console.ResetColor();
            return dtoOrder;
        }
    }
}