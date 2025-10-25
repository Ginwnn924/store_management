using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public class ProductMapper : IMapper<Product, ProductResponse>
    {
        public ProductResponse ToDto(Product entity)
        {
            return new ProductResponse
            {
                ProductId = entity.ProductId,
                ProductName = entity.ProductName,
                //CategoryId = entity.CategoryId,
                CategoryName = entity.Category?.CategoryName,
                //SupplierId = entity.SupplierId,
                SupplierName = entity.Supplier?.Name,
                Barcode = entity.Barcode,
                Price = entity.Price,
                Unit = entity.Unit,
                ImageUrl = entity.ImageUrl,
                CreatedAt = entity.CreatedAt,
                StockQuantity = entity.Inventory?.Quantity,
                
            };
        }

        public Product ToModel(ProductResponse dto)
        {
            return new Product
            {
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                //CategoryId = dto.CategoryId,
                //SupplierId = dto.SupplierId,
                Barcode = dto.Barcode,
                Price = dto.Price,
                Unit = dto.Unit,
                ImageUrl = dto.ImageUrl,
                CreatedAt = dto.CreatedAt
            };
        }

        public Product ToModel(ProductCreateRequest dto)
        {
            return new Product
            {
                ProductName = dto.ProductName,
                CategoryId = dto.CategoryId,
                SupplierId = dto.SupplierId,
                Barcode = dto.Barcode,
                Price = dto.Price,
                Unit = dto.Unit,
                ImageUrl = dto.ImageUrl
            };
        }

        public Product ToModel(ProductUpdateRequest dto,int id)
        {
            return new Product
            {
                ProductId = id,
                ProductName = dto.ProductName,
                CategoryId = dto.CategoryId,
                SupplierId = dto.SupplierId,
                Barcode = dto.Barcode,
                Price = dto.Price,
                Unit = dto.Unit,
                ImageUrl = dto.ImageUrl
            };
        }

        public IEnumerable<ProductResponse> ToDtoList(IEnumerable<Product> entities)
        {
            return entities.Select(ToDto).ToList();
        }

        public IEnumerable<Product> ToModelList(IEnumerable<ProductResponse> dtos)
        {
            return dtos.Select(ToModel).ToList();
        }

        public void MapToExistingModel(ProductResponse dto, Product entity)
        {
            throw new NotImplementedException();
        }
    }
}