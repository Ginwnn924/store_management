using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Response;
using StoreManagement.DTOs.Response.StatisticResponse;
using StoreManagement.Services;
using StoreManagement.Utils;
using SM = StoreManagement;
namespace StoreManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticController : ControllerBase
{
    private readonly IStatisticService _statisticService;

    public StatisticController(IStatisticService statisticService)
    {
        _statisticService = statisticService;
    }

    [HttpGet("revenue")]
    [ProducesDefaultResponseType(typeof(Response<IEnumerable<RevenueResponse>>))]
    public async Task<IActionResult> GetDailyRevenue(
            [FromQuery] int year ,
            [FromQuery] int? month,
            [FromQuery] int? day
    )
    {
        try
        {
            var result = await _statisticService.GetRevenueAsync(year, month, day);
            var response = new Response<IEnumerable<RevenueResponse>>("Get Revenue Successfully !", result);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpGet("top-seller/{top:int}")]
    [ProducesDefaultResponseType(typeof(Response<IEnumerable<TopSellerProductResponse>>))]
    public async Task<IActionResult> GetTopSellerProduct(int top = 5)
    {
        try
        {
            var result = await _statisticService.GetTopProduct(top);
            var response = new Response<IEnumerable<TopSellerProductResponse>>("Get top-seller products Successfully !", result);
            return Ok(response);
        }
        catch (Exception ex) {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }
}

