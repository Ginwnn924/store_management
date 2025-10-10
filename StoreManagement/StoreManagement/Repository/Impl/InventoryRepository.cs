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

       

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Inventories.FindAsync(id);
            if (entity == null)
                return false;

            _context.Inventories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<Inventory> AddAsync(Inventory entity)
        {
            throw new NotImplementedException();
        }

        public Task<Inventory> UpdateAsync(Inventory entity)
        {
            throw new NotImplementedException();
        }
    }
}
