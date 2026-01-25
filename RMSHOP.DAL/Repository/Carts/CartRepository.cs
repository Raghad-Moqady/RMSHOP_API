using Microsoft.EntityFrameworkCore;
using RMSHOP.DAL.Data;
using RMSHOP.DAL.Models.cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository.Carts
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Cart?> GetCartItemAsync(string userId, int productId)
        {
            //Include?
             return await _context.Carts.FirstOrDefaultAsync
                (c=>c.UserId==userId && c.ProductId==productId);
        }

        public async Task UpdateCartItemCountAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddToCartAsync(Cart cart)
        {
            await _context.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Cart>> GetCartItemsForUserAsync(string userId)
        {
            return await _context.Carts
                .Where(c=>c.UserId==userId)
                .Include(c=>c.Product.Translations)
                .Include (c=>c.Product.Category.Translations)
                .ToListAsync();
        }

        public async Task ClearCartAsync(string userId)
        {
            var cartItems= await _context.Carts.Where(c=>c.UserId == userId).ToListAsync();
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(Cart cartItem)
        {
            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}
