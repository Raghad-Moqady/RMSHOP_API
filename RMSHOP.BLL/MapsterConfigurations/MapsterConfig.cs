using Mapster;
using RMSHOP.DAL.DTO.Response.Categories;
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
        }
    }
}
