using RMSHOP.DAL.DTO.Request.Cart;
using RMSHOP.DAL.DTO.Response;
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
    }
}
