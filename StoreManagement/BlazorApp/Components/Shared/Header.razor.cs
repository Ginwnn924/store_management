using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Components.Shared;

public partial class Header : IDisposable
{
    [Inject]
    private ICartService CartService { get; set; } = default!;

    [Inject]
    private IAuthService AuthService { get; set; } = default!;

    [Inject]
    private ILogger<Header> Logger { get; set; } = default!;

    private string Hotline { get; set; } = "1900 1234";

    // Cart Drawer state
    private bool IsCartOpen { get; set; }

    private int CartItemCount => CartService.TotalCount;

    protected override async Task OnInitializedAsync()
    {
        CartService.OnChange += HandleCartChanged;
        AuthService.OnAuthStateChanged += HandleAuthStateChanged;

        await CartService.InitializeAsync();
        await AuthService.InitializeAsync();
        
        Logger.LogInformation("Header initialized - IsAuthenticated: {IsAuth}, CurrentUser: {CurrentUser}", 
            AuthService.IsAuthenticated, 
            AuthService.CurrentUser?.Name ?? "null");
    }

    private void HandleCartChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    private void HandleAuthStateChanged()
    {
        Logger.LogInformation("Auth state changed - IsAuthenticated: {IsAuth}, CurrentUser: {CurrentUser}", 
            AuthService.IsAuthenticated, 
            AuthService.CurrentUser?.Name ?? "null");
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        CartService.OnChange -= HandleCartChanged;
        AuthService.OnAuthStateChanged -= HandleAuthStateChanged;
    }

    private void OpenCart()
    {
        IsCartOpen = true;
    }

    /// <summary>
    /// L?y tên cu?i cùng t? tên ð?y ð?
    /// VD: "Nguy?n V? Trung Hýng" => "Hýng"
    /// </summary>
    private string GetLastName(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return "B?n";

        // Trim whitespace và split theo space
        var parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        
        // L?y ph?n t? cu?i cùng
        if (parts.Length > 0)
            return parts[^1]; // C# 8.0+ index from end
        
        return "B?n";
    }
}
