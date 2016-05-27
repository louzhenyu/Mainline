using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using ServiceContract;
using ServiceContract.RequestDTO;
using ServiceContract.ResponseDTO;
using ServiceStack;
using ServiceStack.Text;
using ServiceStack.ProtoBuf;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderSOAClient orderSOAClient = new OrderSOAClient();
            orderSOAClient.GetOrderListWithJsonServClt();
            orderSOAClient.GetOrderWithJsonServClt();
            orderSOAClient.CreateOrderWithJsonServClt();
            orderSOAClient.UpdateOrderWithJsonServClt();
            orderSOAClient.DeleteOrderWithJsonServClt();

            ProductSOAClient productSOAClient = new ProductSOAClient();
            productSOAClient.GetProductListWithProtoBufServClt();
            productSOAClient.GetProductWithProtoBufServClt();
        }        
    }
}
