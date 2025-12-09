using System;
using System.Collections.Generic;

namespace BlazorApp.Models
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? EmployeeName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemResponse> ListItem { get; set; } = new();
    }
}
