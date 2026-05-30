using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.Services.DTOs
{
    
        // What the frontend RECEIVES when asking for a product
        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string NameAr { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string ImageUrl { get; set; } = string.Empty;
            public bool IsAvailable { get; set; }
            public bool IsFeatured { get; set; }
            public int CategoryId { get; set; }
            public int? CalorieCount { get; set; }
        }

        // What the frontend SENDS when creating a product (admin)
        public class CreateProductDto
        {
            public string Name { get; set; } = string.Empty;
            public string NameAr { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string DescriptionAr { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string ImageUrl { get; set; } = string.Empty;
            public int CategoryId { get; set; }
            public bool IsFeatured { get; set; } = false;
            public int DisplayOrder { get; set; } = 0;
            public int? CalorieCount { get; set; }
        }

        // What the frontend SENDS when updating a product
        public class UpdateProductDto
        {
            public string Name { get; set; } = string.Empty;
            public string NameAr { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string DescriptionAr { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string ImageUrl { get; set; } = string.Empty;
            public bool IsAvailable { get; set; }
            public bool IsFeatured { get; set; }
            public int DisplayOrder { get; set; }
            public int? CalorieCount { get; set; }
        }
    
}
