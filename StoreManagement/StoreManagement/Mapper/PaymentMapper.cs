using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public class PaymentMapper : IMapper<Payment, PaymentResponse>
    {
        public PaymentResponse ToDto(Payment entity)
        {
            return new PaymentResponse
            {
                PaymentId = entity.PaymentId,
                OrderId = entity.OrderId,
                Amount = entity.Amount,
                PaymentMethod = entity.PaymentMethod,
                PaymentDate = entity.PaymentDate,
                OrderStatus = entity.Order?.Status
            };
        }

        public Payment ToModel(PaymentResponse dto)
        {
            return new Payment
            {
                PaymentId = dto.PaymentId,
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                PaymentDate = dto.PaymentDate
            };
        }

        public IEnumerable<PaymentResponse> ToDtoList(IEnumerable<Payment> entities)
        {
            return entities.Select(entity => ToDto(entity)).ToList();
        }

        public IEnumerable<Payment> ToModelList(IEnumerable<PaymentResponse> dtos)
        {
            return dtos.Select(dto => ToModel(dto)).ToList();
        }

        public void MapToExistingModel(PaymentResponse dto, Payment entity)
        {
            entity.OrderId = dto.OrderId;
            entity.Amount = dto.Amount;
            entity.PaymentMethod = dto.PaymentMethod;
            entity.PaymentDate = dto.PaymentDate;
        }
    }
}
