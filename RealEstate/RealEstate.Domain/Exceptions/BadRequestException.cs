namespace RealEstate.Domain.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
            : base("The model is null or invalid") { }

        public BadRequestException(string errorMessage)
            : base(errorMessage) { }
    }
}
