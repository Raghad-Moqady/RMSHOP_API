using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using RMSHOP.DAL.DTO.Request.Checkout;
using RMSHOP.DAL.DTO.Response.Cart;
using RMSHOP.DAL.DTO.Response.Checkout;
using RMSHOP.DAL.Models;
using RMSHOP.DAL.Models.order;
using RMSHOP.DAL.Repository.Carts;
using RMSHOP.DAL.Repository.OrderItems;
using RMSHOP.DAL.Repository.Orders;
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
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IOrderItemRepository _orderItemRepository;

        public CheckoutService(ICartRepository cartRepository, IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IOrderItemRepository orderItemRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _emailSender = emailSender;
            _orderItemRepository = orderItemRepository;
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
            Order order= new Order()
            {
                  PaymentMethod= request.PaymentMethod,  
                  AmountPaid = cartTotal,
                  UserId=userId,
            };
            
            //5:
            if (request.PaymentMethod == PaymentMethodEnum.Cash)
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "cash"
                };
            }
            else if(request.PaymentMethod == PaymentMethodEnum.Visa)
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

                order.SessionId= session.Id;
                await _orderRepository.CreateOrderAsync(order); 

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

        public async Task<CheckoutResponse> HandleSuccessAsync(string session_id)
        {
            var service = new SessionService();
            var session = service.Get(session_id);
            var userId = session.Metadata["UserId"];

            //update order تكملة
            var order = await _orderRepository.GetOrderBySessionIdAsync(session_id);
            order.PaymentId= session.PaymentIntentId;
            order.OrderStatus = OrderStatusEnum.Approved;
            await _orderRepository.UpdateAsync(order);
             
            
            var user = await _userManager.FindByIdAsync(userId);

            //1.Transferring products from the user cart to orderItem table
            //Remember that the order contain list of order items(products)

            //ما زبطت لازم اتأكد منها :
            //var cartItems = await _cartRepository.GetCartItemsForUserAsync(userId);
            //var orderItems = cartItems.BuildAdapter().AddParameters("orderId", order.Id).AdaptToType<List<OrderItem>>();
            //await _orderItemRepository.CreateRangeAsync(orderItems);

            var cartItems = await _cartRepository.GetCartItemsForUserAsync(userId);
            var orderItems =new List<OrderItem>();
            foreach(var cartItem in cartItems)
            {
                var orderItem = new OrderItem()
                {
                     OrderId= order.Id,
                     ProductId= cartItem.ProductId,
                     Quantity= cartItem.Count,
                     UnitPrice=cartItem.Product.Price,
                     TotalPrice=cartItem.Product.Price * cartItem.Count,
                };
                orderItems.Add(orderItem);
            }
            //Solve N+1 problem by :
            await _orderItemRepository.CreateRangeAsync(orderItems);

            ////2.Clear Cart
            await _cartRepository.ClearCartAsync(userId);
            //3.Send email to user 
            await _emailSender.SendEmailAsync(user.Email, "Payment Successfully",
                $@"
                     <div style='text-align:center; font-family: Arial, sans-serif;'>
        <h1 style='color:orange;'>Payment Successful</h1>

        <p>Hello {user.UserName},</p>

        <p>
            Your payment for <strong>KidZone Store</strong> has been completed successfully.
            Thank you for your purchase!
        </p>

        <p style='font-size:14px; color:gray;'>
            If you have any questions regarding your order, please contact our support team.
        </p>
    </div>
                    ");


            return new CheckoutResponse()
            {
                 Success= true,
                 Message="Payment Completed Successfully!"
            };
        }
    }
}
