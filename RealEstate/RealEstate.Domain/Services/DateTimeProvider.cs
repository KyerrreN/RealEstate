using RealEstate.Domain.Interfaces;

namespace RealEstate.Domain.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
