using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Models;

namespace StoreManagement.Extensions
{
    public static class SupplierExtension
    {
        public static IQueryable<Supplier> ApplyFilters(
            this IQueryable<Supplier> query,
            SupplierFilterRequest filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                string searchTerm = $"%{filter.Name.Trim()}%";
                query = query.Where(s => EF.Functions.Like(s.Name, searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(filter.Phone))
            {
                var phone = filter.Phone.Trim();
                query = query.Where(s => s.Phone == phone);
            }

            return query;
        }
    }
}


