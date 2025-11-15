using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Extensions;
using StoreManagement.Mapper;
using StoreManagement.Repository;
using StoreManagement.Exceptions;

namespace StoreManagement.Services.Impl
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly SupplierMapper _supplierMapper = new SupplierMapper();

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<SupplierResponse>> GetAllSuppliersAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return _supplierMapper.ToDtoList(suppliers);
        }

        public async Task<PagedResponse<SupplierResponse>> GetALlSuppliersAsync(SupplierFilterRequest filter)
        {
            var query = _supplierRepository.GetQueryable();
            query = query.ApplyFilters(filter);
            var totalItems = await query.CountAsync();
            var suppliers = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var supplierResponses = _supplierMapper.ToDtoList(suppliers).ToList();

            var pagedResponse = new PagedResponse<SupplierResponse>(
                supplierResponses,
                totalItems,
                filter.PageNumber,
                filter.PageSize
            );
            return pagedResponse;
        }

        public async Task<SupplierResponse> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            return _supplierMapper.ToDto(supplier);
        }

        public async Task<SupplierResponse> CreateSupplierAsync(SupplierDto supplierDto)
        {
            // Check if email already exists
            if (!string.IsNullOrEmpty(supplierDto.Email))
            {
                var existingSupplier = await _supplierRepository.GetSupplierByEmailAsync(supplierDto.Email);
                if (existingSupplier != null)
                {
                    throw new ConflictExeption("Đã tồn tại email này.");
                }
            }

            // Check if phone already exists
            if (!string.IsNullOrEmpty(supplierDto.Phone))
            {
                var existingSupplier = await _supplierRepository.GetSupplierByPhoneAsync(supplierDto.Phone);
                if (existingSupplier != null)
                {
                    throw new ConflictExeption("Đã tồn tại số điện thoại này.");
                }
            }

            var supplier = _supplierMapper.ToModel(supplierDto);
            var createdSupplier = await _supplierRepository.AddAsync(supplier);
            var response = await _supplierRepository.GetByIdAsync(createdSupplier.SupplierId);
            var supplierResponse = _supplierMapper.ToDto(response);
            return supplierResponse;
        }

        public async Task<SupplierResponse> UpdateSupplierAsync(int id, SupplierDto supplierDto)
        {
            // Check if email already exists for a different supplier
            if (!string.IsNullOrEmpty(supplierDto.Email))
            {
                var existingSupplier = await _supplierRepository.GetSupplierByEmailAsync(supplierDto.Email);
                if (existingSupplier != null && existingSupplier.SupplierId != id)
                {
                    throw new ConflictExeption("Đã tồn tại email này.");
                }
            }

            // Check if phone already exists for a different supplier
            if (!string.IsNullOrEmpty(supplierDto.Phone))
            {
                var existingSupplier = await _supplierRepository.GetSupplierByPhoneAsync(supplierDto.Phone);
                if (existingSupplier != null && existingSupplier.SupplierId != id)
                {
                    throw new ConflictExeption("Đã tồn tại số điện thoại này.");
                }
            }

            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
            {
                throw new NotFoundException("Không tìm thấy nhà cung cấp để cập nhật.");
            }

            _supplierMapper.MapToExistingModel(supplierDto, supplier);
            var updatedSupplier = await _supplierRepository.UpdateAsync(supplier);
            var response = await _supplierRepository.GetByIdAsync(updatedSupplier.SupplierId);
            var supplierResponse = _supplierMapper.ToDto(response);
            return supplierResponse;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            return await _supplierRepository.DeleteAsync(id);
        }
    }
}