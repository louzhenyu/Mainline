using System;
using System.Configuration;
using System.Text;
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
            const int sendCount = 50;
            using (IBus bus = RabbitHutch.CreateBus(mqHost, reg => reg.Register<IEasyNetQLogger>(log => new NullLogger())))
            {
                for(int i = 0; i < sendCount; ++i)
                {
                    bus.Publish(new DtoOrderWQ
                    {
                        Id = i,
                        Name = "消息" + i
                    });
                }
            }
            Console.WriteLine("已发送{0}个消息.", sendCount);
            Console.ReadKey();
        }
    }
}