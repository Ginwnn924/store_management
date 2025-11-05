using StoreManagement.DTOs.Request;
namespace StoreManagement.DTOs.Request.Filter
{
    public class CategoryFilterRequest : PaginationRequest
    {
        public string? CategoryName { get; set; }          
        
    }
}
