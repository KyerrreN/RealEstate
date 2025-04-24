using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RealEstate.API.IntegrationTests.Constants;
using RealEstate.API.IntegrationTests.TestHelpers;
using RealEstate.DAL.Repositories;
using RealEstate.Domain.Enums;
using RealEstate.Domain.QueryParameters;
using RealEstate.Presentation;
using RealEstate.Presentation.DTOs;
using RealEstate.Presentation.DTOs.RealEstate;
using System.Net.Http.Json;

namespace RealEstate.API.IntegrationTests.IntegrationTest
{
    public class RealEstateControllerTest : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly RealEstateTestHelper _helper;

        private readonly Guid _reId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();

        public RealEstateControllerTest(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _helper = new(factory.Services.GetRequiredService<IServiceScopeFactory>());

            AddTestData();
        }

        private void AddTestData()
        {
            var realEstates = _helper.GetRealEstates(_reId, _userId);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.RealEstates.RemoveRange(dbContext.RealEstates);
            dbContext.RealEstates.AddRange(realEstates);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ShouldReturnCorrectPagedResultWithFilters()
        {
            // arrange
            var filters = new RealEstateFilterParameters
            {
                MinPrice = 10m,
                MaxPrice = 100m,
                City = "Minsk",
                OwnerId = _userId,
                EstateType = [EstateType.House],
                EstateStatus = [EstateStatus.ForSale],
            };
            var paging = new PagingParameters
            {
                PageSize = 2,
                PageNumber = 1,
            };

            // act
            var response = await _client.GetAsync($"{ApiRoutes.RealEstate}" +
                $"?PageNumber={paging.PageNumber}" +
                $"&PageSize={paging.PageSize}" +
                $"&MinPrice={filters.MinPrice}" +
                $"&MaxPrice={filters.MaxPrice}" +
                $"&City={filters.City}" +
                $"&OwnerId={filters.OwnerId}" +
                $"&EstateType={filters.EstateType[0]}" +
                $"&EstateStatus={filters.EstateStatus[0]}"
                );
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PagedEntityDto<RealEstateDto>>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertGetAll(result!, paging.PageNumber, paging.PageSize);
        }

        [Fact]
        public async Task GetById_ShouldReturnDto()
        {
            // arrange

            // act
            var response = await _client.GetAsync($"{ApiRoutes.RealEstate}/{_reId}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RealEstateDto>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertGetById(result!);
        }

        [Fact]
        public async Task Create_ShouldCreateRowAndReturnCreatedDto()
        {
            // arrange
            var createReviewDto = _helper.CreateRealEstateDto(_userId);

            // act
            var response = await _client.PostAsJsonAsync(ApiRoutes.RealEstate, createReviewDto);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RealEstateDto> (content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertCreate(createReviewDto, result!);
        }

        [Fact]
        public async Task Update_ShouldUpdateRow()
        {
            // arrange
            var updateDto = _helper.CreateUpdateRealEstateDto(_userId);

            // act
            var response = await _client.PutAsJsonAsync($"{ApiRoutes.RealEstate}/{_reId}", updateDto);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RealEstateDto>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertUpdate(updateDto, result!);
        }
    }
}
