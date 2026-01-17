using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMSHOP.BLL.Service.Checkout;
using RMSHOP.DAL.DTO.Request.Checkout;
using Stripe.Checkout;
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

         
        // Stripe doesn't have a user token on the checkout page, so...
        // When Stripe calls this action, it doesn't send any token, so we add [AllowAnonymous].
        // However, this action needs to know which user created the payment session.
        // for example, to delete the user's cart from Database later .
        // Solution: send and retrieve the userId using the session_id that Stripe sends in the link.
        [HttpGet("success")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromQuery] string session_id)
        {
            var service = new SessionService();
            var session = service.Get(session_id);
            var userId = session.Metadata["UserId"];
            return Ok(new {message="Success" ,userId });
        }
    }
}
