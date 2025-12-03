using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [MaxLength(100, ErrorMessage = "Họ tên không quá 100 ký tự")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [MaxLength(20, ErrorMessage = "Số điện thoại không quá 20 ký tự")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [MaxLength(100, ErrorMessage = "Email không quá 100 ký tự")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Địa chỉ không quá 500 ký tự")]
    public string? Address { get; set; }
}

