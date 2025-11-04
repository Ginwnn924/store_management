using StoreManagement.Enum;

namespace StoreManagement.DTOs.Request
{
    public class OrderVnpayRequest
    {
        public int? CustomerId { get; set; }
        public int UserId { get; set; }
        public int? promotionId { get; set; }

        public DateTime date { get; set; }

        public long totalAmount { get; set; }
        public long discountAmount { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.pending;

        public List<OrderItemRequest> listItems { get; set; }
    }
}
