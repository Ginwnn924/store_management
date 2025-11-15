using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Response;
using StoreManagement.Services;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;

using SM = StoreManagement;
using StoreManagement.Utils;

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
        [ProducesDefaultResponseType(typeof(Response<PagedResponse<CategoryResponse>>))]
        public async Task<IActionResult> Filter([FromQuery] CategoryFilterRequest request)
        {
            try
            {
                var result = await _categoryService.FilterAsync(request);
                var response = new Response<PagedResponse<CategoryResponse>>("Lấy danh sách danh mục thành công",
                                                                             result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(Response<PagedResponse<CategoryResponse>>))]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var filter = new CategoryFilterRequest()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            try
            {
                var categories = await _categoryService.FilterAsync(filter);
                var response = new Response<PagedResponse<CategoryResponse>>("Lấy danh sách danh mục thành công",
                                                                             categories);


                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(Response<CategoryResponse>))]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound(SM.Response.OnlyMessage("Không tìm thấy danh mục."));
                }

                var response = new Response<CategoryResponse>("Lấy danh mục thành công", category);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(Response<CategoryResponse>))]
        public async Task<IActionResult> Create([FromBody] CategoryRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.CategoryName))
                    return BadRequest(SM.Response.OnlyMessage("Tên danh mục không được để trống."));

                var category = await _categoryService.AddCategoryAsync(request.CategoryName);
                var response = new Response<CategoryResponse>("Thêm thành công", category);

                return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesDefaultResponseType(typeof(Response<CategoryResponse>))]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryRequest request)
        {
            try
            {
                var updated = await _categoryService.UpdateCategoryAsync(id, request.CategoryName);
                if (updated == null)
                    return NotFound(SM.Response.OnlyMessage("Không tìm thấy danh mục cần cập nhật."));

                var response = new Response<CategoryResponse>("Sửa thành công", updated);
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
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result)
                    return NotFound(SM.Response.OnlyMessage("Không tìm thấy danh mục cần xóa."));

                var response = SM.Response.OnlyMessage("Xóa danh mục thành công.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
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
