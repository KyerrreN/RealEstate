using RealEstate.DAL.Enums;

namespace RealEstate.Presentation.DTOs.Booking
{
    public record BookingForCreationDto
    {
        public Guid RealEstateId { get; set; }
        public string Proposal { get; set; } = null!;
        public EstateAction EstateAction { get; set; }
        public Guid UserId { get; set; }
    }
}
