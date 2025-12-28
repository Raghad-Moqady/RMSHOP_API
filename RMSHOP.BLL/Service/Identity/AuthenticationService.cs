using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RMSHOP.DAL.DTO.Request.Identity;
using RMSHOP.DAL.DTO.Response.Identity;
using RMSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Identity
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration
            ,IEmailSender emailSender , SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            //request >> Email & Password
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                //check Email/user exist or not
                if (user is null) {
                    //400
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Email !",
                    };
                }
                //here user is exist => what about Lockout?
                if (await _userManager.IsLockedOutAsync(user)) {
                    //400
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account is locked , try again later",
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
                if (result.IsLockedOut) {
                    //400
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account is locked due to multiple failed attempts",
                    };
                }else if (result.IsNotAllowed)
                {
                    //400
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Plz confirm your email",
                    };
                }
                if(!result.Succeeded)
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
                
                //Send Email & Confirm Email 
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var emailUrl = $"http://localhost:5073/api/auth/Account/ConfirmEmail?token={token}&userId={user.Id}";
                await _emailSender.SendEmailAsync(user.Email, "welcome to KidZone Store",
                    $@" <div style='text-align:center; font-family: Arial, sans-serif;'>
                    <h1 style='color:orange;'>Welcome {user.UserName}!</h1>
                    <p>Thank you for joining <strong>KidZone Store</strong>. We're happy to have you!</p>
                    <a href='{emailUrl}' style='display:inline-block; padding:10px 20px; background-color:orange; color:white; text-decoration:none; margin-top:20px;'>
                      Confirm Email
                    </a>
                    </div>");
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
            var roles= await _userManager.GetRolesAsync(user);
            //Payload: 
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,string.Join(',',roles))
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

        //ConfirmEmail step2
        public async Task<string> ConfirmEmailAsync(string token, string userId)
        {
            var user= await _userManager.FindByIdAsync(userId);
            if(user is null) return "This user is not registered or the account has been deleted";
            if(user.EmailConfirmed == true) return "This email is already confirmed";

            var result = await _userManager.ConfirmEmailAsync(user,token);
            if(!result.Succeeded) return "Email confirmation failed";
            return "Email confirmed successfully"; 
        }

        //Send Code step1
        public  async Task<SendCodeResponse> SendCodeAsync(SendCodeRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new SendCodeResponse()
                {
                    Success=false,
                    Message="Email Not Found"
                };
            }
            //create random code
            var random= new Random();
            var code = random.Next(1000,9999).ToString();

            //store the code in DB
            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);

            //send email with this code
            await _emailSender.SendEmailAsync(request.Email, "Reset Password",
                 $@"
                    <div style='text-align:center; font-family: Arial, sans-serif;'>
                      <h1 style='color:orange;'>Reset Your Password</h1>
                      <p>Hello {user.UserName},</p>
                      <p>Use the verification code below to reset your password for <strong>KidZone Store</strong>:</p>

                      <div style='font-size:24px; font-weight:bold; letter-spacing:4px; margin:20px 0; color:#333;'>
                        {code}
                      </div>

                      <p style='font-size:14px; color:gray;'>
                        This code will expire soon. If you did not request a password reset, please ignore this email.
                      </p>
                    </div>
                    ");

            return new SendCodeResponse()
            {
                Success = true,
                Message = "Code sent successfully to your email"
            };
        }


        //Reset Password step2
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            //Check Email
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                //400
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Email Not Found"
                };
            }
            // Check Code 
            //1.
            if (user.CodeResetPassword != request.Code)
            {
                //400
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "This reset code is invalid or has already been used. Please request a new code."
                };
            }
            //2.
            if(user.PasswordResetCodeExpiry< DateTime.UtcNow)
            {
                //400
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Sorry! Code Expired "
                };
            }
            
            // update user with new password
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user,token,request.NewPassword);

            if (!result.Succeeded)
            {
                //400
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Reset Password Failed ",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            //send email
            await _emailSender.SendEmailAsync(request.Email,"KidZone Store – Password Updated",
                $@"
                <div style='text-align:center; font-family: Arial, sans-serif;'>
                  <h1 style='color:orange;'>Password Updated Successfully</h1>
                  <p>Hello {user.UserName},</p>
                  <p>Your password for <strong>KidZone Store</strong> has been changed successfully.</p>
                  <p>If you did not make this change, please contact our support team immediately.</p>
                  <p style='margin-top:20px; font-size:12px; color:gray;'>
                    Thank you for using KidZone Store.
                  </p>
                </div>
                "
                );

            // Clear reset code after use to prevent reuse
            user.CodeResetPassword = null;
            await _userManager.UpdateAsync(user);

            //200
            return new ResetPasswordResponse()
            {
                Success = true,
                Message = "Password Reset Successfully",
            };

        }
    }
}
