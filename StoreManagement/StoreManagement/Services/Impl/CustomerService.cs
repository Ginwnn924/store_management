using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Exceptions;
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

        public async Task<PagedResponse<CustomerResponse>> GetAllCustomersAsync(CustomerFilterRequest filter)
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

            return pagedResponse;
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            var customerResponses = _customerMapper.ToDtoList(customers);

            return customerResponses;
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            var customerResponse = _customerMapper.ToDto(customer);
            return customerResponse;
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CustomerCreateRequest request)
        {
            // Check if email already exists
            if (!string.IsNullOrEmpty(request.Email))
            {
                var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(request.Email);
                if (existingCustomer != null)
                {
                    throw new DuplicateException("A customer with this email already exists");
                }
            }

            // Check if phone already exists
            if (!string.IsNullOrEmpty(request.Phone))
            {
                var existingCustomer = await _customerRepository.GetCustomerByPhoneAsync(request.Phone);
                if (existingCustomer != null)
                {
                    throw new DuplicateException("A customer with this phone already exists");
                }
            }

            var customer = _customerMapper.ToModel(request);
            var createdCustomer = await _customerRepository.AddAsync(customer);
            var response = await _customerRepository.GetByIdAsync(createdCustomer.CustomerId);
            var customerResponse = _customerMapper.ToDto(response);

            return customerResponse;
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(CustomerCreateRequest request, int id)
        {
            // Check if email already exists for a different customer
            if (!string.IsNullOrEmpty(request.Email))
            {
                var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(request.Email);
                if (existingCustomer != null)
                {
                    throw new DuplicateException("A customer with this email already exists");
                }
            }

            // Check if phone already exists
            if (!string.IsNullOrEmpty(request.Phone))
            {
                var existingCustomer = await _customerRepository.GetCustomerByPhoneAsync(request.Phone);
                if (existingCustomer != null)
                {
                    throw new DuplicateException("A customer with this phone already exists");
                }
            }

            var customer = _customerMapper.ToModel(request, id);
            var updatedCustomer = await _customerRepository.UpdateAsync(customer);
            var response = await _customerRepository.GetByIdAsync(updatedCustomer.CustomerId);
            var customerResponse = _customerMapper.ToDto(response);

            return customerResponse;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var result = await _customerRepository.DeleteAsync(id);
            if (!result)
            {
                throw new NotFoundException("Customer not found");
            }
        }

        public async Task<CustomerResponse> GetCustomerByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(email)
                ?? throw new NotFoundException("Customer not found");

            var customerResponse = _customerMapper.ToDto(customer);
            return customerResponse;
        }

        public async Task<CustomerResponse> GetCustomerByPhoneAsync(string phone)
        {
            var customer = await _customerRepository.GetCustomerByPhoneAsync(phone)
                ?? throw new NotFoundException("Customer not found");

            var customerResponse = _customerMapper.ToDto(customer);
            return customerResponse;
        }

        public async Task<IEnumerable<CustomerResponse>> SearchCustomersByNameAsync(string name)
        {
            var customers = await _customerRepository.SearchCustomersByNameAsync(name);
            var customerResponses = _customerMapper.ToDtoList(customers);

            return customerResponses;
        }
    }
}
