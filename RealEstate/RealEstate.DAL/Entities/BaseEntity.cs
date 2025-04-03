using RealEstate.DAL.Interfaces;

namespace RealEstate.DAL.Entities
{
    public abstract class BaseEntity : IAuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
