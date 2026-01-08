using RMSHOP.DAL.Models;
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
        Task<Product?> FindProductById(int id);
    }
}
