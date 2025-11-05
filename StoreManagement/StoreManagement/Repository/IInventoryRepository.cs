using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface IInventoryRepository :IRepository<int ,Inventory>
    {
        Task<IEnumerable<Inventory>> SearchByProductNameAsync(string productName);
        IQueryable<Inventory> GetQueryable();
    }
}
