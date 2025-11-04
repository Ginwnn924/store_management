using StoreManagement;
using StoreManagement.Models;

namespace StoreManagement.Services
{
    public interface IPaymentService
    {
        Task<Response> GetPaymentsAsync();
        Task<Response> GetPaymentByIdAsync(int id);
        Task<Response> AddAsync(Payment payment);
    }
}


