using RMSHOP.DAL.Data;
using RMSHOP.DAL.Models.order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository.OrderItems
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateRangeAsync(List<OrderItem> orderItems)
        {
            await _context.AddRangeAsync(orderItems);
            await _context.SaveChangesAsync();
        }
    }
}
