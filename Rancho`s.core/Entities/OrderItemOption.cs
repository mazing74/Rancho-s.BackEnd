using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Entities
{
    public class OrderItemOption :  BaseEntity
    {
        public int OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }
        public int OptionId { get; set; }
        public string OptionGroupName { get; set; } = string.Empty; // "Size"
        public string OptionName { get; set; } = string.Empty;      // "Large"
        public decimal AdditionalPrice { get; set; }                // +5 EGP
    }
}
