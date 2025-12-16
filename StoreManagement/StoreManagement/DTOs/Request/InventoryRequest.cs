namespace StoreManagement.DTOs.Request
{
    public class InventoryRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public int SupplierId { get; set; }
    }
}
