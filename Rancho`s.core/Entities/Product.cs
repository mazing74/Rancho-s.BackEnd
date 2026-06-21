using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Entities
{
    public class Product : BaseEntity
    {
        // Basic info
        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        // Pricing
        public decimal Price { get; set; }

        // Image - we store the URL/path to the image, not the image itself
        // Never store images as binary in SQL Server - that kills performance
        public string ImageUrl { get; set; } = string.Empty;

        // Control flags - these are how you "soft delete" and manage visibility
        // IsActive = false means deleted (we never hard delete menu items)
        // IsAvailable = false means "sold out today" but still exists
        public bool IsActive { get; set; } = true;
        public bool IsAvailable { get; set; } = true;
        public bool IsFeatured { get; set; } = false;   // Show on homepage highlights

        // Controls the ORDER items appear in the menu (not alphabetical)
        // Restaurant owner wants Burgers before Salads in the list
        public int DisplayOrder { get; set; } = 0;

        // Calories - optional but professional
        public int? CalorieCount { get; set; }

        // Foreign Key - every product belongs to a category
        // We'll build Category next session, for now just the FK is enough
        public int CategoryId { get; set; }
        // This is how you say "one Product BELONGS TO one Category"
        public Category? Category { get; set; }

        // A product can have multiple option groups
        // Classic Burger has: "Size"
        // Tender Box has: "Flavor" + "Sauce Choice"
        public ICollection<OptionGroup> OptionGroups { get; set; }
            = new List<OptionGroup>();
    }
}
