using RealEstate.Presentation.DTOs.User;

namespace RealEstate.Presentation.DTOs.Review
{
    public record ReviewDto
    {
        public short Rating { get; set; }
        public string Comment { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public Guid RecipientId { get; set; }
        public PartialUserDto? Author { get; set; }
        public PartialUserDto? Recipient { get; set; }
    }
}
