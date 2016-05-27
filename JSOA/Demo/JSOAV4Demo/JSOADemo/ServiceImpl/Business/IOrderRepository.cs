using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract.ResponseDTO;
using ServiceContract.RequestDTO;

namespace ServiceImpl.Business
{
    public interface IOrderRepository
    {
        OrderListResponse GetOrderList();
        OrderResponse GetOrder(GetOrder request);
        OrderResponse GetOrder(int orderId);
        OrderResponse Add(Order request);
        OrderResponse Update(Order request);           
        void Delete(int orderId);
    }
}
