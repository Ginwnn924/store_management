using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Data;
using StoreManagement.DTOs;
using StoreManagement.DTOs.Request;
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
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login( LoginRequest loginDto)
    {   
        var response = await _authService.loginAsync( loginDto);
        return StatusCode(response.Status, response);
    }
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {

        var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return BadRequest(new { message = "Authorization header is missing or invalid" });
        }
        var jwtToken = authHeader.Substring("Bearer ".Length);

        var response = await _authService.logoutAsync(jwtToken);
        return StatusCode(response.Status, response);
    }
}
