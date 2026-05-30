using Rancho_s.core.Entities;
using Rancho_s.core.Interfaces;
using Rancho_s.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.Services.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IReadOnlyList<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            return products.Select(MapToDto).ToList();

        }
      public async Task<IReadOnlyList<ProductDto?>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            return products.Select(MapToDto).ToList();
        }
        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
        var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || !product.IsActive)
                return null;
            return MapToDto(product);

        }
        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto) 
        {
            // Business rule: no duplicate names
            if (await _productRepository.ProductExistsAsync(dto.Name))
                throw new Exception($"A product with the name '{dto.Name}' already exists.");

            if (dto.Price <= 0)
                throw new Exception("Price must be greater than zero.");

            var product = new Product
            {
                Name = dto.Name,
                NameAr = dto.NameAr,
                Description = dto.Description,
                DescriptionAr = dto.DescriptionAr,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                IsFeatured = dto.IsFeatured,
                DisplayOrder = dto.DisplayOrder,
                CalorieCount = dto.CalorieCount,
                IsActive = true,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return MapToDto(product);

        }
        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || !product.IsActive) return null;

            if (dto.Price <= 0)
                throw new Exception("Price must be greater than zero.");

            // Update only the fields that are allowed to change
            product.Name = dto.Name;
            product.NameAr = dto.NameAr;
            product.Description = dto.Description;
            product.DescriptionAr = dto.DescriptionAr;
            product.Price = dto.Price;
            product.ImageUrl = dto.ImageUrl;
            product.IsAvailable = dto.IsAvailable;
            product.IsFeatured = dto.IsFeatured;
            product.DisplayOrder = dto.DisplayOrder;
            product.CalorieCount = dto.CalorieCount;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            return MapToDto(product);
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || !product.IsActive) return false;

            // SOFT DELETE — never hard delete menu items
            // Why? Because orders reference products.
            // If you hard delete a product, old orders break.
            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleAvailabilityAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || !product.IsActive) return false;

            // Flip the availability
            product.IsAvailable = !product.IsAvailable;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        private static ProductDto MapToDto(Product p) => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            NameAr = p.NameAr,
            Description = p.Description,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            IsAvailable = p.IsAvailable,
            IsFeatured = p.IsFeatured,
            CategoryId = p.CategoryId,
            CalorieCount = p.CalorieCount
        };
    }
}
