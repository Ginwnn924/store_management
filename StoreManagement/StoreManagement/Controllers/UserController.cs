using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StoreManagement.Data;
using StoreManagement.Models;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly StoreManagementDbContext _dbContext;

    public UserController(StoreManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);
        var user = _dbContext.Users.Find(userId);

        if (user == null)
            return NotFound();

        // You may want to use a DTO instead of returning the entity directly
        return Ok(new
        {
            user.UserId,
            user.Username,
            user.FullName
        });
    }
    //[Authorize] don't add this if don't want to protect this route
    [HttpGet("unprotectedInfo")]
    public IActionResult GetUnProtectedUserInfo()
    {
        return Ok(new
        {
            a = "la con ga !!!",
            id = 101
        });
    }
}