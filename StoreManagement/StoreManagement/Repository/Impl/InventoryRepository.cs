using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly StoreManagementDbContext _context;

        public InventoryRepository(StoreManagementDbContext context)
        {
            _context = context;
        }
        public IQueryable<Inventory> GetQueryable()
        {
            return _context.Inventories
                .Include(i => i.Product) 
                .AsQueryable();
        }
       
        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _context.Inventories
                                 .Include(i => i.Product)
                                 .ToListAsync();
        }

        public async Task<Inventory?> GetByIdAsync(int id)
        {
            return await _context.Inventories
                                 .Include(i => i.Product)
                                 .FirstOrDefaultAsync(i => i.InventoryId == id);
        }
        public async Task<Inventory> AddAsync(Inventory entity)
        {
         _context.Inventories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Inventory> UpdateAsync(Inventory entity)
        {
            _context.Inventories.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Inventories.FindAsync(id);
            if (entity == null)
                return false;

            _context.Inventories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Inventory>> SearchByProductNameAsync(string productName)
        {
            return await _context.Inventories
                .Include(i => i.Product)
                .Where(i => i.Product.ProductName.ToLower().Contains(productName.ToLower()))
                .ToListAsync();
        }

    }
}
