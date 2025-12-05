namespace StoreManagement.DTOs.Response.StatisticResponse;
public class DailyRevenueResponse
{
    public DateTime PaymentDay { get; set; } // Ánh xạ với payment_day
    public decimal DailyRevenue { get; set; } // Ánh xạ với daily_revenue
    public int TotalOrders { get; set; } // Ánh xạ với total_orders
}

