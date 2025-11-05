using Microsoft.AspNetCore.Mvc;
using StoreManagement.Services;
using StoreManagement.DTOs.Request.Filter;

namespace StoreManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPayments(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var filter = new PaymentFilterRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var response = await _paymentService.GetPaymentsAsync(filter);
        return StatusCode(response.Status, response);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterPayments([FromQuery] PaymentFilterRequest request)
    {
        var response = await _paymentService.GetPaymentsAsync(request);
        return StatusCode(response.Status, response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPaymentById(int id)
    {
        var response = await _paymentService.GetPaymentByIdAsync(id);
        return StatusCode(response.Status, response);
    }
}


