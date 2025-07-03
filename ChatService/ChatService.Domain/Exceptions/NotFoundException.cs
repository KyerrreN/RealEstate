namespace ChatService.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Requested resource was not found") { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(Type type) : base($"Requested resource {type} couldn't be found") { }
    }
}
