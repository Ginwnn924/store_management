using Microsoft.AspNetCore.Mvc;

namespace StoreManagement.Services.Impl
{
    public class OrderService : IOrderService
    {
        public Task<IActionResult> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GetOrders()
        {
            return new OkObjectResult("GetOrders from OrderService");
        }
    }
}
