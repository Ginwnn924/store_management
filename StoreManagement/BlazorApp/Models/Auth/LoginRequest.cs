using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "Vui lòng nhập email hoặc số điện thoại")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    public string Password { get; set; } = string.Empty;
}

