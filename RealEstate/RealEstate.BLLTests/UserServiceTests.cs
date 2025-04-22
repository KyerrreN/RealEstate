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

        public UserServiceTests()
        {
            _repositoryMock = Substitute.For<IBaseRepository<UserEntity>>();
            _userService = new UserService(_repositoryMock);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUserModel_WhenUserExists()
        {
            // arrange
            var id = Guid.NewGuid();

            var userEntity = new UserEntity
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                Phone = "+375151234567"
            };
            _repositoryMock.FindByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns(userEntity);

            // act
            var result = await _userService.GetByIdAsync(id, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<UserModel>();
            result.Id.ShouldBe(id);
            result.FirstName.ShouldBe(userEntity.FirstName);
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
            List<UserEntity> userEntities = [
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@gmail.com",
                    Phone = "+375151234567"
                },
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Mark",
                    LastName = "Someone",
                    Email = "marksomeone@gmail.com",
                    Phone = "+375151234568"
                }
            ];

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
    }
}
