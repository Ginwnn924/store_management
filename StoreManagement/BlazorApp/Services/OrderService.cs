using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public event Action? OnChange;

        public async Task<OrderResponse?> CreateOrderAsync(object orderRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/order/create-order-with-data-response", orderRequest);
            if (response.IsSuccessStatusCode)
            {
                var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<OrderResponse>>();
                return apiResult?.Data;
            }
            return null;
        }

        public Task GetOderById(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class ApiResponse<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
