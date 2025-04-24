using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RealEstate.API.IntegrationTests.Constants;
using RealEstate.API.IntegrationTests.TestHelpers;
using RealEstate.DAL.Repositories;
using RealEstate.Domain.Enums;
using RealEstate.Presentation;
using RealEstate.Presentation.DTOs.Booking;
using System.Net.Http.Json;

namespace RealEstate.API.IntegrationTests.IntegrationTest
{
    public class BookingControllerTest : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly BookingTestHelper _helper;

        private readonly Guid _firstBookingId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _realEstateId = Guid.NewGuid();
        private readonly Guid _ownerId = Guid.NewGuid();

        public BookingControllerTest(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _helper = new(_factory.Services.GetRequiredService<IServiceScopeFactory>());

            AddTestData();
        }

        private void AddTestData()
        {
            var testData = _helper.CreateTestData(_firstBookingId, _userId, _realEstateId, _ownerId);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Bookings.RemoveRange(dbContext.Bookings);
            dbContext.Bookings.AddRange(testData);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task CreateBookingFromClient_ShouldCreateRowAndReturnCreatedBooking()
        {
            // arrange
            var dto = _helper.CreateBookingDto(_realEstateId, _userId);

            // act
            var response = await _client.PostAsJsonAsync($"{ApiRoutes.Booking}", dto);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BookingDto>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertCreateBookingFromClient(dto, result!);
        }

        [Fact]
        public async Task GetAllOwner_ShouldReturnListOfBookings()
        {
            // arrange

            // act
            var response = await _client.GetAsync($"{ApiRoutes.BookingOwner}/{_userId}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<BookingDto>>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertGetOwner(result!, _ownerId);
        }

        [Fact]
        public async Task GetAllClient_ShouldReturnListOfBookings()
        {
            // arrange

            // act
            var response = await _client.GetAsync($"{ApiRoutes.BookingClient}/{_ownerId}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<BookingDto>>(content);

            // assert
            response.EnsureSuccessStatusCode();
            _helper.AssertGetAllClient(result!, _userId);
        }
    }
}
