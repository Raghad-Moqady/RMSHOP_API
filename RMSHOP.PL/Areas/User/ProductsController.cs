using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RMSHOP.BLL.Service.Products;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.DTO.Response.Products;
using RMSHOP.PL.Resources;

namespace RMSHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ProductsController(IProductService productService ,IStringLocalizer<SharedResource> localizer)
        {
            _productService = productService;
            _localizer = localizer;
        }

        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetAllProductsByCategoryForUser([FromRoute] int id,[FromQuery] string lang="en")
        {
            var response= await _productService.GetAllProductsByCategoryForUserAsync(id,lang);
            return Ok(new { message = _localizer["Success"].Value, products=response });
        }


        [HttpGet("")]
        public async Task<IActionResult> Index(
            [FromQuery] string lang = "en",
            [FromQuery] string? search=null,
            [FromQuery] int? categoryId=null,
            [FromQuery] int page=1, [FromQuery] int limit=3)
        {
            var response= await _productService.GetAllForUserAsync(lang, search ,page,limit, categoryId);
            return Ok(new {message= _localizer["Success"].Value ,response });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetails([FromRoute] int id,[FromQuery] string lang="en")
        {
            var response= await _productService.GetProductDetailsForUserAsync(id, lang);
            return Ok(new {message = _localizer["Success"].Value, product = response});
        }

       
        
        }
}
