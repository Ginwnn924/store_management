using StoreManagement;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Models;
using VNPAY.NET.Models;

namespace StoreManagement.Services
{
    public interface IPaymentService
    {
        Task<Response> GetPaymentsAsync();
        Task<Response> GetPaymentsAsync(PaymentFilterRequest filter);
        Task<Response> GetPaymentByIdAsync(int id);
        Task<Response> AddAsync(Payment payment);
        Task<Response> ProcessVnpayCallbackAsync(PaymentResult paymentResult, string? vnpAmountStr);
    }
}


