using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public class OrderItemMapper
    {
        public void MapToExistingModel(OrderItemResponse dto, OrderItem entity)
        {
            throw new NotImplementedException();
        }

        public OrderItemResponse ToDto(OrderItem entity)
        {
            OrderItemResponse orderItemResponse = new OrderItemResponse();
            orderItemResponse.Id = entity.ProductId;
            orderItemResponse.productName = entity.Product?.ProductName;
            orderItemResponse.quantity = entity.Quantity;
            orderItemResponse.price = entity.Price;
            orderItemResponse.subTotal = entity.Subtotal;
            
            return orderItemResponse;
        }

        public IEnumerable<OrderItemResponse> ToDtoList(IEnumerable<OrderItem> entities)
        {
            List<OrderItemResponse> listOrderItems = new List<OrderItemResponse>();
            foreach (var entity in entities)
            {
                OrderItemResponse dto = ToDto(entity);
                listOrderItems.Add(dto);

            }
            return listOrderItems;
        }

        public OrderItem ToModel(OrderItemRequest dto)
        {
            return new OrderItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Subtotal = dto.Price * dto.Quantity
            };
        }

        public IEnumerable<OrderItem> ToModelList(IEnumerable<OrderItemRequest> dtos)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (OrderItemRequest dto in dtos) 
            {
                OrderItem item = ToModel(dto);
                orderItems.Add(item);
            }
            return orderItems;
        }
    }
}
