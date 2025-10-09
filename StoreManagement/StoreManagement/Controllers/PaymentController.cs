using Microsoft.AspNetCore.Mvc;
using StoreManagement.Services;

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
    public async Task<IActionResult> GetPayments()
    {
        var response = await _paymentService.GetPaymentsAsync();
        return StatusCode(response.Status, response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPaymentById(int id)
    {
        var response = await _paymentService.GetPaymentByIdAsync(id);
        return StatusCode(response.Status, response);
    }
}


