using FluentValidation;
using RealEstate.Domain.Enums;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs.RealEstate;

namespace RealEstate.Presentation.Validators
{
    public class CreateRealEstateValidator : AbstractValidator<CreateRealEstateDto>
    {
        public CreateRealEstateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .MaximumLength(200)
                .WithMessage(ValidatorConstants.MaximumLength);

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .MaximumLength(2000)
                .WithMessage(ValidatorConstants.MaximumLength);

            RuleFor(x => x.Price)
                .NotNull()
                .WithMessage(ValidatorConstants.NotNull)
                .PrecisionScale(10, 2, false)
                .WithMessage(ValidatorConstants.PrecisionScale)
                .InclusiveBetween(0m, 100_000_000m)
                .WithMessage(ValidatorConstants.InclusiveBetween);

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .MaximumLength(150)
                .WithMessage(ValidatorConstants.MaximumLength);

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .MaximumLength(50)
                .WithMessage(ValidatorConstants.MaximumLength);

            RuleFor(x => x.EstateType)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .NotEqual(EstateType.None)
                .WithMessage(ValidatorConstants.NotEqual);

            RuleFor(x => x.EstateStatus)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .NotEqual(EstateStatus.None)
                .WithMessage(ValidatorConstants.NotEqual);

            RuleFor(x => x.OwnerId)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty);
        }
    }
}
