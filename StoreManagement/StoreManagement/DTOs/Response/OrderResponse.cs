using Microsoft.AspNetCore.Mvc;

namespace StoreManagement.DTOs.Response
{
    public class OrderResponse 
    {
        public int Id { get; set; }
        public string customerName { get; set; }
        public string employeeName { get; set; }
        public decimal totalAmount { get; set; }
        public decimal discountAmount { get; set; }
        public string status { get; set; }
        public DateTime orderDate { get; set; }
        public List<OrderItemResponse> listItem { get; set; }
}
}
