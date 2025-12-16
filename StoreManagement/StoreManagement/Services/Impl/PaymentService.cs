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
        private readonly IInventoryRepository _inventoryRepository;
        private readonly PaymentMapper _paymentMapper = new PaymentMapper();

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IInventoryRepository inventoryRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _inventoryRepository = inventoryRepository;
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

            // Update inventory quantities based on order items (extracted method)
            await UpdateInventoryByOrderAsync(payment.OrderId);

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

            // AddAsync already handles inventory reduction and status update
            return await AddAsync(payment);
        }
        public async Task UpdateInventoryByOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetQueryable()
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null || order.OrderItems == null || !order.OrderItems.Any())
                return;

            foreach (var item in order.OrderItems.Where(i => !i.IsDeleted))
            {
                try
                {
                    var inventory = await _inventoryRepository.GetQueryable()
                        .FirstOrDefaultAsync(i => i.ProductId == item.ProductId);

                    if (inventory == null)
                        continue;

                    var newQuantity = inventory.Quantity - item.Quantity;
                    inventory.Quantity = newQuantity < 0 ? 0 : newQuantity;
                    inventory.UpdatedAt = DateTime.UtcNow;

                    await _inventoryRepository.UpdateAsync(inventory);
                }
                catch
                {
             
                }
            }
        }
    }
}


