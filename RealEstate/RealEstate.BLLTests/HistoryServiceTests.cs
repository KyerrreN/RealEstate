//using Mapster;
//using NSubstitute;
//using RealEstate.BLL.Models;
//using RealEstate.BLL.Services;
//using RealEstate.DAL.Entities;
//using RealEstate.DAL.Interfaces;
//using RealEstate.Domain.Enums;
//using RealEstate.Domain.Exceptions;
//using RealEstate.Domain.QueryParameters;
//using Shouldly;
//using System.Linq.Expressions;

//namespace RealEstate.BLLTests
//{
//    public class HistoryServiceTests
//    {
//        private readonly HistoryService _service;
//        private readonly IBaseRepository<HistoryEntity> _baseRepository;
//        private readonly IHistoryRepository _historyRepository;
//        private readonly IUserRepository _userRepository;

//        private readonly HistoryEntity _historyEntity;
//        private readonly HistoryModel _historyModel;

//        public HistoryServiceTests()
//        {
//            _baseRepository = Substitute.For<IBaseRepository<HistoryEntity>>();
//            _historyRepository = Substitute.For<IHistoryRepository>();
//            _userRepository = Substitute.For<IUserRepository>();

//            _service = new HistoryService(_baseRepository, _historyRepository, _userRepository);

//            _historyEntity = new HistoryEntity
//            {
//                Id = Guid.NewGuid(),
//                UserId = Guid.NewGuid(),
//                CompletedAt = DateTime.UtcNow,
//                EstateAction = EstateAction.Rent,
//                Title = "Test Title",
//                Description = "Test Description",
//            };

//            _historyModel = _historyEntity.Adapt<HistoryModel>();
//        }

//        [Fact]
//        public async Task DeleteFromHistoryAsync_ShouldDelete_WhenModelExists()
//        {
//            // arrange
//            _userRepository.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
//                .Returns(new UserEntity());
//            _historyRepository.FindOneByConditionAsync(Arg.Any<Expression<Func<HistoryEntity, bool>>>(), Arg.Any<CancellationToken>())
//                .Returns(_historyEntity);

//            // act
//            await _service.DeleteFromHistoryAsync(_historyModel.Id, _historyModel.UserId, CancellationToken.None);

//            // assert
//            await _historyRepository.Received(1).DeleteAsync(Arg.Any<HistoryEntity>(), CancellationToken.None);
//        }

//        [Fact]
//        public async Task GetOneByOwnerIdAsync_ShouldReturnModel_WhenUserAndHistoryExist()
//        {
//            // arrange
//            var ownerId = _historyEntity.UserId;
//            var historyId = _historyEntity.Id;

//            _userRepository.FindByIdAsync(ownerId, Arg.Any<CancellationToken>())
//                .Returns(new UserEntity { Id = ownerId });

//            _historyRepository.FindOneByConditionAsync(
//                Arg.Is<Expression<Func<HistoryEntity, bool>>>(expr => expr.Compile().Invoke(_historyEntity)),
//                Arg.Any<CancellationToken>())
//                .Returns(_historyEntity);

//            // act
//            var result = await _service.GetOneByOwnerIdAsync(historyId, ownerId, CancellationToken.None);

//            // assert
//            result.ShouldNotBeNull();
//            result.Id.ShouldBe(historyId);
//            result.UserId.ShouldBe(ownerId);
//            result.Title.ShouldBe(_historyEntity.Title);
//        }

//        [Fact]
//        public async Task GetOneByOwnerIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
//        {
//            // arrange
//            var ownerId = _historyEntity.UserId;
//            var historyId = _historyEntity.Id;

//            _userRepository.FindByIdAsync(ownerId, Arg.Any<CancellationToken>())
//                .Returns((UserEntity?)null);

//            // act/assert
//            await Should.ThrowAsync<NotFoundException>(async () =>
//                await _service.GetOneByOwnerIdAsync(historyId, ownerId, CancellationToken.None));
//        }

//        [Fact]
//        public async Task GetOneByOwnerIdAsync_ShouldThrowNotFoundException_WhenHistoryDoesNotExist()
//        {
//            // arrange
//            var ownerId = _historyEntity.UserId;
//            var historyId = _historyEntity.Id;

//            _userRepository.FindByIdAsync(ownerId, Arg.Any<CancellationToken>())
//                .Returns(new UserEntity { Id = ownerId });

//            _historyRepository.FindOneByConditionAsync(
//                Arg.Any<Expression<Func<HistoryEntity, bool>>>(),
//                Arg.Any<CancellationToken>())
//                .Returns((HistoryEntity?)null);

//            // act/assert
//            await Should.ThrowAsync<NotFoundException>(async () =>
//                await _service.GetOneByOwnerIdAsync(historyId, ownerId, CancellationToken.None));
//        }

//        [Fact]
//        public async Task GetAllByOwnerIdAsync_ShouldReturnPagedHistory_WhenUserExists()
//        {
//            // arrange
//            var ownerId = _historyEntity.UserId;
//            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };

//            _userRepository.FindByIdAsync(ownerId, Arg.Any<CancellationToken>())
//                .Returns(new UserEntity { Id = ownerId });

//            var historyList = new List<HistoryEntity> { _historyEntity };

//            _historyRepository.FindByConditionAsync(
//                Arg.Is<Expression<Func<HistoryEntity, bool>>>(expr => expr.Compile().Invoke(_historyEntity)),
//                Arg.Any<CancellationToken>())
//                .Returns(historyList);

//            // act
//            var result = await _service.GetAllByOwnerIdAsync(paging, ownerId, CancellationToken.None);

//            // assert
//            result.ShouldNotBeNull();
//            result.Items.ShouldNotBeEmpty();
//            result.Items.Count.ShouldBe(1);
//            result.Items.First().Id.ShouldBe(_historyEntity.Id);
//        }

//        [Fact]
//        public async Task GetAllByOwnerIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
//        {
//            // arrange
//            var ownerId = Guid.NewGuid();
//            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };

//            _userRepository.FindByIdAsync(ownerId, Arg.Any<CancellationToken>())
//                .Returns((UserEntity?)null);

//            // act/assert
//            await Should.ThrowAsync<NotFoundException>(async () =>
//                await _service.GetAllByOwnerIdAsync(paging, ownerId, CancellationToken.None));
//        }
//    }
//}
