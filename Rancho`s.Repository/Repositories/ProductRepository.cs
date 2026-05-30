using Microsoft.EntityFrameworkCore;
using Rancho_s.core.Entities;
using Rancho_s.core.Interfaces;
using Rancho_s.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(Rancho_sDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Product>> GetActiveProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.DisplayOrder)
                .ThenBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();
        }

        public async Task<bool> ProductExistsAsync(string name)
        {
            return await _context.Products
                .AnyAsync(p => p.Name.ToLower() == name.ToLower() && p.IsActive);
        }

    }
}
