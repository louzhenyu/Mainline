using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceContract.ResponseDTO
{
    public class OrderListResponse
    {
        public List<OrderResponse> OrderList { get; set; }
    }

    public class OrderResponse
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public bool IsTakeAway { get; set; }
        public DateTime CreatedDate { get; set; }
        public StatusCode StatusCode { get; set; }
        public List<OrderItemResponse> OrderItemList { get; set; }
    }
}
