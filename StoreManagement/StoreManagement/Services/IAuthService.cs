
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;

namespace StoreManagement.Services;

public interface IAuthService
{
    Task<Response> loginAsync(LoginRequest dto);
    Task<Response> logoutAsync(String jwtToken);
}
