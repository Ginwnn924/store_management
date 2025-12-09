namespace StoreManagement.DTOs.Response.StatisticResponse;
public class RevenueResponse
{
    public string PaymentDate { get; set; } = string.Empty; 
    public decimal Revenue { get; set; } 
    public int TotalSold { get; set; } 
}

