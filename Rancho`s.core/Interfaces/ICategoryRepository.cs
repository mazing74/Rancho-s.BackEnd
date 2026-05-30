using Rancho_s.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        // Get all active categories — for the public menu page
        Task<IReadOnlyList<Category>> GetActiveCategoriesAsync();

        // Get category WITH its products loaded — one query, full data
        Task<Category?> GetCategoryWithProductsAsync(int categoryId);

        // Prevent duplicate category names
        Task<bool> CategoryExistsAsync(string name);

    }
}
