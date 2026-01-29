using Mapster;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.DTO.Response.Orders;
using RMSHOP.DAL.DTO.Response.Products;
using RMSHOP.DAL.Models.order;
using RMSHOP.DAL.Repository.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<GetOrdersByStatusResponse>> GetOrdersByStatusAsync(OrderStatusEnum orderStatus)
        {
            var orders= await _orderRepository.GetOrdersByStatusAsync(orderStatus);
            return orders.Adapt<List<GetOrdersByStatusResponse>>(); 
        }
        public async Task<GetOrderDetailsResponse?> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            var response = order.Adapt<GetOrderDetailsResponse>(); 

            foreach (var orderItem in response.OrderItems)
            {
                var translations = new List<ProductTranslationResponse>();
                foreach(var t in order.OrderItems)
                { 
                    var orderTranslations= t.Product.Translations.Where(t=> t.ProductId == orderItem.ProductId).ToList();
                    foreach(var ot in orderTranslations)
                    {
                        var translation = new ProductTranslationResponse()
                        {
                            Name = ot.Name,
                            Description = ot.Description,
                            Language = ot.Language,
                        };
                        translations.Add(translation);
                    }
                }
                orderItem.Translations = translations; 
            }
            return response;
        }
        public async Task<BaseResponse> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newOrderStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order is null)
            {
                //404
                return new BaseResponse()
                {
                    Success = false,
                    Message = "Order Is Not Found"
                };
            }
            //الحالات المنطقية لتغيير ال status
            //Pending → Approved → Shipped → Delivered
            //Pending → Cancelled
            bool isAllowedTransition =
                    (order.OrderStatus == OrderStatusEnum.Pending && newOrderStatus == OrderStatusEnum.Approved) ||
                    (order.OrderStatus == OrderStatusEnum.Pending && newOrderStatus == OrderStatusEnum.Cancelled) ||
                    (order.OrderStatus == OrderStatusEnum.Approved && newOrderStatus == OrderStatusEnum.Shipped) ||
                    (order.OrderStatus == OrderStatusEnum.Shipped && newOrderStatus == OrderStatusEnum.Delivered);

            if (!isAllowedTransition)
            {
                //400
                return new BaseResponse
                {
                    Success = false,
                    Message = "Invalid order status transition"
                };
            }
            //cash or viza
            if(newOrderStatus == OrderStatusEnum.Shipped)
            {
                order.ShippedDate = DateTime.UtcNow;
            }else if(newOrderStatus == OrderStatusEnum.Delivered && order.PaymentMethod== PaymentMethodEnum.Cash)
            {
                order.PaymentStatus = PaymentStatusEnum.Paid;
            }
            order.OrderStatus = newOrderStatus; 
            await _orderRepository.UpdateAsync(order);
            //200
            return new BaseResponse()
            {
                 Success = true,
                 Message= "Order status updated successfully"
            };
             

        }
    }
}
