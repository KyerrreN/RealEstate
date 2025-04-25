using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RealEstate.API.IntegrationTests.Constants;
using RealEstate.API.IntegrationTests.TestHelpers;
using RealEstate.DAL.Repositories;
using RealEstate.Domain.QueryParameters;
using RealEstate.Presentation;
using RealEstate.Presentation.DTOs;
using RealEstate.Presentation.DTOs.Review;
using System.Net.Http.Json;

namespace RealEstate.API.IntegrationTests.IntegrationTest
{
    public class ReviewControllerTest : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly ReviewTestHelper _helper;

        private readonly Guid _reviewId = Guid.NewGuid();
        private readonly Guid _authorId = Guid.NewGuid();
        private readonly Guid _recipientId = Guid.NewGuid();

        public ReviewControllerTest(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _helper = new(factory.Services.GetRequiredService<IServiceScopeFactory>());

            AddTestData();
        }

        private void AddTestData()
        {
            var testReviews = _helper.GetReviews(_reviewId, _authorId, _recipientId);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Reviews.RemoveRange(dbContext.Reviews);
            dbContext.AddRange(testReviews);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ShouldReturnPagedReviews()
        {
            // arrange
            var paging = new PagingParameters
            {
                PageSize = 1,
                PageNumber = 2,
            };

            // act
            var response = await _client.GetAsync($"{ApiRoutes.Review}/{_recipientId}?PageNumber={paging.PageNumber}&PageSize={paging.PageSize}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PagedEntityDto<ReviewDto>>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertGetAll(result!, paging.PageSize, paging.PageNumber);
        }

        [Fact]
        public async Task CreateReview_ShouldCreateAndReturnCreatedDto()
        {
            // arrange
            var createReviewDto = _helper.CreateReviewDto(_authorId, _recipientId);

            // act
            var response = await _client.PostAsJsonAsync(ApiRoutes.Review, createReviewDto);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ReviewDto>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertCreate(result!);
        }
    }
}
