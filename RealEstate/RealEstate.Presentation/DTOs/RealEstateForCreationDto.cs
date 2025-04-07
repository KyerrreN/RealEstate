using RealEstate.DAL.Enums;

namespace RealEstate.Presentation.DTOs
{
    public class RealEstateForCreationDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Address { get; set; } = null!;
        public EstateType EstateType { get; set; }
        public Guid OwnerId { get; set; }
    }
}
