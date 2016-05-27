using System;
using System.Configuration;
using EasyNetQ;
using EasyNetQ.Loggers;
using Messages;

namespace Publisher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string mqHost = ConfigurationManager.AppSettings["RabbitMQHost"];
            using (IBus bus = RabbitHutch.CreateBus(mqHost, reg => reg.Register<IEasyNetQLogger>(log => new NullLogger())))
            {
                Console.WriteLine("请输入发送的消息：");
                while (true)
                {
                    string orderName = Console.ReadLine();
                    bus.Publish(new DtoOrder111
                    {
                        Id = 100,
                        Name = orderName
                    });
                }
            }
        }
    }
}