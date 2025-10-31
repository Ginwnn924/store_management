using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request;
using StoreManagement.Services;

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
    public async Task<IActionResult> GetUsers()
    {
        var response = await _userService.GetUsersAsync();
        return StatusCode(response.Status, response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var response = await _userService.GetUserByIdAsync(id);
        return StatusCode(response.Status, response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(400, StoreManagement.Response.Fail("Dữ liệu không hợp lệ", 400));
        }

        var response = await _userService.CreateUserAsync(request);
        return StatusCode(response.Status, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(400, StoreManagement.Response.Fail("Dữ liệu không hợp lệ", 400));
        }

        var response = await _userService.UpdateUserAsync(id, request);
        return StatusCode(response.Status, response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var response = await _userService.DeleteUserAsync(id);
        return StatusCode(response.Status, response);
    }
}

