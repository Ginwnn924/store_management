using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface IOrderRepository : IRepository<int, Order>
    {
        IQueryable<Order> GetQueryable();
    }
}
