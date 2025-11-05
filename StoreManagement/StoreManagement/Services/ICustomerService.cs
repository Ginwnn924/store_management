using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;

namespace StoreManagement.Services
{
    public interface ICustomerService
    {
        Task<Response> GetAllCustomersAsync(CustomerFilterRequest filter);
        Task<Response> GetAllCustomersAsync();
        Task<Response> GetCustomerByIdAsync(int id);
        Task<Response> CreateCustomerAsync(CustomerCreateRequest request);
        Task<Response> UpdateCustomerAsync(CustomerCreateRequest request, int id);
        Task<Response> DeleteCustomerAsync(int id);
        Task<Response> GetCustomerByEmailAsync(string email);
        Task<Response> GetCustomerByPhoneAsync(string phone);
        Task<Response> SearchCustomersByNameAsync(string name);
    }
}
