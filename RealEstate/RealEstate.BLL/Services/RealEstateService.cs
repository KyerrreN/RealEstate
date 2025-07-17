using Mapster;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificationService.Contracts;
using NotificationService.Contracts.Constants;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Transactions;
using RealEstate.Domain.Enums;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Models;
using RealEstate.Domain.QueryParameters;

namespace RealEstate.BLL.Services
{
    public class RealEstateService
        (IBaseRepository<RealEstateEntity> _repository, 
        IRealEstateRepository _realEstateRepository, 
        IUserRepository _userRepository, 
        IHistoryRepository _historyRepository,
        ITransactionManager transactionManager,
        IPublishEndpoint publishEndpoint,
        IDistributedCache redis,
        ILogger<RealEstateService> logger)
        : GenericService<RealEstateEntity, RealEstateModel>(_repository), IRealEstateService
    {
        public override async Task<RealEstateModel> CreateAsync(RealEstateModel model, CancellationToken ct)
        {
            if (model is null)
                throw new BadRequestException();

            _ = await _userRepository.FindByIdAsync(model.OwnerId, ct)
                ?? throw new NotFoundException(model.OwnerId);

            var entity = model.Adapt<RealEstateEntity>();

            var createdEntity = await _realEstateRepository.CreateAsync(entity, ct);

            var realEstateAddedEvent = createdEntity.Adapt<RealEstateAddedEvent>();

            await publishEndpoint.Publish(realEstateAddedEvent, context =>
            {
                context.SetRoutingKey(NotificationConstants.RealEstateAddedRoutingKey);
            }, ct);

            return createdEntity.Adapt<RealEstateModel>();
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

        public async Task<PagedEntityModel<RealEstateModel>> GetAllWithRequestParameters(RealEstateFilterParameters filters, SortingParameters sorting, CancellationToken ct)
        {
            CheckRealEstateRequestParameters(filters);

            var entities = await _realEstateRepository.GetAllWithRequestParameters(filters, sorting, ct);

            var modelList = entities.Adapt<PagedEntityModel<RealEstateModel>>();

            return modelList;
        }

        public override async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var entityToDelete = await _realEstateRepository.FindByIdWithUserAsync(id, ct)
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

            await using var transaction = await transactionManager.BeginTransactionAsync(ct);

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

            var deletedEvent = entityToDelete.Adapt<RealEstateDeletedEvent>();

            await publishEndpoint.Publish(deletedEvent, context =>
            {
                context.SetRoutingKey(NotificationConstants.RealEstateDeletedRoutingKey);
            }, ct);
        }

        public override async Task<RealEstateModel> GetByIdAsync(Guid id, CancellationToken ct)
        {
            string key = $"re-{id}";

            var cachedValue = await redis.GetStringAsync(key, ct);

            RealEstateModel? model;

            // cache miss
            if (string.IsNullOrEmpty(cachedValue))
            {
                logger.LogInformation("CACHE MISS");

                model = await FetchAndCache(id, key, ct);

                return model;
            }

            // cache hit
            logger.LogInformation("CACHE HIT");
            model = JsonConvert.DeserializeObject<RealEstateModel>(cachedValue);
            
            // broken cache
            if (model is null)
            {
                logger.LogInformation("BROKEN CACHE");
                model = await FetchAndCache(id, key, ct);
            }

            return model;
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

        private async Task<RealEstateModel> FetchAndCache(Guid id, string key, CancellationToken ct)
        {
            var entity = await _realEstateRepository.FindByIdAsync(id, ct)
                ?? throw new NotFoundException(id);

            var model = entity.Adapt<RealEstateModel>();

            await redis.SetStringAsync(key, JsonConvert.SerializeObject(model), ct);

            return model;
        }
    }
}
