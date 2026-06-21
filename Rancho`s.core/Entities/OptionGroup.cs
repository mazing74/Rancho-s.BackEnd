using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Entities
{
    public class OptionGroup :BaseEntity
    {
        // "Size", "Single or Double", "Sauce Choice", "Extras"
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;

        // single   = customer picks EXACTLY ONE (e.g. size)
        // multiple = customer picks ONE OR MORE (e.g. extra toppings)
        public string SelectionType { get; set; } = "single";

        // Must customer choose? Size = yes. Extra toppings = no.
        public bool IsRequired { get; set; } = true;

        public int DisplayOrder { get; set; } = 0;

        // Which product this group belongs to
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        // The options inside this group
        public ICollection<Option> Options { get; set; } = new List<Option>();
    }
}
