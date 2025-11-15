using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using StoreManagement.Data;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Request;
using StoreManagement.Services;
using StoreManagement.Services.Impl;
using VNPAY.NET;
using VNPAY.NET.Utilities;
using StoreManagement.DTOs.Response;

using SM = StoreManagement;
using StoreManagement.Utils;

namespace StoreManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly IVnpay _vnpay;
    private readonly IConfiguration _configuration;
    private readonly string _callbackUrl;
    private readonly string _returnFontEndUrl;
    public OrderController(IOrderService orderService, IVnpay vnPayservice, IConfiguration configuration, ILogger<OrderController> logger, IPaymentService paymentService)
    {

        _orderService = orderService;
        _vnpay = vnPayservice;
        _configuration = configuration;


        var tmnCode = _configuration["Vnpay:TmnCode"] ?? throw new ArgumentNullException("Vnpay:TmnCode");
        var hashSecret = _configuration["Vnpay:HashSecret"] ?? throw new ArgumentNullException("Vnpay:HashSecret");
        var baseUrl = _configuration["Vnpay:BaseUrl"] ?? throw new ArgumentNullException("Vnpay:BaseUrl");
        _callbackUrl = _configuration["Vnpay:CallbackUrl"] ?? throw new ArgumentNullException("Vnpay:CallbackUrl");
        _returnFontEndUrl = _configuration["Vnpay:ReturnFontEndUrl"] ?? throw new ArgumentNullException("Vnpay:ReturnFontEndUrl");

        _vnpay.Initialize(tmnCode, hashSecret, baseUrl, _callbackUrl);
        _paymentService = paymentService;
    }

    [HttpGet]
    [ProducesDefaultResponseType(typeof(Response<PagedResponse<OrderResponse>>))]
    public async Task<IActionResult> GetAllOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
    {
        var filter = new OrderFilterRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        try
        {
            var result = await _orderService.GetAllOrdersAsync(filter);
            var response = new Response<PagedResponse<OrderResponse>>("Orders retrieved successfully", result);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpGet("filter")]
    [ProducesDefaultResponseType(typeof(Response<PagedResponse<OrderResponse>>))]
    public async Task<IActionResult> FilterOrders([FromQuery] OrderFilterRequest filter)
    {
        try
        {
            var result = await _orderService.GetAllOrdersAsync(filter);
            var response = new Response<PagedResponse<OrderResponse>>("Orders retrieved successfully", result);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }


    //[HttpGet]
    //public async Task<IActionResult> GetOrders()
    //{
    //    var response = await _orderService.GetOrders();

    //    // Có thể trả trực tiếp object response (ASP.NET sẽ serialize thành JSON)
    //    return StatusCode(response.Status, response);

    //}
    [HttpPost]
    [ProducesDefaultResponseType(typeof(Response<object>))]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
    {
        try
        {
            await _orderService.CreateOrder(request);
            return Ok(SM.Response.OnlyMessage("Order created successfully"));
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }


    [HttpPost("/api/Order/vnpay")]
    [ProducesDefaultResponseType(typeof(Response<OrderRedirectResponse>))]
    public async Task<IActionResult> CreateVnpayOrder([FromBody] OrderRequest orderRequest)
    {

        try
        {
            var ipAddress = NetworkHelper.GetIpAddress(HttpContext);

            var (orderId, paymentUrl) = await _orderService.CreateOnlyOrder(orderRequest, ipAddress);

            var redirectResponse = new OrderRedirectResponse
            {
                RedirectUrl = paymentUrl
            };

            var response = new Response<OrderRedirectResponse>("Create order successfully", redirectResponse);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpGet("/api/vnpay/check-result/callback")]
    public async Task<IActionResult> Callback()
    {
        if (!Request.QueryString.HasValue)
        {
            return NotFound("Không tìm thấy thông tin thanh toán.");
        }

        try
        {
            var paymentResult = _vnpay.GetPaymentResult(Request.Query);
            var vnpAmountStr = Request.Query["vnp_Amount"].FirstOrDefault();

            var serviceResponse = await _paymentService.ProcessVnpayCallbackAsync(paymentResult, vnpAmountStr);

            var status = serviceResponse.Status == 200 ? "success" : "fail";
            var orderId = serviceResponse.Data;
            Console.WriteLine($" status: {status} order {orderId}");
            var redirect = $"{_returnFontEndUrl}?status={status}&orderId={orderId}";

            return Redirect(redirect);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in VNPAY callback: {ex.Message}");
            return NotFound(ex.Message);
        }
    }
}
