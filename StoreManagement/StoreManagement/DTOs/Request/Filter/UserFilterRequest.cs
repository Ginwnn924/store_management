namespace StoreManagement.DTOs.Request.Filter
{
    public class UserFilterRequest : PaginationRequest
    {
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
}


