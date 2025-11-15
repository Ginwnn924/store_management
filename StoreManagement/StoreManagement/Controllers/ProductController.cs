using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Exceptions;
using StoreManagement.Services;
using StoreManagement.Utils;

using SM = StoreManagement;

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
        [ProducesDefaultResponseType(typeof(Response<PagedResponse<ProductResponse>>))]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var filter = new ProductFilterRequest
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                var result = await _productService.GetAllProductsAsync(filter);
                var response = new Response<PagedResponse<ProductResponse>>("Get products successfully", result);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpGet("filter")]
        [ProducesDefaultResponseType(typeof(Response<PagedResponse<ProductResponse>>))]
        public async Task<IActionResult> FilterProducts([FromQuery] ProductFilterRequest filter)
        {
            try
            {
                var result = await _productService.GetAllProductsAsync(filter);
                var response = new Response<PagedResponse<ProductResponse>>("Get products successfully", result);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(Response<ProductResponse>))]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var result = await _productService.GetProductByIdAsync(id);
                var response = new Response<ProductResponse>("Get product successfully", result);
                
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(SM.Response.OnlyMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(Response<ProductResponse>))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest request)
        {
            try
            {
                var result = await _productService.CreateProductAsync(request);
                var response = new Response<ProductResponse>("Create successfully", result);

                return Ok(response);
            }
            catch (ConflictExeption ex)
            {
                return Conflict(SM.Response.OnlyMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesDefaultResponseType(typeof(Response<ProductResponse>))]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequest request)
        {
            try
            {
                var result = await _productService.UpdateProductAsync(request, id);
                var response = new Response<ProductResponse>("Update successfully", result);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(SM.Response.OnlyMessage(ex.Message));
            }
            catch (ConflictExeption ex)
            {
                return Conflict(SM.Response.OnlyMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }

        }

        [HttpDelete("{id}")]
        [ProducesDefaultResponseType(typeof(Response<object>))]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound(SM.Response.OnlyMessage($"Product with Id {id} not exist"));
                }

                return Ok(SM.Response.OnlyMessage("Delete product successfully"));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpGet("barcode/{barcode}")]
        [ProducesDefaultResponseType(typeof(Response<ProductResponse>))]
        public async Task<IActionResult> GetProductByBarcode(string barcode)
        {
            try
            {
                var result = await _productService.GetProductByBarcodeAsync(barcode);
                var response = new Response<ProductResponse>("Get products successfully", result);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(SM.Response.OnlyMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }
    }
}