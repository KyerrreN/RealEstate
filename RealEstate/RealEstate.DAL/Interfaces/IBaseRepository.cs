using System.Linq.Expressions;

namespace RealEstate.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> FindAllAsync(CancellationToken ct);
        Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, CancellationToken ct);
        Task<T> CreateAsync(T entity, CancellationToken ct);
        Task<T> UpdateAsync(T entity, CancellationToken ct);
        Task DeleteAsync(T entity, CancellationToken ct);
    }
}
