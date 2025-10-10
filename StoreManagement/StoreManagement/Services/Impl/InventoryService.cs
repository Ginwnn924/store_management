using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Mapper;
using StoreManagement.Mapper;
using StoreManagement.Models;
using StoreManagement.Repository;

namespace StoreManagement.Services.Impl
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;

        public InventoryService(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<InventoryResponse>> GetAllAsync()
        {
            var inventories = await _repository.GetAllAsync();
            return inventories.Select(i => i.ToResponse());
        }

        public async Task<InventoryResponse?> GetByIdAsync(int id)
        {
            var inventory = await _repository.GetByIdAsync(id);
            return inventory?.ToResponse();
        }

      
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

       
    }
}