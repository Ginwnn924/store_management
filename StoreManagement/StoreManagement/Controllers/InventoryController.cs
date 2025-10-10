using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Services;

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

     
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _inventoryService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _inventoryService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy tồn kho" });

            return Ok(result);
        }

        
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _inventoryService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = "Không tìm thấy tồn kho để xóa" });

            return Ok(new { message = "Xóa tồn kho thành công" });
        }
    }
}
