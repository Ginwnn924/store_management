using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Extensions;
using StoreManagement.Mapper;
using StoreManagement.Repository;

namespace StoreManagement.Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerMapper _customerMapper = new CustomerMapper();


        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Response> GetAllCustomersAsync(CustomerFilterRequest filter)
        {
            try
            {
                var query = _customerRepository.GetQueryable();
                query = query.ApplyFilters(filter);
                var totalItems = await query.CountAsync();
                var customers = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();
                var customerResponses = _customerMapper.ToDtoList(customers).ToList();
                var pagedResponse = new PagedResponse<CustomerResponse>(
                    customerResponses,
                    totalItems,
                    filter.PageNumber,
                    filter.PageSize
                );
                return Response.Success(pagedResponse, "Customers retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving customers: {ex.Message}", 500);
            }
        }

        public async Task<Response> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                var customerResponses = _customerMapper.ToDtoList(customers);
                return Response.Success(customerResponses, "Customers retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving customers: {ex.Message}", 500);
            }
        }

        public async Task<Response> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                var customerResponse = _customerMapper.ToDto(customer);
                return Response.Success(customerResponse, "Customer retrieved successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return Response.Fail(ex.Message, 404);
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving customer: {ex.Message}", 500);
            }
        }

        public async Task<Response> CreateCustomerAsync(CustomerCreateRequest request)
        {
            try
            {
                // Check if email already exists
                if (!string.IsNullOrEmpty(request.Email))
                {
                    var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(request.Email);
                    if (existingCustomer != null)
                    {
                        return Response.Fail("A customer with this email already exists", 400);
                    }
                }

                // Check if phone already exists
                if (!string.IsNullOrEmpty(request.Phone))
                {
                    var existingCustomer = await _customerRepository.GetCustomerByPhoneAsync(request.Phone);
                    if (existingCustomer != null)
                    {
                        return Response.Fail("A customer with this phone already exists", 400);
                    }
                }

                var customer = _customerMapper.ToModel(request);
                var createdCustomer = await _customerRepository.AddAsync(customer);
                var response = await _customerRepository.GetByIdAsync(createdCustomer.CustomerId);
                var customerResponse = _customerMapper.ToDto(response);

                return Response.Success(customerResponse, "Customer created successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error creating customer: {ex.Message}", 500);
            }
        }

        public async Task<Response> UpdateCustomerAsync(CustomerCreateRequest request, int id)
        {
            try
            {
                // Check if email already exists for a different customer
                if (!string.IsNullOrEmpty(request.Email))
                {
                    var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(request.Email);
                    if (existingCustomer != null && existingCustomer.CustomerId != id)
                    {
                        return Response.Fail("A customer with this email already exists", 400);
                    }
                }

                // Check if phone already exists for a different customer
                if (!string.IsNullOrEmpty(request.Phone))
                {
                    var existingCustomer = await _customerRepository.GetCustomerByPhoneAsync(request.Phone);
                    if (existingCustomer != null && existingCustomer.CustomerId != id)
                    {
                        return Response.Fail("A customer with this phone already exists", 400);
                    }
                }

                var customer = _customerMapper.ToModel(request, id);
                var updatedCustomer = await _customerRepository.UpdateAsync(customer);
                var response = await _customerRepository.GetByIdAsync(updatedCustomer.CustomerId);
                var customerResponse = _customerMapper.ToDto(response);

                return Response.Success(customerResponse, "Customer updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return Response.Fail(ex.Message, 404);
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error updating customer: {ex.Message}", 500);
            }
        }

        public async Task<Response> DeleteCustomerAsync(int id)
        {
            try
            {
                var result = await _customerRepository.DeleteAsync(id);
                if (!result)
                {
                    return Response.Fail("Customer not found", 404);
                }

                return Response.Success(null, "Customer deleted successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error deleting customer: {ex.Message}", 500);
            }
        }

        public async Task<Response> GetCustomerByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return Response.Fail("Email cannot be empty", 400);
                }

                var customer = await _customerRepository.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    return Response.Fail("Customer not found", 404);
                }

                var customerResponse = _customerMapper.ToDto(customer);
                return Response.Success(customerResponse, "Customer retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving customer by email: {ex.Message}", 500);
            }
        }

        public async Task<Response> GetCustomerByPhoneAsync(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    return Response.Fail("Phone cannot be empty", 400);
                }

                var customer = await _customerRepository.GetCustomerByPhoneAsync(phone);
                if (customer == null)
                {
                    return Response.Fail("Customer not found", 404);
                }

                var customerResponse = _customerMapper.ToDto(customer);
                return Response.Success(customerResponse, "Customer retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving customer by phone: {ex.Message}", 500);
            }
        }

        public async Task<Response> SearchCustomersByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Response.Fail("Search name cannot be empty", 400);
                }

                var customers = await _customerRepository.SearchCustomersByNameAsync(name);
                var customerResponses = _customerMapper.ToDtoList(customers);
                return Response.Success(customerResponses, "Customers retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error searching customers: {ex.Message}", 500);
            }
        }
    }
}
