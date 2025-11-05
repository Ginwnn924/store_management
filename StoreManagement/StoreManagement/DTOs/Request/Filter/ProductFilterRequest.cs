namespace StoreManagement.DTOs.Request.Filter
{
    public class ProductFilterRequest : PaginationRequest
    {
        public string? ProductName { get; set; }
        public string? Barcode { get; set; }
        public List<int>? CategoryIds { get; set; }
        public List<int>? SupplierIds { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // true = chỉ lấy hàng còn, false = chỉ lấy hàng hết
        public bool? InStock { get; set; }
    }
}
