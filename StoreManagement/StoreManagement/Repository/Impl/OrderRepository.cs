using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl
{
    public class OrderRepository : IOrderRepository
    {

        private readonly StoreManagementDbContext _context;
        private readonly DbSet<Order> _dbSet;

        public OrderRepository(StoreManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Order>();
        }

        public async Task<Order> AddAsync(Order entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbSet
             .Include(o => o.User)
             .Include(o => o.Customer)
             .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
             .AsNoTracking()
             .ToListAsync();
        }

        public Task<Order> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateAsync(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
