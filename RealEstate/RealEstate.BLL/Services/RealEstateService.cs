using Mapster;
using MapsterMapper;
using RealEstate.BLL.Exceptions;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Models;
using RealEstate.DAL.RequestParameters;

namespace RealEstate.BLL.Services
{
    public class RealEstateService(IBaseRepository<RealEstateEntity> repository, IMapper mapper, IRealEstateRepository realEstateRepository, IUserRepository userRepository) : GenericService<RealEstateEntity, RealEstateModel>(repository, mapper), IRealEstateService
    {
        private readonly IRealEstateRepository _realEstateRepository = realEstateRepository;
        private readonly IUserRepository _userRepository = userRepository;

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

            //_ = await _userRepository.FindByIdAsync(model.OwnerId, ct)
            //    ?? throw new NotFoundException(model.OwnerId);

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
