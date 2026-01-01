using Microsoft.EntityFrameworkCore;
using RMSHOP.DAL.Data;
using RMSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository.Categories
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.Include(c => c.Translations).Include(c=>c.User).ToListAsync();
            return categories;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return _context.Categories.Include(c=>c.User).FirstOrDefault(c=>c.Id==category.Id);
        }

        public async Task<Category?> FindByIdAsync(int id)
        {
            return await _context.Categories.Include(c=> c.Translations)
                   .FirstOrDefaultAsync(c => c.Id == id);
        } 

        //hard delete
        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }
 
    }
}
