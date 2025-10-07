using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Models;

[Table("products")]
public class Product
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("supplier_id")]
    public int? SupplierId { get; set; }

    [Column("product_name")]
    [Required]
    [MaxLength(100)]
    public string ProductName { get; set; } = string.Empty;

    [Column("barcode")]
    [MaxLength(50)]
    public string? Barcode { get; set; }

    [Column("price", TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Column("unit")]
    [MaxLength(20)]
    public string Unit { get; set; } = "pcs";

    [Column("image_url")]
    [MaxLength(255)]
    public string? ImageUrl { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    [ForeignKey("SupplierId")]
    public Supplier? Supplier { get; set; }

    public Inventory? Inventory { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
