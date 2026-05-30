using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.Services.DTOs
{

    
        // What the frontend receives when listing categories (simple, no products)
        public class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string NameAr { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
            public int DisplayOrder { get; set; }
            public int ProductCount { get; set; }  // Useful for admin dashboard
        }

        // What the frontend receives when viewing ONE category with its products
        // Used for the menu page: click "Burgers" → see all burgers
        public class CategoryWithProductsDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string NameAr { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
            public List<ProductDto> Products { get; set; } = new();
        }

        // What the admin sends when creating a category
        public class CreateCategoryDto
        {
            public string Name { get; set; } = string.Empty;
            public string NameAr { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string DescriptionAr { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
            public int DisplayOrder { get; set; } = 0;
        }

        // What the admin sends when updating a category
        public class UpdateCategoryDto
        {
            public string Name { get; set; } = string.Empty;
            public string NameAr { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string DescriptionAr { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
            public int DisplayOrder { get; set; }
            public bool IsActive { get; set; }
        }
    
}
