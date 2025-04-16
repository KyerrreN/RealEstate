using RealEstate.Domain.Enums;

namespace RealEstate.Presentation.DTOs.Booking
{
    public record CloseDealDto
    {
        public Guid Id { get; set; }
        public EstateAction EstateAction { get; set; }
    }
}
