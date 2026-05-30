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
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
         _categoryRepository = categoryRepository;
        }
        public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetActiveCategoriesAsync();
            return categories.Select(MapToDto).ToList();
        }
        public async Task<CategoryWithProductsDto?> GetCategoryWithProductsAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryWithProductsAsync(id);
            if (category == null) return null;

            return new CategoryWithProductsDto
            {
                Id = category.Id,
                Name = category.Name,
                NameAr = category.NameAr,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                // Map each product to its DTO
                Products = category.Products.Select(p => new ProductDto
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
                }).ToList()
            };
        }
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null || !category.IsActive) return null;
            return MapToDto(category);
        }
        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Category name is required.");

            if (await _categoryRepository.CategoryExistsAsync(dto.Name))
                throw new Exception($"A category with the name '{dto.Name}' already exists.");

            var category = new Category
            {
                Name = dto.Name,
                NameAr = dto.NameAr,
                Description = dto.Description,
                DescriptionAr = dto.DescriptionAr,
                ImageUrl = dto.ImageUrl,
                DisplayOrder = dto.DisplayOrder,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return MapToDto(category);
        }



        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null || !category.IsActive) return null;

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Category name is required.");

            category.Name = dto.Name;
            category.NameAr = dto.NameAr;
            category.Description = dto.Description;
            category.DescriptionAr = dto.DescriptionAr;
            category.ImageUrl = dto.ImageUrl;
            category.DisplayOrder = dto.DisplayOrder;
            category.IsActive = dto.IsActive;
            category.UpdatedAt = DateTime.UtcNow;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return MapToDto(category);
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null || !category.IsActive) return false;

            // Soft delete — same rule as products
            category.IsActive = false;
            category.UpdatedAt = DateTime.UtcNow;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();
            return true;
        }


        private static CategoryDto MapToDto(Category c) => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            NameAr = c.NameAr,
            Description = c.Description,
            ImageUrl = c.ImageUrl,
            DisplayOrder = c.DisplayOrder,
            // Count only active products
            ProductCount = c.Products?.Count(p => p.IsActive) ?? 0
        };
    }
}
