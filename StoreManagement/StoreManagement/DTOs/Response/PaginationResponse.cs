namespace StoreManagement.DTOs.Response
{
    // Tương đương với đối tượng Page của Spring
    public class PagedResponse<T>
    {
        // Danh sách các mục trên trang hiện tại
        public List<T> Items { get; set; }

        // Thông tin phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        // Thuộc tính tính toán
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        // Constructor để dễ dàng tạo đối tượng
        public PagedResponse(List<T> items, int totalItems, int pageNumber, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    
}
