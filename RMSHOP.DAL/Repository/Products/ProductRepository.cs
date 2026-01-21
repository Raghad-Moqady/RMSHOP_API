using Microsoft.EntityFrameworkCore;
using RMSHOP.DAL.Data;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.Models.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product?> CreateProductAsync(Product product)
        {
             await _context.AddAsync(product);
             await _context.SaveChangesAsync();
             return await _context.Products.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == product.Id);
        }

        public async Task<Product?> FindProductById(int id)
        {
            return await _context.Products
                .Include(p => p.Translations)
                .Include(p => p.Category.Translations)
                .Include(p=>p.SubImages)
                .FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p=>p.User).Include(p=>p.Translations).Include(p=>p.SubImages).ToListAsync();
        }
 
        public async Task<List<Product>> GetAllProductsByCategoryForUserAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p=> p.Translations)
                .Include(p=>p.Category.Translations).ToListAsync();
        }
        public async Task<List<Product>> GetAllForUserAsync()
        {
            //return await _context.Products.Include(p => p.Translations).Include(p => p.Category.Translations).Include(p=>p.Category.User).ToListAsync();
            return await _context.Products
                .Include(p => p.Translations)
                .Include(p => p.Category.Translations).ToListAsync();
        }
        public IQueryable<Product> Query()
        {
            return  _context.Products
              .Include(p => p.Translations)
              .Include(p => p.Category.Translations).AsQueryable();
        }

        //public async Task<bool> DecreaseProductQuantityAsync(int productId, int quantity)
        //{
        //    var product= await _context.Products.FindAsync(productId);

        //    product.Quantity-=quantity;
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> DecreaseProductsQuantityAsync(List<(int productId, int quantity)> productsToDecreaseQuantity)
        {
            var productIds= productsToDecreaseQuantity.Select(p=>p.productId).ToList();
            var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

            foreach (var product in products)
            {
                var productToDecreaseQuantity = productsToDecreaseQuantity.FirstOrDefault(p=>p.productId==product.Id);
                if (product.Quantity < productToDecreaseQuantity.quantity)
                {
                    return false;
                }
                product.Quantity -= productToDecreaseQuantity.quantity;
               
            }
            await _context.SaveChangesAsync();
            return true;

        }

    }
}
