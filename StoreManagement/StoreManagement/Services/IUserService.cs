using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.DTOs.Request.Filter;

namespace StoreManagement.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetUsersAsync();
    Task<PagedResponse<UserResponse>> GetUsersAsync(UserFilterRequest filter);
    Task<UserResponse> GetUserByIdAsync(int id);
    Task<UserResponse> CreateUserAsync(UserCreateRequest request);
    Task<UserResponse> UpdateUserAsync(int id, UserUpdateRequest request);
    Task<bool> DeleteUserAsync(int id);
}

