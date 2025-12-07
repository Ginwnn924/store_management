using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;

namespace StoreManagement.Services
{
    public interface ICustomerService
    {
        Task<PagedResponse<CustomerResponse>> GetAllCustomersAsync(CustomerFilterRequest filter);
        Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync();
        Task<CustomerResponse> GetCustomerByIdAsync(int id);
        Task<CustomerResponse> CreateCustomerAsync(CustomerCreateRequest request);
        Task<CustomerResponse> UpdateCustomerAsync(CustomerCreateRequest request, int id);
        Task DeleteCustomerAsync(int id);
        Task<CustomerResponse> GetCustomerByEmailAsync(string email);
        Task<CustomerResponse> GetCustomerByPhoneAsync(string phone);
        Task<IEnumerable<CustomerResponse>> SearchCustomersByNameAsync(string name);

        Task<CustomerResponse> Register(CustomerRegisterRequest request);
    }
}
