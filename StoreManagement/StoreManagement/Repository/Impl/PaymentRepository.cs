using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly StoreManagementDbContext _context;
        
        public PaymentRepository(StoreManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            var entry = await _context.AddAsync(payment);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
    }
}
