using Microsoft.EntityFrameworkCore;
using RMSHOP.DAL.Data;
using RMSHOP.DAL.Models.order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateOrderAsync(Order order)
        {
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderBySessionIdAsync(string session_id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.SessionId == session_id);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatusEnum orderStatus)
        {
            return await _context.Orders
                .Where(o=>o.OrderStatus == orderStatus)
                .Include(o=>o.User)
                .ToListAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o=>o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi=>oi.Product.Translations)
                .FirstOrDefaultAsync(o=>o.Id==orderId);
        }
    }
}
