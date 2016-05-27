using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using ServiceContract.RequestDTO;
using ServiceImpl.Business;
using ServiceImpl.Validations;

using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Validation;
using ServiceStack.ProtoBuf;
using ServiceStack.Api.Swagger;
using ServiceStack.Text;

namespace ServiceHost
{
    public class Global : System.Web.HttpApplication
    {
        public class ServiceAppHost : AppHostBase
        {
            public ServiceAppHost()
                : base("JSOA(V2.0) API Demo", typeof(ServiceAppHost).Assembly)
            {
                //配置路由规则：
                //如：/orders/[{path参数}.xml|json|html|jsv|csv][(?query参数1={值}&query参数2={值}&......&query参数n={值})]
                Routes.Add<GetOrderList>("/orders", "GET", "获取订单列表")
                      .Add<GetOrder>("/orders/{Id}", "GET", "获取指定订单详情")
                      .Add<Order>("/orders", "POST", "创建张订单")
                      .Add<Order>("/orders/{Id}", "PUT", "更新指定订单详情")
                      .Add<DeleteOrder>("/orders/{Id}", "DELETE", "删除指定订单")
                      .Add<GetProductList>("/products", "GET", "获取产品列表")
                      .Add<GetProduct>("/products/{Id}", "GET", "获取指定产品详情");

                //启用请求参数合法性验证功能：
                Plugins.Add(new ValidationFeature());            

                Plugins.Add(new ProtoBufFormat());

                JsConfig.EmitCamelCaseNames = true;
                Plugins.Add(new SwaggerFeature());                
                Plugins.Add(new CorsFeature("http://petstore.swagger.wordnik.com"));     
            }

            public override void Configure(Funq.Container container)
            {
                //启用请求参数合法性的验证：             
                container.RegisterValidator(typeof(OrderValidator));
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {         
            new ServiceAppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}