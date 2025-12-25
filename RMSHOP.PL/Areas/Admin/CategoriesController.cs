using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RMSHOP.BLL.Service.Categories;
using RMSHOP.DAL.DTO.Request;
using RMSHOP.PL.Resources;
using System.Security.Claims;

namespace RMSHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService categoryService, IStringLocalizer<SharedResource> localizer) {
            _categoryService = categoryService;
            _localizer = localizer;
        }

        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var response = _categoryService.CreateCategory(request);
            return Ok(new { message = _localizer["Success"].Value, category = response });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);
            //500
            if (response.UnexpectedErrorFlag)
            {
                return StatusCode(500, response);
            } 
            //404
            else if (!response.Success)
            {
                if(response.Message.Contains("Not Found"))
                {
                  return NotFound(response);
                }
            }
            //200
            return Ok(response);
        }

    }
}
