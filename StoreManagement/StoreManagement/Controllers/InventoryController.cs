using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Services;
using StoreManagement.Utils;

using SM = StoreManagement;

namespace StoreManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("filter")]
        [ProducesDefaultResponseType(typeof(Response<PagedResponse<InventoryResponse>>))]
        public async Task<IActionResult> FilterInventories([FromQuery] InventoryFilterRequest request)
        {
            try
            {
                var result = await _inventoryService.GetAllInventoryAsync(request);
                var response = new Response<PagedResponse<InventoryResponse>>("Lấy danh sách tồn kho thành công", result);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(Response<PagedResponse<InventoryResponse>>))]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var filter = new InventoryFilterRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            try
            {
                var result = await _inventoryService.GetAllInventoryAsync(filter);
                var response = new Response<PagedResponse<InventoryResponse>>("Lấy danh sách tồn kho thành công", result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(Response<InventoryResponse>))]
        public async Task<IActionResult> Create([FromBody] InventoryRequest request)
        {
            try
            {
                var created = await _inventoryService.AddAsync(request);
                var response = new Response<InventoryResponse>("Create inventory successfully", created);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(Response<InventoryResponse>))]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _inventoryService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(SM.Response.OnlyMessage("Không tìm thấy tồn kho"));

                var response = new Response<InventoryResponse>("Get successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesDefaultResponseType(typeof(Response<InventoryResponse>))]
        public async Task<IActionResult> Update(int id, [FromBody] InventoryRequest request)
        {
            try
            {
                var updated = await _inventoryService.UpdateAsync(id, request);
                if (updated == null)
                    return NotFound(SM.Response.OnlyMessage("Không tìm thấy tồn kho"));

                var response = new Response<InventoryResponse>("Update successfully", updated);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesDefaultResponseType(typeof(Response<object>))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _inventoryService.DeleteAsync(id);
                if (!success)
                    return NotFound(SM.Response.OnlyMessage("Không tìm thấy tồn kho để xóa"));

                return Ok(SM.Response.OnlyMessage("Xóa tồn kho thành công"));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpGet("search")]
        [ProducesDefaultResponseType(typeof(Response<IEnumerable<InventoryResponse>>))]
        public async Task<IActionResult> SearchByProductName([FromQuery] string productName)
        {
            var result = await _inventoryService.SearchByProductNameAsync(productName);
            if (!result.Any())
                return NotFound(SM.Response.OnlyMessage("Không tìm thấy sản phẩm nào phù hợp."));

            var response = new Response<IEnumerable<InventoryResponse>>("Get successfully", result);
            return Ok(response);
        }
    }
}