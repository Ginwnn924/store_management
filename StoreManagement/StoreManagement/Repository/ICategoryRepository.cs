using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface ICategoryRepository : IRepository<int, Category>
    {
        Task<IEnumerable<Category>> SearchByNameAsync(string categoryName);
        IQueryable<Category> GetQueryable();
    }
}
