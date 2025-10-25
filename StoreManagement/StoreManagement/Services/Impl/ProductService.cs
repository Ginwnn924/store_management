using StoreManagement.DTOs.Request;
using StoreManagement.Mapper;
using StoreManagement.Models;
using StoreManagement.Repository;

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

        public async Task<Response> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var productResponses = _productMapper.ToDtoList(products);
                return Response.Success(productResponses, "Products retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving products: {ex.Message}", 500);
            }
        }

        public async Task<Response> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                var productResponse = _productMapper.ToDto(product);
                return Response.Success(productResponse, "Product retrieved successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return Response.Fail(ex.Message, 404);
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving product: {ex.Message}", 500);
            }
        }

        public async Task<Response> CreateProductAsync(ProductCreateRequest request)
        {
            try
            {
                // Check if barcode already exists
                if (!string.IsNullOrEmpty(request.Barcode))
                {
                    var existingProduct = await _productRepository.GetProductByBarcodeAsync(request.Barcode);
                    if (existingProduct != null)
                    {
                        return Response.Fail("A product with this barcode already exists", 400);
                    }
                }

                var product = _productMapper.ToModel(request);
                var createdProduct = await _productRepository.AddAsync(product);
                var response = await _productRepository.GetByIdAsync(createdProduct.ProductId);
                var productResponse = _productMapper.ToDto(response);
                
                return Response.Success(productResponse, "Product created successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error creating product: {ex.Message}", 500);
            }
        }

        public async Task<Response> UpdateProductAsync(ProductUpdateRequest request,int id )
        {
            try
            {
                // Check if barcode already exists for a different product
                if (!string.IsNullOrEmpty(request.Barcode))
                {
                    var existingProduct = await _productRepository.GetProductByBarcodeAsync(request.Barcode);
                    if (existingProduct != null && existingProduct.ProductId != id)
                    {
                        return Response.Fail("A product with this barcode already exists", 400);
                    }
                }

                var product = _productMapper.ToModel(request,id);
                var updatedProduct = await _productRepository.UpdateAsync(product);
                var response = await _productRepository.GetByIdAsync(updatedProduct.ProductId);
                var productResponse = _productMapper.ToDto(response);
                
                return Response.Success(productResponse, "Product updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return Response.Fail(ex.Message, 404);
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error updating product: {ex.Message}", 500);
            }
        }

        public async Task<Response> DeleteProductAsync(int id)
        {
            try
            {
                var result = await _productRepository.DeleteAsync(id);
                if (!result)
                {
                    return Response.Fail("Product not found", 404);
                }
                
                return Response.Success(null, "Product deleted successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error deleting product: {ex.Message}", 500);
            }
        }

        public async Task<Response> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
                var productResponses = _productMapper.ToDtoList(products);
                return Response.Success(productResponses, "Products retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving products by category: {ex.Message}", 500);
            }
        }

        public async Task<Response> GetProductsBySupplierAsync(int supplierId)
        {
            try
            {
                var products = await _productRepository.GetProductsBySupplierAsync(supplierId);
                var productResponses = _productMapper.ToDtoList(products);
                return Response.Success(productResponses, "Products retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving products by supplier: {ex.Message}", 500);
            }
        }

        public async Task<Response> GetProductByBarcodeAsync(string barcode)
        {
            try
            {
                var product = await _productRepository.GetProductByBarcodeAsync(barcode);
                if (product == null)
                {
                    return Response.Fail("Product not found", 404);
                }
                
                var productResponse = _productMapper.ToDto(product);
                return Response.Success(productResponse, "Product retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error retrieving product by barcode: {ex.Message}", 500);
            }
        }

        public async Task<Response> SearchProductsByNameAsync(string name)
        {
            try
            {
                var products = await _productRepository.SearchProductsByNameAsync(name);
                var productResponses = _productMapper.ToDtoList(products);
                return Response.Success(productResponses, "Products retrieved successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail($"Error searching products: {ex.Message}", 500);
            }
        }
    }
}