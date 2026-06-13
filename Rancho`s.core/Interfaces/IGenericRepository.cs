using Rancho_s.core.Entities;
using Rancho_s.core.Specfction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Queries
        #region Before SpecificationEvaluator
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        // the diiference between IEnumerable and IReadOnlyList
        // is that the latter has a Count property, which can be useful in some cases. Also, IReadOnlyList is
        // more explicit about the fact that the collection is read-only, which can help prevent accidental modifications(cant add items or remove items).

        #endregion

        #region After SpecificationEvaluator
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(Ispecification<T> spec);

        Task<T?> GetByIdWithSpecAsync(Ispecification<T> spec);



        #endregion

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        // Save — why separate? Because you might add 10 items and save once
        // That's one database round trip instead of 10. Much faster.
        Task<int> SaveChangesAsync();

    }
}
