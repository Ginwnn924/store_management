using System.ComponentModel.DataAnnotations;

namespace StoreManagement.DTOs.Request;

    public class CustomerLoginRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

