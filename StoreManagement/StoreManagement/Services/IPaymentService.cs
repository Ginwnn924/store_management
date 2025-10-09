using StoreManagement;

namespace StoreManagement.Services
{
    public interface IPaymentService
    {
        Task<Response> GetPaymentsAsync();
        Task<Response> GetPaymentByIdAsync(int id);
    }
}


