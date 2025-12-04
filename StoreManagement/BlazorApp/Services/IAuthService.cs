using BlazorApp.Models.Auth;

namespace BlazorApp.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<CustomerResponse?> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task InitializeAsync();

    // State management
    bool IsAuthenticated { get; }
    string? CurrentUserName { get; }
    string? Token { get; }
    CustomerResponse? CurrentUser { get; }

    event Action? OnAuthStateChanged;
}

