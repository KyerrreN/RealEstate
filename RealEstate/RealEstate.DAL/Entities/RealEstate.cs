using RealEstate.DAL.Enums;

namespace RealEstate.DAL.Entities
{
    public class RealEstate
    {
        public Guid Id { get; set; } // PK
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Address { get; set; } = null!;
        public EstateType EstateType { get; set; }
        public EstateStatus EstateStatus { get; set; }
        public Guid OwnerId { get; set; } // FK
    }
}
