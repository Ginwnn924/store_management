using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface IProductRepository : IRepository<int, Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId);
        Task<Product?> GetProductByBarcodeAsync(string barcode);
        Task<IEnumerable<Product>> SearchProductsByNameAsync(string name);
    }
}