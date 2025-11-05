using System.ComponentModel.DataAnnotations;

namespace StoreManagement.DTOs.Request;

public class UserCreateRequest
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FullName { get; set; }

    [Required]
    public string Role { get; set; } = "staff";
}

