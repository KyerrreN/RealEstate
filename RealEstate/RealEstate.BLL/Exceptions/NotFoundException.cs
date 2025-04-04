namespace RealEstate.BLL.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string resourceName)
            : base($"Requested resource {resourceName} does not exist") { }

        public NotFoundException(string resourceName, Guid id)
            : base($"Requested resource: {resourceName} with id: {id} does not exist") { }
    }
}
