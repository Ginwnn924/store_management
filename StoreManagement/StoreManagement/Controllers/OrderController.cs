using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using StoreManagement.Data;
using StoreManagement.DTOs;
using StoreManagement.DTOs.Request;
using StoreManagement.Models;
using StoreManagement.Services;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace StoreManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly IVnpay _vnpay;
    private readonly IConfiguration _configuration;

    public OrderController(IOrderService orderService, IVnpay vnPayservice, IConfiguration configuration, ILogger<OrderController> logger, IPaymentService paymentService)
    {

        _orderService = orderService;
        _vnpay = vnPayservice;
        _configuration = configuration;


        var tmnCode = _configuration["Vnpay:TmnCode"] ?? throw new ArgumentNullException("Vnpay:TmnCode");
        var hashSecret = _configuration["Vnpay:HashSecret"] ?? throw new ArgumentNullException("Vnpay:HashSecret");
        var baseUrl = _configuration["Vnpay:BaseUrl"] ?? throw new ArgumentNullException("Vnpay:BaseUrl");
        var callbackUrl = _configuration["Vnpay:CallbackUrl"] ?? throw new ArgumentNullException("Vnpay:CallbackUrl");

        _vnpay.Initialize(tmnCode, hashSecret, baseUrl, callbackUrl);
        _paymentService = paymentService;
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
    
    [HttpPost("/api/Order/vnpay")]
    public async Task<ActionResult<string>> CreateVnpayOrder([FromBody] OrderRequest orderRequest)
    {
        try
        {
            long orderId = await _orderService.CreateOrderOnly(orderRequest); 
            var ipAddress = NetworkHelper.GetIpAddress(HttpContext);

            var paymentRequest = new PaymentRequest
            {
                PaymentId = orderId,
                Money = orderRequest.totalAmount - orderRequest.discountAmount,
                Description = "Thanh toán store_management",
                IpAddress = ipAddress,
                BankCode = BankCode.ANY,
                CreatedDate = DateTime.Now,
                Currency = Currency.VND,
                Language = DisplayLanguage.Vietnamese
            };

            var paymentUrl = _vnpay.GetPaymentUrl(paymentRequest);

            return Created(paymentUrl, paymentUrl);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    

    [HttpGet("/api/vnpay/check-result/callback")]
    public async Task<ActionResult<string>> Callback()
    {
        if (Request.QueryString.HasValue)
        {
            try
            {
                var paymentResult = _vnpay.GetPaymentResult(Request.Query);
                var resultDescription = $"{paymentResult.PaymentResponse.Description}. {paymentResult.TransactionStatus.Description}.";
                var vnpAmountStr = Request.Query["vnp_Amount"].FirstOrDefault();
                decimal vnAmountDecimal;

                if (long.TryParse(vnpAmountStr, out long vnpAmountLong))
                {
                    vnAmountDecimal = (decimal)vnpAmountLong;
                    vnAmountDecimal = vnAmountDecimal / 100;

                    Console.WriteLine($"Successfully parsed and corrected: {vnAmountDecimal}");
                }
                else
                {   
                    throw new Exception("no payment amount");
                }


                if (paymentResult.IsSuccess)
                {

                    var timestamp = paymentResult.Timestamp;
                    var orderId = paymentResult.PaymentId;
                    String paymentMethod;
                    if (paymentResult.BankingInfor.BankCode == "NCB")
                    {
                        paymentMethod = Enum.PaymentMethod.bank_transfer.ToString();
                    } else
                    {
                        paymentMethod = Enum.PaymentMethod.card.ToString();
                    }

                        Payment payment = new Payment()
                        {
                            OrderId = (int)orderId,
                            Amount = vnAmountDecimal,
                            PaymentDate = timestamp,
                            PaymentMethod = paymentMethod
                        };
                    await _paymentService.AddAsync(payment);
                    return Ok("So tien thanh toan =" + vnpAmountStr + "; OderId: " + orderId);
                }

                return BadRequest(resultDescription);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return NotFound("Không tìm thấy thông tin thanh toán.");
    }
}
