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

        public async Task<List<CategoryResponse>> GetAllCategoriesAsync(string lang)
        {
            var categories =await _categoryRepository.GetAllCategoriesAsync();

            foreach (var category in categories)
            {
                var result = category.Translations.Where(t=>t.Language==lang).ToList();
                category.Translations = result;
            }

            return categories.Adapt<List<CategoryResponse>>();
        }
        public async Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            var response =await _categoryRepository.CreateCategoryAsync(category);
            return response.Adapt<CategoryResponse>();
         }

        public async Task<BaseResponse> UpdateCategoryPutAsync(int id, CategoryRequest request)
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
                        Message = "Category Not Found",
                    };
                }
                category.Translations = request.Translations.Adapt<List<CategoryTranslation>>();
                await _categoryRepository.UpdateCategoryAsync(category);

                //200
                return new BaseResponse()
                {
                    Success = true,
                    Message = "Category Updated Successfully"
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
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<BaseResponse> UpdateCategoryPatchAsync(int id, CategoryRequest request)
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
                        Message = "Category Not Found",
                    };
                }
                 
                foreach (var translation in request.Translations)
                    {
                        var existing = category.Translations.FirstOrDefault(t=> t.Language== translation.Language);
                        if(existing is not null)
                        {
                            if(existing.Name!= translation.Name)
                            {
                                existing.Name = translation.Name;
                            }
                        }
                        else
                        {
                            //create new translation
                            //category.Translations.Add(new CategoryTranslation()
                            //{
                            //    Name = translation.Name,
                            //    Language = translation.Language,
                            //});
                            //or return error
                            //400
                            return new BaseResponse()
                            {
                                Success = false,
                                Message = $"language :{translation.Language} not suported!",
                            }; 
                        }
                    } 

                await _categoryRepository.UpdateCategoryAsync(category); 
                //200
                return new BaseResponse()
                {
                    Success = true,
                    Message = "Category Updated Successfully"
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
                    Errors = new List<string> { ex.Message }
                };
            }
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

        public async Task<BaseResponse> ToggleStatusAsync(int id)
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
                        Message = "Category Not Found",
                    };
                }
                category.Status = category.Status == Status.Active ? Status.InActive : Status.Active;
                await _categoryRepository.UpdateCategoryAsync(category);

                //200
                return new BaseResponse()
                {
                    Success = true,
                    Message = "Category Status Toggled Successfully"
                };

            }
            catch(Exception ex)
            {
                //500
                return new BaseResponse()
                {
                    Success = false,
                    UnexpectedErrorFlag = true,
                    Message = "Unexpected Error!",
                    Errors = new List<string> { ex.Message }
                };

            }
        }
    }
}
