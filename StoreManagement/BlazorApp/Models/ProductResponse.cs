namespace BlazorApp.Models;

/// <summary>
/// Product response DTO matching backend ProductResponse
/// </summary>
public class ProductResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public string? SupplierName { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public string Unit { get; set; } = "pcs";
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? StockQuantity { get; set; }
}

