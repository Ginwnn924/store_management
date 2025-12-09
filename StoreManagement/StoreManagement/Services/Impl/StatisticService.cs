using StoreManagement.DTOs.Response;
using StoreManagement.DTOs.Response.StatisticResponse;
using StoreManagement.Mapper;
using StoreManagement.Repository;

namespace StoreManagement.Services.Impl
{
    public class StatisticService : IStatisticService
    {
        //private readonly ProductMapper _productMapper = new ProductMapper();

        private readonly IStatisticRepository _statisticRepository;

        public StatisticService(IStatisticRepository statisticRepository)
        {
            _statisticRepository = statisticRepository;
        }
        public async Task<IEnumerable<RevenueResponse>> GetRevenueAsync(int year , int? month , int? day)
        {
            if (month.HasValue && day.HasValue)
            {
                var dailyRevenue = await _statisticRepository.GetDailyRevenueByDateAsync(year, month.Value, day.Value);
                return dailyRevenue is not null ? new List<RevenueResponse> { dailyRevenue } : Enumerable.Empty<RevenueResponse>();
            }
            else if (month.HasValue)
            {
                return await _statisticRepository.GetMonthlyRevenueAsync(month.Value, year);
            }
            else
            {
                return await _statisticRepository.GetYearlyRevenueAsync(year);
            }
        }

        public async Task<IEnumerable<TopSellerProductResponse>> GetTopProduct(int top )
        {
            return await _statisticRepository.GetTopProduct(top);
        }
    }
}
