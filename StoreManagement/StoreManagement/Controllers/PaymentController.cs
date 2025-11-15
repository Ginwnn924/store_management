using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Exceptions;
using StoreManagement.Services;
using StoreManagement.Utils;
using SM = StoreManagement;

namespace StoreManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    [ProducesDefaultResponseType(typeof(Response<PagedResponse<PaymentResponse>>))
]
    public async Task<IActionResult> GetAllPayments(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var filter = new PaymentFilterRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var response = await _paymentService.GetAllPaymentsAsync(filter);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpGet("filter")]
    [ProducesDefaultResponseType(typeof(Response<PagedResponse<PaymentResponse>>))
]
    public async Task<IActionResult> FilterPayments([FromQuery] PaymentFilterRequest filter)
    {
        try
        {
            var response = await _paymentService.GetAllPaymentsAsync(filter);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpGet("{id:int}")]
    [ProducesDefaultResponseType(typeof(Response<PaymentResponse>))]
    public async Task<IActionResult> GetPaymentById(int id)
    {
        try
        {
            var response = await _paymentService.GetPaymentByIdAsync(id);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }
}


