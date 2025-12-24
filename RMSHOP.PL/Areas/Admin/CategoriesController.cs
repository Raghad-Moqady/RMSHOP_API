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
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService categoryService , IStringLocalizer<SharedResource> localizer) {
            _categoryService = categoryService;
            _localizer = localizer;
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var createdBy= User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Console.WriteLine("user id");
            //Console.WriteLine(createdBy);

            var response= _categoryService.CreateCategory(request,createdBy);
            return Ok(new {message= _localizer["Success"].Value ,category= response});
        }
    }
}
