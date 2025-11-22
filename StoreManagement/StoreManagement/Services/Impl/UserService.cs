using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Extensions;
using StoreManagement.Mapper;
using StoreManagement.Repository;
using StoreManagement.Exceptions;

namespace StoreManagement.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserMapper _userMapper = new UserMapper();

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponse>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _userMapper.ToDtoList(users);
    }

    public async Task<PagedResponse<UserResponse>> GetUsersAsync(UserFilterRequest filter)
    {
        var query = _userRepository.GetQueryable();
        query = query.ApplyFilters(filter);
        var totalItems = await query.CountAsync();
        var users = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
        var userResponses = _userMapper.ToDtoList(users).ToList();

        var pagedResponse = new PagedResponse<UserResponse>(
            userResponses,
            totalItems,
            filter.PageNumber,
            filter.PageSize
        );
        return pagedResponse;
    }

    public async Task<UserResponse> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _userMapper.ToDto(user);
    }

    public async Task<UserResponse> CreateUserAsync(UserCreateRequest request)
    {
        var normalizedUsername = request.Username.Trim();
        var normalizedRole = request.Role.Trim().ToLowerInvariant();

        // Validate role
        if (!IsValidRole(normalizedRole))
        {
            throw new ConflictExeption("Role không hợp lệ. Chỉ chấp nhận 'admin' hoặc 'staff'");
        }

        // Check if username already exists
        var existingUser = await _userRepository.GetByUsernameAsync(normalizedUsername);
        if (existingUser != null)
        {
            throw new ConflictExeption("Username đã tồn tại");
        }

        var user = _userMapper.ToModel(request);
        var createdUser = await _userRepository.AddAsync(user);
        var response = await _userRepository.GetByIdAsync(createdUser.UserId);
        var userResponse = _userMapper.ToDto(response);
        return userResponse;
    }

    public async Task<UserResponse> UpdateUserAsync(int id, UserUpdateRequest request)
    {
        // Validate role if provided
        if (request.Role != null)
        {
            var normalizedRole = request.Role.Trim().ToLowerInvariant();
            if (!IsValidRole(normalizedRole))
            {
                throw new ConflictExeption("Role không hợp lệ. Chỉ chấp nhận 'admin' hoặc 'staff'");
            }
        }

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException("Không tìm thấy người dùng để cập nhật");
        }

        _userMapper.MapToExistingModel(request, user);
        var updatedUser = await _userRepository.UpdateAsync(user);
        var response = await _userRepository.GetByIdAsync(updatedUser.UserId);
        var userResponse = _userMapper.ToDto(response);
        return userResponse;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }

    private static bool IsValidRole(string role)
        => role == "admin" || role == "staff";
}

