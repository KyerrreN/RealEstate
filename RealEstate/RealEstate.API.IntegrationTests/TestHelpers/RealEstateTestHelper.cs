using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Repositories;
using RealEstate.Domain.Enums;
using RealEstate.Presentation.DTOs;
using RealEstate.Presentation.DTOs.RealEstate;
using Shouldly;

namespace RealEstate.API.IntegrationTests.TestHelpers
{
    public class RealEstateTestHelper(IServiceScopeFactory scopeFactory)
    {
        public List<RealEstateEntity> GetRealEstates(Guid firstReId, Guid userId)
        {
            return
            [
                new RealEstateEntity
                {
                    Id = firstReId,
                    Title = "Test Title",
                    Description = "Test Description",
                    Price = 10.99m,
                    Address = "Test Address",
                    City = "Minsk",
                    EstateStatus = EstateStatus.ForSale,
                    EstateType = EstateType.House,
                    OwnerId = userId,
                    Owner = new UserEntity
                    {
                        Id = userId,
                        FirstName = "test",
                        LastName = "test",
                        Phone = "+375251234567",
                        Email = "test@gmail.com"
                    }
                },
                new RealEstateEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Title",
                    Description = "Test Description",
                    Price = 120.99m,
                    Address = "Test Address",
                    City = "Minsk",
                    EstateStatus = EstateStatus.ForSale,
                    EstateType = EstateType.House,
                    OwnerId = userId,
                },
                new RealEstateEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Title",
                    Description = "Test Description",
                    Price = 120.99m,
                    Address = "Test Address",
                    City = "Mogilev",
                    EstateStatus = EstateStatus.ForSale,
                    EstateType = EstateType.House,
                    OwnerId = userId,
                },
                new RealEstateEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Title",
                    Description = "Test Description",
                    Price = 10.99m,
                    Address = "Test Address",
                    City = "Minsk",
                    EstateStatus = EstateStatus.ForRent,
                    EstateType = EstateType.Office,
                    OwnerId = userId,
                },
                new RealEstateEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Title",
                    Description = "Test Description",
                    Price = 150.99m,
                    Address = "Test Address",
                    City = "Minsk",
                    EstateStatus = EstateStatus.ForRent,
                    EstateType = EstateType.Office,
                    OwnerId = userId,
                },
                new RealEstateEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Title",
                    Description = "Test Description",
                    Price = 10.99m,
                    Address = "Test Address",
                    City = "Minsk",
                    EstateStatus = EstateStatus.ForSale,
                    EstateType = EstateType.House,
                    OwnerId = userId,
                },
            ];
        }

        public CreateRealEstateDto CreateRealEstateDto(Guid userId)
        {
            return new CreateRealEstateDto
            {
                Title = "test",
                Description = "desc",
                Price = 10m,
                Address = "test",
                City = "Mogilev",
                EstateStatus = EstateStatus.ForSale,
                EstateType = EstateType.House,
                OwnerId = userId
            };
        }

        public UpdateRealEstateDto CreateUpdateRealEstateDto(Guid userId)
        {
            return new UpdateRealEstateDto
            {
                Title = "Test Title",
                Description = "Test Description",
                Price = 10.99m,
                Address = "Test Address",
                City = "Minsk",
                EstateStatus = EstateStatus.ForSale,
                EstateType = EstateType.House,
                OwnerId = userId,
            };
        }

        public void AssertGetAll(PagedEntityDto<RealEstateDto> result, int pageNumber, int pageSize)
        {
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.TotalPages.ShouldBe((int)Math.Ceiling((double)result.TotalCount / (double)result.Items.Count));
            result.CurrentPage.ShouldBe(1);
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
        }

        public void AssertGetById(RealEstateDto result)
        {
            result.ShouldNotBeNull();
            result.City.ShouldBe("Minsk");
            result.Address.ShouldBe("Test Address");
            result.EstateStatus.ShouldBe(EstateStatus.ForSale.ToString());
        }

        public void AssertCreate(CreateRealEstateDto createDto, RealEstateDto result)
        {
            result.ShouldNotBeNull();
            result.Title.ShouldBe(createDto.Title);
            result.OwnerId.ShouldBe(createDto.OwnerId);
            result.Price.ShouldBe(createDto.Price);

            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.RealEstates.Find(result.Id).ShouldNotBeNull();
        }

        public void AssertUpdate(UpdateRealEstateDto updateDto, RealEstateDto result)
        {
            result.ShouldNotBeNull();
            result.Title.ShouldBe(updateDto.Title);
            result.Description.ShouldBe(updateDto.Description);


            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var row = dbContext.RealEstates.SingleOrDefault(re => re.Id == result.Id);

            row.ShouldNotBeNull();
            row.Title.ShouldBe(result.Title);
        }
    }
}
