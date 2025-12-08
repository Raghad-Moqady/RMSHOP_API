using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RMSHOP.BLL.Service;
using RMSHOP.DAL.Data;
using RMSHOP.DAL.DTO.Request;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.Models;
using RMSHOP.PL.Resources;

namespace RMSHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICategoryService _categoryService;

        public CategoriesController(IStringLocalizer<SharedResource> localizer , ICategoryService categoryService)
        {
            _localizer = localizer;
            _categoryService = categoryService;
        }
         

        [HttpGet("")]
        public IActionResult Index() {
            var response = _categoryService.GetAllCategories();
            return Ok(new {message= _localizer["Success"].Value, response });
        }

        [HttpPost("")]
        public IActionResult Create(CategoryRequest request) {
            var response= _categoryService.CreateCategory(request);
            return Ok(new { message = _localizer["Success"].Value, response });
        }
    }
}
