using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Models;

namespace StoreManagement.Extensions
{
    public static class CustomerExtension
    {
        public static IQueryable<Customer> ApplyFilters(
            this IQueryable<Customer> query,
            CustomerFilterRequest filter)
        {
            
            if (!string.IsNullOrWhiteSpace(filter.FullName))
            {
                string searchTerm = $"%{filter.FullName.Trim()}%";
                query = query.Where(c => EF.Functions.Like(c.Name, searchTerm));
            }
           
            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(c => c.Email == filter.Email.Trim());
            }
           
            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
            {
                query = query.Where(c => c.Phone == filter.PhoneNumber.Trim());
            }
            return query;
        }
    }
}
