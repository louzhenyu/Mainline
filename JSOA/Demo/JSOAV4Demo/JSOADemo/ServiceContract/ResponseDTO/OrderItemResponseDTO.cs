using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceContract.ResponseDTO
{
    public class OrderItemListResponse
    {     
        public List<OrderItemResponse> Items { get; set; }
    }

    public class OrderItemResponse
    {
        public int Id { get; set; }
        public ProductResponse Product { get; set; }
        public int Quantity { get; set; }
    }
}
