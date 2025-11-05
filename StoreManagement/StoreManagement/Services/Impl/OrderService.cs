using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Extensions;
using StoreManagement.DTOs.Request;
using StoreManagement.Enum;
using StoreManagement.Mapper;
using StoreManagement.Models;
using StoreManagement.Repository;
using StoreManagement.Repository.Impl;
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

        public async Task<Response> GetAllOrdersAsync(OrderFilterRequest filter)
        {

            try
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
                return Response.Success(pagedResponse, "Orders retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving Orders: {ex.Message}", 500);
            }
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
