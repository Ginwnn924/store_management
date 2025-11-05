using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface IOrderRepository : IRepository<int, Order>
    {
        Task<int> UpdateStatusAsync(int orderId, string status);
        IQueryable<Order> GetQueryable();
    }
}
