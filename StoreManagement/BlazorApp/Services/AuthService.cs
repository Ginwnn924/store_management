using System.Net.Http.Json;
using BlazorApp.Models;
using BlazorApp.Models.Auth;
using Microsoft.JSInterop;

namespace BlazorApp.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthService> _logger;
    private readonly IJSRuntime _jsRuntime;

    private string? _token;
    private string? _userName;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
    public string? CurrentUserName => _userName;
    public string? Token => _token;

    public event Action? OnAuthStateChanged;

    public AuthService(HttpClient httpClient, ILogger<AuthService> logger, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsRuntime = jsRuntime;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
                if (result?.Data != null)
                {
                    _token = result.Data.Token;
                    _userName = result.Data.Username;

                    // Save to localStorage
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", _token);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userName", _userName);

                    OnAuthStateChanged?.Invoke();
                    return result.Data;
                }
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                _logger.LogWarning("Login failed: {Message}", error?.Message);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return null;
        }
    }

    public async Task<CustomerResponse?> RegisterAsync(RegisterRequest request)
    {
        try
        {
            // Call Customer API to create new customer
            var customerRequest = new
            {
                Name = request.Name,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address
            };

            var response = await _httpClient.PostAsJsonAsync("api/Customer", customerRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<CustomerResponse>>();
                return result?.Data;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Register failed: {Error}", error);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                await _httpClient.PostAsync("api/auth/logout", null);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout API call");
        }
        finally
        {
            _token = null;
            _userName = null;

            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userName");

            OnAuthStateChanged?.Invoke();
        }
    }

    public async Task InitializeAsync()
    {
        try
        {
            _token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");
            _userName = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "userName");

            if (!string.IsNullOrEmpty(_token))
            {
                OnAuthStateChanged?.Invoke();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing auth state");
        }
    }
}

