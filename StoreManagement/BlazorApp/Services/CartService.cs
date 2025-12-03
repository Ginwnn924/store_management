using System.Text.Json;
using BlazorApp.Models;
using Microsoft.JSInterop;

namespace BlazorApp.Services;

public class CartService : ICartService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<CartService> _logger;
    private List<CartItem> _items = new();
    private bool _isInitialized;

    private const string StorageKey = "cart";

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public int TotalCount => _items.Count;
    public decimal TotalPrice => _items.Sum(x => x.Price * x.Quantity);

    public event Action? OnChange;

    public CartService(IJSRuntime jsRuntime, ILogger<CartService> logger)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;

        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(json))
            {
                _items = JsonSerializer.Deserialize<List<CartItem>>(json) ?? new();
                _logger.LogInformation("Loaded {Count} items from localStorage", _items.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading cart from localStorage");
            _items = new();
        }

        _isInitialized = true;
    }

    public async Task AddItemAsync(ProductResponse product, int quantity = 1)
    {
        await InitializeAsync();

        var existingItem = _items.FirstOrDefault(x => x.ProductId == product.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            _logger.LogInformation("Updated quantity for {ProductName}: {Quantity}", product.ProductName, existingItem.Quantity);
        }
        else
        {
            var newItem = new CartItem
            {
                ProductId = product.ProductId,
                Name = product.ProductName,
                Price = product.Price,
                Quantity = quantity,
                ImageUrl = product.ImageUrl ?? "",
                Unit = product.Unit,
                CategoryName = product.CategoryName
            };
            _items.Add(newItem);
            _logger.LogInformation("Added new item to cart: {ProductName}", product.ProductName);
        }

        await SaveToStorageAsync();
        NotifyStateChanged();
    }

    public async Task UpdateQuantityAsync(int productId, int quantity)
    {
        await InitializeAsync();

        var item = _items.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                _items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            await SaveToStorageAsync();
            NotifyStateChanged();
        }
    }

    public async Task RemoveItemAsync(int productId)
    {
        await InitializeAsync();

        var item = _items.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            _logger.LogInformation("Removed item from cart: {ProductName}", item.Name);

            await SaveToStorageAsync();
            NotifyStateChanged();
        }
    }

    public async Task ClearAsync()
    {
        _items.Clear();
        await SaveToStorageAsync();
        NotifyStateChanged();
        _logger.LogInformation("Cart cleared");
    }

    private async Task SaveToStorageAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_items);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving cart to localStorage");
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}

