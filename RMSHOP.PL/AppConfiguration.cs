using RMSHOP.BLL.Service;
using RMSHOP.BLL.Service.Categories;
using RMSHOP.BLL.Service.Identity;
using RMSHOP.DAL.Repository.Categories;
using RMSHOP.DAL.Utils;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace RMSHOP.PL
{
    public static class AppConfiguration
    {
        public static void Config(IServiceCollection Services)
        {
            // ICategoeryRepository dependency Injection
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            // ICategoryService dependency Injection
            Services.AddScoped<ICategoryService, CategoryService>();
            //SeedData
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();
            //Authentication
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            //To send emails
            Services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
