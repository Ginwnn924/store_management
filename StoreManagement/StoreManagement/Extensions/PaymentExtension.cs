using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Models;

namespace StoreManagement.Extensions
{
    public static class PaymentExtension
    {
        public static IQueryable<Payment> ApplyFilters(
            this IQueryable<Payment> query,
            PaymentFilterRequest filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.PaymentMethod))
            {
                var method = filter.PaymentMethod.Trim().ToLowerInvariant();
                query = query.Where(p => p.PaymentMethod.ToLower() == method);
            }

            return query;
        }
    }
}


