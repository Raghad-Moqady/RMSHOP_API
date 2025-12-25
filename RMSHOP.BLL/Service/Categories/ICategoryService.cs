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
        List<CategoryResponse> GetAllCategories ();

        //Create
        CategoryResponse CreateCategory(CategoryRequest category);

        //Delete
        Task<BaseResponse> DeleteCategoryAsync(int id);
    }
}
