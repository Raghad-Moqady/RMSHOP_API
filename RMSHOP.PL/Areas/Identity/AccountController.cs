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


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request) {
          var response=await _authenticationService.RegisterAsync(request);
            //400
            if (!response.Success) {
                 return BadRequest(response);
            }
            //500
            if (response.UnexpectedErrorFlag) {
                return StatusCode(500, response);
            }
            //200
            return Ok(response);
        }
    }
}

