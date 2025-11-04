using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl
{
	public class PaymentRepository : IPaymentRepository
	{
		private readonly StoreManagementDbContext _context;
		private readonly DbSet<Payment> _dbSet;

		public PaymentRepository(StoreManagementDbContext context)
		{
			_context = context;
			_dbSet = context.Set<Payment>();
		}

		public async Task<Payment> AddAsync(Payment entity)
		{
			await _dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            await _dbSet.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeleteAsync(int id)
		{
			var found = await _dbSet.FindAsync(id);
			if (found == null)
				return false;
			_dbSet.Remove(found);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<IEnumerable<Payment>> GetAllAsync()
		{
			return await _dbSet
				.Include(p => p.Order)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Payment> GetByIdAsync(int id)
		{
			var item = await _dbSet
				.Include(p => p.Order)
				.AsNoTracking()
				.FirstOrDefaultAsync(p => p.PaymentId == id);
			return item ?? throw new KeyNotFoundException($"Payment with ID {id} not found");
		}

		public async Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId)
		{
			return await _dbSet
				.Include(p => p.Order)
				.Where(p => p.OrderId == orderId)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Payment> UpdateAsync(Payment entity)
		{
			var existing = await _dbSet.FindAsync(entity.PaymentId);
			if (existing == null)
				throw new KeyNotFoundException($"Payment with ID {entity.PaymentId} not found");

			existing.Amount = entity.Amount;
			existing.PaymentMethod = entity.PaymentMethod;
			existing.PaymentDate = entity.PaymentDate;

			await _context.SaveChangesAsync();
			return existing;
		}

        Task<List<Payment>> IPaymentRepository.GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }
    }
}


