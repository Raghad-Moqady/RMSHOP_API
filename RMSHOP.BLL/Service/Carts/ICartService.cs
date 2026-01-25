using RMSHOP.DAL.DTO.Request.Cart;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.DTO.Response.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Carts
{
    public interface ICartService
    {
        Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request);
        Task<CartSummaryResponse> GetCartSummaryForUserAsync(string userId,string lang);
        Task<BaseResponse> ClearCartAsync(string userId);
        Task<BaseResponse> RemoveItemFromUserCartAsync(int productId, string userId);
    }
}
