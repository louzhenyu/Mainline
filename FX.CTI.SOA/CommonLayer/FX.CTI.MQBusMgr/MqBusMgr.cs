using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyNetQ;
using EasyNetQ.Loggers;
using FX.CTI.ConfigHelper;

namespace FX.CTI.MQBusMgr
{
    public static class MqBusMgr
    {
        private static IBus _bus;
        private static readonly object SyncObj = new object();
        static MqBusMgr()
        {
            var mqHost = ConfigMgr.RabbitMQHost;
            _bus = RabbitHutch.CreateBus(mqHost, reg => reg.Register<IEasyNetQLogger>(log => new NullLogger()));
        }

        public static IBus GetInstance()
        {
            if (_bus == null)
            {
                lock (SyncObj)
                {
                    if (_bus == null)
                    {
                        var mqHost = ConfigMgr.RabbitMQHost;
                        _bus = RabbitHutch.CreateBus(mqHost,
                            reg => reg.Register<IEasyNetQLogger>(log => new NullLogger()));
                    }
                }
            }
            return _bus;
        }
    }
}
