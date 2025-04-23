using Mapster;
using NSubstitute;
using RealEstate.BLL.Models;
using RealEstate.BLL.Services;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Transactions;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Models;
using RealEstate.Domain.QueryParameters;
using Shouldly;

namespace RealEstate.BLLTests
{
    public class RealEstateServiceTests
    {
        private readonly IBaseRepository<RealEstateEntity> _baseRepository;
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly ITransactionManager _transactionManager;

        private readonly RealEstateService _service;

        public RealEstateServiceTests()
        {
            _baseRepository = Substitute.For<IBaseRepository<RealEstateEntity>>();
            _realEstateRepository = Substitute.For<IRealEstateRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _historyRepository = Substitute.For<IHistoryRepository>();
            _transactionManager = Substitute.For<ITransactionManager>();

            _service = new RealEstateService(_baseRepository, _realEstateRepository, _userRepository, _historyRepository, _transactionManager);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException_WhenModelIsNull()
        {
            // arrange
            RealEstateModel model = null;

            // act/assert
            await Should.ThrowAsync<BadRequestException>(async () =>
                await _service.CreateAsync(model, CancellationToken.None));
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateRealEstate_WhenModelIsValidAndUserExists()
        {
            // arrange
            var model = new RealEstateModel
            {
                Id = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Title = "Nice Apartment"
            };

            var entity = model.Adapt<RealEstateEntity>();

            _userRepository.FindByIdAsync(model.OwnerId, Arg.Any<CancellationToken>())
                .Returns(new UserEntity { Id = model.OwnerId });

            _baseRepository.CreateAsync(Arg.Any<RealEstateEntity>(), Arg.Any<CancellationToken>())
                .Returns(entity);

            // act
            var result = await _service.CreateAsync(model, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.Title.ShouldBe(model.Title);
            result.OwnerId.ShouldBe(model.OwnerId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdate_WhenValidModelAndOwnerMatches()
        {
            // arrange
            var id = Guid.NewGuid();
            var ownerId = Guid.NewGuid();

            var model = new RealEstateModel
            {
                OwnerId = ownerId,
                Title = "Updated Title"
            };

            var entity = new RealEstateEntity
            {
                Id = id,
                OwnerId = ownerId,
                Title = "Old Title"
            };

            _realEstateRepository.FindByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns(entity);

            _baseRepository.UpdateAsync(entity, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(entity));

            // act
            var result = await _service.UpdateAsync(id, model, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.Title.ShouldBe(model.Title);
            result.OwnerId.ShouldBe(ownerId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenEntityNotFound()
        {
            // arrange
            var id = Guid.NewGuid();
            var model = new RealEstateModel { OwnerId = Guid.NewGuid() };

            _realEstateRepository.FindByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns((RealEstateEntity?)null);

            // act/assert
            await Should.ThrowAsync<NotFoundException>(() =>
                _service.UpdateAsync(id, model, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowBadRequestException_WhenModelIsNull()
        {
            // arrange
            var id = Guid.NewGuid();

            _realEstateRepository.FindByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns(new RealEstateEntity { Id = id, OwnerId = Guid.NewGuid() });

            // act/assert
            await Should.ThrowAsync<BadRequestException>(() =>
                _service.UpdateAsync(id, null!, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowBadRequestException_WhenOwnerIdMismatch()
        {
            // arrange
            var id = Guid.NewGuid();
            var modelOwnerId = Guid.NewGuid();
            var entityOwnerId = Guid.NewGuid();

            var model = new RealEstateModel
            {
                OwnerId = modelOwnerId,
                Title = "Title"
            };

            var entity = new RealEstateEntity
            {
                Id = id,
                OwnerId = entityOwnerId
            };

            _realEstateRepository.FindByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns(entity);

            // act/ssert
            await Should.ThrowAsync<BadRequestException>(() =>
                _service.UpdateAsync(id, model, CancellationToken.None));
        }

        [Fact]
        public async Task GetAllWithRequestParameters_ShouldReturnPagedModel_WhenValidParametersProvided()
        {
            // arrange
            var filters = new RealEstateFilterParameters
            {
                MinPrice = 10m,
                MaxPrice = 100m,
            };

            var sorting = new SortingParameters
            {
                OrderBy = "Price desc",
            };

            var pagedEntities = new PagedEntityModel<RealEstateEntity>
            {
                Items =
                [
                    new() 
                    {
                        Id = Guid.NewGuid(),
                        OwnerId = Guid.NewGuid(),
                        Title = "Apartment in City Center",
                        Price = 50m,
                    }
                ],
                TotalPages = 1,
                CurrentPage = 1,
                TotalCount = 1
            };

            _realEstateRepository
                .GetAllWithRequestParameters(filters, sorting, Arg.Any<CancellationToken>())
                .Returns(pagedEntities);

            // act
            var result = await _service.GetAllWithRequestParameters(filters, sorting, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeEmpty();
            result.TotalPages.ShouldBe(pagedEntities.TotalPages);
            result.TotalCount.ShouldBe(pagedEntities.TotalCount);
            result.TotalCount.ShouldBe(pagedEntities.TotalCount);
            result.Items.First().Title.ShouldBe("Apartment in City Center");
        }
    }
}
