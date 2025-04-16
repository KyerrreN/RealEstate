using FluentValidation;
using RealEstate.Domain.Enums;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs.Booking;

namespace RealEstate.Presentation.Validators.Booking
{
    public sealed class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
    {
        public CreateBookingDtoValidator()
        {
            RuleFor(x => x.RealEstateId)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty);

            RuleFor(x => x.Proposal)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .MaximumLength(2000)
                .WithMessage(ValidatorConstants.MaximumLength);

            RuleFor(x => x.EstateAction)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .NotEqual(EstateAction.None)
                .WithMessage(ValidatorConstants.NotEqual);

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty);
        }
    }
}
