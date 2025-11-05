using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl
{
	public class SupplierRepository : ISupplierRepository
	{
		private readonly StoreManagementDbContext _context;
		private readonly DbSet<Supplier> _dbSet;

		public SupplierRepository(StoreManagementDbContext context)
		{
			_context = context;
			_dbSet = context.Set<Supplier>();
		}

		public async Task<Supplier> AddAsync(Supplier entity)
		{
			await _dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
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

		public async Task<IEnumerable<Supplier>> GetAllAsync()
		{
			return await _dbSet
				.Include(s => s.Products)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Supplier> GetByIdAsync(int id)
		{
			var item = await _dbSet
				.Include(s => s.Products)
				.AsNoTracking()
				.FirstOrDefaultAsync(s => s.SupplierId == id);
			return item ?? throw new KeyNotFoundException($"Supplier with ID {id} not found");
		}

		public async Task<Supplier> UpdateAsync(Supplier entity)
		{
			var existing = await _dbSet.FindAsync(entity.SupplierId);
			if (existing == null)
				throw new KeyNotFoundException($"Supplier with ID {entity.SupplierId} not found");

			existing.Name = entity.Name;
			existing.Phone = entity.Phone;
			existing.Email = entity.Email;
			existing.Address = entity.Address;

			await _context.SaveChangesAsync();
			return existing;
		}
	}
}


