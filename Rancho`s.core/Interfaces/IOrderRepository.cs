using Rancho_s.core.Entities;
using Rancho_s.core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        // Customer views their order history
        Task<IReadOnlyList<Order>> GetOrdersByCustomerAsync(int customerId);

         // Get one order with ALL details (items, options, history)
        Task<Order?> GetOrderWithDetailsAsync(int orderId);

        // Kitchen dashboard — new incoming orders for a branch
        Task<IReadOnlyList<Order>> GetActiveOrdersByBranchAsync(int branchId);

        // Admin — all orders with filtering
        Task<IReadOnlyList<Order>> GetAllOrdersAsync(
            int? branchId,
            OrderStatus? status,
            DateTime? from,
            DateTime? to);

        // Generate the next order number
        Task<string> GenerateOrderNumberAsync();


    }
}
