using Microsoft.AspNetCore.Identity.UI.Services;
using RMSHOP.BLL.Service;
using RMSHOP.BLL.Service.Carts;
using RMSHOP.BLL.Service.Categories;
using RMSHOP.BLL.Service.Checkout;
using RMSHOP.BLL.Service.Identity;
using RMSHOP.BLL.Service.Products;
using RMSHOP.BLL.Service.Token;
using RMSHOP.DAL.Repository.Carts;
using RMSHOP.DAL.Repository.Categories;
using RMSHOP.DAL.Repository.OrderItems;
using RMSHOP.DAL.Repository.Orders;
using RMSHOP.DAL.Repository.Products;
using RMSHOP.DAL.Utils;

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

            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<IFileService, FileService>();

            //Cart
            Services.AddScoped<ICartService, CartService>();
            Services.AddScoped<ICartRepository, CartRepository>();

            //Checkout
            Services.AddScoped<ICheckoutService, CheckoutService>();

            //Token
            Services.AddScoped<ITokenService, TokenService>();

            //Order
            Services.AddScoped<IOrderRepository, OrderRepository>();
            Services.AddScoped<IOrderItemRepository,OrderItemRepository>();



        }
    }
}
