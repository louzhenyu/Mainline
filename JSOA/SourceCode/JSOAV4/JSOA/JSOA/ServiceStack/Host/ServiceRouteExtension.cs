using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceStack.Web;

//added by Yang Li
namespace ServiceStack.Host
{
    public static class RouteExtension
    {
        public static IServiceRoutes Add<T>(this IServiceRoutes routes, string restPath, string verbs, string summary)
        {
            return routes.Add(typeof(T), restPath, verbs, summary, string.Empty);
        }

        public static IServiceRoutes Add<T>(this IServiceRoutes routes, string restPath, string verbs, string summary, string notes)
        {
            return routes.Add(typeof(T), restPath, verbs, summary, notes);
        }
    }
}
