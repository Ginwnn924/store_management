using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public class CustomerMapper : IMapper<Customer, CustomerResponse>
    {
        public CustomerResponse ToDto(Customer entity)
        {
            return new CustomerResponse
            {
                CustomerId = entity.CustomerId,
                Name = entity.Name,
                Phone = entity.Phone,
                Email = entity.Email,
                Address = entity.Address,
                CreatedAt = entity.CreatedAt,
                OrderCount = entity.Orders?.Count ?? 0
            };
        }

        public Customer ToModel(CustomerResponse dto)
        {
            return new Customer
            {
                CustomerId = dto.CustomerId,
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                CreatedAt = dto.CreatedAt
            };
        }

        public Customer ToModel(CustomerCreateRequest dto)
        {
            return new Customer
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                //Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            };
        }

        public Customer ToModel(CustomerRegisterRequest dto)
        {
            return new Customer
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            };
        }

        public Customer ToModel(CustomerCreateRequest dto, int id)
        {
            return new Customer
            {
                CustomerId = id,
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };
        }

        public IEnumerable<CustomerResponse> ToDtoList(IEnumerable<Customer> entities)
        {
            return entities.Select(ToDto).ToList();
        }

        public IEnumerable<Customer> ToModelList(IEnumerable<CustomerResponse> dtos)
        {
            return dtos.Select(ToModel).ToList();
        }

        public void MapToExistingModel(CustomerResponse dto, Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
