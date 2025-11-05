using System.ComponentModel.DataAnnotations;

namespace StoreManagement.DTOs
{
    public class SupplierDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }
    }
}