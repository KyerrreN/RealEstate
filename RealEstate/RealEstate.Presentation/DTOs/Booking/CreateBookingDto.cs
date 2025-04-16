using RealEstate.Domain.Enums;

namespace RealEstate.Presentation.DTOs.Booking
{
    public record CreateBookingDto
    {
        public Guid RealEstateId { get; set; }
        public string Proposal { get; set; } = null!;
        public EstateAction EstateAction { get; set; }
        public Guid UserId { get; set; }
    }
}
