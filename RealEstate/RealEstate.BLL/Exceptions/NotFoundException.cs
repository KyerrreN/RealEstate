namespace RealEstate.BLL.Exceptions
{
    public class NotFoundException(string resourceName) 
        : Exception($"Requested resource {resourceName} does not exist")
    {
    }
}
