using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Repositories;
using RealEstate.Domain.Enums;
using RealEstate.Presentation.DTOs.Booking;
using Shouldly;

namespace RealEstate.API.IntegrationTests.TestHelpers
{
    public class BookingTestHelper(IServiceScopeFactory scopeFactory)
    {
        public List<BookingEntity> CreateTestData(Guid firstBookingId, Guid userId, Guid realEstateId, Guid ownerId)
        {
            return
            [
                new BookingEntity
                {
                    Id = firstBookingId,
                    Proposal = "test",
                    EstateAction = EstateAction.Sell,
                    UserId = userId,
                    User = new UserEntity
                    {
                        Id = userId,
                        FirstName = "test",
                        LastName = "test",
                        Email = "test@gmail.com",
                        Phone = "+375251234567",
                    },
                    RealEstateId = realEstateId,
                    RealEstate = new RealEstateEntity
                    {
                        Id = realEstateId,
                        Title = "test",
                        Description = "test",
                        Price = 10m,
                        Address = "test",
                        City = "test",
                        EstateStatus = EstateStatus.ForSale,
                        EstateType = EstateType.House,
                        OwnerId = userId,
                        Owner = new UserEntity
                        {
                            Id = userId,
                            FirstName = "test",
                            LastName = "test",
                            Email = "test@gmail.com",
                            Phone = "+375251234567",
                        }
                    }
                },
                new BookingEntity
                {
                    Id = Guid.NewGuid(),
                    Proposal = "test",
                    EstateAction = EstateAction.Sell,
                    UserId = ownerId,
                    RealEstateId = realEstateId,
                }
            ];
        }

        public CreateBookingDto CreateBookingDto(Guid realEstateId, Guid userId)
        {
            return new CreateBookingDto
            {
                RealEstateId = realEstateId,
                Proposal = "test",
                EstateAction = EstateAction.Sell,
                UserId = userId,
            };
        }

        public void AssertCreateBookingFromClient(CreateBookingDto dto, BookingDto result)
        {
            result.ShouldNotBeNull();
            result.Proposal.ShouldBe(dto.Proposal);
            result.EstateAction.ShouldBe(dto.EstateAction.ToString());

            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var row = dbContext.Bookings.Find(result.Id).ShouldNotBeNull();

            result.Proposal.ShouldBe(row.Proposal);
        }

        public void AssertGetOwner(List<BookingDto> result, Guid ownerId)
        {
            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
        }

        public void AssertGetAllClient(List<BookingDto> result, Guid userId)
        {
            result.ShouldNotBeNull();
            result.Count.ShouldBe(1);
        }
    }
}
