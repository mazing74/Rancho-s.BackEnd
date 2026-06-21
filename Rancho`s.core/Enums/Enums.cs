using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Enums
{
    public enum OrderType
    {
        Delivery = 1,
        Pickup = 2,
        DineIn = 3
    }
    public enum OrderStatus
    {
        Pending = 1,  // Just placed, waiting for restaurant to confirm
        Confirmed = 2,  // Restaurant accepted it
        Preparing = 3,  // Kitchen is cooking
        Ready = 4,  // Food is ready (for pickup) or with driver (delivery)
        OutForDelivery = 5, // Driver picked up, on the way
        Delivered = 6,  // Customer received
        Cancelled = 7,  // Customer cancelled
        Rejected = 8   // Restaurant rejected (out of stock, closed, etc.)
    }
    public enum PaymentMethod
    {
        CashOnDelivery = 1,
        CreditCard = 2,
        Fawry = 3,
        VodafoneCash = 4,
        InstaPay = 5
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Failed = 3,
        Refunded = 4
    }
}
