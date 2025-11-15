using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Models;
using VNPAY.NET.Models;
using PaymentResponse = StoreManagement.DTOs.Response.PaymentResponse;
using PagedResponse = StoreManagement.DTOs.Response.PagedResponse<StoreManagement.DTOs.Response.PaymentResponse>;

namespace StoreManagement.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponse>> GetAllPaymentsAsync();
        Task<StoreManagement.DTOs.Response.PagedResponse<PaymentResponse>> GetAllPaymentsAsync(PaymentFilterRequest filter);
        Task<PaymentResponse> GetPaymentByIdAsync(int id);
        Task<PaymentResponse> AddAsync(Payment payment);
        Task<PaymentResponse> ProcessVnpayCallbackAsync(PaymentResult paymentResult, string? vnpAmountStr);
    }
}


