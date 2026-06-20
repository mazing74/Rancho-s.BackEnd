using Rancho_s.core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Entities
{
    public class Order :BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;


        // Who placed this order
        // Nullable because we might allow guest orders later
        public int? CustomerId { get; set; }
        public AppUser? Customer { get; set; }

        // Which branch received this order
        public int BranchId { get; set; }

        // Delivery, Pickup, or DineIn
        public OrderType OrderType { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // ── Delivery specific fields ──────────────────────────────
        // Only filled when OrderType = Delivery
        public string? DeliveryAddress { get; set; }
        public string? DeliveryCity { get; set; }
        public string? DeliveryDistrict { get; set; }
        public string? DeliveryNotes { get; set; }  // "Ring the bell", "3rd floor"

        // ── Dine-in specific ──────────────────────────────────────
        public string? TableNumber { get; set; }

        // ── Money ─────────────────────────────────────────────────
        public decimal SubTotal { get; set; }       // Sum of all items
        public decimal DeliveryFee { get; set; }    // 0 if pickup/dine-in
        public decimal DiscountAmount { get; set; } // From coupon
        public decimal TaxAmount { get; set; }      // VAT
        public decimal TotalAmount { get; set; }    // What customer pays


        // ── Payment ───────────────────────────────────────────────
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // ── Extra info ────────────────────────────────────────────
        public string? SpecialInstructions { get; set; }
        public string? RejectionReason { get; set; } // If admin rejects the order
        public DateTime? EstimatedDeliveryAt { get; set; }
        public DateTime? ActualDeliveredAt { get; set; }

        // ── Navigation ────────────────────────────────────────────
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public ICollection<OrderStatusHistory> StatusHistory { get; set; }
         = new HashSet<OrderStatusHistory>();


    }
}
