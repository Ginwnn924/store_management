using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Models;

namespace StoreManagement.Extensions
{
    public static class UserExtension
    {
        public static IQueryable<User> ApplyFilters(
            this IQueryable<User> query,
            UserFilterRequest filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Username))
            {
                string searchTerm = $"%{filter.Username.Trim()}%";
                query = query.Where(u => EF.Functions.Like(u.Username, searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(filter.FullName))
            {
                string searchTerm = $"%{filter.FullName.Trim()}%";
                query = query.Where(u => u.FullName != null && EF.Functions.Like(u.FullName, searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(filter.Role))
            {
                var role = filter.Role.Trim().ToLowerInvariant();
                query = query.Where(u => u.Role == role);
            }

            return query;
        }
    }
}


