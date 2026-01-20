using Mapster;
using RMSHOP.DAL.DTO.Response.Cart;
using RMSHOP.DAL.DTO.Response.Categories;
using RMSHOP.DAL.DTO.Response.Products;
using RMSHOP.DAL.Models.cart;
using RMSHOP.DAL.Models.category;
using RMSHOP.DAL.Models.order;
using RMSHOP.DAL.Models.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.MapsterConfigurations
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister() 
        {
            //TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
            //      .Map(dest => dest.CategoryId, source => source.Id).TwoWays();

            TypeAdapterConfig<Category, CategoryResponseForAdmin>.NewConfig()
                .Map(dest => dest.CreatedByUserName, source => source.User.UserName);


            TypeAdapterConfig<Category, CategoryResponseForUser>.NewConfig()
                .Map(dest => dest.Name, source=> source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
               .Map(dest => dest.CreatedBy, source => source.User.UserName)
               .Map(dest => dest.MainImage, source => $"https://localhost:7281/images/{source.MainImage}")
               .Map(dest=> dest.SubImages, source => source.SubImages.Select(s=> $"https://localhost:7281/images/{s.ImageName}").ToList());
        
           TypeAdapterConfig<Product,ProductUserResponse>.NewConfig()
                .Map(dest=> dest.Name, source=>source.Translations
                .Where(t=> t.Language== MapContext.Current.Parameters["lang"].ToString())
                .Select(t=>t.Name).FirstOrDefault())

                .Map(dest=> dest.CategoryName, source=> source.Category.Translations
                .Where(t=>t.Language== MapContext.Current.Parameters["lang"].ToString())
                .Select(t=>t.Name).FirstOrDefault())
                
                .Map(dest=> dest.MainImage, source=> $"https://localhost:7281/images/{source.MainImage}");

            TypeAdapterConfig<Product, ProductDetailsForUserResponse>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t =>t.Name).FirstOrDefault())
                
                .Map(dest=> dest.Description, source=> source.Translations
                .Where(t=>t.Language== MapContext.Current.Parameters["lang"].ToString())
                .Select(t=>t.Description).FirstOrDefault())

                .Map(dest=>dest.CategoryName,source=>source.Category.Translations
                .Where(t=>t.Language== MapContext.Current.Parameters["lang"].ToString())
                .Select(t=>t.Name).FirstOrDefault())
                
                .Map(dest=>dest.MainImage, source=> $"https://localhost:7281/images/{source.MainImage}")
                .Map(dest=> dest.SubImages, source => source.SubImages.Select(s=> $"https://localhost:7281/images/{s.ImageName}"))
                ;

            TypeAdapterConfig<Cart, CartProductResponse>.NewConfig()
                .Map(dest => dest.ProductName, source => source.Product.Translations
                .FirstOrDefault(t=>t.Language== MapContext.Current.Parameters["lang"].ToString()).Name)
                .Map(dest=>dest.Price, source=>source.Product.Price)
                .Map(dest=>dest.MainImage, source=> $"https://localhost:7281/images/{source.Product.MainImage}")
                .Map(dest=> dest.ProductCount, source=>source.Count)
                .Map(dest=>dest.CategoryName, source=>source.Product.Category.Translations
                .FirstOrDefault(t => t.Language == MapContext.Current.Parameters["lang"].ToString()).Name)
                ;

            //TypeAdapterConfig<Cart, OrderItem>.NewConfig()
            //     .Map(dest=> dest.OrderId, source=> MapContext.Current.Parameters["orderId"].ToString())
            //     .Map(dest => dest.ProductId, source => source.ProductId)
            //     .Map(dest=> dest.Quantity, source=> source.Count)
            //     .Map(dest => dest.UnitPrice, source => source.Product.Price)
            //     .Map(dest => dest.TotalPrice, source => source.Count * source.Product.Price);

        }

    }
}
