using StoreManagement;
using StoreManagement.DTOs.Request.Filter;

namespace StoreManagement.Services
{
    public interface IPaymentService
    {
        Task<Response> GetPaymentsAsync();
        Task<Response> GetPaymentsAsync(PaymentFilterRequest filter);
        Task<Response> GetPaymentByIdAsync(int id);
    }
}


