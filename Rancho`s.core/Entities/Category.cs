using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Entities
{
    public class Category : BaseEntity
    {
    
        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        // The image shown on the menu tab (e.g. a burger icon for "Burgers")
        public string ImageUrl { get; set; } = string.Empty;

        // Controls the order of tabs on the menu page
        // Burgers = 1, Meals = 2, Sides = 3, Drinks = 4
        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;
        // Navigation property — EF Core uses this to do JOINs automatically
        // This is how you say "one Category HAS MANY Products"
        public ICollection<Product> Products { get; set; } = new HashSet<Product>(); 
    }
}
