using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "Vui lòng nhập email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    public string Password { get; set; } = string.Empty;
}

