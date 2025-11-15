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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(Response<PagedResponse<CustomerResponse>>))]
        public async Task<IActionResult> GetAllCustomers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var filter = new CustomerFilterRequest
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                var result = await _customerService.GetAllCustomersAsync(filter);
                var response = new Response<PagedResponse<CustomerResponse>>("Customers retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        [HttpGet("filter")]
        [ProducesDefaultResponseType(typeof(Response<PagedResponse<CustomerResponse>>))]
        public async Task<IActionResult> FilterCustomer([FromQuery] CustomerFilterRequest request
            )
        {
            try
            {
                var result = await _customerService.GetAllCustomersAsync(request);
                var response = new Response<PagedResponse<CustomerResponse>>("Customers retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        /// <summary>
        /// Get customer by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(Response<CustomerResponse>))]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var result = await _customerService.GetCustomerByIdAsync(id);
                var response = new Response<CustomerResponse>("Customer retrieved successfully", result);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(SM.Response.OnlyMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        [HttpPost]
        [ProducesDefaultResponseType(typeof(Response<CustomerResponse>))]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _customerService.CreateCustomerAsync(request);
                var response = new Response<CustomerResponse>("Customer created successfully", result);

                return Ok(response);
            }
            catch (DuplicateException ex)
            {
                return BadRequest(SM.Response.OnlyMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        /// <summary>
        /// Update an existing customer
        /// </summary>
        [HttpPut("{id}")]
        [ProducesDefaultResponseType(typeof(Response<CustomerResponse>))]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _customerService.UpdateCustomerAsync(request, id);
                var response = new Response<CustomerResponse>("Customer updated successfully", result);

                return Ok(response);
            }
            catch (DuplicateException ex)
            {
                return BadRequest(SM.Response.OnlyMessage(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(SM.Response.OnlyMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }

        /// <summary>
        /// Delete a customer
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType(typeof(Response<object>))]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
                return Ok(SM.Response.OnlyMessage("Customer deleted successfully"));
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

        /// <summary>
        /// Get customer by email
        /// </summary>
        [HttpGet("email/{email}")]
        [ProducesDefaultResponseType(typeof(Response<CustomerResponse>))]
        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(SM.Response.OnlyMessage("Email cannot be empty"));
            }

            try
            {
                var result = await _customerService.GetCustomerByEmailAsync(email);
                var response = new Response<CustomerResponse>("Customer retrieved successfully", result);

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

        /// <summary>
        /// Get customer by phone
        /// </summary>
        [HttpGet("phone/{phone}")]
        [ProducesDefaultResponseType(typeof(Response<CustomerResponse>))]
        public async Task<IActionResult> GetCustomerByPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return BadRequest(SM.Response.OnlyMessage("Phone cannot be empty"));
            }

            try
            {
                var result = await _customerService.GetCustomerByPhoneAsync(phone);
                var response = new Response<CustomerResponse>("Customer retrieved successfully", result);

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

        /// <summary>
        /// Search customers by name
        /// </summary>
        [HttpGet("search")]
        [ProducesDefaultResponseType(typeof(Response<IEnumerable<CustomerResponse>>))]
        public async Task<IActionResult> SearchCustomersByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(SM.Response.OnlyMessage("Search name cannot be empty"));
            }

            try
            {
                var result = await _customerService.SearchCustomersByNameAsync(name);
                var response = new Response<IEnumerable<CustomerResponse>>("Customers retrieved successfully", result);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(SM.Response.OnlyMessage(ex.Message));
            }
        }
    }
}
