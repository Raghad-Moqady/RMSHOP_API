using RMSHOP.DAL.DTO.Request;
using RMSHOP.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Identity
{
    public interface IAuthenticationService
    {
        //Register
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);

        //LogIn
        Task<LoginResponse> LoginAsync(LoginRequest request);

        //Confirm Email
        Task<string> ConfirmEmailAsync(string token, string userId);

        //Send Code
        Task<SendCodeResponse> SendCodeAsync(SendCodeRequest request);
    }
}
