using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Models;

namespace StoreManagement.Services
{
    public interface IOrderService
    {
        Task<Response> GetAllOrdersAsync(OrderFilterRequest request);
        Task<Response> GetOrders();
        Task<Response> GetOrderById(int id);
    }
}
