using RealEstate.Domain.Enums;

namespace RealEstate.BLL.Models
{
    public record CloseDealModel
    {
        public Guid Id { get; set; }
        public EstateAction EstateAction { get; set; }
    }
}
