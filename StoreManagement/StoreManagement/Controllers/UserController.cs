using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Exceptions;
using StoreManagement.Services;
using StoreManagement.Utils;
using SM = StoreManagement;

namespace StoreManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesDefaultResponseType(typeof(Response<PagedResponse<UserResponse>>))]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var filter = new UserFilterRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _userService.GetUsersAsync(filter);
            var response = new Response<PagedResponse<UserResponse>>("Get users successfully", result);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpGet("filter")]
    [ProducesDefaultResponseType(typeof(Response<PagedResponse<UserResponse>>))]
    public async Task<IActionResult> FilterUsers([FromQuery] UserFilterRequest filter)
    {
        try
        {
            var result = await _userService.GetUsersAsync(filter);
            var response = new Response<PagedResponse<UserResponse>>("Get users successfully", result);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpGet("{id:int}")]
    [ProducesDefaultResponseType(typeof(Response<UserResponse>))]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var result = await _userService.GetUserByIdAsync(id);
            var response = new Response<UserResponse>("Get user successfully", result);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(SM.Response.OnlyMessage(ex.Message));
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpPost]
    [ProducesDefaultResponseType(typeof(Response<UserResponse>))]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest request)
    {
        try
        {
            var result = await _userService.CreateUserAsync(request);
            var response = new Response<UserResponse>("Create successfully", result);

            return Ok(response);
        }
        catch (ConflictExeption ex)
        {
            return Conflict(SM.Response.OnlyMessage(ex.Message));
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpPut("{id:int}")]
    [ProducesDefaultResponseType(typeof(Response<UserResponse>))]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateRequest request)
    {
        try
        {
            var result = await _userService.UpdateUserAsync(id, request);
            var response = new Response<UserResponse>("Update successfully", result);
            
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(SM.Response.OnlyMessage(ex.Message));
        }
        catch (ConflictExeption ex)
        {
            return Conflict(SM.Response.OnlyMessage(ex.Message));
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesDefaultResponseType(typeof(Response<object>))]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound(SM.Response.OnlyMessage($"User with Id {id} not exist"));

            return Ok(SM.Response.OnlyMessage("Delete successfully"));
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }
}