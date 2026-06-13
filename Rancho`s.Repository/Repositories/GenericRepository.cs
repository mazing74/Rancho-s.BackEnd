using Microsoft.EntityFrameworkCore;
using Rancho_s.core.Entities;
using Rancho_s.core.Interfaces;
using Rancho_s.core.Specfction;
using Rancho_s.Repository.Data;

namespace Rancho_s.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly Rancho_sDbContext _context;

        public GenericRepository(Rancho_sDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
               => await _context.Set<T>().AddAsync(entity);


        public void Delete(T entity)
                   => _context.Set<T>().Remove(entity);


        public async Task<IReadOnlyList<T>> GetAllAsync()   
                   => await _context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(Ispecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
            => await _context.Set<T>().FindAsync(id);

        public async Task<T?> GetByIdWithSpecAsync(Ispecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> SaveChangesAsync()
                  => await _context.SaveChangesAsync();


        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        private IQueryable<T> ApplySpecification(Ispecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_context.Set<T>(), spec);
        }
    }
}
