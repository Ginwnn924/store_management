using System.ComponentModel.DataAnnotations;

namespace StoreManagement.DTOs.Request
{
    public class ProductCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        public int? SupplierId { get; set; }

        [MaxLength(50)]
        public string? Barcode { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [MaxLength(20)]
        public string Unit { get; set; } = "pcs";

        [MaxLength(255)]
        public string? ImageUrl { get; set; }
    }
}