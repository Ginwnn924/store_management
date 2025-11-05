namespace StoreManagement.DTOs.Request.Filter
{
    public class PaymentFilterRequest : PaginationRequest
    {
        public string? PaymentMethod { get; set; }
    }
}


