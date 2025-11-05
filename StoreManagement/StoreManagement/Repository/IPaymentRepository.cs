using StoreManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagement.Repository
{
    public interface IPaymentRepository
    {   
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<List<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
    }
}
