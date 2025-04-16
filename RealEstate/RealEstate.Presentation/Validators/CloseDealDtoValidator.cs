using FluentValidation;
using RealEstate.Domain.Enums;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs.Booking;

namespace RealEstate.Presentation.Validators
{
    public sealed class CloseDealDtoValidator : AbstractValidator<CloseDealDto>
    {
        public CloseDealDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(ValidatorConstants.PropertyNameRequired);

            RuleFor(x => x.EstateAction)
                .NotEmpty()
                .WithMessage(ValidatorConstants.PropertyNameRequired)
                .NotEqual(EstateAction.None)
                .WithMessage("{PropertyName} cannot equal to {ComparisonValue}");
        }
    }
}
