using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.DTO.Response.Orders;
using RMSHOP.DAL.Models.order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Orders
{
    public interface IOrderService
    {
        Task<List<GetOrdersByStatusResponse>> GetOrdersByStatusAsync(OrderStatusEnum orderStatus);
        Task<GetOrderDetailsResponse?> GetOrderByIdAsync(int orderId);
        Task<BaseResponse> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newOrderStatus);
        
    }
}
