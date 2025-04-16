﻿using FluentValidation;
using RealEstate.Domain.Enums;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs.Booking;

namespace RealEstate.Presentation.Validators
{
    public sealed class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
    {
        public CreateBookingDtoValidator()
        {
            RuleFor(x => x.RealEstateId)
                .NotEmpty()
                .WithMessage(ValidatorConstants.PropertyNameRequired);

            RuleFor(x => x.Proposal)
                .NotEmpty()
                .WithMessage(ValidatorConstants.PropertyNameRequired)
                .MaximumLength(2000)
                .WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

            RuleFor(x => x.EstateAction)
                .NotEmpty()
                .WithMessage(ValidatorConstants.PropertyNameRequired)
                .NotEqual(EstateAction.None)
                .WithMessage("{PropertyName} cannot equal to {ComparisonValue}");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(ValidatorConstants.PropertyNameRequired);
        }
    }
}
