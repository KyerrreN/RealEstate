using FluentValidation;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs.Review;

namespace RealEstate.Presentation.Validators.Review
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.Rating)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty)
                .InclusiveBetween((short)1, (short)5)
                .WithMessage(ValidatorConstants.InclusiveBetween);

            RuleFor(x => x.Comment)
                .NotNull()
                .WithMessage(ValidatorConstants.NotNull)
                .MaximumLength(200)
                .WithMessage(ValidatorConstants.MaximumLength);

            RuleFor(x => x.RecipientId)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty);

            RuleFor(x => x.AuthorId)
                .NotEmpty()
                .WithMessage(ValidatorConstants.NotEmpty);
        }
    }
}
