using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;
using StoreManagement.Extensions;

namespace StoreManagement.Services.Impl;

public class UserService : IUserService
{
    private readonly StoreManagementDbContext _dbContext;

    public UserService(StoreManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response> GetUsersAsync()
    {
        var users = await _dbContext.Users.AsNoTracking().ToListAsync();
        var data = users.Select(MapToResponse).ToList();
        return Response.Success(data);
    }

    public async Task<Response> GetUsersAsync(UserFilterRequest filter)
    {
        var query = _dbContext.Users.AsNoTracking().AsQueryable();
        query = query.ApplyFilters(filter);

        var totalItems = await query.CountAsync();
        var users = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var items = users.Select(MapToResponse).ToList();
        var paged = new PagedResponse<UserResponse>(items, totalItems, filter.PageNumber, filter.PageSize);
        return Response.Success(paged);
    }

    public async Task<Response> GetUserByIdAsync(int id)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null)
        {
            return Response.Fail("Không tìm thấy người dùng", 404);
        }

        return Response.Success(MapToResponse(user));
    }

    public async Task<Response> CreateUserAsync(UserCreateRequest request)
    {
        var normalizedUsername = request.Username.Trim();
        var normalizedRole = request.Role.Trim().ToLowerInvariant();

        if (!IsValidRole(normalizedRole))
        {
            return Response.Fail("Role không hợp lệ. Chỉ chấp nhận 'admin' hoặc 'staff'", 400);
        }

        var existed = await _dbContext.Users.AnyAsync(u => u.Username == normalizedUsername);
        if (existed)
        {
            return Response.Fail("Username đã tồn tại", 400);
        }

        var user = new User
        {
            Username = normalizedUsername,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FullName = request.FullName?.Trim(),
            Role = normalizedRole,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return new Response(201, "Tạo người dùng thành công", MapToResponse(user));
    }

    public async Task<Response> UpdateUserAsync(int id, UserUpdateRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null)
        {
            return Response.Fail("Không tìm thấy người dùng", 404);
        }

        if (request.Role != null)
        {
            var normalizedRole = request.Role.Trim().ToLowerInvariant();
            if (!IsValidRole(normalizedRole))
            {
                return Response.Fail("Role không hợp lệ. Chỉ chấp nhận 'admin' hoặc 'staff'", 400);
            }
            user.Role = normalizedRole;
        }

        if (request.FullName != null)
        {
            user.FullName = request.FullName.Trim();
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        await _dbContext.SaveChangesAsync();

        return Response.Success(MapToResponse(user), "Cập nhật người dùng thành công");
    }

    public async Task<Response> DeleteUserAsync(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null)
        {
            return Response.Fail("Không tìm thấy người dùng", 404);
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return Response.Success(null!, "Xóa người dùng thành công");
    }

    private static bool IsValidRole(string role)
        => role == "admin" || role == "staff";

    private static UserResponse MapToResponse(User user) => new()
    {
        UserId = user.UserId,
        Username = user.Username,
        FullName = user.FullName,
        Role = user.Role,
        CreatedAt = user.CreatedAt
    };
}

