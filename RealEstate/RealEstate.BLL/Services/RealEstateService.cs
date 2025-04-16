using Mapster;
using MapsterMapper;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.Domain.Enums;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Repositories;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Models;
using RealEstate.Domain.QueryParameters;

namespace RealEstate.BLL.Services
{
    public class RealEstateService
        (IBaseRepository<RealEstateEntity> _repository, 
        IMapper _mapper, 
        IRealEstateRepository _realEstateRepository, 
        IUserRepository _userRepository, 
        IHistoryRepository _historyRepository,
        AppDbContext _context)
        : GenericService<RealEstateEntity, RealEstateModel>(_repository, _mapper), IRealEstateService
    {
        public override async Task<RealEstateModel> CreateAsync(RealEstateModel model, CancellationToken ct)
        {
            if (model is null)
                throw new BadRequestException();

            _ = await _userRepository.FindByIdAsync(model.OwnerId, ct)
                ?? throw new NotFoundException(model.OwnerId);

            return await base.CreateAsync(model, ct);
        }

        public override async Task<RealEstateModel> UpdateAsync(Guid id, RealEstateModel model, CancellationToken ct)
        {
            var entityToUpdate = await _realEstateRepository.FindByIdAsync(id, ct)
                ?? throw new NotFoundException(id);

            if (model is null)
                throw new BadRequestException();

            CheckOwnerEqualityAndThrow(model.OwnerId, entityToUpdate.OwnerId);

            model.Id = id;

            model.Adapt(entityToUpdate);

            await _repository.UpdateAsync(entityToUpdate, ct);

            return entityToUpdate.Adapt<RealEstateModel>();
        }

        public async Task<PagedEntityModel<RealEstateModel>> GetAllWithRequestParameters(RealEstateFilterParameters filters, CancellationToken ct)
        {
            CheckRealEstateRequestParameters(filters);

            var entities = await _realEstateRepository.GetAllWithRequestParameters(filters, ct);

            var modelList = entities.Adapt<PagedEntityModel<RealEstateModel>>();

            return modelList;
        }

        public override async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var entityToDelete = await _realEstateRepository.FindByIdAsync(id, ct)
                ?? throw new NotFoundException(id);

            var historyModel = new HistoryModel
            {
                RealEstateId = entityToDelete.Id,
                UserId = entityToDelete.OwnerId,
                CompletedAt = DateTime.UtcNow,
                EstateAction = EstateAction.None,
                Title = entityToDelete.Title,
                Description = entityToDelete.Description
            };

            var historyEntity = historyModel.Adapt<HistoryEntity>();

            using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                await _historyRepository.CreateAsync(historyEntity, ct);
                await _realEstateRepository.DeleteAsync(entityToDelete, ct);

                await transaction.CommitAsync(ct);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
        private static void CheckRealEstateRequestParameters(RealEstateFilterParameters filters)
        {
            if (filters.MinPrice < 0)
                throw new BadRequestException("Min price cannot be negative");

            if (filters.MinPrice > filters.MaxPrice)
                throw new BadRequestException("Max price must be greater than min price");
        }

        private static void CheckOwnerEqualityAndThrow(Guid ownerModelId, Guid ownerEntityId)
        {
            if (ownerModelId != ownerEntityId)
                throw new BadRequestException("Owner id in model and entity are not equal");
        }
    }
}
