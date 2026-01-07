using Mapster;
using RMSHOP.DAL.DTO.Response.Categories;
using RMSHOP.DAL.DTO.Response.Products;
using RMSHOP.DAL.Models;
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

        }

    }
}
