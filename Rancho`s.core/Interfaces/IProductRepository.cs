using Rancho_s.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        // Get all active products (IsActive = true only)
        Task<IReadOnlyList<Product>> GetActiveProductsAsync();

        // Get products belonging to one category
        Task<IReadOnlyList<Product>> GetProductsByCategoryAsync(int categoryId);

        // Check if a product name already exists (prevent duplicates)
        Task<bool> ProductExistsAsync(string name);

    }
}
