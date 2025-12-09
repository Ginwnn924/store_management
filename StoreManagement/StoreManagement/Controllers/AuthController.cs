using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Exceptions;
using StoreManagement.Services;
using StoreManagement.Utils;
using SM = StoreManagement;

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
    [ProducesDefaultResponseType(typeof(Response<AuthResponseDto>))]
    public async Task<ActionResult> Login(LoginRequest loginDto)
    {
        try
        {
            var result = await _authService.LoginAsync(loginDto);
            var response = new Response<LoginResponse>("Đăng nhập thành công", result);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(SM.Response.OnlyMessage(ex.Message));
        }
        catch (VerifyException ex)
        {
            return Unauthorized(SM.Response.OnlyMessage(ex.Message));
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }
    [HttpPost("customer-login")]
    [ProducesDefaultResponseType(typeof(Response<AuthResponseDto>))]
    public async Task<ActionResult> CustomerLogin(CustomerLoginRequest loginDto)
    {
        try
        {
            var result = await _authService.CustomerLoginAsync(loginDto);
            var response = new Response<CustomerLoginResponse>("Đăng nhập thành công", result);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(SM.Response.OnlyMessage(ex.Message));
        }
        catch (VerifyException ex)
        {
            return Unauthorized(SM.Response.OnlyMessage(ex.Message));
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpPost("logout")]
    [ProducesDefaultResponseType(typeof(Response<AuthResponseDto>))]
    [Authorize]
    public async Task<ActionResult> Logout()
    {

        try
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(SM.Response.OnlyMessage("Authorization header is missing or invalid"));
            }
            var jwtToken = authHeader.Substring("Bearer ".Length);
            await _authService.LogoutAsync(jwtToken);

            return Ok(SM.Response.OnlyMessage("Logout Successfully"));
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }
}
