using Azure.Core;
using RMSHOP.DAL.DTO.Request.categories;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.DTO.Response.Categories;
using RMSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Categories
{
    public interface ICategoryService
    {
        //Get All For User : 
        Task<List<CategoryResponseForUser>> GetAllCategoriesForUsersAsync(string lang);

        //Get All For Admin :
        Task<List<CategoryResponseForAdmin>>GetAllCategoriesForAdminAsync();

        //Create :Admin
        Task<CategoryResponseForAdmin> CreateCategoryAsync(CategoryRequest request);

        //Delete: Admin
        Task<BaseResponse> DeleteCategoryAsync(int id);

        //Update (Put way) :Admin
        Task<BaseResponse> UpdateCategoryPutAsync(int id,CategoryRequest request);

        //Update (Patch way) :Admin
        Task<BaseResponse> UpdateCategoryPatchAsync(int id, CategoryRequest request);

        //Toggle Status :Admin
        Task<BaseResponse> ToggleStatusAsync(int id);

    }
}
