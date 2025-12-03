using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Models;

[Table("categories")]
public class Category
{
    [Key]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("category_name")]
    [Required]
    [MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
