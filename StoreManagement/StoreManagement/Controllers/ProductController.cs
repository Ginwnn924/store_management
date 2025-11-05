using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Services;

namespace StoreManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var filter = new ProductFilterRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var response = await _productService.GetAllProductsAsync(filter);
            return StatusCode(response.Status, response); 
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterProducts([FromQuery] ProductFilterRequest filter)
        {
            var response = await _productService.GetAllProductsAsync(filter);
            return StatusCode(response.Status, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return StatusCode(response.Status, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _productService.CreateProductAsync(request);
            return StatusCode(response.Status, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _productService.UpdateProductAsync(request,id);
            return StatusCode(response.Status, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            return StatusCode(response.Status, response);
        }
  
        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetProductByBarcode(string barcode)
        {
            var response = await _productService.GetProductByBarcodeAsync(barcode);
            return StatusCode(response.Status, response);
        }

        
    }
}