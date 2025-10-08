using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Models;

namespace StoreManagement.Services
{
    public interface IOrderService
    {
        Task<Response<Order>> GetOrders();
        Task<Response<Order>> GetOrderById(int id);
    }
}
