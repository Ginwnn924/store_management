//using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;

namespace StoreManagement.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryResponse>> GetAllAsync();
        Task<InventoryResponse?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
      
    }
}
