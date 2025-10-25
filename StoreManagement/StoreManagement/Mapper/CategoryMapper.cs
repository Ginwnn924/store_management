using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public static class CategoryMapper
    {
        public static CategoryResponse ToCategoryResponse(this Category category)
        {
            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public static Category ToEntity(this CategoryResponse dto)
        {
            return new Category
            {
                //CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName
            };
        }
    }
}
