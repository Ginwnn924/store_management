using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Extensions;
using StoreManagement.Mapper;
using StoreManagement.Models;
using StoreManagement.Repository;
using StoreManagement.Exceptions;
using VNPAY.NET.Models;
using PaymentResponse = StoreManagement.DTOs.Response.PaymentResponse;
using StoreManagement.DTOs.Response;

namespace StoreManagement.Services.Impl
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentMapper _paymentMapper = new PaymentMapper();

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<PaymentResponse>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return _paymentMapper.ToDtoList(payments);
        }

        public async Task<PagedResponse<PaymentResponse>> GetAllPaymentsAsync(PaymentFilterRequest filter)
        {
            var query = _paymentRepository.GetQueryable();
            query = query.ApplyFilters(filter);
            var totalItems = await query.CountAsync();
            var payments = await query
                .OrderByDescending(p => p.PaymentDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var paymentResponses = _paymentMapper.ToDtoList(payments).ToList();

            var pagedResponse = new PagedResponse<PaymentResponse>(
                paymentResponses,
                totalItems,
                filter.PageNumber,
                filter.PageSize
            );
            return pagedResponse;
        }

        public async Task<PaymentResponse> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return _paymentMapper.ToDto(payment);
        }

        public async Task<PaymentResponse> AddAsync(Payment payment)
        {
            // Update order status to paid
            var newStatus = Enum.OrderStatus.paid.ToString();
            await _orderRepository.UpdateStatusAsync(payment.OrderId, newStatus);

            // Create payment
            var createdPayment = await _paymentRepository.AddAsync(payment);
            var response = await _paymentRepository.GetByIdAsync(createdPayment.PaymentId);
            var paymentResponse = _paymentMapper.ToDto(response);
            return paymentResponse;
        }

        public async Task<PaymentResponse> ProcessVnpayCallbackAsync(PaymentResult paymentResult, string? vnpAmountStr)
        {
            if (string.IsNullOrWhiteSpace(vnpAmountStr) || !long.TryParse(vnpAmountStr, out var vnpAmountLong))
            {
                throw new InvalidException("Invalid VNPAY amount.");
            }

            var amount = (decimal)vnpAmountLong / 100m;

            if (!paymentResult.IsSuccess)
            {
                var message = $"{paymentResult.PaymentResponse.Description}. {paymentResult.TransactionStatus.Description}.";
                throw new PaymentFailedException(message);
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

            return await AddAsync(payment);
        }
    }
}


