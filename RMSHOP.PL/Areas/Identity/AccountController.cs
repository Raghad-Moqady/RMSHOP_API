using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMSHOP.BLL.Service.Identity;
using RMSHOP.DAL.DTO.Request;
using System.Threading.Tasks;

namespace RMSHOP.PL.Areas.Identity
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService) {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response= await _authenticationService.LoginAsync(request);
            //500
            if (response.UnexpectedErrorFlag)
            {
                return StatusCode(500, response);
            }

            //400
            if (!response.Success)
            {
                return BadRequest(response);
            }
            //200
            return Ok(response);

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request) {
          var response=await _authenticationService.RegisterAsync(request);
            //500
            if (response.UnexpectedErrorFlag) {
                return StatusCode(500, response);
            }
            //400
            if (!response.Success) {
                 return BadRequest(response);
            }
            //200
            return Ok(response);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId) {
            var response=await _authenticationService.ConfirmEmailAsync(token, userId);
            return Ok(new {message= response});
        }

        [HttpPost("SendCode")]
        public async Task<IActionResult> SendCode(SendCodeRequest request)
        {
            var response = await _authenticationService.SendCodeAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}

