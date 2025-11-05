using StoreManagement.DTOs;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Extensions;
using StoreManagement.Models;
using StoreManagement.Repository;
using VNPAY.NET.Models;

namespace StoreManagement.Services.Impl
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<Response> GetPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            var paymentDtos = payments
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentDate = p.PaymentDate
                })
                .ToList();

            return Response.Success(paymentDtos);
        }

        public async Task<Response> GetPaymentsAsync(PaymentFilterRequest filter)
        {
            //var query = _dbContext.Payments.AsNoTracking().AsQueryable();
            //query = query.ApplyFilters(filter);

            //var totalItems = await query.CountAsync();
            //var items = await query
            //    .OrderByDescending(p => p.PaymentDate)
            //    .Skip((filter.PageNumber - 1) * filter.PageSize)
            //    .Take(filter.PageSize)
            //    .Select(p => new PaymentDto
            //    {
            //        PaymentId = p.PaymentId,
            //        OrderId = p.OrderId,
            //        Amount = p.Amount,
            //        PaymentMethod = p.PaymentMethod,
            //        PaymentDate = p.PaymentDate
            //    })
            //    .ToListAsync();

            //var paged = new PagedResponse<PaymentDto>(items, totalItems, filter.PageNumber, filter.PageSize);
            return Response.Success(null);
        }

        public async Task<Response> GetPaymentByIdAsync(int id)
        {
            var p = await _paymentRepository.GetByIdAsync(id);
            if (p == null)
            {
                return Response.Fail("Không tìm thấy thanh toán", 404);
            }
            var dto = new PaymentDto
            {
                PaymentId = p.PaymentId,
                OrderId = p.OrderId,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                PaymentDate = p.PaymentDate
            };
            return Response.Success(dto);
        }

        public async Task<Response> AddAsync(Payment payment)
        {
            try
            {
                var newStatus = Enum.OrderStatus.paid.ToString();
                await _orderRepository.UpdateStatusAsync(payment.OrderId, newStatus);

                await _paymentRepository.CreatePaymentAsync(payment);
                // Return order id in Data to support redirect building
                return Response.Success(payment.OrderId);
            }
            catch (Exception ex)
            {
                return Response.Fail($"An error occurred while processing the payment: {ex.Message}", 500);
            }
        }

        public async Task<Response> ProcessVnpayCallbackAsync(PaymentResult paymentResult, string? vnpAmountStr)
        {
            if (string.IsNullOrWhiteSpace(vnpAmountStr) || !long.TryParse(vnpAmountStr, out var vnpAmountLong))
            {
                return Response.Fail("Invalid payment amount.", 400);
            }

            var amount = (decimal)vnpAmountLong / 100m;

            if (!paymentResult.IsSuccess)
            {
                return Response.Fail($"{paymentResult.PaymentResponse.Description}. {paymentResult.TransactionStatus.Description}.", 400);
            }

            var orderId = paymentResult.PaymentId;
            var paymentMethod = paymentResult.BankingInfor?.BankCode == "NCB"
                ? Enum.PaymentMethod.bank_transfer.ToString()
                : Enum.PaymentMethod.card.ToString();

            var payment = new Payment
            {
                OrderId = (int)orderId,
                Amount = amount,
                PaymentDate = paymentResult.Timestamp,
                PaymentMethod = paymentMethod
            };
            await AddAsync(payment);
            return Response.Success(payment.OrderId);
        }
    }
}


