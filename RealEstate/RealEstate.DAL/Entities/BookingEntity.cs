using RealEstate.DAL.Enums;

namespace RealEstate.DAL.Entities
{
    public class BookingEntity : BaseEntity
    {
        public Guid RealEstateId { get; set; }
        public string Proposal { get; set; } = null!;
        public EstateAction EstateAction { get; set; }
        public Guid UserId { get; set; }

        public UserEntity User { get; set; } = null!;
        public RealEstateEntity RealEstate { get; set; } = null!;
    }
}   
