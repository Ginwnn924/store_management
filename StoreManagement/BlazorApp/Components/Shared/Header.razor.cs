using BlazorApp.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components.Shared;

public partial class Header : IDisposable
{
    [Inject]
    private ICartService CartService { get; set; } = default!;

    private string Hotline { get; set; } = "1900 1234";

    // Cart Drawer state
    private bool IsCartOpen { get; set; }

    private int CartItemCount => CartService.TotalCount;

    protected override async Task OnInitializedAsync()
    {
        CartService.OnChange += HandleCartChanged;
        await CartService.InitializeAsync();
    }

    private void HandleCartChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        CartService.OnChange -= HandleCartChanged;
    }

    private void OpenCart()
    {
        IsCartOpen = true;
    }
}
