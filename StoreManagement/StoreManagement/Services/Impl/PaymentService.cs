using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.DTOs;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Extensions;
using StoreManagement.Models;

namespace StoreManagement.Services.Impl
{
    public class PaymentService : IPaymentService
    {
        private readonly StoreManagementDbContext _dbContext;

        public PaymentService(StoreManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> GetPaymentsAsync()
        {
            var payments = await _dbContext.Payments
                .AsNoTracking()
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentDate = p.PaymentDate
                })
                .ToListAsync();

            return Response.Success(payments);
        }

        public async Task<Response> GetPaymentsAsync(PaymentFilterRequest filter)
        {
            var query = _dbContext.Payments.AsNoTracking().AsQueryable();
            query = query.ApplyFilters(filter);

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.PaymentDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentDate = p.PaymentDate
                })
                .ToListAsync();

            var paged = new PagedResponse<PaymentDto>(items, totalItems, filter.PageNumber, filter.PageSize);
            return Response.Success(paged);
        }

        public async Task<Response> GetPaymentByIdAsync(int id)
        {
            var payment = await _dbContext.Payments
                .AsNoTracking()
                .Where(p => p.PaymentId == id)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentDate = p.PaymentDate
                })
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return Response.Fail("Không tìm thấy thanh toán", 404);
            }

            return Response.Success(payment);
        }
    }
}


