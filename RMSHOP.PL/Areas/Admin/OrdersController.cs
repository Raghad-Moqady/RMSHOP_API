using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RMSHOP.BLL.Service.Orders;
using RMSHOP.DAL.DTO.Request.Orders;
using RMSHOP.DAL.Models.order;
using RMSHOP.PL.Resources;

namespace RMSHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public OrdersController(IOrderService orderService, IStringLocalizer<SharedResource> localizer)
        {
            _orderService = orderService;
            _localizer = localizer;
        }

        [HttpGet()]
        public async Task<IActionResult> GetOrdersByStatus([FromQuery] OrderStatusEnum? orderStatus)
        {
            var response = await _orderService.GetOrdersByStatusAsync(orderStatus?? OrderStatusEnum.Pending);
            return Ok(response); 
        }
         
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetailsById([FromRoute] int orderId)
        {
            var response = await _orderService.GetOrderByIdAsync(orderId);
            return Ok(response);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int orderId, [FromBody] UpdateOrderStatusRequest request)
        {
            var response= await _orderService.UpdateOrderStatusAsync(orderId,request.NewOrderStatus);
            if (!response.Success)
            {
                if (response.Message.Contains("Not Found"))
                {
                    //404
                    return NotFound(response);
                }
                else
                {
                    //400
                    return BadRequest(response);
                }
            }
            //200
            return Ok(response);

        }

    }
}
