namespace StoreManagement.DTOs.Response.StatisticResponse;

public class TopSellerProductResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int TotalSold { get; set; }
}

