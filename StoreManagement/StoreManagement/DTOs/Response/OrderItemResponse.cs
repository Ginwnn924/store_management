using Microsoft.AspNetCore.Mvc;

namespace StoreManagement.DTOs.Response
{
    public class OrderItemResponse
    {
        public int Id { get; set; }
        public string productName { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public decimal subTotal { get; set; }
    }
}
