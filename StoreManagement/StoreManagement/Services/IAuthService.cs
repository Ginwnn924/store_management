
using StoreManagement.DTOs;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;

namespace StoreManagement.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest dto);
    Task LogoutAsync(string jwtToken);
}
