using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface IPaymentRepository : IRepository<int, Payment>
    {
        IQueryable<Payment> GetQueryable();
    }
}
