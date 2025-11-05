namespace StoreManagement.DTOs.Request.Filter
{
    public class SupplierFilterRequest : PaginationRequest
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
    }
}


