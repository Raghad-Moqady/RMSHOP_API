using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMSHOP.BLL.Service.Checkout;
using RMSHOP.DAL.DTO.Request.Checkout;
using System.Security.Claims;

namespace RMSHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }
        [HttpPost("")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response= await _checkoutService.CheckoutAsync(userId, request);
            return Ok(response);
        }
    }
}
