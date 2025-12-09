using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.DTOs.Response
{
    public class PromotionResponse
    {
        public int PromoId { get; set; }

        public string PromoCode { get; set; } = string.Empty;

        public string? Description { get; set; }


        public string DiscountType { get; set; } = "percent";

        public decimal DiscountValue { get; set; }
    }
}
