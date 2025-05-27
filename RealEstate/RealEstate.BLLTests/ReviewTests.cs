using Mapster;
using MassTransit;
using MassTransit.Transports;
using NSubstitute;
using RealEstate.BLL.Models;
using RealEstate.BLL.Services;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.QueryParameters;
using Shouldly;
using System.Linq.Expressions;

namespace RealEstate.BLLTests
{
    public class ReviewTests
    {
        private readonly IBaseRepository<ReviewEntity> _baseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IPublishEndpoint _endpointMock;

        private readonly ReviewService _service;

        public ReviewTests()
        {
            _baseRepository = Substitute.For<IBaseRepository<ReviewEntity>>();
            _userRepository = Substitute.For<IUserRepository>();
            _reviewRepository = Substitute.For<IReviewRepository>();
            _endpointMock = Substitute.For<IPublishEndpoint>();

            _service = new ReviewService(_baseRepository, _userRepository, _reviewRepository, _endpointMock);
        }

        [Fact]
        public async Task GetReviewsOfUserAsync_ShouldReturnReviews_WhenUserExists()
        {
            // arrange
            var userId = Guid.NewGuid();
            var userEntity = new UserEntity
            {
                Id = userId,
            };
            var reviewEntityList = new List<ReviewEntity>
            {
                new() 
                {
                    Id= Guid.NewGuid(),
                    AuthorId = Guid.NewGuid(),
                    RecipientId = userId,
                    Comment = "Great service!",
                    Rating = 5
                },
                new() 
                {
                    Id = Guid.NewGuid(),
                    AuthorId = Guid.NewGuid(),
                    RecipientId = userId,
                    Comment = "BadService",
                    Rating = 1
                }
            };
            PagingParameters paging = new()
            {
                PageNumber = 1,
                PageSize = 2
            };

            _userRepository.FindByIdAsync(userId, Arg.Any<CancellationToken>())
                .Returns(userEntity);

            _reviewRepository.FindByConditionAsync(Arg.Any<Expression<Func<ReviewEntity, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(reviewEntityList);

            // act
            var result = await _service.GetReviewsOfUserAsync(paging, userId, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.Items.Count.ShouldBe(2);
            result.TotalCount.ShouldBe(2);
            result.CurrentPage.ShouldBe(1);

            var firstReview = result.Items.First();
            firstReview.Comment.ShouldBe("Great service!");

            var secondReview = result.Items.Last();
            secondReview.Rating.ShouldBe((short)1);
        }

        [Fact]
        public async Task GetReviewsOfUserAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // arrange
            _userRepository.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((UserEntity?)null);

            // act/assert
            await Should.ThrowAsync<NotFoundException>(async () => 
                await _service.GetReviewsOfUserAsync(new PagingParameters(), new Guid(), CancellationToken.None));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowNotFoundException_WhenAuthorNotFound()
        {
            // arrange
            var model = new ReviewModel
            {
                AuthorId = Guid.NewGuid(),
                RecipientId = Guid.NewGuid(),
                Comment = "Good service",
                Rating = 5
            };

            _userRepository.FindByIdAsync(model.AuthorId, Arg.Any<CancellationToken>())
                .Returns((UserEntity?)null);

            // act/assert
            await Should.ThrowAsync<NotFoundException>(async () =>
                await _service.CreateAsync(model, CancellationToken.None));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException_WhenAuthorEqualsRecipient()
        {
            // arrange
            var userId = Guid.NewGuid();

            var model = new ReviewModel
            {
                AuthorId = userId,
                RecipientId = userId,
                Comment = "Can't review myself",
                Rating = 4
            };

            // act/assert
            await Should.ThrowAsync<BadRequestException>(() =>
                _service.CreateAsync(model, CancellationToken.None));
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnReviewModel_WhenValid()
        {
            // arrange
            var authorId = Guid.NewGuid();
            var recipientId = Guid.NewGuid();

            var model = new ReviewModel
            {
                AuthorId = authorId,
                RecipientId = recipientId,
                Comment = "Great experience",
                Rating = 5,
                Author = new UserModel
                {
                    Id = authorId,
                    FirstName = "John",
                    LastName = "Doe",
                },
                Recipient = new UserModel
                {
                    Id = recipientId,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "example@gmail.com"
                }
            };

            var entity = model.Adapt<ReviewEntity>();

            _userRepository.FindByIdAsync(authorId, Arg.Any<CancellationToken>())
                .Returns(new UserEntity { Id = authorId });

            _userRepository.FindByIdAsync(recipientId, Arg.Any<CancellationToken>())
                .Returns(new UserEntity { Id = recipientId });

            _reviewRepository.CreateAsync(Arg.Any<ReviewEntity>(), Arg.Any<CancellationToken>())
                .Returns(entity);

            // act
            var result = await _service.CreateAsync(model, CancellationToken.None);

            // assert
            result.ShouldNotBeNull();
            result.AuthorId.ShouldBe(authorId);
            result.RecipientId.ShouldBe(recipientId);
            result.Comment.ShouldBe(model.Comment);
            result.Rating.ShouldBe(model.Rating);
        }
    }
}
