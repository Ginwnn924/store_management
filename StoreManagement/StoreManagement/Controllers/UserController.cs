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
            var response = await _userService.GetUsersAsync(filter);
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
            var response = await _userService.GetUsersAsync(filter);
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
            var response = await _userService.GetUserByIdAsync(id);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
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
            var response = await _userService.CreateUserAsync(request);
            return Ok(response);
        }
        catch (ConflictExeption ex)
        {
            return Conflict(ex.Message);
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
            var response = await _userService.UpdateUserAsync(id, request);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictExeption ex)
        {
            return Conflict(ex.Message);
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
            var response = await _userService.DeleteUserAsync(id);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }
}

