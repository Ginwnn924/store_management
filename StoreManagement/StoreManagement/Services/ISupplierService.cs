using StoreManagement.DTOs;
using StoreManagement.DTOs.Request.Filter;

namespace StoreManagement.Services
{
    public interface ISupplierService
    {
        Task<Response> GetSuppliersAsync();
        Task<Response> GetSuppliersAsync(SupplierFilterRequest filter);
        Task<Response> GetSupplierByIdAsync(int id);
        Task<Response> CreateSupplierAsync(SupplierDto supplierDto);
        Task<Response> UpdateSupplierAsync(int id, SupplierDto supplierDto);
        Task<Response> DeleteSupplierAsync(int id);
    }
}