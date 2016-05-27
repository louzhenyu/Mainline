using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flight.Product.Utility
{
    public class UtilityService
    {
        static IContainer container = null;
        static IContainer Container
        {
            get
            {
                if (container == null)
                {
                    UtilityService.InitRegister();
                }
                return container;
            }
        }
        private static void InitRegister()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RedisCacheProvider>().As<ICacheProvider>();
            builder.RegisterType<LocalCacheProvider>().Named<ICacheProvider>("Local");

            builder.RegisterType<Log4netProvider>().As<ILogProvider>();
            container = builder.Build();
        }

        /// <summary>
        /// 分布式缓存：Redis
        /// </summary>
        /// <returns></returns>
        public static ICacheProvider GetRedisCache()
        {
            return container.Resolve<ICacheProvider>(new NamedParameter("redisServerIP", System.Configuration.ConfigurationManager.AppSettings["RedisFxServerIP"]));
        }

        private static ICacheProvider localCache;
        /// <summary>
        /// 获取本地缓存对象
        /// </summary>
        /// <returns></returns>
        public static ICacheProvider GetLocalCache()
        {
            if (UtilityService.localCache == null)
            {
                UtilityService.localCache = Container.ResolveNamed<ICacheProvider>("Local");
            }
            return UtilityService.localCache;
        }
        /// <summary>
        /// 获取日志对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILogProvider GetLogger(Type type)
        {
            return Container.Resolve<ILogProvider>(new NamedParameter("type", type));
        }
    }
}
