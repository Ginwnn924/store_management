namespace BlazorApp.Models
{
    public class OrderItemResponse
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
    }
}