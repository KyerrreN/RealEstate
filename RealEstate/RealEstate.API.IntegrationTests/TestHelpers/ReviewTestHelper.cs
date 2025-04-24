using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Repositories;
using RealEstate.Presentation.DTOs;
using RealEstate.Presentation.DTOs.Review;
using Shouldly;

namespace RealEstate.API.IntegrationTests.TestHelpers
{
    public class ReviewTestHelper(IServiceScopeFactory scopeFactory)
    {
        public List<ReviewEntity> GetReviews(Guid firstReviewId, Guid authorId, Guid reciepientId)
        {
            return
            [
                new ReviewEntity
                {
                    Id = firstReviewId,
                    Rating = 5,
                    Comment = "Great",
                    AuthorId = authorId,
                    RecipientId = reciepientId,
                    Author = new UserEntity 
                    {
                        Id = authorId,
                        FirstName = "Author",
                        LastName = "Author",
                        Phone = "+375251234567",
                        Email = "author@gmail.com"
                    },
                    Recipient = new UserEntity
                    {
                        Id = reciepientId,
                        FirstName = "Recipient",
                        LastName = "Recipient",
                        Phone = "+375251234568",
                        Email = "recipient@gmail.com"
                    }
                },
                new ReviewEntity
                {
                    Id = Guid.NewGuid(),
                    Rating = 4,
                    Comment = "Good",
                    AuthorId = authorId,
                    RecipientId = reciepientId,
                },
                new ReviewEntity
                {
                    Id = Guid.NewGuid(),
                    Rating = 2,
                    Comment = "Bad",
                    AuthorId = authorId,
                    RecipientId = reciepientId,
                },
                new ReviewEntity
                {
                    Id = Guid.NewGuid(),
                    Rating = 1,
                    Comment = "Very Bad",
                    AuthorId = authorId,
                    RecipientId = reciepientId,
                },
            ];
        }

        public CreateReviewDto CreateReviewDto(Guid authorId, Guid recipientId)
        {
            return new CreateReviewDto
            {
                Rating = 5,
                AuthorId = authorId,
                RecipientId = recipientId,
                Comment = "No comment",
            };
        }

        public void AssertGetAll(PagedEntityDto<ReviewDto> result, int pageSize, int pageNumber)
        {
            result.ShouldNotBeNull();
            result.TotalCount.ShouldBe(4);
            result.CurrentPage.ShouldBe(pageNumber);
            result.Items.Count.ShouldBe(pageSize);
            result.TotalPages.ShouldBe(result.TotalCount / pageSize);
        }

        public void AssertCreate(ReviewDto result)
        {
            result.ShouldNotBeNull();

            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Reviews.SingleOrDefault(
                re => re.AuthorId == result.AuthorId 
                && re.RecipientId == result.RecipientId
                && re.Comment == result.Comment)
                .ShouldNotBeNull();

            result.Rating.ShouldBe((short)5);
            result.Comment.ShouldBe("No comment");
        }
    }
}
