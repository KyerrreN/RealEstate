using RealEstate.Presentation.DTOs.RealEstate;

namespace RealEstate.Presentation.DTOs.Booking
{
    public record BookingDto : BaseDto
    {
        public Guid RealEstateId { get; set; }
        public string Proposal { get; set; } = null!;
        public string EstateAction { get; set; } = null!;
        public Guid UserId { get; set; }
        public RealEstateDto? RealEstate { get; set; }
    }
}
