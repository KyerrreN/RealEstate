﻿using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs.Booking;

namespace RealEstate.Presentation.Controllers
{
    [Route(ApiRoutes.BookingEndpoint)]
    [ApiController]
    [Authorize]
    public class BookingController(IBookingService _bookingService,
        IValidator<CreateBookingDto> _createBookingValidator,
        IValidator<CloseDealDto> _closeDealValidator) 
        : ControllerBase
    {
        [HttpPost]
        public async Task<BookingDto> CreateBookingFromClient([FromBody] CreateBookingDto bookingForCreationDto, CancellationToken ct)
        {
            await _createBookingValidator.ValidateAndThrowAsync(bookingForCreationDto, ct);

            var bookingModel = bookingForCreationDto.Adapt<BookingModel>();

            var createdModel = await _bookingService.CreateAsync(bookingModel, ct);

            return createdModel.Adapt<BookingDto>();
        }

        [HttpGet("owner/{ownerId:guid}")]
        public async Task<List<BookingDto>> GetAllOwner(Guid ownerId, CancellationToken ct)
        {
            var bookings = await _bookingService.GetByExpression(b => b.RealEstate.OwnerId == ownerId, ct);

            var bookingsDto = bookings.Adapt<List<BookingDto>>();

            return bookingsDto;
        }

        [HttpGet("client/{clientId:guid}")]
        public async Task<List<BookingDto>> GetAllClient(Guid clientId, CancellationToken ct)
        {
            var bookings = await _bookingService.GetByExpression(b => b.UserId == clientId, ct);

            var bookingsDto = bookings.Adapt<List<BookingDto>>();

            return bookingsDto;
        }

        [HttpPost("close")]
        public async Task CloseDeal([FromBody] CloseDealDto closeDealDto, CancellationToken ct)
        {
            await _closeDealValidator.ValidateAndThrowAsync(closeDealDto, ct);

            var closeDealModel = closeDealDto.Adapt<CloseDealModel>();
            await _bookingService.CloseDeal(closeDealModel, ct);
        }

        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id, CancellationToken ct)
        {
            await _bookingService.DeleteAsync(id, ct);
        }
    }
}
