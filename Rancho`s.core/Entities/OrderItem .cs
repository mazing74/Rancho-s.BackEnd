using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Entities
{
    public class OrderItem:BaseEntity
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // We store ProductId for reference
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }   // Price at time of order

        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }  // UnitPrice * Quantity

        public string? SpecialInstructions { get; set; } // "No onions"
                                                         // Selected customizations (size, extras, etc.)
        public ICollection<OrderItemOption> Options { get; set; }
            = new HashSet<OrderItemOption>();

    }
}
