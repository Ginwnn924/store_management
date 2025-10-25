using StoreManagement.DTOs.Response;
using StoreManagement.Models;
using StoreManagement.Repository;
using StoreManagement.Mapper;

namespace StoreManagement.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => c.ToCategoryResponse());
        }

        public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category?.ToCategoryResponse();
        }

        public async Task<CategoryResponse> AddCategoryAsync(string categoryName)
        {
            var category = new Category
            {
                CategoryName = categoryName
            };

            await _categoryRepository.AddAsync(category);
            return category.ToCategoryResponse();
        }

        public async Task<CategoryResponse?> UpdateCategoryAsync(int id, string categoryName)
        {
            var existing = await _categoryRepository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.CategoryName = categoryName;
            await _categoryRepository.UpdateAsync(existing);
            return existing.ToCategoryResponse();
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }
    }
}
