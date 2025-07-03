namespace ChatService.Domain.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("The incoming model is null or empty") { }

        public BadRequestException(string message) : base(message) { }
    }
}
