using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface IPaymentRepository
    {
        Task<Payment> CreatePaymentAsync(Payment payment);
    }
}
