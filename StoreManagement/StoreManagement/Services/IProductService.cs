using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;

namespace StoreManagement.Services
{
    public interface IProductService
    {
        Task<Response> GetAllProductsAsync(ProductFilterRequest filter);
        Task<Response> GetAllProductsAsync();
        Task<Response> GetProductByIdAsync(int id);
        Task<Response> CreateProductAsync(ProductCreateRequest request);
        Task<Response> UpdateProductAsync(ProductUpdateRequest request,int id);
        Task<Response> DeleteProductAsync(int id);
        Task<Response> GetProductsByCategoryAsync(int categoryId);
        Task<Response> GetProductsBySupplierAsync(int supplierId);
        Task<Response> GetProductByBarcodeAsync(string barcode);
        Task<Response> SearchProductsByNameAsync(string name);
    }
}