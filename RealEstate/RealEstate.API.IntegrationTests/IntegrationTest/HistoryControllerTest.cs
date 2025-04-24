using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RealEstate.API.IntegrationTests.Constants;
using RealEstate.API.IntegrationTests.TestHelpers;
using RealEstate.DAL.Repositories;
using RealEstate.Domain.QueryParameters;
using RealEstate.Presentation;
using RealEstate.Presentation.DTOs;
using RealEstate.Presentation.DTOs.History;
using RealEstate.Presentation.DTOs.Review;
using Shouldly;

namespace RealEstate.API.IntegrationTests.IntegrationTest
{
    public class HistoryControllerTest : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly HistoryTestHelper _helper;

        private readonly Guid _historyId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();
        private readonly DateTime _date = DateTime.UtcNow;

        private readonly string _route;

        public HistoryControllerTest(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _helper = new(_factory.Services.GetRequiredService<IServiceScopeFactory>());

            _route = $"{ApiRoutes.User}/{_userId}/{ApiRoutes.History}";

            AddTestData();
        }

        private void AddTestData()
        {
            var data = _helper.GetTestData(_historyId, _userId, _date);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Histories.RemoveRange(dbContext.Histories);
            dbContext.Histories.AddRange(data);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ShouldReturnCorrectPagedDto()
        {
            // arrange
            var paging = new PagingParameters
            {
                PageNumber = 1,
                PageSize = 2,
            };

            // act
            var response = await _client.GetAsync($"{_route}?PageNumber={paging.PageNumber}&PageSize={paging.PageSize}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PagedEntityDto<HistoryDto>>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertGetAll(result!, paging.PageNumber, paging.PageSize);
        }

        [Fact]
        public async Task GetById_ShouldReturnDto()
        {
            // arrange

            // act
            var response = await _client.GetAsync(_route + $"/{_historyId}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<HistoryDto>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertGetById(result!, _historyId);
        }

        [Fact]
        public async Task Delete_ShouldDeleteRow()
        {
            // arrange

            // act
            var response = await _client.DeleteAsync(_route + $"/{_historyId}");

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertDelete(_historyId);
        }
    }
}
