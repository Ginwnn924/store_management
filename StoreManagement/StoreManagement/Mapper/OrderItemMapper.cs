using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public class OrderItemMapper : IMapper<OrderItem, OrderItemResponse>
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

        public OrderItem ToModel(OrderItemResponse dto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderItem> ToModelList(IEnumerable<OrderItemResponse> dtos)
        {
            throw new NotImplementedException();
        }
    }
}
