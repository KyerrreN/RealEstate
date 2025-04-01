using RealEstate.DAL.Enums;

namespace RealEstate.DAL.Entities
{
    public class Booking
    {
        public Guid Id { get; set; } // PK
        public Guid RealEstateId { get; set; } // FK
        public string Proposal { get; set; } = null!;
        public EstateAction EstateAction { get; set; }
    }
}
