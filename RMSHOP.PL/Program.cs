
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RMSHOP.BLL;
using RMSHOP.BLL.MapsterConfigurations;
using RMSHOP.DAL.Data;
using RMSHOP.DAL.Models;
using RMSHOP.DAL.Utils;
using RMSHOP.PL.Middleware;
using Stripe;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //to solve CORS  policy issue
            var MyAllowSpecificOrigins = "_myAllowOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  { 
                                      policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                  });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //Localization
            builder.Services.AddLocalization(options => options.ResourcesPath = "");

            //way1 to connect with database using dependency Injection way
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(builder.Configuration["ConnectionStrings: DefaultConnection"]));

            //way2 to connect with database using dependency Injection way
            //  builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]));

            //way3 to connect with database using dependency Injection way
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
             

            // Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // These validations are performed before adding to the database
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                // Required Symbols
                options.Password.RequireNonAlphanumeric = true;
                //min= 8 char
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //jwt: check comming token
            builder.Services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //Cancel extra time
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
                };
            });
 
            const string defaultCulture = "en";

            var supportedCultures = new[]
            {
                new CultureInfo(defaultCulture),
                new CultureInfo("ar")
            };

            builder.Services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                //to change from ?Culture to ?Lang for example 
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider
                {
                    QueryStringKey = "lang"
                });
                 
            });

            //Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //App Configurations (contain all Services)
            AppConfiguration.Config(builder.Services);

            //Mapster Configurations
            MapsterConfig.MapsterConfigRegister();

            //audit
            //builder.Services.AddHttpContextAccessor();

            // Configure Stripe settings
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            var app = builder.Build();

            //Localization
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //to solve CORS  policy issue
            app.UseCors(MyAllowSpecificOrigins);

            // global exception handling middleware(way 1:old)
            //app.UseMiddleware<GlobalExceptionHandling>();

            // global exception handler(way 2:new)
            app.UseExceptionHandler();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // add SeedData
            using (var scope =app.Services.CreateScope()) 
            {
                var services= scope.ServiceProvider;
                //create 2 objects
                var seeders = services.GetServices<ISeedData>();
                foreach (var seeder in seeders) 
                {
                    await seeder.DataSeed();
                }
            }

            app.MapControllers();

            //test custom middleware
            //app.Use(async(context, next) =>
            //{
            //    Console.WriteLine("Request");
            //    await next();
            //    Console.WriteLine("Response");
            //});

            //app.Run(async (context) =>
            //{
            //    Console.WriteLine("Run");
            //});
            app.UseMiddleware<CustomMiddleware>();
            app.Run();
        }
    }
}
