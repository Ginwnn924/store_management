using Microsoft.AspNetCore.Components;
using BlazorApp.Models;
using BlazorApp.Services;

namespace BlazorApp.Components.Shared;

/// <summary>
/// Logic layer: Chỉ chứa Parameters
/// UI logic (format, animation) nằm trong file .razor
/// </summary>
public partial class ProductCard
{
    [Inject]
    private ICartService CartService { get; set; } = default!;

    [Inject]
    private IToastService ToastService { get; set; } = default!;

    [Parameter]
    public ProductResponse? Product { get; set; }

    [Parameter]
    public double AnimationDelay { get; set; }

    private async Task AddToCartAsync()
    {
        if (Product != null)
        {
            await CartService.AddItemAsync(Product);
            ToastService.ShowSuccess($"Đã thêm {Product.ProductName} vào giỏ hàng!", "🛒 Thêm thành công");
        }
    }
}
