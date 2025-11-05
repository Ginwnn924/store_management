using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request;
namespace StoreManagement.DTOs.Request.Filter
{
    public class InventoryFilterRequest : PaginationRequest

    {
        public string? ProductName { get; set; }
       
    }
}
