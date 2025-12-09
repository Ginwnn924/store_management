using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;

namespace StoreManagement.Services
{
    public interface IOrderService
    {
        Task<PagedResponse<OrderResponse>> GetAllOrdersAsync(OrderFilterRequest request);
        Task<IEnumerable<OrderResponse>> GetOrders();
        Task<OrderResponse> GetOrderById(int id);
        Task CreateOrder(OrderRequest request);
        Task<OrderResponse> CreateOrderReturnOrder(OrderRequest request);
        Task<(long OrderId, string PaymentUrl)> CreateOnlyOrder(OrderRequest request, string ipAddress);
    }
}
