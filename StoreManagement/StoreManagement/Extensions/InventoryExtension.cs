using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Models;

namespace StoreManagement.Extensions
{
    public static class InventoryExtension
    {
        public static IQueryable<Inventory> ApplyFilters(
            this IQueryable<Inventory> query,
            InventoryFilterRequest filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.ProductName))
            {
                string searchTerm = $"%{filter.ProductName.Trim()}%";
                query = query.Where(i => EF.Functions.Like(i.Product.ProductName, searchTerm));
            }

            return query;
        }
    }
}
