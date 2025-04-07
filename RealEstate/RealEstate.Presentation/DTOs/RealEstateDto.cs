namespace RealEstate.Presentation.DTOs
{
    public class RealEstateDto : BaseDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Address { get; set; } = null!;
        public string EstateType { get; set; } = null!;
        public string EstateStatus { get; set; } = null!;
        public Guid OwnerId { get; set; }
    }
}
