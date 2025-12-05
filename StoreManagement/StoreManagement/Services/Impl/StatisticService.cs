using StoreManagement.DTOs.Response;
using StoreManagement.DTOs.Response.StatisticResponse;
using StoreManagement.Mapper;
using StoreManagement.Repository;

namespace StoreManagement.Services.Impl
{
    public class StatisticService : IStatisticService
    {
        private readonly ProductMapper _productMapper = new ProductMapper();

        private readonly IStatisticRepository _statisticRepository;

        public StatisticService(IStatisticRepository statisticRepository)
        {
            _statisticRepository = statisticRepository;
        }
        public Task<List<DailyRevenueResponse>> GetDailyRevenueAsync()
        {
            return _statisticRepository.GetDailyRevenueAsync();
        }

        //public Task<IEnumerable<ProductResponse>> GetTopProduct()
        //{
        //    var listEntity = _statisticRepository.GetTopProduct();
        //    return _productMapper.ToDtoList(listEntity);
        //}
    }
}
