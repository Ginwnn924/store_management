using Microsoft.AspNetCore.Mvc;
using StoreManagement.Data;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Services;
using StoreManagement.Services.Impl;

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
    public async Task<IActionResult> GetAllOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
    {
        var filter = new OrderFilterRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var response = await _orderService.GetAllOrdersAsync(filter);
        return StatusCode(response.Status, response);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterOrders([FromQuery] OrderFilterRequest filter)
    {
        var response = await _orderService.GetAllOrdersAsync(filter);
        return StatusCode(response.Status, response);
    }


    //[HttpGet]
    //public async Task<IActionResult> GetOrders()
    //{
    //    var response = await _orderService.GetOrders();

    //    // Có thể trả trực tiếp object response (ASP.NET sẽ serialize thành JSON)
    //    return StatusCode(response.Status, response);

    //}
}
