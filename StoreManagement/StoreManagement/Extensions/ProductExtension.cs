using Microsoft.EntityFrameworkCore; // Cần thiết cho EF.Functions.Like
using StoreManagement.Models;
// Giả sử bạn tạo DTO Request trong thư mục DTOs/Request
using StoreManagement.DTOs.Request.Filter;
using System.Linq;

namespace StoreManagement.Extensions
{
    public static class ProductExtension
    {
        /// <summary>
        /// Áp dụng các bộ lọc động cho truy vấn IQueryable<Product>.
        /// </summary>
        /// <param name="query">Truy vấn IQueryable gốc.</param>
        /// <param name="filter">Đối tượng chứa các tham số lọc.</param>
        /// <returns>Truy vấn IQueryable đã được thêm các điều kiện WHERE.</returns>
        public static IQueryable<Product> ApplyFilters(
            this IQueryable<Product> query,
            ProductFilterRequest filter)
        {
            // 1. Lọc theo Tên sản phẩm (tìm kiếm mờ, tương đương LIKE)
            if (!string.IsNullOrWhiteSpace(filter.ProductName))
            {
                string searchTerm = $"%{filter.ProductName.Trim()}%";
                query = query.Where(p => EF.Functions.Like(p.ProductName, searchTerm));
            }

            // 2. Lọc theo Barcode (tìm kiếm chính xác)
            if (!string.IsNullOrWhiteSpace(filter.Barcode))
            {
                query = query.Where(p => p.Barcode == filter.Barcode.Trim());
            }

            // 3. Lọc theo danh sách CategoryId (tương đương IN)
            // Model của bạn cho phép CategoryId là nullable (int?), 
            // nên chúng ta cần kiểm tra p.CategoryId.HasValue
            if (filter.CategoryIds != null && filter.CategoryIds.Any())
            {
                query = query.Where(p => p.CategoryId.HasValue &&
                                         filter.CategoryIds.Contains(p.CategoryId.Value));
            }

            // 4. Lọc theo danh sách SupplierId (tương đương IN)
            if (filter.SupplierIds != null && filter.SupplierIds.Any())
            {
                query = query.Where(p => p.SupplierId.HasValue &&
                                         filter.SupplierIds.Contains(p.SupplierId.Value));
            }

            // 5. Lọc theo Khoảng giá (Price Range)
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                // Thêm điều kiện MaxPrice phải lớn hơn MinPrice nếu cả hai đều được cung cấp
                if (filter.MinPrice.HasValue && filter.MaxPrice.Value < filter.MinPrice.Value)
                {
                    // Bỏ qua MaxPrice nếu nó vô lý
                }
                else
                {
                    query = query.Where(p => p.Price <= filter.MaxPrice.Value);
                }
            }

            // 6. Lọc theo Tình trạng tồn kho (dựa trên navigation property 'Inventory')
            // Giả sử Inventory có thuộc tính 'Quantity' (int)
            if (filter.InStock.HasValue)
            {
                if (filter.InStock.Value)
                {
                    // Chỉ lấy sản phẩm có tồn kho (Inventory.Quantity > 0)
                    query = query.Where(p => p.Inventory != null && p.Inventory.Quantity > 0);
                }
                else
                {
                    // Chỉ lấy sản phẩm đã hết hàng (Không có Inventory hoặc Quantity <= 0)
                    query = query.Where(p => p.Inventory == null || p.Inventory.Quantity <= 0);
                }
            }

            return query;
        }
    }
}