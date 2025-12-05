using StoreManagement.DTOs.Response.StatisticResponse;
using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface IStatisticRepository
    {
        Task<List<DailyRevenueResponse>> GetDailyRevenueAsync();
        Task<IEnumerable<Product>> GetTopProduct();
    }
}
