using Mapster;
using MapsterMapper;
using RealEstate.BLL.Exceptions;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Models;
using RealEstate.DAL.Repositories.Extensions;
using RealEstate.DAL.RequestParameters;

namespace RealEstate.BLL.Services
{
    public class HistoryService
        (IBaseRepository<HistoryEntity> _repository, 
        IMapper _mapper, 
        IHistoryRepository _historyRepository, 
        IUserRepository _userRepository) 
        : GenericService<HistoryEntity, HistoryModel>(_repository, _mapper), IHistoryService
    {
        public async Task DeleteFromHistoryAsync(Guid historyId, Guid ownerId, CancellationToken ct)
        {
            var historyModel = await GetOneByOwnerIdAsync(historyId, ownerId, ct);

            var historyEntity = historyModel.Adapt<HistoryEntity>();

            await _historyRepository.DeleteAsync(historyEntity, ct);
        }

        public async Task<PagedEntityModel<HistoryModel>> GetAllByOwnerIdAsync(PagingParameters paging, Guid ownerId, CancellationToken ct)
        {
            _ = await _userRepository.FindByIdAsync(ownerId, ct)
                ?? throw new NotFoundException(ownerId);

            var historyEntities = await _historyRepository.FindByConditionAsync(h => h.UserId.Equals(ownerId), ct);

            var historyModels = historyEntities.Adapt<List<HistoryModel>>();

            var pagedEntity = PagedEntityModel<HistoryModel>.ToPagedEntityModel(paging.PageNumber, paging.PageSize, historyModels);

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
