namespace RealEstate.BLL.Interfaces
{
    public interface IGenericService<TEntity, TModel>
    {
        Task<List<TModel>> GetAllAsync(CancellationToken ct);
        Task<TModel> GetByIdAsync(Guid id, CancellationToken ct);
        Task<TModel> CreateAsync(TModel model, CancellationToken ct);
        Task<TModel> UpdateAsync(Guid id, TModel model, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
