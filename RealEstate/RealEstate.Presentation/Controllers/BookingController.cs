using Mapster;
using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs.Booking;

namespace RealEstate.Presentation.Controllers
{
    [Route(ApiRoutes.BookingEndpoint)]
    [ApiController]
    public class BookingController(IBookingService bookingService) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;

        [HttpPost]
        public async Task<BookingDto> CreateBookingFromClient([FromBody] BookingForCreationDto bookingForCreationDto, CancellationToken ct)
        {
            var bookingModel = bookingForCreationDto.Adapt<BookingModel>();

            var createdModel = await _bookingService.CreateAsync(bookingModel, ct);

            return createdModel.Adapt<BookingDto>();
        }
    }
}
