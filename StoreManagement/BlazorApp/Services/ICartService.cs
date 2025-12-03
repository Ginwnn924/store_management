using BlazorApp.Models;

namespace BlazorApp.Services;

public interface ICartService
{
    IReadOnlyList<CartItem> Items { get; }
    int TotalCount { get; }
    decimal TotalPrice { get; }

    Task InitializeAsync();
    Task AddItemAsync(ProductResponse product, int quantity = 1);
    Task UpdateQuantityAsync(int productId, int quantity);
    Task RemoveItemAsync(int productId);
    Task ClearAsync();

    event Action? OnChange;
}

