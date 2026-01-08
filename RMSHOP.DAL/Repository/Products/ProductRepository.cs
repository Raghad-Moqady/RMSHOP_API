using Microsoft.EntityFrameworkCore;
using RMSHOP.DAL.Data;
using RMSHOP.DAL.Models;
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
        
    }
}
