using RealEstate.DAL.Models;
using System.Linq.Expressions;

namespace RealEstate.BLL.Interfaces
{
    public interface IGenericService<TEntity, TModel>
        where TEntity: class
        where TModel : class
    {
        Task<List<TModel>> GetAllAsync(CancellationToken ct);
        Task<TModel> GetByIdAsync(Guid id, CancellationToken ct);
        Task<TModel> CreateAsync(TModel model, CancellationToken ct);
        Task<TModel> UpdateAsync(Guid id, TModel model, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
        Task<PagedEntityModel<TModel>> GetPagingAsync(int pageNumber, int pageSize, CancellationToken ct);
        Task<List<TModel>> GetByExpression(Expression<Func<TEntity, bool>> expression, CancellationToken ct);
    }
}
