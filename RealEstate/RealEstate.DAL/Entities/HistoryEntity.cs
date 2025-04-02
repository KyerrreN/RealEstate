using RealEstate.DAL.Enums;

namespace RealEstate.DAL.Entities
{
    public class HistoryEntity : BaseEntity
    {
        public Guid RealEstateId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CompletedAt { get; set; }
        public EstateAction EstateAction { get; set; }

        public RealEstateEntity RealEstate { get; set; } = null!;
        public UserEntity User { get; set; } = null!;
    }
}
