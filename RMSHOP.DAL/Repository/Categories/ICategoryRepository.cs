using RMSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Repository.Categories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category Create(Category category);
        Task<Category?> FindByIdAsync(int id);
        Task DeleteCategoryAsync(Category category);
         
    }
}
