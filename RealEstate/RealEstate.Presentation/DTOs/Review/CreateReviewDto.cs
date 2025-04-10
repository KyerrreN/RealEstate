using System.ComponentModel.DataAnnotations;

namespace RealEstate.Presentation.DTOs.Review
{
    public record CreateReviewDto
    {
        public short Rating { get; set; }
        public string Comment { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public Guid RecipientId { get; set; }
    }
}
