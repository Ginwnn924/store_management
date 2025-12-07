using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IOrderService
{

    Task<OrderResponse?> CreateOrderAsync(object orderRequest);
    Task GetOderById(int id);

    event Action? OnChange;
}

