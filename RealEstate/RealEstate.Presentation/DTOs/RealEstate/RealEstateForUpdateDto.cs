using RealEstate.DAL.Enums;

namespace RealEstate.Presentation.DTOs.RealEstate
{
    public class RealEstateForUpdateDto
    {
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public decimal Price { get; init; }
        public string Address { get; init; } = null!;
        public EstateType EstateType { get; init; }
        public EstateStatus EstateStatus { get; init; }
        public Guid OwnerId { get; set; }
    }
}
