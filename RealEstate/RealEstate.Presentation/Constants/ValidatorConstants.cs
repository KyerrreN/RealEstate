namespace RealEstate.Presentation.Constants
{
    public sealed class ValidatorConstants
    {
        public const string NotEmpty = "{PropertyName} is required";
        public const string MaximumLength = "{PropertyName} cannot exceed {MaxLength} characters.";
        public const string NotEqual = "{PropertyName} cannot equal to {ComparisonValue}";
        public const string NotNull = "{PropertyName} cannot be null";
        public const string PrecisionScale = "{PropertyName} cannot have more than {ExpectedPrecision} digits and {ExpectedScale} digits after a decimal point";
        public const string InclusiveBetween = "{PropertyName} must be in range from {From} to {To}";
    }
}
