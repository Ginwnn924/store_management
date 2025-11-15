using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Data;
using StoreManagement.DTOs;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Exceptions;
using StoreManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreManagement.Services.Impl;

public class AuthService : IAuthService
{
    private readonly StoreManagementDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IRedisCacheService _redisCacheService;
    public AuthService(StoreManagementDbContext context, IConfiguration configuration, IRedisCacheService redisCacheService)
    {
        _context = context;
        _configuration = configuration;
        _redisCacheService = redisCacheService;
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username)
            ?? throw new NotFoundException("Không tìm thấy người dùng");

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        {
            throw new VerifyException("Invalid username or password"); // can have better way than the exception
        }

        var token = GenerateJwtToken(user);
        string cacheKey = "jwt:" + token;
        await _redisCacheService.SetCacheAsync(cacheKey, "active", TimeSpan.FromDays(2));

        return new LoginResponse { Token = token, Username = user.Username, Role = user.Role };
    }
    public async Task LogoutAsync(string jwtToken)
    {

        string cacheKey = "jwt:" + jwtToken;
        await _redisCacheService.RemoveCacheAsync(cacheKey);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "StoreManagement";
        var audience = jwtSettings["Audience"] ?? "StoreManagementAPI";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

