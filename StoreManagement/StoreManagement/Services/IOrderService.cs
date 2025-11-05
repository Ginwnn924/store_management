using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Request;
using StoreManagement.Models;

namespace StoreManagement.Services
{
    public interface IOrderService
    {
        Task<Response> GetAllOrdersAsync(OrderFilterRequest request);
        Task<Response> GetOrders();
        Task<Response> GetOrderById(int id);
        Task<Response> CreateOrder(OrderRequest request);
    }
}
