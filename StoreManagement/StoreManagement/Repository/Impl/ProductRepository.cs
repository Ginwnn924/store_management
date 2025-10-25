using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreManagementDbContext _context;
        private readonly DbSet<Product> _dbSet;

        public ProductRepository(StoreManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Product>();
        }

        public async Task<Product> AddAsync(Product entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _dbSet.FindAsync(id);
            if (product == null)
                return false;

            _dbSet.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Inventory)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Inventory)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id);

            return product ?? throw new KeyNotFoundException($"Product with ID {id} not found");
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            var existingProduct = await _dbSet.FindAsync(entity.ProductId);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {entity.ProductId} not found");

            // Update properties
            existingProduct.ProductName = entity.ProductName;
            existingProduct.CategoryId = entity.CategoryId;
            existingProduct.SupplierId = entity.SupplierId;
            existingProduct.Barcode = entity.Barcode;
            existingProduct.Price = entity.Price;
            existingProduct.Unit = entity.Unit;
            existingProduct.ImageUrl = entity.ImageUrl;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Inventory)
                .Where(p => p.CategoryId == categoryId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Inventory)
                .Where(p => p.SupplierId == supplierId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetProductByBarcodeAsync(string barcode)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Inventory)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Barcode == barcode);
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameAsync(string name)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Inventory)
                .Where(p => p.ProductName.Contains(name))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}