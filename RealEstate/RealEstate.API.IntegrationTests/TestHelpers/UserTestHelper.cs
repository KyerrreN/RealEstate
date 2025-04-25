using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Repositories;
using RealEstate.Presentation.DTOs.User;
using Shouldly;

namespace RealEstate.API.IntegrationTests.TestHelpers
{
    internal class UserTestHelper(IServiceScopeFactory scopeFactory)
    {
        public List<UserEntity> GetTestUsers(Guid userId)
        {
            return 
            [
                new UserEntity
                {
                    Id = userId,
                    FirstName = "Misha",
                    LastName = "Iz-za-ugla",
                    Email = "mishanya@gmail.com",
                    Phone = "+1234567890"
                },
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Vasya",
                    LastName = "Kosar'",
                    Email = "vasek228@gmail.com",
                    Phone = "+1234567891"
                },
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Anton",
                    LastName = "Samosval",
                    Email = "samosvalepta@gmail.com",
                    Phone = "+1234567892"
                },
            ];
        }

        public CreateUserDto GetCreateUserDto()
        {
            return new CreateUserDto
            {
                FirstName = "Pisya",
                LastName = "Popa",
                Email = "kakapopa@gmail.com",
                Phone = "+375251234567"
            };
        }

        public void AssertGetAll(List<UserDto> result, Guid userId)
        {
            result.ShouldNotBeNull();
            result.Count.ShouldBe(3);

            var testUser = result.Single(u => u.Id == userId);
            testUser.ShouldNotBeNull();
            testUser.FirstName.ShouldBe("Misha");
        }

        public void AssertGetById(UserDto result, Guid userid)
        {
            result.ShouldNotBeNull();
            result.Id.ShouldBe(userid);
            result.FirstName.ShouldBe("Misha");
        }

        public void AssertCreate(UserDto result, CreateUserDto input)
        {
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(Guid.Empty);
            result.FirstName.ShouldBe(input.FirstName);
            result.LastName.ShouldBe(input.LastName);
            result.Email.ShouldBe(input.Email);
            result.Phone.ShouldBe(input.Phone);

            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Users.SingleOrDefault(u => u.Id == result.Id).ShouldNotBeNull();
        }

        public void AssertDelete(Guid userId)
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Users.SingleOrDefault(u => u.Id == userId).ShouldBeNull();
        }
    }
}
