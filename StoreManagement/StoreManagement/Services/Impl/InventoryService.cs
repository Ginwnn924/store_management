using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Extensions;
using StoreManagement.Mapper;
using StoreManagement.Repository;

namespace StoreManagement.Services.Impl
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;

        private readonly IProductRepository _productRepository;

        public InventoryService(IInventoryRepository repository, IProductRepository productRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
        }
        public async Task<PagedResponse<InventoryResponse>> GetAllInventoryAsync(InventoryFilterRequest filter)
        {
            var query = _repository.GetQueryable();
            query = query.ApplyFilters(filter);

            int totalRecords = await query.CountAsync();
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var responseItems = items.Select(i => i.ToResponse()).ToList();

            var paged = new PagedResponse<InventoryResponse>(
                responseItems,
                totalRecords,
                filter.PageNumber,
                filter.PageSize
            );

            return paged;
        }
        public async Task<IEnumerable<InventoryResponse>> GetAllAsync()
        {
            var inventories = await _repository.GetAllAsync();
            return inventories.Select(i => i.ToResponse());
        }

        public async Task<InventoryResponse?> GetByIdAsync(int id)
        {
            var inventory = await _repository.GetByIdAsync(id);
            return inventory.ToResponse();
        }

        private async Task UpdateProductSupplier(int productId, int supplierId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product.SupplierId != supplierId)
            {
                product.SupplierId = supplierId;
                Console.WriteLine(supplierId);
                await _productRepository.UpdateAsync(product);
            }
        }

        public async Task<InventoryResponse> AddAsync(InventoryRequest request)
        {
            var allInventory = await _repository.GetAllAsync();
            var existing = allInventory.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existing != null)
            {
                existing.Quantity += request.Quantity;
                existing.UpdatedAt = DateTime.UtcNow;
                var updated = await _repository.UpdateAsync(existing);
                await UpdateProductSupplier(request.ProductId, request.SupplierId);
                return updated.ToResponse();
            }
            await UpdateProductSupplier(request.ProductId, request.SupplierId);

            var entity = request.ToEntity();
            var created = await _repository.AddAsync(entity);
            return created.ToResponse();
        }
        public async Task<InventoryResponse?> UpdateAsync(int id, InventoryRequest request)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Quantity = request.Quantity;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return updated.ToResponse();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<InventoryResponse>> SearchByProductNameAsync(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                return Enumerable.Empty<InventoryResponse>();

            var results = await _repository.SearchByProductNameAsync(productName.Trim());
            return results.Select(i => i.ToResponse());
        }
    }
}