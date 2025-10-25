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
    }
}
