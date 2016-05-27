using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract.RequestDTO;
using ServiceContract.ResponseDTO;

using ServiceStack;

namespace ServiceImpl.IService
{
    public interface IOrderService
    {
        //获取订单列表：
        OrderListResponse Get(GetOrderList request);
        //获取指定订单详情：
        OrderResponse Get(GetOrder request);
        //新增张订单：
        OrderResponse Post(Order request);
        //更新指定订单详情：
        OrderResponse Put(Order request);
        //删除指定订单：
        HttpResult Delete(DeleteOrder request);
    }
}
