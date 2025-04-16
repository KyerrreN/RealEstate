using FluentValidation;
using RealEstate.Domain.Enums;
using RealEstate.Presentation.DTOs.Booking;

namespace RealEstate.Presentation.Validators
{
    public sealed class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
    {
        private const string PropertyNameRequired = "{PropertyName} is required";

        public CreateBookingDtoValidator()
        {
            RuleFor(x => x.RealEstateId)
                .NotEmpty()
                .WithMessage(PropertyNameRequired);

            RuleFor(x => x.Proposal)
                .NotEmpty()
                .WithMessage(PropertyNameRequired)
                .MaximumLength(2000)
                .WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

            RuleFor(x => x.EstateAction)
                .NotEmpty()
                .WithMessage(PropertyNameRequired)
                .NotEqual(EstateAction.None)
                .WithMessage("{PropertyName} cannot equal to {ComparisonValue}");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(PropertyNameRequired);
        }
    }
}
