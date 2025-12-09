using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using StoreManagement.Data;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly StoreManagementDbContext _context;
        private readonly DbSet<Promotion> _dbSet;

        public PromotionRepository(StoreManagementDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Promotion>();
        }
        public async Task<IEnumerable<Promotion>> GetAllPromotion(long min_order_amount)
        {
            return await _dbSet
               .Where(p => !p.IsDeleted)
               .Where(p => p.MinOrderAmount <= min_order_amount)
               .Where(p => p.UsedCount < p.UsageLimit || p.UsedCount == 0)
               .Where(p => p.Status == "active")
               //.AsNoTracking()
               .ToListAsync();
        }

        public async Task<Promotion> UpdateUsedCountAsync(int? id)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            // Update properties
            existingEntity.UsedCount++;

            await _context.SaveChangesAsync();
            return existingEntity;
        }

    }
}
