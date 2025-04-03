using Microsoft.EntityFrameworkCore;
using RealEstate.DAL.Interfaces;
using System.Linq.Expressions;

namespace RealEstate.DAL.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected DbSet<T> Query => _context.Set<T>();
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public virtual Task<List<T>> FindAllAsync(CancellationToken ct)
        {
            return Query
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public virtual Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, CancellationToken ct)
        {
            return Query
                .AsNoTracking()
                .Where(expression)
                .ToListAsync(ct);
        }

        public virtual async Task<T> CreateAsync(T entity, CancellationToken ct)
        {
            var newEntity = await Query.AddAsync(entity);
            await _context.SaveChangesAsync(ct);

            return newEntity.Entity;
        }

        public virtual async Task<T> UpdateAsync(T entity, CancellationToken ct)
        {
            var updatedEntity = Query.Update(entity);
            await _context.SaveChangesAsync(ct);

            return updatedEntity.Entity;
        }

        public virtual async Task DeleteAsync(T entity, CancellationToken ct)
        {
            Query.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
        public virtual Task<List<T>> FindPagingAsync(int pageNumber, int pageSize, CancellationToken ct)
        {
            return Query
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }
    }
}
