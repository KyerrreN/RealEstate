using NSubstitute;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.BLL.Services;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.Domain.Exceptions;
using Shouldly;

namespace RealEstate.BLLTests
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;
        private readonly IBaseRepository<UserEntity> _repositoryMock;
        private readonly UserEntity _firstUserEntity;
        private readonly UserEntity _secondUserEntity;
        private readonly UserModel _firstUserModel;

        public UserServiceTests()
        {
            _repositoryMock = Substitute.For<IBaseRepository<UserEntity>>();
            _userService = new UserService(_repositoryMock);
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
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                Phone = "+375151234567"
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

            // act
            var exception = await Record.ExceptionAsync(() => _userService.CreateAsync(model, CancellationToken.None));
            
            // assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BadRequestException>();
        }
    }
}
