using Microsoft.EntityFrameworkCore;
using RMSHOP.DAL.Data;
using RMSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) {
            _context = context;
        }
        public List<Category> GetAll()
        {
            var categories = _context.Categories.Include(c => c.Translations).ToList();
            return categories;
        }

        public Category Create(Category category)
        {
            _context.Add(category);
            _context.SaveChanges();
            return category;
        }

    }
}
