using StoreManagement.DTOs;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
namespace StoreManagement.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierResponse>> GetAllSuppliersAsync();
        Task<PagedResponse<SupplierResponse>> GetALlSuppliersAsync(SupplierFilterRequest filter);
        Task<SupplierResponse> GetSupplierByIdAsync(int id);
        Task<SupplierResponse> CreateSupplierAsync(SupplierDto supplierDto);
        Task<SupplierResponse> UpdateSupplierAsync(int id, SupplierDto supplierDto);
        Task<bool> DeleteSupplierAsync(int id);
    }
}