using System.ComponentModel.DataAnnotations;

namespace StoreManagement.DTOs.Request;

public class UserUpdateRequest
{
    [MaxLength(100)]
    public string? FullName { get; set; }

    [MinLength(6)]
    [MaxLength(255)]
    public string? Password { get; set; }

    [MaxLength(10)]
    public string? Role { get; set; }
}

