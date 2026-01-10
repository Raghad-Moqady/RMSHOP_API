using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMSHOP.BLL.Service.Carts;
using RMSHOP.DAL.DTO.Request.Cart;
using System.Security.Claims;

namespace RMSHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost("")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _cartService.AddToCartAsync(userId,request);
            // لازم نضيف حالات الايرور
            //500
            if (response.UnexpectedErrorFlag)
            {
                return StatusCode(500, response);
            }
            else if (!response.Success)
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
