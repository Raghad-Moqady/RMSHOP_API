using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.Models.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository.Products
{
    public interface IProductRepository
    {
        Task<Product?> CreateProductAsync(Product product);
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetAllProductsByCategoryForUserAsync(int categoryId);
        Task<List<Product>> GetAllForUserAsync();
        IQueryable<Product> Query();
        Task<Product?> FindProductById(int id);
        Task<bool> DecreaseProductsQuantityAsync(List<(int productId, int quantity)> productsToDecreaseQuantity);
    }
}
