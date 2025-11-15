namespace StoreManagement.DTOs.Response
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        
        // Optional: Include order info if needed
        public string? OrderStatus { get; set; }
    }
}
