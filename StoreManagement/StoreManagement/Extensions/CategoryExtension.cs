using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Models;

namespace StoreManagement.Extensions
{
    public static class CategoryExtensions
    {
        public static IQueryable<Category> ApplyFilters(
            this IQueryable<Category> query,
            CategoryFilterRequest filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
            {
                string searchTerm = $"%{filter.CategoryName.Trim()}%";
                query = query.Where(c => EF.Functions.Like(c.CategoryName, searchTerm));
            }

           

            return query;
        }
    }
}
