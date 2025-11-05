
using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly StoreManagementDbContext _context;

        public CategoryRepository(StoreManagementDbContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetQueryable()
        {
            return _context.Categories.AsQueryable();
        }

        public async Task<Category> AddAsync(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == id);
        }

       

        public async Task<Category> UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<Category>> SearchByNameAsync(string categoryName)
        {
            return await _context.Categories
                .Where(c => c.CategoryName.ToLower().Contains(categoryName.ToLower()))
                .ToListAsync();
        }
    }
}
