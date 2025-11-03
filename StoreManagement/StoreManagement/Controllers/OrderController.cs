using Microsoft.AspNetCore.Mvc;
using StoreManagement.Data;
using StoreManagement.DTOs.Request;
using StoreManagement.Services;

namespace StoreManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {

        _orderService = orderService;
    }


    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var response = await _orderService.GetOrders();

        // Có thể trả trực tiếp object response (ASP.NET sẽ serialize thành JSON)
        return StatusCode(response.Status, response);

    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
    {
        var response = await _orderService.CreateOrder(request);
        return StatusCode(response.Status, response);
    }
}
