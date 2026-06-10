using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rancho_s.Services.DTOs;
using Rancho_s.Services.Services;

namespace Rancho_s_Wilson.Controllers
{
 
    public class CategoriesController : APiBaseController
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ─────────────────────────────────────────────────────
        // GET /api/categories
        // Public — returns all active categories (menu tabs)
        // ─────────────────────────────────────────────────────
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // ─────────────────────────────────────────────────────
        // GET /api/categories/3
        // Public — get one category info (no products)
        // ─────────────────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
                return NotFound(new { Message = $"Category with id {id} was not found." });

            return Ok(category);
        }

        // ─────────────────────────────────────────────────────
        // GET /api/categories/3/products
        // Public — THE main menu endpoint
        // Customer clicks "Burgers" tab → calls this → gets all burgers
        // ─────────────────────────────────────────────────────
        [HttpGet("{id}/products")]
        public async Task<ActionResult<CategoryWithProductsDto>> GetWithProducts(int id)
        {
            var result = await _categoryService.GetCategoryWithProductsAsync(id);

            if (result == null)
                return NotFound(new { Message = $"Category with id {id} was not found." });

            return Ok(result);
        }

        // ─────────────────────────────────────────────────────
        // POST /api/categories
        // Admin only — create a new menu section
        // ─────────────────────────────────────────────────────
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
        {
            try
            {
                var created = await _categoryService.CreateCategoryAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // ─────────────────────────────────────────────────────
        // PUT /api/categories/3
        // Admin only — update category info
        // ─────────────────────────────────────────────────────
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            try
            {
                var updated = await _categoryService.UpdateCategoryAsync(id, dto);

                if (updated == null)
                    return NotFound(new { Message = $"Category with id {id} was not found." });

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // ─────────────────────────────────────────────────────
        // DELETE /api/categories/3
        // Admin only — soft delete a category
        // ─────────────────────────────────────────────────────
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);

            if (!result)
                return NotFound(new { Message = $"Category with id {id} was not found." });

            return NoContent();
        }
    }
}
