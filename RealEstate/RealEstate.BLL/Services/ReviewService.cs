using Mapster;
using MapsterMapper;
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
    public class ReviewService
        (IBaseRepository<ReviewEntity> _repository, 
        IMapper _mapper, 
        IUserRepository _userRepository, 
        IReviewRepository _reviewRepository) 
        : GenericService<ReviewEntity, ReviewModel>(_repository, _mapper), IReviewService
    {
        // To refactor, paging on IQueryable
        public async Task<PagedEntityModel<ReviewModel>> GetReviewsOfUserAsync(PagingParameters paging, Guid userId, CancellationToken ct)
        {
            _ = await _userRepository.FindByIdAsync(userId, ct)
                ?? throw new NotFoundException(userId);

            var reviewEntities = await _reviewRepository.FindByConditionAsync(re => re.RecipientId == userId, ct);

            var reviewModels = reviewEntities.Adapt<List<ReviewModel>>();

            return Utilities.ToPagedEntityModel(paging.PageNumber, paging.PageSize, reviewModels);
        }

        public override async Task<ReviewModel> CreateAsync(ReviewModel model, CancellationToken ct)
        {
            await ValidateModelAndIdsAndThrowAsync(model, ct);

            var reviewEntity = model.Adapt<ReviewEntity>();

            var createdReviewEntity = await _reviewRepository.CreateAsync(reviewEntity, ct);

            return createdReviewEntity.Adapt<ReviewModel>();
        }
        private async Task ValidateModelAndIdsAndThrowAsync(ReviewModel model, CancellationToken ct)
        {
            if (model is null)
                throw new BadRequestException("Model is null");

            if (model.AuthorId == Guid.Empty)
                throw new BadRequestException("AuthorId is empty");

            if (model.RecipientId == Guid.Empty)
                throw new BadRequestException("RecipientId is empty");

            if (model.AuthorId == model.RecipientId)
                throw new BadRequestException("AuthorId cannot be equal to RecipientId");

            _ = await _userRepository.FindByIdAsync(model.AuthorId, ct)
                ?? throw new NotFoundException(model.AuthorId);

            _ = await _userRepository.FindByIdAsync(model.RecipientId, ct)
                ?? throw new NotFoundException(model.RecipientId);
        }
    }
}
