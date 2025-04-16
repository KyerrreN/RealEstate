using Mapster;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Repositories.Extensions;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Models;
using RealEstate.Domain.QueryParameters;

namespace RealEstate.BLL.Services
{
    public class HistoryService
        (IBaseRepository<HistoryEntity> _repository,
        IHistoryRepository _historyRepository, 
        IUserRepository _userRepository) 
        : GenericService<HistoryEntity, HistoryModel>(_repository), IHistoryService
    {
        public async Task DeleteFromHistoryAsync(Guid historyId, Guid ownerId, CancellationToken ct)
        {
            var historyModel = await GetOneByOwnerIdAsync(historyId, ownerId, ct);

            var historyEntity = historyModel.Adapt<HistoryEntity>();

            await _historyRepository.DeleteAsync(historyEntity, ct);
        }

        // to refactor, replace paging on list with paging on IQueryable
        public async Task<PagedEntityModel<HistoryModel>> GetAllByOwnerIdAsync(PagingParameters paging, Guid ownerId, CancellationToken ct)
        {
            _ = await _userRepository.FindByIdAsync(ownerId, ct)
                ?? throw new NotFoundException(ownerId);

            var historyEntities = await _historyRepository.FindByConditionAsync(h => h.UserId.Equals(ownerId), ct);

            var historyModels = historyEntities.Adapt<List<HistoryModel>>();

            var pagedEntity = Utilities.ToPagedEntityModel(paging.PageNumber, paging.PageSize, historyModels);

            return pagedEntity;
        }

        public async Task<HistoryModel> GetOneByOwnerIdAsync(Guid historyId, Guid ownerId, CancellationToken ct)
        {
            _ = await _userRepository.FindByIdAsync(ownerId, ct)
                ?? throw new NotFoundException(ownerId);

            var historyEntity = await _historyRepository.FindOneByConditionAsync(x => x.Id == historyId && x.UserId == ownerId, ct)
                ?? throw new NotFoundException(historyId);

            var historyModel = historyEntity.Adapt<HistoryModel>();

            return historyModel;
        }
    }
}
