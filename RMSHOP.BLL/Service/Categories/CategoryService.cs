using Mapster;
using RMSHOP.DAL.DTO.Request;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.Models;
using RMSHOP.DAL.Repository.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Categories
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        public List<CategoryResponse> GetAllCategories()
        {
            var categories = _categoryRepository.GetAll();
            return categories.Adapt<List<CategoryResponse>>();
         }
        public CategoryResponse CreateCategory(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            var response = _categoryRepository.Create(category);
            return response.Adapt<CategoryResponse>();
         }

        public async Task<BaseResponse> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    //404
                    return new BaseResponse()
                    {
                        Success = false,
                        Message= "Category Not Found",
                    };
                }
                await _categoryRepository.DeleteCategoryAsync(category);
                //200
                return new BaseResponse()
                {
                    Success = true,
                    Message = "Category Deleted Successfully"
                };
            }
            catch (Exception ex)
            {
                //500
                return new BaseResponse()
                {
                    Success = false,
                    UnexpectedErrorFlag = true,
                    Message = "Unexpected Error!",
                    Errors= new List<string> { ex.Message }
                };

            } 
        }
    }
}
