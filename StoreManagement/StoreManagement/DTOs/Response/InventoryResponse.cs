namespace StoreManagement.DTOs.Response
{
    public class InventoryResponse
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
