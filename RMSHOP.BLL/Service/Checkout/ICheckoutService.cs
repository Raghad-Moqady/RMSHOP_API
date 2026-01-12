using RMSHOP.DAL.DTO.Request.Checkout;
using RMSHOP.DAL.DTO.Response.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Checkout
{
    public interface ICheckoutService
    {
         Task<CheckoutResponse> CheckoutAsync(string userId, CheckoutRequest request);
    }
}
