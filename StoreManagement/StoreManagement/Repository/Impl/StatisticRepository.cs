using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.DTOs.Response.StatisticResponse;
using StoreManagement.Models;

namespace StoreManagement.Repository.Impl;

public class StatisticRepository : IStatisticRepository
{
    private readonly StoreManagementDbContext _dbContext;
    
    public StatisticRepository(StoreManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<DailyRevenueResponse>> GetDailyRevenueAsync()
    {
        var sqlQuery = @"
        SELECT
            DATE(p.payment_date) AS PaymentDay,   
            SUM(p.amount) AS DailyRevenue,       
            COUNT(DISTINCT p.order_id) AS TotalOrders
        FROM
            payments p
        JOIN
            orders o ON p.order_id = o.order_id
        WHERE
            o.status = 'paid' 
            AND o.is_deleted = 0 
            AND p.is_deleted = 0 
        GROUP BY
            PaymentDay
        ORDER BY
            PaymentDay DESC;
        ";

        // Sử dụng FromSqlRaw để ánh xạ kết quả vào DTO
        return await _dbContext.DailyRevenueResponses
            .FromSqlRaw(sqlQuery)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetTopProduct()
    {
        string sqlQuery = 
            @"
                        SELECT p.* FROM products p 
                        JOIN order_items oi ON p.product_id = oi.product_id 
                        GROUP BY p.product_id 
                        ORDER BY COUNT(*) DESC 
                        LIMIT 4"")
                    
            ";
        return await _dbContext.Products
            .FromSqlRaw(sqlQuery)
            .Include(p => p.Category)
            .Include(p => p.Inventory)
            .ToListAsync();
            
    }
}

