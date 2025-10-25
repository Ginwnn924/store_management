using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;

namespace StoreManagement.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryResponse>> GetAllAsync();
        Task<InventoryResponse?> GetByIdAsync(int id);
        Task<InventoryResponse?> AddAsync(InventoryRequest request);
        Task<InventoryResponse?> UpdateAsync(int id, InventoryRequest request);
        Task<bool> DeleteAsync(int id);
      
    }
}
