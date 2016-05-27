using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract.RequestDTO;

using ServiceStack.FluentValidation;

namespace ServiceImpl.Validations
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(r => r.OrderItemList)
                .NotEmpty()
                .WithMessage("请指定订单明细列表");
        }
    }
}
