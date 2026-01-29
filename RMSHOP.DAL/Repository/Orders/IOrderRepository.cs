using RMSHOP.DAL.Models.order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository.Orders
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<Order?> GetOrderBySessionIdAsync(string session_id);
        Task UpdateAsync(Order order);

        Task<List<Order>> GetOrdersByStatusAsync(OrderStatusEnum orderStatus);
        Task<Order?> GetOrderByIdAsync(int orderId);


    }
}
