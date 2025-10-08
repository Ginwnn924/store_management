using Microsoft.AspNetCore.Mvc;
using StoreManagement.Data;
using StoreManagement.Services;

namespace StoreManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : Controller
{
    private readonly StoreManagementDbContext _context;
    private readonly IOrderService _orderService;

    public OrderController(StoreManagementDbContext context,
                            IOrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }


    [HttpGet]
    public async Task<IActionResult> GetPayments()
    {
        // Placeholder for getting payments logic
        return await _orderService.GetOrders();
    }
}
