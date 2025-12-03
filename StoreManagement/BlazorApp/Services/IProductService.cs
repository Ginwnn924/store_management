using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IProductService
{
    Task<PagedResponse<ProductResponse>?> GetProductsAsync(int pageNumber = 1, int pageSize = 10);
    Task<PagedResponse<ProductResponse>?> FilterProductsAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? productName = null,
        List<int>? categoryIds = null,
        decimal? minPrice = null,
        decimal? maxPrice = null);
    Task<ProductResponse?> GetProductByIdAsync(int id);
}

