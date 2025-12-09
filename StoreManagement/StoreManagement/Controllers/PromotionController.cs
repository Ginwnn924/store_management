using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Services;
using StoreManagement.Utils;

using SM = StoreManagement;
namespace StoreManagement.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase
    {
        
      
            private readonly IPromotionService _promotionService ;

        public PromotionController(IPromotionService service)
        {
            _promotionService = service;
        }

        [HttpGet]
            [ProducesDefaultResponseType(typeof(Response<IEnumerable<PromotionResponse>>))]
            public async Task<IActionResult> GetAllProducts(
                [FromQuery] long minOrderAmount = 0
                )
            {
                try
                {
                    var result = await _promotionService.GetPromotions(minOrderAmount);
                    var response = new Response<IEnumerable<PromotionResponse>>("Get promotion successfully", result);

                return Ok(response);
            }
                catch (Exception ex)
                {
                    return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
                }
            }
        }
}
