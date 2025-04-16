using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Enums;

namespace RealEstate.DAL.Entities
{
    public class HistoryEntity : BaseEntity
    {
        public Guid? RealEstateId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CompletedAt { get; set; }
        public EstateAction EstateAction { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public RealEstateEntity? RealEstate { get; set; } = null!;
        public UserEntity User { get; set; } = null!;
    }
}
