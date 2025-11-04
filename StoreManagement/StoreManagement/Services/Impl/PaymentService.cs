using StoreManagement.DTOs;
using StoreManagement.Models;
using StoreManagement.Repository;
namespace StoreManagement.Services.Impl
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository,IOrderRepository orderRepository)
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
                Console.WriteLine($"Console: 3 Update order status {payment.OrderId} {newStatus}");

                Console.WriteLine("Console 4: bat dau create payment");
                await _paymentRepository.CreatePaymentAsync(payment);

                Console.WriteLine("Console final: DA LU THANH TOAN");
                return Response.Success("Thêm thanh toán thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Console ERROR: {ex.Message}");
                return Response.Fail("An error occurred while processing the payment", 500);
            }
        }
    }
}


