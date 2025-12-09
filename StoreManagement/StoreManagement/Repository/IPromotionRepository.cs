using StoreManagement.Models;

namespace StoreManagement.Repository
{
    public interface IPromotionRepository 
    {
        Task<IEnumerable<Promotion>> GetAllPromotion(long min_order_amount);

    }
}
