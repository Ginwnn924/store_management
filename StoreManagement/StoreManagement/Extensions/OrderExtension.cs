using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Models;

namespace StoreManagement.Extensions
{
    public static class OrderExtension
    {
        public static IQueryable<Order> ApplyFilters(
            this IQueryable<Order> query,
            OrderFilterRequest filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.customerName))
            {
                string searchTerm = $"%{filter.customerName.Trim()}%";
                query = query.Where(o => o.Customer != null && EF.Functions.Like(o.Customer.Name, searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(filter.employeeName))
            {
                string searchTerm = $"%{filter.employeeName.Trim()}%";
                query = query.Where(o => o.User != null && EF.Functions.Like(o.User.FullName, searchTerm));
            }

            if (filter.minTotalAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= filter.minTotalAmount.Value);
            }

            if (filter.maxTotalAmount.HasValue)
            {
                if (filter.minTotalAmount.HasValue && filter.maxTotalAmount.Value < filter.minTotalAmount.Value)
                {
                    // Skip invalid MaxTotalAmount
                }
                else
                {
                    query = query.Where(o => o.TotalAmount <= filter.maxTotalAmount.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(filter.status))
            {
                query = query.Where(o => o.Status == filter.status.Trim());
            }

            if (filter.startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= filter.startDate.Value);
            }

            if (filter.endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= filter.endDate.Value);
            }

            return query;
        }
    }
}
