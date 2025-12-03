using System.Net.Http.Json;
using BlazorApp.Models;

namespace BlazorApp.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductService> _logger;

    public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PagedResponse<ProductResponse>?> GetProductsAsync(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<ProductResponse>>>(
                $"api/Product?pageNumber={pageNumber}&pageSize={pageSize}");

            return response?.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching products");
            return null;
        }
    }

    public async Task<PagedResponse<ProductResponse>?> FilterProductsAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? productName = null,
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"pageNumber={pageNumber}",
                $"pageSize={pageSize}"
            };

            if (!string.IsNullOrWhiteSpace(productName))
                queryParams.Add($"ProductName={Uri.EscapeDataString(productName)}");

            if (categoryId.HasValue)
                queryParams.Add($"categoryId={categoryId.Value}");

            if (minPrice.HasValue)
                queryParams.Add($"MinPrice={minPrice.Value}");

            if (maxPrice.HasValue)
                queryParams.Add($"MaxPrice={maxPrice.Value}");

            var queryString = string.Join("&", queryParams);
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<ProductResponse>>>(
                $"api/Product/filter?{queryString}");

            return response?.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error filtering products");
            return null;
        }
    }

    public async Task<ProductResponse?> GetProductByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<ProductResponse>>(
                $"api/Product/{id}");

            return response?.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product {ProductId}", id);
            return null;
        }
    }
}

