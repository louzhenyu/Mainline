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
            using (IBus bus = RabbitHutch.CreateBus(mqHost, reg => reg.Register<IEasyNetQLogger>(log => new NullLogger())))
            {
                Console.WriteLine("请输入发送的主题:shanghai.beijing或lasa.beijing或shanghai.chongqing等.");
                while (true)
                {
                    string topic = Console.ReadLine();
                    bus.Publish(new DtoOrderTopic
                    {
                        Id = 100,
                        Name = "主题为" + topic + "的消息"
                    }, topic);
                }
            }
        }
    }
}