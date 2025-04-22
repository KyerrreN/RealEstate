using Mapster;
using NSubstitute;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.BLL.Services;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Transactions;
using RealEstate.Domain.Enums;
using RealEstate.Domain.Exceptions;
using Shouldly;
using System.Linq.Expressions;

namespace RealEstate.BLLTests
{
    public class BookingServiceTests
    {
        private readonly IBookingService _service;
        private readonly IBaseRepository<BookingEntity> _baseRepository;
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IHistoryRepository _historyRepository;

        private readonly BookingModel _bookingModel;
        private readonly CloseDealModel _closeDealModel;

        public BookingServiceTests()
        {
            _baseRepository = Substitute.For<IBaseRepository<BookingEntity>>();
            _realEstateRepository = Substitute.For<IRealEstateRepository>();
            _bookingRepository = Substitute.For<IBookingRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _transactionManager = Substitute.For<ITransactionManager>();
            _historyRepository = Substitute.For<IHistoryRepository>();
            _service = new BookingService(_baseRepository, _realEstateRepository, _bookingRepository, _userRepository, _transactionManager, _historyRepository);

            _bookingModel = new BookingModel
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                RealEstateId = Guid.NewGuid(),
                EstateAction = EstateAction.Rent,
                Proposal = "Test proposal",
            };

            _closeDealModel = new CloseDealModel
            {
                Id = _bookingModel.Id,
                EstateAction = EstateAction.Rent,
            };
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException_WhenModelIsNull()
        {
            // arrange
            BookingModel model = null;

            // act/assert
            await Should.ThrowAsync<BadRequestException>(async () => await _service.CreateAsync(model, CancellationToken.None));
            await _bookingRepository.DidNotReceive().CreateAsync(Arg.Any<BookingEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // arrange
            _userRepository.FindByIdAsync(Guid.NewGuid(), CancellationToken.None).Returns((UserEntity)null);

            // act/assert
            await Should.ThrowAsync<NotFoundException>(async () => await _service.CreateAsync(_bookingModel, CancellationToken.None));
            await _bookingRepository.DidNotReceive().CreateAsync(Arg.Any<BookingEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowNotFoundException_WhenRealEstateDoesNotExist()
        {
            // arrange
            _userRepository.FindByIdAsync(_bookingModel.UserId, CancellationToken.None)
                .Returns(new UserEntity());

            _realEstateRepository.FindByIdAsync(_bookingModel.RealEstateId, CancellationToken.None)
                .Returns((RealEstateEntity)null);

            // act/assert
            await Should.ThrowAsync<NotFoundException>(async () => await _service.CreateAsync(_bookingModel, CancellationToken.None));
            await _bookingRepository.DidNotReceive().CreateAsync(Arg.Any<BookingEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException_WhenUserDoesNotOwnRealEstate()
        {
            // arrange
            var realEstateEntity = new RealEstateEntity
            {
                OwnerId = Guid.NewGuid()
            };
            _userRepository.FindByIdAsync(_bookingModel.UserId, CancellationToken.None)
                .Returns(new UserEntity());
            _realEstateRepository.FindByIdAsync(_bookingModel.RealEstateId, CancellationToken.None)
                .Returns(realEstateEntity);

            // act/assert
            await Should.ThrowAsync<BadRequestException>(async () => await _service.CreateAsync(_bookingModel, CancellationToken.None));
            await _bookingRepository.DidNotReceive().CreateAsync(Arg.Any<BookingEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedModel_WhenArgumentsAreValid()
        {
            // arrange
            var userEntity = new UserEntity
            {
                Id = _bookingModel.UserId
            };
            var realEstateEntity = new RealEstateEntity
            {
                Id = _bookingModel.RealEstateId,
                OwnerId = _bookingModel.UserId
            };

            _userRepository.FindByIdAsync(_bookingModel.UserId, CancellationToken.None)
                .Returns(userEntity);

            _realEstateRepository.FindByIdAsync(_bookingModel.RealEstateId, CancellationToken.None)
                .Returns(new RealEstateEntity
                {
                    OwnerId = _bookingModel.UserId
                });
            _bookingRepository.CreateAsync(Arg.Any<BookingEntity>(), CancellationToken.None)
                .Returns(_bookingModel.Adapt<BookingEntity>());

            // act
            var result = await _service.CreateAsync(_bookingModel, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.RealEstateId.ShouldBe(_bookingModel.RealEstateId);
            result.UserId.ShouldBe(_bookingModel.UserId);
            result.Proposal.ShouldBe(_bookingModel.Proposal);
        }

        [Fact]
        public async Task CloseDeal_ShouldThrowNotFoundException_WhenBookingDoesNotExist()
        {
            // arrange
            _bookingRepository.FindByIdAsync(_bookingModel.Id, CancellationToken.None)
                .Returns((BookingEntity)null);

            // act/assert
            await Should.ThrowAsync<NotFoundException>(async () => await _service.CloseDeal(_closeDealModel, CancellationToken.None));
            await _transactionManager.DidNotReceive().BeginTransactionAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CloseDeal_ShouldThrowsBadRequestException_WhenAssociatedRealEstateDoesNotExist()
        {
            // arrange
            _bookingRepository.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(new BookingEntity());
            _realEstateRepository.FindByConditionAsync(Arg.Any<Expression<Func<RealEstateEntity, bool>>>(), Arg.Any<CancellationToken>())
                .Returns([]);

            // act/assert
            await Should.ThrowAsync<BadRequestException>(async () => await _service.CloseDeal(_closeDealModel, CancellationToken.None));
            await _transactionManager.DidNotReceive().BeginTransactionAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CloseDeal_ShouldCompleteTransaction_WhenValid()
        {
            // arrange
            var bookingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var realEstateId = Guid.NewGuid();

            var bookingEntity = new BookingEntity 
            {
                Id = bookingId, 
                UserId = userId 
            };

            var realEstateEntity = new RealEstateEntity
            {
                Id = realEstateId,
                Title = "Test Property",
                Description = "Nice place",
            };

            var closeModel = new CloseDealModel
            {
                Id = bookingId,
                EstateAction = EstateAction.Rent
            };

            var mockTransaction = Substitute.For<ITransaction>();

            _bookingRepository.FindByIdAsync(bookingId, Arg.Any<CancellationToken>())
                .Returns(bookingEntity);

            _realEstateRepository.FindByConditionAsync(Arg.Any<Expression<Func<RealEstateEntity, bool>>>(), Arg.Any<CancellationToken>())
                .Returns([realEstateEntity]);

            _transactionManager.BeginTransactionAsync(Arg.Any<CancellationToken>())
                .Returns(mockTransaction);

            // Act
            await _service.CloseDeal(closeModel, CancellationToken.None);

            // Assert
            await _bookingRepository.Received(1).DeleteAsync(bookingEntity, Arg.Any<CancellationToken>());
            await _historyRepository.Received(1).CreateAsync(Arg.Any<HistoryEntity>(), Arg.Any<CancellationToken>());
            await _realEstateRepository.Received(1).DeleteAsync(realEstateEntity, Arg.Any<CancellationToken>());
            await mockTransaction.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }
    }
}
