namespace BlazorApp.Models;

/// <summary>
/// Category response DTO matching backend CategoryResponse
/// </summary>
public class CategoryResponse
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

