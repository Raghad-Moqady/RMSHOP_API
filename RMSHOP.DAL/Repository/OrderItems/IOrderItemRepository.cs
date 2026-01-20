using RMSHOP.DAL.Models.order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository.OrderItems
{
    public interface IOrderItemRepository
    {
        Task CreateRangeAsync(List<OrderItem> orderItems);
    }
}
