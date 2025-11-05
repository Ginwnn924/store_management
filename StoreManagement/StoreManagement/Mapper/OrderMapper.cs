using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public class OrderMapper
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

        public Order ToModel(OrderRequest dto)
        {
            return new Order
            {
                CustomerId = dto.CustomerId,
                UserId = dto.UserId,
                PromoId = dto.promotionId,
                OrderDate = dto.date,
                TotalAmount = dto.totalAmount,
                DiscountAmount = dto.discountAmount,
                Status = dto.OrderStatus.ToString(),
                OrderItems = _orderItemMapper
                                .ToModelList(dto.listItems)
                                .ToList()

            };

        }

        public IEnumerable<Order> ToModelList(IEnumerable<OrderResponse> dtos)
        {
            throw new NotImplementedException();
        }
    }
}
