using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StoreManagement.Mapper;
using StoreManagement.Models;
using StoreManagement.Repository;
using Order = StoreManagement.Models.Order;

namespace StoreManagement.Services.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrderMapper _orderMapper = new OrderMapper();

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
    }
}
