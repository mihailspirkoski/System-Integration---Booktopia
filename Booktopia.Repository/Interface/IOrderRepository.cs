using Booktopia.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Repository.Interface
{
    public interface IOrderRepository
    {
        List<Order> getAllOrders();
        Order getOrderDetails(BaseEntity model);
    }
}
