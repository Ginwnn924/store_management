using StoreManagement.DTOs;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public class SupplierMapper : IMapper<Supplier, SupplierResponse>
    {
        public SupplierResponse ToDto(Supplier entity)
        {
            return new SupplierResponse
            {
                SupplierId = entity.SupplierId,
                Name = entity.Name,
                Phone = entity.Phone,
                Email = entity.Email,
                Address = entity.Address
            };
        }

        public Supplier ToModel(SupplierResponse dto)
        {
            return new Supplier
            {
                SupplierId = dto.SupplierId,
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };
        }

        public Supplier ToModel(SupplierDto dto)
        {
            return new Supplier
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };
        }

        public IEnumerable<SupplierResponse> ToDtoList(IEnumerable<Supplier> entities)
        {
            return entities.Select(entity => ToDto(entity)).ToList();
        }

        public IEnumerable<Supplier> ToModelList(IEnumerable<SupplierResponse> dtos)
        {
            return dtos.Select(dto => ToModel(dto)).ToList();
        }

        public void MapToExistingModel(SupplierResponse dto, Supplier entity)
        {
            entity.Name = dto.Name;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.Address = dto.Address;
        }

        public void MapToExistingModel(SupplierDto dto, Supplier entity)
        {
            entity.Name = dto.Name;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.Address = dto.Address;
        }
    }
}
