using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;

namespace StoreManagement.Services;

public interface IUserService
{
    Task<Response> GetUsersAsync();
    Task<Response> GetUserByIdAsync(int id);
    Task<Response> CreateUserAsync(UserCreateRequest request);
    Task<Response> UpdateUserAsync(int id, UserUpdateRequest request);
    Task<Response> DeleteUserAsync(int id);
}

