using RealEstate.Domain.Interfaces;

namespace RealEstate.Domain.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
