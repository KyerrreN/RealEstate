using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.Entities;
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
    }
}
