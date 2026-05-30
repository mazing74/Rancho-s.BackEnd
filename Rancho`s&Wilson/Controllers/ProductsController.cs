using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rancho_s.Services.DTOs;
using Rancho_s.Services.Services;

namespace Rancho_s_Wilson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }


        // ─────────────────────────────────────────
        // GET /api/products
        // Public — anyone can browse the menu
        // ─────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        // ─────────────────────────────────────────
        // GET /api/products/5
        // Public — get one product's full details
        // ─────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { Message = $"Product with id {id} was not found." });

            return Ok(product);
        }
        // ─────────────────────────────────────────
        // GET /api/products/category/3
        // Public — get all products in a category
        // ─────────────────────────────────────────
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);

        }
        // ─────────────────────────────────────────
        // POST /api/products
        // Admin only — create new product
        // ─────────────────────────────────────────
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody]CreateProductDto dto)
        {
            try
            {
                var created = await _productService.CreateProductAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // ─────────────────────────────────────────
        // PUT /api/products/5
        // Admin only — update a product fully
        // ─────────────────────────────────────────
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductDto dto)
        {
            try
            {
                var updated = await _productService.UpdateProductAsync(id, dto);

                if (updated == null)
                    return NotFound(new { Message = $"Product with id {id} was not found." });

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // ─────────────────────────────────────────
        // DELETE /api/products/5
        // Admin only — soft delete a product
        // ─────────────────────────────────────────
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
                return NotFound(new { Message = $"Product with id {id} was not found." });

            return NoContent(); // 204 — success, nothing to return
        }

        // ─────────────────────────────────────────
        // PATCH /api/products/5/toggle-availability
        // Kitchen/Admin — mark item as available or sold out
        // ─────────────────────────────────────────
        [HttpPatch("{id}/toggle-availability")]
        public async Task<IActionResult> ToggleAvailability(int id)
        {
            var result = await _productService.ToggleAvailabilityAsync(id);

            if (!result)
                return NotFound(new { Message = $"Product with id {id} was not found." });

            return Ok(new { Message = "Availability updated successfully." });
        }

    }
}
