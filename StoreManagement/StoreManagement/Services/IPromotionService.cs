using StoreManagement.DTOs.Response;

namespace StoreManagement.Services;

    public interface IPromotionService
    {
    Task<IEnumerable<PromotionResponse>> GetPromotions(long minOrderAmount);

}

