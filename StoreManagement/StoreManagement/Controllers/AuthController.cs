using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Data;
using StoreManagement.DTOs;
using StoreManagement.Models;
using StoreManagement.Services;
using StoreManagement.Services.Impl;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreManagement.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly StoreManagementDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IRedisCacheService _redisCacheService;
    public AuthController(StoreManagementDbContext context, IConfiguration configuration, IRedisCacheService redisCacheService)
    {
        _context = context;
        _configuration = configuration;
        _redisCacheService = redisCacheService;
    }

    [HttpPost("register/test")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
        {
            return BadRequest(new { message = "Username already exists" });
        }

        if (registerDto.Role != "admin" && registerDto.Role != "staff")
        {
            return BadRequest(new { message = "Invalid role. Must be 'admin' or 'staff'" });
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Username = registerDto.Username,
            Password = hashedPassword,
            FullName = registerDto.FullName,
            Role = registerDto.Role,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var token = GenerateJwtToken(user);

        string cacheKey = "jwt:" + token ;
        await _redisCacheService.SetCacheAsync(cacheKey, "active", TimeSpan.FromDays(2));

        return Ok(new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        });
    }
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<Object>> Logout()
    {

        var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return BadRequest(new { message = "Authorization header is missing or invalid" });
        }
        var jwtToken = authHeader.Substring("Bearer ".Length);


        string cacheKey = "jwt:" + jwtToken;
        await _redisCacheService.RemoveCacheAsync(cacheKey);

        return Ok(new
        {
            message = "Loggout successfully",
            status = "sucess"  
        });
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
