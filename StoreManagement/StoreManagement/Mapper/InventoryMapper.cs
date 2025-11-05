using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public static class InventoryMapper
    {
        public static Inventory ToEntity(this InventoryRequest dto)
        {
            return new Inventory
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UpdatedAt = DateTime.UtcNow
            };
        }

    
        public static InventoryResponse ToResponse(this Inventory entity)
        {
            return new InventoryResponse
            {
                InventoryId = entity.InventoryId,
                ProductId = entity.ProductId,
                ProductName =entity.Product?.ProductName ?? "",
                Quantity = entity.Quantity,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
