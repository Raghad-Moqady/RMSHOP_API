using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RMSHOP.BLL.Service.Categories;
using RMSHOP.PL.Resources;
using System.Threading.Tasks;

namespace RMSHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService categoryService , IStringLocalizer<SharedResource> localizer) {
            _categoryService = categoryService;
            _localizer = localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] string lang ="en") {
            var response =await _categoryService.GetAllCategoriesForUsersAsync(lang);
            return Ok(new { message = _localizer["Success"].Value,categories=response});
        }
    }
}
