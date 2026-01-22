using Mapster;
using Microsoft.EntityFrameworkCore;
using RMSHOP.DAL.DTO.Request.Products;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.DTO.Response.Categories;
using RMSHOP.DAL.DTO.Response.Products;
using RMSHOP.DAL.Models.product;
using RMSHOP.DAL.Repository.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository, IFileService fileService)
        {
            _productRepository = productRepository;
            _fileService = fileService;
        }
        public async Task<ProductResponse> CreateProductAsync(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            if(request.MainImage != null)
            {
                var fileName= await _fileService.UploadAsync(request.MainImage);
                product.MainImage = fileName;
            }

            if(request.SubImages != null)
            {
                product.SubImages = new List<ProductSubImage>();
                foreach(var image in request.SubImages)
                {
                    var fileName = await _fileService.UploadAsync(image);
                    product.SubImages.Add(new ProductSubImage
                    {
                        ImageName = fileName
                    });
                }
            }
            var result= await _productRepository.CreateProductAsync(product);
            return result.Adapt<ProductResponse>();
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
             var response= await _productRepository.GetAllAsync();
             return response.Adapt<List<ProductResponse>>();
        }


        public async Task<List<ProductUserResponse>> GetAllProductsByCategoryForUserAsync(int categoryId, string lang)
        {
            var filteredProducts = await _productRepository.GetAllProductsByCategoryForUserAsync(categoryId);
            return filteredProducts.BuildAdapter().AddParameters("lang",lang).AdaptToType<List<ProductUserResponse>>();
        }
        public async Task<PaginatedResponse<ProductUserResponse>> GetAllForUserAsync(
            string lang,
            string search,
            int page,
            int limit,
            int? categoryId)
        {
            //old code without pagination & sort &... so on :
            //var products = await _productRepository.GetAllForUserAsync();
            //return products.BuildAdapter().AddParameters("lang",lang).AdaptToType<List<ProductUserResponse>>();

            var query = _productRepository.Query();
            //0*.Search
            if(search is not null)
            {
                query = query.Where(p => p.Translations.Any(t => t.Language == lang && (t.Name.Contains(search)||t.Description.Contains(search))));
            };
            //filter products by category id
            if (categoryId is not null)
            {
                query= query.Where(p=>p.CategoryId== categoryId);
            }
             

            //1*.total Count
            var totalCount =await query.CountAsync();
            //2*. Pagination
            query= query.Skip((page - 1) * limit).Take(limit);
             
            var response= query.BuildAdapter().AddParameters("lang",lang).AdaptToType<List<ProductUserResponse>>();

            return new PaginatedResponse<ProductUserResponse>()
            {
                 TotalCount = totalCount,
                 Limit = limit,
                 Page = page,
                 Data= response
            };

        }

        public async Task<ProductDetailsForUserResponse> GetProductDetailsForUserAsync(int id, string lang)
        {
            var product = await _productRepository.FindProductById(id);
            return product.BuildAdapter().AddParameters("lang",lang).AdaptToType<ProductDetailsForUserResponse>();
        }
    }
}
