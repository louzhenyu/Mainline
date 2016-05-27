using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract.ResponseDTO;
using ServiceStack;

namespace ServiceContract.RequestDTO
{
    //用于获取订单列表：
    public class GetOrderList : IReturn<OrderListResponse>
    {

    }

    //用于获取指定订单详情：
    public class GetOrder : IReturn<OrderResponse>
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsTakeAway { get; set; }
        public StatusCode? StatusCode { get; set; }
    }

    //用于删除指定的订单：
    public class DeleteOrder : IReturn<HttpResult>
    {
        public int Id { get; set; }
    }

    //用于新增或更新订单：  
    public class Order : IReturn<OrderResponse>
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public bool IsTakeAway { get; set; }
        public DateTime CreatedDate { get; set; }
        public StatusCode StatusCode { get; set; }
        public List<OrderItem> OrderItemList { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StatusCode StatusCode { get; set; }
    }
}
