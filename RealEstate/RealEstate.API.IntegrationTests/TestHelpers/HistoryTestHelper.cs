using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Repositories;
using RealEstate.Presentation.DTOs;
using RealEstate.Presentation.DTOs.History;
using RealEstate.Presentation.DTOs.Review;
using Shouldly;

namespace RealEstate.API.IntegrationTests.TestHelpers
{
    public class HistoryTestHelper(IServiceScopeFactory scopeFactory)
    {
        public List<HistoryEntity> GetTestData(Guid firstHistoryId, Guid userId, DateTime date)
        {
            return
            [
                new HistoryEntity
                {
                    Id = firstHistoryId,
                    UserId = userId,
                    CompletedAt = date,
                    Title = "test",
                    Description = "test",
                    User = new UserEntity
                    {
                        Id = userId,
                        Phone = "+375251234567",
                        Email = "a@gmail.com",
                        FirstName = "test",
                        LastName = "test",
                    }
                },
                new HistoryEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CompletedAt = date,
                    Title = "test",
                    Description = "test",
                },
                new HistoryEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CompletedAt = date,
                    Title = "test",
                    Description = "test",
                }
            ];
        }

        public void AssertGetAll(PagedEntityDto<HistoryDto> result, int pageNumber, int pageSize)
        {
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.TotalCount.ShouldBe(3);
            result.TotalPages.ShouldBe((int)Math.Ceiling((double)result.TotalCount / pageSize));
            result.CurrentPage.ShouldBe(pageNumber);
        }

        public void AssertGetById(HistoryDto result, Guid historyId)
        {
            result.ShouldNotBeNull();
            result.Id.ShouldBe(historyId);
        }

        public void AssertDelete(Guid historyId)
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Histories.Find(historyId).ShouldBeNull();
        }
    }
}
