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
                .WithMessage(ValidatorConstants.NotEmpty);

            RuleFor(x => x.EstateAction)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .NotEqual(EstateAction.None)
                .WithMessage(ValidatorConstants.NotEqual);
        }
    }
}
