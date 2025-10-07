using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("username")]
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Column("password")]
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    [Column("full_name")]
    [MaxLength(100)]
    public string? FullName { get; set; }

    [Column("role")]
    [MaxLength(10)]
    public string Role { get; set; } = "staff";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
