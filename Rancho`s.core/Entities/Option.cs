using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Entities
{
    public class Option :BaseEntity 
    {

        // "150g", "200g", "Single", "Double", "Extra Cheese"
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;

        // How much THIS option adds to the base price
        // 150g = +0 (base price covers it)
        // 200g = +30
        // Double = +69
        public decimal AdditionalPrice { get; set; } = 0;

        // Is this the pre-selected default?
        // e.g. "150g" is the default size shown first
        public bool IsDefault { get; set; } = false;

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;

        public int OptionGroupId { get; set; }
        public OptionGroup? OptionGroup { get; set; }
    }
}
