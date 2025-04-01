using RealEstate.DAL.Enums;

namespace RealEstate.DAL.Entities
{
    public class History
    {
        public Guid RealEstateId { get; set; } // PK, FK
        public Guid UserId { get; set; } // PK, FK
        public DateTime CompletedAt { get; set; }
        public EstateAction EstateAction { get; set; }

        // nav props
        public RealEstate RealEstate { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
