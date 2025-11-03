using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StoreManagement.DTOs.Request;
using StoreManagement.Enum;
using StoreManagement.Mapper;
using StoreManagement.Models;
using StoreManagement.Repository;
using Order = StoreManagement.Models.Order;

namespace StoreManagement.Services.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;

        private readonly OrderMapper _orderMapper = new OrderMapper();

        public OrderService(IOrderRepository orderRepository, IPaymentRepository paymentRepository)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
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
                _paymentRepository.CreatePaymentAsync(payment);
            }
            return Response.Success("Order created successfully");
        }
    }
}
