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


    // Trong StatisticRepository.cs

    public async Task<IEnumerable<RevenueResponse>> GetMonthlyRevenueAsync(int month, int year)
    {
        return await _dbContext.RevenueResponses
            .FromSqlInterpolated($@"SELECT
            CAST(DATE_FORMAT(p.payment_date, '%Y-%m') AS CHAR(7)) AS PaymentDate,
            SUM(p.amount) AS Revenue,
            COUNT(DISTINCT p.order_id) AS TotalSold
        FROM payments p
        JOIN orders o ON p.order_id = o.order_id
        WHERE
            o.status = 'paid'
            AND o.is_deleted = 0
            AND p.is_deleted = 0
            AND MONTH(p.payment_date) = {month}   
            AND YEAR(p.payment_date) = {year}     
        GROUP BY
            PaymentDate                          
        ORDER BY
            PaymentDate ASC")
            .ToListAsync();
    }

    public async Task<IEnumerable<RevenueResponse>> GetYearlyRevenueAsync(int year)
    {
        return await _dbContext.RevenueResponses
            .FromSqlInterpolated($@"SELECT
            DATE_FORMAT(p.payment_date, '%Y-%m') AS PaymentDate,
            SUM(p.amount) AS Revenue,
            COUNT(DISTINCT p.order_id) AS TotalSold
        FROM payments p
        JOIN orders o ON p.order_id = o.order_id
        WHERE
            o.status = 'paid'
            AND o.is_deleted = 0
            AND p.is_deleted = 0
            AND YEAR(p.payment_date) = {year} -- Chỉ lọc theo năm
        GROUP BY
            PaymentDate
        ORDER BY
            PaymentDate ASC")
            .ToListAsync();
    }

    // Trong StatisticRepository.cs

    public async Task<RevenueResponse?> GetDailyRevenueByDateAsync(int year, int month, int day)
    {
        var specificDate = new DateTime(year, month, day);
        string dateString = specificDate.ToString("yyyy-MM-dd");
        return await _dbContext.RevenueResponses
            .FromSqlInterpolated($@"SELECT
                CAST(DATE(p.payment_date) AS CHAR(10)) AS PaymentDate,   
                SUM(p.amount) AS Revenue,          
                COUNT(DISTINCT p.order_id) AS TotalSold
            FROM
                payments p
            JOIN
                orders o ON p.order_id = o.order_id
            WHERE
                o.status = 'paid'
                AND o.is_deleted = 0
                AND p.is_deleted = 0
                AND DATE(p.payment_date) = {dateString}
            GROUP BY
                PaymentDate
            ")
            .FirstOrDefaultAsync(); 
    }

    public async Task<IEnumerable<TopSellerProductResponse>> GetTopProduct(int top)
    {
        return await _dbContext.TopSellerProductResponses
         
         .FromSqlInterpolated($@"SELECT 
             p.product_id AS ProductId,
             p.product_name AS ProductName, 
             COUNT(oi.product_id) AS TotalSold
           FROM products p
           JOIN order_items oi ON p.product_id = oi.product_id
           GROUP BY p.product_id, p.product_name 
           ORDER BY TotalSold DESC
           LIMIT {top}") 
         .ToListAsync();

    }
}

