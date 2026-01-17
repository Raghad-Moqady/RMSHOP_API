using RMSHOP.DAL.DTO.Request.Checkout;
using RMSHOP.DAL.DTO.Response.Checkout;
using RMSHOP.DAL.Repository.Carts;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Checkout
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;

        public CheckoutService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<CheckoutResponse> CheckoutAsync(string userId, CheckoutRequest request)
        {
            // 1. check :if user Cart is Empty or not
            // 2. if cart is not empty => check if each cartItem count is still available in stock or not 
            // 3. calculate Cart total again >> from user cart in DB , اضمن 
            // 4. Create Order
            // 5. check PaymentMethod (visa or cash?)
            // 6. بناء عليه رح يتم تحديد الرد المناسب وارساله للفرونت ليقوم بتحويل اليوزر على الصفحة المناسبة
            //////////////////////////////////
            
            //1:
            var cartItems = await _cartRepository.GetCartItemsForUserAsync(userId);
            if (!cartItems.Any())
            {
                //400
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Sorry! Your Cart Is Empty"
                };
            }
            //2+3: 
            decimal cartTotal = 0;
            foreach (var cartItem in cartItems)
            {
                if(cartItem.Count> cartItem.Product.Quantity)
                {
                    //400
                    return new CheckoutResponse
                    {
                        Success = false,
                        Message = $"The requested quantity for '{cartItem.Product.Translations.FirstOrDefault(t=>t.Language=="en").Name}' exceeds the available stock." +
                        $" Available quantity: {cartItem.Product.Quantity}."
                    };
                }
                cartTotal += cartItem.Count * cartItem.Product.Price;
            }
            
            //4. Create order 
            
            //5:
            if (request.PaymentMethod == "cash")
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "cash"
                };
            }
            else if(request.PaymentMethod == "visa")
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"https://localhost:7281/api/checkouts/success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"https://localhost:7281/api/checkouts/cancel",
                    Metadata= new Dictionary<string, string>
                    {
                        {"UserId", userId },
                    }
                };
                foreach (var cartItem in cartItems)
                {
                    options.LineItems.Add(
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",// eur
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = cartItem.Product.Translations.FirstOrDefault(t => t.Language == "en").Name,
                            },
                            UnitAmount = (long)(cartItem.Product.Price * 100),
                        },
                        Quantity = cartItem.Count,
                    });
                }
                var service = new SessionService();
                var session = service.Create(options);

                return new CheckoutResponse
                {
                     Success= true,
                     Message="Payment Session Created Successfully",
                     Url = session.Url
                };
            
            }
            else
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Payment Method is Not Supported"
                };
            }
        }
    }
}
