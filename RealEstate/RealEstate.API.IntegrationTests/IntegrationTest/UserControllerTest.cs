using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RealEstate.API.IntegrationTests.Constants;
using RealEstate.API.IntegrationTests.TestHelpers;
using RealEstate.DAL.Repositories;
using RealEstate.Presentation;
using RealEstate.Presentation.DTOs.User;
using System.Net.Http.Json;

namespace RealEstate.API.IntegrationTests.IntegrationTest
{
    public class UserControllerTest : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly UserTestHelper _helper;
        private readonly Guid _userId = Guid.NewGuid();

        public UserControllerTest(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _helper = new(factory.Services.GetRequiredService<IServiceScopeFactory>());

            AddTestData();
        }

        private void AddTestData()
        {
            var testUsers = _helper.GetTestUsers(_userId);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Users.RemoveRange(dbContext.Users);
            dbContext.Users.AddRange(testUsers);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ShouldGetCorrectListOfUsers()
        {
            // arrange

            // act
            var response = await _client.GetAsync(ApiRoutes.User);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<UserDto>>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertGetAll(result!, _userId);
        }

        [Fact]
        public async Task GetById_ShouldGetCorrectUser()
        {
            // arrange

            // act
            var response = await _client.GetAsync($"{ApiRoutes.User}/{_userId}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserDto>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertGetById(result!, _userId);
        }

        [Fact]
        public async Task CreateUser_ShouldCreateAndReturnCreatedUser()
        {
            // arrange
            var toCreateUserDto = new CreateUserDto
            {
                FirstName = "Pisya",
                LastName = "Popa",
                Email = "kakapopa@gmail.com",
                Phone = "+375251234567"
            };

            // act
            var response = await _client.PostAsJsonAsync(ApiRoutes.User, toCreateUserDto);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserDto>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertCreate(result!, toCreateUserDto);
        }
    }
}
