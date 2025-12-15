using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Components.Pages;


public partial class ProductDetail
{
    [Inject]
    private IProductService ProductService { get; set; } = default!;

    [Inject]
    private ICartService CartService { get; set; } = default!;

    [Inject]
    private IToastService ToastService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public int Id { get; set; }


    protected ProductResponse? Product { get; set; }
    protected bool IsLoading { get; set; } = true;
    protected string? ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadProductAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("scrollToTop");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadProductAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            Product = await ProductService.GetProductByIdAsync(Id);

            if (Product == null)
            {
                ErrorMessage = "Sản phẩm không tồn tại hoặc đã bị xoá.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Đã xảy ra lỗi khi tải sản phẩm: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }



    private async Task AddToCartAsync()
    {
        if (Product == null || (Product.StockQuantity ?? 0) <= 0)
        {
            return;
        }

        await CartService.AddItemAsync(Product);
        ToastService.ShowSuccess($"Đã thêm {Product.ProductName} vào giỏ hàng!", "🛒 Thêm thành công");
    }

    private void GoBack()
    {
        Navigation.NavigateTo("/", forceLoad: false);
    }

    private static string FormatPrice(decimal price) => $"{price:N0}đ";

    private Task ReloadAsync() => LoadProductAsync();
}


