using StoreManagement.DTOs.Response.StatisticResponse;

namespace StoreManagement.Services;

public interface IStatisticService
{
    Task<List<DailyRevenueResponse>> GetDailyRevenueAsync();

}

