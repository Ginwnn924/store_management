using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.DTOs.Request.Filter;
namespace StoreManagement.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryResponse>> GetAllAsync();
        Task<InventoryResponse?> GetByIdAsync(int id);
        Task<InventoryResponse> AddAsync(InventoryRequest request);
        Task<InventoryResponse?> UpdateAsync(int id, InventoryRequest request);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<InventoryResponse>> SearchByProductNameAsync(string productName);
        Task<PagedResponse<InventoryResponse>> GetAllInventoryAsync(InventoryFilterRequest filter);
    }
}
