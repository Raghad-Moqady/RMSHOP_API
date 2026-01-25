using RMSHOP.DAL.Models.cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository.Carts
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartItemAsync(string userId, int productId);
        Task UpdateCartItemCountAsync();
        Task AddToCartAsync(Cart cart);

        Task<List<Cart>> GetCartItemsForUserAsync(string userId);
        Task ClearCartAsync(string userId); 
        Task RemoveCartItemAsync(Cart cartItem);
    }
}
