using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.DTOs;
using StoreManagement.Models;

namespace StoreManagement.Services.Impl
{
    public class SupplierService : ISupplierService
    {
        private readonly StoreManagementDbContext _dbContext;

        public SupplierService(StoreManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> GetSuppliersAsync()
        {
            var suppliers = await _dbContext.Suppliers.AsNoTracking().ToListAsync();
            return Response.Success(suppliers);
        }

        public async Task<Response> GetSupplierByIdAsync(int id)
        {
            var supplier = await _dbContext.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.SupplierId == id);
            if (supplier == null)
            {
                return Response.Fail("Không tìm thấy nhà cung cấp", 404);
            }
            return Response.Success(supplier);
        }

        public async Task<Response> CreateSupplierAsync(SupplierDto supplierDto)
        {
            var supplier = new Supplier
            {
                Name = supplierDto.Name,
                Phone = supplierDto.Phone,
                Email = supplierDto.Email,
                Address = supplierDto.Address
            };

            await _dbContext.Suppliers.AddAsync(supplier);
            await _dbContext.SaveChangesAsync();
            return new Response(201, "Tạo nhà cung cấp thành công", supplier);
        }

        public async Task<Response> UpdateSupplierAsync(int id, SupplierDto supplierDto)
        {
            var existing = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == id);
            if (existing == null)
            {
                return Response.Fail("Không tìm thấy nhà cung cấp", 404);
            }

            existing.Name = supplierDto.Name;
            existing.Phone = supplierDto.Phone;
            existing.Email = supplierDto.Email;
            existing.Address = supplierDto.Address;

            await _dbContext.SaveChangesAsync();
            return Response.Success(existing, "Cập nhật nhà cung cấp thành công");
        }

        public async Task<Response> DeleteSupplierAsync(int id)
        {
            var existing = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == id);
            if (existing == null)
            {
                return Response.Fail("Không tìm thấy nhà cung cấp", 404);
            }

            _dbContext.Suppliers.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return Response.Success(null!, "Xóa nhà cung cấp thành công");
        }
    }
}