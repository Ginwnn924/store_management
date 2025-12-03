namespace BlazorApp.Models;

public class CartItem
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; } = "";
    public string? Unit { get; set; }
    public string? CategoryName { get; set; }
}
