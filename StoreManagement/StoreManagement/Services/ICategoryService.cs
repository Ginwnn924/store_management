using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;

namespace StoreManagement.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();
        Task<CategoryResponse?> GetCategoryByIdAsync(int id);
        Task<CategoryResponse> AddCategoryAsync(string categoryName);
        Task<CategoryResponse?> UpdateCategoryAsync(int id, string categoryName);
        Task<bool> DeleteCategoryAsync(int id);
        Task<IEnumerable<CategoryResponse>> SearchByNameAsync(string categoryName);
        Task<PagedResponse<CategoryResponse>> FilterAsync(CategoryFilterRequest filter);
    }
}
