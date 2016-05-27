using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract.RequestDTO;
using ServiceContract.ResponseDTO;
using ServiceImpl.IService;
using ServiceImpl.Business;

using System.Net;
using ServiceStack;

namespace ServiceHost
{
    public class OrderService : Service, IOrderService
    {
        private IOrderRepository OrderRepository { get; set; }

        public OrderService()
        {
            OrderRepository = new OrderRepository();
        }

        //获取订单列表：
        public OrderListResponse Get(GetOrderList request)
        {
            OrderListResponse result = OrderRepository.GetOrderList();
            return result;
        }

        //获取指定订单详情：
        public OrderResponse Get(GetOrder request)
        {
            OrderResponse result = OrderRepository.GetOrder(request);
            if (result == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return default(OrderResponse);
            }

            return result;
        }

        //新增张订单：
        public OrderResponse Post(Order request)
        {
            OrderResponse result = OrderRepository.Add(request);

            Response.AddHeader("Location", Request.AbsoluteUri + "/" + result.Id);
            Response.StatusCode = (int)HttpStatusCode.Created;

            return result;
        }

        //更新指定订单详情：
        public OrderResponse Put(Order request)
        {
            OrderResponse result = OrderRepository.GetOrder(request.Id);
            if (result == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return default(OrderResponse);
            }

            result = OrderRepository.Update(request);

            return result;
        }

        //删除指定订单：
        public HttpResult Delete(DeleteOrder request)
        {
            OrderResponse result = OrderRepository.GetOrder(request.Id);
            if (result == null)
            {
                return new HttpResult { StatusCode = HttpStatusCode.NotFound };
            }

            OrderRepository.Delete(request.Id);

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }
    }
}
