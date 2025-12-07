using StoreManagement.DTOs.Response;
using StoreManagement.DTOs.Response.StatisticResponse;

namespace StoreManagement.Services;

public interface IStatisticService
{
    Task<IEnumerable<RevenueResponse>> GetRevenueAsync(int year, int? month, int? day);
    Task<IEnumerable<TopSellerProductResponse>> GetTopProduct(int top);
}

