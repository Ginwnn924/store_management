using StoreManagement.DTOs.Response;
using StoreManagement.Mapper;
using StoreManagement.Repository;

namespace StoreManagement.Services.Impl
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly PromotionMapper _mapper;

        public PromotionService(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
            _mapper = new PromotionMapper();
        }
        public async Task<IEnumerable<PromotionResponse>> GetPromotions(long minOrderAmount)
        {
            var entity = await _promotionRepository.GetAllPromotion(minOrderAmount);
            return _mapper.ToDtoList(entity);
        }
    }
}
