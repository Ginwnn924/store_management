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

    [HttpGet("daily")]
    [ProducesDefaultResponseType(typeof(Response<object>))]
    public async Task<IActionResult> GetDailyRevenue()
    {
        try
        {
            var result = await _statisticService.GetDailyRevenueAsync();
            var response = new Response<List<DailyRevenueResponse>>("Get Daily Revenue Successfully !", result);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }
}

