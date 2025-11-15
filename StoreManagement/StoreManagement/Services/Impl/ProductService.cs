using Microsoft.EntityFrameworkCore;
using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Request.Filter;
using StoreManagement.DTOs.Response;
using StoreManagement.Extensions;
using StoreManagement.Mapper;
using StoreManagement.Models;
using StoreManagement.Repository;
using StoreManagement.Exceptions;
namespace StoreManagement.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductMapper _productMapper = new ProductMapper();

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PagedResponse<ProductResponse>> GetAllProductsAsync(ProductFilterRequest filter)
        {
            
                var query = _productRepository.GetQueryable();
                query = query.ApplyFilters(filter);
                var totalItems = await query.CountAsync();                
                var products = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize) 
                    .Take(filter.PageSize)
                    .ToListAsync(); 
                var productResponses = _productMapper.ToDtoList(products).ToList();

                var pagedResponse = new PagedResponse<ProductResponse>(
                    productResponses,
                    totalItems,
                    filter.PageNumber,
                    filter.PageSize
                );
            return pagedResponse;
        }


        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            
            var products = await _productRepository.GetAllAsync();
            var productResponses = _productMapper.ToDtoList(products);
            return productResponses;
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return _productMapper.ToDto(product);
        }

        public async Task<ProductResponse> CreateProductAsync(ProductCreateRequest request)
        {
            
                // Check if barcode already exists
                if (!string.IsNullOrEmpty(request.Barcode))
                {
                    var existingProduct = await _productRepository.GetProductByBarcodeAsync(request.Barcode);
                    throw new ConflictExeption("Đã tồn tại barcode này .");
                }

                var product = _productMapper.ToModel(request);
                var createdProduct = await _productRepository.AddAsync(product);
                var response = await _productRepository.GetByIdAsync(createdProduct.ProductId);
                var productResponse = _productMapper.ToDto(response);
                return productResponse;
        }

        public async Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest request,int id )
        {
           
                // Check if barcode already exists for a different product
                if (!string.IsNullOrEmpty(request.Barcode))
                {
                    var existingProduct = await _productRepository.GetProductByBarcodeAsync(request.Barcode);
                    throw new ConflictExeption("Đã tồn tại barcode này .");
                }

                var product = _productMapper.ToModel(request,id);
                if (product == null)
                {
                    throw new NotFoundException("Không tìm thấy sản phẩm để cập nhật.");
                }
                var updatedProduct = await _productRepository.UpdateAsync(product);
                var response = await _productRepository.GetByIdAsync(updatedProduct.ProductId);
                var productResponse = _productMapper.ToDto(response);

            return productResponse;
         
        }

        public async Task<bool> DeleteProductAsync(int id) { 
                return await _productRepository.DeleteAsync(id);
        }

        

        
        public async Task<ProductResponse> GetProductByBarcodeAsync(string barcode)
        {
                var product = await _productRepository.GetProductByBarcodeAsync(barcode);
                if(product == null)
                {
                    throw new NotFoundException("Không tìm thấy sản phẩm với mã vạch đã cho.");
            }
            return _productMapper.ToDto(product);
        }

        
    }
}