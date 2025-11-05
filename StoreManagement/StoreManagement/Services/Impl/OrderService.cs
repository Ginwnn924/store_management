using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StoreManagement.DTOs.Request;
using StoreManagement.Enum;
using StoreManagement.Mapper;
using StoreManagement.Models;
using StoreManagement.Repository;
using Order = StoreManagement.Models.Order;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace StoreManagement.Services.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;
        private readonly OrderMapper _orderMapper = new OrderMapper();

        public OrderService(IOrderRepository orderRepository, IPaymentRepository paymentRepository, IVnpay vnPayservice, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;

            _vnpay = vnPayservice;
            _configuration = configuration;


            var tmnCode = _configuration["Vnpay:TmnCode"] ?? throw new ArgumentNullException("Vnpay:TmnCode");
            var hashSecret = _configuration["Vnpay:HashSecret"] ?? throw new ArgumentNullException("Vnpay:HashSecret");
            var baseUrl = _configuration["Vnpay:BaseUrl"] ?? throw new ArgumentNullException("Vnpay:BaseUrl");
            var callbackUrl = _configuration["Vnpay:CallbackUrl"] ?? throw new ArgumentNullException("Vnpay:CallbackUrl");

            _vnpay.Initialize(tmnCode, hashSecret, baseUrl, callbackUrl);

        }

        public Task<Response> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetOrders()
        {
            var listOrder = await _orderRepository.GetAllAsync();
            return Response.Success(
                _orderMapper.ToDtoList(listOrder)
            );
        }

        public async Task<Response> CreateOrder(OrderRequest request)
        {

            if (request.PaymentMethod == Enum.PaymentMethod.cash) 
            {
                request.OrderStatus = Enum.OrderStatus.paid;
                Order newOrder = _orderMapper.ToModel(request);
                await _orderRepository.AddAsync(newOrder);
                Payment payment = new Payment()
                {
                    OrderId = newOrder.OrderId,
                    Amount = newOrder.TotalAmount - newOrder.DiscountAmount,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = PaymentMethod.cash.ToString(),
                };
                // Ensure we await the repository call to keep the DbContext alive within the request scope
                await _paymentRepository.CreatePaymentAsync(payment);
            }
            return Response.Success("Order created successfully");
        }
        public async Task<long> CreateOrderOnly(OrderRequest request)
        {
            Order createdOrder = null;
            if (request.PaymentMethod == Enum.PaymentMethod.cash)
            {
                request.OrderStatus = Enum.OrderStatus.paid;
                Order newOrder = _orderMapper.ToModel(request);
                newOrder.Status = Enum.OrderStatus.pending.ToString();
                createdOrder = await _orderRepository.AddAsync(newOrder);
            }
            if (createdOrder == null)
            {
                throw new Exception("Order creation failed");
            }
            return createdOrder.OrderId;
        }

        public async Task<(long OrderId, string PaymentUrl)> CreateOnlyOrder(OrderRequest request, string ipAddress)
        {
            // Map the order request to the Order model
            Order newOrder = _orderMapper.ToModel(request);
            newOrder.Status = Enum.OrderStatus.pending.ToString();

            // Save the order to the database
            Order createdOrder = await _orderRepository.AddAsync(newOrder);
            if (createdOrder == null)
            {
                throw new Exception("Order creation failed");
            }

            // Calculate the payment amount
            long paymentAmount = request.totalAmount - request.discountAmount;

            // Create the payment request
            var paymentRequest = new PaymentRequest
            {
                PaymentId = createdOrder.OrderId,
                Money = paymentAmount,
                Description = "Thanh toán store_management",
                IpAddress = ipAddress,
                BankCode = BankCode.ANY,
                CreatedDate = DateTime.Now,
                Currency = Currency.VND,
                Language = DisplayLanguage.Vietnamese
            };

            // Generate the payment URL using VNPAY
            var paymentUrl = _vnpay.GetPaymentUrl(paymentRequest);

            // Return the order ID and payment URL
            return (createdOrder.OrderId, paymentUrl);
        }
    }
}
