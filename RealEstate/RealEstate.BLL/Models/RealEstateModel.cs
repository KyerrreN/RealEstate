using RealEstate.DAL.Entities;
using RealEstate.DAL.Enums;

namespace RealEstate.BLL.Models
{
    public class RealEstateModel : BaseModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Address { get; set; } = null!;
        public EstateType EstateType { get; set; }
        public EstateStatus EstateStatus { get; set; }
        public Guid OwnerId { get; set; }
        public UserEntity? Owner { get; set; }
    }
}
