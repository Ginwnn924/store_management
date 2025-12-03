using System.Net.Http.Json;
using BlazorApp.Models;

namespace BlazorApp.Services;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(HttpClient httpClient, ILogger<CategoryService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PagedResponse<CategoryResponse>?> GetCategoriesAsync(int pageNumber = 1, int pageSize = 50)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<CategoryResponse>>>(
                $"api/Category?pageNumber={pageNumber}&pageSize={pageSize}");

            return response?.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching categories");
            return null;
        }
    }

    public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<CategoryResponse>>(
                $"api/Category/{id}");

            return response?.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching category {CategoryId}", id);
            return null;
        }
    }
}

