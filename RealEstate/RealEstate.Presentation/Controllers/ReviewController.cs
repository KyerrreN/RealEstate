﻿using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.Domain.QueryParameters;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs;
using RealEstate.Presentation.DTOs.Review;

namespace RealEstate.Presentation.Controllers
{
    [Route(ApiRoutes.ReviewEndpoint)]
    [ApiController]
    [Authorize]
    public class ReviewController(IReviewService _reviewService, IValidator<CreateReviewDto> _createReviewValidator) : ControllerBase
    {
        [HttpGet("{userId:guid}")]
        [AllowAnonymous]
        public async Task<PagedEntityDto<ReviewDto>> GetAll([FromQuery] PagingParameters paging, Guid userId, CancellationToken ct)
        {
            var reviews = await _reviewService.GetReviewsOfUserAsync(paging, userId, ct);

            var pagedDto = reviews.Adapt<PagedEntityDto<ReviewDto>>();

            return pagedDto;
        }

        [HttpPost]
        public async Task<ReviewDto> PostReview([FromBody] CreateReviewDto reviewDto, CancellationToken ct)
        {
            await _createReviewValidator.ValidateAndThrowAsync(reviewDto, ct);

            var reviewModel = reviewDto.Adapt<ReviewModel>();

            var createdReviewModel = await _reviewService.CreateAsync(reviewModel, ct);

            var createdReviewDto = createdReviewModel.Adapt<ReviewDto>();

            return createdReviewDto;
        }
    }
}
