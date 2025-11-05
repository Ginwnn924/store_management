namespace StoreManagement.DTOs.Request.Filter
{
    public class OrderFilterRequest : PaginationRequest
    {
        public string? customerName { get; set; }
        public string? employeeName { get; set; }
        public decimal? minTotalAmount { get; set; }
        public decimal? maxTotalAmount { get; set; }
        public string? status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
}
