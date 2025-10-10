using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface ICategoryRepository : IRepository<int, Category>
    {
        Task<Category> GetCategoryAsync(String CategoryName);
    }
}
