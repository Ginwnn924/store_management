using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Extensions;
using StoreManagement.DTOs.Request;
using StoreManagement.Enum;
using StoreManagement.Mapper;
using StoreManagement.Models;
using StoreManagement.Repository;
using Order = StoreManagement.Models.Order;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using Microsoft.Extensions.Logging;

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


        public OrderService(IOrderRepository orderRepository, IPaymentRepository paymentRepository)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<PagedResponse<OrderResponse>> GetAllOrdersAsync(OrderFilterRequest filter)
        {
            var query = _orderRepository.GetQueryable();
            query = query.ApplyFilters(filter);
            var totalItems = await query.CountAsync();
            var orders = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var orderResponses = _orderMapper.ToDtoList(orders).ToList();

            var pagedResponse = new PagedResponse<OrderResponse>(
                orderResponses,
                totalItems,
                filter.PageNumber,
                filter.PageSize
            );

            return pagedResponse;
        }

        public async Task<OrderResponse> GetOrderById(int id)
        {
            // Ensure includes for User, Customer, OrderItems, Product
            var order = await _orderRepository.GetQueryable()
                .FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                throw new Exception($"Order with id {id} not found");
            }
            return _orderMapper.ToDto(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrders()
        {
            var listOrder = await _orderRepository.GetAllAsync();
            return _orderMapper.ToDtoList(listOrder);
        }

        public async Task CreateOrder(OrderRequest request)
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
                await _paymentRepository.AddAsync(payment);
            }
        }

        public async Task<OrderResponse> CreateOrderReturnOrder(OrderRequest request)
        {
            Order createdOrder = null;
            if (request.PaymentMethod == Enum.PaymentMethod.cash)
            {
                request.OrderStatus = Enum.OrderStatus.paid;
                Order newOrder = _orderMapper.ToModel(request);
                newOrder.Status = Enum.OrderStatus.paid.ToString();
                createdOrder = await _orderRepository.AddAsync(newOrder);
            }
            
           
            if (createdOrder == null)
            {
                throw new Exception("Order creation failed");
            }
            Payment payment = new Payment()
            {
                OrderId = createdOrder.OrderId,
                Amount = createdOrder.TotalAmount - createdOrder.DiscountAmount,
                PaymentDate = DateTime.Now,
                PaymentMethod = PaymentMethod.cash.ToString(),
            };
            await _paymentRepository.AddAsync(payment);

            var orderWithProduct = await _orderRepository.GetQueryable()
                .FirstOrDefaultAsync(o => o.OrderId == createdOrder.OrderId);

            if (orderWithProduct == null)
                throw new Exception("Order not found after creation");

            OrderResponse createdOrderResponse = _orderMapper.ToDto(orderWithProduct);
            return createdOrderResponse;
        }


        public async Task<(long OrderId, string PaymentUrl)> CreateOnlyOrder(OrderRequest request, string ipAddress)
        {
            // Map the order request to the Order model
            Order newOrder = _orderMapper.ToModel(request);
            newOrder.Status = Enum.OrderStatus.pending.ToString();

            Order createdOrder = await _orderRepository.AddAsync(newOrder);
            if (createdOrder == null)
            {
                throw new Exception("Order creation failed");
            }

            long paymentAmount = request.totalAmount - request.discountAmount;

            var paymentRequest = new PaymentRequest
            {
                PaymentId = createdOrder.OrderId,
                Money = paymentAmount,
                Description = $"Order Id :{createdOrder.OrderId}- Amount :{paymentAmount}",
                IpAddress = ipAddress,
                BankCode = BankCode.ANY,
                CreatedDate = DateTime.Now,
                Currency = Currency.VND,
                Language = DisplayLanguage.Vietnamese
            };

            var paymentUrl = _vnpay.GetPaymentUrl(paymentRequest);

            return (createdOrder.OrderId, paymentUrl);
        }
    }
}
