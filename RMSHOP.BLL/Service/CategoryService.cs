using Mapster;
using RMSHOP.DAL.DTO.Request;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.Models;
using RMSHOP.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service
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

    }
}
