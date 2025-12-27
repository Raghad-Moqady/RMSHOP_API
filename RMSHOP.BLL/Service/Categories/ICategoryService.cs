using Azure.Core;
using RMSHOP.DAL.DTO.Request;
using RMSHOP.DAL.DTO.Response;
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
        //Get All
        Task<List<CategoryResponse>> GetAllCategoriesAsync(string lang);

        //Create
        Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request);

        //Delete
        Task<BaseResponse> DeleteCategoryAsync(int id);

        //Update (Put way)
        Task<BaseResponse> UpdateCategoryPutAsync(int id,CategoryRequest request);

        //Update (Patch way)
        Task<BaseResponse> UpdateCategoryPatchAsync(int id, CategoryRequest request);

        //Toggle Status
        Task<BaseResponse> ToggleStatusAsync(int id);

    }
}
