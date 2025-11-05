using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;
using StoreManagement.Repository;
using StoreManagement.Mapper;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Extensions;

namespace StoreManagement.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Response> FilterAsync(CategoryFilterRequest filter)
        {
            var query = _categoryRepository.GetQueryable();
            query = query.ApplyFilters(filter);

            int totalRecords = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.CategoryId)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var responseItems = items.Select(c => c.ToCategoryResponse()).ToList();

            var paged = new PagedResponse<CategoryResponse>(
                responseItems,
                totalRecords,
                filter.PageNumber,
                filter.PageSize
            );

            return new Response
            {
                Status = 200,
                Message = "Lấy danh sách danh mục thành công",
                Data = paged
            };
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
        public async Task<IEnumerable<CategoryResponse>> SearchByNameAsync(string categoryName)
        {
           
            var results = await _categoryRepository.SearchByNameAsync(categoryName.Trim());
            return results.Select(c => c.ToCategoryResponse());
        }
    }
}
