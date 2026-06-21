using Microsoft.EntityFrameworkCore;
using Rancho_s.core.Entities;
using Rancho_s.core.Enums;
using Rancho_s.core.Interfaces;
using Rancho_s.Repository.Data;

namespace Rancho_s.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(Rancho_sDbContext _context) : base(_context) { }

        public async Task<string> GenerateOrderNumberAsync()
        {
            // Format: ORD-20240115-0042
            var today = DateTime.UtcNow;
            var datePrefix = today.ToString("yyyyMMdd");

            // Count today's orders to get the sequence number
            var todayCount = await _context.Orders
                .CountAsync(o => o.CreatedAt.Date == today.Date);
            return $"ORD-{datePrefix}-{(todayCount + 1):D4}";


        }

        public Task<IReadOnlyList<Order>> GetActiveOrdersByBranchAsync(int branchId)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync(int? branchId, OrderStatus? status, DateTime? from, DateTime? to)
        {
            var query = _context.Orders
                       .Include(o => o.Items)
                       .AsQueryable();

            if (branchId.HasValue)
                query = query.Where(o => o.BranchId == branchId.Value);

            if (status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            if (from.HasValue)
                query = query.Where(o => o.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(o => o.CreatedAt <= to.Value);

            return await query
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _context.Orders
                  .Include(o => o.Items)
                      .ThenInclude(i => i.Options)
                  .Where(o => o.CustomerId == customerId)
                  .OrderByDescending(o => o.CreatedAt)
                  .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Options)
                .Include(o => o.StatusHistory.OrderByDescending(h => h.CreatedAt))
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
