using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public class OrderMapper : IMapper<Order, OrderResponse>
    {
        private readonly OrderItemMapper _orderItemMapper = new OrderItemMapper();
        public void MapToExistingModel(OrderResponse dto, Order entity)
        {
            throw new NotImplementedException();
        }

        public OrderResponse ToDto(Order entity)
        {
            OrderResponse response = new OrderResponse();
            response.Id = entity.OrderId;
            response.employeeName = entity.User.FullName;
            response.customerName = entity.Customer?.Name;
            response.totalAmount = entity.TotalAmount;
            response.discountAmount = entity.DiscountAmount;
            response.orderDate = entity.OrderDate;

            response.listItem = _orderItemMapper.ToDtoList(entity.OrderItems).ToList();



            return response;
        }

        public IEnumerable<OrderResponse> ToDtoList(IEnumerable<Order> entities)
        {
            List<OrderResponse> listOrders = new List<OrderResponse>();

            foreach (var entity in entities)
            {
                OrderResponse dto = ToDto(entity);
                listOrders.Add(dto);
                
            }
            return listOrders;
        }

        public Order ToModel(OrderResponse dto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> ToModelList(IEnumerable<OrderResponse> dtos)
        {
            throw new NotImplementedException();
        }
    }
}
