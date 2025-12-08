using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RMSHOP.DAL.DTO.Request;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Identity
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            //request >> Email & Password
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null) {
                    //400
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Email !",
                    };
                }
                var result = await _userManager.CheckPasswordAsync(user, request.Password);
                if(!result)
                {
                    //400
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Password !",
                    };
                }
                //200
                return new LoginResponse()
                {
                    Success = true,
                    Message = "Login Successfully",
                    AccessToken = await GenerateAccessToken(user),
                };
            }
            catch (Exception ex) {
                //500
                return new LoginResponse()
                {
                    Success = false,
                    UnexpectedErrorFlag = true,
                    Message = "An Unexpected error !",
                    Errors = new List<string> { ex.Message }
                };
            
            }
        }


        //Domain Model is >> ApplicationUser:IdentityUser
        //DTO >> RegisterRequest
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var user = request.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    //400
                    return new RegisterResponse()
                    {
                        Success = false,
                        Message = "User Creation Failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }
                await _userManager.AddToRoleAsync(user, "User");
                //200
                return new RegisterResponse()
                {
                    Success = true,
                    Message = "Success"
                };


            }
            catch (Exception ex) {
                //500
                return new RegisterResponse()
                {
                    Success = false,
                    UnexpectedErrorFlag=true,
                    Message = "An Unexpected error !",
                    Errors = new List<string> { ex.Message }
                };
            }
        }


        //Generate Token (jwt)
        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            //Payload: 
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            //Generate Token :
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
