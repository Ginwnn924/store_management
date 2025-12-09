namespace BlazorApp.Models;

public class OrderRequest
{
    public int? CustomerId { get; set; }
    public int UserId { get; set; }
    public int? promotionId { get; set; }
    public DateTime date { get; set; }
    public int PaymentMethod { get; set; }
    public long totalAmount { get; set; }
    public long discountAmount { get; set; }
    public int OrderStatus { get; set; }
    public List<OrderItemRequest> listItems { get; set; } = new();
}

public class OrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
