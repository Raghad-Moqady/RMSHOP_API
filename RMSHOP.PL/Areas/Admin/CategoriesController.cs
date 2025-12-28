using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RMSHOP.BLL.Service.Categories;
using RMSHOP.DAL.DTO.Request.categories;
using RMSHOP.PL.Resources;
using System.Security.Claims;
using System.Threading.Tasks;

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


        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _categoryService.GetAllCategoriesForAdminAsync();
            return Ok(new { message = _localizer["Success"].Value, categories = response });
        }


        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            var response =await _categoryService.CreateCategoryAsync(request);
            return Ok(new { message = _localizer["Success"].Value, category = response });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryPut([FromRoute] int id, [FromBody] CategoryRequest request)
        {
            var response = await _categoryService.UpdateCategoryPutAsync(id, request);
            //500
            if (response.UnexpectedErrorFlag)
            {
                return StatusCode(500, response);
            }
            //404
            else if (!response.Success)
            {
                if (response.Message.Contains("Not Found"))
                {
                    return NotFound(response);
                }
            }
            //200
            return Ok(response);
         }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategoryPatch([FromRoute] int id, [FromBody] CategoryRequest request)
        {
            var response = await _categoryService.UpdateCategoryPatchAsync(id, request);
            //500
            if (response.UnexpectedErrorFlag)
            {
                return StatusCode(500, response);
            }
            else if (!response.Success)
            {
                if (response.Message.Contains("Not Found"))
                {
                    //404
                    return NotFound(response);
                }else
                {
                    //400
                    return BadRequest(response);
                }
            }
            //200
            return Ok(response);
        }


        [HttpPatch("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id)
        {
            var response = await _categoryService.ToggleStatusAsync(id);
            //500
            if (response.UnexpectedErrorFlag)
            {
                return StatusCode(500, response);
            }
            //404
            else if (!response.Success)
            {
                if (response.Message.Contains("Not Found"))
                {
                    return NotFound(response);
                }
            }
            //200
            return Ok(response);
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
