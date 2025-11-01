using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl
{
	public class UserRepository : IUserRepository
	{
		private readonly StoreManagementDbContext _context;
		private readonly DbSet<User> _dbSet;

		public UserRepository(StoreManagementDbContext context)
		{
			_context = context;
			_dbSet = context.Set<User>();
		}

		public async Task<User> AddAsync(User entity)
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

		public async Task<IEnumerable<User>> GetAllAsync()
		{
			return await _dbSet.AsNoTracking().ToListAsync();
		}

		public async Task<User> GetByIdAsync(int id)
		{
			var user = await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
			return user ?? throw new KeyNotFoundException($"User with ID {id} not found");
		}

		public async Task<User?> GetByUsernameAsync(string username)
		{
			return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task<User> UpdateAsync(User entity)
		{
			var existing = await _dbSet.FindAsync(entity.UserId);
			if (existing == null)
				throw new KeyNotFoundException($"User with ID {entity.UserId} not found");

			existing.Username = entity.Username;
			existing.Password = entity.Password;
			existing.FullName = entity.FullName;
			existing.Role = entity.Role;

			await _context.SaveChangesAsync();
			return existing;
		}
	}
}


