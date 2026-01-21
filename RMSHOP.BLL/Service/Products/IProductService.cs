using RMSHOP.DAL.DTO.Request.Products;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.DTO.Response.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Products
{
    public interface IProductService
    {
        //for Admin 
        Task<ProductResponse> CreateProductAsync(ProductRequest request);
        Task<List<ProductResponse>> GetAllAsync();
        //for user
        Task<List<ProductUserResponse>> GetAllProductsByCategoryForUserAsync(int categoryId, string lang);
        Task<PaginatedResponse<ProductUserResponse>> GetAllForUserAsync(string lang, string search, int page, int limit);
        Task<ProductDetailsForUserResponse> GetProductDetailsForUserAsync(int id, string lang);

    }
}
