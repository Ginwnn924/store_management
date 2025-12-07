using StoreManagement.DTOs.Response.StatisticResponse;
using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface IStatisticRepository
    {
        //Task<IEnumerable<DailyRevenueResponse>> GetDailyRevenueAsync();
        Task<IEnumerable<TopSellerProductResponse>> GetTopProduct(int top);

        Task<IEnumerable<RevenueResponse>> GetYearlyRevenueAsync(int year);
        Task<RevenueResponse?> GetDailyRevenueByDateAsync(int year, int month, int day);

        Task<IEnumerable<RevenueResponse>> GetMonthlyRevenueAsync(int month, int year);
    }
}