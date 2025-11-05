using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly StoreManagementDbContext _context;
        private readonly DbSet<Customer> _dbSet;

        public CustomerRepository(StoreManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Customer>();
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _dbSet.FindAsync(id);
            if (customer == null)
                return false;

            _dbSet.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Orders)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var customer = await _dbSet
                .Include(c => c.Orders)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            return customer ?? throw new KeyNotFoundException($"Customer with ID {id} not found");
        }

        public async Task<Customer> UpdateAsync(Customer entity)
        {
            var existingCustomer = await _dbSet.FindAsync(entity.CustomerId);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with ID {entity.CustomerId} not found");

            // Update properties
            existingCustomer.Name = entity.Name;
            existingCustomer.Phone = entity.Phone;
            existingCustomer.Email = entity.Email;
            existingCustomer.Address = entity.Address;

            await _context.SaveChangesAsync();
            return existingCustomer;
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _dbSet
                .Include(c => c.Orders)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetCustomerByPhoneAsync(string phone)
        {
            return await _dbSet
                .Include(c => c.Orders)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Phone == phone);
        }

        public async Task<IEnumerable<Customer>> SearchCustomersByNameAsync(string name)
        {
            return await _dbSet
                .Include(c => c.Orders)
                .Where(c => c.Name.Contains(name))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
