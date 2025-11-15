using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Exceptions;
using StoreManagement.Services;
using StoreManagement.Utils;
using SM = StoreManagement;

namespace StoreManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    [ProducesDefaultResponseType(typeof(Response<PagedResponse<SupplierResponse>>))]
    public async Task<IActionResult> GetAllSuppliers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var filter = new SupplierFilterRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await _supplierService.GetALlSuppliersAsync(filter);
            var response = new Response<PagedResponse<SupplierResponse>>("Get suppliers successfully", result);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpGet("filter")]
    [ProducesDefaultResponseType(typeof(Response<PagedResponse<SupplierResponse>>))]
    public async Task<IActionResult> FilterSuppliers([FromQuery] SupplierFilterRequest filter)
    {
        try
        {
            var result = await _supplierService.GetALlSuppliersAsync(filter);
            var response = new Response<PagedResponse<SupplierResponse>>("Get suppliers successfully", result);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }

    [HttpGet("{id:int}")]
    [ProducesDefaultResponseType(typeof(Response<SupplierResponse>))]
    public async Task<IActionResult> GetSupplierById(int id)
    {
        try
        {
            var result = await _supplierService.GetSupplierByIdAsync(id);
            var response = new Response<SupplierResponse>("Get supplier successfully", result);

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
    [ProducesDefaultResponseType(typeof(Response<SupplierResponse>))]
    public async Task<IActionResult> CreateSupplier([FromBody] SupplierDto supplierDto)
    {
        try
        {
            var result = await _supplierService.CreateSupplierAsync(supplierDto);
            var response = new Response<SupplierResponse>("Create supplier successfully", result);

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

    [HttpPut("{id:int}")]
    [ProducesDefaultResponseType(typeof(Response<SupplierResponse>))]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierDto supplierDto)
    {
        try
        {
            var result = await _supplierService.UpdateSupplierAsync(id, supplierDto);
            var response = new Response<SupplierResponse>("Update supplier successfully", result);

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

    [HttpDelete("{id:int}")]
    [ProducesDefaultResponseType(typeof(Response<object>))]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        try
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            if (!result)
                return NotFound($"Supplier with Id {id} not exist");

            return Ok(SM.Response.OnlyMessage("Delete supplier successfully"));
        }
        catch (Exception ex)
        {
            return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
        }
    }
}