using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.DTOs.Request.Filter;

namespace StoreManagement.Services;

public interface IUserService
{
    Task<Response> GetUsersAsync();
    Task<Response> GetUsersAsync(UserFilterRequest filter);
    Task<Response> GetUserByIdAsync(int id);
    Task<Response> CreateUserAsync(UserCreateRequest request);
    Task<Response> UpdateUserAsync(int id, UserUpdateRequest request);
    Task<Response> DeleteUserAsync(int id);
}

