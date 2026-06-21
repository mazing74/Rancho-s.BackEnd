using Rancho_s.core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Entities
{
    public class OrderStatusHistory : BaseEntity
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public OrderStatus FromStatus { get; set; }
        public OrderStatus ToStatus { get; set; }

        // Who made this change — kitchen staff, admin, system
        public int? ChangedByUserId { get; set; }
        public AppUser? ChangedBy { get; set; }

        public string? Notes { get; set; }
    }
}
