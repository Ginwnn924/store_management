using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs;
using StoreManagement.Services;
using StoreManagement;

namespace StoreManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSuppliers()
    {
        var response = await _supplierService.GetSuppliersAsync();
        return StatusCode(response.Status, response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSupplierById(int id)
    {
        var response = await _supplierService.GetSupplierByIdAsync(id);
        return StatusCode(response.Status, response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody] SupplierDto supplierDto)
    {
        if (!ModelState.IsValid)
            return StatusCode(400, StoreManagement.Response.Fail("Dữ liệu không hợp lệ", 400));

        var response = await _supplierService.CreateSupplierAsync(supplierDto);
        return StatusCode(response.Status, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierDto supplierDto)
    {
        if (!ModelState.IsValid)
            return StatusCode(400, StoreManagement.Response.Fail("Dữ liệu không hợp lệ", 400));

        var response = await _supplierService.UpdateSupplierAsync(id, supplierDto);
        return StatusCode(response.Status, response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var response = await _supplierService.DeleteSupplierAsync(id);
        return StatusCode(response.Status, response);
    }
}