using MassTransit;
using NSubstitute;
using RealEstate.BLL.Models;
using RealEstate.BLL.Services;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Models;
using Shouldly;
using System.Linq.Expressions;

namespace RealEstate.BLLTests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly IBaseRepository<UserEntity> _repositoryMock;
        private readonly IPublishEndpoint _endpointMock;
        private readonly UserEntity _firstUserEntity;
        private readonly UserEntity _secondUserEntity;
        private readonly UserModel _firstUserModel;
        private readonly UserModel _secondUserModel;

        public UserServiceTests()
        {
            _repositoryMock = Substitute.For<IBaseRepository<UserEntity>>();
            _endpointMock = Substitute.For<IPublishEndpoint>();

            _userService = new UserService(_repositoryMock, _endpointMock);
            _firstUserEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                Phone = "+375151234567"
            };
            _secondUserEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                FirstName = "Mark",
                LastName = "Someone",
                Email = "marksomeone@gmail.com",
                Phone = "+375151234568"
            };
            _firstUserModel = new UserModel
            {
                Id = _firstUserEntity.Id,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                Phone = "+375151234567"
            };
            _secondUserModel = new UserModel
            {
                Id = _secondUserEntity.Id,
                FirstName = "Mark",
                LastName = "Someone",
                Email = "marksomeone@gmail.com",
                Phone = "+375151234568"
            };
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUserModel_WhenUserExists()
        {
            // arrange
            _repositoryMock.FindByIdAsync(_firstUserEntity.Id, Arg.Any<CancellationToken>())
                .Returns(_firstUserEntity);

            // act
            var result = await _userService.GetByIdAsync(_firstUserEntity.Id, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<UserModel>();
            result.Id.ShouldBe(_firstUserEntity.Id);
            result.FirstName.ShouldBe(_firstUserEntity.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // arrange
            var id = Guid.NewGuid();
            _repositoryMock.FindByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns((UserEntity?)null);

            // act
            var exception = await Record.ExceptionAsync(() => _userService.GetByIdAsync(id, CancellationToken.None));

            // assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<NotFoundException>();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListUserModel_WhenThereAreUsers()
        {
            // arrange
            List<UserEntity> userEntities = [_firstUserEntity, _secondUserEntity];

            _repositoryMock.GetAllAsync(Arg.Any<CancellationToken>())
                .Returns(userEntities);

            // act
            var result = await _userService.GetAllAsync(CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
            result.ShouldContain(u => u.Id == userEntities[0].Id || u.Id == userEntities[1].Id);
            result[0].FirstName.ShouldBeSameAs(userEntities[0].FirstName);
            result[1].LastName.ShouldBeSameAs(userEntities[1].LastName);
            result[0].Email.ShouldBeSameAs(userEntities[0].Email);
            result[0].Email.ShouldNotBeSameAs(userEntities[1].Email);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsers()
        {
            // arrange
            _repositoryMock.GetAllAsync(Arg.Any<CancellationToken>())
                .Returns([]);

            // act
            var result = await _userService.GetAllAsync(CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedModel_WhenModelProvided()
        {
            // arrange
            _repositoryMock.CreateAsync(Arg.Any<UserEntity>(), Arg.Any<CancellationToken>())
                .Returns(callInfo =>
                {
                    var inputEntity = callInfo.Arg<UserEntity>();
                    return inputEntity;
                });

            // act
            var result = await _userService.CreateAsync(_firstUserModel, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(_firstUserModel);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException_WhenModelIsNull()
        {
            // arrange
            UserModel? model = null;

            // act/assert
            await Should.ThrowAsync<BadRequestException>(() => _userService.CreateAsync(model, CancellationToken.None));
            await _repositoryMock.DidNotReceive().CreateAsync(Arg.Any<UserEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDelete_WhenEntityExists()
        {
            // arrange
            _repositoryMock.FindByIdAsync(_firstUserEntity.Id, Arg.Any<CancellationToken>())
                .Returns(_firstUserEntity);

            // act
            await _userService.DeleteAsync(_firstUserEntity.Id, CancellationToken.None);

            // assert
            await _repositoryMock.Received(1).DeleteAsync(_firstUserEntity, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenEntityDoesNotExist()
        {
            // arrange
            _repositoryMock.FindByIdAsync(Guid.Empty, Arg.Any<CancellationToken>())
                .Returns((UserEntity?)null);

            // act/assert
            await Should.ThrowAsync<NotFoundException>(() => _userService.DeleteAsync(Guid.Empty, CancellationToken.None));
            await _repositoryMock.DidNotReceive().DeleteAsync(Arg.Any<UserEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCorrectly_WhenIdAndModelProvided()
        {
            // arrange
            _repositoryMock.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(_firstUserEntity);
            _repositoryMock.UpdateAsync(Arg.Any<UserEntity>(), Arg.Any<CancellationToken>())
                .Returns(callInfo => Task.FromResult(callInfo.Arg<UserEntity>()));

            // act
            var result = await _userService.UpdateAsync(_firstUserEntity.Id, _secondUserModel, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(_secondUserModel);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsNotFoundException_WhenEntityDoesNotExist()
        {
            // arrange
            var id = Guid.NewGuid();
            _repositoryMock.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((UserEntity?)null);

            // act/assert
            await Should.ThrowAsync<NotFoundException>(() => _userService.UpdateAsync(id, _secondUserModel, CancellationToken.None));
            await _repositoryMock.DidNotReceive().UpdateAsync(Arg.Any<UserEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_ThrowsBadRequestException_WhenModelIsNull()
        {
            // arrange
            _repositoryMock.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(_firstUserEntity);

            // act/assert
            await Should.ThrowAsync<BadRequestException>(() => _userService.UpdateAsync(Guid.Empty, null, CancellationToken.None));
            await _repositoryMock.DidNotReceive().UpdateAsync(Arg.Any<UserEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetPagingAsync_ReturnsPagedEntityModel_WhenProvideArgs()
        {
            // arrange
            var pagedEntity = new PagedEntityModel<UserEntity>
            {
                TotalCount = 2,
                Items = [_firstUserEntity, _secondUserEntity],
                CurrentPage = 1,
                TotalPages = 1
            };
            _repositoryMock.GetPagedAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(pagedEntity);

            // act
            var result = await _userService.GetPagingAsync(1, 2, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.TotalPages.ShouldBe(1);
            result.CurrentPage.ShouldBe(1);
            result.TotalCount.ShouldBe(2);
            result.Items[0].FirstName.ShouldBe(_firstUserModel.FirstName);
            result.Items[1].LastName.ShouldBe(_secondUserModel.LastName);
        }

        [Fact]
        public async Task GetByExpression_ShouldReturnListUserModel()
        {
            // arrange
            Expression<Func<UserEntity, bool>> expression = u => u.FirstName == "John";
            List<UserEntity> matchingUsers = [_firstUserEntity];

            _repositoryMock.FindByConditionAsync(expression, Arg.Any<CancellationToken>())
                .Returns(matchingUsers);

            // act
            var result = await _userService.GetByExpression(expression, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result.First().ShouldBeEquivalentTo(_firstUserModel);
        }
    }
}
