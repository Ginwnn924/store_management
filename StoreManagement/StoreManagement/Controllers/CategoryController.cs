using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Response;
using StoreManagement.Services;
using StoreManagement.DTOs.Request;
using StoreManagement.Models;
using StoreManagement.DTOs.Request.Filter;

namespace StoreManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet("filter")]
        public async Task<IActionResult> FilterCategory([FromQuery] CategoryFilterRequest request)
        {
            var response = await _categoryService.FilterAsync(request);
            return StatusCode(response.Status, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var filter = new CategoryFilterRequest()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var categories = await _categoryService.FilterAsync(filter);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(new { Message = "Không tìm thấy danh mục." });

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CategoryName))
                return BadRequest(new { Message = "Tên danh mục không được để trống." });

            var category = await _categoryService.AddCategoryAsync(request.CategoryName);
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId },
                new {Message = "Thêm thành công", category });
           
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryRequest request)
        {
            var updated = await _categoryService.UpdateCategoryAsync(id, request.CategoryName);
            if (updated == null)
                return NotFound(new { Message = "Không tìm thấy danh mục cần cập nhật." });

            return Ok(new { Message = "Sửa thành công", updated });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);
            if (!deleted)
                return NotFound(new { Message = "Không tìm thấy danh mục cần xóa." });

            return Ok(new { Message = "Xóa danh mục thành công." });
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string categoryName)
        {
            var result = await _categoryService.SearchByNameAsync(categoryName);
            if (!result.Any())
                return NotFound("Không tìm thấy loại sản phẩm");
            return Ok(result);
        }
    }
}
