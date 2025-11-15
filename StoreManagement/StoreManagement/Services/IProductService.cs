using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;

namespace StoreManagement.Services
{
    public interface IProductService
    {
        Task<PagedResponse<ProductResponse>> GetAllProductsAsync(ProductFilterRequest filter);
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse> GetProductByIdAsync(int id);
        Task<ProductResponse> CreateProductAsync(ProductCreateRequest request);
        Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest request,int id);
        Task<bool> DeleteProductAsync(int id);
        //Task<ProductResponse> GetProductsByCategoryAsync(int categoryId);
        //Task<ProductResponse> GetProductsBySupplierAsync(int supplierId);
        Task<ProductResponse> GetProductByBarcodeAsync(string barcode);
        //Task<ProductResponse> SearchProductsByNameAsync(string name);
    }
}