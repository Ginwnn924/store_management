using BlazorApp.Models;

namespace BlazorApp.Services;

public interface ICategoryService
{
    Task<PagedResponse<CategoryResponse>?> GetCategoriesAsync(int pageNumber = 1, int pageSize = 50);
    Task<CategoryResponse?> GetCategoryByIdAsync(int id);
}

