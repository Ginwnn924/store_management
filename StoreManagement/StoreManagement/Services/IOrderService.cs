using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Models;

namespace StoreManagement.Services
{
    public interface IOrderService
    {
        Task<Response> GetOrders();
        Task<Response> GetOrderById(int id);
    }
}
