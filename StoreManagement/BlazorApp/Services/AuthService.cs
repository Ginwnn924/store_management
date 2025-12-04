using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlazorApp.Models;
using BlazorApp.Models.Auth;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;

namespace BlazorApp.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthService> _logger;
    private readonly IJSRuntime _jsRuntime;

    private string? _token;
    private string? _email;
    private string? _userName;
    private CustomerResponse? _currentUser;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
    public string? CurrentUserName => _userName;
    public string? Token => _token;
    public CustomerResponse? CurrentUser => _currentUser;

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
            var response = await _httpClient.PostAsJsonAsync("api/auth/customer-login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
                if (result?.Data != null)
                {
                    _token = result.Data.Token;
                    _email = result.Data.Email;

                    // Save to localStorage
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", _token);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "Email", _email);

                    // Set Authorization header
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                    // Load user info
                    await LoadCurrentUserAsync();

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
            // Map BlazorApp RegisterRequest sang DTO ð? g?i API
            // API endpoint expects: { name, phone, email, address, password }
            var customerRequest = new
            {
                name = request.Name,
                phone = request.Phone,
                email = request.Email,
                address = request.Address,
                password = request.Password
            };

            var response = await _httpClient.PostAsJsonAsync("api/customer/register", customerRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<CustomerResponse>>();
                if (result?.Data != null)
                {
                    _logger.LogInformation("Registration successful for email: {Email}", request.Email);
                    return result.Data;
                }
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                _logger.LogWarning("Register failed: {Message}", error?.Message);
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
            _currentUser = null;

            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "Email");

            _httpClient.DefaultRequestHeaders.Authorization = null;

            OnAuthStateChanged?.Invoke();
        }
    }

    public async Task InitializeAsync()
    {
        try
        {
            _token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");
            _email = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "Email");

            if (!string.IsNullOrEmpty(_token))
            {
                // Set Authorization header
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                // Load user info
                await LoadCurrentUserAsync();

                // Trigger event to notify UI
                OnAuthStateChanged?.Invoke();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing auth state");
        }
    }

    private async Task LoadCurrentUserAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_token))
                return;

            // Decode JWT to get customer ID or email
            var customerId = GetCustomerIdFromToken(_token);

            if (customerId.HasValue)
            {
                try
                {
                    // Call API to get customer info
                    var response = await _httpClient.GetFromJsonAsync<ApiResponse<CustomerResponse>>(
                        $"api/Customer/{customerId.Value}");

                    if (response?.Data != null)
                    {
                        _currentUser = response.Data;
                        _userName = _currentUser.Name; // Update username with full name
                        _logger.LogInformation("Successfully loaded customer info: {CustomerName}", _currentUser.Name);
                        return;
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogWarning(ex, "Failed to load customer by ID {CustomerId}, trying by email...", customerId);
                }
            }

            // Fallback: try to get by email
            var email = GetEmailFromToken(_token);
            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<ApiResponse<CustomerResponse>>(
                        $"api/Customer/email/{Uri.EscapeDataString(email)}");

                    if (response?.Data != null)
                    {
                        _currentUser = response.Data;
                        _userName = _currentUser.Name;
                        _logger.LogInformation("Successfully loaded customer info by email: {CustomerName}", _currentUser.Name);
                        return;
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogWarning(ex, "Failed to load customer by email {Email}", email);
                }
            }

            _logger.LogWarning("Could not load customer info from either customer ID or email");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading current user");
        }
    }

    private int? GetCustomerIdFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Try to get customer ID from claims
            // Backend s? d?ng ClaimTypes.NameIdentifier nên s? có key "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            var customerIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type == "sub" ||
                c.Type == "customerId" ||
                c.Type == "customer_id");

            if (customerIdClaim != null && int.TryParse(customerIdClaim.Value, out int customerId))
            {
                _logger.LogInformation("Extracted customer ID from token: {CustomerId}", customerId);
                return customerId;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error decoding JWT token");
        }

        return null;
    }

    private string? GetEmailFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Backend s? d?ng ClaimTypes.Email nên s? có key "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
            var emailClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Email ||
                c.Type == "email" ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

            if (emailClaim != null)
            {
                _logger.LogInformation("Extracted email from token: {Email}", emailClaim.Value);
                return emailClaim.Value;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting email from token");
        }

        return null;
    }
}

