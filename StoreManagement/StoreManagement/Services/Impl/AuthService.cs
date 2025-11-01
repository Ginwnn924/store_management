using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Data;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreManagement.Services.Impl;

public class AuthService : IAuthService
{
    private readonly StoreManagementDbContext _context;
    private readonly IConfiguration _configuration;
    public AuthService(StoreManagementDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<Response> loginAsync(LoginRequest loginRequest)
    {
        User user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

        if (user == null)
        {
            return Response.Fail("Không tìm thấy người dùng", 404);
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        {
            return Response.Fail("Invalid username or password", 401);
        }

        var token = GenerateJwtToken(user);
        return Response.Success(new LoginResponse { Token = token, Username = user.Username, Role = user.Role }, "Đăng nhập thành công");
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

