using FluentValidation;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs.User;

namespace RealEstate.Presentation.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .MaximumLength(60)
                .WithMessage(ValidatorConstants.MaximumLength);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .MaximumLength(60)
                .WithMessage(ValidatorConstants.MaximumLength);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .EmailAddress()
                .WithMessage(ValidatorConstants.EmailAddress)
                .MaximumLength(150)
                .WithMessage(ValidatorConstants.MaximumLength);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty);

            RuleFor(x => x.Auth0Id)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty);
        }
    }
}
