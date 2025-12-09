using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public class PromotionMapper : IMapper<Promotion, PromotionResponse>
    {
        public void MapToExistingModel(PromotionResponse dto, Promotion entity)
        {
            throw new NotImplementedException();
        }

        public PromotionResponse ToDto(Promotion entity)
        {
            return new PromotionResponse
            {
                PromoId = entity.PromoId,
                PromoCode = entity.PromoCode,
                Description = entity.Description,
                DiscountType = entity.DiscountType,
                DiscountValue = entity.DiscountValue
            };
        }

        public IEnumerable<PromotionResponse> ToDtoList(IEnumerable<Promotion> entities)
        {
            return entities.Select(e => ToDto(e));
        }

        public Promotion ToModel(PromotionResponse dto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Promotion> ToModelList(IEnumerable<PromotionResponse> dtos)
        {
            throw new NotImplementedException();
        }
    }
}
