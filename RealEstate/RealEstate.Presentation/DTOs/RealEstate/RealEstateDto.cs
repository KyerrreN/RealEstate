using RealEstate.DAL.Entities;
using RealEstate.Presentation.DTOs.User;

namespace RealEstate.Presentation.DTOs.RealEstate
{
    public record RealEstateDto : BaseDto
    {
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public decimal Price { get; init; }
        public string Address { get; init; } = null!;
        public string EstateType { get; init; } = null!;
        public string EstateStatus { get; init; } = null!;
        public Guid OwnerId { get; init; }
        public PartialUserDto? Owner { get; set; }
    }
}
