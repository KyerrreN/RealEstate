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
                .Matches(@"^\+375(15|16|17|21|22|23|25|29|33|44)\d{7}$")
                .WithMessage(ValidatorConstants.PhoneNumber);

            RuleFor(x => x.Auth0Id)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty);
        }
    }
}
