using Microsoft.AspNetCore.Mvc;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.Services;

namespace StoreManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var filter = new CustomerFilterRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var response = await _customerService.GetAllCustomersAsync(filter);
            return StatusCode(response.Status, response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterCustomer([FromQuery] CustomerFilterRequest request
            )
        {
            var response = await _customerService.GetAllCustomersAsync(request);
            return StatusCode(response.Status, response);
        }

        /// <summary>
        /// Get customer by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);
            return StatusCode(response.Status, response);
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _customerService.CreateCustomerAsync(request);
            return StatusCode(response.Status, response);
        }

        /// <summary>
        /// Update an existing customer
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _customerService.UpdateCustomerAsync(request, id);
            return StatusCode(response.Status, response);
        }

        /// <summary>
        /// Delete a customer
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var response = await _customerService.DeleteCustomerAsync(id);
            return StatusCode(response.Status, response);
        }

        /// <summary>
        /// Get customer by email
        /// </summary>
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            var response = await _customerService.GetCustomerByEmailAsync(email);
            return StatusCode(response.Status, response);
        }

        /// <summary>
        /// Get customer by phone
        /// </summary>
        [HttpGet("phone/{phone}")]
        public async Task<IActionResult> GetCustomerByPhone(string phone)
        {
            var response = await _customerService.GetCustomerByPhoneAsync(phone);
            return StatusCode(response.Status, response);
        }

        /// <summary>
        /// Search customers by name
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomersByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search name cannot be empty");
            }

            var response = await _customerService.SearchCustomersByNameAsync(name);
            return StatusCode(response.Status, response);
        }
    }
}
