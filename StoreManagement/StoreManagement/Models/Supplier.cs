using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Models;

[Table("suppliers")]
public class Supplier
{
    [Key]
    [Column("supplier_id")]
    public int SupplierId { get; set; }

    [Column("name")]
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("phone")]
    [MaxLength(20)]
    public string? Phone { get; set; }

    [Column("email")]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();

    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
