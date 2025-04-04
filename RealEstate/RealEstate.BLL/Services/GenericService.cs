using Mapster;
using MapsterMapper;
using RealEstate.BLL.Interfaces;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Models;
using System.Linq.Expressions;

namespace RealEstate.BLL.Services
{
    public class GenericService<TEntity, TModel>(IBaseRepository<TEntity> repository, IMapper mapper) 
        : IGenericService<TEntity, TModel>
        where TEntity : BaseEntity
        where TModel : class
    {
        protected readonly IBaseRepository<TEntity> _repository = repository;
        protected readonly IMapper _mapper = mapper;

        public virtual async Task<List<TModel>> GetAllAsync(CancellationToken ct)
        {
            var entities = await _repository.GetAllAsync(ct);

            var entitiesModel = entities.Adapt<List<TModel>>();

            return entitiesModel;
        }

        public virtual async Task<TModel> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _repository.FindByIdAsync(id, ct)
                ?? throw new Exception($"{typeof(TEntity)} does not exist");

            var entityModel = entity.Adapt<TModel>();

            return entityModel;
        }

        public async Task<TModel> CreateAsync(TModel model, CancellationToken ct)
        {
            var entity = model.Adapt<TEntity>();

            var createdEntity = await _repository.CreateAsync(entity, ct);

            return createdEntity.Adapt<TModel>();
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var entity = await _repository.FindByIdAsync(id, ct);

            if (entity is not null)
            {
                await _repository.DeleteAsync(entity, ct);
            }
        }

        public async Task<TModel> UpdateAsync(Guid id, TModel model, CancellationToken ct)
        {
            var entityToUpdate = await _repository.FindByIdAsync(id, ct)
                ?? throw new Exception($"{typeof(TEntity)} does not exist");

            model.Adapt(entityToUpdate);

            await _repository.UpdateAsync(entityToUpdate, ct);

            return entityToUpdate.Adapt<TModel>();
        }

        public async Task<PagedEntityModel<TModel>> GetPagingAsync(int pageNumber, int pageSize, CancellationToken ct)
        {
            var entities = await _repository.GetPagedAsync(pageNumber, pageSize, ct);

            var entitiesModel = entities.Adapt<PagedEntityModel<TModel>>();

            return entitiesModel;
        }

        public async Task<List<TModel>> GetByExpression(Expression<Func<TEntity, bool>> expression, CancellationToken ct)
        {
            var entities = await _repository.FindByConditionAsync(expression, ct);

            var entitiesModel = entities.Adapt<List<TModel>>();

            return entitiesModel;
        }
    }
}
